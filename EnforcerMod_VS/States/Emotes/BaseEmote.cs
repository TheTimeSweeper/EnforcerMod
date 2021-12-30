using Enforcer.Emotes;
using EntityStates.Nemforcer.Emotes;
using RoR2;
using UnityEngine;
using EnforcerPlugin.Modules;

namespace EntityStates.Enforcer
{
    public class BaseEmote : BaseState
    {
        private Animator animator;
        private ChildLocator childLocator;
        private MemeRigController memeRig;
        private EnforcerWeaponComponent weaponComponent;

        //protected string soundString;
        //protected string animString;
        private float duration;
        //private float animDuration;

        private uint activePlayID;
        private float cameraInitialTime;

        public override void OnEnter()
        {
            base.OnEnter();
            //init
            this.animator = base.GetModelAnimator();
            this.childLocator = base.GetModelChildLocator();
            this.memeRig = base.GetModelTransform().GetComponent<MemeRigController>();
            this.weaponComponent = base.GetComponent<EnforcerWeaponComponent>();

            this.cameraInitialTime = Time.fixedTime;

            //hide shit
            HideShit();

            //do shit
            //if (base.characterBody.skinIndex == EnforcerPlugin.EnforcerPlugin.doomGuyIndex && base.characterBody.baseNameToken == "ENFORCER_NAME")
            //    soundString = EnforcerPlugin.Sounds.DOOM;
        }


        protected void PlayEmote(string animString, string soundString = "", float animDuration = 0)
        {
            PlayEmote(animString, soundString, GetModelAnimator(), animDuration);
        }
        protected void PlayEmote(string animString, string soundString, Animator animator, float animDuration = 0)
        {
            if (animDuration >= 0 && this.duration != 0)
                animDuration = this.duration;

            if (duration > 0)
            {
                PlayAnimationOnAnimator(animator, "FullBody, Override", animString, "Emote.playbackRate", animDuration);
            }
            else
            {
                animator.SetFloat("Emote.playbackRate", 1f);
                PlayAnimationOnAnimator(animator, "FullBody, Override", animString);
            }

            if (!string.IsNullOrEmpty(soundString))
            {
                activePlayID = Util.PlaySound(soundString, gameObject);
            };
        }

        protected void PlayFromMemeRig(string animString, string soundString = "", float animDuration = 0)
        {
            memeRig.playMemeAnim();
            PlayEmote(animString, soundString, memeRig.MemeAnimator, animDuration);
        }

        public void HideShit(bool show = false)
        {
            if (!show)
            {
                weaponComponent.HideEquips();
            }
            else
            {
                weaponComponent.UnHideEquips();
            }

            if (base.GetAimAnimator()) 
                base.GetAimAnimator().enabled = show;
            int aim = show ? 1 : 0;
            this.animator.SetLayerWeight(animator.GetLayerIndex("AimPitch"), aim);
            this.animator.SetLayerWeight(animator.GetLayerIndex("AimYaw"), aim);

            base.characterBody.hideCrosshair = !show;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            bool endEmote = false;

            if (base.characterMotor)
            {
                if (!base.characterMotor.isGrounded) endEmote = true;
                //if (base.characterMotor.velocity != Vector3.zero) flag = true;
            }

            if (base.inputBank)
            {
                if (base.inputBank.skill1.down) endEmote = true;
                if (base.inputBank.skill2.down) endEmote = true;
                if (base.inputBank.skill3.down) endEmote = true;
                if (base.inputBank.skill4.down) endEmote = true;

                if (base.inputBank.moveVector != Vector3.zero) endEmote = true;
            }

            //dance cancels lol
            if (base.isAuthority)
            {
                if (base.characterBody.baseNameToken == "ENFORCER_NAME")
                {
                    if (Input.GetKeyDown(Config.restKey.Value))
                    {
                        endEmote = false;
                        this.outer.SetInterruptState(new Rest(), InterruptPriority.Any);
                        return;
                    }
                    else if (Input.GetKeyDown(Config.saluteKey.Value))
                    {
                        endEmote = false;
                        this.outer.SetInterruptState(new EnforcerSalute(), InterruptPriority.Any);
                        return;
                    }
                    else if (Input.GetKeyDown(Config.danceKey.Value))
                    {
                        endEmote = false;
                        this.outer.SetInterruptState(new DefaultDance(), InterruptPriority.Any);
                        return;
                    }
                    else if (Input.GetKeyDown(Config.runKey.Value))
                    {

                        endEmote = false;
                        this.outer.SetInterruptState(new FLINTLOCKWOOD(), InterruptPriority.Any);
                        return;
                    }
                }
                else
                {
                    if (Input.GetKeyDown(Config.restKey.Value))
                    {
                        endEmote = false;
                        this.outer.SetInterruptState(new Rest(), InterruptPriority.Any);
                        return;
                    }
                    else if (Input.GetKeyDown(Config.saluteKey.Value))
                    {
                        endEmote = false;
                        this.outer.SetInterruptState(new Salute(), InterruptPriority.Any);
                        return;
                    }
                }
            }

            if (this.duration > 0 && base.fixedAge >= this.duration)
                endEmote = true;

            if (endEmote)
            {
                this.outer.SetNextStateToMain();
            }

            updateCamera();
        }

        private void updateCamera()
        {
            CameraTargetParams ctp = base.cameraTargetParams;

            if (Vector3.Distance(ctp.idealLocalCameraPos, new Vector3(0f, -2.4f, -8f)) < 0.1f)
                return;
            float denom = (1 + Time.fixedTime - this.cameraInitialTime);
            float smoothFactor = 8 / Mathf.Pow(denom, 2);
            Vector3 smoothVector = new Vector3(-3 / 20, 1 / 16, -1);
            ctp.idealLocalCameraPos = new Vector3(0f, -1.8f, -8f) + smoothFactor * smoothVector;
        }

        public override void OnExit()
        {
            base.OnExit();

            HideShit(true);

            base.PlayAnimation("FullBody, Override", "BufferEmpty");
            if (this.activePlayID != 0) AkSoundEngine.StopPlayingID(this.activePlayID);

            if (memeRig.isPlaying)
                memeRig.stopAnim();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Any;
        }
    }
}