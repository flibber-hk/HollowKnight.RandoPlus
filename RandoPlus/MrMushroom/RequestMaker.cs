using System;
using System.Collections.Generic;
using System.Linq;
using ItemChanger;
using RandomizerMod.RandomizerData;
using RandomizerMod.RC;

namespace RandoPlus.MrMushroom
{
    public static class RequestMaker
    {
        public static void Hook()
        {
            RequestBuilder.OnUpdate.Subscribe(50, AddMrMushroom);
        }

        private static void AddMrMushroom(RequestBuilder rb)
        {
            if (!RandoPlus.GS.MrMushroom) return;

            ItemGroupBuilder mushGroup = rb.GetItemGroupFor(ItemNames.Lore_Tablet_City_Entrance);

            for (int i = 0; i < 7; i++)
            {
                mushGroup.Items.Add(Consts.MrMushroomLevelUp);
            }
            rb.EditItemRequest(Consts.MrMushroomLevelUp, info =>
            {
                info.getItemDef = () => new()
                {
                    Name = Consts.MrMushroomLevelUp,
                    Pool = Consts.MushPool,
                    MajorItem = false,
                    PriceCap = 500
                };
            });

            HashSet<string> mushrooms = new()
            {
                Consts.MrMushroomFungalWastes,
                Consts.MrMushroomKingdomsEdge,
                Consts.MrMushroomDeepnest,
                Consts.MrMushroomHowlingCliffs,
                Consts.MrMushroomAncientBasin,
                Consts.MrMushroomFogCanyon,
                Consts.MrMushroomKingsPass,
            };

            foreach (string loc in mushrooms)
            {
                mushGroup.Locations.Add(loc);
                rb.EditLocationRequest(loc, info =>
                {
                    info.getLocationDef = () => new()
                    {
                        Name = loc,
                        SceneName = Finder.GetLocation(loc).sceneName,
                        FlexibleCount = false,
                        AdditionalProgressionPenalty = false,
                    };
                });
            }

            rb.OnGetGroupFor.Subscribe(-100f, MatchMushroomGroup);

            bool MatchMushroomGroup(RequestBuilder rb, string item, RequestBuilder.ElementType type, out GroupBuilder gb)
            {
                if (item == Consts.MrMushroomLevelUp && (type == RequestBuilder.ElementType.Unknown || type == RequestBuilder.ElementType.Item))
                {
                    gb = mushGroup;
                    return true;
                }
                else if (mushrooms.Contains(item) && (type == RequestBuilder.ElementType.Unknown || type == RequestBuilder.ElementType.Location))
                {
                    gb = mushGroup;
                    return true;
                }
                gb = default;
                return false;
            }
        }
    }
}
