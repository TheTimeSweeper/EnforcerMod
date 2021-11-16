using UnityEngine;
using UnityEngine.Networking;
using RoR2;

namespace EntityStates.Enforcer
{
    public class Skateboard : BaseSkillState
    {
        public static float enterDuration = 0.5f;
        public static float exitDuration = 0.6f;
        public static float hopHeight = 10f;

        private float duration;
        private Animator animator;
        private ChildLocator childLocator;
        private EnforcerWeaponComponent weaponComponent;

        public override void OnEnter()
        {
            base.OnEnter();
            this.animator = GetModelAnimator();
            this.childLocator = base.GetModelChildLocator();
            this.weaponComponent = base.GetComponent<EnforcerWeaponComponent>();

            if (base.isAuthority && base.characterMotor.isGrounded)
            {
                base.SmallHop(base.characterMotor, Skateboard.hopHeight);
            }

            if (base.HasBuff(EnforcerPlugin.Modules.Buffs.skateboardBuff))
            {
                base.modelLocator.normalizeToFloor = false;
                this.duration = Skateboard.exitDuration / this.attackSpeedStat;

                base.PlayAnimation("SkateBoard, Override", "BoardDown", "BoardUp.playbackRate", this.duration);

                if (base.skillLocator)
                {
                    base.skillLocator.special.SetBaseSkill(EnforcerPlugin.EnforcerModPlugin.boardDownDef);
                }

                if (NetworkServer.active)
                {
                    base.characterBody.RemoveBuff(EnforcerPlugin.Modules.Buffs.skateboardBuff);
                }

                string soundString = EnforcerPlugin.Sounds.ShieldDown;

                if (this.weaponComponent) this.weaponComponent.ReparentSkateboard(EnforcerWeaponComponent.SkateBoardParent.HAND);

                Util.PlayAttackSpeedSound(soundString, base.gameObject, 2f);
            }
            else
            {
                base.modelLocator.normalizeToFloor = true;
                this.duration = Skateboard.enterDuration / this.attackSpeedStat;

                base.PlayAnimation("SkateBoard, Override", "BoardUp", "BoardUp.playbackRate", this.duration);

                if (base.skillLocator)
                {
                    base.skillLocator.special.SetBaseSkill(EnforcerPlugin.EnforcerModPlugin.boardUpDef);
                }

                if (NetworkServer.active)
                {
                    base.characterBody.AddBuff(EnforcerPlugin.Modules.Buffs.skateboardBuff);
                }

                string soundString = EnforcerPlugin.Sounds.ShieldUp;

                Util.PlayAttackSpeedSound(soundString, base.gameObject, 2f);
            }
        }

        public override void OnExit() 
        {
            base.OnExit();

            if (this.weaponComponent && base.HasBuff(EnforcerPlugin.Modules.Buffs.skateboardBuff)) {
                this.weaponComponent.ReparentSkateboard(EnforcerWeaponComponent.SkateBoardParent.BASE);
            }
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
            if (base.HasBuff(EnforcerPlugin.Modules.Buffs.skateboardBuff)) return InterruptPriority.PrioritySkill;
            else return InterruptPriority.Skill;
        }
    }
}