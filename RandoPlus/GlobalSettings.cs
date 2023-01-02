using System;
using System.Linq;
using System.Reflection;

namespace RandoPlus
{
    public class HashIgnoreAttribute : Attribute { }

    public class GlobalSettings
    {
        private static readonly FieldInfo[] _fieldInfos;

        static GlobalSettings()
        {
            _fieldInfos = typeof(GlobalSettings)
                .GetFields(BindingFlags.Instance | BindingFlags.Public)
                .Where(fi => fi.GetCustomAttribute<HashIgnoreAttribute>() is null)
                .ToArray();
        }

        public bool DefineRefs;

        public bool MrMushroom;
        public bool DupeSporeShroom;
        
        public bool NoTear;
        public bool NoLantern;
        public bool NoSwim;

        public bool AnyUsefulItemsRemoved => DefineRefs || NoTear || NoSwim || NoLantern;

        public bool AreaBlitz;
        public bool FullFlexibleCount;

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

            foreach (FieldInfo fi in _fieldInfos)
            {
                object val = fi.GetValue(gs);
                fi.SetValue(this, val);
            }
        }
    }
}