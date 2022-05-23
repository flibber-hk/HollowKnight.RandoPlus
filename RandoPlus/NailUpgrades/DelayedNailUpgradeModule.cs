using System;
using System.Collections.Generic;
using System.Linq;
using ItemChanger;
using ItemChanger.Extensions;
using ItemChanger.FsmStateActions;
using ItemChanger.Modules;

namespace RandoPlus.NailUpgrades
{
    public class DelayedNailUpgradeModule : Module
    {
        public int UnclaimedUpgrades = 0;
        public int DamagePerNailUpgrade = 4;

        public bool GiveNailUpgradesOnPickup { get; set; } = false;

        public void GiveNailUpgrade() 
        { 
            UnclaimedUpgrades++;

            if (GiveNailUpgradesOnPickup)
            {
                TryClaimNailUpgrade();
            }
        }

        public override void Initialize()
        {
            Events.AddFsmEdit(new("Inv", "UI Inventory"), AllowNailUpgradeClaim);
            for (int i = 1; i <= 5; i++) Events.AddLanguageEdit(new("UI", "INV_DESC_NAIL" + i), ShowNailUpgrades);
        }
        public override void Unload()
        {
            Events.RemoveFsmEdit(new("Inv", "UI Inventory"), AllowNailUpgradeClaim);
            for (int i = 1; i <= 5; i++) Events.RemoveLanguageEdit(new("UI", "INV_DESC_NAIL" + i), ShowNailUpgrades);
        }

        private void ShowNailUpgrades(ref string value)
        {
            if (UnclaimedUpgrades > 0)
            {
                value += $"<br><br>Press the attack button to claim a nail upgrade ({UnclaimedUpgrades} unclaimed).";
            }
            else if (!GiveNailUpgradesOnPickup)
            {
                value += $"<br><br>No unclaimed nail upgrades.";
            }
        }

        private void AllowNailUpgradeClaim(PlayMakerFSM fsm)
        {
            fsm.GetState("Nail").AddLastAction(new LambdaEveryFrame(ListenForNailPress));

            void ListenForNailPress()
            {
                if (InputHandler.Instance.inputActions.attack.WasPressed)
                {
                    if (!TryClaimNailUpgrade()) return;

                    int nailsmithUpgrades = 1 + PlayerData.instance.GetInt(nameof(PlayerData.nailSmithUpgrades));
                    PlayMakerFSM updateText = fsm.gameObject.LocateMyFSM("Update Text");
                    updateText.FsmVariables.GetFsmString("Convo Name").Value = "INV_NAME_NAIL" + nailsmithUpgrades;
                    updateText.FsmVariables.GetFsmString("Convo Desc").Value = "INV_DESC_NAIL" + nailsmithUpgrades;
                    fsm.gameObject.GetComponentInChildren<InvNailSprite>().SendMessage("OnEnable");

                    updateText.SendEvent("UPDATE TEXT");
                }
            }
        }

        public bool TryClaimNailUpgrade()
        {
            if (UnclaimedUpgrades == 0) return false;

            UnclaimedUpgrades--;

            PlayerData.instance.SetBool(nameof(PlayerData.honedNail), true);
            PlayerData.instance.IntAdd(nameof(PlayerData.nailDamage), DamagePerNailUpgrade);
            PlayMakerFSM.BroadcastEvent("UPDATE NAIL DAMAGE");
            if (PlayerData.instance.GetInt(nameof(PlayerData.nailSmithUpgrades)) < 4)
            {
                PlayerData.instance.IncrementInt(nameof(PlayerData.nailSmithUpgrades));
            }

            return true;
        }
    }
}
