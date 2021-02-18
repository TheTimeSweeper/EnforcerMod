using RoR2;
using UnityEngine;

namespace EntityStates.Nemforcer
{
    public class HeatCrash : BaseSkillState
    {
        public static float jumpDuration = 1.2f;
        public static float dropForce = 80f;

        public static float slamRadius = 15f;
        public static float slamDamageCoefficient = 24f;
        public static float slamProcCoefficient = 1f;
        public static float slamForce = 5000f;

        private bool hasDropped;
        private Vector3 flyVector = Vector3.zero;
        private Transform modelTransform;
        private Transform slamIndicatorInstance;
        private Transform slamCenterIndicatorInstance;
        private Ray downRay;

        public override void OnEnter()
        {
            base.OnEnter();
            this.modelTransform = base.GetModelTransform();
            this.flyVector = Vector3.up;
            this.hasDropped = false;

            base.PlayAnimation("FullBody, Override", "HeatCrash", "HighJump.playbackRate", HeatCrash.jumpDuration);
            Util.PlaySound(Croco.Leap.leapSoundString, base.gameObject);

            base.characterMotor.Motor.ForceUnground();
            base.characterMotor.velocity = Vector3.zero;

            base.characterBody.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
        }

        public override void Update()
        {
            base.Update();

            if (this.slamIndicatorInstance) this.UpdateSlamIndicator();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!this.hasDropped)
            {
                base.characterMotor.rootMotion += this.flyVector * ((0.8f * this.moveSpeedStat) * Mage.FlyUpState.speedCoefficientCurve.Evaluate(base.fixedAge / HeatCrash.jumpDuration) * Time.fixedDeltaTime);
                base.characterMotor.velocity.y = 0f;
            }

            if (base.fixedAge >= (0.25f * HeatCrash.jumpDuration) && !this.slamIndicatorInstance)
            {
                this.CreateIndicator();
            }

            if (base.fixedAge >= HeatCrash.jumpDuration && !this.hasDropped)
            {
                this.StartDrop();
            }

            if (this.hasDropped && base.isAuthority && !base.characterMotor.disableAirControlUntilCollision)
            {
                this.LandingImpact();
                this.outer.SetNextStateToMain();
            }
        }

        private void StartDrop()
        {
            this.hasDropped = true;

            base.characterMotor.disableAirControlUntilCollision = true;
            base.characterMotor.velocity.y = -HeatCrash.dropForce;

            base.PlayAnimation("FullBody, Override", "HeatCrashSlam", "HighJump.playbackRate", 0.2f);
        }

        private void CreateIndicator()
        {
            if (Huntress.ArrowRain.areaIndicatorPrefab)
            {
                this.downRay = new Ray
                {
                    direction = Vector3.down,
                    origin = base.transform.position
                };

                this.slamIndicatorInstance = UnityEngine.Object.Instantiate<GameObject>(Huntress.ArrowRain.areaIndicatorPrefab).transform;
                this.slamIndicatorInstance.localScale = Vector3.one * HeatCrash.slamRadius;

                this.slamCenterIndicatorInstance = UnityEngine.Object.Instantiate<GameObject>(Huntress.ArrowRain.areaIndicatorPrefab).transform;
                this.slamCenterIndicatorInstance.localScale = (Vector3.one * HeatCrash.slamRadius) / 3f;
            }
        }

        private void LandingImpact()
        {
            base.characterMotor.velocity *= 0.1f;

            BlastAttack blastAttack = new BlastAttack();
            blastAttack.radius = SuperDededeJump.slamRadius;
            blastAttack.procCoefficient = SuperDededeJump.slamProcCoefficient;
            blastAttack.position = base.characterBody.footPosition;
            blastAttack.attacker = base.gameObject;
            blastAttack.crit = base.RollCrit();
            blastAttack.baseDamage = base.characterBody.damage * SuperDededeJump.slamDamageCoefficient;
            blastAttack.falloffModel = BlastAttack.FalloffModel.SweetSpot;
            blastAttack.baseForce = SuperDededeJump.slamForce;
            blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
            blastAttack.damageType = DamageType.Stun1s;
            blastAttack.attackerFiltering = AttackerFiltering.NeverHit;
            blastAttack.Fire();

            AkSoundEngine.SetRTPCValue("M2_Charge", 100f);
            Util.PlaySound(EnforcerPlugin.Sounds.NemesisSmash, base.gameObject);

            for (int i = 0; i <= 8; i += 1)
            {
                Vector3 effectPosition = base.characterBody.footPosition + (UnityEngine.Random.insideUnitSphere * 8f);
                effectPosition.y = base.characterBody.footPosition.y;
                EffectManager.SpawnEffect(EntityStates.LemurianBruiserMonster.SpawnState.spawnEffectPrefab, new EffectData
                {
                    origin = effectPosition,
                    scale = 4f
                }, true);
            }
        }

        private void UpdateSlamIndicator()
        {
            if (this.slamIndicatorInstance)
            {
                float maxDistance = 250f;

                this.downRay = new Ray
                {
                    direction = Vector3.down,
                    origin = base.transform.position
                };

                RaycastHit raycastHit;
                if (Physics.Raycast(this.downRay, out raycastHit, maxDistance, LayerIndex.world.mask))
                {
                    this.slamIndicatorInstance.transform.position = raycastHit.point;
                    this.slamIndicatorInstance.transform.up = raycastHit.normal;

                    this.slamCenterIndicatorInstance.transform.position = raycastHit.point;
                    this.slamCenterIndicatorInstance.transform.up = raycastHit.normal;
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            if (this.slamIndicatorInstance) EntityState.Destroy(this.slamIndicatorInstance.gameObject);
            if (this.slamCenterIndicatorInstance) EntityState.Destroy(this.slamCenterIndicatorInstance.gameObject);

            base.PlayAnimation("FullBody, Override", "BufferEmpty");

            base.characterBody.bodyFlags &= ~CharacterBody.BodyFlags.IgnoreFallDamage;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}