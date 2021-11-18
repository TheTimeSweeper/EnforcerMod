using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace EntityStates.Nemforcer
{
    public class ThrowHammer : BaseSkillState
    {
        public static float damageCoefficient = 6.2f;
        public static float baseDuration = 0.75f;
        public static float recoil = 1f;

        private float duration;
        private float fireDuration;
        private bool hasFired;
        private Animator animator;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = ThrowHammer.baseDuration / this.attackSpeedStat;
            this.fireDuration = 0.35f * this.duration;
            base.characterBody.SetAimTimer(2f);
            this.animator = base.GetModelAnimator();

            base.PlayAnimation("Gesture, Override", "ThrowHammer", "ThrowHammer.playbackRate", this.duration);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        private void FireHammer()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                base.characterBody.AddSpreadBloom(ThrowHammer.recoil);
                Ray aimRay = base.GetAimRay();

                base.AddRecoil(-1f * ThrowHammer.recoil, -2f * ThrowHammer.recoil, -0.5f * ThrowHammer.recoil, 0.5f * ThrowHammer.recoil);
                Util.PlaySound(EnforcerPlugin.Sounds.NemesisGrenadeThrow, base.gameObject);

                if (base.isAuthority)
                {
                    ProjectileManager.instance.FireProjectile(EnforcerPlugin.NemforcerPlugin.hammerProjectile, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), base.gameObject, ThrowHammer.damageCoefficient * this.damageStat, 0f, base.RollCrit(), DamageColorIndex.Default, null, 160f);
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.fireDuration)
            {
                this.FireHammer();
            }

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}