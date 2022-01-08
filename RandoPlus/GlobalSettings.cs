namespace RandoPlus
{
    public class GlobalSettings
    {
        public bool MrMushroom;
        public bool NoTear;
        public bool NoLantern;
        public bool NoSwim;

        [Newtonsoft.Json.JsonIgnore]
        public bool Any => MrMushroom || NoSwim || NoTear || NoLantern;
    }
}
