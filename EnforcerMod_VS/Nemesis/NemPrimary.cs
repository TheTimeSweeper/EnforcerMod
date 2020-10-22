using RoR2;
using UnityEngine;

namespace EntityStates.Nemforcer
{
    public class HammerSwing : BaseSkillState
    {
        //this is just shield bash code for now, change it to an overlap attack eventually
        // the hitbox for it is already set up im just lazy rn
        public static string hitboxString = "HammerModel2";
        public static float baseDuration = 0.8f;
        public static float damageCoefficient = 3.5f;
        public static float procCoefficient = 1f;
        public static float blastRadius = 6f;
        public static float deflectRadius = 3f;
        public static float recoilAmplitude = 1f;
        public static float parryInterval = 0.12f;

        private float duration;
        private float fireDuration;
        private Ray aimRay;
        private BlastAttack blastAttack;
        private ChildLocator childLocator;
        private bool hasFired;

        public override void OnEnter()
        {
            base.OnEnter();

            this.duration = baseDuration / this.attackSpeedStat;
            this.fireDuration = this.duration * 0.35f;
            this.aimRay = base.GetAimRay();
            this.hasFired = false;
            this.childLocator = base.GetModelChildLocator();

            base.StartAimMode(aimRay, 2f, false);

            bool grounded = base.characterMotor.isGrounded;

            base.PlayAnimation("Gesture, Override", "HammerSwing", "HammerSwing.playbackRate", this.duration);

            if (this.childLocator.FindChild("Hammer"))
            {
                var anim = this.GetModelChildLocator().FindChild("Hammer").GetComponentInChildren<Animator>();
                if (anim)
                {
                    anim.SetFloat("HammerSwing.playbackRate", this.duration);
                    anim.SetTrigger("HammerSwing");
                }
            }

            Util.PlayScaledSound(EnforcerPlugin.Sounds.ShieldBash, base.gameObject, 0.5f + this.attackSpeedStat);
        }

        private void FireBlast()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                //EffectManager.SimpleMuzzleFlash(EnforcerPlugin.Assets.shieldBashFX, base.gameObject, hitboxString, true);

                if (base.isAuthority)
                {
                    base.AddRecoil(-0.5f * HammerSwing.recoilAmplitude * 3f, -0.5f * HammerSwing.recoilAmplitude * 3f, -0.5f * HammerSwing.recoilAmplitude * 8f, 0.5f * HammerSwing.recoilAmplitude * 3f);

                    Vector3 center = childLocator.FindChild(hitboxString).position;

                    blastAttack = new BlastAttack();
                    blastAttack.radius = HammerSwing.blastRadius;
                    blastAttack.procCoefficient = HammerSwing.procCoefficient;
                    blastAttack.position = center;
                    blastAttack.attacker = base.gameObject;
                    blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
                    blastAttack.baseDamage = base.characterBody.damage * HammerSwing.damageCoefficient;
                    blastAttack.falloffModel = BlastAttack.FalloffModel.None;
                    blastAttack.baseForce = 0f;
                    blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
                    blastAttack.damageType = DamageType.Generic;
                    blastAttack.attackerFiltering = AttackerFiltering.NeverHit;
                    blastAttack.impactEffect = BeetleGuardMonster.GroundSlam.hitEffectPrefab.GetComponent<EffectComponent>().effectIndex;

                    blastAttack.Fire();
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