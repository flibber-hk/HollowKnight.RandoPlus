using HutongGames.PlayMaker;
using ItemChanger;
using ItemChanger.Extensions;
using ItemChanger.FsmStateActions;
using ItemChanger.Locations;
using System;
namespace RandoPlus.NailUpgrades
{
    public class NailsmithLocation : AutoLocation
    {
        private int UpgradesTaken = RandoPlus.GS.UpgradesTaken;

        protected override void OnLoad()
        {
            Events.AddFsmEdit(new("Nailsmith", "Conversation Control"), ConvoPatcher);
            Events.AddLanguageEdit(new("NAILSMITH_UPGRADE_" + Placement.Name[Placement.Name.Length - 1].ToString()), UpdateText);
            Events.AddLanguageEdit(new("NAILSMITH_OFFER"), ShowPickups);


        }
        protected override void OnUnload()
        {
            Events.RemoveFsmEdit(new("Nailsmith", "Conversation Control"), ConvoPatcher);
            Events.RemoveLanguageEdit(new("NAILSMITH_UPGRADE_" + Placement.Name[Placement.Name.Length - 1].ToString()), UpdateText);
            Events.RemoveLanguageEdit(new("NAILSMITH_OFFER"), ShowPickups);

        }

        private void ShowPickups(ref string value)
        {
            value += $"<br> Nail {Placement.Name[Placement.Name.Length - 1].ToString()}: {Placement.Items[0].GetPreviewName()} ";
        }

        public void UpdateText(ref string value)
        {
            if (Placement.Name[Placement.Name.Length - 1].ToString() == (RandoPlus.GS.UpgradesTaken + 1).ToString())
            {
                value = $"Give your Geo for {Placement.Items[0].GetPreviewName()}";
            }

        }

        public void ConvoPatcher(PlayMakerFSM fsm)
        {
            FsmState upgrade = fsm.GetState("Upgrade");

            upgrade.AddLastAction(new AsyncLambda(GiveItem));


            void GiveItem(Action callback = null)
            {
                if (Placement.Name[Placement.Name.Length - 1].ToString() != (RandoPlus.GS.UpgradesTaken + 1).ToString()) { return; }

                RandoPlus.GS.UpgradesTaken = RandoPlus.GS.UpgradesTaken + 1;
                fsm.FsmVariables.GetFsmInt("Upgrades Completed").Value = RandoPlus.GS.UpgradesTaken;

                GiveAll();
                fsm.SendEvent("FINISHED");
            }
        }
    }
}
