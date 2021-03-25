using RoR2;
using RoR2.Projectile;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;
using System.Collections;

namespace EntityStates.Enforcer
{
    public class ShieldBash : BaseSkillState
    {
        public static string hitboxString = "ShieldHitbox"; //transform where the hitbox is fired
        public static float baseDuration = 0.8f;
        public static float damageCoefficient = 2.5f;
        public static float procCoefficient = 1f;
        public static float knockbackForce = 0.14f;
        public static float blastRadius = 6f;
        public static float deflectRadius = 3f;
        public static float beefDurationNoShield = 0.4f;
        public static float beefDurationShield = 0.8f;
        public static float recoilAmplitude = 1f;
        public static float parryInterval = 0.12f;

        private float attackStopDuration;
        private float duration;
        private float fireDuration;
        private float deflectDuration;
        private Ray aimRay;
        private BlastAttack blastAttack;
        private ChildLocator childLocator;
        private bool hasFired;
        private bool usingBash;
        private bool hasDeflected;
        private ShieldComponent shieldComponent;

        private Transform _origOrigin;
        private int _parries = 0;

        private List<CharacterBody> victimList = new List<CharacterBody>();

        public override void OnEnter()
        {
            base.OnEnter();

            this.duration = baseDuration / this.attackSpeedStat;
            this.fireDuration = this.duration * 0.15f;
            this.deflectDuration = this.duration * 0.45f;
            this.aimRay = base.GetAimRay();
            this.hasFired = false;
            this.hasDeflected = false;
            this.usingBash = false;
            this.childLocator = base.GetModelTransform().GetComponent<ChildLocator>();
            this.shieldComponent = base.characterBody.GetComponent<ShieldComponent>();

            base.StartAimMode(aimRay, 2f, false);

            //yep cock
            if (base.characterBody.isSprinting)
            {
                this.hasDeflected = true;
                this.usingBash = true;
                this.hasFired = true;
                base.skillLocator.secondary.skillDef.activationStateMachineName = "Body";
                this.outer.SetNextState(new ShoulderBash());
                return;
            }

            _origOrigin = characterBody.aimOriginTransform;

            this.shieldComponent.onLaserHit += EnforcerMain_onLaserHit;

            bool grounded = base.characterMotor.isGrounded;

            if (base.HasBuff(EnforcerPlugin.EnforcerPlugin.jackBoots))
            {
                if (grounded) base.PlayAnimation("FullBody, Override", "ShieldBash", "ShieldBash.playbackRate", this.duration);
                else base.PlayAnimation("Gesture, Override", "Bash", "ShieldBash.playbackRate", this.duration);
                this.attackStopDuration = ShieldBash.beefDurationShield / this.attackSpeedStat;
            }
            else
            {
                if (grounded) base.PlayAnimation("FullBody, Override", "Bash", "ShieldBash.playbackRate", this.duration);
                else base.PlayAnimation("Gesture, Override", "Bash", "ShieldBash.playbackRate", this.duration);
                this.attackStopDuration = ShieldBash.beefDurationNoShield / this.attackSpeedStat;
            }

            Util.PlayScaledSound(EnforcerPlugin.Sounds.ShieldBash, base.gameObject, this.attackSpeedStat);
        }

        private void FireBlast()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                if (base.isAuthority)
                {
                    EffectManager.SimpleMuzzleFlash(EnforcerPlugin.Assets.shieldBashFX, base.gameObject, hitboxString, true);
                    base.AddRecoil(-0.5f * ShieldBash.recoilAmplitude * 3f, -0.5f * ShieldBash.recoilAmplitude * 3f, -0.5f * ShieldBash.recoilAmplitude * 8f, 0.5f * ShieldBash.recoilAmplitude * 3f);

                    Vector3 center = childLocator.FindChild(hitboxString).position;

                    blastAttack = new BlastAttack();
                    blastAttack.radius = ShieldBash.blastRadius;
                    blastAttack.procCoefficient = ShieldBash.procCoefficient;
                    blastAttack.position = center;
                    blastAttack.attacker = base.gameObject;
                    blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
                    blastAttack.baseDamage = base.characterBody.damage * ShieldBash.damageCoefficient;
                    blastAttack.falloffModel = BlastAttack.FalloffModel.None;
                    blastAttack.baseForce = 0f;
                    blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
                    blastAttack.damageType = DamageType.Stun1s;
                    blastAttack.attackerFiltering = AttackerFiltering.NeverHit;
                    blastAttack.impactEffect = BeetleGuardMonster.GroundSlam.hitEffectPrefab.GetComponent<EffectComponent>().effectIndex;

                    blastAttack.Fire();
                }

                if (NetworkServer.active)
                {
                    Vector3 pushForce = ((aimRay.origin + 200 * aimRay.direction) - childLocator.FindChild(hitboxString).position + (75 * Vector3.up)) * ShieldBash.knockbackForce;

                    Collider[] array = Physics.OverlapSphere(childLocator.FindChild(hitboxString).position, ShieldBash.blastRadius, LayerIndex.defaultLayer.mask);
                    for (int i = 0; i < array.Length; i++)
                    {
                        HealthComponent healthComponent = array[i].GetComponent<HealthComponent>();
                        if (healthComponent)
                        {
                            TeamComponent component2 = healthComponent.GetComponent<TeamComponent>();

                            bool enemyTeam = component2.teamIndex != base.teamComponent.teamIndex;
                            bool configKnockbackAllies = EnforcerPlugin.EnforcerPlugin.stupidShieldBash.Value && healthComponent != base.healthComponent;

                            bool redirecting = false;

                            if (enemyTeam || configKnockbackAllies)
                            {
                                Util.PlaySound(EnforcerPlugin.Sounds.BashHitEnemy, healthComponent.gameObject);

                                CharacterBody hitCharacterBody = healthComponent.body;
                                if (hitCharacterBody)
                                {
                                    //TODO: redirecting allies
                                    //CharacterDirection hitCharacterDirection = hitCharacterBody.GetComponent<CharacterDirection>();

                                    //if (hitCharacterDirection) {
                                    //    hitCharacterDirection.forward = aimRay.direction;

                                    //    AimAnimator hitAimAnimator = hitCharacterBody.modelLocator.modelTransform?.GetComponent<AimAnimator>();
                                    //    if (hitAimAnimator) {
                                    //        hitAimAnimator.AimImmediate();
                                    //    }
                                    //}

                                    CharacterMotor hitCharacterMotor = hitCharacterBody.GetComponent<CharacterMotor>();
                                    Rigidbody hitRigidbody = hitCharacterBody.GetComponent<Rigidbody>();
                                    Vector3 force = pushForce;

                                    float mass = 0f;

                                    if (hitCharacterMotor) {

                                        mass = hitCharacterMotor.mass;

                                        //TODO: redirecting allies
                                        //if(hitCharacterMotor.velocity.magnitude > niggaspeed) {
                                        //    redirectBody(hitCharacterMotor);
                                        //    continue;
                                        //}
                                        //float vel = hitCharacterMotor.velocity.magnitude;
                                        //hitCharacterMotor.velocity = aimRay.direction * vel;

                                    } else if (hitRigidbody) {
                                        mass = hitRigidbody.mass;
                                    }

                                    if (mass <= 100f) mass = 100f;
                                    if (EnforcerPlugin.EnforcerPlugin.balancedShieldBash.Value && mass > 500f) mass = 500f; 

                                    force *= mass;

                                    DamageInfo info = new DamageInfo
                                    {
                                        attacker = base.gameObject,
                                        inflictor = base.gameObject,
                                        damage = 0,
                                        damageColorIndex = DamageColorIndex.Default,
                                        damageType = DamageType.Generic,
                                        crit = false,
                                        dotIndex = DotController.DotIndex.None,
                                        force = force,
                                        position = base.transform.position,
                                        procChainMask = default(ProcChainMask),
                                        procCoefficient = 0
                                    };

                                    hitCharacterBody.healthComponent.TakeDamageForce(info, true, true);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void redirectBody(CharacterMotor hitCharacterMotor) {

        }

        public override void OnExit()
        {

            base.OnExit();

            this.shieldComponent.onLaserHit -= EnforcerMain_onLaserHit;
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
                this.FireBlast();
            }

            if (base.fixedAge < this.deflectDuration)
            {
                this.Deflect();

                this.shieldComponent.isDeflecting = true;
            } 
            else 
            {
                this.shieldComponent.isDeflecting = false;

                this.ParryLasers();
            }

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        private void Deflect()
        {
            if (this.usingBash) return;

            Collider[] array = Physics.OverlapSphere(childLocator.FindChild(hitboxString).position, ShieldBash.deflectRadius, LayerIndex.projectile.mask);

            for (int i = 0; i < array.Length; i++)
            {
                ProjectileController pc = array[i].GetComponentInParent<ProjectileController>();
                if (pc)
                {
                    if (pc.owner != gameObject)
                    {
                        Ray aimRay = base.GetAimRay();
                        Vector3 aimSpot = (aimRay.origin + 100 * aimRay.direction) - pc.gameObject.transform.position;

                        pc.owner = gameObject;

                        FireProjectileInfo info = new FireProjectileInfo()
                        {
                            projectilePrefab = pc.gameObject,
                            position = pc.gameObject.transform.position,
                            rotation = base.characterBody.transform.rotation * Quaternion.FromToRotation(new Vector3(0, 0, 1), aimSpot),
                            owner = base.characterBody.gameObject,
                            damage = base.characterBody.damage * 10f,
                            force = 200f,
                            crit = base.RollCrit(),
                            damageColorIndex = DamageColorIndex.Default,
                            target = null,
                            speedOverride = 120f,
                            fuseOverride = -1f
                        };
                        ProjectileManager.instance.FireProjectile(info);

                        Util.PlayScaledSound(EnforcerPlugin.Sounds.BashDeflect, base.gameObject, UnityEngine.Random.Range(0.9f, 1.1f));

                        Destroy(pc.gameObject);

                        if (!this.hasDeflected)
                        {
                            this.hasDeflected = true;

                            if (EnforcerPlugin.EnforcerPlugin.sirenOnDeflect.Value) Util.PlaySound(EnforcerPlugin.Sounds.SirenSpawn, base.gameObject);

                            base.characterBody.GetComponent<EnforcerLightController>().FlashLights(2);
                            base.characterBody.GetComponent<EnforcerLightControllerAlt>().FlashLights(8);
                        }
                    }
                }
            }
        }        

        private void ParryLasers() 
        {
            if (this.usingBash) 
                return;

            if (_parries <= 0)
                return;

            Util.PlayScaledSound(EnforcerPlugin.Sounds.BashDeflect, base.gameObject, UnityEngine.Random.Range(0.9f, 1.1f));

            for (int i = 0; i < _parries; i++)
            {

                this.shieldComponent.drOctagonapus.StartCoroutine(ShootParriedLaser(i * parryInterval));
            }

            _parries = 0;

            if (!this.hasDeflected)
            {
                this.hasDeflected = true;

                if (EnforcerPlugin.EnforcerPlugin.sirenOnDeflect.Value) 
                    Util.PlaySound(EnforcerPlugin.Sounds.SirenSpawn, base.gameObject);

                base.characterBody.GetComponent<EnforcerLightController>().FlashLights(2);
                base.characterBody.GetComponent<EnforcerLightControllerAlt>().FlashLights(8);
            }
        }

        private IEnumerator ShootParriedLaser(float delay)
        {

            yield return new WaitForSeconds(delay);

            Vector3 point = GetAimRay().GetPoint(1000);
            Vector3 laserDirection = point - transform.position;

            GolemMonster.FireLaser fireLaser = new GolemMonster.FireLaser();
            fireLaser.laserDirection = laserDirection;
            this.shieldComponent.drOctagonapus.SetInterruptState(fireLaser, InterruptPriority.Skill);
        }

        private void EnforcerMain_onLaserHit()
        {
            _parries++;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }

    public class ShoulderBash : BaseSkillState
    {
        public static float baseDuration = 0.65f;
        public static float chargeDamageCoefficient = 4.5f;
        public static float knockbackDamageCoefficient = 7f;
        public static float massThresholdForKnockback = 150;
        public static float knockbackForce = 24f;
        public static float smallHopVelocity = 12f;

        public static float initialSpeedCoefficient = 6f;
        public static float finalSpeedCoefficient = 0.1f;

        private float dashSpeed;
        private Vector3 forwardDirection;
        private Vector3 previousPosition;

        private bool shieldCancel;
        private float duration;
        private float hitPauseTimer;
        private OverlapAttack attack;
        private bool inHitPause;
        private List<HealthComponent> victimsStruck = new List<HealthComponent>();

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = ShoulderBash.baseDuration;
            this.shieldCancel = false;

            base.characterBody.GetComponent<EnforcerLightController>().FlashLights(2);
            base.characterBody.GetComponent<EnforcerLightControllerAlt>().FlashLights(6);
            base.characterBody.isSprinting = true;

            Util.PlayScaledSound(Croco.Leap.leapSoundString, base.gameObject, 1.75f);

            if (!base.HasBuff(EnforcerPlugin.EnforcerPlugin.skateboardBuff)) base.PlayAnimation("FullBody, Override", "ShoulderBash");//, "ShoulderBash.playbackRate", this.duration

            if (base.isAuthority && base.inputBank && base.characterDirection)
            {
                this.forwardDirection = ((base.inputBank.moveVector == Vector3.zero) ? base.characterDirection.forward : base.inputBank.moveVector).normalized;
            }

            this.RecalculateSpeed();

            if (base.characterMotor && base.characterDirection)
            {
                base.characterMotor.velocity.y *= 0.2f;
                base.characterMotor.velocity = this.forwardDirection * this.dashSpeed;
            }

            Vector3 b = base.characterMotor ? base.characterMotor.velocity : Vector3.zero;
            this.previousPosition = base.transform.position - b;

            HitBoxGroup hitBoxGroup = null;
            Transform modelTransform = base.GetModelTransform();

            if (modelTransform)
            {
                hitBoxGroup = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == "Charge");
            }

            this.attack = new OverlapAttack();
            this.attack.attacker = base.gameObject;
            this.attack.inflictor = base.gameObject;
            this.attack.teamIndex = base.GetTeam();
            this.attack.damage = ShoulderBash.chargeDamageCoefficient * this.damageStat;
            this.attack.hitEffectPrefab = Loader.SwingChargedFist.overchargeImpactEffectPrefab;
            this.attack.forceVector = Vector3.up * Toolbot.ToolbotDash.upwardForceMagnitude;
            this.attack.pushAwayForce = Toolbot.ToolbotDash.awayForceMagnitude;
            this.attack.hitBoxGroup = hitBoxGroup;
            this.attack.isCrit = base.RollCrit();

            if (base.isAuthority) EffectManager.SimpleMuzzleFlash(EnforcerPlugin.Assets.shoulderBashFX, base.gameObject, "ShieldHitbox", true);
        }

        private void RecalculateSpeed()
        {
            this.dashSpeed = (4 + (0.25f * this.moveSpeedStat)) * Mathf.Lerp(ShoulderBash.initialSpeedCoefficient, ShoulderBash.finalSpeedCoefficient, base.fixedAge / this.duration);
        }

        public override void OnExit()
        {
            if (base.characterBody)
            {
                if (this.shieldCancel) base.characterBody.isSprinting = false;
                else base.characterBody.isSprinting = true;
            }

            if (base.characterMotor) base.characterMotor.disableAirControlUntilCollision = false;// this should be a thing on all movement skills tbh

            if (base.skillLocator) base.skillLocator.secondary.skillDef.activationStateMachineName = "Weapon";

            if (base.cameraTargetParams)
            {
                base.cameraTargetParams.fovOverride = -1f;
            }

            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            base.characterBody.isSprinting = true;

            if (base.fixedAge >= this.duration)
            {
                this.outer.SetNextStateToMain();
                return;
            }

            this.RecalculateSpeed();

            if (base.cameraTargetParams)
            {
                base.cameraTargetParams.fovOverride = Mathf.Lerp(Commando.DodgeState.dodgeFOV, 60f, base.fixedAge / this.duration);
            }

            if (base.isAuthority)
            {
                if (base.skillLocator && base.inputBank)
                {
                    if (base.inputBank.skill4.down && base.fixedAge >= 0.4f * this.duration)
                    {
                        this.shieldCancel = true;
                        base.characterBody.isSprinting = false;
                        base.skillLocator.special.ExecuteIfReady();
                    }
                }

                if (!this.inHitPause)
                {
                    Vector3 normalized = (base.transform.position - this.previousPosition).normalized;

                    if (base.characterDirection)
                    {
                        if (normalized != Vector3.zero)
                        {
                            Vector3 vector = normalized * this.dashSpeed;
                            float d = Mathf.Max(Vector3.Dot(vector, this.forwardDirection), 0f);
                            vector = this.forwardDirection * d;
                            vector.y = base.characterMotor.velocity.y;
                            base.characterMotor.velocity = vector;
                        }

                        base.characterDirection.forward = this.forwardDirection;
                    }

                    this.previousPosition = base.transform.position;

                    this.attack.damage = this.damageStat * ShoulderBash.chargeDamageCoefficient;

                    if (this.attack.Fire(this.victimsStruck))
                    {
                        Util.PlaySound(EnforcerPlugin.Sounds.ShoulderBashHit, base.gameObject);
                        this.inHitPause = true;
                        this.hitPauseTimer = Toolbot.ToolbotDash.hitPauseDuration;
                        base.AddRecoil(-0.5f * Toolbot.ToolbotDash.recoilAmplitude, -0.5f * Toolbot.ToolbotDash.recoilAmplitude, -0.5f * Toolbot.ToolbotDash.recoilAmplitude, 0.5f * Toolbot.ToolbotDash.recoilAmplitude);

                        for (int i = 0; i < this.victimsStruck.Count; i++)
                        {
                            float mass = 0f;
                            HealthComponent healthComponent = this.victimsStruck[i];
                            CharacterMotor characterMotor = healthComponent.GetComponent<CharacterMotor>();
                            if (characterMotor)
                            {
                                mass = characterMotor.mass;
                            }
                            else
                            {
                                Rigidbody rigidbody = healthComponent.GetComponent<Rigidbody>();
                                if (rigidbody)
                                {
                                    mass = rigidbody.mass;
                                }
                            }
                            if (mass >= ShoulderBash.massThresholdForKnockback)
                            {
                                this.outer.SetNextState(new ShoulderBashImpact
                                {
                                    victimHealthComponent = healthComponent,
                                    idealDirection = this.forwardDirection,
                                    isCrit = this.attack.isCrit
                                });
                                return;
                            }
                        }
                        return;
                    }
                }
                else
                {
                    base.characterMotor.velocity = Vector3.zero;
                    this.hitPauseTimer -= Time.fixedDeltaTime;
                    if (this.hitPauseTimer < 0f)
                    {
                        this.inHitPause = false;
                    }
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            if (this.shieldCancel) return InterruptPriority.Any;
            else return InterruptPriority.Frozen;
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(this.forwardDirection);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            this.forwardDirection = reader.ReadVector3();
        }
    }

    public class ShoulderBashImpact : BaseState
    {
        public HealthComponent victimHealthComponent;
        public Vector3 idealDirection;
        public bool isCrit;

        public static float baseDuration = 0.35f;
        public static float recoilAmplitude = 4.5f;

        private float duration;
        private bool shieldCancel;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = ShoulderBashImpact.baseDuration / this.attackSpeedStat;
            if (!base.HasBuff(EnforcerPlugin.EnforcerPlugin.skateboardBuff)) base.PlayAnimation("FullBody, Override", "BashRecoil");
            base.SmallHop(base.characterMotor, ShoulderBash.smallHopVelocity);

            Util.PlayScaledSound(EnforcerPlugin.Sounds.ShoulderBashHit, base.gameObject, 0.5f);

            if (NetworkServer.active)
            {
                if (this.victimHealthComponent)
                {
                    DamageInfo damageInfo = new DamageInfo
                    {
                        attacker = base.gameObject,
                        damage = this.damageStat * ShoulderBash.knockbackDamageCoefficient,
                        crit = this.isCrit,
                        procCoefficient = 1f,
                        damageColorIndex = DamageColorIndex.Item,
                        damageType = DamageType.Stun1s,
                        position = base.characterBody.corePosition
                    };

                    this.victimHealthComponent.TakeDamage(damageInfo);
                    GlobalEventManager.instance.OnHitEnemy(damageInfo, this.victimHealthComponent.gameObject);
                    GlobalEventManager.instance.OnHitAll(damageInfo, this.victimHealthComponent.gameObject);
                }
            }

            if (base.characterMotor) base.characterMotor.velocity = this.idealDirection * -ShoulderBash.knockbackForce;

            if (base.isAuthority)
            {
                base.AddRecoil(-0.5f * ShoulderBashImpact.recoilAmplitude * 3f, -0.5f * ShoulderBashImpact.recoilAmplitude * 3f, -0.5f * ShoulderBashImpact.recoilAmplitude * 8f, 0.5f * ShoulderBashImpact.recoilAmplitude * 3f);
                EffectManager.SimpleImpactEffect(Loader.SwingZapFist.overchargeImpactEffectPrefab, this.victimHealthComponent.transform.position, base.characterDirection.forward, true);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.inputBank && base.isAuthority)
            {
                if (base.skillLocator && base.inputBank)
                {
                    if (base.inputBank.skill4.down && base.fixedAge >= 0.4f * this.duration)
                    {
                        this.shieldCancel = true;
                        base.characterBody.isSprinting = false;
                        base.skillLocator.special.ExecuteIfReady();
                    }
                }
            }

            if (base.fixedAge >= this.duration)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(this.victimHealthComponent ? this.victimHealthComponent.gameObject : null);
            writer.Write(this.idealDirection);
            writer.Write(this.isCrit);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            GameObject gameObject = reader.ReadGameObject();
            this.victimHealthComponent = (gameObject ? gameObject.GetComponent<HealthComponent>() : null);
            this.idealDirection = reader.ReadVector3();
            this.isCrit = reader.ReadBoolean();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            if (this.shieldCancel) return InterruptPriority.Any;
            else return InterruptPriority.Frozen;
        }
    }
}