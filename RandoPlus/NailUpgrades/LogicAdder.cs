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

            lmb.AddLogicDef(new RawLogicDef(Consts.NailPlace + "1", SceneNames.Room_nailsmith + "[left1]"));
            lmb.AddLogicDef(new RawLogicDef(Consts.NailPlace + "2", $"{SceneNames.Room_nailsmith}[left1] + PALEORE>0+ ARCANEEGGS>0"));
            lmb.AddLogicDef(new RawLogicDef(Consts.NailPlace + "3", $"{SceneNames.Room_nailsmith}[left1] + PALEORE>2+ ARCANEEGGS>1"));
            lmb.AddLogicDef(new RawLogicDef(Consts.NailPlace + "4", $"{SceneNames.Room_nailsmith}[left1] + PALEORE>5+ ARCANEEGGS>3"));


        }

        private static void DefineTermsAndItems(GenerationSettings gs, LogicManagerBuilder lmb)
        {
            if (!RandoPlus.GS.NailUpgrades) return;
            Term nailupTerm = lmb.GetOrAddTerm("NAILUPGRADE");
            lmb.AddItem(new SingleItem(Consts.Nail_Upgrade, new TermValue(nailupTerm, 1)));//, new TermValue(t,1)));
                                                                                           //lmb.AddItem(new SingleItem(Consts.MrMushroomLevelUp, new TermValue(mushroomTerm, 1)));
                                                                                           // lmb.AddItem(new EmptyItem(Consts.Nail_Upgrade));
        }
    }
}
