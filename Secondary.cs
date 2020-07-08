using RoR2;
using RoR2.Projectile;
using UnityEngine;
using System.Collections.Generic;

namespace EntityStates.Enforcer
{
    public class ShieldBash : BaseSkillState
    {
        public float baseDuration = 0.4f;
        public static float damageCoefficient = 2.5f;
        public static float procCoefficient = 1f;
        public static float knockbackForce = 0.2f;

        private float duration;
        Ray aimRay;
        BlastAttack blastAttack;

        List<CharacterBody> victimList = new List<CharacterBody>();

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / this.attackSpeedStat;
            aimRay = base.GetAimRay();
            base.StartAimMode(aimRay, 2f, false);

            //base.PlayAnimation("Fullbody, Override", "ShieldBash");

            if (base.isAuthority)
            {
                Vector3 center = aimRay.origin + 3 * aimRay.direction;

                blastAttack = new BlastAttack();
                blastAttack.radius = 3.5f;
                blastAttack.procCoefficient = ShieldBash.procCoefficient;
                blastAttack.position = center;
                blastAttack.attacker = base.gameObject;
                blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
                blastAttack.baseDamage = base.characterBody.damage * ShieldBash.damageCoefficient;
                blastAttack.falloffModel = BlastAttack.FalloffModel.SweetSpot;
                blastAttack.baseForce = 3f;
                blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
                blastAttack.damageType = DamageType.Generic;
                blastAttack.attackerFiltering = AttackerFiltering.NeverHit;

                blastAttack.Fire();

                KnockBack();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            Deflect();

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        private void KnockBack()
        {
            Collider[] array = Physics.OverlapSphere(base.characterBody.corePosition, 5.5f, LayerIndex.defaultLayer.mask);
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
            Vector3 velocity = ((aimRay.origin + 200 * aimRay.direction) - charb.corePosition) * ShieldBash.knockbackForce;
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
            Collider[] array = Physics.OverlapSphere(base.characterBody.corePosition, 4f, LayerIndex.projectile.mask);
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