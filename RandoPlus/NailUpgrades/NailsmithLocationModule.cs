using System;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using ItemChanger;
using ItemChanger.Extensions;
using ItemChanger.FsmStateActions;
using ItemChanger.Util;
using Newtonsoft.Json;

namespace RandoPlus.NailUpgrades
{
    /// <summary>
    /// Module which manages the common changes to the nailsmith location FSM.
    /// </summary>
    public class NailsmithLocationModule : ItemChanger.Modules.Module
    {
        [JsonIgnore]
        private Dictionary<int, NailsmithLocation> SubscribedLocations;

        public HashSet<int> SlotsBought = new();

        public int NextSlot
        {
            get
            {
                int i = 1;
                while (SlotsBought.Contains(i)
                    || (SubscribedLocations.TryGetValue(i, out NailsmithLocation loc)
                        && loc.Placement.CheckVisitedAll(VisitState.Accepted)))
                {
                    i++;
                }
                return i;
            }
        }
        public bool ShouldGoModdedPath => SubscribedLocations.ContainsKey(NextSlot);

        public void SubscribeLocation(NailsmithLocation loc)
        {
            SubscribedLocations ??= new();
            if (SubscribedLocations.ContainsKey(loc.NailUpgradeSlot))
            {
                throw new NotSupportedException("Multiple locations at the same nailsmith slot are not supported");
            }
            SubscribedLocations[loc.NailUpgradeSlot] = loc;
        }
        public void UnsubscribeLocation(NailsmithLocation loc)
        {
            SubscribedLocations ??= new();

            SubscribedLocations.Remove(loc.NailUpgradeSlot);
        }

        public override void Initialize()
        {
            SubscribedLocations ??= new();
            Events.AddFsmEdit(SceneNames.Room_nailsmith, new("Nailsmith", "Conversation Control"), PatchFsm);
            Events.AddLanguageEdit(new("Nailsmith", "NAILSMITH_OFFER"), ShowPickups);
            Events.AddLanguageEdit(new("Nailsmith", "NAILSMITH_OFFER_ORE"), ShowPickupsOre);
            Imports.QoL.OverrideSettingToggle("NPCSellAll", "NailsmithBuyAll", false);
        }

        public override void Unload()
        {
            Events.RemoveFsmEdit(SceneNames.Room_nailsmith, new("Nailsmith", "Conversation Control"), PatchFsm);
            Events.RemoveLanguageEdit(new("Nailsmith", "NAILSMITH_OFFER"), ShowPickups);
            Events.RemoveLanguageEdit(new("Nailsmith", "NAILSMITH_OFFER_ORE"), ShowPickupsOre);
            Imports.QoL.RemoveSettingOverride("NPCSellAll", "NailsmithBuyAll");
        }


        private void ShowPickups(ref string value)
        {
            if (!SubscribedLocations.TryGetValue(NextSlot, out NailsmithLocation loc))
            {
                return;
            }

            value = $"If you like, I can forge you something.";
        }
        private void ShowPickupsOre(ref string value)
        {
            if (!SubscribedLocations.TryGetValue(NextSlot, out NailsmithLocation loc))
            {
                return;
            }

            value = $"I see you have some Pale Ore. A rare, fine metal, that. Give me the ore and some Geo for my efforts, and I'll forge you something.";
        }

        public void PatchFsm(PlayMakerFSM fsm)
        {
            // Add FSM int to track the slot we're currently buying
            fsm.AddFsmInt("Current Slot RP", -1);

            FsmState offerType = fsm.GetState("Offer Type");
            offerType.RemoveFirstActionOfType<GetPlayerDataInt>();
            offerType.AddFirstAction(new Lambda(() =>
            {
                // Used to determine the next slot
                fsm.FsmVariables.GetFsmInt("Upgrades Completed").Value = NextSlot - 1;

                // Assign the slot we're currently buying
                fsm.FsmVariables.GetFsmInt("Current Slot RP").Value = NextSlot;
            }));

            FsmState YNVanilla = fsm.GetState("Box Up YN");
            YNVanilla.AddFirstAction(new DelegateBoolTest(() => ShouldGoModdedPath, "DIALOGUE MODDED", null));

            FsmState YNModded = new(fsm.Fsm);
            YNVanilla.AddTransition("DIALOGUE MODDED", YNModded);

            YNModded.AddLastAction(new Lambda(() =>
            {
                NailsmithLocation currentLocation = SubscribedLocations[NextSlot];
                if (currentLocation.GetCost() is GeoCost gc)
                {
                    fsm.FsmVariables.GetFsmInt("Upgrade Cost").Value = gc.amount;
                }
                else
                {
                    fsm.FsmVariables.GetFsmInt("Upgrade Cost").Value = 0;
                }
                YNUtil.OpenYNDialogue(fsm.gameObject, currentLocation.Placement, currentLocation.Placement.Items, currentLocation.GetCost());
            }));

            FsmState yesState = fsm.GetState("Yes");
            YNModded.AddTransition("NO", fsm.GetState("Decline Pause"));
            YNModded.AddTransition("YES", yesState);

            yesState.GetFirstActionOfType<Wait>().time = 0.75f;


            FsmState upgrade = fsm.GetState("Upgrade");
            upgrade.AddFirstAction(new DelegateBoolTest(() => ShouldGoModdedPath, "PAY MODDED", null));

            FsmState payModded = new(fsm.Fsm)
            {
                Name = "Pay And Give Modded"
            };
            upgrade.AddTransition("PAY MODDED", payModded);
            payModded.AddFirstAction(new Lambda(() =>
            {
                NailsmithLocation currentLocation = SubscribedLocations[NextSlot];
                currentLocation.GetCost()?.Pay();
                currentLocation.Placement.AddVisitFlag(VisitState.Accepted);
                currentLocation.GiveAll(() => fsm.SendEvent("GIVE COMPLETE"));
            }));
            payModded.AddTransition("GIVE COMPLETE", "Box Up 4");

            fsm.GetState("Upgrade").RemoveActionsOfType<IncrementPlayerDataInt>();

            fsm.GetState("Box Up 4").AddFirstAction(new Lambda(() =>
            {
                int currentSlot = fsm.FsmVariables.GetFsmInt("Current Slot RP").Value;
                SlotsBought.Add(currentSlot);
            }));

            fsm.GetState("Complete Convo").Actions[2] = new Lambda(() =>
            {
                // Used to decide which convo to show
                fsm.FsmVariables.GetFsmInt("Upgrades Completed").Value = SlotsBought.Count;
            });

        }
    }
}