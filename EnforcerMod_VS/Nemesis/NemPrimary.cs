using EntityStates.Enforcer;
using RoR2;
using RoR2.Skills;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace EntityStates.Nemforcer
{
    public class HammerSwing : BaseSkillState
    {
        public static string hitboxString = "HammerHitbox";
        public static float baseDuration = 1.2f;
        public static float damageCoefficient = 4f;
        public static float procCoefficient = 1f;
        public static float attackRecoil = 1.5f;
        public static float hitHopVelocity = 5.5f;
        public static string mecanimRotateParameter = "baseRotate";
        public int currentSwing;
        private float earlyExitTime = 0.95f;

        private float duration;
        private float earlyExitDuration;
        private Ray aimRay;
        private ChildLocator childLocator;
        private bool hasFired;
        private float hitPauseTimer;
        private OverlapAttack attack;
        private bool inHitPause;
        private bool hasHopped;
        private float stopwatch;
        private Animator animator;
        private BaseState.HitStopCachedState hitStopCachedState;
        private Transform modelBaseTransform;
        private NemforcerController nemController;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = baseDuration / this.attackSpeedStat;
            this.earlyExitDuration = this.duration * earlyExitTime;
            this.hasFired = false;
            base.characterBody.isSprinting = false;
            this.nemController = base.GetComponent<NemforcerController>();

            this.childLocator = base.GetModelChildLocator();
            this.modelBaseTransform = base.GetModelBaseTransform();
            this.animator = base.GetModelAnimator();
            bool grounded = base.characterMotor.isGrounded;

            aimRay = base.GetAimRay();

            string swingAnimState = currentSwing % 2 == 0 ? "HammerSwing" : "HammerSwing2";

            HitBoxGroup hitBoxGroup = Array.Find<HitBoxGroup>(base.GetModelTransform().GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == "Hammer");

            base.PlayAnimation("Gesture, Override", swingAnimState, "HammerSwing.playbackRate", this.duration);
            //base.PlayAnimation("Legs, Override", "SwingLegs", "HammerSwing.playbackRate", this.duration);

            float dmg = HammerSwing.damageCoefficient;

            this.attack = new OverlapAttack();
            this.attack.damageType = DamageType.Generic;
            this.attack.attacker = base.gameObject;
            this.attack.inflictor = base.gameObject;
            this.attack.teamIndex = base.GetTeam();
            this.attack.damage = dmg * this.damageStat;
            this.attack.procCoefficient = 1;
            this.attack.hitEffectPrefab = EnforcerPlugin.Assets.nemImpactFX;
            this.attack.forceVector = Vector3.zero;
            this.attack.pushAwayForce = 1800f;
            this.attack.hitBoxGroup = hitBoxGroup;
            this.attack.isCrit = base.RollCrit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            this.hitPauseTimer -= Time.fixedDeltaTime;

            if (this.hitPauseTimer <= 0f && this.inHitPause)
            {
                base.ConsumeHitStopCachedState(this.hitStopCachedState, base.characterMotor, this.animator);
                this.inHitPause = false;
            }

            if (!this.inHitPause)
            {
                this.stopwatch += Time.fixedDeltaTime;
            }
            else
            {
                if (base.characterMotor) base.characterMotor.velocity = Vector3.zero;
                if (this.animator) this.animator.SetFloat("HammerSwing.playbackRate", 0f);
            }

            if (this.stopwatch >= this.duration * 0.45f && this.stopwatch <= this.duration * 0.75f)
            {
                this.FireAttack();
            }


            Vector3 aimDirection = base.GetAimRay().direction;
            aimDirection.y = 0;

            Vector3 turnDirection = Vector3.Cross(aimDirection, Vector3.up);

            float rot = animator.GetFloat(mecanimRotateParameter);

            Ray aimRayTurned = aimRay;
            aimRayTurned.direction = Vector3.RotateTowards(aimDirection, turnDirection, -rot * Mathf.Deg2Rad, 0.0f);
            
            pseudoAimMode(aimRayTurned);

            if (base.fixedAge >= this.earlyExitDuration && base.inputBank.skill1.down && currentSwing != 1)
            {
                var nextSwing = new HammerSwing();
                nextSwing.currentSwing = currentSwing + 1;
                this.outer.SetNextState(nextSwing);
                return;
            }

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                base.StartAimMode(2f, false);
                return;
            }
        }

        public void FireAttack()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                if (this.nemController && this.attack.isCrit) this.nemController.hammerBurst.Play();

                Util.PlayScaledSound(EnforcerPlugin.Sounds.NemesisSwing, base.gameObject, this.attackSpeedStat);

                base.AddRecoil(-1f * HammerSwing.attackRecoil, -2f * HammerSwing.attackRecoil, -0.5f * HammerSwing.attackRecoil, 0.5f * HammerSwing.attackRecoil);

                string muzzleString = "SwingCenter";
                EffectManager.SimpleMuzzleFlash(EnforcerPlugin.Assets.nemSwingFX, base.gameObject, muzzleString, true);
            }

            if (base.isAuthority)
            {
                Ray aimRay = base.GetAimRay();

                if (this.attack.Fire())
                {
                    Util.PlaySound(EnforcerPlugin.Sounds.NemesisImpact, base.gameObject);

                    if (!this.hasHopped)
                    {
                        if (base.characterMotor && !base.characterMotor.isGrounded)
                        {
                            base.SmallHop(base.characterMotor, HammerSwing.hitHopVelocity);
                        }

                        this.hasHopped = true;
                    }

                    if (!this.inHitPause)
                    {
                        this.hitStopCachedState = base.CreateHitStopCachedState(base.characterMotor, this.animator, "HammerSwing.playbackRate");
                        this.hitPauseTimer = (3f * EntityStates.Merc.GroundLight.hitPauseDuration) / this.attackSpeedStat;
                        this.inHitPause = true;
                    }
                }
            }
        }

        public override void OnExit()
        {
            if (!this.hasFired) this.FireAttack();

            base.OnExit();
        }

        //copied and pasted only what we need from SetAimMode cause using the whole thing is a little fucky
        //todo: move this to charactermain so we can use it for other moves/manage it better
        private void pseudoAimMode(Ray ray)
        {
            base.characterDirection.forward = ray.direction;
            base.characterDirection.moveVector = ray.direction;

            if (base.modelLocator)
            {
                Transform modelTransform = base.modelLocator.modelTransform;
                if (modelTransform)
                {
                    AimAnimator component = modelTransform.GetComponent<AimAnimator>();
                    component.AimImmediate();
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
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