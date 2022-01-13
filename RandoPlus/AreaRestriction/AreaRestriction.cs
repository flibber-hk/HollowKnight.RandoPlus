using System;
using System.Collections.Generic;
using ItemChanger;
using ItemChanger.Extensions;

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

        public static void HookStartNewGame() => ItemChanger.Events.BeforeStartNewGame += BeforeGameStart;

        private static void BeforeGameStart()
        {
            if (!RandoPlus.GS.AreaBlitz) return;

            // Not rando file
            if (RandomizerMod.RandomizerMod.RS.GenerationSettings is null) return;

            // TODO - add an in-game indicator for allowed areas?

            ItemChangerMod.Modules.GetOrAdd<AreaLimitModule>().PlacedAreas = new(PlacedAreas);

            if (RandoPlus.GS.PreferMultiShiny)
            {
                foreach (AbstractPlacement pmt in ItemChanger.Internal.Ref.Settings.GetPlacements())
                {
                    pmt.AddTag<ItemChanger.Tags.UnsupportedContainerTag>().containerType = Container.Chest;
                }
            }

            AbstractItem nothing = Finder.GetItem(ItemNames.Lumafly_Escape);
            nothing.AddTag<ItemChanger.Tags.CompletionWeightTag>().Weight = 0;

            // TODO - make placements using an ICFactory?
            foreach (string loc in InvalidLocations)
            {
                try
                {
                    AbstractPlacement pmt = Finder.GetLocation(loc).Wrap();
                    // Adding the tag to the item and the placement is redundant, but we can do it anyway
                    pmt.Add(nothing.Clone());
                    pmt.AddTag<ItemChanger.Tags.CompletionWeightTag>().Weight = 0;
                    ItemChangerMod.AddPlacements(pmt.Yield(), PlacementConflictResolution.Ignore);
                }
                catch
                {
                    RandoPlus.instance.LogWarn("Unable to find " + loc + " in Finder. Ignoring...");
                }
            }
        }
    }
}
