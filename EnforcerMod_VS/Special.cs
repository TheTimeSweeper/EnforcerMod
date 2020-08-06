using System;
using UnityEngine;
using UnityEngine.Networking;

namespace EntityStates.Enforcer
{
    public class ProtectAndServe : BaseSkillState
    {
        public static float enterDuration = 0.5f;
        public static float exitDuration = 0.6f;
        public static float bonusMass = 15000;

        private float duration;
        private ShieldComponent shieldComponent;
        private Animator animator;

        public override void OnEnter()
        {
            base.OnEnter();
            this.animator = GetModelAnimator();

            this.shieldComponent = base.characterBody.GetComponent<ShieldComponent>();

            if (base.HasBuff(EnforcerPlugin.EnforcerPlugin.jackBoots))
            {
                this.duration = ProtectAndServe.exitDuration / this.attackSpeedStat;

                this.shieldComponent.isShielding = false;

                this.playShieldAnimation(false);

                if (base.skillLocator)
                {
                    base.skillLocator.special.SetBaseSkill(EnforcerPlugin.EnforcerPlugin.shieldDownDef);
                }

                if (base.characterMotor) base.characterMotor.mass = 200f;

                if (NetworkServer.active)
                {
                    base.characterBody.RemoveBuff(EnforcerPlugin.EnforcerPlugin.jackBoots);
                }
            }
            else
            {
                this.duration = ProtectAndServe.enterDuration / this.attackSpeedStat;

                this.shieldComponent.isShielding = true;

                this.playShieldAnimation(true);

                if (base.skillLocator)
                {
                    base.skillLocator.special.SetBaseSkill(EnforcerPlugin.EnforcerPlugin.shieldUpDef);
                }

                if (base.characterMotor) base.characterMotor.mass = ProtectAndServe.bonusMass;

                if (NetworkServer.active)
                {
                    base.characterBody.AddBuff(EnforcerPlugin.EnforcerPlugin.jackBoots);
                }
            }
        }

        private void playShieldAnimation(bool setting)
        {
            this.animator.SetBool("shieldUp", setting);
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
            return InterruptPriority.PrioritySkill;
        }
    }
}