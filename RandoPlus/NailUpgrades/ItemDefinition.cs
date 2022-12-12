using ItemChanger;
using ItemChanger.Tags;
using ItemChanger.UIDefs;

namespace RandoPlus.NailUpgrades
{
    public static class ItemDefinition
    {
        public static void DefineItemsAndLocations()
        {
            AbstractItem NailUpgrade = new DelayedNailUpgradeItem()
            {
                name = Consts.NailUpgrade,
                UIDef = new MsgUIDef()
                {
                    name = new BoxedString("Nail Upgrade"),
                    shopDesc = new BoxedString("Haha big meme go brr"),
                    sprite = new ItemChangerSprite("ShopIcons.Downslash")
                }
            };
            SupplementalMetadataTagFactory.AddTagToItem(NailUpgrade, poolGroup: Consts.NailUpgradePoolGroup);
            NailUpgrade.AddTag(new InteropTag()
            {
                Message = "CurseData",
                Properties = new()
                {
                    ["MimicNames"] = new string[] {"Upgrade Nail", "Nai1 Upgrade"},
                    ["CanMimic"] =  new BoxedBool(true)
                }
            });
            Finder.DefineCustomItem(NailUpgrade);

            for (int i = 1; i < 5; i++)
            {
                AbstractLocation Nailsmith = new NailsmithLocation()
                {
                    HintActive = true,
                    flingType = FlingType.Everywhere,
                    sceneName = SceneNames.Room_nailsmith,
                    NailUpgradeSlot = i,
                    name = Consts.NailsmithLocationPrefix + i
                };
                SupplementalMetadataTagFactory.AddTagToLocation(Nailsmith, poolGroup: Consts.NailUpgradePoolGroup, vanillaItem: Consts.NailUpgrade);
                Finder.DefineCustomLocation(Nailsmith);
            }
        }
    }
}
