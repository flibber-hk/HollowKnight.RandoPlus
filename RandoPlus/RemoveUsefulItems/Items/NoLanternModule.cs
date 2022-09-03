using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using ItemChanger;
using ItemChanger.Extensions;
using ItemChanger.FsmStateActions;
using System;

namespace RandoPlus.RemoveUsefulItems.Items
{
    public class NoLanternModule : ItemChanger.Modules.Module
    {
        // This should be synced with hasLantern unless lantern is given through a separate mechanism.
        public bool gotNoLantern { get; set; }

        /// <summary>
        /// If this is true, hazard respawns will stay when the player has not lantern. If not, then
        /// the vanilla behaviour (respawn at start of room) will persist when obtaining not lantern.
        /// </summary>
        public bool KeepHazardRespawnsWithNoLantern { get; set; } = false;

        public override void Initialize()
        {
            Modding.ModHooks.GetPlayerBoolHook += OverrideBoolGet;
            Modding.ModHooks.SetPlayerBoolHook += OverrideSetBool;
            Events.AddLanguageEdit(new("UI", "INV_NAME_LANTERN"), EditLanternName);
            Events.AddLanguageEdit(new("UI", "INV_DESC_LANTERN"), EditLanternDesc);
            Events.AddFsmEdit(new("Vignette", "Darkness Control"), RemoveLanternFromVignette);
            On.DeactivateInDarknessWithoutLantern.Start += RemoveHazardRespawnsWithNoLantern;
            Events.AddFsmEdit(SceneNames.Fungus1_35, new("Ghost Warrior NPC", "Conversation Control"), SetNoEyesHazardRespawn);
        }

        public override void Unload()
        {
            Modding.ModHooks.GetPlayerBoolHook -= OverrideBoolGet;
            Modding.ModHooks.SetPlayerBoolHook -= OverrideSetBool;
            Events.RemoveLanguageEdit(new("UI", "INV_NAME_LANTERN"), EditLanternName);
            Events.RemoveLanguageEdit(new("UI", "INV_DESC_LANTERN"), EditLanternDesc);
            Events.RemoveFsmEdit(new("Vignette", "Darkness Control"), RemoveLanternFromVignette);
            On.DeactivateInDarknessWithoutLantern.Start -= RemoveHazardRespawnsWithNoLantern;
            Events.RemoveFsmEdit(SceneNames.Fungus1_35, new("Ghost Warrior NPC", "Conversation Control"), SetNoEyesHazardRespawn);
        }

        private void SetNoEyesHazardRespawn(PlayMakerFSM fsm)
        {
            if (KeepHazardRespawnsWithNoLantern) return;

            fsm.GetState("Start Fight").AddFirstAction(new Lambda(() =>
            {
                // Compatibility with mods that change the darkness level of F1_35
                if (GameManager.instance.sm.darknessLevel == 2)
                {
                    HeroController.instance.SetHazardRespawn(HeroController.instance.transform.position, true);
                }
            }));
        }

        private void RemoveHazardRespawnsWithNoLantern(On.DeactivateInDarknessWithoutLantern.orig_Start orig, DeactivateInDarknessWithoutLantern self)
        {
            orig(self);

            if (KeepHazardRespawnsWithNoLantern) return;

            // If gotNoLantern is false, then assume also hasLantern is false and orig(self) would correctly deactivate the object
            // If gotNoLantern is true, then assume also hasLantern is true so orig(self) would incorrectly fail to deactivate the object.
            if (self.GetComponent<HazardRespawnTrigger>() != null && gotNoLantern && self.gameObject.activeSelf)
            {
                self.gameObject.SetActive(false);
                return;
            }
        }

        private void RemoveLanternFromVignette(PlayMakerFSM fsm)
        {
            fsm.GetState("Dark Lev Check").RemoveFirstActionOfType<PlayerDataBoolTest>();
            fsm.GetState("Scene Reset").RemoveFirstActionOfType<PlayerDataBoolTest>();
        }

        private bool OverrideSetBool(string boolName, bool value)
        {
            switch (boolName)
            {
                case nameof(gotNoLantern):
                    gotNoLantern = value;
                    PlayerData.instance.SetBool(nameof(PlayerData.hasLantern), value);
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

        private bool OverrideBoolGet(string name, bool orig)
        {
            return name switch
            {
                nameof(gotNoLantern) => gotNoLantern,
                _ => orig
            };
        }
    }
}
