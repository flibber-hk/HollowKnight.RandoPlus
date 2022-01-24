using ItemChanger;

namespace RandoPlus.NailUpgrades
{
    /// <summary>
    /// Item that allows the user to claim a nail upgrade (increase nail damage etc) at a later time from the inventory.
    /// </summary>
    public class DelayedNailUpgradeItem : AbstractItem
    {
        protected override void OnLoad()
        {
            ItemChangerMod.Modules.GetOrAdd<DelayedNailUpgradeModule>();
        }

        public override void GiveImmediate(GiveInfo info)
        {
            ItemChangerMod.Modules.Get<DelayedNailUpgradeModule>().GiveNailUpgrade();
        }
    }
}
