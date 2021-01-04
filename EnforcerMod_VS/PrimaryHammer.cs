using RoR2;
using System;
using UnityEngine;

namespace EntityStates.Enforcer
{
    public class HammerSwing : BaseSkillState
    {
        public static float baseDuration = 0.75f;
        public static float baseShieldDuration = 0.5f;
        public static float damageCoefficient = 4f;
        public static float procCoefficient = 1f;
        public static float attackRecoil = 1.15f;
        public static float hitHopVelocity = 5.5f;
        public static GameObject slamPrefab = EnforcerPlugin.EnforcerPlugin.hammerSlamEffect;

        private float duration;
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

        public override void OnEnter()
        {
            base.OnEnter();
            base.StartAimMode(2f, false);
            this.duration = baseDuration / this.attackSpeedStat;
            this.hasFired = false;
            base.characterBody.isSprinting = false;

            this.childLocator = base.GetModelChildLocator();
            this.modelBaseTransform = base.GetModelBaseTransform();
            this.animator = base.GetModelAnimator();
            bool grounded = base.characterMotor.isGrounded;

            base.PlayAnimation("Gesture, Override", "HammerSwing", "HammerSwing.playbackRate", this.duration);

            Animator hammerAnim = null;
            if (this.childLocator.FindChild("Hammer"))
            {
                hammerAnim = base.GetModelChildLocator().FindChild("Hammer").GetComponentInChildren<Animator>();
            if (hammerAnim) {
                PlayAnimationOnAnimator(hammerAnim, "Base Layer", "HammerSwing", "HammerSwing.playbackRate", this.duration);
            }
            }

            if (base.HasBuff(EnforcerPlugin.EnforcerPlugin.jackBoots) || base.HasBuff(EnforcerPlugin.EnforcerPlugin.energyShieldBuff)) {
                this.duration = HammerSwing.baseShieldDuration / this.attackSpeedStat;

                base.PlayAnimation("RightArm, Override", "HammerSwing", "HammerSwing.playbackRate", this.duration);
            } else {
                this.duration = HammerSwing.baseDuration / this.attackSpeedStat;

                base.PlayAnimation("Gesture, Override", "HammerSwing", "HammerSwing.playbackRate", this.duration);
            }

            HitBoxGroup hitBoxGroup = Array.Find<HitBoxGroup>(base.GetModelTransform().GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == "Hammer");

            float dmg = HammerSwing.damageCoefficient;

            this.attack = new OverlapAttack();
            this.attack.damageType = DamageType.Generic;
            this.attack.attacker = base.gameObject;
            this.attack.inflictor = base.gameObject;
            this.attack.teamIndex = base.GetTeam();
            this.attack.damage = dmg * this.damageStat;
            this.attack.procCoefficient = 1;
            this.attack.hitEffectPrefab = EnforcerPlugin.Assets.hammerImpactFX;
            this.attack.forceVector = Vector3.zero;
            this.attack.pushAwayForce = 800f;
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

            if (this.stopwatch >= this.duration * 0.5f && this.stopwatch <= this.duration * 0.8)
            {
                this.FireAttack();
            }


            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public void FireAttack()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                Util.PlayScaledSound(EnforcerPlugin.Sounds.NemesisSwing, base.gameObject, 0.25f + this.attackSpeedStat);

                base.AddRecoil(-1f * HammerSwing.attackRecoil, -2f * HammerSwing.attackRecoil, -0.5f * HammerSwing.attackRecoil, 0.5f * HammerSwing.attackRecoil);

                string muzzleString = "ShieldHitbox";
                EffectManager.SimpleMuzzleFlash(EnforcerPlugin.Assets.hammerSwingFX, base.gameObject, muzzleString, true);

                //Vector3 sex = this.childLocator.FindChild("SlamEffectCenter").transform.position;

                //EffectData effectData = new EffectData();
                //effectData.origin = sex - Vector3.up;
                //effectData.scale = 1;

                //EffectManager.SpawnEffect(slamPrefab, effectData, true);

            }

            if (base.isAuthority)
            {
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
                        this.hitPauseTimer = (1.5f * EntityStates.Merc.GroundLight.hitPauseDuration) / this.attackSpeedStat;
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

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}