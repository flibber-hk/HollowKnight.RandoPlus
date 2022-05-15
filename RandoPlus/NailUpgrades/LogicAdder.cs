using ItemChanger;
using RandomizerCore;
using RandomizerCore.Logic;
using RandomizerCore.LogicItems;
using RandomizerMod.RC;
using RandomizerMod.Settings;

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

            lmb.AddLogicDef(new RawLogicDef(Consts.NailsmithLocationPrefix + "1", $"{SceneNames.Room_nailsmith}[left1]"));
            lmb.AddLogicDef(new RawLogicDef(Consts.NailsmithLocationPrefix + "2", $"{SceneNames.Room_nailsmith}[left1] + PALEORE>0"));
            lmb.AddLogicDef(new RawLogicDef(Consts.NailsmithLocationPrefix + "3", $"{SceneNames.Room_nailsmith}[left1] + PALEORE>2"));
            lmb.AddLogicDef(new RawLogicDef(Consts.NailsmithLocationPrefix + "4", $"{SceneNames.Room_nailsmith}[left1] + PALEORE>5"));
        }

        private static void DefineTermsAndItems(GenerationSettings gs, LogicManagerBuilder lmb)
        {
            if (!RandoPlus.GS.Any) return;

            Term nailupTerm = lmb.GetOrAddTerm("NAILUPGRADE");
            lmb.AddItem(new SingleItem(Consts.NailUpgrade, new TermValue(nailupTerm, 1)));
        }
    }
}
