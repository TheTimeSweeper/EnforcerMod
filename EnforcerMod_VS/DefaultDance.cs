using RoR2;
using UnityEngine;

namespace EntityStates.Enforcer
{
    public class BaseEmote : BaseState
    {
        public string soundString;
        public string animString;

        private uint activePlayID;
        private float initialTime;
        private Animator animator;
        private ChildLocator childLocator;

        public override void OnEnter()
        {
            base.OnEnter();
            this.animator = base.GetModelAnimator();
            this.childLocator = base.GetModelTransform().GetComponent<ChildLocator>();

            base.characterBody.hideCrosshair = true;

            if (base.characterMotor) base.characterMotor.velocity = Vector3.zero;

            base.PlayAnimation("FullBody, Override", this.animString);
            this.activePlayID = Util.PlaySound(soundString, base.gameObject);

            this.initialTime = Time.fixedTime;

            if (base.GetComponent<EnforcerWeaponComponent>()) base.GetComponent<EnforcerWeaponComponent>().HideWeapon();

            this.ToggleShield(false);
        }

        public override void OnExit()
        {
            base.OnExit();

            base.characterBody.hideCrosshair = false;

            if (base.GetComponent<EnforcerWeaponComponent>()) base.GetComponent<EnforcerWeaponComponent>().ResetWeapon();

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

            CameraTargetParams ctp = base.cameraTargetParams;
            float denom = (1 + Time.fixedTime - this.initialTime);
            float smoothFactor = 8 / Mathf.Pow(denom, 2);
            Vector3 smoothVector = new Vector3(-3 / 20, 1 / 16, -1);
            ctp.idealLocalCameraPos = new Vector3(0f, -1.4f, -6f) + smoothFactor * smoothVector;

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

    public class DefaultDance : BaseEmote
    {
        public override void OnEnter()
        {
            this.animString = "DefaultDance";
            this.soundString = EnforcerPlugin.Sounds.DefaultDance;
            base.OnEnter();
        }
    }

    public class Floss : BaseEmote
    {
        public override void OnEnter()
        {
            this.animString = "Floss";
            this.soundString = EnforcerPlugin.Sounds.Floss;
            base.OnEnter();
        }
    }

    public class InfiniteDab : BaseEmote
    {
        public override void OnEnter()
        {
            this.animString = "InfiniteDab";
            this.soundString = EnforcerPlugin.Sounds.InfiniteDab;
            base.OnEnter();
        }
    }
}