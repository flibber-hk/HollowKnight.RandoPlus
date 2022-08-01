using RandoPlus.Imports;

namespace RandoPlus.RemoveUsefulItems
{
    public static class RemoveUsefulItems
    {
        public static void Hook(bool randoInstalled, bool itemchangerInstalled)
        {
            if (randoInstalled) LogicPatcher.Hook();
            if (randoInstalled) RequestModifier.Hook();
            if (itemchangerInstalled) Items.ItemDefinition.DefineItems();

            CondensedSpoilerLogger.AddCategory("Removed Useful Items", _ => RandoPlus.GS.AnyUsefulItemsRemoved, new()
            {
                Consts.NoLantern,
                Consts.NoTear,
                Consts.NoSwim,
            });
        }
    }
}
