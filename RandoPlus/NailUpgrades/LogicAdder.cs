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
using RandoPlus;

namespace RandoPlus.NailUpgrades
{
    class LogicAdder
    {
        public static void Hook()
        {
            RCData.RuntimeLogicOverride.Subscribe(Consts.LOGICPRIORITY, DefineTermsAndItems);
            RCData.RuntimeLogicOverride.Subscribe(Consts.LOGICPRIORITY, ApplyLogic);
        }

        private static void ApplyLogic(GenerationSettings gs, LogicManagerBuilder lmb)
        {
            if (!RandoPlus.GS.NailUpgrades) return;
            for (int counter = 1; counter < 5; counter++)
            {
                lmb.AddLogicDef(new RawLogicDef(Consts.NailPlace+counter.ToString(), "Town"));
            }
            //lmb.AddLogicDef(new RawLogicDef(Consts.NailUpgradeL2, $"Town + PALEORE > 2"));


        }

        private static void DefineTermsAndItems(GenerationSettings gs, LogicManagerBuilder lmb)
        {
            if (!RandoPlus.GS.NailUpgrades) return;
            Term t = lmb.GetTerm("DREAMNAIL");
            lmb.AddItem(new SingleItem(Consts.Nail_Upgrade, new TermValue(t,1)));

        }
    }
}
