using System;
using System.Reflection;

namespace RandoPlus
{
    public class HashIgnoreAttribute : Attribute { }

    public class GlobalSettings
    {
        public bool DefineRefs;

        public bool MrMushroom;
        public bool DupeSporeShroom;
        
        public bool NoTear;
        public bool NoLantern;
        public bool NoSwim;

        public bool AnyUsefulItemsRemoved => DefineRefs || NoTear || NoSwim || NoLantern;

        public bool AreaBlitz;
        public bool FullFlexibleCount;
        [HashIgnore] public bool PreferMultiShiny;

        public bool NailUpgrades;
        [HashIgnore] public bool GiveNailUpgradesOnPickup;
        public bool TwoDupePaleOre;

        public bool GhostEssence;

        public bool DisperseGroups;
        public bool EnforceAllConstraints;

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
            || TwoDupePaleOre
            || DisperseGroups
            || EnforceAllConstraints;

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