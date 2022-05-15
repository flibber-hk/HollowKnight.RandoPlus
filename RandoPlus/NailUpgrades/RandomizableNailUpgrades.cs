using ItemChanger;
using System.Collections.Generic;

namespace RandoPlus.NailUpgrades
{
    public static class RandomizableNailUpgrades
    {
        public static void Hook(bool rando, bool ic)
        {
            if (ic) ItemDefinition.DefineItemsAndLocations();
            if (rando) RequestMaker.Hook();
            if (rando) LogicAdder.Hook();
            if (rando) CondensedSpoilerLogger.AddCategory("Nail Upgrades", (args) => true, new() { Consts.NailUpgrade });
        }
    }
}
