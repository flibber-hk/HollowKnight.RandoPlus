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

            // Lantern
            ProvideLogicSubstitution("Mines_33[right1]", "LANTERN", "LANTERN | NOLANTERN + DARKROOMS");
            ProvideLogicSubstitution("Mines_33[left1]", "LANTERN", "LANTERN | NOLANTERN + DARKROOMS");
            ProvideLogicSubstitution("Defeated_No_Eyes", "LANTERN", "LANTERN | NOLANTERN + DARKROOMS");
            ProvideLogicSubstitution(LocationNames.Boss_Essence_No_Eyes, "LANTERN", "LANTERN | NOLANTERN + DARKROOMS");
            ProvideLogicSubstitution(LocationNames.Geo_Rock_Crystal_Peak_Entrance, "LANTERN", "LANTERN | NOLANTERN + DARKROOMS");
            ProvideLogicSubstitution(LocationNames.Geo_Rock_Crystal_Peak_Entrance_Dupe_1, "LANTERN", "LANTERN | NOLANTERN + DARKROOMS");
            ProvideLogicSubstitution(LocationNames.Geo_Rock_Crystal_Peak_Entrance_Dupe_2, "LANTERN", "LANTERN | NOLANTERN + DARKROOMS");

            // Isma's
            ProvideLogicSubstitution("Fungus2_06[left2]", "ACID", "ACID | NOACID + WINGS + (LEFTDASH | (RIGHTCLAW + LEFTSUPERDASH))");
            ProvideLogicSubstitution("Fungus2_06", "ACID", "ACID | NOACID + RIGHTDASH + (WINGS | RIGHTCLAW)");
            ProvideLogicSubstitution("Fungus1_26[left1]", "ACID", "ACID | NOACID + LEFTSUPERDASH + LEFTDASH + WINGS");
            ProvideLogicSubstitution("Deepnest_East_04", "ACID", "ACID | NOACID + RIGHTSKIPACID");
            ProvideLogicSubstitution("Deepnest_East_04[left1]", "ACID", "ACID | NOACID + LEFTDASH + LEFTSUPERDASH + WINGS");
            ProvideLogicSubstitution(LocationNames.Mask_Shard_Grey_Mourner, "ACID", "ACID | NOACID");

            // Swim
            // No changes required
        }

        private static void DefineTermsAndItems(GenerationSettings gs, LogicManagerBuilder lmb)
        {
            if (!RandoPlus.GS.Any) return;

            Term noLanternTerm = lmb.GetOrAddTerm("NOLANTERN");
            Term darkrooms = lmb.GetOrAddTerm("DARKROOMS");
            lmb.AddItem(new MultiSetItem(Consts.NoLantern, new Term[] { noLanternTerm, darkrooms }));
            
            Term noTearTerm = lmb.GetOrAddTerm("NOACID");
            Term noSwimTerm = lmb.GetOrAddTerm("NOSWIM");
            Term acidskips = lmb.GetOrAddTerm("ACIDSKIPS");
            lmb.AddItem(new MultiSetItem(Consts.NoTear, new Term[] { noTearTerm, acidskips }));
            lmb.AddItem(new MultiSetItem(Consts.NoSwim, new Term[] { noSwimTerm, acidskips }));
        }
    }
}
