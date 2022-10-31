using System;
using System.Reflection;

public class HashIgnoreAttribute : Attribute { }

namespace RandoPlus
{
    public class GlobalSettings
    {
        public bool DefineRefs;

        public bool MrMushroom;
        public bool DupeSporeShroom;
        
        public bool NoTear;
        public bool NoLantern;
        public bool NoSwim;

        [Newtonsoft.Json.JsonIgnore] public bool AnyUsefulItemsRemoved => DefineRefs || NoTear || NoSwim || NoLantern;

        public bool AreaBlitz;
        public bool FullFlexibleCount;
        [HashIgnore] public bool PreferMultiShiny;

        public bool NailUpgrades;
        [HashIgnore] public bool GiveNailUpgradesOnPickup;
        public bool TwoDupePaleOre;

        [Newtonsoft.Json.JsonIgnore]
        public bool Any => DefineRefs
            || MrMushroom
            || DupeSporeShroom
            || NoSwim
            || NoTear
            || NoLantern
            || AreaBlitz
            || FullFlexibleCount
            || NailUpgrades
            || TwoDupePaleOre;

        public void LoadFrom(GlobalSettings gs)
        {
            gs ??= new();

            foreach (FieldInfo fi in GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                if (fi.GetCustomAttribute<HashIgnoreAttribute>() is not null)
                {
                    continue;
                }

                object val = fi.GetValue(gs);
                fi.SetValue(this, val);
            }
        }
    }
}
