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

        public bool AreaBlitz;
        public bool PreferMultiShiny;

        [Newtonsoft.Json.JsonIgnore]
        public bool Any => DefineRefs
            || MrMushroom
            || NoSwim
            || NoTear
            || NoLantern
            || AreaBlitz;
    }
}
