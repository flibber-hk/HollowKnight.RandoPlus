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
            // High (late) priority
            RCData.RuntimeLogicOverride.Subscribe(100_000, AllowSkips);
        }

        private static void AllowSkips(GenerationSettings gs, LogicManagerBuilder lmb)
        {
            TermToken acidSkips = lmb.LP.GetTermToken("ACIDSKIPS");
            TermToken swim = lmb.LP.GetTermToken("SWIM");
            TermToken acid = lmb.LP.GetTermToken("ACID");

            CreateMacros("SKIPACID");
            CreateMacros("LEFTSKIPACID");
            CreateMacros("RIGHTSKIPACID");
            CreateMacros("FULLSKIPACID");

            void CreateMacros(string orig)
            {
                LogicClauseBuilder waterLcb = new(lmb.LP.GetMacro(orig));
                waterLcb.Subst(acidSkips, lmb.LP.ParseInfixToClause("ACIDSKIPS | NOSWIM"));
                lmb.LP.SetMacro("WATER_" + orig, new LogicClause(waterLcb));

                LogicClauseBuilder acidLcb = new(lmb.LP.GetMacro(orig));
                acidLcb.Subst(acidSkips, lmb.LP.ParseInfixToClause("ACIDSKIPS | NOACID"));
                lmb.LP.SetMacro("ACID_" + orig, new LogicClause(acidLcb));
            }

            List<string> AllLogic = lmb.LogicLookup.Keys.ToList();

            foreach (string key in AllLogic)
            {
                if (lmb.LogicLookup[key].Tokens.Contains(swim))
                {
                    lmb.DoSubst(new(key, "SKIPACID", "WATER_SKIPACID"));
                    lmb.DoSubst(new(key, "LEFTSKIPACID", "WATER_LEFTSKIPACID"));
                    lmb.DoSubst(new(key, "RIGHTSKIPACID", "WATER_RIGHTSKIPACID"));
                    lmb.DoSubst(new(key, "FULLSKIPACID", "WATER_FULLSKIPACID"));
                    lmb.DoSubst(new(key, "ACIDSKIPS", "ACIDSKIPS | NOSWIM"));
                }
                else if (lmb.LogicLookup[key].Tokens.Contains(acid))
                {
                    lmb.DoSubst(new(key, "SKIPACID", "ACID_SKIPACID"));
                    lmb.DoSubst(new(key, "LEFTSKIPACID", "ACID_LEFTSKIPACID"));
                    lmb.DoSubst(new(key, "RIGHTSKIPACID", "ACID_RIGHTSKIPACID"));
                    lmb.DoSubst(new(key, "FULLSKIPACID", "ACID_FULLSKIPACID"));
                    lmb.DoSubst(new(key, "ACIDSKIPS", "ACIDSKIPS | NOACID"));
                }

                lmb.DoSubst(new(key, "DARKROOMS", "DARKROOMS | NOLANTERN"));
            }
        }

        private static void InternalModifyLogic(GenerationSettings gs, LogicManagerBuilder lmb)
        {
            if (!RandoPlus.GS.Any) return;

            // Lantern
            lmb.DoSubst(new("Mines_33[right1]", "LANTERN", "LANTERN | NOLANTERN + DARKROOMS"));
            lmb.DoSubst(new("Mines_33[left1]", "LANTERN", "LANTERN | NOLANTERN + DARKROOMS"));
            lmb.DoSubst(new("Defeated_No_Eyes", "LANTERN", "LANTERN | NOLANTERN + DARKROOMS"));
            lmb.DoSubst(new(LocationNames.Boss_Essence_No_Eyes, "LANTERN", "LANTERN | NOLANTERN + DARKROOMS"));
            lmb.DoSubst(new(LocationNames.Geo_Rock_Crystal_Peak_Entrance, "LANTERN", "LANTERN | NOLANTERN + DARKROOMS"));
            lmb.DoSubst(new(LocationNames.Geo_Rock_Crystal_Peak_Entrance_Dupe_1, "LANTERN", "LANTERN | NOLANTERN + DARKROOMS"));
            lmb.DoSubst(new(LocationNames.Geo_Rock_Crystal_Peak_Entrance_Dupe_2, "LANTERN", "LANTERN | NOLANTERN + DARKROOMS"));

            // Isma's
            lmb.DoSubst(new("Fungus2_06[left2]", "ACID", "ACID | NOACID + WINGS + (LEFTDASH | (RIGHTCLAW + LEFTSUPERDASH))"));
            lmb.DoSubst(new("Fungus2_06", "ACID", "ACID | NOACID + RIGHTDASH + (WINGS | RIGHTCLAW)"));
            lmb.DoSubst(new("Fungus1_26[left1]", "ACID", "ACID | NOACID + LEFTSUPERDASH + LEFTDASH + WINGS"));
            lmb.DoSubst(new("Deepnest_East_04", "ACID", "ACID | NOACID + RIGHTSKIPACID"));
            lmb.DoSubst(new("Deepnest_East_04[left1]", "ACID", "ACID | NOACID + LEFTDASH + LEFTSUPERDASH + WINGS"));
            lmb.DoSubst(new(LocationNames.Mask_Shard_Grey_Mourner, "ACID", "ACID | NOACID"));

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
