using GlobalEnums;
using HutongGames.PlayMaker;
using ItemChanger;
using ItemChanger.Extensions;
using ItemChanger.FsmStateActions;

namespace RandoPlus.Items
{
    public class NoSwimModule : ItemChanger.Modules.Module
    {
        public const int NoSwimDamage = 36931;

        public bool gotNoSwim { get; set; }

        public override void Initialize()
        {
            Events.AddFsmEdit(new("Surface Water Region"), EditWaterSurface);
            Modding.ModHooks.GetPlayerBoolHook += SkillBoolGetOverride;
            Modding.ModHooks.SetPlayerBoolHook += SkillBoolSetOverride;
            Modding.ModHooks.AfterTakeDamageHook += ChangeDamageAmount;
        }

        private int ChangeDamageAmount(int hazardType, int damageAmount)
        {
            if (hazardType == (int)HazardType.ACID && damageAmount == NoSwimDamage && gotNoSwim)
            {
                return 0;
            }
            return damageAmount;
        }

        public override void Unload()
        {
            Events.RemoveFsmEdit(new("Surface Water Region"), EditWaterSurface);
            Modding.ModHooks.GetPlayerBoolHook -= SkillBoolGetOverride;
            Modding.ModHooks.SetPlayerBoolHook -= SkillBoolSetOverride;
            Modding.ModHooks.AfterTakeDamageHook -= ChangeDamageAmount;
        }

        private bool SkillBoolGetOverride(string boolName, bool value)
        {
            return boolName switch
            {
                nameof(gotNoSwim) => gotNoSwim,
                _ => value,
            };
        }

        private bool SkillBoolSetOverride(string boolName, bool value)
        {
            switch (boolName)
            {
                case nameof(gotNoSwim):
                    gotNoSwim = value;
                    break;
            }
            return value;
        }

        private void EditWaterSurface(PlayMakerFSM fsm)
        {
            if (fsm.gameObject.LocateFSM("Acid Armour Check") != null) return; // acid

            FsmState splash = fsm.GetState("Big Splash?");
            FsmStateAction acidDeath = new Lambda(() =>
            {
                // this is actually the spike death despite the enum, because the acid death splashes green stuff
                HeroController.instance.TakeDamage(fsm.gameObject, CollisionSide.other, gotNoSwim ? NoSwimDamage : 1, (int)HazardType.ACID);
                PlayMakerFSM.BroadcastEvent("SWIM DEATH");
            });

            splash.AddFirstAction(acidDeath);
            splash.AddTransition("SWIM DEATH", "Idle");
        }
    }
}
