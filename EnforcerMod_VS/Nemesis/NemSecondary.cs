using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace EntityStates.Nemforcer
{
    public class HammerCharge : BaseSkillState
    {
        public static float baseChargeDuration = 2.75f;

        private float chargeDuration;
        private bool finishedCharge;
        private ChildLocator childLocator;
        private Animator animator;
        private Transform modelBaseTransform;

        public override void OnEnter()
        {
            base.OnEnter();
            this.chargeDuration = HammerCharge.baseChargeDuration / this.attackSpeedStat;
            this.childLocator = base.GetModelChildLocator();
            this.modelBaseTransform = base.GetModelBaseTransform();
            this.animator = base.GetModelAnimator();
            base.PlayAnimation("Gesture, Override", "HammerCharge", "HammerCharge.playbackRate", this.chargeDuration);

            if (base.cameraTargetParams)
            {
                base.cameraTargetParams.aimMode = CameraTargetParams.AimType.AimThrow;
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            float charge = this.CalcCharge();

            if (charge >= 1f && !this.finishedCharge)
            {
                this.finishedCharge = true;
                Util.PlaySound(EntityStates.Captain.Weapon.FireCaptainShotgun.tightSoundString, base.gameObject);

                if (base.cameraTargetParams)
                {
                    base.cameraTargetParams.aimMode = CameraTargetParams.AimType.Standard;
                }
            }

            if (base.isAuthority && ((!base.IsKeyDownAuthority() && base.fixedAge >= 0.1f)) && !base.IsKeyDownAuthority())
            {
                HammerUppercut nextState = new HammerUppercut();
                nextState.charge = charge;
                this.outer.SetNextState(nextState);
            }
        }

        protected float CalcCharge()
        {
            return Mathf.Clamp01(base.fixedAge / this.chargeDuration);
        }

        public override void OnExit()
        {
            base.OnExit();

            base.PlayAnimation("Gesture, Override", "BufferEmpty");

            if (base.cameraTargetParams)
            {
                base.cameraTargetParams.aimMode = CameraTargetParams.AimType.Standard;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }

    public class HammerUppercut : BaseSkillState
    {
        public float charge;
        public static string hitboxString = "UppercutHitbox";
        public static float maxDamageCoefficient = 25f;
        public static float minDamageCoefficient = 3f;
        public static float procCoefficient = 1f;
        public static float maxRecoil = 5f;
        public static float minRecoil = 0.4f;
        public static float initialMaxSpeedCoefficient = 12f;
        public static float initialMinSpeedCoefficient = 4f;
        public static float finalSpeedCoefficient = 0.1f;
        public static float baseDuration = 0.6f;
        public static float knockupForce = 5000f;
        public static float hopVelocity = 15f;

        private float speedCoefficient;
        private float damageCoefficient;
        private float recoil;
        private float duration;

        private float dashSpeed;
        private Vector3 forwardDirection;
        private Vector3 previousPosition;

        private float stopwatch;
        private ChildLocator childLocator;
        private bool hasFired;
        private float hitPauseTimer;
        private OverlapAttack attack;
        private bool inHitPause;
        private Animator animator;
        private BaseState.HitStopCachedState hitStopCachedState;
        private Transform modelBaseTransform;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = HammerUppercut.baseDuration / this.attackSpeedStat;
            this.stopwatch = 0f;
            this.hasFired = false;
            base.characterBody.isSprinting = true;
            this.speedCoefficient = Util.Remap(this.charge, 0f, 1f, HammerUppercut.initialMinSpeedCoefficient, HammerUppercut.initialMaxSpeedCoefficient);
            this.damageCoefficient = Util.Remap(this.charge, 0f, 1f, HammerUppercut.minDamageCoefficient, HammerUppercut.maxDamageCoefficient);
            this.recoil = Util.Remap(this.charge, 0f, 1f, HammerUppercut.minRecoil, HammerUppercut.maxRecoil);

            this.childLocator = base.GetModelChildLocator();
            this.modelBaseTransform = base.GetModelBaseTransform();
            this.animator = base.GetModelAnimator();

            Util.PlayScaledSound(Croco.Leap.leapSoundString, base.gameObject, 1.95f);

            if (base.isAuthority && base.inputBank && base.characterDirection)
            {
                this.forwardDirection = ((base.inputBank.moveVector == Vector3.zero) ? base.characterDirection.forward : base.inputBank.moveVector).normalized;
            }

            this.RecalculateSpeed();

            if (base.characterMotor && base.characterDirection)
            {
                base.characterMotor.velocity.y *= 0.1f;
                base.characterMotor.velocity = this.forwardDirection * this.dashSpeed;
            }

            Vector3 b = base.characterMotor ? base.characterMotor.velocity : Vector3.zero;
            this.previousPosition = base.transform.position - b;

            HitBoxGroup hitBoxGroup = Array.Find<HitBoxGroup>(base.GetModelTransform().GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == "Uppercut");

            base.PlayAnimation("FullBody, Override", "Uppercut", "Uppercut.playbackRate", this.duration);
            //base.PlayAnimation("Legs, Override", "SwingLegs", "HammerSwing.playbackRate", this.duration);

            this.attack = new OverlapAttack();
            this.attack.damageType = DamageType.Stun1s;
            this.attack.attacker = base.gameObject;
            this.attack.inflictor = base.gameObject;
            this.attack.teamIndex = base.GetTeam();
            this.attack.damage = this.damageCoefficient * this.damageStat;
            this.attack.procCoefficient = 1;
            this.attack.hitEffectPrefab = EnforcerPlugin.Assets.nemImpactFX;
            this.attack.forceVector = Vector3.up * HammerUppercut.knockupForce;
            this.attack.pushAwayForce = 50f;
            this.attack.hitBoxGroup = hitBoxGroup;
            this.attack.isCrit = base.RollCrit();

            EffectManager.SimpleMuzzleFlash(EnforcerPlugin.Assets.shoulderBashFX, base.gameObject, "SwingCenter", true);
        }

        private void RecalculateSpeed()
        {
            if (this.hasFired) this.dashSpeed = 0.1f;
            else this.dashSpeed = (4 + (0.25f * this.moveSpeedStat)) * Mathf.Lerp(this.speedCoefficient, HammerUppercut.finalSpeedCoefficient, this.stopwatch / this.duration);
        }

        public override void OnExit()
        {
            if (base.characterMotor) base.characterMotor.disableAirControlUntilCollision = false;

            if (base.cameraTargetParams)
            {
                base.cameraTargetParams.fovOverride = -1f;
            }

            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            base.characterBody.isSprinting = true;

            if (base.fixedAge >= this.duration)
            {
                this.outer.SetNextStateToMain();
                return;
            }

            this.RecalculateSpeed();

            if (base.cameraTargetParams)
            {
                base.cameraTargetParams.fovOverride = Mathf.Lerp(Commando.DodgeState.dodgeFOV, 60f, this.stopwatch / this.duration);
            }

            if (base.isAuthority)
            {
                if (!this.inHitPause)
                {
                    this.stopwatch += Time.fixedDeltaTime;
                    Vector3 normalized = (base.transform.position - this.previousPosition).normalized;

                    if (base.characterDirection)
                    {
                        if (normalized != Vector3.zero)
                        {
                            Vector3 vector = normalized * this.dashSpeed;
                            float d = Mathf.Max(Vector3.Dot(vector, this.forwardDirection), 0f);
                            vector = this.forwardDirection * d;
                            vector.y = base.characterMotor.velocity.y;
                            base.characterMotor.velocity = vector;
                        }

                        base.characterDirection.forward = this.forwardDirection;
                    }

                    this.previousPosition = base.transform.position;

                    if (this.stopwatch >= (0.35f * this.duration))
                    {
                        if (!this.hasFired)
                        {
                            this.hasFired = true;

                            base.SmallHop(base.characterMotor, HammerUppercut.hopVelocity);
                            base.AddRecoil(-1f * this.recoil, -2f * this.recoil, -0.5f * this.recoil, 0.5f * this.recoil);
                        }
                    }
                    else
                    {
                        if (this.attack.Fire())
                        {
                            Util.PlaySound(EnforcerPlugin.Sounds.ShoulderBashHit, base.gameObject);
                            this.hitStopCachedState = base.CreateHitStopCachedState(base.characterMotor, this.animator, "HammerSwing.playbackRate");
                            this.inHitPause = true;
                            this.hitPauseTimer = (3f * EntityStates.Merc.GroundLight.hitPauseDuration) / this.attackSpeedStat;
                        }

                        base.characterMotor.velocity.y *= 0.1f;
                    }
                }
                else
                {
                    base.characterMotor.velocity = Vector3.zero;
                    this.hitPauseTimer -= Time.fixedDeltaTime;
                    if (this.hitPauseTimer < 0f)
                    {
                        base.ConsumeHitStopCachedState(this.hitStopCachedState, base.characterMotor, this.animator);
                        this.inHitPause = false;
                    }
                }
            }
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(this.forwardDirection);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            this.forwardDirection = reader.ReadVector3();
        }
    }
}