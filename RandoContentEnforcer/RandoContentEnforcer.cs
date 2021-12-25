using System;
using System.Collections.Generic;
using System.Linq;
using Modding;
using UnityEngine;

namespace RandoContentEnforcer
{
    public class RandoContentEnforcer : Mod, IGlobalSettings<GlobalSettings>
    {
        internal static RandoContentEnforcer instance;

        public static GlobalSettings GS = new();
        public void OnLoadGlobal(GlobalSettings gs) => GS = gs;
        public GlobalSettings OnSaveGlobal() => GS;

        public RandoContentEnforcer() : base(null)
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

            bool rando = ModHooks.GetMod("Randomizer 4") is Mod;
            bool ic = ModHooks.GetMod("ItemChangerMod") is Mod;

            if (rando) MenuHolder.Hook();
            if (rando) LogicPatcher.Hook();
            if (rando) RequestModifier.Hook();
            if (ic) Items.ItemDefinition.DefineItems();
        }
    }
}