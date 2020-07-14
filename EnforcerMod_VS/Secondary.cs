using RoR2;
using RoR2.Projectile;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;

namespace EntityStates.Enforcer
{
    public class ShieldBash : BaseSkillState
    {
        public float baseDuration = 0.8f;
        public static float damageCoefficient = 2.5f;
        public static float procCoefficient = 1f;
        public static float knockbackForce = 0.2f;
        public static float blastRadius = 5f;
        public static float deflectRadius = 8f;
        public static string hitboxString = "ShieldHitbox"; //transform where the hitbox is fired

        private float duration;
        private float fireDuration;
        private Ray aimRay;
        private BlastAttack blastAttack;
        private ChildLocator childLocator;
        private bool hasFired;

        private List<CharacterBody> victimList = new List<CharacterBody>();

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / this.attackSpeedStat;
            this.fireDuration = this.duration * 0.17f;
            this.aimRay = base.GetAimRay();
            this.hasFired = false;
            this.childLocator = base.GetModelTransform().GetComponent<ChildLocator>();
            base.StartAimMode(aimRay, 2f, false);

            if (base.characterBody.GetComponent<ShieldComponent>().isShielding)
            {
                //base.PlayAnimation("Gesture, Override", "ShieldBashAlt", "ShieldBash.playbackRate", this.duration);
            }
            else
            {
                //base.PlayAnimation("Gesture, Override", "ShieldBash", "ShieldBash.playbackRate", this.duration);
            }

            Util.PlayScaledSound(Croco.Leap.leapSoundString, base.gameObject, 1.5f);
        }

        private void FireBlast()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                if (base.isAuthority)
                {
                    Vector3 center = childLocator.FindChild(hitboxString).position;

                    blastAttack = new BlastAttack();
                    blastAttack.radius = ShieldBash.blastRadius;
                    blastAttack.procCoefficient = ShieldBash.procCoefficient;
                    blastAttack.position = center;
                    blastAttack.attacker = base.gameObject;
                    blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
                    blastAttack.baseDamage = base.characterBody.damage * ShieldBash.damageCoefficient;
                    blastAttack.falloffModel = BlastAttack.FalloffModel.None;
                    blastAttack.baseForce = 3f;
                    blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
                    blastAttack.damageType = DamageType.Stun1s;
                    blastAttack.attackerFiltering = AttackerFiltering.NeverHit;

                    blastAttack.Fire();

                    KnockBack();
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.fireDuration)
            {
                FireBlast();
            }
            else
            {
                Deflect();
            }

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        private void KnockBack()
        {
            Collider[] array = Physics.OverlapSphere(childLocator.FindChild(hitboxString).position, ShieldBash.blastRadius, LayerIndex.defaultLayer.mask);
            for (int i = 0; i < array.Length; i++)
            {
                HealthComponent component = array[i].GetComponent<HealthComponent>();
                if (component)
                {
                    TeamComponent component2 = component.GetComponent<TeamComponent>();
                    if (component2.teamIndex != TeamIndex.Player)
                    {
                        AddToList(component.gameObject);
                    }
                }
            }

            victimList.ForEach(Push);
        }

        private void AddToList(GameObject affectedObject)
        {
            CharacterBody component = affectedObject.GetComponent<CharacterBody>();
            if (!this.victimList.Contains(component))
            {
                this.victimList.Add(component);
            }
        }

        void Push(CharacterBody charb)
        {
            Vector3 velocity = ((aimRay.origin + 200 * aimRay.direction) - childLocator.FindChild(hitboxString).position + (75 * Vector3.up)) * ShieldBash.knockbackForce;

            if (charb.characterMotor)
            {
                charb.characterMotor.velocity += velocity;
            }
            else
            {
                Rigidbody component2 = charb.GetComponent<Rigidbody>();
                if (component2)
                {
                    component2.velocity += velocity;
                }
            }
        }

        private void Deflect()
        {
            Collider[] array = Physics.OverlapSphere(childLocator.FindChild(hitboxString).position, ShieldBash.deflectRadius, LayerIndex.projectile.mask);

            for (int i = 0; i < array.Length; i++)
            {
                ProjectileController pc = array[i].GetComponentInParent<ProjectileController>();
                if (pc)
                {
                    if (pc.teamFilter.teamIndex != TeamIndex.Player)
                    {
                        Ray aimRay = base.GetAimRay();
                        Vector3 aimSpot = (aimRay.origin + 100 * aimRay.direction) - pc.gameObject.transform.position;
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

                        Destroy(pc.gameObject);
                    }
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }

    public class ExperimentalShieldBash : BaseSkillState
    {
        public float baseDuration = 0.8f;
        public static float damageCoefficient = 2.5f;
        public static float procCoefficient = 1f;
        public static float knockbackForce = 0.2f;
        public static float blastRadius = 5f;
        public static float deflectRadius = 8f;
        public static string hitboxString = "Shield"; //transform where the hitbox is fired

        private float duration;
        private float fireDuration;
        private Ray aimRay;
        private BlastAttack blastAttack;
        private ChildLocator childLocator;
        private bool hasFired;

        private List<CharacterBody> victimList = new List<CharacterBody>();

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / this.attackSpeedStat;
            this.fireDuration = this.duration * 0.17f;
            this.aimRay = base.GetAimRay();
            this.hasFired = false;
            this.childLocator = base.GetModelTransform().GetComponent<ChildLocator>();
            base.StartAimMode(aimRay, 2f, false);

            //yeah
            if (base.characterBody.isSprinting)
            {
                base.skillLocator.secondary.skillDef.activationStateMachineName = "Body";
                this.outer.SetNextState(new ShoulderBash());
                return;
            }

            if (base.characterBody.GetComponent<ShieldComponent>().isShielding)
            {
                //base.PlayAnimation("Gesture, Override", "ShieldBashAlt", "ShieldBash.playbackRate", this.duration);
            }
            else
            {
                //base.PlayAnimation("Gesture, Override", "ShieldBash", "ShieldBash.playbackRate", this.duration);
            }

            Util.PlayScaledSound(Croco.Leap.leapSoundString, base.gameObject, 1.5f);
        }

        private void FireBlast()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                if (base.isAuthority)
                {
                    Vector3 center = childLocator.FindChild(hitboxString).position;

                    blastAttack = new BlastAttack();
                    blastAttack.radius = ShieldBash.blastRadius;
                    blastAttack.procCoefficient = ShieldBash.procCoefficient;
                    blastAttack.position = center;
                    blastAttack.attacker = base.gameObject;
                    blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
                    blastAttack.baseDamage = base.characterBody.damage * ShieldBash.damageCoefficient;
                    blastAttack.falloffModel = BlastAttack.FalloffModel.None;
                    blastAttack.baseForce = 3f;
                    blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
                    blastAttack.damageType = DamageType.Stun1s;
                    blastAttack.attackerFiltering = AttackerFiltering.NeverHit;

                    blastAttack.Fire();

                    KnockBack();
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.fireDuration)
            {
                FireBlast();
            }
            else
            {
                Deflect();
            }

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        private void KnockBack()
        {
            Collider[] array = Physics.OverlapSphere(childLocator.FindChild(hitboxString).position, ShieldBash.blastRadius, LayerIndex.defaultLayer.mask);
            for (int i = 0; i < array.Length; i++)
            {
                HealthComponent component = array[i].GetComponent<HealthComponent>();
                if (component)
                {
                    TeamComponent component2 = component.GetComponent<TeamComponent>();
                    if (component2.teamIndex != TeamIndex.Player)
                    {
                        AddToList(component.gameObject);
                    }
                }
            }

            victimList.ForEach(Push);
        }

        private void AddToList(GameObject affectedObject)
        {
            CharacterBody component = affectedObject.GetComponent<CharacterBody>();
            if (!this.victimList.Contains(component))
            {
                this.victimList.Add(component);
            }
        }

        void Push(CharacterBody charb)
        {
            Vector3 velocity = ((aimRay.origin + 200 * aimRay.direction) - childLocator.FindChild(hitboxString).position + (75 * Vector3.up)) * ShieldBash.knockbackForce;

            if (charb.characterMotor)
            {
                charb.characterMotor.velocity += velocity;
            }
            else
            {
                Rigidbody component2 = charb.GetComponent<Rigidbody>();
                if (component2)
                {
                    component2.velocity += velocity;
                }
            }
        }

        private void Deflect()
        {
            Collider[] array = Physics.OverlapSphere(childLocator.FindChild(hitboxString).position, ShieldBash.deflectRadius, LayerIndex.projectile.mask);

            for (int i = 0; i < array.Length; i++)
            {
                ProjectileController pc = array[i].GetComponentInParent<ProjectileController>();
                if (pc)
                {
                    if (pc.teamFilter.teamIndex != TeamIndex.Player)
                    {
                        Ray aimRay = base.GetAimRay();
                        Vector3 aimSpot = (aimRay.origin + 100 * aimRay.direction) - pc.gameObject.transform.position;
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

                        Destroy(pc.gameObject);
                    }
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }

    public class ShoulderBash : BaseSkillState
    {
        [SerializeField]
        public float baseDuration = 0.4f;
        [SerializeField]
        public float speedMultiplier = 1.1f;
        public static float chargeDamageCoefficient = 4.5f;
        public static float knockbackDamageCoefficient = 6f;
        public static float massThresholdForKnockback = 150;
        public static float knockbackForce = 3400;
        public static float smallHopVelocity = 16f;

        private float duration;
        private float hitPauseTimer;
        private Vector3 idealDirection;
        private OverlapAttack attack;
        private bool inHitPause;
        private List<HealthComponent> victimsStruck = new List<HealthComponent>();

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration;

            if (base.isAuthority)
            {
                if (base.inputBank)
                {
                    this.idealDirection = base.inputBank.aimDirection;
                    this.idealDirection.y = 0f;
                }
                this.UpdateDirection();
            }

            if (base.characterDirection)
            {
                base.characterDirection.forward = this.idealDirection;
            }

            Util.PlayScaledSound(Croco.Leap.leapSoundString, base.gameObject, 1.75f);

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
            this.attack.hitEffectPrefab = Toolbot.ToolbotDash.impactEffectPrefab;
            this.attack.forceVector = Vector3.up * Toolbot.ToolbotDash.upwardForceMagnitude;
            this.attack.pushAwayForce = Toolbot.ToolbotDash.awayForceMagnitude;
            this.attack.hitBoxGroup = hitBoxGroup;
            this.attack.isCrit = base.RollCrit();
        }

        public override void OnExit()
        {
            if (base.characterBody)
            {
                base.characterBody.isSprinting = true;
            }

            if (base.characterMotor && !base.characterMotor.disableAirControlUntilCollision)
            {
                base.characterMotor.velocity += this.GetIdealVelocity();
            }

            if (base.skillLocator) base.skillLocator.secondary.skillDef.activationStateMachineName = "Weapon";

            base.OnExit();
        }

        private void UpdateDirection()
        {
            if (base.inputBank)
            {
                Vector2 vector = Util.Vector3XZToVector2XY(base.inputBank.moveVector);
                if (vector != Vector2.zero)
                {
                    vector.Normalize();
                    this.idealDirection = new Vector3(vector.x, 0f, vector.y).normalized;
                }
            }
        }

        private Vector3 GetIdealVelocity()
        {
            return base.characterDirection.forward * base.characterBody.moveSpeed * this.speedMultiplier;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.duration)
            {
                this.outer.SetNextStateToMain();
                return;
            }

            if (base.isAuthority)
            {
                if (base.characterBody)
                {
                    base.characterBody.isSprinting = true;
                }

                if (!this.inHitPause)
                {
                    if (base.characterDirection)
                    {
                        base.characterDirection.moveVector = this.idealDirection;
                        if (base.characterMotor && !base.characterMotor.disableAirControlUntilCollision)
                        {
                            base.characterMotor.rootMotion += this.GetIdealVelocity() * Time.fixedDeltaTime;
                        }
                    }

                    this.attack.damage = this.damageStat * ShoulderBash.chargeDamageCoefficient;

                    if (this.attack.Fire(this.victimsStruck))
                    {
                        Util.PlaySound(Toolbot.ToolbotDash.impactSoundString, base.gameObject);
                        this.inHitPause = true;
                        this.hitPauseTimer = Toolbot.ToolbotDash.hitPauseDuration;
                        base.AddRecoil(-0.5f * Toolbot.ToolbotDash.recoilAmplitude, -0.5f * Toolbot.ToolbotDash.recoilAmplitude, -0.5f * Toolbot.ToolbotDash.recoilAmplitude, 0.5f * Toolbot.ToolbotDash.recoilAmplitude);

                        for (int i = 0; i < this.victimsStruck.Count; i++)
                        {
                            float num = 0f;
                            HealthComponent healthComponent = this.victimsStruck[i];
                            CharacterMotor component = healthComponent.GetComponent<CharacterMotor>();
                            if (component)
                            {
                                num = component.mass;
                            }
                            else
                            {
                                Rigidbody component2 = healthComponent.GetComponent<Rigidbody>();
                                if (component2)
                                {
                                    num = component2.mass;
                                }
                            }
                            if (num >= ShoulderBash.massThresholdForKnockback)
                            {
                                this.outer.SetNextState(new ShoulderBashImpact
                                {
                                    victimHealthComponent = healthComponent,
                                    idealDirection = this.idealDirection,
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
            return InterruptPriority.Frozen;
        }
    }

    public class ShoulderBashImpact : BaseState
    {
        public HealthComponent victimHealthComponent;
        public Vector3 idealDirection;
        public bool isCrit;

        public override void OnEnter()
        {
            base.OnEnter();

            base.SmallHop(base.characterMotor, ShoulderBash.smallHopVelocity);

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

                base.healthComponent.TakeDamageForce(this.idealDirection * -ShoulderBash.knockbackForce, true, false);
            }

            if (base.isAuthority)
            {
                base.AddRecoil(-0.5f * Toolbot.ToolbotDash.recoilAmplitude * 3f, -0.5f * Toolbot.ToolbotDash.recoilAmplitude * 3f, -0.5f * Toolbot.ToolbotDash.recoilAmplitude * 8f, 0.5f * Toolbot.ToolbotDash.recoilAmplitude * 3f);
                EffectManager.SimpleImpactEffect(Toolbot.ToolbotDash.knockbackEffectPrefab, base.characterBody.corePosition, base.characterDirection.forward, true);
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
            return InterruptPriority.Frozen;
        }
    }
}