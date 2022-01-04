using ItemChanger;
using System;
using ItemChanger.Extensions;
using HutongGames.PlayMaker.Actions;

namespace RandoPlus.Items
{
    public class NoTearModule : ItemChanger.Modules.Module
    {
        public bool hasAcidArmourAny => gotNoTear || PlayerData.instance.GetBool(nameof(PlayerData.hasAcidArmour));
        public bool gotNoTear { get; set; }

        public override void Initialize()
        {
            Modding.ModHooks.GetPlayerBoolHook += OverrideGetBool;
            Events.AddFsmEdit(SceneNames.Fungus2_06, new("Acid Blocker", "FSM"), DisableAcidBlocker);
            Events.AddFsmEdit(new("Equipment", "Build Equipment List"), ShowNoTearInInventory);
            Modding.ModHooks.AfterTakeDamageHook += DisableAcidDamage;
            Events.AddLanguageEdit(new("UI", "INV_NAME_ACIDARMOUR"), EditAcidArmourName);
            Events.AddLanguageEdit(new("UI", "INV_DESC_ACIDARMOUR"), EditAcidArmourDesc);
            Modding.ModHooks.SetPlayerBoolHook += OverrideSetBool;
        }

        public override void Unload()
        {
            Modding.ModHooks.GetPlayerBoolHook -= OverrideGetBool;
            Events.RemoveFsmEdit(SceneNames.Fungus2_06, new("Acid Blocker", "FSM"), DisableAcidBlocker);
            Events.RemoveFsmEdit(new("Equipment", "Build Equipment List"), ShowNoTearInInventory);
            Modding.ModHooks.AfterTakeDamageHook -= DisableAcidDamage;
            Events.RemoveLanguageEdit(new("UI", "INV_NAME_ACIDARMOUR"), EditAcidArmourName);
            Events.RemoveLanguageEdit(new("UI", "INV_DESC_ACIDARMOUR"), EditAcidArmourDesc);
            Modding.ModHooks.SetPlayerBoolHook -= OverrideSetBool;
        }

        private bool OverrideSetBool(string boolName, bool value)
        {
            switch (boolName)
            {
                case nameof(gotNoTear):
                    gotNoTear = value;
                    break;
            }
            return value;
        }

        private void EditAcidArmourName(ref string value)
        {
            value = "Not " + value;
        }

        private void EditAcidArmourDesc(ref string value)
        {
            value = value.Replace("protection", "no protection");
        }

        private void ShowNoTearInInventory(PlayMakerFSM fsm)
        {
            fsm.GetState("Acid Armour").GetFirstActionOfType<PlayerDataBoolTest>().boolName.Value = nameof(hasAcidArmourAny);
        }

        private bool OverrideGetBool(string name, bool orig)
        {
            return name switch
            {
                nameof(gotNoTear) => gotNoTear,
                nameof(hasAcidArmourAny) => hasAcidArmourAny,
                _ => orig
            };
        }
        private void DisableAcidBlocker(PlayMakerFSM fsm)
        {
            fsm.FsmVariables.FindFsmString("playerData bool").Value = nameof(hasAcidArmourAny);
        }
        private int DisableAcidDamage(int hazardType, int damageAmount)
        {
            // For some reason, we need to subtract one from the hazardType
            if (hazardType - 1 == (int)GlobalEnums.HazardType.ACID && gotNoTear)
            {
                return 0;
            }
            return damageAmount;
        }
    }
}
