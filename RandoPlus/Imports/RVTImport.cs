using System;
using System.Collections.Generic;
using MonoMod.ModInterop;
using RandomizerMod.RandomizerData;

namespace RandoPlus.Imports
{
    internal static class RandoVanillaTracker
    {
        [ModImportName("RandoVanillaTracker")]
        private static class RandoVanillaTrackerImport
        {
            public static Action<string, Func<List<VanillaDef>>> AddInterop = null;
        }
        static RandoVanillaTracker()
        {
            typeof(RandoVanillaTrackerImport).ModInterop();
        }

        public static void AddInterop(string pool, Func<List<VanillaDef>> GetPlacements)
            => RandoVanillaTrackerImport.AddInterop?.Invoke(pool, GetPlacements);
    }
}