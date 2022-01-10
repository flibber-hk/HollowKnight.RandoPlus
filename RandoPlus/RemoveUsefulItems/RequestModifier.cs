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
        public static readonly (string oldItem, string newItem, string errorMessage, string skipType, Func<bool> isActive)[] settings = new (string oldItem, string newItem, string errorMessage, string skipType, Func<bool> isActive)[]
        {
            (ItemNames.Lumafly_Lantern, Consts.NoLantern,
                "Dark room skips required for no Lantern", nameof(RandomizerMod.Settings.SkipSettings.DarkRooms),
                () => RandoPlus.GS.NoLantern),
            (ItemNames.Ismas_Tear, Consts.NoTear,
                "Acid skips required for no Tear", nameof(RandomizerMod.Settings.SkipSettings.AcidSkips),
                () => RandoPlus.GS.NoTear),
            (ItemNames.Swim, Consts.NoSwim,
                "Acid skips required for no Swim", nameof(RandomizerMod.Settings.SkipSettings.AcidSkips),
                () => RandoPlus.GS.NoSwim)
        };

        public static void Hook()
        {
            RequestBuilder.OnUpdate.Subscribe(-499f, SetupRefs);

            foreach (var tuple in settings)
            {
                RequestBuilder.OnUpdate.Subscribe(50f, CreateRemover(tuple.oldItem, tuple.newItem, tuple.errorMessage, tuple.skipType, tuple.isActive));
            }
        }

        private static void SetupRefs(RequestBuilder rb)
        {
            if (!RandoPlus.GS.Any) return;

            foreach (var tuple in settings)
            {
                rb.EditItemRequest(tuple.newItem, info =>
                {
                    info.getItemDef = () => new ItemDef()
                    {
                        Name = tuple.newItem,
                        Pool = Consts.RemoveUsefulItems,
                        MajorItem = false,
                        PriceCap = 500
                    };
                });

                rb.OnGetGroupFor.Subscribe(-999f, MatchGroup);

                bool MatchGroup(RequestBuilder rb, string item, RequestBuilder.ElementType type, out GroupBuilder gb)
                {
                    if (item == tuple.newItem && (type == RequestBuilder.ElementType.Item || type == RequestBuilder.ElementType.Unknown))
                    {
                        gb = rb.GetGroupFor(tuple.oldItem, type);
                        return true;
                    }
                    gb = default;
                    return false;
                }
            }
        }

        private static RequestBuilder.RequestBuilderUpdateHandler CreateRemover(string oldItem, string newItem, string errorMessage,
            string skipSetting, Func<bool> isRandomized)
        {
            void RemoveItem(RequestBuilder rb)
            {
                if (!isRandomized()) return;
                if (!rb.gs.SkipSettings.GetFieldByName(skipSetting))
                {
                    RandoPlus.instance.LogError(errorMessage);
                    return;
                }
                rb.ReplaceItem(oldItem, newItem);
                rb.ReplaceItem(PlaceholderItem.Prefix + oldItem, PlaceholderItem.Prefix + newItem);
                rb.StartItems.Replace(oldItem, newItem);

                List<VanillaRequest> vanilla = rb.Vanilla.EnumerateWithMultiplicity().Where(x => x.Item == oldItem).ToList();
                foreach (VanillaRequest req in vanilla)
                {
                    rb.Vanilla.RemoveAll(req);
                    rb.Vanilla.Add(new(newItem, req.Location));
                }
            }

            return RemoveItem;
        }
    }
}
