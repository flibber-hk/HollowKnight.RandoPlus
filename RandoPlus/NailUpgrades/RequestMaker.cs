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

        }

        private static void SetupRefs(RequestBuilder rb)
        {
            if (!RandoPlus.GS.Any) return;
            {
                rb.EditItemRequest(Consts.Nail_Upgrade, info =>
                {
                    info.getItemDef = () => new ItemDef()
                    {
                        Name = Consts.Nail_Upgrade,
                        Pool = "NailUpgrades",
                        MajorItem = false, // true if progressive
                        PriceCap = 500,
                        
                    };
                });

                rb.EditLocationRequest(Consts.NailPlace, info =>
                {
                    info.getLocationDef = () => new()
                    {
                        Name = Consts.NailPlace,
                        SceneName = Finder.GetLocation(Consts.NailPlace).sceneName,
                        FlexibleCount = false,
                        AdditionalProgressionPenalty = false,
                       
                    };
                });
            }
        }

            private static void AddNailUpgrades(RequestBuilder rb)
            {
            if (RandoPlus.GS.NailUpgrades)
            {
                rb.AddItemByName(Consts.Nail_Upgrade, 4);

                for (int counter = 1; counter < 5; counter++)
                {
                    rb.AddLocationByName(Consts.NailPlace+counter.ToString());
                }

            }
            else if (RandoPlus.GS.DefineRefs)
            {
               rb.AddToVanilla(Consts.Nail_Upgrade, Consts.NailPlace);

            }
        }

    }
}
