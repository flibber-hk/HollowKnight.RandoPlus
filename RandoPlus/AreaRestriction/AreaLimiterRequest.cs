﻿using System;
using System.Collections.Generic;
using System.Linq;
using ConnectionMetadataInjector;
using ItemChanger;
using RandomizerCore.Extensions;
using RandomizerMod.RandomizerData;
using RandomizerMod.RC;

namespace RandoPlus.AreaRestriction
{
    public static class AreaLimiterRequest
    {
        public static void Hook()
        {
            RequestBuilder.OnUpdate.Subscribe(150f, ApplyAreaLimit);
        }


        private static void ApplyAreaLimit(RequestBuilder rb)
        {
            if (!RandoPlus.GS.AreaBlitz) return;

            AreaRestriction.PlacedAreas.Clear();
            AreaRestriction.ExcludedAreas.Clear();

            // Select areas
            HashSet<string> DistinctAreas = new();
            foreach (string loc in rb.EnumerateItemGroups().SelectMany(x => x.Locations.EnumerateDistinct()))
            {
                if (rb.TryGetLocationDef(loc, out LocationDef def))
                {
                    DistinctAreas.Add(def.MapArea);
                }
            }

            List<string> AllAreas = DistinctAreas.ToList();

            if (rb.gs.LongLocationSettings.WhitePalaceRando == RandomizerMod.Settings.LongLocationSettings.WPSetting.ExcludeWhitePalace)
            {
                AllAreas.Remove("White Palace");
                AreaRestriction.ExcludedAreas.Add("White Palace");
            }

            void PlaceArea(string Area)
            {
                if (AllAreas.Remove(Area))
                {
                    AreaRestriction.PlacedAreas.Add(Area);
                }
            }
            
            PlaceArea("Dirtmouth");

            string startScene = Data.GetStartDef(rb.gs.StartLocationSettings.StartLocation).SceneName;
            PlaceArea(Data.GetRoomDef(startScene).MapArea);

            while (AreaRestriction.PlacedAreas.Count < AreaRestriction.AreaCount)
            {
                PlaceArea(rb.rng.Next(AllAreas));
            }

            AreaRestriction.ExcludedAreas.AddRange(AllAreas);


            AreaRestriction.InvalidLocations.Clear();
            foreach (ItemGroupBuilder igb in rb.EnumerateItemGroups())
            {
                foreach (string loc in igb.Locations.EnumerateDistinct())
                {
                    if (!IsPlaceable(loc, rb))
                    {
                        AreaRestriction.InvalidLocations.Add(loc);
                    }
                }

                foreach (string loc in AreaRestriction.InvalidLocations)
                {
                    igb.Locations.RemoveAll(loc);
                }

                igb.LocationPadder = GetPadder(rb.rng, igb);
            }
        }

        private static readonly MetadataProperty<AbstractLocation, IEnumerable<string>> MapAreas = new(nameof(MapAreas), _ => Enumerable.Empty<string>());

        private static bool IsPlaceable(string loc, RequestBuilder rb)
        {
            if (rb.TryGetLocationDef(loc, out LocationDef def) && !string.IsNullOrEmpty(def.MapArea))
            {
                return AreaRestriction.PlacedAreas.Contains(def.MapArea);
            }

            AbstractLocation icLoc = Finder.GetLocation(loc);
            if (icLoc != null)
            {
                IEnumerable<string> mapAreas = SupplementalMetadata.Of(icLoc).Get(MapAreas);
                return mapAreas.Any(area => AreaRestriction.PlacedAreas.Contains(area));
            }

            return false;
        }

        public static ItemGroupBuilder.LocationPaddingHandler GetPadder(Random rng,
            ItemGroupBuilder gb, string defaultMultiLocation = LocationNames.Sly)
        {
            IEnumerable<RandoModLocation> PadLocations(RandoFactory factory, int count)
            {
                List<string> locs = gb.Locations.EnumerateWithMultiplicity().ToList();

                if (locs.Count == 0)
                {
                    for (int i = 0; i < count;i++)
                    {
                        yield return factory.MakeLocation(defaultMultiLocation);
                    }
                    yield break;
                }

                rng.PermuteInPlace(locs);

                for (int i = 0; i < count; i++)
                {
                    yield return factory.MakeLocation(locs[i % locs.Count]);
                }
            }

            return PadLocations;
        }
    }
}
