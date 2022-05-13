using System;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;
using Modules;
using Modules.Characters;

namespace EntityStates.Enforcer {
    public class EnergyShield : BaseSkillState
    {
        public static float enterDuration = 0.7f;
        public static float exitDuration = 0.9f;
        public static float bonusMass = 15000;

        private float duration;
        private EnforcerComponent shieldComponent;
        private Animator animator;
        private ChildLocator childLocator;

        public override void OnEnter()
        {
            base.OnEnter();
            this.animator = GetModelAnimator();
            this.shieldComponent = base.characterBody.GetComponent<EnforcerComponent>();
            this.childLocator = base.GetModelTransform().GetComponent<ChildLocator>();

            this.shieldComponent.isShielding = !base.HasBuff(Buffs.energyShieldBuff);

            if (base.HasBuff(Buffs.energyShieldBuff))
            {
                this.duration = EnergyShield.exitDuration / this.attackSpeedStat;
                this.EnableEnergyShield(false);

                base.PlayAnimation("LeftArm, Override", "ShieldDown", "ShieldUp.playbackRate", this.duration);

                if (base.skillLocator)
                {
                    base.skillLocator.special.SetBaseSkill(EnforcerSurvivor.energyShieldEnterDef);
                }

                if (base.characterMotor) base.characterMotor.mass = 200f;

                if (NetworkServer.active)
                {
                    base.characterBody.RemoveBuff(Buffs.energyShieldBuff);
                }

                Util.PlaySound(Sounds.EnergyShieldDown, base.gameObject);
            }
            else
            {
                this.duration = EnergyShield.enterDuration / this.attackSpeedStat;
                this.EnableEnergyShield(true);

                base.PlayAnimation("LeftArm, Override", "ShieldUp", "ShieldUp.playbackRate", this.duration);

                if (base.skillLocator)
                {
                    base.skillLocator.special.SetBaseSkill(EnforcerSurvivor.energyShieldExitDef);
                }

                if (base.characterMotor) base.characterMotor.mass = EnergyShield.bonusMass;

                if (NetworkServer.active)
                {
                    base.characterBody.AddBuff(Buffs.energyShieldBuff);
                }

                Util.PlaySound(Sounds.EnergyShieldUp, base.gameObject);
            }
        }

        private void EnableEnergyShield(bool what)
        {
            if (this.childLocator)
            {
                this.childLocator.FindChild("EnergyShield").gameObject.SetActive(what);
            }
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