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

            return ghostLocation;
        }

        public static void DefineItemsAndLocations()
        {
            DefineItems();
            DefineLocations();
        }

        public static void DefineItems()
        {
            EssenceItem oneEssence = EssenceItem.MakeEssenceItem(1);
            oneEssence.name = Consts.GhostEssenceItemName;
            SupplementalMetadataTagFactory.AddTagToItem(oneEssence, poolGroup: Consts.GhostPoolGroup);
            Finder.DefineCustomItem(oneEssence);
        }

        public static void DefineLocations()
        {
            if (GhostInfos is null)
            {
                GhostInfos = Finder.DeserializeResource<List<GhostInfo>>(typeof(ICInterop).Assembly, "RandoPlus.Resources.GhostEssence.ghostdata.json");
            }

            foreach (GhostInfo info in GhostInfos)
            {
                Finder.DefineCustomLocation(DefineGhostLocation(info));
            }
        }
    }
}
