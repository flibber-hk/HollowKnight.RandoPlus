using System;
using System.Collections.Generic;
using System.Linq;
using ItemChanger;
using RandomizerCore;
using RandomizerCore.Randomization;
using RandomizerMod.RandomizerData;
using RandomizerMod.RC;

namespace RandoPlus.GhostEssence
{
    public static class RequestMaker
    {

        public static void Hook()
        {
            // Add Items and Locations to the pool
            RequestBuilder.OnUpdate.Subscribe(25, AddGhost);
            // Register the Deranged constraint
            RequestBuilder.OnUpdate.Subscribe(101, DerangeGhostEssence);
            // Set up OnGetGroupFor matcher and define infos for item and locations
            RequestBuilder.OnUpdate.Subscribe(-499, SetupRefs);
            // Option to dupe Spore Shroom
        }


        private static void DerangeGhostEssence(RequestBuilder rb)
        {
            if (!rb.gs.CursedSettings.Deranged) return;
            if (!RandoPlus.GS.Any) return;

            static bool NotVanillaGhost(IRandoItem item, IRandoLocation location)
            {
                if (GhostNames.ToArray().Contains(location.Name) && item.Name == location.Name)
                {
                    return false;
                }
                return true;
            }

            foreach (ItemGroupBuilder gb in rb.EnumerateItemGroups())
            {
                if (gb.strategy is DefaultGroupPlacementStrategy dgps)
                {
                    dgps.Constraints += NotVanillaGhost;
                }
            }
        }

        private static void SetupRefs(RequestBuilder rb)
        {
            if (!RandoPlus.GS.Any) return;

            foreach (string ghost in GhostNames.ToArray())
            {
                rb.EditItemRequest(ghost, info =>
                {
                    info.getItemDef = () => new()
                    {
                        Name = ghost,
                        Pool = Consts.GhostPoolGroup,
                        MajorItem = false,
                        PriceCap = 10
                    };
                });

                rb.EditLocationRequest(ghost, info =>
                {
                    info.getLocationDef = () => new()
                    {
                        Name = ghost,
                        SceneName = Finder.GetLocation(ghost).sceneName,
                        FlexibleCount = false,
                        AdditionalProgressionPenalty = false,
                    };
                });
            }

            rb.OnGetGroupFor.Subscribe(0f, MatchGhostGroup);

            static bool MatchGhostGroup(RequestBuilder rb, string item, RequestBuilder.ElementType type, out GroupBuilder gb)
            {
                if (item.StartsWith("Ghost_Essence") && (type == RequestBuilder.ElementType.Unknown || type == RequestBuilder.ElementType.Item))
                {
                    gb = rb.GetGroupFor(ItemNames.Boss_Essence_Xero);
                    return true;
                }
                gb = default;
                return false;
            }
        }

        private static void AddGhost(RequestBuilder rb)
        {
            if (RandoPlus.GS.GhostEssence)
            {
                foreach (string ghost in GhostNames.ToArray())
                {
                    rb.AddItemByName(ghost);
                    rb.AddLocationByName(ghost);
                }
            }
            else if (RandoPlus.GS.DefineRefs)
            {
                foreach (string ghost in GhostNames.ToArray())
                {
                    rb.AddToVanilla(ghost, ghost);
                }
            }
        }
    }
}
