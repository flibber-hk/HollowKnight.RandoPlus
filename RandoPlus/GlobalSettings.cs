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

        public bool AnyUsefulItemsRemoved => DefineRefs || NoTear || NoSwim || NoLantern;

        public bool AreaBlitz;
        public bool PreferMultiShiny;

        public bool NailUpgrades;
        public bool GiveNailUpgradesOnPickup;
        public bool TwoDupePaleOre;

        [Newtonsoft.Json.JsonIgnore]
        public bool Any => DefineRefs
            || MrMushroom
            || NoSwim
            || NoTear
            || NoLantern
            || AreaBlitz
            || NailUpgrades;
    }
}