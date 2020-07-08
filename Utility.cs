using RoR2;
using UnityEngine;

namespace EntityStates.Enforcer
{
    public class StunGrenade : BaseSkillState
    {
        public float baseDuration = 0.5f;
        public static float damageCoefficient = 1.5f;
        public static float procCoefficient = 0.6f;
        public static float blastRadius = 5f;

        private float duration;
        public GameObject grenadePrefab = EnforcerPlugin.Assets.grenade;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = baseDuration / this.attackSpeedStat;

            Ray aimRay = base.GetAimRay();
            /*if (base.isAuthority)
            {
                GameObject grenade = UnityEngine.Object.Instantiate<GameObject>(grenadePrefab, aimRay.origin + 2 * aimRay.direction, Quaternion.LookRotation(aimRay.direction));

                Rigidbody rig = grenade.GetComponent<Rigidbody>();
                rig.velocity = 50 * aimRay.direction;

                GrenadeController grc = grenade.GetComponentInChildren<GrenadeController>();
                BlastAttack blastAttack = new BlastAttack();
                blastAttack.radius = StunGrenade.blastRadius;
                blastAttack.procCoefficient = StunGrenade.procCoefficient;
                blastAttack.position = transform.position;
                blastAttack.attacker = base.gameObject;
                blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
                blastAttack.baseDamage = base.characterBody.damage * StunGrenade.damageCoefficient;
                blastAttack.falloffModel = BlastAttack.FalloffModel.SweetSpot;
                blastAttack.baseForce = 3f;
                blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
                blastAttack.damageType = DamageType.Stun1s;
                blastAttack.attackerFiltering = AttackerFiltering.NeverHit;
                grc.blastAttack = blastAttack;
            }*/
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
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