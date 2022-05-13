using RoR2;
using RoR2.Projectile;
using UnityEngine;
using EntityStates.Toolbot;
using Modules;

namespace EntityStates.Enforcer {
    public class AimDamageGas : AimThrowableBase
    {
        private AimStunDrone goodState;

        public override void OnEnter()
        {
            if (goodState == null) goodState = new AimStunDrone();

            maxDistance = 48;
            rayRadius = 2f;
            arcVisualizerPrefab = goodState.arcVisualizerPrefab;
            projectilePrefab = EnforcerPlugin.EnforcerModPlugin.damageGasProjectile;
            endpointVisualizerPrefab = goodState.endpointVisualizerPrefab;
            endpointVisualizerRadiusScale = 4f;
            setFuse = false;
            damageCoefficient = 0.5f;
            baseMinimumDuration = 0.1f;
            projectileBaseSpeed = 80;
            
            base.characterBody.aimOriginTransform = base.GetModelChildLocator().FindChild("GrenadeAimOrigin");

            base.OnEnter();
        }

        public override void FixedUpdate()
        {
            base.characterBody.SetAimTimer(0.25f);
            this.fixedAge += Time.fixedDeltaTime;

            bool isShielded = base.HasBuff(Buffs.protectAndServeBuff) || base.HasBuff(Buffs.energyShieldBuff);
            //if (!isShielded) base.PlayAnimation("RightArm, Override", "FireRifle");

            bool flag = false;

            if (base.isAuthority && !this.KeyIsDown() && base.fixedAge >= this.minimumDuration) flag = true;
            if (base.characterBody && base.characterBody.isSprinting) flag = true;

            if (flag)
            {
                this.UpdateTrajectoryInfo(out this.currentTrajectoryInfo);

                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            Util.PlayAttackSpeedSound(Sounds.LaunchTearGas, base.gameObject, 0.7f);

            if (base.HasBuff(Buffs.protectAndServeBuff) || base.HasBuff(Buffs.energyShieldBuff))
            {
                base.PlayAnimation("RightArm, Override", "FireShotgunShielded");
            }
            else
            {
                base.PlayAnimation("RightArm, Override", "FireShotgun");
            }

            base.AddRecoil(-2f * TearGas.bulletRecoil, -3f * TearGas.bulletRecoil, -1f * TearGas.bulletRecoil, 1f * TearGas.bulletRecoil);
            base.characterBody.AddSpreadBloom(0.33f * TearGas.bulletRecoil);
            EffectManager.SimpleMuzzleFlash(Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, base.gameObject, TearGas.muzzleString, false);

            GetComponent<EnforcerComponent>().ResetAimOrigin(base.characterBody);
        }
    }

    public class ShockGrenade : BaseSkillState
    {
        public static float baseDuration = 0.5f;
        public static float damageCoefficient = 7.2f;
        public static float bulletRecoil = 2.5f;

        //TODO: why did fucking "grenademuzzle" break here but not in the others?
        public static string muzzleString = "Muzzle";

        public static GameObject effectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/MuzzleFlashes/MuzzleflashMageLightning");

        private float duration;
        private ChildLocator childLocator;
        private Animator animator;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = ShockGrenade.baseDuration / this.attackSpeedStat;
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

            Util.PlayAttackSpeedSound(Sounds.LaunchStunGrenade, base.gameObject, 2.5f);

            base.AddRecoil(-2f * ShockGrenade.bulletRecoil, -3f * ShockGrenade.bulletRecoil, -1f * ShockGrenade.bulletRecoil, 1f * ShockGrenade.bulletRecoil);
            base.characterBody.AddSpreadBloom(0.33f * ShockGrenade.bulletRecoil);
            EffectManager.SimpleMuzzleFlash(ShockGrenade.effectPrefab, base.gameObject, ShockGrenade.muzzleString, false);

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
                    damage = ShockGrenade.damageCoefficient * this.damageStat,
                    damageColorIndex = DamageColorIndex.Default,
                    damageTypeOverride = DamageType.Shock5s,
                    force = 0f,
                    owner = base.gameObject,
                    position = aimRay.origin,
                    procChainMask = default(ProcChainMask),
                    projectilePrefab = EnforcerPlugin.EnforcerModPlugin.shockGrenade,
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