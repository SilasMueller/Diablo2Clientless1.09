﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ConsoleBot.Clients.ExternalMessagingClient;
using ConsoleBot.Enums;
using D2NG.Core;
using D2NG.Core.D2GS;
using D2NG.Core.D2GS.Items;
using D2NG.Core.D2GS.Objects;
using Serilog;
using Attribute = D2NG.Core.D2GS.Players.Attribute;

namespace ConsoleBot.Helpers
{
    public static class InventoryHelpers
    {
        private static readonly TimeSpan MoveItemTimeout = TimeSpan.FromSeconds(2);
        public static void CleanupCursorItem(this Game game)
        {
            if (game.CursorItem != null)
            {
                var item = game.CursorItem;
                var freeSpaceCube = game.Cube.FindFreeSpace(item);
                var freeSpaceInventory = game.Inventory.FindFreeSpace(item);
                if (freeSpaceInventory != null)
                {
                    game.InsertItemIntoContainer(game.CursorItem, freeSpaceInventory, ItemContainer.Inventory);
                    bool resultMove = GeneralHelpers.TryWithTimeout((retryCount) => game.CursorItem == null && game.Inventory.FindItemById(item.Id) != null, MoveItemTimeout);
                    if (!resultMove)
                    {
                        Log.Error($"{game.Me.Name}: Moving item {item.Id} - {item.Name} from cursor to inventory failed");
                    }
                }
                else if (freeSpaceCube != null)
                {
                    game.InsertItemIntoContainer(item, freeSpaceCube, ItemContainer.Cube);
                    bool resultMove = GeneralHelpers.TryWithTimeout((retryCount) => game.CursorItem == null && game.Cube.FindItemById(item.Id) != null, MoveItemTimeout);
                    if (!resultMove)
                    {
                        Log.Error($"{game.Me.Name}: Moving item {item.Id} - {item.Name} from cursor to cube failed");
                    }
                }
                else
                {
                    Log.Error($"{game.Me.Name}: Moving item {item.Id} - {item.Name} from cursor failed, no space");
                }
            }
        }

        public static MoveItemResult StashItemsAndGold(Game game, List<Item> items, int gold)
        {
            var stashes = game.GetEntityByCode(EntityCode.Stash);
            if (!stashes.Any())
            {
                Log.Error($"{game.Me.Name}: No stash found");
                return MoveItemResult.Failed;
            }

            var stash = stashes.Single();

            bool result = GeneralHelpers.TryWithTimeout((retryCount) =>
            {
                if (game.Me.Location.Distance(stash.Location) >= 5)
                {
                    game.MoveTo(stash);
                }
                else
                {
                    return game.OpenStash(stash);
                }

                return false;
            }, TimeSpan.FromSeconds(4));

            if (!result)
            {
                Log.Error($"{game.Me.Name}: Failed to open stash while at location {game.Me.Location} with stash at {stash.Location}");
                return MoveItemResult.Failed;
            }

            if (gold > 0)
            {
                game.MoveGoldToStash(gold);
            }

            var moveResult = MoveItemResult.Succes;

            Thread.Sleep(100);
            foreach (Item item in items)
            {
                if (game.Stash.FindFreeSpace(item) == null)
                {
                    moveResult = MoveItemResult.NoSpace;
                    continue;
                }

                var currentMoveResult = MoveItemToStash(game, item);
                if (currentMoveResult != MoveItemResult.Succes)
                {
                    moveResult = currentMoveResult;
                    break;
                };
            }

            game.ClickButton(ClickType.CloseStash);
            Thread.Sleep(100);
            game.ClickButton(ClickType.CloseStash);
            return moveResult;
        }

        public static MoveItemResult MoveStashItemsToInventory(Game game, List<Item> items)
        {
            var stashes = game.GetEntityByCode(EntityCode.Stash);
            if (!stashes.Any())
            {
                Log.Error($"{game.Me.Name}: No stash found");
                return MoveItemResult.Failed;
            }

            var stash = stashes.Single();

            bool result = GeneralHelpers.TryWithTimeout((retryCount) =>
            {
                if (game.Me.Location.Distance(stash.Location) >= 5)
                {
                    game.MoveTo(stash);
                }
                else
                {
                    return game.OpenStash(stash);
                }

                return false;
            }, TimeSpan.FromSeconds(4));

            if (!result)
            {
                Log.Error($"{game.Me.Name}: Failed to open stash while at location {game.Me.Location} with stash at {stash.Location}");
                return MoveItemResult.Failed;
            }

            var moveItemResult = MoveItemResult.Succes;

            Thread.Sleep(100);
            foreach (Item item in items)
            {
                var currentMoveResult = MoveItemFromStashToInventory(game, item);
                if(currentMoveResult == MoveItemResult.Failed)
                {
                    break;
                }
                else if (currentMoveResult == MoveItemResult.NoSpace)
                {
                    moveItemResult = currentMoveResult;
                    continue;
                };
            }

            game.ClickButton(ClickType.CloseStash);
            Thread.Sleep(100);
            game.ClickButton(ClickType.CloseStash);
            return moveItemResult;
        }

        public static bool HasAnyItemsToStash(Game game)
        {
            return game.Inventory.Items.Any(i => i.IsIdentified && Pickit.Pickit.ShouldKeepItem(game, i) && Pickit.Pickit.CanTouchInventoryItem(game, i))
                || game.Cube.Items.Any(i => i.IsIdentified && Pickit.Pickit.ShouldKeepItem(game, i))
                || game.Me.Attributes.GetValueOrDefault(Attribute.GoldOnPerson, 0) > 500000;
        }

        public static MoveItemResult StashItemsToKeep(Game game, IExternalMessagingClient externalMessagingClient)
        {
            if (!HasAnyItemsToStash(game))
            {
                return MoveItemResult.Succes;
            }

            var itemsToKeep = game.Inventory.Items.Where(i => i.IsIdentified && Pickit.Pickit.ShouldKeepItem(game, i) && Pickit.Pickit.CanTouchInventoryItem(game, i)).ToList();
            itemsToKeep.AddRange(game.Cube.Items.Where(i => i.IsIdentified && Pickit.Pickit.ShouldKeepItem(game, i)));
            var goldOnPerson = game.Me.Attributes.GetValueOrDefault(Attribute.GoldOnPerson, 0);
            foreach (var item in itemsToKeep)
            {
                Log.Information($"{game.Me.Name}: Want to keep {item.GetFullDescription()}");
                if (!(item.Name == ItemName.Ring && item.Quality == QualityType.Unique) && item.Classification != ClassificationType.Gem)
                {
                    externalMessagingClient.SendMessage($"{game.Me.Name}: Want to keep {item.GetFullDescription()}");
                }
            }

            return StashItemsAndGold(game, itemsToKeep, goldOnPerson);
        }

        public static bool TransmuteItemsInCube(Game game, bool newItemsSpawn)
        {
            var cube = game.Inventory.FindItemByName(ItemName.HoradricCube);
            if (cube != null)
            {
                if (!game.ActivateCube(cube))
                {
                    return false;
                }
                var oldItems = game.Cube.Items.Select(i => i.Id).ToHashSet();
                game.ClickButton(ClickType.TransmuteItems);

                var transmuteResult = GeneralHelpers.TryWithTimeout((retryCount) =>
                {
                    var newItems = game.Cube.Items.Select(i => i.Id).ToHashSet();
                    return !newItems.Intersect(oldItems).Any() && (!newItemsSpawn || newItems.Count > 0);
                }, MoveItemTimeout);

                if (!transmuteResult)
                {
                    Log.Error($"{game.Me.Name}: Transmuting items failed");
                    game.ClickButton(ClickType.CloseHoradricCube);
                    return false;
                }

                game.ClickButton(ClickType.CloseHoradricCube);
                return true;
            }

            return false;
        }

        public static MoveItemResult MoveItemFromStashToInventory(Game game, Item item)
        {
            Point location = game.Inventory.FindFreeSpace(item);
            if (location == null)
            {
                return MoveItemResult.NoSpace;
            }

            game.RemoveItemFromContainer(item);
            bool resultToBuffer = GeneralHelpers.TryWithTimeout((retryCount) => game.CursorItem?.Id == item.Id, MoveItemTimeout);

            if (!resultToBuffer)
            {
                Log.Error($"{game.Me.Name}: Moving item {item.Id} - {item.Name} to buffer failed");
                return MoveItemResult.Failed;
            }
            game.InsertItemIntoContainer(item, location, ItemContainer.Inventory);

            return GeneralHelpers.TryWithTimeout(
                (retryCount) => game.CursorItem == null && game.Inventory.FindItemById(item.Id) != null,
                MoveItemTimeout) ? MoveItemResult.Succes : MoveItemResult.Failed;

        }

        public static MoveItemResult MoveItemToStash(Game game, Item item)
        {
            Point location = game.Stash.FindFreeSpace(item);
            if (location == null)
            {
                return MoveItemResult.NoSpace;
            }

            game.RemoveItemFromContainer(item);
            bool resultToBuffer = GeneralHelpers.TryWithTimeout((retryCount) => game.CursorItem?.Id == item.Id,
                MoveItemTimeout);
            if (!resultToBuffer)
            {
                Log.Error($"{game.Me.Name}: Moving item {item.Id} - {item.Name} to buffer failed");
                return MoveItemResult.Failed;
            }

            game.InsertItemIntoContainer(item, location, ItemContainer.Stash);

            if (!GeneralHelpers.TryWithTimeout(
                (retryCount) => game.CursorItem == null && game.Stash.FindItemById(item.Id) != null,
               MoveItemTimeout))
            {
                Log.Error($"{game.Me.Name}: Inserting item {item.Id} - {item.Name} into stash failed");
                return MoveItemResult.Failed;
            }

            return MoveItemResult.Succes;
        }

        public static MoveItemResult DropItemFromCube(Game game, Item item)
        {
            var cube = game.Inventory.FindItemByName(ItemName.HoradricCube);
            if (cube == null)
            {
                Log.Error($"{game.Me.Name}: Cube not found");
                return MoveItemResult.Failed;
            }

            if (!game.ActivateCube(cube))
            {
                Log.Error($"{game.Me.Name}: Opening cube for {item.Id} - {item.GetFullDescription()} failed with cursor {game.CursorItem?.Id}");
                return MoveItemResult.Failed;
            }
            game.RemoveItemFromContainer(item);

            bool resultToBuffer = GeneralHelpers.TryWithTimeout((retryCount) => game.CursorItem?.Id == item.Id, MoveItemTimeout);
            if (!resultToBuffer)
            {
                Log.Error($"{game.Me.Name}: Moving item {item.Id} - {item.Name} to buffer failed");
                game.ClickButton(ClickType.CloseHoradricCube);
                return MoveItemResult.Failed;
            }

            game.DropItem(item);

            bool resultMove = GeneralHelpers.TryWithTimeout((retryCount) => game.CursorItem == null, MoveItemTimeout);
            if (!resultMove)
            {
                Log.Error($"{game.Me.Name}: Dropping item {item.Id} - {item.Name} failed");
                game.ClickButton(ClickType.CloseHoradricCube);
                return MoveItemResult.Failed;
            }

            game.ClickButton(ClickType.CloseHoradricCube);
            return MoveItemResult.Succes;
        }

        public static MoveItemResult PutCubeItemInInventory(Game game, Item item)
        {
            Point location = game.Inventory.FindFreeSpace(item);
            if (location == null)
            {
                return MoveItemResult.NoSpace;
            }

            var cube = game.Inventory.FindItemByName(ItemName.HoradricCube);
            if (cube == null)
            {
                Log.Error($"{game.Me.Name}: Cube not found");
                return MoveItemResult.Failed;
            }

            if (!game.ActivateCube(cube))
            {
                Log.Error($"{game.Me.Name}: Opening cube for {item.Id} - {item.GetFullDescription()} failed with cursor {game.CursorItem?.Id}");
                return MoveItemResult.Failed;
            }

            game.RemoveItemFromContainer(item);

            bool resultToBuffer = GeneralHelpers.TryWithTimeout((retryCount) => game.CursorItem?.Id == item.Id, MoveItemTimeout);
            if (!resultToBuffer)
            {
                Log.Error($"{game.Me.Name}: Moving item {item.Id} - {item.Name} to buffer failed");
                game.ClickButton(ClickType.CloseHoradricCube);
                return MoveItemResult.Failed;
            }

            game.InsertItemIntoContainer(item, location, ItemContainer.Inventory);

            bool resultMove = GeneralHelpers.TryWithTimeout((retryCount) => game.CursorItem == null && game.Inventory.FindItemById(item.Id) != null, MoveItemTimeout);
            if (!resultMove)
            {
                Log.Error($"{game.Me.Name}: Moving item {item.Id} - {item.Name} to cube failed");
                game.ClickButton(ClickType.CloseHoradricCube);
                return MoveItemResult.Failed;
            }

            game.ClickButton(ClickType.CloseHoradricCube);
            return MoveItemResult.Succes;
        }

        public static MoveItemResult PutInventoryItemInCube(Game game, Item item, Point point)
        {
            var cube = game.Inventory.FindItemByName(ItemName.HoradricCube);
            if (cube == null)
            {
                Log.Error($"{game.Me.Name}: Cube not found");
                return MoveItemResult.Failed;
            }

            if (!game.ActivateCube(cube))
            {
                Log.Error($"{game.Me.Name}: Opening cube for {item.Id} - {item.GetFullDescription()} failed with cursor {game.CursorItem?.Id}");
                return MoveItemResult.Failed;
            }

            game.RemoveItemFromContainer(item);

            bool resultToBuffer = GeneralHelpers.TryWithTimeout((retryCount) => game.CursorItem?.Id == item.Id, MoveItemTimeout);
            if (!resultToBuffer)
            {
                Log.Error($"{game.Me.Name}: Moving item {item.Id} - {item.Name} to buffer failed");
                game.ClickButton(ClickType.CloseHoradricCube);
                return MoveItemResult.Failed;
            }

            game.InsertItemIntoContainer(item, point, ItemContainer.Cube);

            bool resultMove = GeneralHelpers.TryWithTimeout((retryCount) => game.CursorItem == null && game.Cube.FindItemById(item.Id) != null, MoveItemTimeout);
            if (!resultMove)
            {
                Log.Error($"{game.Me.Name}: Moving item {item.Id} - {item.Name} to cube failed");
                game.ClickButton(ClickType.CloseHoradricCube);
                return MoveItemResult.Failed;
            }

            game.ClickButton(ClickType.CloseHoradricCube);
            return MoveItemResult.Succes;
        }

        public static void MoveInventoryItemsToCube(Game game)
        {
            foreach (var item in game.Inventory.Items)
            {
                if (Pickit.Pickit.CanTouchInventoryItem(game, item))
                {
                    var freeSpace = game.Cube.FindFreeSpace(item);
                    if (freeSpace != null)
                    {
                        PutInventoryItemInCube(game, item, freeSpace);
                    }
                }
            }
        }

        public static bool MoveCubeItemsToInventory(Game game)
        {
            foreach (var item in game.Cube.Items)
            {
                if(PutCubeItemInInventory(game, item) != MoveItemResult.Succes)
                {
                    return false;
                }
            }

            return true;
        }

        public static MoveItemResult MoveBeltItemToInventory(Game game, Item item)
        {
            Point location = game.Inventory.FindFreeSpace(item);
            if (location == null)
            {
                return MoveItemResult.NoSpace;
            }

            game.RemoveItemFromBelt(item);
            bool resultToBuffer = GeneralHelpers.TryWithTimeout((retryCount) => game.Belt.FindItemById(item.Id) == null,
                MoveItemTimeout);
            if (!resultToBuffer)
            {
                Log.Error($"{game.Me.Name}: Moving item {item.Id} - {item.Name} to buffer failed");
                return MoveItemResult.Failed;
            }

            game.InsertItemIntoContainer(item, location, ItemContainer.Inventory);

            if (!GeneralHelpers.TryWithTimeout(
                (retryCount) => game.Inventory.FindItemById(item.Id) != null,
               MoveItemTimeout))
            {
                Log.Error($"{game.Me.Name}: Inserting item {item.Id} - {item.Name} into Inventory failed");
                return MoveItemResult.Failed;
            }

            return MoveItemResult.Succes;
        }

        public static void CleanupPotionsInBelt(Game game)
        {
            var manaPotionsInWrongSlot = game.Belt.GetManaPotionsInSlots(new List<int>() { 0, 1 });
            foreach (var manaPotion in manaPotionsInWrongSlot)
            {
                game.UseBeltItem(manaPotion);
            }

            var healthPotionsInWrongSlot = game.Belt.GetHealthPotionsInSlots(new List<int>() { 2, 3 });
            foreach (var healthPotion in healthPotionsInWrongSlot)
            {
                game.UseBeltItem(healthPotion);
            }

            var revPotions = game.Belt.GetRejuvenationPotions();
            foreach(var revPotion in revPotions)
            {
                MoveBeltItemToInventory(game, revPotion);
            }
        }
    }
}
