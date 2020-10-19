﻿using D2NG.Core.D2GS.Items;
using System.Collections.Generic;

namespace ConsoleBot.Pickit
{
    public static class Sets
    {
        public static bool ShouldPickupItem(Item item)
        {
            if (item.Quality != QualityType.Set)
            {
                return false;
            }

            // Death's
            /*
            if (item.Name == "Sash" || item.Name == "Leather Gloves")
            {
                return true;
            }
            */
            /*
            if (item.Name == "Crown" || item.Name == "Amulet" || item.Name == "Light Gauntlets" || item.Name == "Heavy Belt")
            {
                return true;
            }
            */
            return false;
        }

        public static bool ShouldKeepItem(Item item)
        {
            if (item.Quality != QualityType.Set)
            {
                return false;
            }

            // Death's
            /*
            if (item.Name == "Sash" || item.Name == "Leather Gloves")
            {
                return true;
            }
            */

            // Iratha's set:
            /*
            if (item.Name == "Crown" && item.GetValueOfStatType(StatType.FireResistance) == 30)
            {
                return true;
            }

            if (item.Name == "Light Gauntlets" && item.GetValueOfStatType(StatType.ColdResistance) == 30)
            {
                return true;
            }

            if (item.Name == "Amulet" && item.GetValueOfStatType(StatType.PoisonResistance) == 30)
            {
                return true;
            }

            if (item.Name == "Heavy Belt" && item.GetValueOfStatType(StatType.MinimumDamage) == 5)
            {
                return true;
            }
            */

            return false;
        }
    }
}
