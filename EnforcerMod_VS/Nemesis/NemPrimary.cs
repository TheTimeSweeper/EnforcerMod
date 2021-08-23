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
        public static float baseDuration = 1.05f;
        public static float damageCoefficient = 5f;
        public static float procCoefficient = 1f;
        public static float attackRecoil = 1.5f;
        public static float hitHopVelocity = 5.5f;
        public static string mecanimRotateParameter = "baseRotate";
        public int currentSwing;
        private float earlyExitTime = 0.90f;

        private float duration;
        private float earlyExitDuration;
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
        private Vector3 storedVelocity;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = baseDuration / this.attackSpeedStat;
            this.earlyExitDuration = this.duration * earlyExitTime;
            this.hasFired = false;
            base.characterBody.isSprinting = false;
            base.characterBody.outOfCombatStopwatch = 0f;

            this.childLocator = base.GetModelChildLocator();
            this.modelBaseTransform = base.GetModelBaseTransform();
            this.animator = base.GetModelAnimator();
            this.nemController = base.GetComponent<NemforcerController>();

            bool grounded = base.characterMotor.isGrounded;
            bool moving = this.animator.GetBool("isMoving");

            string swingAnimState = currentSwing % 2 == 0 ? "HammerSwing" : "HammerSwing2";

            HitBoxGroup hitBoxGroup = base.FindHitBoxGroup("Hammer");
            this.animator.SetBool("swinging", true);

            base.PlayCrossfade("Gesture, Override", swingAnimState, "HammerSwing.playbackRate", this.duration, 0.05f);
            if (grounded && !moving) {
                base.PlayCrossfade("Legs, Override", swingAnimState, "HammerSwing.playbackRate", this.duration, 0.05f);
            }

            NetworkSoundEventDef hitSound = EnforcerPlugin.Assets.nemHammerHitSoundEvent;

            if (base.characterBody.skinIndex == 2 && EnforcerPlugin.EnforcerModPlugin.starstormInstalled) hitSound = EnforcerPlugin.Assets.nemAxeHitSoundEvent;

            float dmg = HammerSwing.damageCoefficient;

            this.attack = new OverlapAttack();
            this.attack.damageType = DamageType.Generic;
            this.attack.attacker = base.gameObject;
            this.attack.inflictor = base.gameObject;
            this.attack.teamIndex = base.GetTeam();
            this.attack.damage = dmg * this.damageStat;
            this.attack.procCoefficient = 1;
            this.attack.hitEffectPrefab = EnforcerPlugin.Assets.nemImpactFX;
            if (base.characterBody.skinIndex == 2 && EnforcerPlugin.EnforcerModPlugin.starstormInstalled) this.attack.hitEffectPrefab = EnforcerPlugin.Assets.nemAxeImpactFX;
            this.attack.forceVector = Vector3.zero;
            this.attack.pushAwayForce = 1800f;
            this.attack.hitBoxGroup = hitBoxGroup;
            this.attack.isCrit = base.RollCrit();
            this.attack.impactSound = hitSound.index;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            this.hitPauseTimer -= Time.fixedDeltaTime;

            if (this.hitPauseTimer <= 0f && this.inHitPause)
            {
                base.ConsumeHitStopCachedState(this.hitStopCachedState, base.characterMotor, this.animator); 
                this.inHitPause = false;
                if (this.storedVelocity != Vector3.zero) base.characterMotor.velocity = this.storedVelocity;
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

            float rot = this.animator.GetFloat(mecanimRotateParameter);
            this.nemController.pseudoAimMode(rot);

            if (base.fixedAge >= this.earlyExitDuration && base.inputBank.skill1.down)
            {
                var nextSwing = new HammerSwing();
                nextSwing.currentSwing = this.currentSwing + 1;
                this.outer.SetNextState(nextSwing);
                return;
            }

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                base.StartAimMode(0.2f, false);
                return;
            }
        }

        public void FireAttack()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                if (this.nemController && this.attack.isCrit) this.nemController.hammerBurst.Play();

                string soundString = EnforcerPlugin.Sounds.NemesisSwing2;
                if (base.characterBody.skinIndex == 2 && EnforcerPlugin.EnforcerModPlugin.starstormInstalled) soundString = EnforcerPlugin.Sounds.NemesisSwingAxe;
                Util.PlayAttackSpeedSound(soundString, base.gameObject, this.attackSpeedStat);

                base.AddRecoil(-1f * HammerSwing.attackRecoil, -2f * HammerSwing.attackRecoil, -0.5f * HammerSwing.attackRecoil, 0.5f * HammerSwing.attackRecoil);

                if (base.isAuthority) EffectManager.SimpleMuzzleFlash(EnforcerPlugin.Assets.nemSwingFX, base.gameObject, "SwingCenter", true);
            }

            if (base.isAuthority) 
            {
                Ray aimRay = base.GetAimRay();

                if (this.attack.Fire())
                {
                    base.AddRecoil(-1f * HammerSwing.attackRecoil, -2f * HammerSwing.attackRecoil, -0.5f * HammerSwing.attackRecoil, 0.5f * HammerSwing.attackRecoil);

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
                        if (base.characterMotor.velocity != Vector3.zero) this.storedVelocity = base.characterMotor.velocity;

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

            this.animator.SetBool("swinging", false);

            base.OnExit();
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