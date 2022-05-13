using EntityStates.Enforcer;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace Enforcer.Emotes {
    public class EnforcerSalute : EnforcerMain
    {
        protected string soundString;
        protected string animString;
        public float duration;
        public float animDuration;

        private uint activePlayID;
        private float initialTime;
        private Animator animator;
        private ChildLocator childLocator;

        public override void OnEnter()
        {
            base.OnEnter();
            this.animString = "Salute";
            this.soundString = "";
            this.duration = 1.8f;
            this.animDuration = 2.4f;
            this.animator = base.GetModelAnimator();
            this.childLocator = base.GetModelChildLocator();

            base.characterBody.hideCrosshair = true;

            if (base.GetAimAnimator()) base.GetAimAnimator().enabled = false;
            this.animator.SetLayerWeight(animator.GetLayerIndex("AimPitch"), 0);
            this.animator.SetLayerWeight(animator.GetLayerIndex("AimYaw"), 0);

            if (this.animDuration == 0 && this.duration != 0) this.animDuration = this.duration;

            if (this.duration > 0) base.PlayAnimation("Gesture, Override", this.animString, "Emote.playbackRate", this.animDuration);
            else
            {
                this.animator.SetFloat("Emote.playbackRate", 1f);
                base.PlayAnimation("Gesture, Override", this.animString);
            }

            if (!string.IsNullOrEmpty(soundString))
            {
                this.activePlayID = Util.PlaySound(soundString, base.gameObject);
            }

            if (NetworkServer.active) base.characterBody.AddBuff(Modules.Buffs.bigSlowBuff);

            this.initialTime = Time.fixedTime;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            base.characterBody.isSprinting = false;
            bool flag = false;

            this.animator.SetBool("inCombat", true);

            if (base.characterMotor)
            {
                if (!base.characterMotor.isGrounded) flag = true;
            }

            if (base.inputBank)
            {
                if (base.inputBank.skill1.down) flag = true;
                if (base.inputBank.skill2.down) flag = true;
                if (base.inputBank.skill3.down) flag = true;
                if (base.inputBank.skill4.down) flag = true;
                if (base.inputBank.sprint.justPressed) flag = true;
            }

            if (this.duration > 0 && base.fixedAge >= this.duration) flag = true;

            /*CameraTargetParams ctp = base.cameraTargetParams;
            float denom = (1 + Time.fixedTime - this.initialTime);
            float smoothFactor = 8 / Mathf.Pow(denom, 2);
            Vector3 smoothVector = new Vector3(-3 / 20, 1 / 16, -1);
            ctp.idealLocalCameraPos = new Vector3(0f, -1.4f, -6f) + smoothFactor * smoothVector;*/

            if (flag)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            base.characterBody.hideCrosshair = false;

            if (NetworkServer.active) base.characterBody.RemoveBuff(Modules.Buffs.bigSlowBuff);

            if (base.GetAimAnimator()) base.GetAimAnimator().enabled = true;
            this.animator.SetLayerWeight(animator.GetLayerIndex("AimPitch"), 1);
            this.animator.SetLayerWeight(animator.GetLayerIndex("AimYaw"), 1);

            base.PlayAnimation("Emote, Override", "BufferEmpty");
            if (this.activePlayID != 0) AkSoundEngine.StopPlayingID(this.activePlayID);
        }
    }
}
