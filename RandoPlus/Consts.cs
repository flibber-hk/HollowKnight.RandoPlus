﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemChanger;

namespace RandoPlus
{
    public static class Consts
    {
        public const string NoLantern = $"Not_{ItemNames.Lumafly_Lantern}";
        public const string NoTear = $"Not_{ItemNames.Ismas_Tear}";
        public const string NoSwim = $"Not_{ItemNames.Swim}";
        public const string RemoveUsefulItems = "RemoveUsefulItems";

        public const string MrMushroomLevelUp = "Mr_Mushroom_Level_Up";

        public const string MrMushroomFungalWastes = "Mr_Mushroom-Fungal_Wastes";
        public const string MrMushroomKingdomsEdge = "Mr_Mushroom-Kingdom's_Edge";
        public const string MrMushroomDeepnest = "Mr_Mushroom-Deepnest";
        public const string MrMushroomHowlingCliffs = "Mr_Mushroom-Howling_Cliffs";
        public const string MrMushroomAncientBasin = "Mr_Mushroom-Ancient_Basin";
        public const string MrMushroomFogCanyon = "Mr_Mushroom-Fog_Canyon";
        public const string MrMushroomKingsPass = "Mr_Mushroom-King's_Pass";

        public const string SkillsPoolGroup = "Skills";
        public const string KeysPoolGroup = "Keys";
        public const string MrMushroomPoolGroup = "Mr Mushroom";

        public const string NailUpgrade = "Nail_Upgrade";
        public const string NailsmithLocationPrefix = "Nailsmith_Upgrade_";
        public const string NailUpgradePoolGroup = "Skills";

        public const string GhostPoolGroup = "Ghost Essence";
        public const string GhostEssenceItemName = "Ghost_Essence";

        public static readonly int[] VanillaNailUpgradeCosts = new[] { 0, 250, 800, 2000, 4000 };

        public const float LOGICPRIORITY = 50f;
    }
}
