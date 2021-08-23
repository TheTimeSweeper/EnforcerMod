using RoR2;
using RoR2.Projectile;
using UnityEngine;
using EntityStates.Toolbot;

namespace EntityStates.Enforcer
{
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

            base.OnEnter();
        }

        public override void FixedUpdate()
        {
            base.characterBody.SetAimTimer(0.25f);
            this.fixedAge += Time.fixedDeltaTime;

            bool isShielded = base.HasBuff(EnforcerPlugin.Modules.Buffs.protectAndServeBuff) || base.HasBuff(EnforcerPlugin.Modules.Buffs.energyShieldBuff);
            if (!isShielded) base.PlayAnimation("RightArm, Override", "FireRifle");

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

            Util.PlayAttackSpeedSound(EnforcerPlugin.Sounds.LaunchTearGas, base.gameObject, 0.7f);

            if (base.HasBuff(EnforcerPlugin.Modules.Buffs.protectAndServeBuff) || base.HasBuff(EnforcerPlugin.Modules.Buffs.energyShieldBuff))
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
        }
    }

    public class ShockGrenade : BaseSkillState
    {
        public static float baseDuration = 0.75f;
        public static float damageCoefficient = 5f;
        public static float procCoefficient = 0.6f;
        public static float bulletRecoil = 2.5f;
        public static float projectileSpeed = 90f;

        //TODO: why did fucking "grenademuzzle" break here but not in the others?
        public static string muzzleString = "Muzzle";

        public static GameObject effectPrefab = Resources.Load<GameObject>("Prefabs/Effects/MuzzleFlashes/MuzzleflashMageLightning");

        private float duration;
        private ChildLocator childLocator;
        private Animator animator;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = ShockGrenade.baseDuration / this.attackSpeedStat;
            this.childLocator = base.GetModelTransform().GetComponent<ChildLocator>();
            this.animator = base.GetModelAnimator();

            if (base.HasBuff(EnforcerPlugin.Modules.Buffs.protectAndServeBuff) || base.HasBuff(EnforcerPlugin.Modules.Buffs.energyShieldBuff))
            {
                base.PlayAnimation("Gesture, Override", "ShieldFireShotgun", "FireShotgun.playbackRate", this.duration);
            }
            else
            {
                base.PlayAnimation("Gesture, Override", "FireShotgun", "FireShotgun.playbackRate", this.duration);
            }

            Util.PlayAttackSpeedSound(EnforcerPlugin.Sounds.LaunchStunGrenade, base.gameObject, 2.5f);

            base.AddRecoil(-2f * ShockGrenade.bulletRecoil, -3f * ShockGrenade.bulletRecoil, -1f * ShockGrenade.bulletRecoil, 1f * ShockGrenade.bulletRecoil);
            base.characterBody.AddSpreadBloom(0.33f * ShockGrenade.bulletRecoil);
            EffectManager.SimpleMuzzleFlash(ShockGrenade.effectPrefab, base.gameObject, ShockGrenade.muzzleString, false);

            if (base.isAuthority) {

                Debug.LogWarning(childLocator);

                Debug.LogWarning(childLocator.FindChild(ShockGrenade.muzzleString));
                Ray aimRay = base.GetAimRay();
                FireProjectileInfo info = new FireProjectileInfo()
                {
                    crit = base.RollCrit(),
                    damage = ShockGrenade.damageCoefficient * this.damageStat,
                    damageColorIndex = DamageColorIndex.Default,
                    damageTypeOverride = DamageType.Shock5s,
                    force = 250,
                    owner = base.gameObject,
                    position = childLocator.FindChild(ShockGrenade.muzzleString).position,
                    procChainMask = default(ProcChainMask),
                    projectilePrefab = EnforcerPlugin.EnforcerModPlugin.shockGrenade,
                    rotation = Quaternion.LookRotation(base.GetAimRay().direction),
                    useFuseOverride = false,
                    useSpeedOverride = true,
                    speedOverride = ShockGrenade.projectileSpeed,
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