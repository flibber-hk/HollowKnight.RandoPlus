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
        public bool PreferMultiShiny;

        public bool NailUpgrades;
        public bool GiveNailUpgradesOnPickup;
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
    }
}
