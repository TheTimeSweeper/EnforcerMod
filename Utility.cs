using EntityStates;
using RoR2;
using UnityEngine;

namespace Gnome.EntityStatez
{
    public class UtilityState : BaseSkillState
    {
        public float baseDuration = 0.5f;
        private float duration;
        //public GameObject grenadePrefab = Assets2.minerAssetBundle.LoadAsset<GameObject>("Grenade");
        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / base.attackSpeedStat;
            Ray aimRay = base.GetAimRay();
            if (base.isAuthority)
            {
                //GameObject grenade = UnityEngine.Object.Instantiate<GameObject>(grenadePrefab, aimRay.origin + 2 * aimRay.direction, Quaternion.LookRotation(aimRay.direction));
                //Rigidbody rig = grenade.GetComponent<Rigidbody>();
                //rig.velocity = 50 * aimRay.direction;

                //GrenadeController grc = grenade.GetComponentInChildren<GrenadeController>();
                //BlastAttack blastAttack = new BlastAttack();
                //blastAttack.radius = 4f;
                //blastAttack.procCoefficient = 1f;
                //blastAttack.position = transform.position;
                //blastAttack.attacker = base.gameObject;
                //blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
                //blastAttack.baseDamage = base.characterBody.damage * 1.8f;
                //blastAttack.falloffModel = BlastAttack.FalloffModel.SweetSpot;
                //blastAttack.baseForce = 3f;
                //blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
                //blastAttack.damageType = DamageType.Stun1s;
                //blastAttack.attackerFiltering = AttackerFiltering.NeverHit;
                //grc.blastAttack = blastAttack;
            }
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
            return InterruptPriority.Skill;
        }
    }
}