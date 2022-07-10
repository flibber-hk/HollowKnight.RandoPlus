using System;
using System.Collections.Generic;
using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using ItemChanger;
using ItemChanger.Extensions;
using ItemChanger.FsmStateActions;
using ItemChanger.Locations;
using ItemChanger.Placements;
using ItemChanger.Tags;
using ItemChanger.Util;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RandoPlus.MrMushroom
{
    public class MrMushroomLocation : ContainerLocation, ILocalHintLocation
    {
        public int mushroomState;
        public string objectName;
        public bool HintActive { get; set; }


        public string GetLangKey(int i) => $"MR_MUSHROOM_SHROOMISH_{Placement.Name}_{i}";

        public bool MushroomUnlocked() => PlayerData.instance.GetInt(nameof(PlayerData.mrMushroomState)) >= mushroomState;
        public bool SuccessfullyInteracted() => Placement.CheckVisitedAll(VisitState.Accepted);
        public bool Appears() => MushroomUnlocked()
            && !SuccessfullyInteracted()
            && !Placement.AllObtained();

        protected override void OnLoad()
        {
            Events.AddFsmEdit(sceneName, new(objectName, "Control"), ModifyMrMushroom);
            Events.AddFsmEdit(sceneName, new(objectName, "Conversation Control"), ModifyMushConvo);
            Events.AddFsmEdit(sceneName, new(objectName, "Conversation Control"), AddHintText);

            for (int i = 1; i <= 3; i++)
            {
                Events.AddLanguageEdit(new LanguageKey("Minor NPC", GetLangKey(i)), GetHintText);
            }
        }
        protected override void OnUnload()
        {
            Events.RemoveFsmEdit(sceneName, new(objectName, "Control"), ModifyMrMushroom);
            Events.RemoveFsmEdit(sceneName, new(objectName, "Conversation Control"), ModifyMushConvo);
            Events.RemoveFsmEdit(sceneName, new(objectName, "Conversation Control"), AddHintText);

            for (int i = 1; i <= 3; i++)
            {
                Events.RemoveLanguageEdit(new LanguageKey("Minor NPC", GetLangKey(i)), GetHintText);
            }
        }

        private void GetHintText(ref string value)
        {
            char i = value[value.Length - 4];
            value = Language.Language.Get($"MR_MUSHROOM_SHROOMISH{i}", "Minor NPC");

            if (!this.GetItemHintActive()) return;

            string preview = Placement.GetUIName();
            string[] valueSplit = value.Split(new[] { ',' }, 2);
            value = $"{valueSplit[0]}, {preview},{valueSplit[1]}";
            Placement.OnPreview(preview);
        }

        private void AddHintText(PlayMakerFSM fsm)
        {
            for (int i = 1; i <= 3; i++)
            {
                FsmState shrumish = fsm.GetState($"Shrumish {i}");
                CallMethodProper cmp = shrumish.GetFirstActionOfType<CallMethodProper>();
                cmp.parameters[0].stringValue = GetLangKey(i);
            }
        }

        private void ModifyMushConvo(PlayMakerFSM fsm)
        {
            FsmState convo = fsm.GetState("Convo");
            convo.RemoveActionsOfType<IncrementPlayerDataInt>();
            convo.Actions[0] = new Lambda(() => fsm.FsmVariables.GetFsmInt("Mushroom State").Value = mushroomState);

            fsm.GetState("Send Rocket").AddFirstAction(new Lambda(() => Placement.AddVisitFlag(VisitState.Accepted)));
            fsm.GetState("Send Leave").AddFirstAction(new Lambda(() => Placement.AddVisitFlag(VisitState.Accepted)));
        }

        private void ModifyMrMushroom(PlayMakerFSM fsm)
        {
            fsm.GetState("Check").Actions[1] = new DelegateBoolTest(Appears, "FINISHED", "DESTROY");

            fsm.GetState("Left").AddFirstAction(new Lambda(() => PlaceContainer(fsm.gameObject)));
            fsm.GetState("Box Down").AddFirstAction(new Lambda(() => PlaceContainer(fsm.gameObject)));
            fsm.GetState("Destroy").AddFirstAction(new Lambda(() => 
            { 
                // This handles respawned items and geo rock shells, as well as dropped but unclaimed items
                if (SuccessfullyInteracted())
                {
                    PlaceContainer(fsm.gameObject);
                }
            }));
        }

        private void PlaceContainer(GameObject mush)
        {
            base.GetContainer(out GameObject obj, out string containerType);
            Container.GetContainer(containerType).ApplyTargetContext(obj, mush, 0);
            if (containerType == Container.Shiny && !Placement.GetPlacementAndLocationTags().OfType<ShinyFlingTag>().Any())
            {
                ShinyUtility.SetShinyFling(obj.LocateMyFSM("Shiny Control"), ShinyFling.RandomLR);
            }
        }
    }
}
