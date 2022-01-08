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
        public static void Hook()
        {
            RequestBuilder.OnUpdate.Subscribe(50f, CreateRemover(ItemNames.Lumafly_Lantern, Consts.NoLantern, 
                "Dark room skips required for no Lantern", nameof(RandomizerMod.Settings.SkipSettings.DarkRooms), 
                () => RandoPlus.GS.NoLantern));
            RequestBuilder.OnUpdate.Subscribe(50f, CreateRemover(ItemNames.Ismas_Tear, Consts.NoTear,
                "Acid skips required for no Tear", nameof(RandomizerMod.Settings.SkipSettings.AcidSkips),
                () => RandoPlus.GS.NoTear));
            RequestBuilder.OnUpdate.Subscribe(50f, CreateRemover(ItemNames.Swim, Consts.NoSwim,
                "Acid skips required for no Swim", nameof(RandomizerMod.Settings.SkipSettings.AcidSkips),
                () => RandoPlus.GS.NoSwim));
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
                        gb = rb.GetGroupFor(item, type);
                        return true;
                    }
                    gb = default;
                    return false;
                }
            }

            return RemoveItem;
        }
    }
}
