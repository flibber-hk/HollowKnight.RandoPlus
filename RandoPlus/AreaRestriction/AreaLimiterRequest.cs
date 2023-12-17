using System;
using System.Collections.Generic;
using System.Linq;
using ConnectionMetadataInjector;
using ItemChanger;
using RandomizerCore.Extensions;
using RandomizerMod.Menu;
using RandomizerMod.RandomizerData;
using RandomizerMod.RC;
using StartDef = RandomizerMod.RandomizerData.StartDef;

namespace RandoPlus.AreaRestriction
{
    public static class AreaLimiterRequest
    {
        public static void Hook()
        {
            RequestBuilder.OnUpdate.Subscribe(99.9f, ApplyPadders);
            RequestBuilder.OnUpdate.Subscribe(150f, ApplyAreaLimit);
        }

        private static void ApplyPadders(RequestBuilder rb)
        {
            if (!RandoPlus.GS.AreaBlitz && !RandoPlus.GS.FullFlexibleCount)
            {
                return;
            }

            // Apply padders just before rando applies them
            foreach (ItemGroupBuilder igb in rb.EnumerateItemGroups())
            {
                igb.LocationPadder ??= GetPadder(rb.rng, igb);
            }
        }

        private static void ApplyAreaLimit(RequestBuilder rb)
        {
            if (!RandoPlus.GS.AreaBlitz) return;

            AreaRestriction.PlacedAreas.Clear();
            AreaRestriction.ExcludedAreas.Clear();

            // Select areas
            // Note - the candidate areas include any map area with at least one randomized location whose only
            // map area is the given. It is intentional that map areas, all of whose locations are also found
            // in another map area, are excluded.
            HashSet<string> DistinctAreas = new();
            foreach (string loc in rb.EnumerateItemGroups().SelectMany(x => x.Locations.EnumerateDistinct()))
            {
                if (rb.TryGetLocationDef(loc, out LocationDef def))
                {
                    string mapArea = def.MapArea;
                    if (!string.IsNullOrWhiteSpace(mapArea))
                    {
                        DistinctAreas.Add(mapArea);
                    }
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

            string startLocation = rb.gs.StartLocationSettings.StartLocation;
            string[] startLocationNames;
            if (!startLocation.Contains("|"))
            {
                startLocationNames = new[] { startLocation };
            }
            else
            {
                string[] startsPlusEnds = startLocation.Split('|');
                startLocationNames = new string[startsPlusEnds.Length - 2];
                for (int i = 0; i < startLocationNames.Length; i++)
                {
                    startLocationNames[i] = startsPlusEnds[i + 1];
                }
            }

            Dictionary<string, StartDef> startDict = RandomizerMenuAPI.GenerateStartLocationDict();
            foreach (string startLocationName in startLocationNames)
            {
                string startScene = startDict[startLocationName].SceneName;
                PlaceArea(Data.GetRoomDef(startScene).MapArea);
            }

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
            }
        }

        private static readonly MetadataProperty<AbstractLocation, IEnumerable<string>> MapAreasProperty = new("MapAreas", _ => Enumerable.Empty<string>());

        private static bool IsPlaceable(string loc, RequestBuilder rb)
        {
            if (rb.TryGetLocationDef(loc, out LocationDef def) && !string.IsNullOrWhiteSpace(def.MapArea))
            {
                return AreaRestriction.PlacedAreas.Contains(def.MapArea);
            }

            AbstractLocation icLoc = Finder.GetLocation(loc);
            if (icLoc != null)
            {
                IEnumerable<string> mapAreas = SupplementalMetadata.Of(icLoc).Get(MapAreasProperty);
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
