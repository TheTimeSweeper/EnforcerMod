using RoR2;
using UnityEngine;
using EntityStates.ClayBruiser.Weapon;
using UnityEngine.Networking;

namespace EntityStates.Nemforcer
{
    public class NemMinigunFire : NemMinigunState
    {
        public static float baseDamageCoefficient = 0.65f;
        public static float baseForce = 15f;
        public static float baseProcCoefficient = 0.3f;
        public static float recoilAmplitude = 2f;
        public static float minFireRate = 0.75f;
        public static float maxFireRate = 1.35f;
        public static float fireRateGrowth = 0.01f;
        public static float selfPushForce = 0.6f;

        private float spreadMod;
        private float fireTimer;
        private Transform muzzleVfxTransform;
        private float baseFireRate;
        private float baseBulletsPerSecond;
        private Run.FixedTimeStamp critEndTime;
        private Run.FixedTimeStamp lastCritCheck;
        private float currentFireRate;

        private uint playID;

        public override void OnEnter()
        {
            base.OnEnter();
            this.spreadMod = 0f;

            if (this.muzzleTransform && MinigunFire.muzzleVfxPrefab)
            {
                this.muzzleVfxTransform = UnityEngine.Object.Instantiate<GameObject>(MinigunFire.muzzleVfxPrefab, this.muzzleTransform).transform;
                if (this.muzzleVfxTransform.Find("Ring, Dark")) Destroy(this.muzzleVfxTransform.Find("Ring, Dark").gameObject);
            }

            this.baseFireRate = 1f / MinigunFire.baseFireInterval;
            this.baseBulletsPerSecond = (float)MinigunFire.baseBulletCount * this.baseFireRate;

            currentFireRate = minFireRate;

            this.critEndTime = Run.FixedTimeStamp.negativeInfinity;
            this.lastCritCheck = Run.FixedTimeStamp.negativeInfinity;

            this.playID = Util.PlaySound(EnforcerPlugin.Sounds.NemesisMinigunShooting, base.gameObject);
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

            AkSoundEngine.SetRTPCValue("Minigun_Shooting", 1);
            if (!this.critEndTime.hasPassed)
            {
                AkSoundEngine.SetRTPCValue("Minigun_Crit", 1);
            }
            else
            {
                AkSoundEngine.SetRTPCValue("Minigun_Crit", 0);
            }
        }

        public override void OnExit()
        {
            if (this.muzzleVfxTransform)
            {
                EntityState.Destroy(this.muzzleVfxTransform.gameObject);
                this.muzzleVfxTransform = null;
            }

            AkSoundEngine.StopPlayingID(this.playID);

            base.OnExit();
        }

        private void OnFireShared()
        {
            base.characterBody.AddSpreadBloom(0.25f);

            if (base.isAuthority)
            {
                this.OnFireAuthority();
            }
        }

        private void OnFireAuthority()
        {
            this.UpdateCrits();
            bool isCrit = !this.critEndTime.hasPassed;

            base.AddRecoil(-0.6f * NemMinigunFire.recoilAmplitude, -0.8f * NemMinigunFire.recoilAmplitude, -0.3f * NemMinigunFire.recoilAmplitude, 0.3f * NemMinigunFire.recoilAmplitude);

            this.currentFireRate = Mathf.Clamp(currentFireRate + fireRateGrowth, minFireRate, maxFireRate);

            this.spreadMod += 0.1f;
            if (this.spreadMod >= 1f) this.spreadMod = 1f;

            float damage = NemMinigunFire.baseDamageCoefficient * this.damageStat;
            float force = NemMinigunFire.baseForce;
            float procCoefficient = NemMinigunFire.baseProcCoefficient;

            Ray aimRay = base.GetAimRay();

            if (!base.characterMotor.isGrounded)
            {
                base.characterMotor.velocity += (NemMinigunFire.selfPushForce * -aimRay.direction);
            }

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
                minSpread = this.spreadMod * MinigunFire.bulletMinSpread,
                maxSpread = this.spreadMod * MinigunFire.bulletMaxSpread * 1.5f,
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
                tracerEffectPrefab = EnforcerPlugin.EnforcerPlugin.minigunTracer,
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
            this.animator.SetFloat("Minigun.spinSpeed", rate);

            if (base.characterMotor)
            {
                //animator.speed = 0;
                base.characterMotor.moveDirection /= 1.5f;
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
        public static float baseDuration = 0.2f;

        private float duration;
        private float spin;
        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = NemMinigunSpinDown.baseDuration / this.attackSpeedStat;
            Util.PlayScaledSound(EnforcerPlugin.Sounds.NemesisMinigunWindDown, base.gameObject, this.attackSpeedStat);

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
        public static float baseDuration = 0.6f;

        private GameObject chargeInstance;
        private float duration;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = NemMinigunSpinUp.baseDuration / this.attackSpeedStat;
            Util.PlayScaledSound(EnforcerPlugin.Sounds.NemesisMinigunWindUp, base.gameObject, this.attackSpeedStat);

            if (this.muzzleTransform && MinigunSpinUp.chargeEffectPrefab)
            {
                this.chargeInstance = UnityEngine.Object.Instantiate<GameObject>(MinigunSpinUp.chargeEffectPrefab, this.muzzleTransform.position, this.muzzleTransform.rotation);
                this.chargeInstance.transform.parent = this.muzzleTransform;
                ScaleParticleSystemDuration component = this.chargeInstance.GetComponent<ScaleParticleSystemDuration>();
                if (component)
                {
                    component.newDuration = this.duration;
                }

                if (this.chargeInstance.transform.Find("Ring, Dark")) Destroy(this.chargeInstance.transform.Find("Ring, Dark").gameObject);
                if (this.chargeInstance.transform.Find("SmokeBillboard")) Destroy(this.chargeInstance.transform.Find("SmokeBillboard").gameObject);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            float spinlerp = Mathf.Lerp(0, 0.5f, base.fixedAge / this.duration);
            this.animator.SetFloat("Minigun.spinSpeed", spinlerp);

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

            if (NetworkServer.active) base.characterBody.AddBuff(EnforcerPlugin.EnforcerPlugin.tempSlowDebuff);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            AkSoundEngine.SetRTPCValue("Minigun_Speed", this.attackSpeedStat);
        }

        public override void OnExit()
        {
            base.OnExit();

            if (NetworkServer.active && base.characterBody.HasBuff(EnforcerPlugin.EnforcerPlugin.tempSlowDebuff)) base.characterBody.RemoveBuff(EnforcerPlugin.EnforcerPlugin.tempSlowDebuff);
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