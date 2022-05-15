using System;
using System.Collections.Generic;
using System.Linq;
using ItemChanger;
using RandomizerCore;
using RandomizerCore.Randomization;
using RandomizerMod.RandomizerData;
using RandomizerMod.RC;
namespace RandoPlus.NailUpgrades
{
    public static class RequestMaker
    {
        public static void Hook()
        {
            RequestBuilder.OnUpdate.Subscribe(50, AddNailUpgrades);
            RequestBuilder.OnUpdate.Subscribe(-499.2f, SetupRefs);
            RequestBuilder.OnUpdate.Subscribe(101, DerangeNailUpgrades);
        }

        private static void SetupRefs(RequestBuilder rb)
        {
            if (!RandoPlus.GS.Any) return;
            {
                rb.EditItemRequest(Consts.NailUpgrade, info =>
                {
                    info.getItemDef = () => new ItemDef()
                    {
                        Name = Consts.NailUpgrade,
                        Pool = Consts.NailUpgradePoolGroup,
                        MajorItem = false,
                        PriceCap = 500,
                    };
                });

                for (int i = 1; i < 5; i++)
                {
                    // copy the iteration variable so the original isn't captured
                    int copyi = i;

                    rb.EditLocationRequest(Consts.NailsmithLocationPrefix + i, info =>
                    {
                        info.getLocationDef = () => new()
                        {
                            Name = Consts.NailsmithLocationPrefix + i,
                            SceneName = SceneNames.Room_nailsmith,
                            FlexibleCount = false,
                            AdditionalProgressionPenalty = false,
                        };

                        info.onRandoLocationCreation += (factory, rl) =>
                        {
                            rl.AddCost(new LogicGeoCost(factory.lm, 250 * copyi));
                        };
                    });
                }

                rb.OnGetGroupFor.Subscribe(0f, MatchNailUpgradeGroup);

                static bool MatchNailUpgradeGroup(RequestBuilder rb, string item, RequestBuilder.ElementType type, out GroupBuilder gb)
                {
                    if (item == Consts.NailUpgrade && (type == RequestBuilder.ElementType.Unknown || type == RequestBuilder.ElementType.Item))
                    {
                        gb = rb.GetGroupFor(ItemNames.Vengeful_Spirit);
                        return true;
                    }
                    else if (item.StartsWith(Consts.NailsmithLocationPrefix) && (type == RequestBuilder.ElementType.Unknown || type == RequestBuilder.ElementType.Location))
                    {
                        gb = rb.GetGroupFor(ItemNames.Vengeful_Spirit);
                        return true;
                    }

                    gb = default;
                    return false;
                }

            }
        }

        private static void AddNailUpgrades(RequestBuilder rb)
        {
            if (RandoPlus.GS.NailUpgrades)
            {
                rb.AddItemByName(Consts.NailUpgrade, 4);

                for (int i = 1; i < 5; i++)
                {
                    rb.AddLocationByName(Consts.NailsmithLocationPrefix + i);
                }
            }

            else if (RandoPlus.GS.DefineRefs)
            {
                for (int i = 1; i < 5; i++)
                {
                    rb.AddToVanilla(Consts.NailUpgrade, Consts.NailsmithLocationPrefix + i);
                }
            }
        }

        private static void DerangeNailUpgrades(RequestBuilder rb)
        {
            if (!rb.gs.CursedSettings.Deranged) return;
            if (!RandoPlus.GS.Any) return;

            static bool NotVanillaMushroom(IRandoItem item, IRandoLocation location)
            {
                if (item.Name == Consts.NailUpgrade && location.Name.StartsWith(Consts.NailsmithLocationPrefix))
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
    }
}
