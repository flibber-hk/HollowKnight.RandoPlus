using ItemChanger;
using ItemChanger.UIDefs;

namespace RandoPlus.NailUpgrades
{
    public static class ItemDefinition
    {
        public static void DefineItemsAndLocations()
        {
            AbstractItem NailUpgrade = new DelayedNailUpgradeItem()
            {
                name = Consts.Nail_Upgrade,
                UIDef = new MsgUIDef()
                {
                    name = new BoxedString("Nail Upgrade"),
                    shopDesc = new BoxedString("Haha big meme go brr"),
                    sprite = new ItemChangerSprite("ShopIcons.Downslash")
                }
            };

            Finder.DefineCustomItem(NailUpgrade);
        }
    }
}
