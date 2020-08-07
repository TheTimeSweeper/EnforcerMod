using RoR2;
using UnityEngine;

namespace EntityStates.Enforcer
{
    public class DefaultDance : BaseSkillState
    {
        public static string soundString = EnforcerPlugin.Sounds.DefaultDance;

        private uint activePlayID;
        private Animator animator;
        private ChildLocator childLocator;

        public override void OnEnter()
        {
            base.OnEnter();
            this.animator = base.GetModelAnimator();
            this.childLocator = base.GetModelTransform().GetComponent<ChildLocator>();

            base.characterBody.hideCrosshair = true;

            if (base.characterMotor) base.characterMotor.velocity = Vector3.zero;

            base.PlayAnimation("FullBody, Override", "DefaultDance");
            this.activePlayID = Util.PlaySound(soundString, base.gameObject);

            //no don't comment this out it looks fucking stupid with the shield
            this.ToggleShield(false);
        }

        public override void OnExit()
        {
            base.OnExit();

            base.characterBody.hideCrosshair = false;

            base.PlayAnimation("FullBody, Override", "BufferEmpty");
            if (this.activePlayID != 0) AkSoundEngine.StopPlayingID(this.activePlayID);
            this.ToggleShield(true);
        }

        private void ToggleShield(bool sex)
        {
            if (this.childLocator) this.childLocator.FindChild("Shield").gameObject.SetActive(sex);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            bool flag = false;

            if (base.characterMotor)
            {
                if (!base.characterMotor.isGrounded) flag = true;
                if (base.characterMotor.velocity != Vector3.zero) flag = true;
            }

            if (base.inputBank)
            {
                if (base.inputBank.skill1.down) flag = true;
                if (base.inputBank.skill2.down) flag = true;
                if (base.inputBank.skill3.down) flag = true;
                if (base.inputBank.skill4.down) flag = true;

                if (base.inputBank.moveVector != Vector3.zero) flag = true;
            }

            if (flag)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Any;
        }
    }
}