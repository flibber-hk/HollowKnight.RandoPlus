using System;
using System.Collections.Generic;
using System.Linq;
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
            if (!RandoPlus.GS.DeleteAreas) return;

            AreaRestriction.PlacedAreas.Clear();
            AreaRestriction.ExcludedAreas.Clear();

            // Select areas
            List<string> AllAreas = new HashSet<string>(Data.GetMapAreaTransitionNames().Select(x => Data.GetTransitionDef(x).MapArea)).ToList();
            
            if (rb.gs.LongLocationSettings.RandomizationInWhitePalace == RandomizerMod.Settings.LongLocationSettings.WPSetting.ExcludeWhitePalace)
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


            // Squish locations
            AreaRestriction.InvalidLocations.Clear();
            int invalidLocationCountFromGroup;
            List<string> ValidLocationsInGroup = new();
            foreach (ItemGroupBuilder igb in rb.EnumerateItemGroups())
            {
                invalidLocationCountFromGroup = 0;
                ValidLocationsInGroup.Clear();

                foreach (string loc in igb.Locations.EnumerateWithMultiplicity())
                {
                    if (rb.TryGetLocationDef(loc, out LocationDef def) && AreaRestriction.PlacedAreas.Contains(def.MapArea))
                    {
                        ValidLocationsInGroup.Add(loc);
                    }
                    else
                    {
                        // Location excluded if no location def defined
                        AreaRestriction.InvalidLocations.Add(loc);
                        invalidLocationCountFromGroup++;
                    }
                }

                foreach (string loc in AreaRestriction.InvalidLocations)
                {
                    igb.Locations.RemoveAll(loc);
                }

                if (ValidLocationsInGroup.Count == 0)
                {
                    for (int i = 0; i < invalidLocationCountFromGroup; i++)
                    {
                        igb.Locations.Add(LocationNames.Sly);
                    }
                }
                else
                {
                    rb.rng.PermuteInPlace(ValidLocationsInGroup);
                    for (int i = 0; i < invalidLocationCountFromGroup; i++)
                    {
                        igb.Locations.Add(ValidLocationsInGroup[i % ValidLocationsInGroup.Count]);
                    }
                }
            }
        }
    }
}
