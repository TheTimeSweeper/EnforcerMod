using RoR2;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using RoR2.Projectile;

namespace EntityStates.Nemforcer
{
    public class HammerSlam : BaseSkillState
    {
        public static string hitboxString = "SwingCenter"; //transform where the hitbox is fired
        public static float baseDuration = 0.8f;
        public static float damageCoefficient = 8f;
        public static float procCoefficient = 1f;
        public static float knockbackForce = 0.14f;
        public static float blastRadius = 12f;
        public static float beefDuration = 0.8f;
        public static float recoilAmplitude = 3f;

        private float attackStopDuration;
        private float duration;
        private float fireDuration;
        private Ray aimRay;
        private BlastAttack blastAttack;
        private ChildLocator childLocator;
        private bool hasFired;
        private bool usingBash;

        private List<CharacterBody> victimList = new List<CharacterBody>();

        public override void OnEnter()
        {
            base.OnEnter();

            this.duration = baseDuration / this.attackSpeedStat;
            this.fireDuration = this.duration * 0.75f;
            this.aimRay = base.GetAimRay();
            this.hasFired = false;
            this.usingBash = false;
            this.childLocator = base.GetModelTransform().GetComponent<ChildLocator>();

            base.StartAimMode(aimRay, 2f, false);

            this.attackStopDuration = HammerSlam.beefDuration / this.attackSpeedStat;

            Util.PlayScaledSound(EnforcerPlugin.Sounds.NemesisSwing, base.gameObject, this.attackSpeedStat);

            base.PlayAnimation("FullBody, Override", "HammerSlam", "HammerSlam.playbackRate", this.duration);
        }

        private void FireBlast()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                Vector3 sex = this.childLocator.FindChild("SwingCenter").transform.position;

                EffectData effectData = new EffectData();
                effectData.origin = sex;
                effectData.scale = 15;

                EffectManager.SpawnEffect(Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/PodGroundImpact"), effectData, true);

                Util.PlaySound("Play_parent_attack1_slam", base.gameObject);

                if (base.isAuthority)
                {
                    base.AddRecoil(-0.5f * HammerSlam.recoilAmplitude * 3f, -0.5f * HammerSlam.recoilAmplitude * 3f, -0.5f * HammerSlam.recoilAmplitude * 8f, 0.5f * HammerSlam.recoilAmplitude * 3f);

                    Vector3 center = childLocator.FindChild(hitboxString).position;

                    blastAttack = new BlastAttack();
                    blastAttack.radius = HammerSlam.blastRadius;
                    blastAttack.procCoefficient = HammerSlam.procCoefficient;
                    blastAttack.position = center;
                    blastAttack.attacker = base.gameObject;
                    blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
                    blastAttack.baseDamage = base.characterBody.damage * HammerSlam.damageCoefficient;
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
                    Vector3 pushForce = ((aimRay.origin + 200 * aimRay.direction) - childLocator.FindChild(hitboxString).position + (75 * Vector3.up)) * HammerSlam.knockbackForce;

                    Collider[] array = Physics.OverlapSphere(childLocator.FindChild(hitboxString).position, HammerSlam.blastRadius, LayerIndex.defaultLayer.mask);
                    for (int i = 0; i < array.Length; i++)
                    {
                        HealthComponent healthComponent = array[i].GetComponent<HealthComponent>();
                        if (healthComponent)
                        {
                            TeamComponent component2 = healthComponent.GetComponent<TeamComponent>();
                            if (component2.teamIndex != base.teamComponent.teamIndex)
                            {
                                Util.PlaySound(EnforcerPlugin.Sounds.NemesisImpact, healthComponent.gameObject);

                                var charb = healthComponent.body;
                                if (charb)
                                {
                                    var motor = charb.GetComponent<CharacterMotor>();
                                    var rb = charb.GetComponent<Rigidbody>();
                                    Vector3 force = pushForce;

                                    float mass = 0f;

                                    if (motor) mass = motor.mass;
                                    else if (rb) mass = rb.mass;

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

                                    charb.healthComponent.TakeDamageForce(info, true, true);
                                }

                            }
                        }
                    }
                }
            }
        }

        private void DestroyProjectiles()
        {
            Collider[] array = Physics.OverlapSphere(childLocator.FindChild(hitboxString).position, HammerSlam.blastRadius, LayerIndex.projectile.mask);

            for (int i = 0; i < array.Length; i++)
            {
                ProjectileController pc = array[i].GetComponentInParent<ProjectileController>();
                if (pc)
                {
                    if (pc.owner != gameObject)
                    {
                        Destroy(pc.gameObject);
                    }
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

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}