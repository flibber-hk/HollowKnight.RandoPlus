using RandomizerMod.RandomizerData;
using RandoPlus.Imports;
using System.Collections.Generic;

namespace RandoPlus.MrMushroom
{
    public static class MrMushroom
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
            RandoVanillaTracker.AddInterop("Mr Mushroom", () => new List<VanillaDef>()
            {
                new(Consts.MrMushroomLevelUp, Consts.MrMushroomFungalWastes),
                new(Consts.MrMushroomLevelUp, Consts.MrMushroomKingdomsEdge),
                new(Consts.MrMushroomLevelUp, Consts.MrMushroomDeepnest),
                new(Consts.MrMushroomLevelUp, Consts.MrMushroomHowlingCliffs),
                new(Consts.MrMushroomLevelUp, Consts.MrMushroomAncientBasin),
                new(Consts.MrMushroomLevelUp, Consts.MrMushroomFogCanyon),
                new(Consts.MrMushroomLevelUp, Consts.MrMushroomKingsPass),
            });
        }
    }
}
