using ItemChanger;
using System.Collections.Generic;
using RandomizerMod.RandomizerData;
using RandomizerMod.RC;
using RandoPlus.Imports;
using System.Linq;

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
            if (rando) HookRandoController();
            if (rando) ExportVanillaPool();
        }

        private static void ExportVanillaPool()
        {
            RandoVanillaTracker.AddInterop("Nail Upgrades", () => GetVanillaNailUpgrades().ToList());
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

        private static void HookRandoController()
        {
            RandoController.OnExportCompleted += SetNailUpgradeModule;
        }

        private static void SetNailUpgradeModule(RandoController rc)
        {
            if (RandoPlus.GS.NailUpgrades && RandoPlus.GS.GiveNailUpgradesOnPickup)
            {
                ItemChangerMod.Modules.GetOrAdd<DelayedNailUpgradeModule>().GiveNailUpgradesOnPickup = true;
            }
        }
    }
}
