using System;
using UnityEngine;

namespace EntityStates.Enforcer
{
    public class ProtectAndServe : BaseSkillState
    {
        public float enterDuration = 0.4f;
        public float exitDuration = 1.2f;

        private float duration;
        private ShieldComponent shieldComponent;
        private bool sprintBuffer;
        private Animator animator;

        public override void OnEnter()
        {
            base.OnEnter();

            this.shieldComponent = base.characterBody.GetComponent<ShieldComponent>();

            //I like Ternary Operators -ts
            duration = shieldComponent.isShielding? exitDuration : enterDuration;

            //if (shieldComponent.isShielding)
            //    this.duration = this.exitDuration;
            //else
            //    this.duration = this.enterDuration; 
            

            this.duration /= this.attackSpeedStat;

            Ray aimRay = base.GetAimRay();
            base.StartAimMode(aimRay, 2f, false);
            animator = GetModelAnimator();

            if (base.characterMotor) base.characterMotor.mass = 1500f;

            if (base.isAuthority)
            {
                shieldComponent.isShielding = !shieldComponent.isShielding;
                if (shieldComponent.isShielding)
                {
                    base.characterBody.AddBuff(EnforcerPlugin.EnforcerPlugin.jackBoots);

                    //base.PlayAnimation("Gesture, Override", "ShieldUp", "ShieldUp.playbackRate", this.duration);

                    if (base.skillLocator) base.skillLocator.special.skillDef.icon = EnforcerPlugin.Assets.icon4B;

                    playShieldAnimation(true);
                }
                else
                {
                    //base.PlayAnimation("Gesture, Override", "ShieldDown", "ShieldUp.playbackRate", this.duration);
                    if (base.skillLocator) base.skillLocator.special.skillDef.icon = EnforcerPlugin.Assets.icon4;
                    playShieldAnimation(false);

                }
            }
        }

        private void playShieldAnimation(bool setting) {
            animator.SetBool("shieldUp", setting);
        }

        public override void OnExit()
        {
            if (!shieldComponent.isShielding)
            {
                base.characterBody.RemoveBuff(EnforcerPlugin.EnforcerPlugin.jackBoots);
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