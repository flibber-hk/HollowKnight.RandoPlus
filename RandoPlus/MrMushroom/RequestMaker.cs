using System;
using System.Collections.Generic;
using System.Linq;
using ItemChanger;
using RandomizerCore;
using RandomizerCore.Randomization;
using RandomizerMod.RandomizerData;
using RandomizerMod.RC;

namespace RandoPlus.MrMushroom
{
    public static class RequestMaker
    {
        static readonly HashSet<string> mushrooms = new()
        {
            Consts.MrMushroomFungalWastes,
            Consts.MrMushroomKingdomsEdge,
            Consts.MrMushroomDeepnest,
            Consts.MrMushroomHowlingCliffs,
            Consts.MrMushroomAncientBasin,
            Consts.MrMushroomFogCanyon,
            Consts.MrMushroomKingsPass,
        };

        public static void Hook()
        {
            RequestBuilder.OnUpdate.Subscribe(50, AddMrMushroom);
            RequestBuilder.OnUpdate.Subscribe(101, DerangeMrMushroom);
            RequestBuilder.OnUpdate.Subscribe(-499, MatchMrMushroom);
        }

        private static void DerangeMrMushroom(RequestBuilder rb)
        {
            if (!rb.gs.CursedSettings.Deranged) return;
            if (!RandoPlus.GS.MrMushroom) return;

            static bool NotVanillaMushroom(IRandoItem item, IRandoLocation location)
            {
                if (item.Name == Consts.MrMushroomLevelUp && mushrooms.Contains(location.Name))
                {
                    return false;
                }
                return true;
            }

            foreach (ItemGroupBuilder gb in rb.EnumerateItemGroups())
            {
                if (gb.strategy is DefaultGroupPlacementStrategy dgps)
                {
                    dgps.Constraints += NotVanillaMushroom;
                }
            }
        }

        private static void MatchMrMushroom(RequestBuilder rb)
        {
            if (!RandoPlus.GS.MrMushroom) return;

            rb.OnGetGroupFor.Subscribe(-100f, MatchMushroomGroup);

            bool MatchMushroomGroup(RequestBuilder rb, string item, RequestBuilder.ElementType type, out GroupBuilder gb)
            {
                if (item == Consts.MrMushroomLevelUp && (type == RequestBuilder.ElementType.Unknown || type == RequestBuilder.ElementType.Item))
                {
                    gb = rb.GetGroupFor(ItemNames.Lore_Tablet_City_Entrance);
                    return true;
                }
                else if (mushrooms.Contains(item) && (type == RequestBuilder.ElementType.Unknown || type == RequestBuilder.ElementType.Location))
                {
                    gb = rb.GetGroupFor(ItemNames.Lore_Tablet_City_Entrance);
                    return true;
                }
                gb = default;
                return false;
            }
        }

        private static void AddMrMushroom(RequestBuilder rb)
        {
            if (!RandoPlus.GS.MrMushroom) return;

            rb.AddItemByName(Consts.MrMushroomLevelUp, 7);
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

            foreach (string loc in mushrooms)
            {
                rb.AddLocationByName(loc);
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
        }
    }
}
