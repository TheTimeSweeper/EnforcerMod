using RoR2;
using UnityEngine;
using EntityStates.ClayBruiser.Weapon;

namespace EntityStates.Enforcer
{
    public class FireAssaultRifle : AssaultRifleState
    {
        public static float damageCoefficient = 0.5f;
        public static float procCoefficient = 0.7f;
        public static float bulletForce = 5f;
        public static float recoilAmplitude = 0.9f;
        public static float baseFireInterval = 0.2f;
        public static int baseBulletCount = 2;
        public static float bulletRange = 128f;
        public static float bulletRadius = 0.1f;
        public static float minSpread = 0;
        public static float maxSpread = 10;

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
            }

            this.baseFireRate = 1f / FireAssaultRifle.baseFireInterval;
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

            if (this.attackSpeedStat >= 20)
            {
                soundString = EnforcerPlugin.Sounds.FireAssaultRifleFast;
            }
            else
            {
                soundString = EnforcerPlugin.Sounds.FireAssaultRifleSlow;
            }

            Util.PlaySound(soundString, base.gameObject);

            if (base.isAuthority)
            {
                this.OnFireAuthority();
            }
        }

        private void OnFireAuthority()
        {
            this.isCrit = this.RollCrit();

            base.AddRecoil(-0.5f * FireAssaultRifle.recoilAmplitude, -0.5f * FireAssaultRifle.recoilAmplitude, -0.5f * FireAssaultRifle.recoilAmplitude, 0.5f * FireAssaultRifle.recoilAmplitude);

            float damage = FireAssaultRifle.damageCoefficient * this.damageStat;
            float force = FireAssaultRifle.bulletForce / this.baseBulletsPerSecond;

            Ray aimRay = base.GetAimRay();

            new BulletAttack
            {
                bulletCount = (uint)FireAssaultRifle.baseBulletCount,
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
                muzzleName = FireAssaultRifle.muzzleName,
                smartCollision = false,
                procChainMask = default(ProcChainMask),
                procCoefficient = FireAssaultRifle.procCoefficient,
                radius = FireAssaultRifle.bulletRadius,
                sniper = false,
                stopperMask = LayerIndex.CommonMasks.bullet,
                weapon = null,
                tracerEffectPrefab = MinigunFire.bulletTracerEffectPrefab,
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

            this.baseFireRate = (1f / (FireAssaultRifle.baseFireInterval * 1f));
            this.baseBulletsPerSecond = ((float)FireAssaultRifle.baseBulletCount * 1f) * this.baseFireRate * 1f;

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
        public static float baseDuration = 0.2f;

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
            this.muzzleTransform = base.FindModelChild(AssaultRifleState.muzzleName);
            this.animator = base.GetModelAnimator();

            this.animator.SetBool("gunUp", true);
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

            this.animator.SetBool("gunUp", false);
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

    //don't use this shit
    public class AssaultRifle : BaseSkillState
    {
        public static GameObject bulletTracerEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/Tracers/TracerClayBruiserMinigun");

        public static float damageCoefficient = 0.3f;
        public static float procCoefficient = 0.6f;
        public static float bulletForce = 20f;
        public static float baseDuration = 0.3f; // the base skill duration
        public static float baseShieldDuration = 0.28f; // the duration used while shield is active
        public static float earlyExitDuration = 0.1f;
        public static int projectileCount = 1;
        public float bulletRecoil = 0.5f;
        public static float beefDurationNoShield = 0.0f;
        public static float beefDurationShield = 0.05f;

        private float attackStopDuration;
        private float duration;
        private float fireDuration;
        private float exitDuration;
        private bool hasFired;
        private Animator animator;
        private string muzzleString;

        public override void OnEnter()
        {
            base.OnEnter();
            base.characterBody.SetAimTimer(0.5f);
            this.animator = base.GetModelAnimator();
            this.muzzleString = "RifleMuzzle";
            this.hasFired = false;
            this.exitDuration = AssaultRifle.earlyExitDuration / this.attackSpeedStat;

            if (base.characterBody.GetComponent<ShieldComponent>().isShielding)
            {
                this.duration = AssaultRifle.baseShieldDuration / this.attackSpeedStat;
                this.attackStopDuration = AssaultRifle.beefDurationShield / this.attackSpeedStat;
            }
            else
            {
                this.duration = AssaultRifle.baseDuration / this.attackSpeedStat;
                this.attackStopDuration = AssaultRifle.beefDurationNoShield / this.attackSpeedStat;
                //base.PlayAnimation("Gesture, Override", "FireShotgun", "FireShotgun.playbackRate", this.duration);
            }

            this.fireDuration = 0.25f * this.duration;

            this.setGunAnimation(true);
        }

        private void setGunAnimation(bool setting)
        {
            this.animator.SetBool("gunUp", setting);
        }

        public override void OnExit()
        {
            base.OnExit();

            this.setGunAnimation(false);
        }

        private void FireBullet()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                string soundString = "";

                bool isCrit = base.RollCrit();

                soundString = ClayBruiser.Weapon.MinigunFire.fireSound;

                Util.PlaySound(soundString, base.gameObject);

                base.AddRecoil(-2f * this.bulletRecoil, -3f * this.bulletRecoil, -1f * this.bulletRecoil, 1f * this.bulletRecoil);
                base.characterBody.AddSpreadBloom(0.33f * this.bulletRecoil);
                EffectManager.SimpleMuzzleFlash(Commando.CommandoWeapon.FirePistol.effectPrefab, base.gameObject, this.muzzleString, false);

                if (base.isAuthority)
                {
                    float damage = AssaultRifle.damageCoefficient * this.damageStat;

                    Ray aimRay = base.GetAimRay();

                    new BulletAttack
                    {
                        bulletCount = (uint)projectileCount,
                        aimVector = aimRay.direction,
                        origin = aimRay.origin,
                        damage = damage,
                        damageColorIndex = DamageColorIndex.Default,
                        damageType = DamageType.Generic,
                        falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                        maxDistance = 88,
                        force = AssaultRifle.bulletForce,
                        hitMask = LayerIndex.CommonMasks.bullet,
                        minSpread = 0,
                        maxSpread = 6f,
                        isCrit = isCrit,
                        owner = base.gameObject,
                        muzzleName = muzzleString,
                        smartCollision = false,
                        procChainMask = default(ProcChainMask),
                        procCoefficient = AssaultRifle.procCoefficient,
                        radius = 0.2f,
                        sniper = false,
                        stopperMask = LayerIndex.CommonMasks.bullet,
                        weapon = null,
                        tracerEffectPrefab = AssaultRifle.bulletTracerEffectPrefab,
                        spreadPitchScale = 0.5f,
                        spreadYawScale = 0.5f,
                        queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                        hitEffectPrefab = Commando.CommandoWeapon.FirePistol.hitEffectPrefab,
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

            if (base.fixedAge >= this.duration - this.exitDuration)
            {
                if (base.inputBank)
                {
                    if (base.inputBank.skill1.down)
                    {
                        this.outer.SetNextState(new AssaultRifle());
                    }
                }
            }

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                setGunAnimation(false);
                this.outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}