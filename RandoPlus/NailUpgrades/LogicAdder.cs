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
        private const string PALEORE = "PALEORE";
        private const string LISTEN = "(LISTEN ? TRUE)";

        public static void Hook()
        {
            ProgressionInitializer.OnCreateProgressionInitializer += AddOreTolerance;
            RCData.RuntimeLogicOverride.Subscribe(Consts.LOGICPRIORITY, DefineTermsAndItems);
            RCData.RuntimeLogicOverride.Subscribe(Consts.LOGICPRIORITY, ApplyLogic);
        }

        private static void AddOreTolerance(LogicManager lm, GenerationSettings gs, ProgressionInitializer pi)
        {

            if (!RandoPlus.GS.NailUpgrades || !RandoPlus.GS.TwoDupePaleOre) return;

            if (lm.GetTerm(PALEORE) is not Term oreTerm)
            {
                RandoPlus.instance.LogWarn($"Cannot add ore tolerance because term {PALEORE} not found");
                return;
            }

            pi.Setters.Add(new(oreTerm, -1));
        }

        private static void ApplyLogic(GenerationSettings gs, LogicManagerBuilder lmb)
        {
            if (!RandoPlus.GS.NailUpgrades || !RandoPlus.GS.DefineRefs) return;

            lmb.AddLogicDef(new RawLogicDef(Consts.NailsmithLocationPrefix + "1", $"{SceneNames.Room_nailsmith}[left1] + {LISTEN}"));
            lmb.AddLogicDef(new RawLogicDef(Consts.NailsmithLocationPrefix + "2", $"{SceneNames.Room_nailsmith}[left1] + {LISTEN} + {PALEORE} > 0"));
            lmb.AddLogicDef(new RawLogicDef(Consts.NailsmithLocationPrefix + "3", $"{SceneNames.Room_nailsmith}[left1] + {LISTEN} + {PALEORE} > 2"));
            lmb.AddLogicDef(new RawLogicDef(Consts.NailsmithLocationPrefix + "4", $"{SceneNames.Room_nailsmith}[left1] + {LISTEN} + {PALEORE} > 5"));
        }

        private static void DefineTermsAndItems(GenerationSettings gs, LogicManagerBuilder lmb)
        {
            if (!RandoPlus.GS.NailUpgrades || !RandoPlus.GS.DefineRefs) return;

            Term nailupTerm = lmb.GetOrAddTerm("NAILUPGRADE");
            lmb.AddItem(new SingleItem(Consts.NailUpgrade, new TermValue(nailupTerm, 1)));
        }
    }
}
