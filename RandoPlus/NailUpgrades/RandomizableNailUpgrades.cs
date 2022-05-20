using ItemChanger;
using System.Collections.Generic;
using RandomizerMod.RandomizerData;

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

        internal static IEnumerable<VanillaDef> GetVanillaNailUpgrades()
        {
            for (int i = 1; i < 5; i++)
            {
                yield return new(
                    Consts.NailUpgrade,
                    Consts.NailsmithLocationPrefix + i,
                    new[] { new CostDef("GEO", Consts.VanillaNailUpgradeCosts[i]) });
            }
        }
    }
}
