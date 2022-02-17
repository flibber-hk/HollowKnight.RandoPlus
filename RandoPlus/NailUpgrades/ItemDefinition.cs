using ItemChanger;
using ItemChanger.UIDefs;
using ItemChanger.Locations;

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
   

            void DefineLocations(string pos)
            {
                AutoLocation Nailsmith = new NailsmithLocation()
                {
                    name = Consts.NailPlace+pos,
                    sceneName = SceneNames.Room_nailsmith,
                    flingType = FlingType.DirectDeposit,
                    
                };

                Finder.DefineCustomLocation(Nailsmith);
            }


            for(int counter=1;counter<5; counter++)
            {
                DefineLocations(counter.ToString());
            }



        }

       




    }
}
