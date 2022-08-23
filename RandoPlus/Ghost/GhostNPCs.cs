using RandomizerMod.RandomizerData;
using RandoPlus.Imports;
using System.Collections.Generic;

namespace RandoPlus.Ghost
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
            RandoVanillaTracker.AddInterop(Consts.GhostPoolGroup, () => new List<VanillaDef>()
            {
                new("Ghost_Essence-Gravedigger","Ghost_Essence-Gravedigger")
            });
        }
    }
}
