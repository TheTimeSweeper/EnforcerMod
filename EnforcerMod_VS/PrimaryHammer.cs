using RoR2;
using System;
using UnityEngine;

namespace EntityStates.Enforcer
{
    public class HammerSwing : BaseSkillState
    {
        public static float baseDuration = 1f;
        public static float baseShieldDuration = 1.2f;
        public static float damageCoefficient = 5f;
        public static float shieldDamageCoefficient = 10f;

        public static float baseDownwardForce = 480;
        public static float shieldDownwardForce = 800;
        public static float basePushawayForce = 120;
        public static float shieldPushawayForce = 160;

        public static GameObject slamEffectPrefab = null;// EnforcerPlugin.EnforcerPlugin.hammerSlamEffect; // Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/ParentSlamEffect");
        public static GameObject shieldSlamEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/ParentSlamEffect");

        public static float baseEarlyExitTime = 0.78f;
        public static float procCoefficient = 1f;
        public static float attackRecoil = 1.15f;
        public static float hitHopVelocity = 5.5f;

        private float duration;
        private float damage;
        private float downwardForce;
        private float pushawayForce;
        private GameObject slamPrefab;
        private float earlyExitDuration;
        private string hitboxString;

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
            this.hasFired = false;
            base.characterBody.isSprinting = false;

            this.childLocator = base.GetModelChildLocator();
            this.modelBaseTransform = base.GetModelBaseTransform();
            this.animator = base.GetModelAnimator();
            bool grounded = base.characterMotor.isGrounded;

            if (base.HasBuff(EnforcerPlugin.EnforcerPlugin.jackBoots) || base.HasBuff(EnforcerPlugin.EnforcerPlugin.energyShieldBuff)) {
                this.duration = HammerSwing.baseShieldDuration / this.attackSpeedStat;
                this.damage = HammerSwing.shieldDamageCoefficient;
                this.hitboxString = "HammerBig";
                this.downwardForce = HammerSwing.shieldDownwardForce;
                this.pushawayForce = HammerSwing.shieldPushawayForce;
                this.slamPrefab = HammerSwing.shieldSlamEffectPrefab;

                base.PlayCrossfade("RightArm, Override", "HammerSwing", "HammerSwing.playbackRate", this.duration, 0.05f);

            } else {
                this.duration = HammerSwing.baseDuration / this.attackSpeedStat;
                this.damage = HammerSwing.damageCoefficient;
                this.hitboxString = "Hammer";
                this.downwardForce = HammerSwing.baseDownwardForce;
                this.pushawayForce = HammerSwing.basePushawayForce;
                this.slamPrefab = HammerSwing.slamEffectPrefab;

                base.PlayCrossfade("Gesture, Override", "HammerSwing", "HammerSwing.playbackRate", this.duration, 0.05f);
            }

            this.earlyExitDuration = this.duration * HammerSwing.baseEarlyExitTime;

            Animator hammerAnim = null;
            if (this.childLocator.FindChild("Hammer")) {
                hammerAnim = this.childLocator.FindChild("Hammer").GetComponentInChildren<Animator>();
                if (hammerAnim) {
                    PlayAnimationOnAnimator(hammerAnim, "Base Layer", "HammerSwing", "HammerSwing.playbackRate", this.duration);
                }
            }


            HitBoxGroup hitBoxGroup = base.FindHitBoxGroup(hitboxString);

            this.attack = new OverlapAttack();
            this.attack.damageType = DamageType.Generic;
            this.attack.attacker = base.gameObject;
            this.attack.inflictor = base.gameObject;
            this.attack.teamIndex = base.GetTeam();
            this.attack.damage = this.damage * this.damageStat;
            this.attack.procCoefficient = 1;
            this.attack.hitEffectPrefab = EnforcerPlugin.Assets.hammerImpactFX;
            this.attack.forceVector = Vector3.down * this.downwardForce;
            this.attack.pushAwayForce = this.pushawayForce;
            this.attack.hitBoxGroup = hitBoxGroup;
            this.attack.isCrit = base.RollCrit();
            this.attack.impactSound = EnforcerPlugin.Assets.hammerHitSoundEvent.index;
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

            if (this.stopwatch >= this.duration * 0.48f && this.stopwatch <= this.duration * 0.7)
            {
                this.FireAttack();
            }

            if (base.fixedAge >= this.earlyExitDuration && base.inputBank.skill1.down && base.isAuthority) {
                this.outer.SetNextState(new HammerSwing());
                return;
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

                if (slamPrefab) {
                    Vector3 sex = this.childLocator.FindChild("SlamEffectCenter").transform.position;

                    EffectData effectData = new EffectData();
                    effectData.origin = sex - Vector3.up;
                    effectData.scale = 1;

                    EffectManager.SpawnEffect(slamPrefab, effectData, true);
                }
            }

            if (base.isAuthority)
            {
                if (this.attack.Fire())
                {
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