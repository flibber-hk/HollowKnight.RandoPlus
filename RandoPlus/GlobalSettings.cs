namespace RandoPlus
{
    public class GlobalSettings
    {
        public bool MrMushroom;
        public bool NoTear;
        public bool NoLantern;
        public bool NoSwim;

        public bool DeleteAreas;
        public bool PreferMultiShiny;

        [Newtonsoft.Json.JsonIgnore]
        public bool Any => MrMushroom
            || NoSwim
            || NoTear
            || NoLantern
            || DeleteAreas;
    }
}
