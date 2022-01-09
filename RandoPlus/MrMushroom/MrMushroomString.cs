using ItemChanger;

namespace RandoPlus.MrMushroom
{
    public class MrMushroomString : IString
    {
        public string Value => $"Mr Mushroom Level {PlayerData.instance.GetInt(nameof(PlayerData.mrMushroomState))}/8";

        public IString Clone() => (MrMushroomString)MemberwiseClone();
    }
}
