using System;
using System.Collections.Generic;
using System.Linq;
using ItemChanger;
using RandomizerMod.RandomizerData;
using RandomizerMod.RC;
using RandomizerMod.Settings;

namespace RandoPlus.RemoveUsefulItems
{
    public static class RequestModifier
    {
        public record struct RemoverInfo
        (
            string OldItem,
            string VanillaLocation,
            string NewItem,
            Func<bool> IsActive
        );

        public static readonly RemoverInfo[] settings = new RemoverInfo[]
        {
            new(ItemNames.Lumafly_Lantern, LocationNames.Sly, Consts.NoLantern, () => RandoPlus.GS.NoLantern),
            new(ItemNames.Ismas_Tear, LocationNames.Ismas_Tear, Consts.NoTear, () => RandoPlus.GS.NoTear),
            new(ItemNames.Swim, null, Consts.NoSwim, () => RandoPlus.GS.NoSwim)
        };

        public static void Hook()
        {
            RequestBuilder.OnUpdate.Subscribe(-499f, SetupRefs);

            foreach (RemoverInfo info in settings)
            {
                RequestBuilder.OnUpdate.Subscribe(50f, CreateRemover(info));
            }
        }

        private static void SetupRefs(RequestBuilder rb)
        {
            if (!RandoPlus.GS.AnyUsefulItemsRemoved) return;

            foreach (RemoverInfo ri in settings)
            {
                rb.EditItemRequest(ri.NewItem, info =>
                {
                    info.getItemDef = () => new ItemDef()
                    {
                        Name = ri.NewItem,
                        Pool = Consts.RemoveUsefulItems,
                        MajorItem = false,
                        PriceCap = 500
                    };
                });

                rb.OnGetGroupFor.Subscribe(-999f, MatchGroup);

                bool MatchGroup(RequestBuilder rb, string item, RequestBuilder.ElementType type, out GroupBuilder gb)
                {
                    if (item == ri.NewItem && (type == RequestBuilder.ElementType.Item || type == RequestBuilder.ElementType.Unknown))
                    {
                        gb = rb.GetGroupFor(ri.OldItem, type);
                        return true;
                    }
                    gb = default;
                    return false;
                }
            }
        }

        private static RequestBuilder.RequestBuilderUpdateHandler CreateRemover(RemoverInfo info)
        {
            void RemoveItem(RequestBuilder rb)
            {
                if (!info.IsActive()) return;

                rb.ReplaceItem(info.OldItem, info.NewItem);
                rb.ReplaceItem(PlaceholderItem.Prefix + info.OldItem, PlaceholderItem.Prefix + info.NewItem);
                rb.StartItems.Replace(info.OldItem, info.NewItem);

                if (!string.IsNullOrEmpty(info.VanillaLocation))
                {
                    rb.RemoveFromVanilla(info.VanillaLocation, info.OldItem);
                }
            }

            return RemoveItem;
        }
    }
}
