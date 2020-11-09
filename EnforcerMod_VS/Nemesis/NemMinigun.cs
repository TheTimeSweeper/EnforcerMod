using RoR2;
using UnityEngine;
using EntityStates.ClayBruiser.Weapon;
using UnityEngine.Networking;

namespace EntityStates.Nemforcer
{
    public class NemMinigunFire : NemMinigunState
    {
        public static float baseDamageCoefficient = 0.69f;
        public static float baseForce = 0.5f;
        public static float baseProcCoefficient = 0.5f;
        public static float recoilAmplitude = 2f;
        public static float minFireRate = 0.75f;
        public static float maxFireRate = 1.35f;
        public static float fireRateGrowth = 0.01f;

        private float fireTimer;
        private Transform muzzleVfxTransform;
        private float baseFireRate;
        private float baseBulletsPerSecond;
        private Run.FixedTimeStamp critEndTime;
        private Run.FixedTimeStamp lastCritCheck;
        private float currentFireRate;

        public override void OnEnter()
        {
            base.OnEnter();
            if (this.muzzleTransform && MinigunFire.muzzleVfxPrefab)
            {
                this.muzzleVfxTransform = UnityEngine.Object.Instantiate<GameObject>(MinigunFire.muzzleVfxPrefab, this.muzzleTransform).transform;
            }

            this.baseFireRate = 1f / MinigunFire.baseFireInterval;
            this.baseBulletsPerSecond = (float)MinigunFire.baseBulletCount * this.baseFireRate;

            currentFireRate = minFireRate;

            this.critEndTime = Run.FixedTimeStamp.negativeInfinity;
            this.lastCritCheck = Run.FixedTimeStamp.negativeInfinity;
            Util.PlaySound(MinigunFire.startSound, base.gameObject);
        }

        private void UpdateCrits()
        {
            this.critStat = base.characterBody.crit;

            if (this.lastCritCheck.timeSince >= 0.2f)
            {
                this.lastCritCheck = Run.FixedTimeStamp.now;
                if (base.RollCrit())
                {
                    this.critEndTime = Run.FixedTimeStamp.now + 0.4f;
                }
            }
        }

        public override void OnExit()
        {
            Util.PlaySound(MinigunFire.endSound, base.gameObject);
            if (this.muzzleVfxTransform)
            {
                EntityState.Destroy(this.muzzleVfxTransform.gameObject);
                this.muzzleVfxTransform = null;
            }

            base.OnExit();
        }

        private void OnFireShared()
        {
            Util.PlaySound(MinigunFire.fireSound, base.gameObject);

            if (base.isAuthority)
            {
                this.OnFireAuthority();
            }
        }

        private void OnFireAuthority()
        {
            this.UpdateCrits();
            bool isCrit = !this.critEndTime.hasPassed;

            base.characterBody.AddSpreadBloom(0.25f);
            base.AddRecoil(-0.6f * NemMinigunFire.recoilAmplitude, -0.8f * NemMinigunFire.recoilAmplitude, -0.3f * NemMinigunFire.recoilAmplitude, 0.3f * NemMinigunFire.recoilAmplitude);

            currentFireRate = Mathf.Clamp(currentFireRate + fireRateGrowth, minFireRate, maxFireRate);

            float damage = NemMinigunFire.baseDamageCoefficient * this.damageStat;
            float force = NemMinigunFire.baseForce;
            float procCoefficient = NemMinigunFire.baseProcCoefficient;

            Ray aimRay = base.GetAimRay();

            new BulletAttack
            {
                bulletCount = (uint)MinigunFire.baseBulletCount,
                aimVector = aimRay.direction,
                origin = aimRay.origin,
                damage = damage,
                damageColorIndex = DamageColorIndex.Default,
                damageType = DamageType.Generic,
                falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                maxDistance = MinigunFire.bulletMaxDistance,
                force = force,
                hitMask = LayerIndex.CommonMasks.bullet,
                minSpread = MinigunFire.bulletMinSpread,
                maxSpread = MinigunFire.bulletMaxSpread * 1.5f,
                isCrit = isCrit,
                owner = base.gameObject,
                muzzleName = NemMinigunState.muzzleName,
                smartCollision = false,
                procChainMask = default(ProcChainMask),
                procCoefficient = procCoefficient,
                radius = 0f,
                sniper = false,
                stopperMask = LayerIndex.CommonMasks.bullet,
                weapon = null,
                tracerEffectPrefab = MinigunFire.bulletTracerEffectPrefab,
                spreadPitchScale = 1f,
                spreadYawScale = 1f,
                queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                hitEffectPrefab = MinigunFire.bulletHitEffectPrefab,
                HitEffectNormal = MinigunFire.bulletHitEffectNormal
            }.Fire();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            this.baseFireRate = 1f / MinigunFire.baseFireInterval;
            this.baseBulletsPerSecond = (float)MinigunFire.baseBulletCount * this.baseFireRate;

            this.fireTimer -= Time.fixedDeltaTime;

            float rateLerp = Mathf.InverseLerp(minFireRate, maxFireRate, currentFireRate);
            float rate = Mathf.Lerp(0.5f, 2, rateLerp);
            animator.SetFloat("Minigun.spinSpeed", rate);

            if (base.characterMotor) {
                //animator.speed = 0;
                base.characterMotor.moveDirection /= 2;
            }

            if (this.fireTimer <= 0f)
            {
                this.attackSpeedStat = this.characterBody.attackSpeed;

                float num = (MinigunFire.baseFireInterval / this.attackSpeedStat) / currentFireRate;
                this.fireTimer += num;

                this.OnFireShared();
            }

            if (base.isAuthority && !base.skillButtonState.down)
            {
                this.outer.SetNextState(new NemMinigunSpinDown());
                return;
            }
        }


    }

    public class NemMinigunSpinDown : NemMinigunState
    {
        private float duration;
        private float spin;
        public override void OnEnter()
        {
            base.OnEnter();

            this.duration = (MinigunSpinDown.baseDuration * 0.25f) / this.attackSpeedStat;
            Util.PlayScaledSound(MinigunSpinDown.sound, base.gameObject, this.attackSpeedStat);

            spin = animator.GetFloat("Minigun.spinSpeed");
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            float spinlerp = Mathf.Lerp(spin, 0, fixedAge / duration);
            animator.SetFloat("Minigun.spinSpeed", spinlerp);

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
            }
        }
    }

    public class NemMinigunSpinUp : NemMinigunState
    {
        private GameObject chargeInstance;
        private float duration;

        public override void OnEnter()
        {
            base.OnEnter();
            base.characterBody.SetSpreadBloom(2f, false);
            base.characterBody.crosshairPrefab = Resources.Load<GameObject>("Prefabs/Crosshair/BanditCrosshair");

            this.duration = MinigunSpinUp.baseDuration / this.attackSpeedStat;
            Util.PlaySound(MinigunSpinUp.sound, base.gameObject);


            base.PlayAnimation("FullBody, Override", "MinigunUp", "ShieldUp.playbackRate", this.duration);

            if (this.muzzleTransform && MinigunSpinUp.chargeEffectPrefab)
            {
                this.chargeInstance = UnityEngine.Object.Instantiate<GameObject>(MinigunSpinUp.chargeEffectPrefab, this.muzzleTransform.position, this.muzzleTransform.rotation);
                this.chargeInstance.transform.parent = this.muzzleTransform;
                ScaleParticleSystemDuration component = this.chargeInstance.GetComponent<ScaleParticleSystemDuration>();
                if (component)
                {
                    component.newDuration = this.duration;
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            float spinlerp = Mathf.Lerp(0, 0.5f, fixedAge / duration);
            animator.SetFloat("Minigun.spinSpeed", spinlerp);

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextState(new NemMinigunFire());
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            if (this.chargeInstance)
            {
                EntityState.Destroy(this.chargeInstance);
            }
        }

    }

    public class NemMinigunState : BaseState
    {
        protected Transform muzzleTransform;
        protected static string muzzleName = "MinigunMuzzle";
        protected Animator animator;

        public override void OnEnter()
        {
            base.OnEnter();
            this.muzzleTransform = base.FindModelChild("MinigunMuzzle");
            animator = base.GetModelAnimator();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
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