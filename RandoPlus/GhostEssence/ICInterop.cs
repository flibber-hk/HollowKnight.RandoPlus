using System;
using System.Collections.Generic;
using System.Linq;
using ItemChanger;
using ItemChanger.Items;
using ItemChanger.UIDefs;

namespace RandoPlus.GhostEssence
{
    public static class ICInterop
    {
        public static void DefineItemsAndLocations()
        {
            void DefineItem(string ghostName)
            {
                EssenceItem oneEssence = new()
                {
                    name = ghostName,
                    amount = 1,
                    UIDef = new MsgUIDef()
                    {
                        sprite = new ItemChangerSprite("ShopIcons.Essence"),
                        shopDesc = new BoxedString("Wield your dream nail and blast the ghost haha"),
                        name = new LanguageString("UI", "ITEMCHANGER_NAME_ESSENCE_1"),
                    }
                };
                SupplementalMetadataTagFactory.AddTagToItem(oneEssence, poolGroup: Consts.GhostPoolGroup);
                Finder.DefineCustomItem(oneEssence);
            }

            foreach(string ghostName in GhostNames.Names)
            {
                DefineItem(ghostName);
            }

            void DefineLocation(string ghostName, string scene, string objectName)
            {
                GhostLocation ghostLocation = new()
                {
                    name = "Ghost_Essence-" + ghostName,
                    objectName = objectName,
                    flingType = FlingType.DirectDeposit,
                    sceneName = scene,
                };
                SupplementalMetadataTagFactory.AddTagToLocation(ghostLocation, poolGroup: Consts.GhostPoolGroup, vanillaItem: ghostLocation.name);
                Finder.DefineCustomLocation(ghostLocation);
            }

            DefineLocation("Gravedigger", SceneNames.Town, "Gravedigger NPC");
            DefineLocation("Vespa", SceneNames.Hive_05, "Vespa NPC");
            DefineLocation("Joni", SceneNames.Cliffs_05, "Ghost NPC Joni");
            DefineLocation("Marissa", SceneNames.Ruins_Bathhouse, "Ghost NPC");
            DefineLocation("Poggy_Thorax", SceneNames.Ruins_Elevator, "Ghost NPC");
            DefineLocation("Caelif_&_Fera_Orthop", SceneNames.Fungus1_24, "Ghost NPC");

            //DefineLocation("Cloth", SceneNames.Fungus3_23, "Cloth Ghost NPC");
            GhostClothLocation clothLocation = new()
            {
                name = "Ghost_Essence-Cloth",
                objectName = "Cloth Ghost NPC",
                flingType = FlingType.Everywhere,
                sceneName = SceneNames.Fungus3_23,
            };
            SupplementalMetadataTagFactory.AddTagToLocation(clothLocation, poolGroup: Consts.GhostPoolGroup, vanillaItem: "Ghost_Essence-Cloth");
            Finder.DefineCustomLocation(clothLocation);

            DefineLocation("Revek", SceneNames.RestingGrounds_08, "Ghost revek");
            DefineLocation("Millybug", SceneNames.RestingGrounds_08, "Ghost milly");
            DefineLocation("Caspian", SceneNames.RestingGrounds_08, "Ghost caspian");
            DefineLocation("Atra", SceneNames.RestingGrounds_08, "Ghost atra");
            DefineLocation("Dr_Chagax", SceneNames.RestingGrounds_08, "Ghost chagax");
            DefineLocation("Garro", SceneNames.RestingGrounds_08, "Ghost garro");
            DefineLocation("Kcin", SceneNames.RestingGrounds_08, "Ghost kcin");
            DefineLocation("Karina", SceneNames.RestingGrounds_08, "karina");
            DefineLocation("Hundred_Nail_Warrior", SceneNames.RestingGrounds_08, "Ghost NPC 100 nail");
            DefineLocation("Grohac", SceneNames.RestingGrounds_08, "Ghost grohac");
            DefineLocation("Perpetos_Noo", SceneNames.RestingGrounds_08, "Ghost perpetos");
            DefineLocation("Molten", SceneNames.RestingGrounds_08, "Ghost molten");
            DefineLocation("Magnus_Strong", SceneNames.RestingGrounds_08, "Ghost magnus");
            DefineLocation("Waldie", SceneNames.RestingGrounds_08, "Ghost waldie");
            DefineLocation("Wayner", SceneNames.RestingGrounds_08, "Ghost wayner");
            DefineLocation("Wyatt", SceneNames.RestingGrounds_08, "Ghost wyatt");
            DefineLocation("Hex", SceneNames.RestingGrounds_08, "Ghost hex");
            DefineLocation("Thistlewind", SceneNames.RestingGrounds_08, "Ghost thistlewind");
            DefineLocation("Boss", SceneNames.RestingGrounds_08, "Ghost boss");
        }
    }
}
