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
            // Add Items and Locations to the pool
            RequestBuilder.OnUpdate.Subscribe(50, AddMrMushroom);
            // Register the Deranged constraint
            RequestBuilder.OnUpdate.Subscribe(101, DerangeMrMushroom);
            // Set up OnGetGroupFor matcher and define infos for item and locations
            RequestBuilder.OnUpdate.Subscribe(-499, SetupRefs);
            // Option to dupe Spore Shroom
            RequestBuilder.OnUpdate.Subscribe(20.1f, DupeSporeShroom);
        }

        private static void DupeSporeShroom(RequestBuilder rb)
        {
            if (!RandoPlus.GS.DupeSporeShroom) return;

            if (!rb.IsAtStart(ItemNames.Spore_Shroom))
            {
                rb.AddItemByName($"{PlaceholderItem.Prefix}{ItemNames.Spore_Shroom}");
            }
        }

        private static void DerangeMrMushroom(RequestBuilder rb)
        {
            if (!rb.gs.CursedSettings.Deranged) return;
            if (!RandoPlus.GS.Any) return;

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

        private static void SetupRefs(RequestBuilder rb)
        {
            if (!RandoPlus.GS.Any) return;

            rb.EditItemRequest(Consts.MrMushroomLevelUp, info =>
            {
                info.getItemDef = () => new()
                {
                    Name = Consts.MrMushroomLevelUp,
                    Pool = Consts.MrMushroomPoolGroup,
                    MajorItem = false,
                    PriceCap = 500
                };
            });
            foreach (string loc in mushrooms)
            {
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

            rb.OnGetGroupFor.Subscribe(0f, MatchMushroomGroup);

            static bool MatchMushroomGroup(RequestBuilder rb, string item, RequestBuilder.ElementType type, out GroupBuilder gb)
            {
                if (item == Consts.MrMushroomLevelUp && (type == RequestBuilder.ElementType.Unknown || type == RequestBuilder.ElementType.Item))
                {
                    gb = rb.GetGroupFor(ItemNames.Lore_Tablet_Kingdoms_Edge);
                    return true;
                }
                else if (mushrooms.Contains(item) && (type == RequestBuilder.ElementType.Unknown || type == RequestBuilder.ElementType.Location))
                {
                    gb = rb.GetGroupFor(ItemNames.Lore_Tablet_Kingdoms_Edge);
                    return true;
                }
                gb = default;
                return false;
            }
        }

        private static void AddMrMushroom(RequestBuilder rb)
        {
            if (RandoPlus.GS.MrMushroom)
            {
                rb.AddItemByName(Consts.MrMushroomLevelUp, 7);
                foreach (string loc in mushrooms)
                {
                    rb.AddLocationByName(loc);
                }
            }
            else if (RandoPlus.GS.DefineRefs)
            {
                foreach (string loc in mushrooms)
                {
                    rb.AddToVanilla(Consts.MrMushroomLevelUp, loc);
                }
            }
        }
    }
}
