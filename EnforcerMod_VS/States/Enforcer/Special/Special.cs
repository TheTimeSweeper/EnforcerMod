using System;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;
using Modules;
using Modules.Characters;

namespace EntityStates.Enforcer {
    public class ProtectAndServe : BaseSkillState
    {
        public static float enterDuration = 0.5f;
        public static float exitDuration = 0.6f;
        public static float bonusMass = 15000;

        private float duration;
        private EnforcerComponent shieldComponent;
        private Animator animator;
        private ChildLocator childLocator;
        private EnforcerWeaponComponent weaponComponent;

        public override void OnEnter() {
            base.OnEnter();
            this.animator = GetModelAnimator();
            this.shieldComponent = base.characterBody.GetComponent<EnforcerComponent>();
            this.childLocator = base.GetModelChildLocator();
            this.weaponComponent = base.GetComponent<EnforcerWeaponComponent>();

            //bool isEngi = base.characterBody.skinIndex == EnforcerPlugin.EnforcerModPlugin.engiIndex && Config.cursed.Value;
            //if (EnforcerPlugin.EnforcerPlugin.oldEngiShield.Value) isEngi = false;
            //bool isDoom = base.characterBody.skinIndex == EnforcerPlugin.EnforcerModPlugin.doomGuyIndex;

            if (base.HasBuff(Buffs.protectAndServeBuff))
            {
                this.duration = ProtectAndServe.exitDuration / this.attackSpeedStat;

                this.shieldComponent.isShielding = false;

                base.PlayAnimation("Gesture, Override", "BufferEmpty");
                base.PlayAnimation("Grenade, Override", "BufferEmpty");
                base.PlayAnimation("FullBody, Override", "BufferEmpty");
                base.PlayAnimation("Shield", "ShieldDown", "ShieldMode.playbackRate", this.duration);

                this.childLocator.FindChild("ShieldHurtboxParent").gameObject.SetActive(false);

                /*if (isEngi)
                {
                    this.childLocator.FindChild("BungusShield").gameObject.SetActive(false);
                }

                if (isDoom)
                {
                    this.childLocator.FindChild("MarauderShield").gameObject.SetActive(false);
                }*/

                if (base.skillLocator)
                {
                    base.skillLocator.special.SetBaseSkill(EnforcerSurvivor.shieldEnterDef);
                }

                if (base.characterMotor) base.characterMotor.mass = 200f;

                if (NetworkServer.active)
                {
                    base.characterBody.RemoveBuff(Buffs.protectAndServeBuff);
                }

                string soundString = Sounds.ShieldDown;
                //if (isEngi || isDoom) soundString = EnforcerPlugin.Sounds.EnergyShieldDown;

                Util.PlaySound(soundString, base.gameObject);

                base.characterBody.aimOriginTransform = shieldComponent.origOrigin;

                //todo CUM2 delete this
                //if (this.weaponComponent)
                //{
                //    this.weaponComponent.shieldUp = false;
                //    this.weaponComponent.UpdateCamera();
                //}
            }
            else
            {
                this.duration = ProtectAndServe.enterDuration / this.attackSpeedStat;

                this.shieldComponent.isShielding = true;

                base.PlayAnimation("Gesture, Override", "BufferEmpty");
                base.PlayAnimation("Grenade, Override", "BufferEmpty");
                base.PlayAnimation("FullBody, Override", "BufferEmpty");
                base.PlayAnimation("Shield", "ShieldUp", "ShieldMode.playbackRate", this.duration);

                this.childLocator.FindChild("ShieldHurtboxParent").gameObject.SetActive(true);

                /*if (isEngi)
                {
                    this.childLocator.FindChild("BungusShield").gameObject.SetActive(true);
                }

                if (isDoom)
                {
                    this.childLocator.FindChild("MarauderShield").gameObject.SetActive(true);
                }*/

                if (base.skillLocator)
                {
                    base.skillLocator.special.SetBaseSkill(EnforcerSurvivor.shieldExitDef);
                }

                if (base.characterMotor) base.characterMotor.mass = ProtectAndServe.bonusMass;

                if (NetworkServer.active)
                {
                    base.characterBody.AddBuff(Buffs.protectAndServeBuff);
                }

                string soundString = Sounds.ShieldUp;
                //if (isEngi || isDoom) soundString = EnforcerPlugin.Sounds.EnergyShieldUp;

                Util.PlaySound(soundString, base.gameObject); 

                characterBody.aimOriginTransform = childLocator.FindChild("ShieldAimOrigin");

                //todo CUM2 delete this
                //if (this.weaponComponent)
                //{
                //    this.weaponComponent.shieldUp = true;
                //    this.weaponComponent.UpdateCamera();
                //}
            }
        }

        public override void OnExit()
        {
            if (base.characterBody)
            {
                base.characterBody.SetSpreadBloom(0f, false);
            }
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
            if (base.HasBuff(Buffs.protectAndServeBuff)) return InterruptPriority.PrioritySkill;
            else return InterruptPriority.Skill;
        }
    }
}