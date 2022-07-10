using System;
using HutongGames.PlayMaker;
using ItemChanger;
using ItemChanger.Extensions;
using ItemChanger.FsmStateActions;
using ItemChanger.Locations;
using ItemChanger.Util;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace RandoPlus.NailUpgrades
{
    public class NailsmithLocation : AutoLocation, ILocalHintLocation
    {
        /// <summary>
        /// 1, 2, 3 or 4 corresponding to which nail level is being "bought".
        /// </summary>
        public int NailUpgradeSlot;

        public bool HintActive { get; set; }

        protected override void OnLoad()
        {
            ItemChangerMod.Modules.GetOrAdd<NailsmithLocationModule>().SubscribeLocation(this);
            Events.AddSceneChangeEdit(sceneName, MakeShinyForRespawnedItems);
            Events.AddLanguageEdit(new("Nailsmith", $"NAILSMITH_COMPLETE_{NailUpgradeSlot}"), RemoveNailTextFromCompleteText);
            Events.AddLanguageEdit(new("Nailsmith", $"NAILSMITH_NEED_ORE{NailUpgradeSlot - 1}"), ShowPreviewWhenNotEnoughOre);
        }

        protected override void OnUnload()
        {
            ItemChangerMod.Modules.Get<NailsmithLocationModule>().UnsubscribeLocation(this);
            Events.RemoveSceneChangeEdit(sceneName, MakeShinyForRespawnedItems);
            Events.RemoveLanguageEdit(new("Nailsmith", $"NAILSMITH_COMPLETE_{NailUpgradeSlot}"), RemoveNailTextFromCompleteText);
            Events.RemoveLanguageEdit(new("Nailsmith", $"NAILSMITH_NEED_ORE{NailUpgradeSlot - 1}"), ShowPreviewWhenNotEnoughOre);
        }

        private void ShowPreviewWhenNotEnoughOre(ref string value)
        {
            if (!this.GetItemHintActive()) return;
            if (NailUpgradeSlot == 1) return;

            string pieces = NailUpgradeSlot switch
            {
                2 => "a piece",
                3 => "two pieces",
                4 => "three pieces",
                _ => throw new ArgumentException(nameof(NailUpgradeSlot)),
            };

            string prefix = $"If you bring me {pieces} of Pale Ore, I can forge you";
            string preview = Placement.GetUIName(60);
            value = $"{prefix} {preview}.";
            Placement.OnPreview(preview);
        }

        private void RemoveNailTextFromCompleteText(ref string value)
        {
            value = value.Split(new[] { "<page>" }, StringSplitOptions.None)[0];
            value = value.Replace("reforging", "forging");
            if (NailUpgradeSlot == 4)
            {
                value += "<page>To think this moment has come upon me so soon...<page>...I... I must step outside a moment...";
            }
        }

        private void MakeShinyForRespawnedItems(Scene scene)
        {
            if (Placement.CheckVisitedAny(VisitState.Accepted) && !Placement.AllObtained())
            {
                GameObject nailsmith = scene.FindGameObjectByName("Nailsmith");
                Container c = Container.GetContainer(Container.Shiny);

                ContainerInfo info = new(c.Name, Placement, flingType, (Placement as ItemChanger.Placements.ISingleCostPlacement)?.Cost);
                GameObject shiny = c.GetNewContainer(info);

                c.ApplyTargetContext(shiny, nailsmith.transform.position.x, nailsmith.transform.position.y, 0f);
                ShinyUtility.FlingShinyLeft(shiny.LocateMyFSM("Shiny Control"));
            }
        }

        internal Cost GetCost()
        {
            if (Placement is ItemChanger.Placements.ISingleCostPlacement iscp && iscp.Cost is Cost c) return c;
            return Placement.GetTag<CostTag>()?.Cost;
        }
    }
}
