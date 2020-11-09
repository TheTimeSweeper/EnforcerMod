using EntityStates.Enforcer;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.Networking;

namespace EntityStates.Nemforcer {

    public class HammerSwing : BaseSkillState {
        //this is just shield bash code for now, change it to an overlap attack eventually
        // the hitbox for it is already set up im just lazy rn
        public static string hitboxString = "HammerHitbox";
        public static float baseDuration = 1.2f;
        public static float damageCoefficient = 3.5f;
        public static float procCoefficient = 1f;
        public static float blastRadius = 6f;
        public static float deflectRadius = 3f;
        public static float recoilAmplitude = 1f;
        public static float parryInterval = 0.12f;
        public static string mecanimRotateParameter = "baseRotate";

        public int currentSwing;

        private float fireBlastTime = 0.45f;
        private float earlyExitTime = 0.95f;

        private float duration;
        private float fireDuration;
        private float earlyExitDuration;
        private Ray aimRay;
        private BlastAttack blastAttack;
        private ChildLocator childLocator;
        private bool hasFired;
        private Animator animator;
        private Transform hitboxPivot;
        private Transform modelBaseTransform;

        public override void OnEnter() {
            base.OnEnter();

            this.duration = baseDuration / this.attackSpeedStat;
            this.fireDuration = this.duration * fireBlastTime;
            this.earlyExitDuration = this.duration * earlyExitTime;
            this.hasFired = false;

            this.childLocator = base.GetModelChildLocator();
            this.modelBaseTransform = base.GetModelBaseTransform();
            this.animator = base.GetModelAnimator();

            this.hitboxPivot = childLocator.FindChild("HammerHitboxPivot");

            bool grounded = base.characterMotor.isGrounded;

            aimRay = base.GetAimRay();

            string swingAnimState = currentSwing % 2 == 0 ? "HammerSwing" : "HammerSwing2";

            base.PlayAnimation("Gesture, Override", swingAnimState, "HammerSwing.playbackRate", this.duration);
            //base.PlayAnimation("Legs, Override", "SwingLegs", "HammerSwing.playbackRate", this.duration);

            Util.PlayScaledSound(EnforcerPlugin.Sounds.ShieldBash, base.gameObject, 0.5f + this.attackSpeedStat);
        }

        public override void FixedUpdate() {
            base.FixedUpdate();

            Vector3 aimDirection = base.GetAimRay().direction;
            aimDirection.y = 0;

            Vector3 turnDirection = Vector3.Cross(aimDirection, Vector3.up);

            float rot = animator.GetFloat(mecanimRotateParameter);
            float fuckingMath = Mathf.Sin(Mathf.Deg2Rad * rot * 2);

            Ray aimRayTurned = aimRay;
            aimRayTurned.direction = aimDirection + turnDirection * -fuckingMath;

            pseudoAimMode(aimRayTurned);

            if (base.fixedAge >= this.fireDuration) {
                this.FireBlast();
            }

            if (base.fixedAge >= this.earlyExitDuration && base.inputBank.skill1.down) {
                var nextSwing = new HammerSwing();
                nextSwing.currentSwing = currentSwing + 1;
                this.outer.SetNextState(nextSwing);
                return;
            }

            if (base.fixedAge >= this.duration && base.isAuthority) {


                base.StartAimMode(2f, false);
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override void OnExit() {
            base.OnExit();
        }

        private void FireBlast() {
            if (!this.hasFired) {
                this.hasFired = true;

                //EffectManager.SimpleMuzzleFlash(EnforcerPlugin.Assets.shieldBashFX, base.gameObject, hitboxString, true);

                if (base.isAuthority) {
                    base.AddRecoil(-0.5f * recoilAmplitude * 3f, -0.5f * recoilAmplitude * 3f, -0.5f * recoilAmplitude * 8f, 0.5f * recoilAmplitude * 3f);

                    Vector3 center = childLocator.FindChild(hitboxString).position;

                    blastAttack = new BlastAttack();
                    blastAttack.radius = blastRadius;
                    blastAttack.procCoefficient = procCoefficient;
                    blastAttack.position = center;
                    blastAttack.attacker = base.gameObject;
                    blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
                    blastAttack.baseDamage = base.characterBody.damage * damageCoefficient;
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

        //copied and pasted only what we need from SetAimMode cause using the whole thing is a little fucky
        private void pseudoAimMode(Ray ray) {

            base.characterDirection.forward = ray.direction;
            base.characterDirection.moveVector = ray.direction;

            if (base.modelLocator) {

                Transform modelTransform = base.modelLocator.modelTransform;
                if (modelTransform) {
                    AimAnimator component = modelTransform.GetComponent<AimAnimator>();
                    component.AimImmediate();
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.PrioritySkill;
        }

        //steppedskill didn't work so I'm just doing it the old way that merc's combo did it.
        //public void SetStep(int i) {
        //    currentSwing = i;
        //    Debug.LogWarning($"hammer step {i}");
        //}

        ////copied from all the other steppedskills
        //public override void OnSerialize(NetworkWriter writer) {
        //    base.OnSerialize(writer);
        //    writer.Write((byte)this.currentSwing);
        //}
        //public override void OnDeserialize(NetworkReader reader) {
        //    base.OnDeserialize(reader);
        //    this.currentSwing = (int)reader.ReadByte();
        //}
    }
}