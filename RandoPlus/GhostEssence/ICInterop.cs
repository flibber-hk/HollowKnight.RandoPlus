using ItemChanger;
using ItemChanger.Items;
using ItemChanger.UIDefs;
using Newtonsoft.Json;
using RandoPlus.GhostEssence.Locations;
using System.Collections.Generic;
using System.IO;

namespace RandoPlus.GhostEssence
{
    public static class ICInterop
    {
        public class GhostInfo
        {
            public string Name { get; set; }
            public string SceneName { get; set; }
            public string ObjectName { get; set; }
        }

        public static List<GhostInfo> GhostInfos;

        private static AbstractLocation DefineGhostLocation(GhostInfo info)
        {
            GhostLocation ghostLocation = info.Name switch
            {
                GhostNames.Ghost_Essence_Cloth => new ClothGhostLocation(),
                GhostNames.Ghost_Essence_Joni => new JoniGhostLocation(),
                _ => new GhostLocation()
            };
            ghostLocation.name = info.Name;
            ghostLocation.objectName = info.ObjectName;
            ghostLocation.flingType = FlingType.DirectDeposit;
            ghostLocation.sceneName = info.SceneName;
            ghostLocation.Revek = info.Name == GhostNames.Ghost_Essence_Revek;
            SupplementalMetadataTagFactory.AddTagToLocation(ghostLocation, poolGroup: Consts.GhostPoolGroup, vanillaItem: ghostLocation.name);

            // TODO - Special case for Revek?

            return ghostLocation;
        }

        public static void DefineItemsAndLocations()
        {
            DefineItems();
            DefineLocations();
        }

        public static void DefineItems()
        {
            // TODO - use static method on EssenceItem to define this (when available)

            AbstractItem prefab = Finder.GetItem(ItemNames.Boss_Essence_Xero);
            MsgUIDef prefabDef = (MsgUIDef)prefab.UIDef;
            EssenceItem oneEssence = new()
            {
                amount = 1,
                name = Consts.GhostEssenceItemName,
                UIDef = new MsgUIDef()
                {
                    name = new LanguageString("UI", "ITEMCHANGER_NAME_ESSENCE_1"),
                    shopDesc = prefabDef.shopDesc,
                    sprite = prefabDef.sprite
                }
            };
            SupplementalMetadataTagFactory.AddTagToItem(oneEssence, poolGroup: Consts.GhostPoolGroup);
            Finder.DefineCustomItem(oneEssence);
        }

        public static void DefineLocations()
        {
            if (GhostInfos is null)
            {
                JsonSerializer js = new()
                {
                    Formatting = Formatting.Indented,
                    TypeNameHandling = TypeNameHandling.Auto,
                };

                using Stream s = typeof(ICInterop).Assembly.GetManifestResourceStream("RandoPlus.Resources.GhostEssence.ghostdata.json");
                using StreamReader sr = new(s);
                using JsonTextReader jtr = new(sr);
                GhostInfos = js.Deserialize<List<GhostInfo>>(jtr);
            }

            foreach (GhostInfo info in GhostInfos)
            {
                Finder.DefineCustomLocation(DefineGhostLocation(info));
            }
        }
    }
}
