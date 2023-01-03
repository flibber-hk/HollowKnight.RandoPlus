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
                // If this is false, Cloth hasn't died to Traitor Lord so won't appear.
                // Replace this check so that Cloth appears if Traitor Lord has been killed and Cloth rescued.
                return PlayerData.instance.GetBool(nameof(PlayerData.savedCloth)) && PlayerData.instance.GetBool(nameof(PlayerData.killedTraitorLord));
            }
            else if (name == nameof(PlayerData.clothGhostSpoken))
            {
                // If this is true, Cloth has been spoken to so won't appear.
                // Replace this check so that speaking to Cloth doesn't stop her from appearing again.
                return false;
            }
            else if (name == nameof(PlayerData.clothInTown))
            {
                // Set in Abyss_17 by the Emerge FSM if Traitor Lord is dead (and checked by a DIPDF component in Town).
                // We don't want Cloth to come to Town, so we override this.
                return false;
            }
            return orig;
        }
    }

}
