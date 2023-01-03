using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ItemChanger;
using RandomizerCore;
using RandomizerCore.Logic;
using RandomizerCore.LogicItems;
using RandomizerCore.StringLogic;
using RandomizerMod.RC;
using RandomizerMod.Settings;

namespace RandoPlus.GhostEssence
{
    public static class LogicAdder
    {
        public static void Hook()
        {
            RCData.RuntimeLogicOverride.Subscribe(Consts.LOGICPRIORITY, DefineTermsAndItems);
            RCData.RuntimeLogicOverride.Subscribe(Consts.LOGICPRIORITY, ApplyLogic);
        }

        private static void ApplyLogic(GenerationSettings gs, LogicManagerBuilder lmb)
        {
            if (!RandoPlus.GS.Any) return;

            using Stream wpStream = typeof(LogicAdder).Assembly.GetManifestResourceStream("RandoPlus.Resources.GhostEssence.waypoints.json");
            lmb.DeserializeJson(LogicManagerBuilder.JsonType.Waypoints, wpStream);

            using Stream locStream = typeof(LogicAdder).Assembly.GetManifestResourceStream("RandoPlus.Resources.GhostEssence.locations.json");
            lmb.DeserializeJson(LogicManagerBuilder.JsonType.Locations, locStream);
        }

        private static void DefineTermsAndItems(GenerationSettings gs, LogicManagerBuilder lmb)
        {
            if (!RandoPlus.GS.Any) return;

            Term essence = lmb.GetTerm("ESSENCE");
            lmb.AddItem(new SingleItem(Consts.GhostEssenceItemName, new TermValue(essence, 1)));
        }
    }
}
