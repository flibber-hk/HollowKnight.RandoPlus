using System;
using System.Collections.Generic;
using System.Linq;
using ItemChanger;
using RandomizerMod.RandomizerData;
using RandomizerMod.RC;

namespace RandoPlus.RemoveUsefulItems
{
    public static class RequestModifier
    {
        public static readonly (string oldItem, string vanillaLocation, string newItem, Func<bool> isActive)[] settings = new (string oldItem, string vanillaLocation, string newItem, Func<bool> isActive)[]
        {
            (ItemNames.Lumafly_Lantern, LocationNames.Sly, Consts.NoLantern,() => RandoPlus.GS.NoLantern),
            (ItemNames.Ismas_Tear, LocationNames.Ismas_Tear, Consts.NoTear, () => RandoPlus.GS.NoTear),
            (ItemNames.Swim, null, Consts.NoSwim, () => RandoPlus.GS.NoSwim)
        };

        public static void Hook()
        {
            RequestBuilder.OnUpdate.Subscribe(-499f, SetupRefs);

            foreach ((string oldItem, string vanillaLocation, string newItem, Func<bool> isActive) in settings)
            {
                RequestBuilder.OnUpdate.Subscribe(50f, CreateRemover(oldItem, vanillaLocation, newItem, isActive));
            }
        }

        private static void SetupRefs(RequestBuilder rb)
        {
            if (!RandoPlus.GS.Any) return;

            foreach ((string oldItem, string vanillaLocation, string newItem, Func<bool> isActive) in settings)
            {
                rb.EditItemRequest(newItem, info =>
                {
                    info.getItemDef = () => new ItemDef()
                    {
                        Name = newItem,
                        Pool = Consts.RemoveUsefulItems,
                        MajorItem = false,
                        PriceCap = 500
                    };
                });

                rb.OnGetGroupFor.Subscribe(-999f, MatchGroup);

                bool MatchGroup(RequestBuilder rb, string item, RequestBuilder.ElementType type, out GroupBuilder gb)
                {
                    if (item == newItem && (type == RequestBuilder.ElementType.Item || type == RequestBuilder.ElementType.Unknown))
                    {
                        gb = rb.GetGroupFor(oldItem, type);
                        return true;
                    }
                    gb = default;
                    return false;
                }
            }
        }

        private static RequestBuilder.RequestBuilderUpdateHandler CreateRemover(
            string oldItem, 
            string vanillaLocation,
            string newItem, 
            Func<bool> isRandomized)
        {
            void RemoveItem(RequestBuilder rb)
            {
                if (!isRandomized()) return;

                rb.ReplaceItem(oldItem, newItem);
                rb.ReplaceItem(PlaceholderItem.Prefix + oldItem, PlaceholderItem.Prefix + newItem);
                rb.StartItems.Replace(oldItem, newItem);

                if (!string.IsNullOrEmpty(vanillaLocation))
                {
                    rb.RemoveFromVanilla(vanillaLocation, oldItem);
                }
            }

            return RemoveItem;
        }
    }
}
