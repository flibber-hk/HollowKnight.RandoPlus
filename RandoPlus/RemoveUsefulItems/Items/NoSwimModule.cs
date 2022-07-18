using GlobalEnums;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using ItemChanger;
using ItemChanger.Extensions;
using ItemChanger.FsmStateActions;
using System.Linq;
using UnityEngine;

namespace RandoPlus.RemoveUsefulItems.Items
{
    public class NoSwimModule : ItemChanger.Modules.Module
    {
        // Have to do a positive amount of damage to trigger the respawn
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
            // Code for swimn't modified from the implementation of swimn't in ItemChanger

            if (fsm.gameObject.LocateMyFSM("Acid Armour Check") != null) return; // acid

            GameObject splashSurface = fsm.transform.Find("Splash Surface").gameObject;
            splashSurface.layer = 17; // orig is 8, which can enable seam jumping when it intersects with other terrain colliders
            splashSurface.AddComponent<NonBouncer>();

            FsmState idle = fsm.GetState("Idle");
            FsmState damageHero = fsm.AddState("Damage Hero");

            idle.Transitions[0].SetToState(damageHero);

            damageHero.Actions = fsm.GetState("Splash In Norm").Actions.Where(a => a is not SetPosition).ToArray(); // play splash audio and fling splash particles
            damageHero.AddLastAction(new Lambda(() => HeroController.instance.TakeDamage(
                fsm.gameObject, CollisionSide.bottom, gotNoSwim ? NoSwimDamage : 1, 2)));
            damageHero.AddTransition(FsmEvent.Finished, idle);
        }
    }
}
