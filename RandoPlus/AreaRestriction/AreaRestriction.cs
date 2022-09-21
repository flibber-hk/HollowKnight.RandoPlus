using System;
using System.Collections.Generic;
using ItemChanger;
using ItemChanger.Extensions;
using RandomizerMod.IC;
using RandomizerMod.RC;

namespace RandoPlus.AreaRestriction
{
    public static class AreaRestriction
    {
        public const int AreaCount = 7;

        // Data
        public static readonly List<string> PlacedAreas = new();
        public static readonly List<string> ExcludedAreas = new();
        public static readonly HashSet<string> InvalidLocations = new();

        public static void Hook(bool rando, bool ic)
        {
            if (rando) AreaLimiterRequest.Hook();
            if (rando) HookStartNewGame();
        }

        public static void HookStartNewGame() => RandoController.OnExportCompleted += BeforeGameStart;

        private static void BeforeGameStart(RandoController rc)
        {
            if (RandoPlus.GS.Any && RandoPlus.GS.PreferMultiShiny)
            {
                PreventMultiChests();
            }

            if (!RandoPlus.GS.AreaBlitz) return;

            // TODO - add an in-game indicator for allowed areas?

            ItemChangerMod.Modules.GetOrAdd<AreaLimitModule>().PlacedAreas = new(PlacedAreas);

            Dictionary<string, AbstractPlacement> nothingPlacements = new();
            RandoFactory randoFactory = new(rc.rb);
            ICFactory icFactory = new(rc.rb, nothingPlacements);

            foreach (string locName in InvalidLocations)
            {
                RandoModItem nothing = randoFactory.MakeItemInternal(ItemNames.Lumafly_Escape);
                RandoModLocation madeLocation = randoFactory.MakeLocation(locName);
                madeLocation.info?.onRandomizerFinish?.Invoke(new(nothing, madeLocation));
                icFactory.HandlePlacement(-1, nothing, madeLocation);
            }

            foreach (AbstractPlacement pmt in nothingPlacements.Values)
            {
                ModifyNothingPlacement(pmt);
            }

            ItemChangerMod.AddPlacements(nothingPlacements.Values);
        }

        private static void ModifyNothingPlacement(AbstractPlacement pmt)
        {
            pmt.RemoveTags<RandoPlacementTag>();
            pmt.AddTag<ItemChanger.Tags.CompletionWeightTag>().Weight = 0;
            if (pmt is ItemChanger.Placements.IMultiCostPlacement)
            {
                pmt.Items.Clear();
                pmt.Add(Finder.GetItem(ItemNames.Lumafly_Escape));
            }
            foreach (AbstractItem item in pmt.Items)
            {
                item.RemoveTags<RandoItemTag>();
                item.AddTag<ItemChanger.Tags.CompletionWeightTag>().Weight = 0;
            }
        }

        public static void PreventMultiChests()
        {
            foreach (AbstractPlacement pmt in ItemChanger.Internal.Ref.Settings.GetPlacements())
            {
                if (pmt.HasTag<RandoPlacementTag>())
                {
                    pmt.AddTag<ItemChanger.Tags.UnsupportedContainerTag>().containerType = Container.Chest;
                }
            }
        }
    }
}
