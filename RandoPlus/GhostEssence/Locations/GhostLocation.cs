using System.Collections.Generic;
using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using ItemChanger;
using ItemChanger.Extensions;
using ItemChanger.FsmStateActions;
using ItemChanger.Locations;

namespace RandoPlus.GhostEssence.Locations
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
            fsmStateActions.Add(new Lambda(() => Placement.AddVisitFlag(VisitState.Accepted)));
            fsmStateActions.Add(new AsyncLambda(GiveAllAsync(fsm.transform)));
            impact.Actions = fsmStateActions.ToArray();

            FsmState init = fsm.GetState("Init");
            BoolTest oldtest = init.GetFirstActionOfType<BoolTest>();
            FsmStateAction newtest = new DelegateBoolTest(() => Placement.AllObtained(), oldtest);
            init.RemoveAction(2);
            init.AddLastAction(newtest);

            // The Revek check works as follows:
            // - Each non-revek ghost increments the alive ghosts counter in "Spirit Glade" state
            // - The GhostBattleRevek-Control fsm receives the increments, and broadcasts the REVEK DEJECTED event
            //   to inform other fsms that Revek should be sad
            // We do not want to increment the count for alive ghosts that have previously been checked.
            FsmState glade = fsm.GetState("Spirit Glade");
            glade.AddFirstAction(new DelegateBoolTest(() => Placement.CheckVisitedAny(VisitState.Accepted | VisitState.ObtainedAnyItem), "FINISHED", null));
        }
    }
}
