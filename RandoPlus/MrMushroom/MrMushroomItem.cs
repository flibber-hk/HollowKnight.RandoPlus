using ItemChanger;
using ItemChanger.Items;

namespace RandoPlus.MrMushroom
{
    public class MrMushroomItem : IntItem
    {
        protected override void OnLoad()
        {
            base.OnLoad();
            ItemChangerMod.Modules.GetOrAdd<MushroomInventoryTrackerAddon>();
        }
    }
}
