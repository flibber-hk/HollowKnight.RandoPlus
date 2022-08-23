﻿using System;
using System.Collections.Generic;
using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using ItemChanger;
using ItemChanger.Extensions;
using ItemChanger.FsmStateActions;
using ItemChanger.Locations;
using ItemChanger.Placements;
using ItemChanger.Tags;
using ItemChanger.Util;
using UnityEngine;
using UnityEngine.SceneManagement;
using Modding;

namespace RandoPlus.Ghost
{
    public class GhostLocation : AutoLocation
    {
        public string objectName;

        protected override void OnLoad()
        {
            Events.AddFsmEdit(sceneName, new(objectName, "ghost_npc_death"), ModifyGhostDeath);            
        }

        protected override void OnUnload()
        {
            Events.RemoveFsmEdit(sceneName, new(objectName, "ghost_npc_death"), ModifyGhostDeath);
        }
        private void ModifyGhostDeath(PlayMakerFSM fsm)
        {
            FsmState impact = fsm.GetState("Impact");
            List<FsmStateAction> fsmStateActions = impact.Actions.ToList();
            fsmStateActions.RemoveAt(fsmStateActions.Count - 1);
            fsmStateActions.RemoveAt(fsmStateActions.Count - 1);
            fsmStateActions.Add(new AsyncLambda(GiveAllAsync(fsm.transform)));
            impact.Actions = fsmStateActions.ToArray();

            FsmState init = fsm.GetState("Init");
            BoolTest oldtest = init.GetFirstActionOfType<BoolTest>();
            FsmStateAction newtest = new DelegateBoolTest(() => Placement.AllObtained(), oldtest);
            init.RemoveAction(2);
            init.AddLastAction(newtest);
        }
    }

    public class GhostClothLocation : GhostLocation
    {
        protected override void OnLoad()
        {
            base.OnLoad();
            ModHooks.GetPlayerBoolHook += ClothBoolHook;
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            ModHooks.GetPlayerBoolHook -= ClothBoolHook;
        }

        private bool ClothBoolHook(string name, bool orig)
        {
            if (name == "clothKilled")
            {
                return PlayerData.instance.GetBool("savedCloth") && PlayerData.instance.GetBool("killedTraitorLord");
            }
            if (name == "clothGhostSpoken")
            {
                return false;
            }
            return orig;
        }
    }
}
