using RoR2;
using UnityEngine;
using EntityStates.ClayBruiser.Weapon;

namespace EntityStates.Enforcer
{
    public class FireAssaultRifle : AssaultRifleState
    {
        public static float damageCoefficient = 0.65f;
        public static float procCoefficient = 0.5f;
        public static float shieldProcCoefficient = 0.25f;
        public static float bulletForce = 5f;
        public static float recoilAmplitude = 1.25f;
        public static float shieldRecoilAmplitude = 0.2f;
        public static float spreadBloom = 0.075f;
        public static float shieldSpreadBloom = 0.025f;
        public static float baseFireInterval = 0.18f;
        public static int baseBulletCount = 1;
        public static float bulletRange = 256;
        public static float bulletRadius = 0.1f;
        public static float minSpread = 0;
        public static float maxSpread = 6.5f;

        public static GameObject bulletTracer = Resources.Load<GameObject>("Prefabs/Effects/Tracers/TracerCommandoDefault");

        private float fireTimer;
        private Transform muzzleVfxTransform;
        private float baseFireRate;
        private float baseBulletsPerSecond;
        private bool isCrit;

        public override void OnEnter()
        {
            base.OnEnter();

            if (this.muzzleTransform && MinigunFire.muzzleVfxPrefab)
            {
                this.muzzleVfxTransform = Object.Instantiate<GameObject>(MinigunFire.muzzleVfxPrefab, this.muzzleTransform).transform;

                if (this.muzzleVfxTransform.Find("Ring, Dark")) Destroy(this.muzzleVfxTransform.Find("Ring, Dark").gameObject);
                if (this.muzzleVfxTransform.Find("Ray")) Destroy(this.muzzleVfxTransform.Find("Ray").gameObject);

                this.muzzleTransform.transform.localScale *= 0.35f;
                this.muzzleTransform.GetComponentInChildren<Light>().range *= 0.25f;
            }

            this.UpdateFireRate();
        }

        private void UpdateFireRate()
        {
            float fireInterval = FireAssaultRifle.baseFireInterval;

            this.baseFireRate = 1f / fireInterval;
            this.baseBulletsPerSecond = ((float)FireAssaultRifle.baseBulletCount * 2f) * this.baseFireRate;
        }

        public override void OnExit()
        {
            if (this.muzzleVfxTransform)
            {
                EntityState.Destroy(this.muzzleVfxTransform.gameObject);
                this.muzzleVfxTransform = null;
            }

            base.OnExit();
        }

        private void OnFireShared()
        {
            string soundString = "";

            if (EnforcerMain.shotgunToggle)
            {
                soundString = EnforcerPlugin.Sounds.FireAssaultRifleFast;
            }
            else
            {
                soundString = EnforcerPlugin.Sounds.FireAssaultRifleSlow;
            }

            if (base.characterBody.skinIndex == EnforcerPlugin.EnforcerPlugin.stormtrooperIndex) soundString = EnforcerPlugin.Sounds.FireBlasterRifle;

            Util.PlayScaledSound(soundString, base.gameObject, this.attackSpeedStat);

            if (base.isAuthority)
            {
                this.OnFireAuthority();
            }
        }

        private void OnFireAuthority()
        {
            base.PlayAnimation("RightArm, Override", "FireShotgun");

            this.critStat = base.characterBody.crit;
            this.isCrit = this.RollCrit();

            float bloom = FireAssaultRifle.spreadBloom;
            float recoil = FireAssaultRifle.recoilAmplitude;
            if (base.characterBody.HasBuff(EnforcerPlugin.EnforcerPlugin.jackBoots))
            {
                bloom = FireAssaultRifle.shieldSpreadBloom;
                recoil = FireAssaultRifle.shieldRecoilAmplitude;
            }

            base.AddRecoil(-0.5f * FireAssaultRifle.recoilAmplitude, -0.5f * FireAssaultRifle.recoilAmplitude, -0.5f * FireAssaultRifle.recoilAmplitude, 0.5f * FireAssaultRifle.recoilAmplitude);
            base.characterBody.AddSpreadBloom(FireAssaultRifle.spreadBloom);

            float damage = FireAssaultRifle.damageCoefficient * this.damageStat;
            float force = FireAssaultRifle.bulletForce / this.baseBulletsPerSecond;

            //unique tracer for stormtrooper skin
            GameObject tracerEffect = FireAssaultRifle.bulletTracer;

            if (base.characterBody.skinIndex == EnforcerPlugin.EnforcerPlugin.stormtrooperIndex) tracerEffect = EnforcerPlugin.EnforcerPlugin.laserTracer;

            Ray aimRay = base.GetAimRay();

            string muzzleString = FireAssaultRifle.muzzleName;
            if (base.characterBody.skinIndex == EnforcerPlugin.EnforcerPlugin.stormtrooperIndex) muzzleString = "BlasterRifleMuzzle";

            int bullets = FireAssaultRifle.baseBulletCount;
            float procCoeff = FireAssaultRifle.procCoefficient;

            if (base.HasBuff(EnforcerPlugin.EnforcerPlugin.jackBoots))
            {
                bullets++;
                procCoeff = FireAssaultRifle.shieldProcCoefficient;
            }

            new BulletAttack
            {
                bulletCount = (uint)bullets,
                aimVector = aimRay.direction,
                origin = aimRay.origin,
                damage = damage,
                damageColorIndex = DamageColorIndex.Default,
                damageType = DamageType.Generic,
                falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                maxDistance = FireAssaultRifle.bulletRange,
                force = force,
                hitMask = LayerIndex.CommonMasks.bullet,
                minSpread = FireAssaultRifle.minSpread,
                maxSpread = FireAssaultRifle.maxSpread,
                isCrit = this.isCrit,
                owner = base.gameObject,
                muzzleName = muzzleString,
                smartCollision = false,
                procChainMask = default(ProcChainMask),
                procCoefficient = procCoeff,
                radius = FireAssaultRifle.bulletRadius,
                sniper = false,
                stopperMask = LayerIndex.CommonMasks.bullet,
                weapon = null,
                tracerEffectPrefab = tracerEffect,
                spreadPitchScale = 0.35f,
                spreadYawScale = 0.35f,
                queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                hitEffectPrefab = MinigunFire.bulletHitEffectPrefab,
                HitEffectNormal = MinigunFire.bulletHitEffectNormal
            }.Fire();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            this.UpdateFireRate();

            this.fireTimer -= Time.fixedDeltaTime;

            if (this.fireTimer <= 0f)
            {
                this.attackSpeedStat = this.characterBody.attackSpeed;

                float num = FireAssaultRifle.baseFireInterval / this.attackSpeedStat;

                this.fireTimer += num;
                this.OnFireShared();
            }

            if (base.isAuthority && !base.skillButtonState.down)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }
    }

    public class AssaultRifleState : BaseSkillState
    {
        public static string muzzleName = "RifleMuzzle";

        protected Transform muzzleTransform;
        protected Animator animator;

        public override void OnEnter()
        {
            base.OnEnter();
            string muzzleString = FireAssaultRifle.muzzleName;
            if (base.characterBody.skinIndex == EnforcerPlugin.EnforcerPlugin.stormtrooperIndex) muzzleString = "BlasterRifleMuzzle";
            this.muzzleTransform = base.FindModelChild(muzzleString);
            this.animator = base.GetModelAnimator();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            base.StartAimMode(0.5f, false);
            base.characterBody.isSprinting = false;
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        protected ref InputBankTest.ButtonState skillButtonState
        {
            get
            {
                return ref base.inputBank.skill1;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}