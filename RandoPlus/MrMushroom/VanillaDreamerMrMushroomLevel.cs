using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using ItemChanger;
using ItemChanger.Extensions;
using ItemChanger.FsmStateActions;
using ItemChanger.Modules;

namespace RandoPlus.MrMushroom
{
    public class VanillaDreamerMrMushroomLevel : Module
    {
        public override void Initialize()
        {
            Events.AddFsmEdit(new("Dreamer NPC", "Control"), PatchMrMushroomIncrement);
        }

        public override void Unload()
        {
            Events.RemoveFsmEdit(new("Dreamer NPC", "Control"), PatchMrMushroomIncrement);
        }

        private void PatchMrMushroomIncrement(PlayMakerFSM fsm)
        {
            FsmState state = fsm.GetState("All Guardians Defeated");

            for (int i = 0; i < state.Actions.Length; i++)
            {
                if (state.Actions[i] is SetPlayerDataInt { intName.Value: nameof(PlayerData.mrMushroomState) })
                {
                    state.Actions[i] = new Lambda(() => PlayerData.instance.IncrementInt(nameof(PlayerData.mrMushroomState)));
                }
            }
        }
    }
}
