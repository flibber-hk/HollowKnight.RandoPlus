using HutongGames.PlayMaker.Actions;
using ItemChanger;
using ItemChanger.Extensions;
using ItemChanger.FsmStateActions;
using System;

namespace RandoContentEnforcer.Items
{
    public class NoLanternModule : ItemChanger.Modules.Module
    {
        public bool hasLanternAny => gotNoLantern || PlayerData.instance.GetBool(nameof(PlayerData.hasLantern));
        public bool gotNoLantern { get; set; }

        public override void Initialize()
        {
            Modding.ModHooks.GetPlayerBoolHook += OverrideBoolGet;
            Events.AddFsmEdit(SceneNames.Mines_33, new("Toll Gate Machine", "Disable if No Lantern"), RemoveLanternCheck);
            Events.AddFsmEdit(SceneNames.Mines_33, new("Toll Gate Machine (1)", "Disable if No Lantern"), RemoveLanternCheck);
            Events.AddFsmEdit(SceneNames.Fungus1_35, new("Ghost Warrior NPC", "FSM"), RemoveLanternCheck);
            Events.AddFsmEdit(SceneNames.Fungus1_35, new("Ghost Warrior NPC", "Conversation Control"), SetHazardRespawn);
            Events.AddLanguageEdit(new("UI", "INV_NAME_LANTERN"), EditLanternName);
            Events.AddLanguageEdit(new("UI", "INV_DESC_LANTERN"), EditLanternDesc);
            Events.AddFsmEdit(new("Equipment", "Build Equipment List"), ShowNoLanternInInventory);
            Modding.ModHooks.SetPlayerBoolHook += OverrideSetBool;
        }

        public override void Unload()
        {
            Modding.ModHooks.GetPlayerBoolHook -= OverrideBoolGet;
            Events.RemoveFsmEdit(SceneNames.Mines_33, new("Toll Gate Machine", "Disable if No Lantern"), RemoveLanternCheck);
            Events.RemoveFsmEdit(SceneNames.Mines_33, new("Toll Gate Machine (1)", "Disable if No Lantern"), RemoveLanternCheck);
            Events.RemoveFsmEdit(SceneNames.Fungus1_35, new("Ghost Warrior NPC", "FSM"), RemoveLanternCheck);
            Events.RemoveFsmEdit(SceneNames.Fungus1_35, new("Ghost Warrior NPC", "Conversation Control"), SetHazardRespawn);
            Events.RemoveLanguageEdit(new("UI", "INV_NAME_LANTERN"), EditLanternName);
            Events.RemoveLanguageEdit(new("UI", "INV_DESC_LANTERN"), EditLanternDesc);
            Events.RemoveFsmEdit(new("Equipment", "Build Equipment List"), ShowNoLanternInInventory);
            Modding.ModHooks.SetPlayerBoolHook -= OverrideSetBool;
        }

        private void ShowNoLanternInInventory(PlayMakerFSM fsm)
        {
            fsm.GetState("Lantern").GetFirstActionOfType<PlayerDataBoolTest>().boolName.Value = nameof(hasLanternAny);
        }

        private bool OverrideSetBool(string boolName, bool value)
        {
            switch (boolName)
            {
                case nameof(gotNoLantern):
                    gotNoLantern = value;
                    break;
            }
            return value;
        }

        private void EditLanternDesc(ref string value)
        {
            value = "Crystal lantern not containing a Lumafly. Doesn't brighten dark caverns so wanderers can't find their way.";
        }

        private void EditLanternName(ref string value)
        {
            value = "Not " + value;
        }

        private void SetHazardRespawn(PlayMakerFSM fsm)
        {
            fsm.GetState("Start Fight").AddFirstAction(new Lambda(() => 
            {
                HeroController.instance.SetHazardRespawn(HeroController.instance.transform.position, true);
            }));
        }

        private void RemoveLanternCheck(PlayMakerFSM fsm)
        {
            fsm.GetState("Check").GetFirstActionOfType<PlayerDataBoolTest>().boolName = nameof(hasLanternAny);
        }

        private bool OverrideBoolGet(string name, bool orig)
        {
            return name switch
            {
                nameof(hasLanternAny) => hasLanternAny,
                nameof(gotNoLantern) => gotNoLantern,
                _ => orig
            };
        }
    }
}
