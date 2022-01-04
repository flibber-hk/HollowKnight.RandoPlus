using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemChanger;
using RandomizerMod.RC;

namespace RandoPlus.RemoveUsefulItems
{
    public static class RequestModifier
    {
        public static void Hook()
        {
            RequestBuilder.OnUpdate.Subscribe(50f, RemoveLantern);
            RequestBuilder.OnUpdate.Subscribe(50f, RemoveTear);
            RequestBuilder.OnUpdate.Subscribe(50f, RemoveSwim);
        }

        private static void RemoveLantern(RequestBuilder rb)
        {
            if (!RandoPlus.GS.NoLantern) return;
            if (!rb.gs.SkipSettings.DarkRooms)
            {
                RandoPlus.instance.LogError("Dark room skips required!");
                return;
            }
            rb.ReplaceItem(ItemNames.Lumafly_Lantern, Consts.NoLantern);
            rb.ReplaceItem(PlaceholderItem.Prefix + ItemNames.Lumafly_Lantern, PlaceholderItem.Prefix + Consts.NoLantern);
            rb.StartItems.Replace(ItemNames.Lumafly_Lantern, Consts.NoLantern);

            List<VanillaRequest> vanillaLantern = rb.Vanilla.EnumerateWithMultiplicity().Where(x => x.Item == ItemNames.Lumafly_Lantern).ToList();
            foreach (VanillaRequest req in vanillaLantern)
            {
                rb.Vanilla.RemoveAll(req);
                rb.Vanilla.Add(new(Consts.NoLantern, req.Location));
            }
        }

        private static void RemoveTear(RequestBuilder rb)
        {
            if (!RandoPlus.GS.NoTear) return;
            if (!rb.gs.SkipSettings.AcidSkips)
            {
                RandoPlus.instance.LogError("Acid skips required!");
                return;
            }
            rb.ReplaceItem(ItemNames.Ismas_Tear, Consts.NoTear);
            rb.ReplaceItem(PlaceholderItem.Prefix + ItemNames.Ismas_Tear, PlaceholderItem.Prefix + Consts.NoTear);
            rb.StartItems.Replace(ItemNames.Ismas_Tear, Consts.NoTear);

            List<VanillaRequest> vanillaTear = rb.Vanilla.EnumerateWithMultiplicity().Where(x => x.Item == ItemNames.Ismas_Tear).ToList();
            foreach (VanillaRequest req in vanillaTear)
            {
                rb.Vanilla.RemoveAll(req);
                rb.Vanilla.Add(new(Consts.NoTear, req.Location));
            }
        }

        private static void RemoveSwim(RequestBuilder rb)
        {
            if (!RandoPlus.GS.NoSwim) return;
            if (!rb.gs.SkipSettings.AcidSkips)
            {
                RandoPlus.instance.LogError("Acid skips required!");
                return;
            }
            rb.ReplaceItem(ItemNames.Swim, Consts.NoSwim);
            rb.ReplaceItem(PlaceholderItem.Prefix + ItemNames.Swim, PlaceholderItem.Prefix + Consts.NoSwim);
            rb.StartItems.Replace(ItemNames.Swim, Consts.NoSwim);

            List<VanillaRequest> vanillaSwim = rb.Vanilla.EnumerateWithMultiplicity().Where(x => x.Item == ItemNames.Swim).ToList();
            foreach (VanillaRequest req in vanillaSwim)
            {
                rb.Vanilla.RemoveAll(req);
                rb.Vanilla.Add(new(Consts.NoSwim, req.Location));
            }
        }
    }
}
