using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using ItemChanger;
using ItemChanger.Extensions;

namespace RandoPlus.RemoveUsefulItems.Items
{
    public class NoLanternModule : ItemChanger.Modules.Module
    {
        public bool gotNoLantern { get; set; }

        public override void Initialize()
        {
            Modding.ModHooks.GetPlayerBoolHook += OverrideBoolGet;
            Modding.ModHooks.SetPlayerBoolHook += OverrideSetBool;
            Events.AddLanguageEdit(new("UI", "INV_NAME_LANTERN"), EditLanternName);
            Events.AddLanguageEdit(new("UI", "INV_DESC_LANTERN"), EditLanternDesc);
            Events.AddFsmEdit(new("Vignette", "Darkness Control"), RemoveLanternFromVignette);
        }

        public override void Unload()
        {
            Modding.ModHooks.GetPlayerBoolHook -= OverrideBoolGet;
            Modding.ModHooks.SetPlayerBoolHook -= OverrideSetBool;
            Events.RemoveLanguageEdit(new("UI", "INV_NAME_LANTERN"), EditLanternName);
            Events.RemoveLanguageEdit(new("UI", "INV_DESC_LANTERN"), EditLanternDesc);
            Events.RemoveFsmEdit(new("Vignette", "Darkness Control"), RemoveLanternFromVignette);
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
