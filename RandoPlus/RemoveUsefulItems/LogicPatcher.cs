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
