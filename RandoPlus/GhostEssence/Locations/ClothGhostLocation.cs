using Modding;

namespace RandoPlus.GhostEssence.Locations
{
    public class ClothGhostLocation : GhostLocation
    {
        protected override void OnLoad()
        {
            base.OnLoad();
            ModHooks.GetPlayerBoolHook += ClothBoolHook;
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            ModHooks.GetPlayerBoolHook -= ClothBoolHook;
        }

        private bool ClothBoolHook(string name, bool orig)
        {
            if (name == nameof(PlayerData.clothKilled))
            {
                return PlayerData.instance.GetBool(nameof(PlayerData.savedCloth)) && PlayerData.instance.GetBool(nameof(PlayerData.killedTraitorLord));
            }
            if (name == nameof(PlayerData.clothGhostSpoken))
            {
                return false;
            }
            return orig;
        }
    }

}
