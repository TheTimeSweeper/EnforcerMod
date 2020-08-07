using System;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;

namespace EntityStates.Enforcer
{
    public class EnergyShield : BaseSkillState
    {
        public static float enterDuration = 0.7f;
        public static float exitDuration = 0.9f;
        public static float bonusMass = 15000;

        private float duration;
        private ShieldComponent shieldComponent;
        private Animator animator;
        private ChildLocator childLocator;

        public override void OnEnter()
        {
            base.OnEnter();
            this.animator = GetModelAnimator();
            this.shieldComponent = base.characterBody.GetComponent<ShieldComponent>();
            this.childLocator = base.GetModelTransform().GetComponent<ChildLocator>();

            this.shieldComponent.isShielding = !base.HasBuff(EnforcerPlugin.EnforcerPlugin.energyShieldBuff);

            if (base.HasBuff(EnforcerPlugin.EnforcerPlugin.energyShieldBuff))
            {
                this.duration = EnergyShield.exitDuration / this.attackSpeedStat;
                this.EnableEnergyShield(false);
                this.playShieldAnimation(false);

                if (base.skillLocator)
                {
                    base.skillLocator.special.SetBaseSkill(EnforcerPlugin.EnforcerPlugin.shieldOffDef);
                }

                if (base.characterMotor) base.characterMotor.mass = 200f;

                if (NetworkServer.active)
                {
                    base.characterBody.RemoveBuff(EnforcerPlugin.EnforcerPlugin.energyShieldBuff);
                }

                Util.PlaySound(EnforcerPlugin.Sounds.EnergyShieldDown, base.gameObject);
            }
            else
            {
                this.duration = EnergyShield.enterDuration / this.attackSpeedStat;
                this.EnableEnergyShield(true);
                this.playShieldAnimation(true);

                if (base.skillLocator)
                {
                    base.skillLocator.special.SetBaseSkill(EnforcerPlugin.EnforcerPlugin.shieldOnDef);
                }

                if (base.characterMotor) base.characterMotor.mass = EnergyShield.bonusMass;

                if (NetworkServer.active)
                {
                    base.characterBody.AddBuff(EnforcerPlugin.EnforcerPlugin.energyShieldBuff);
                }

                Util.PlaySound(EnforcerPlugin.Sounds.EnergyShieldUp, base.gameObject);
            }
        }

        private void EnableEnergyShield(bool what)
        {
            if (this.childLocator)
            {
                this.childLocator.FindChild("EnergyShield").gameObject.SetActive(what);
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
            return InterruptPriority.Death;
        }
    }
}