using System;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;

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
        private ChildLocator childLocator;

        public override void OnEnter()
        {
            base.OnEnter();
            this.animator = GetModelAnimator();
            this.shieldComponent = base.characterBody.GetComponent<ShieldComponent>();
            this.childLocator = base.GetModelChildLocator();

            bool isEngi = base.characterBody.skinIndex == EnforcerPlugin.EnforcerPlugin.engiIndex;

            if (base.HasBuff(EnforcerPlugin.EnforcerPlugin.jackBoots))
            {
                this.duration = ProtectAndServe.exitDuration / this.attackSpeedStat;

                this.shieldComponent.isShielding = false;

                base.PlayAnimation("FullBody, Override", "ShieldDown", "ShieldUp.playbackRate", this.duration);
                //base.PlayAnimation("LeftArm, Override", "ShieldDown", "ShieldUp.playbackRate", this.duration);
                //base.GetModelAnimator().SetBool("shieldUp", false);

                this.childLocator.FindChild("ShieldHurtbox").gameObject.SetActive(false);

                if (isEngi) this.childLocator.FindChild("EngiShield").gameObject.SetActive(false);

                if (base.skillLocator)
                {
                    base.skillLocator.special.SetBaseSkill(EnforcerPlugin.EnforcerPlugin.shieldDownDef);
                }

                if (base.characterMotor) base.characterMotor.mass = 200f;

                if (NetworkServer.active)
                {
                    base.characterBody.RemoveBuff(EnforcerPlugin.EnforcerPlugin.jackBoots);
                }

                string soundString = EnforcerPlugin.Sounds.ShieldDown;
                if (isEngi) soundString = EnforcerPlugin.Sounds.EnergyShieldDown;

                Util.PlaySound(soundString, base.gameObject);

                characterBody.aimOriginTransform = shieldComponent.origOrigin;
            }
            else
            {
                this.duration = ProtectAndServe.enterDuration / this.attackSpeedStat;

                this.shieldComponent.isShielding = true;

                base.PlayAnimation("RightArm, Override", "BufferEmpty");
                base.PlayAnimation("FullBody, Override", "ShieldUp", "ShieldUp.playbackRate", this.duration);
                //base.PlayAnimation("LeftArm, Override", "ShieldUp", "ShieldUp.playbackRate", this.duration);
                //base.GetModelAnimator().SetBool("shieldUp", true);

                this.childLocator.FindChild("ShieldHurtbox").gameObject.SetActive(true);

                if (isEngi) this.childLocator.FindChild("EngiShield").gameObject.SetActive(true);

                if (base.skillLocator)
                {
                    base.skillLocator.special.SetBaseSkill(EnforcerPlugin.EnforcerPlugin.shieldUpDef);
                }

                if (base.characterMotor) base.characterMotor.mass = ProtectAndServe.bonusMass;

                if (NetworkServer.active)
                {
                    base.characterBody.AddBuff(EnforcerPlugin.EnforcerPlugin.jackBoots);
                }

                string soundString = EnforcerPlugin.Sounds.ShieldUp;
                if (isEngi) soundString = EnforcerPlugin.Sounds.EnergyShieldUp;

                Util.PlaySound(soundString, base.gameObject);

                characterBody.aimOriginTransform = childLocator.FindChild("ShieldHitbox");
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
            if (base.HasBuff(EnforcerPlugin.EnforcerPlugin.jackBoots)) return InterruptPriority.PrioritySkill;
            else return InterruptPriority.Skill;
        }
    }
}