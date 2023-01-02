using HutongGames.PlayMaker;
using ItemChanger;
using ItemChanger.Extensions;
using ItemChanger.FsmStateActions;

namespace RandoPlus.GhostEssence.Locations
{
    public class JoniGhostLocation : GhostLocation
    {
        protected override void OnLoad()
        {
            base.OnLoad();
            Events.AddFsmEdit(sceneName, new("Ghost Activator", "FSM"), MakeGhostAppear);
            AbstractItem.AfterGiveGlobal += BroadcastShiny;
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            Events.RemoveFsmEdit(sceneName, new("Ghost Activator", "FSM"), MakeGhostAppear);
            AbstractItem.AfterGiveGlobal -= BroadcastShiny;
        }

        private void BroadcastShiny(ReadOnlyGiveEventArgs args)
        {
            if (args.Placement.Name == LocationNames.Jonis_Blessing && GameManager.instance.sceneName == SceneNames.Cliffs_05)
            {
                PlayMakerFSM.BroadcastEvent("SHINY PICKED UP");
            }
        }

        private void MakeGhostAppear(PlayMakerFSM fsm)
        {
            FsmState idle = fsm.GetState("Idle");
            idle.RemoveActionsOfType<FsmStateAction>();

            // If they checked Joni
            idle.AddLastAction(new DelegateBoolTest(
                JonisBlessingCollected,
                "SHINY PICKED UP",
                null));
            
        }

        private static bool JonisBlessingCollected()
        {
            if (ItemChanger.Internal.Ref.Settings.Placements.TryGetValue(LocationNames.Jonis_Blessing, out AbstractPlacement pmt))
            {
                return pmt.CheckVisitedAny(VisitState.ObtainedAnyItem);
            }
            else
            {
                return PlayerData.instance.GetBool(nameof(PlayerData.gotCharm_27));
            }
        }
    }
}
