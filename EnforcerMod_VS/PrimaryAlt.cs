using RoR2;
using UnityEngine;
using EntityStates.ClayBruiser.Weapon;

namespace EntityStates.Enforcer
{
    public class FireAssaultRifle : AssaultRifleState
    {
        public static float damageCoefficient = 0.7f;
        public static float procCoefficient = 0.7f;
        public static float bulletForce = 5f;
        public static float recoilAmplitude = 1.1f;
        public static float baseFireInterval = 0.18f;
        public static int baseBulletCount = 1;
        public static float bulletRange = 128f;
        public static float bulletRadius = 0.1f;
        public static float minSpread = 0;
        public static float maxSpread = 7;

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

            this.isCrit = this.RollCrit();

            base.AddRecoil(-0.5f * FireAssaultRifle.recoilAmplitude, -0.5f * FireAssaultRifle.recoilAmplitude, -0.5f * FireAssaultRifle.recoilAmplitude, 0.5f * FireAssaultRifle.recoilAmplitude);

            float damage = FireAssaultRifle.damageCoefficient * this.damageStat;
            float force = FireAssaultRifle.bulletForce / this.baseBulletsPerSecond;

            //unique tracer for stormtrooper skin
            GameObject tracerEffect = EnforcerPlugin.EnforcerPlugin.bulletTracer;

            if (base.characterBody.skinIndex == EnforcerPlugin.EnforcerPlugin.stormtrooperIndex) tracerEffect = EnforcerPlugin.EnforcerPlugin.laserTracer;

            Ray aimRay = base.GetAimRay();

            string muzzleString = FireAssaultRifle.muzzleName;
            if (base.characterBody.skinIndex == EnforcerPlugin.EnforcerPlugin.stormtrooperIndex) muzzleString = "BlasterRifleMuzzle";

            int bullets = FireAssaultRifle.baseBulletCount;
            if (base.HasBuff(EnforcerPlugin.EnforcerPlugin.jackBoots)) bullets++;

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
                procCoefficient = FireAssaultRifle.procCoefficient,
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
                this.outer.SetNextState(new AssaultRifleExit());
                return;
            }
        }


    }

    public class AssaultRifleExit : AssaultRifleState
    {
        public static float baseDuration = 0.1f;

        private float duration;
        public override void OnEnter()
        {
            base.OnEnter();

            this.duration = AssaultRifleExit.baseDuration / this.attackSpeedStat;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
            }
        }
    }

    public class AssaultRifleState : BaseState
    {
        public static string muzzleName = "RifleMuzzle";

        protected Transform muzzleTransform;
        protected Animator animator;

        public override void OnEnter()
        {
            base.OnEnter();
            string muzzleString = FireAssaultRifle.muzzleName;
            if (base.characterBody.skinIndex == 3) muzzleString = "BlasterRifleMuzzle";
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