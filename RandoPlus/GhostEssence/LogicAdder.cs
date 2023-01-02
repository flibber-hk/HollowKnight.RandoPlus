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

            using Stream s = typeof(LogicAdder).Assembly.GetManifestResourceStream("RandoPlus.Resources.GhostEssence.logic.json");
            lmb.DeserializeJson(LogicManagerBuilder.JsonType.Locations, s);
        }

        private static void DefineTermsAndItems(GenerationSettings gs, LogicManagerBuilder lmb)
        {
            if (!RandoPlus.GS.Any) return;

            Term essence = lmb.GetTerm("ESSENCE");
            foreach (string ghost in GhostNames.ToArray())
            {
                lmb.AddItem(new SingleItem(ghost, new TermValue(essence, 1)));
            }
        }
    }
}
