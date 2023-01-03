using RandomizerMod.RandomizerData;
using RandoPlus.Imports;
using System.Collections.Generic;
using System.Linq;

namespace RandoPlus.GhostEssence
{
    public static class GhostNPCs
    {
        public static void Hook(bool rando, bool ic)
        {
            if (ic) ICInterop.DefineItemsAndLocations();
            if (rando) RequestMaker.Hook();
            if (rando) LogicAdder.Hook();

            if (rando) ExportVanillaPool();
        }

        private static void ExportVanillaPool()
        {
            RandoVanillaTracker.AddInterop("Ghost Essence", () => GhostNames.ToArray().Select(name => new VanillaDef(Consts.GhostEssenceItemName, name)).ToList());
        }
    }
}
