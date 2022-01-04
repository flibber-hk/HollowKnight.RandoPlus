using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemChanger;
using RandomizerCore;
using RandomizerCore.Logic;
using RandomizerCore.LogicItems;
using RandomizerCore.StringLogic;
using RandomizerMod.RC;
using RandomizerMod.Settings;

namespace RandoPlus.RemoveUsefulItems
{
    public static class LogicPatcher
    {
        public static void Hook()
        {
            RCData.RuntimeLogicOverride.Subscribe(Consts.LOGICPRIORITY, DefineTermsAndItems);
            RCData.RuntimeLogicOverride.Subscribe(Consts.LOGICPRIORITY, InternalModifyLogic);
        }

        private static void InternalModifyLogic(GenerationSettings gs, LogicManagerBuilder lmb)
        {
            if (!RandoPlus.GS.Any) return;

            void ProvideLogicSubstitution(string name, string pre, string post)
            {
                LogicClauseBuilder lcb = new(lmb.LogicLookup[name]);
                lcb.Subst(lmb.LP.GetTermToken(pre), new LogicClause(post, lmb.LP));
                lmb.LogicLookup[name] = new(lcb);
            }
            void ProvideMacroSubstitution(string name, string pre, string post)
            {
                LogicClauseBuilder lcb = new(lmb.LP.GetMacro(name));
                lcb.Subst(lmb.LP.GetTermToken(pre), new LogicClause(post, lmb.LP));
                lmb.LP.SetMacro(name, new LogicClause(lcb));
            }

            // Lantern
            ProvideLogicSubstitution("Mines_33[right1]", "LANTERN", "LANTERN | NOLANTERN + DARKROOMS");
            ProvideLogicSubstitution("Mines_33[left1]", "LANTERN", "LANTERN | NOLANTERN + DARKROOMS");
            ProvideLogicSubstitution(LocationNames.Boss_Essence_No_Eyes, "LANTERN", "LANTERN | NOLANTERN + DARKROOMS");
            ProvideLogicSubstitution(LocationNames.Geo_Rock_Crystal_Peak_Entrance, "LANTERN", "LANTERN | NOLANTERN + DARKROOMS");
            ProvideLogicSubstitution(LocationNames.Geo_Rock_Crystal_Peak_Entrance_Dupe_1, "LANTERN", "LANTERN | NOLANTERN + DARKROOMS");
            ProvideLogicSubstitution(LocationNames.Geo_Rock_Crystal_Peak_Entrance_Dupe_2, "LANTERN", "LANTERN | NOLANTERN + DARKROOMS");

            // Isma's
            ProvideLogicSubstitution("Fungus2_06[left2]", "ACID", "ACID | NOACID + WINGS + (LEFTDASH | (RIGHTCLAW + LEFTSUPERDASH))");
            ProvideMacroSubstitution("Fungus2_06", "ACID", "ACID | NOACID + RIGHTDASH + (WINGS | RIGHTCLAW)");
            ProvideLogicSubstitution("Fungus1_26[left1]", "ACID", "ACID | NOACID + LEFTSUPERDASH + LEFTDASH + WINGS");
            ProvideMacroSubstitution("Deepnest_East_04", "ACID", "ACID | RIGHTSKIPACID");
            ProvideLogicSubstitution("Deepnest_East_04[left1]", "ACID", "ACID | NOACID + LEFTDASH + LEFTSUPERDASH + WINGS");
            ProvideLogicSubstitution(LocationNames.Love_Key, "SPICYSKIPS", "SPICYSKIPS | NOACID");
            ProvideLogicSubstitution(LocationNames.Mask_Shard_Grey_Mourner, "ACID", "ACID | NOACID");


            // Swim
            // No changes required
        }

        private static void DefineTermsAndItems(GenerationSettings gs, LogicManagerBuilder lmb)
        {
            if (!RandoPlus.GS.Any) return;

            Term noLanternTerm = lmb.GetOrAddTerm("NOLANTERN");
            lmb.AddItem(new CappedItem(Consts.NoLantern, new TermValue[] { new(noLanternTerm, 1) }, new(noLanternTerm, 1)));
            Term noTearTerm = lmb.GetOrAddTerm("NOACID");
            lmb.AddItem(new CappedItem(Consts.NoTear, new TermValue[] { new(noTearTerm, 1) }, new(noTearTerm, 1)));
            Term noSwimTerm = lmb.GetOrAddTerm("NOSWIM");
            lmb.AddItem(new CappedItem(Consts.NoSwim, new TermValue[] { new(noSwimTerm, 1) }, new(noSwimTerm, 1)));
        }
    }
}
