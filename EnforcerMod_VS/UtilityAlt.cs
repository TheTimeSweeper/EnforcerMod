using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace EntityStates.Enforcer
{
    public class StunGrenade : BaseSkillState
    {
        public static float baseDuration = 0.5f;
        public static float damageCoefficient = 4f;
        public static float procCoefficient = 0.6f;
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

            if (base.HasBuff(EnforcerPlugin.EnforcerPlugin.jackBoots) || base.HasBuff(EnforcerPlugin.EnforcerPlugin.energyShieldBuff))
            {
                base.PlayAnimation("RightArm, Override", "FireShotgunShielded", "FireShotgun.playbackRate", this.duration);
            }
            else
            {
                base.PlayAnimation("RightArm, Override", "FireShotgun", "FireShotgun.playbackRate", this.duration);
            }

            Util.PlaySound(EnforcerPlugin.Sounds.LaunchStunGrenade, base.gameObject);

            base.AddRecoil(-2f * StunGrenade.bulletRecoil, -3f * StunGrenade.bulletRecoil, -1f * StunGrenade.bulletRecoil, 1f * StunGrenade.bulletRecoil);
            base.characterBody.AddSpreadBloom(0.33f * StunGrenade.bulletRecoil);
            EffectManager.SimpleMuzzleFlash(Commando.CommandoWeapon.FirePistol.effectPrefab, base.gameObject, StunGrenade.muzzleString, false);

            if (base.isAuthority)
            {
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
                    force = 2000,
                    owner = base.gameObject,
                    position = aimRay.origin,
                    procChainMask = default(ProcChainMask),
                    projectilePrefab = EnforcerPlugin.EnforcerPlugin.stunGrenade,
                    rotation = RoR2.Util.QuaternionSafeLookRotation(aimRay.direction + aimTweak * 0.08f),
                    useFuseOverride = false,
                    useSpeedOverride = true,
                    speedOverride = StunGrenade.projectileSpeed,
                    target = null
                };
                ProjectileManager.instance.FireProjectile(info);
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