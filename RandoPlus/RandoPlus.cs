using System;
using System.Collections.Generic;
using System.Linq;
using Modding;
using UnityEngine;

namespace RandoPlus
{
    public class RandoPlus : Mod, IGlobalSettings<GlobalSettings>
    {
        internal static RandoPlus instance;

        public static GlobalSettings GS { get; set; } = new();
        public void OnLoadGlobal(GlobalSettings gs) => GS = gs;
        public GlobalSettings OnSaveGlobal() => GS;

        public RandoPlus() : base(null)
        {
            instance = this;
        }
        
        public override string GetVersion()
        {
            return GetType().Assembly.GetName().Version.ToString();
        }
        
        public override void Initialize()
        {
            Log("Initializing Mod...");

            On.DeactivateIfPlayerdataFalse.OnEnable += (o, s) => Log($"FALSE {s.gameObject.name} - {s.boolName}");
            On.DeactivateIfPlayerdataTrue.OnEnable += (o, s) => Log($"TRUE {s.gameObject.name} - {s.boolName}");

            bool rando = ModHooks.GetMod("Randomizer 4") is Mod;
            bool ic = ModHooks.GetMod("ItemChangerMod") is Mod;

            if (rando) MenuHolder.Hook();
            RemoveUsefulItems.RemoveUsefulItems.Hook(rando, ic);
            MrMushroom.MrMushroom.Hook(rando, ic);
            AreaRestriction.AreaRestriction.Hook(rando, ic);
            NailUpgrades.RandomizableNailUpgrades.Hook(rando, ic);
            GhostEssence.GhostNPCs.Hook(rando, ic);
            Advanced.AdvancedRequests.Hook(rando, ic);

            Common.Hook(rando, ic);

            if (ModHooks.GetMod("RandoSettingsManager") is Mod)
            {
                RandoSettingsManagerInterop.Hook();
            }
        }
    }
}