using RoR2;
using UnityEngine;
using System.Linq;
using Enforcer.Nemesis;
using UnityEngine.Networking;
using Modules;
using EnforcerPlugin;
using BepInEx.Configuration;

namespace EntityStates.Nemforcer {
    public class HeatCrash : BaseSkillState
    {
        public static float jumpDuration = 1.2f;
        public static float dropForce = 80f;

        public static float slamRadius = 15f;
        public static float slamDamageCoefficient = 24f;
        public static float slamProcCoefficient = 1f;
        public static float slamForce = 5000f;
        public static ConfigEntry<bool> allowChampions;

        private bool hasDropped;
        private Vector3 flyVector = Vector3.zero;
        private Transform modelTransform;
        private Transform slamIndicatorInstance;
        private Transform slamCenterIndicatorInstance;
        private Ray downRay;
        private NemforcerGrabController grabController;
        private float jankEndTime = -1;

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

            base.gameObject.layer = LayerIndex.fakeActor.intVal;
            base.characterMotor.Motor.RebuildCollidableLayers();
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
                base.characterMotor.rootMotion += this.flyVector * ((0.6f * this.moveSpeedStat) * Mage.FlyUpState.speedCoefficientCurve.Evaluate(base.fixedAge / HeatCrash.jumpDuration) * Time.deltaTime);
                base.characterMotor.velocity.y = 0f;

                this.AttemptGrab(5f);
            }

            if (base.fixedAge >= (0.25f * HeatCrash.jumpDuration) && !this.slamIndicatorInstance)
            {
                this.CreateIndicator();
            }
            
            if (base.fixedAge >= HeatCrash.jumpDuration && !this.hasDropped)
            {
                this.StartDrop();
            }

            if (this.hasDropped && base.isAuthority && !base.characterMotor.disableAirControlUntilCollision && jankEndTime <0)
            {
                this.LandingImpact();
                jankEndTime = fixedAge + 0.2f;
            }

            if(jankEndTime > 0 && fixedAge > jankEndTime)
            {
                this.outer.SetNextStateToMain();
            }
        }

        private void StartDrop()
        {
            this.hasDropped = true;

            base.characterMotor.disableAirControlUntilCollision = true;
            base.characterMotor.velocity.y = -HeatCrash.dropForce;

            base.PlayAnimation("FullBody, Override", "HeatCrashSlam", "HighJump.playbackRate", 0.2f);

            this.AttemptGrab(10f);
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
            if (this.grabController) this.grabController.Release();

            SmallHop(characterMotor, 10);

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
            blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;
            var result = blastAttack.Fire();
            if (result.hitCount > 0 && NemforcerPlugin.reworkPassive.Value) base.characterBody.AddTimedBuffAuthority(Modules.Buffs.nemforcerRegenBuff.buffIndex, NemforcerPlugin.nemforcerRegenBuffDuration);

            AkSoundEngine.SetRTPCValue("M2_Charge", 100f);
            Util.PlaySound(Sounds.NemesisSmash, base.gameObject);

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

            if (this.grabController) this.grabController.Release();

            if (this.slamIndicatorInstance) EntityState.Destroy(this.slamIndicatorInstance.gameObject);
            if (this.slamCenterIndicatorInstance) EntityState.Destroy(this.slamCenterIndicatorInstance.gameObject);

            base.PlayAnimation("FullBody, Override", "BufferEmpty");

            base.characterBody.bodyFlags &= ~CharacterBody.BodyFlags.IgnoreFallDamage;

            if (NetworkServer.active && base.characterBody.HasBuff(RoR2Content.Buffs.HiddenInvincibility)) base.characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);

            base.gameObject.layer = LayerIndex.defaultLayer.intVal;
            base.characterMotor.Motor.RebuildCollidableLayers();
        }

        private void AttemptGrab(float grabRadius)
        {
            if (this.grabController) return;

            Ray aimRay = base.GetAimRay();

            BullseyeSearch search = new BullseyeSearch
            {
                teamMaskFilter = TeamMask.GetEnemyTeams(base.GetTeam()),
                filterByLoS = false,
                searchOrigin = base.transform.position,
                searchDirection = Random.onUnitSphere,
                sortMode = BullseyeSearch.SortMode.Distance,
                maxDistanceFilter = grabRadius,
                maxAngleFilter = 360f
            };

            search.RefreshCandidates();
            search.FilterOutGameObject(base.gameObject);

            var results = search.GetResults();
            if (!allowChampions.Value)
            {
                results = results.Where(hurtBox =>
                {
                    return hurtBox.healthComponent && hurtBox.healthComponent.body && !hurtBox.healthComponent.body.isChampion;
                });
            }

            HurtBox target = results.FirstOrDefault<HurtBox>();
            if (target)
            {
                if (target.healthComponent && target.healthComponent.body)
                {
                    if (BodyMeetsGrabConditions(target.healthComponent.body))
                    {
                        this.grabController = target.healthComponent.body.gameObject.AddComponent<NemforcerGrabController>();
                        this.grabController.pivotTransform = this.FindModelChild("HandL");
                    }

                    if (NetworkServer.active)
                    {
                        base.characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);
                    }
                }
            }
        }

        private bool BodyMeetsGrabConditions(CharacterBody targetBody)
        {
            bool meetsConditions = true;

            //if (targetBody.hullClassification == HullClassification.BeetleQueen) meetsConditions = false;

            return meetsConditions;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}