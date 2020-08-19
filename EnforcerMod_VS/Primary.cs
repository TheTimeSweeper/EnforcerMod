using RoR2;
using UnityEngine;

namespace EntityStates.Enforcer
{
    public class RiotShotgun : BaseSkillState 
    {
        public static float damageCoefficient = 0.4f;
        public static float procCoefficient = 0.5f;
        public static float bulletForce = 35f;
        public static float baseDuration = 0.9f; // the base skill duration
        public static float baseShieldDuration = 0.6f; // the duration used while shield is active
        public static int projectileCount = 8;
        public static float bulletRecoil = 3f;
        public static float shieldedBulletRecoil = 1.25f;
        public static float beefDurationNoShield = 0.0f;
        public static float beefDurationShield = 0.25f;

        private float attackStopDuration;   
        private float duration;
        private float fireDuration;
        public bool hasFired;
        private Animator animator;
        public string muzzleString;

        public override void OnEnter()
        {
            base.OnEnter();
            base.characterBody.SetAimTimer(2f);
            this.animator = base.GetModelAnimator();
            this.muzzleString = "Muzzle";
            if (base.characterBody.skinIndex == 3) this.muzzleString = "BlasterMuzzle";
            this.hasFired = false;

            if (base.HasBuff(EnforcerPlugin.EnforcerPlugin.jackBoots) || base.HasBuff(EnforcerPlugin.EnforcerPlugin.energyShieldBuff))
            {
                this.duration = RiotShotgun.baseShieldDuration / this.attackSpeedStat;
                this.attackStopDuration = RiotShotgun.beefDurationShield / this.attackSpeedStat;
            }
            else
            {
                this.duration = RiotShotgun.baseDuration / this.attackSpeedStat;
                this.attackStopDuration = RiotShotgun.beefDurationNoShield / this.attackSpeedStat;
            }

            base.PlayAnimation("RightArm, Override", "FireShotgun", "FireShotgun.playbackRate", this.duration);

            this.fireDuration = 0.1f * this.duration;
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public virtual void FireBullet()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                string soundString = "";

                bool isCrit = base.RollCrit();

                if (EnforcerMain.shotgunToggle)
                {
                    soundString = EnforcerPlugin.Sounds.FireClassicShotgun;
                }
                else
                {
                    soundString = isCrit ? EnforcerPlugin.Sounds.FireShotgun : EnforcerPlugin.Sounds.FireShotgunCrit;
                }

                if (base.characterBody.skinIndex == EnforcerPlugin.EnforcerPlugin.stormtrooperIndex) soundString = EnforcerPlugin.Sounds.FireBlasterShotgun;

                Util.PlayScaledSound(soundString, base.gameObject, this.attackSpeedStat);

                float recoil = RiotShotgun.bulletRecoil;

                if (base.HasBuff(EnforcerPlugin.EnforcerPlugin.jackBoots) || base.HasBuff(EnforcerPlugin.EnforcerPlugin.energyShieldBuff)) recoil = RiotShotgun.shieldedBulletRecoil;

                base.AddRecoil(-2f * recoil, -3f * recoil, -1f * recoil, 1f * recoil);
                base.characterBody.AddSpreadBloom(0.33f * recoil);
                EffectManager.SimpleMuzzleFlash(Commando.CommandoWeapon.FireBarrage.effectPrefab, base.gameObject, this.muzzleString, false);

                if (base.isAuthority)
                {
                    float damage = RiotShotgun.damageCoefficient * this.damageStat;

                    //unique tracer for stormtrooper skin because this is oddly high effort
                    GameObject tracerEffect = EnforcerPlugin.EnforcerPlugin.bulletTracer;

                    if (base.characterBody.skinIndex == EnforcerPlugin.EnforcerPlugin.stormtrooperIndex) tracerEffect = EnforcerPlugin.EnforcerPlugin.laserTracer;

                    Ray aimRay = base.GetAimRay();

                    new BulletAttack
                    {
                        bulletCount = (uint)projectileCount,
                        aimVector = aimRay.direction,
                        origin = aimRay.origin,
                        damage = damage,
                        damageColorIndex = DamageColorIndex.Default,
                        damageType = DamageType.Generic,
                        falloffModel = BulletAttack.FalloffModel.None,
                        maxDistance = 48,
                        force = RiotShotgun.bulletForce,
                        hitMask = LayerIndex.CommonMasks.bullet,
                        minSpread = 0,
                        maxSpread = 12f,
                        isCrit = isCrit,
                        owner = base.gameObject,
                        muzzleName = muzzleString,
                        smartCollision = false,
                        procChainMask = default(ProcChainMask),
                        procCoefficient = RiotShotgun.procCoefficient,
                        radius = 0.5f,
                        sniper = false,
                        stopperMask = LayerIndex.world.collisionMask,
                        weapon = null,
                        tracerEffectPrefab = tracerEffect,
                        spreadPitchScale = 0.5f,
                        spreadYawScale = 0.5f,
                        queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                        hitEffectPrefab = ClayBruiser.Weapon.MinigunFire.bulletHitEffectPrefab,
                        HitEffectNormal = ClayBruiser.Weapon.MinigunFire.bulletHitEffectNormal
                    }.Fire();
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge < this.attackStopDuration)
            {
                if (base.characterMotor)
                {
                    base.characterMotor.moveDirection = Vector3.zero;
                }
            }

            if (base.fixedAge >= this.fireDuration)
            {
                this.FireBullet();
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

    public class SuperShotgun : RiotShotgun
    {
        public new static float damageCoefficient = 0.7f;
        public new static float procCoefficient = 0.75f;
        public new static float bulletForce = 75f;

        public override void FireBullet()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                string soundString = "";

                bool isCrit = base.RollCrit();

                if (EnforcerMain.shotgunToggle)
                {
                    soundString = EnforcerPlugin.Sounds.FireClassicShotgun;
                }
                else
                {
                    soundString = isCrit ? EnforcerPlugin.Sounds.FireShotgun : EnforcerPlugin.Sounds.FireShotgunCrit;
                }

                if (base.characterBody.skinIndex == EnforcerPlugin.EnforcerPlugin.stormtrooperIndex) soundString = EnforcerPlugin.Sounds.FireBlasterShotgun;

                Util.PlayScaledSound(soundString, base.gameObject, this.attackSpeedStat);

                float recoil = RiotShotgun.bulletRecoil;

                if (base.HasBuff(EnforcerPlugin.EnforcerPlugin.jackBoots) || base.HasBuff(EnforcerPlugin.EnforcerPlugin.energyShieldBuff)) recoil = RiotShotgun.shieldedBulletRecoil;

                base.AddRecoil(-2f * recoil, -3f * recoil, -1f * recoil, 1f * recoil);
                base.characterBody.AddSpreadBloom(0.33f * recoil);
                EffectManager.SimpleMuzzleFlash(Commando.CommandoWeapon.FireBarrage.effectPrefab, base.gameObject, this.muzzleString, false);

                if (base.isAuthority)
                {
                    float damage = SuperShotgun.damageCoefficient * this.damageStat;

                    GameObject tracerEffect = EnforcerPlugin.EnforcerPlugin.bulletTracer;

                    if (base.characterBody.skinIndex == EnforcerPlugin.EnforcerPlugin.stormtrooperIndex) tracerEffect = EnforcerPlugin.EnforcerPlugin.laserTracer;

                    Ray aimRay = base.GetAimRay();

                    new BulletAttack
                    {
                        bulletCount = (uint)projectileCount,
                        aimVector = aimRay.direction,
                        origin = aimRay.origin,
                        damage = damage,
                        damageColorIndex = DamageColorIndex.Default,
                        damageType = DamageType.Generic,
                        falloffModel = BulletAttack.FalloffModel.Buckshot,
                        maxDistance = 80,
                        force = SuperShotgun.bulletForce,
                        hitMask = LayerIndex.CommonMasks.bullet,
                        minSpread = 0,
                        maxSpread = 15f,
                        isCrit = isCrit,
                        owner = base.gameObject,
                        muzzleName = muzzleString,
                        smartCollision = false,
                        procChainMask = default(ProcChainMask),
                        procCoefficient = SuperShotgun.procCoefficient,
                        radius = 0.5f,
                        sniper = false,
                        stopperMask = LayerIndex.CommonMasks.bullet,
                        weapon = null,
                        tracerEffectPrefab = tracerEffect,
                        spreadPitchScale = 0.5f,
                        spreadYawScale = 0.5f,
                        queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                        hitEffectPrefab = ClayBruiser.Weapon.MinigunFire.bulletHitEffectPrefab,
                        HitEffectNormal = ClayBruiser.Weapon.MinigunFire.bulletHitEffectNormal
                    }.Fire();
                }
            }
        }
    }
}