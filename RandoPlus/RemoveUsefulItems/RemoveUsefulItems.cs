namespace RandoPlus.RemoveUsefulItems
{
    public static class RemoveUsefulItems
    {
        public static void Hook(bool randoInstalled, bool itemchangerInstalled)
        {
            if (randoInstalled) LogicPatcher.Hook();
            if (randoInstalled) RequestModifier.Hook();
            if (itemchangerInstalled) Items.ItemDefinition.DefineItems();
        }
    }
}
