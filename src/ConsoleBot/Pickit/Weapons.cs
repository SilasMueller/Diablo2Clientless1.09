﻿using D2NG.Core;
using D2NG.Core.D2GS.Enums;
using D2NG.Core.D2GS.Items;
using D2NG.Core.D2GS.Players;
using System;
using System.Collections.Generic;

namespace ConsoleBot.Pickit
{
    public static class Weapons
    {
        private static readonly HashSet<ItemName> desirableExceptionalWeapons = new HashSet<ItemName> { ItemName.MarteldeFer, ItemName.BattleHammer, ItemName.AncientAxe, ItemName.Lance, ItemName.ExecutionerSword, ItemName.Naga };

        private static readonly HashSet<ItemName> interestingExceptionalWeapons = new HashSet<ItemName> { ItemName.Tabar, ItemName.GothicSword, ItemName.BecDeCorbin, ItemName.GrimScythe, ItemName.Zweihander };

        private static readonly HashSet<ItemName> desirableBows = new HashSet<ItemName> { ItemName.DoubleBow, ItemName.RuneBow };

        private static readonly HashSet<ItemName> eliteWeapons = new HashSet<ItemName> { ItemName.PhaseBlade, ItemName.ConquestSword, ItemName.CrypticSword, ItemName.MythicalSword,
            ItemName.ChampionSword, ItemName.ColossusSword, ItemName.ColossusBlade,
            ItemName.GhostSpear, ItemName.WarPike, ItemName.ColossusVoulge, ItemName.Thresher, ItemName.CrypticAxe, ItemName.GreatPoleaxe, ItemName.GiantThresher };


        private static readonly HashSet<ItemName> boWeapons = new HashSet<ItemName> { ItemName.CrystalSword, ItemName.Dagger, ItemName.ShortSpear, ItemName.Glaive, ItemName.ThrowingSpear, ItemName.ThrowingKnife, ItemName.ThrowingAxe, ItemName.ThrowingKnife, ItemName.BalancedKnife };

        public static bool ShouldPickupItemExpansion(Item item)
        {
            if ((item.Quality == QualityType.Magical || item.Quality == QualityType.Rare) && eliteWeapons.Contains(item.Name))
            {
                return true;
            }

            if (boWeapons.Contains(item.Name) && !item.Ethereal && (item.Quality == QualityType.Magical || item.Quality == QualityType.Rare))
            {
                return true;
            }

            if (item.Quality == QualityType.Magical && (item.Classification == ClassificationType.Javelin || item.Classification == ClassificationType.AmazonJavelin))
            {
                return true;
            }

            if ((item.Quality == QualityType.Normal || item.Quality == QualityType.Superior) && item.Ethereal && eliteWeapons.Contains(item.Name))
            {
                return true;
            }

            if ((item.Quality == QualityType.Normal || item.Quality == QualityType.Superior) && item.Sockets >= 5 && eliteWeapons.Contains(item.Name))
            {
                return true;
            }

            if (item.Quality == QualityType.Unique)
            {
                switch (item.Name)
                {
                    //case ItemName.Tulwar:
                    //case ItemName.Dagger:
                    case ItemName.HydraBow:
                    case ItemName.Ballista:
                    case ItemName.ColossusCrossbow:
                    //case ItemName.BoneKnife:
                    case ItemName.LegendaryMallet:
                    case ItemName.ThunderMaul:
                    case ItemName.ColossusBlade:
                        return true;
                    case ItemName.CeremonialJavelin:
                    case ItemName.Yari:
                        return item.Ethereal;
                }
            }

            return false;
        }

        public static bool ShouldPickupItemClassic(Item item)
        {
            if (item.Quality == QualityType.Rare)
            {
                return true;
            }

            if (item.Quality == QualityType.Unique && item.Name == ItemName.Blade)
            {
                return true;
            }

            return false;
        }
        public static bool ShouldKeepItemExpansion(Item item)
        {
            if (item.Quality == QualityType.Unique)
            {
                switch (item.Name)
                {
                    //case ItemName.Tulwar:
                    //case ItemName.Dagger:
                    case ItemName.HydraBow:
                    case ItemName.Ballista:
                    case ItemName.ColossusCrossbow:
                    //case ItemName.BoneKnife:
                    case ItemName.LegendaryMallet:
                    case ItemName.ThunderMaul:
                    case ItemName.ColossusBlade:
                        return true;
                    case ItemName.CeremonialJavelin:
                    case ItemName.Yari:
                        return item.Ethereal;
                }
            }

            /*if((item.Name == ItemName.ColossusBlade || item.Name == ItemName.ColossusSword) && item.Quality == QualityType.Magical)
            {
                return true;
            }
            */
            if (item.Quality == QualityType.Rare
                && item.Ethereal
                && eliteWeapons.Contains(item.Name)
                && item.GetValueOfStatType(StatType.EnhancedMaximumDamage) >= 150
                && item.GetValueOfStatType(StatType.RepairsDurability) > 0)
            {
                return true;
            }

            if (item.Quality == QualityType.Magical && item.Ethereal && eliteWeapons.Contains(item.Name) && item.GetValueOfStatType(StatType.EnhancedMaximumDamage) >= 200)
            {
                return true;
            }

            if ((item.Quality == QualityType.Normal || item.Quality == QualityType.Superior) && item.Ethereal && eliteWeapons.Contains(item.Name))
            {
                return true;
            }

            if (item.Ethereal && eliteWeapons.Contains(item.Name) && item.GetValueOfStatType(StatType.EnhancedMaximumDamage) >= 150)
            {
                return true;
            }

            if (item.Quality == QualityType.Magical && eliteWeapons.Contains(item.Name) && item.GetValueOfStatType(StatType.EnhancedMaximumDamage) >= 200)
            {
                return true;
            }

            if (item.Quality == QualityType.Magical && item.GetValueToSkillTab(SkillTab.AmazonJavelinAndSpearSkills) >= 5 && item.GetValueOfStatType(StatType.IncreasedAttackSpeed) >= 40)
            {
                return true;
            }

            if (item.GetValueOfStatType(StatType.BarbarianSkills) + item.GetValueToSkillTab(SkillTab.BarbarianWarcries) >= 3)
            {
                return true;
            }

            return false;
        }

        public static bool ShouldKeepItemClassic(Item item)
        {
            var additionalDamage = item.GetValueOfStatType(StatType.EnhancedMaximumDamage);
            additionalDamage += (int)1.5 * Math.Max(item.GetValueOfStatType(StatType.MinimumDamage), item.GetValueOfStatType(StatType.SecondaryMinimumDamage));
            additionalDamage += (int)1.5 * Math.Max(item.GetValueOfStatType(StatType.MaximumDamage), item.GetValueOfStatType(StatType.SecondaryMaximumDamage));

            if (item.Classification == ClassificationType.Bow)
            {
                additionalDamage += item.GetValueOfStatType(StatType.Dexterity);
                additionalDamage += (item.GetValueOfStatType(StatType.AmazonSkills) * 5);
            }
            else
            {
                additionalDamage += (item.GetValueOfStatType(StatType.BarbarianSkills) * 5);
            }

            if (item.Quality == QualityType.Rare
            && desirableExceptionalWeapons.Contains(item.Name)
            && additionalDamage > 180 && item.GetValueOfStatType(StatType.EnhancedMaximumDamage) >= 170)
            {
                return true;
            }

            var desirablePvmWeapons = new HashSet<ItemName> { ItemName.MarteldeFer, ItemName.AncientAxe, ItemName.Lance, ItemName.ExecutionerSword };
            if (item.Quality == QualityType.Rare
            && desirablePvmWeapons.Contains(item.Name)
            && additionalDamage > 140 && item.GetValueOfStatType(StatType.ReducedRequirements) <= -20 && item.GetValueOfStatType(StatType.MinimumLifeStolenPerHit) >= 6)
            {
                return true;
            }

            if (item.Quality == QualityType.Rare
            && interestingExceptionalWeapons.Contains(item.Name)
            && (additionalDamage > 210 && item.GetValueOfStatType(StatType.EnhancedMaximumDamage) >= 190))
            {
                return true;
            }

            if (item.Quality == QualityType.Rare
            && item.Name == ItemName.GothicBow
            && item.GetValueOfStatType(StatType.IncreasedAttackSpeed) >= 10
            && item.GetValueOfStatType(StatType.EnhancedMaximumDamage) >= 150
            && additionalDamage > 170)
            {
                return true;
            }

            if (item.Quality == QualityType.Rare
            && desirableBows.Contains(item.Name)
            && item.GetValueOfStatType(StatType.IncreasedAttackSpeed) >= 20
            && item.GetValueOfStatType(StatType.EnhancedMaximumDamage) >= 150
            && additionalDamage > 170)
            {
                return true;
            }

            if (item.Quality == QualityType.Rare
            && (item.Name == ItemName.LargeSiegeBow || item.Name == ItemName.RazorBow)
            && item.GetValueOfStatType(StatType.IncreasedAttackSpeed) >= 20
            && item.GetValueOfStatType(StatType.EnhancedMaximumDamage) >= 170
            && additionalDamage > 180)
            {
                return true;
            }

            if (item.Quality == QualityType.Rare
            && item.Name == ItemName.Flail
            && item.GetValueOfStatType(StatType.PaladinSkills) >= 2
            && item.GetValueOfStatType(StatType.IncreasedAttackSpeed) >= 40
            && (item.GetTotalResistFrLrCr() > 10 || item.GetTotalLifeFromStats(CharacterClass.Paladin) > 10))
            {
                return true;
            }
            /*
            if (item.Quality == QualityType.Unique && item.Name == ItemName.Blade && item.GetValueOfStatType(StatType.FasterCastRate) == 50)
            {
                return true;
            }
            */
            return false;
        }
    }
}
