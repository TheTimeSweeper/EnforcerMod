using RoR2;
using RoR2.Projectile;
using UnityEngine;
using System.Collections.Generic;

namespace EntityStates.Enforcer
{
    public class ShieldBash : BaseSkillState
    {
        public float baseDuration = 0.8f;
        public static float damageCoefficient = 2.5f;
        public static float procCoefficient = 1f;
        public static float knockbackForce = 0.2f;
        public static float blastRadius = 3f;
        public static float deflectRadius = 6f;
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

            if (base.characterBody.GetComponent<ShieldComponent>().isShielding)
            {
                //this anim not added yet

                //base.PlayAnimation("Gesture, Override", "ShieldBashAlt", "ShieldBash.playbackRate", this.duration);
            }
            else
            {
                base.PlayAnimation("Gesture, Override", "ShieldBash", "ShieldBash.playbackRate", this.duration);
            }
        }

        private void FireBlast()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                Deflect();

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
            Collider[] array = Physics.OverlapSphere(base.characterBody.corePosition, ShieldBash.deflectRadius, LayerIndex.projectile.mask);

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
}