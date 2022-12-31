using RandoSettingsManager;
using RandoSettingsManager.SettingsManagement;
using RandoSettingsManager.SettingsManagement.Versioning;

namespace RandoPlus
{
    internal static class RandoSettingsManagerInterop
    {
        public static void Hook()
        {
            RandoSettingsManagerMod.Instance.RegisterConnection(new RandoPlusSettingsProxy());
        }
    }

    internal class RandoPlusSettingsProxy : RandoSettingsProxy<GlobalSettings, string>
    {
        public override string ModKey => RandoPlus.instance.GetName();

        public override VersioningPolicy<string> VersioningPolicy { get; }
            = new EqualityVersioningPolicy<string>(RandoPlus.instance.GetVersion());

        public override void ReceiveSettings(GlobalSettings settings)
        {
            RandoPlus.GS.LoadFrom(settings);
            MenuHolder.Instance.ResetMenu();
        }

        public override bool TryProvideSettings(out GlobalSettings settings)
        {
            settings = RandoPlus.GS;
            return settings.Any;
        }
    }
}
