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
            int invalidLocationCount = 0;
            List<string> ValidLocations = new();
            foreach (ItemGroupBuilder igb in rb.EnumerateItemGroups())
            {
                foreach (string loc in igb.Locations.EnumerateWithMultiplicity())
                {
                    if (AreaRestriction.PlacedAreas.Contains(Data.GetLocationDef(loc).MapArea))
                    {
                        ValidLocations.Add(loc);
                    }
                    else
                    {
                        AreaRestriction.InvalidLocations.Add(loc);
                        invalidLocationCount++;
                    }
                }

                foreach (string loc in AreaRestriction.InvalidLocations)
                {
                    igb.Locations.RemoveAll(loc);
                }

                if (ValidLocations.Count == 0)
                {
                    for (int i = 0; i < invalidLocationCount; i++)
                    {
                        igb.Locations.Add(LocationNames.Sly);
                    }
                }
                else
                {
                    rb.rng.PermuteInPlace(ValidLocations);
                    for (int i = 0; i < invalidLocationCount; i++)
                    {
                        igb.Locations.Add(ValidLocations[i % ValidLocations.Count]);
                    }
                }
            }
        }
    }
}
