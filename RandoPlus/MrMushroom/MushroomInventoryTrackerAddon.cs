using ItemChanger;
using ItemChanger.Modules;
using System.Text;

namespace RandoPlus.MrMushroom
{
    public class MushroomInventoryTrackerAddon : Module
    {
        public override void Initialize()
        {
            if (ItemChangerMod.Modules.Get<InventoryTracker>() is InventoryTracker t)
            {
                t.OnGenerateFocusDesc += AddMushroomToDesc;
            }
        }
        public override void Unload()
        {
            if (ItemChangerMod.Modules.Get<InventoryTracker>() is InventoryTracker t)
            {
                t.OnGenerateFocusDesc -= AddMushroomToDesc;
            }
        }

        public void AddMushroomToDesc(StringBuilder sb)
        {
            sb.Append($"Mr Mushroom Level {PlayerData.instance.GetInt(nameof(PlayerData.mrMushroomState))}.");
        }
    }
}
