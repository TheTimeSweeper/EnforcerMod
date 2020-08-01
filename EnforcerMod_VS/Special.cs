using System;
using UnityEngine;
using UnityEngine.Networking;

namespace EntityStates.Enforcer
{
    public class ProtectAndServe : BaseSkillState
    {
        public float enterDuration = 0.3f;
        public float exitDuration = 0.4f;

        private float duration;
        private ShieldComponent shieldComponent;
        private bool sprintBuffer;
        private Animator animator;

        public override void OnEnter()
        {
            base.OnEnter();

            this.shieldComponent = base.characterBody.GetComponent<ShieldComponent>();

            //I like Ternary Operators -ts
            this.duration = this.shieldComponent.isShielding? this.exitDuration : this.enterDuration;

            //if (shieldComponent.isShielding)
            //    this.duration = this.exitDuration;
            //else
            //    this.duration = this.enterDuration; 
            

            this.duration /= this.attackSpeedStat;

            Ray aimRay = base.GetAimRay();
            base.StartAimMode(aimRay, 2f, false);
            this.animator = GetModelAnimator();

            if (base.isAuthority)
            {
                this.shieldComponent.isShielding = !this.shieldComponent.isShielding;

                if (this.shieldComponent.isAlternate)
                {
                    this.shieldComponent.toggleEngergyShield();
                }

                if (this.shieldComponent.isShielding)
                {
                    if (NetworkServer.active) base.characterBody.AddBuff(EnforcerPlugin.EnforcerPlugin.jackBoots);

                    //base.PlayAnimation("Gesture, Override", "ShieldUp", "ShieldUp.playbackRate", this.duration);

                    if (base.skillLocator)
                    {
                        base.skillLocator.special.SetBaseSkill(EnforcerPlugin.EnforcerPlugin.shieldUpDef);
                        if (this.shieldComponent.isAlternate)
                        {
                            base.skillLocator.special.SetBaseSkill(EnforcerPlugin.EnforcerPlugin.shieldOnDef);
                        }
                    }

                    this.playShieldAnimation(true);
                }
                else
                {
                    //base.PlayAnimation("Gesture, Override", "ShieldDown", "ShieldUp.playbackRate", this.duration);
                    if (base.skillLocator)
                    {
                        base.skillLocator.special.SetBaseSkill(EnforcerPlugin.EnforcerPlugin.shieldDownDef);
                        if (this.shieldComponent.isAlternate)
                        {
                            base.skillLocator.special.SetBaseSkill(EnforcerPlugin.EnforcerPlugin.shieldOffDef);
                        }
                    }
                    this.playShieldAnimation(false);
                }
            }
        }

        private void playShieldAnimation(bool setting)
        {
            this.animator.SetBool("shieldUp", setting);
        }

        public override void OnExit()
        {
            if (NetworkServer.active)
            {
                if (!this.shieldComponent.isShielding)
                {
                    base.characterBody.RemoveBuff(EnforcerPlugin.EnforcerPlugin.jackBoots);
                }
            }


            if (base.characterMotor) base.characterMotor.mass = 100f;

            if (this.sprintBuffer) base.characterBody.isSprinting = true;

            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.inputBank)
            {
                if (base.inputBank.sprint.down) this.sprintBuffer = true;
            }
                
            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
    }
}