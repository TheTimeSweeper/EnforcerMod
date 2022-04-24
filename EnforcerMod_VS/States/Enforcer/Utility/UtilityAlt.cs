using Modules;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace EntityStates.Enforcer {
    public class StunGrenade : BaseSkillState
    {
        public static float baseDuration = 0.5f;
        public static float damageCoefficient = 5.4f;
        public static float bulletRecoil = 2.5f;
        public static float projectileSpeed = 75f;

        public static string muzzleString = "GrenadeMuzzle";

        private float duration;
        private ChildLocator childLocator;
        private Animator animator;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = StunGrenade.baseDuration / this.attackSpeedStat;
            this.childLocator = base.GetModelTransform().GetComponent<ChildLocator>();
            this.animator = base.GetModelAnimator();

            base.StartAimMode();

            if (base.HasBuff(Buffs.protectAndServeBuff) || base.HasBuff(Buffs.energyShieldBuff))
            {
                base.PlayAnimation("Gesture, Override", "ShieldFireShotgun", "FireShotgun.playbackRate", this.duration);
            }
            else
            {
                base.PlayAnimation("Gesture, Override", "FireShotgun", "FireShotgun.playbackRate", this.duration);
            }

            Util.PlaySound(Sounds.LaunchStunGrenade, base.gameObject);

            base.AddRecoil(-2f * StunGrenade.bulletRecoil, -3f * StunGrenade.bulletRecoil, -1f * StunGrenade.bulletRecoil, 1f * StunGrenade.bulletRecoil);
            base.characterBody.AddSpreadBloom(0.33f * StunGrenade.bulletRecoil);
            EffectManager.SimpleMuzzleFlash(Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, base.gameObject, StunGrenade.muzzleString, false);

            if (base.isAuthority) {

                Transform OGrigin = base.characterBody.aimOriginTransform;

                base.characterBody.aimOriginTransform = childLocator.FindChild("GrenadeAimOrigin");

                Ray aimRay = base.GetAimRay();
                Vector3 aimTweak;

                Vector3 aimCross = Vector3.Cross(aimRay.direction, Vector3.up).normalized;
                Vector3 aimUpPerpendicular = Vector3.Cross(aimCross, aimRay.direction).normalized;
                aimTweak = aimUpPerpendicular;

                FireProjectileInfo info = new FireProjectileInfo()
                {
                    crit = base.RollCrit(),
                    damage = StunGrenade.damageCoefficient * this.damageStat,
                    damageColorIndex = DamageColorIndex.Default,
                    damageTypeOverride = DamageType.Stun1s,
                    force = 0f,
                    owner = base.gameObject,
                    position = aimRay.origin,
                    procChainMask = default(ProcChainMask),
                    projectilePrefab = EnforcerPlugin.EnforcerModPlugin.stunGrenade,
                    rotation = RoR2.Util.QuaternionSafeLookRotation(aimRay.direction + aimTweak * 0.08f),
                    useFuseOverride = false,
                    useSpeedOverride = true,
                    speedOverride = StunGrenade.projectileSpeed,
                    target = null
                };
                ProjectileManager.instance.FireProjectile(info);

                base.characterBody.aimOriginTransform = OGrigin;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}