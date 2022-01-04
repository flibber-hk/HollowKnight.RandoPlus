namespace RandoPlus
{
    public class GlobalSettings
    {
        public bool NoTear;
        public bool NoLantern;
        public bool NoSwim;

        [Newtonsoft.Json.JsonIgnore]
        public bool Any => NoSwim || NoTear || NoLantern;
    }
}
