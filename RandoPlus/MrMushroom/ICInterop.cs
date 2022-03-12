using ItemChanger;
using ItemChanger.UIDefs;

namespace RandoPlus.MrMushroom
{
    public static class ICInterop
    {
        public static void DefineItemsAndLocations()
        {
            MrMushroomItem mushItem = new()
            {
                fieldName = nameof(PlayerData.mrMushroomState),
                name = Consts.MrMushroomLevelUp,
                amount = 1,
                UIDef = new SplitUIDef()
                {
                    sprite = new EmbeddedSprite("MrMushroom.mushroom"),
                    shopDesc = new LanguageString("Minor NPC", "MR_MUSHROOM_SHROOMISH1"),
                    preview = new BoxedString("Mr Mushroom Level Up"),
                    name = new MrMushroomString(),
                }
            };
            SupplementalMetadataTag itemMetadata = mushItem.AddTag<SupplementalMetadataTag>();
            itemMetadata.PoolGroup = Consts.LoreTabletPoolGroup;
            Finder.DefineCustomItem(mushItem);

            void DefineLocation(string name, string scene, string objectName, int level)
            {
                MrMushroomLocation mushLocation = new()
                {
                    name = name,
                    objectName = objectName,
                    mushroomState = level,
                    flingType = FlingType.Everywhere,
                    sceneName = scene,
                };
                SupplementalMetadataTag locationMetadata = mushLocation.AddTag<SupplementalMetadataTag>();
                locationMetadata.PoolGroup = Consts.LoreTabletPoolGroup;
                Finder.DefineCustomLocation(mushLocation);
            }

            DefineLocation(Consts.MrMushroomFungalWastes, SceneNames.Fungus2_18, "Mr Mushroom NPC", 1);
            DefineLocation(Consts.MrMushroomKingdomsEdge, SceneNames.Deepnest_East_01, "Mr Mushroom NPC", 2);
            DefineLocation(Consts.MrMushroomDeepnest, SceneNames.Deepnest_40, "Mr Mushroom NPC", 3);
            DefineLocation(Consts.MrMushroomHowlingCliffs, SceneNames.Room_nailmaster, "Mr Mushroom NPC", 4);
            DefineLocation(Consts.MrMushroomAncientBasin, SceneNames.Abyss_21, "Mr Mushroom NPC", 5);
            DefineLocation(Consts.MrMushroomFogCanyon, SceneNames.Fungus3_44, "Mr Mushroom NPC", 6);
            DefineLocation(Consts.MrMushroomKingsPass, SceneNames.Tutorial_01, "Mr Mushroom NPC (1)", 7);
        }
    }
}
