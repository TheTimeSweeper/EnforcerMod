using Enforcer.Emotes;
using EntityStates.Nemforcer.Emotes;
using RoR2;
using UnityEngine;

namespace EntityStates.Enforcer
{
    public class BaseEmote : BaseState
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
            this.animator = base.GetModelAnimator();
            this.childLocator = base.GetModelChildLocator();

            base.characterBody.hideCrosshair = true;

            var weaponComponent = base.GetComponent<EnforcerWeaponComponent>();
            if (weaponComponent)
            {
                //weaponComponent.HideWeapon();
                //this.ToggleShield(false);
                //this.childLocator.FindChild("Skateboard").gameObject.SetActive(false);
            }

            if (base.GetAimAnimator()) base.GetAimAnimator().enabled = false;
            this.animator.SetLayerWeight(animator.GetLayerIndex("AimPitch"), 0);
            this.animator.SetLayerWeight(animator.GetLayerIndex("AimYaw"), 0);

            //if (base.characterBody.skinIndex == EnforcerPlugin.EnforcerPlugin.doomGuyIndex && base.characterBody.baseNameToken == "ENFORCER_NAME") soundString = EnforcerPlugin.Sounds.DOOM;

            if (this.animDuration == 0 && this.duration != 0) this.animDuration = this.duration;

            if (this.duration > 0) base.PlayAnimation("FullBody, Override", this.animString, "Emote.playbackRate", this.animDuration);
            else
            {
                this.animator.SetFloat("Emote.playbackRate", 1f);
                base.PlayAnimation("FullBody, Override", this.animString);
            }

            if (!string.IsNullOrEmpty(soundString))
            {
                this.activePlayID = Util.PlaySound(soundString, base.gameObject);
            }

            this.initialTime = Time.fixedTime;

            if (base.GetComponent<EnforcerWeaponComponent>()) 
                base.GetComponent<EnforcerWeaponComponent>().HideWeapons();
        }

        public override void OnExit()
        {
            base.OnExit();

            base.characterBody.hideCrosshair = false;

            if (base.GetAimAnimator()) base.GetAimAnimator().enabled = true;
            this.animator.SetLayerWeight(animator.GetLayerIndex("AimPitch"), 1);
            this.animator.SetLayerWeight(animator.GetLayerIndex("AimYaw"), 1);

            var weaponComponent = base.GetComponent<EnforcerWeaponComponent>();
            if (weaponComponent)
            {
                weaponComponent.ResetWeapon();
                //this.ToggleShield(true);
            }

            base.PlayAnimation("FullBody, Override", "BufferEmpty");
            if (this.activePlayID != 0) AkSoundEngine.StopPlayingID(this.activePlayID);
        }

        //private void ToggleShield(bool sex)
        //{
        //    if (this.childLocator)
        //    {
        //        if (this.childLocator.FindChild("Shield"))
        //        {
        //            //this.childLocator.FindChild("Shield").gameObject.SetActive(sex);
        //            //this.childLocator.FindChild("Skateboard").gameObject.SetActive(sex);
        //        }
        //    }
        //}

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            bool flag = false;

            if (base.characterMotor)
            {
                if (!base.characterMotor.isGrounded) flag = true;
                //if (base.characterMotor.velocity != Vector3.zero) flag = true;
            }

            if (base.inputBank)
            {
                if (base.inputBank.skill1.down) flag = true;
                if (base.inputBank.skill2.down) flag = true;
                if (base.inputBank.skill3.down) flag = true;
                if (base.inputBank.skill4.down) flag = true;

                if (base.inputBank.moveVector != Vector3.zero) flag = true;
            }

            //dance cancels lol
            if (base.isAuthority)
            {
                if (base.characterBody.baseNameToken == "ENFORCER_NAME")
                {
                    if (Input.GetKeyDown(EnforcerPlugin.EnforcerModPlugin.defaultDanceKey.Value))
                    {
                        flag = false;
                        this.outer.SetInterruptState(new NemesisRest(), InterruptPriority.Any);
                        return;
                    }
                    else if (Input.GetKeyDown(EnforcerPlugin.EnforcerModPlugin.flossKey.Value))
                    {
                        flag = false;
                        this.outer.SetInterruptState(new EnforcerSalute(), InterruptPriority.Any);
                        return;
                    }
                    else if (Input.GetKeyDown(EnforcerPlugin.EnforcerModPlugin.earlKey.Value))
                    {
                        //flag = false;
                        //this.outer.SetInterruptState(new FLINTLOCKWOOD(), InterruptPriority.Any);
                        //return;
                    }
                }
                else
                {
                    if (Input.GetKeyDown(EnforcerPlugin.EnforcerModPlugin.defaultDanceKey.Value))
                    {
                        flag = false;
                        this.outer.SetInterruptState(new NemesisRest(), InterruptPriority.Any);
                        return;
                    }
                    else if (Input.GetKeyDown(EnforcerPlugin.EnforcerModPlugin.flossKey.Value))
                    {
                        flag = false;
                        this.outer.SetInterruptState(new Salute(), InterruptPriority.Any);
                        return;
                    }
                }
            }

            if (this.duration > 0 && base.fixedAge >= this.duration) flag = true;

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

    public class FLINTLOCKWOOD : BaseEmote
    {
        public override void OnEnter()
        {
            this.animString = "FLINT LOCK WOOD";
            this.soundString = "";
            base.OnEnter();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            base.StartAimMode(1, true);
            base.characterMotor.rootMotion = base.characterDirection.forward * this.moveSpeedStat * base.characterBody.sprintingSpeedMultiplier * Time.fixedDeltaTime;
        }
    }

    public class NemesisRest : BaseEmote
    {
        public override void OnEnter()
        {
            this.animString = "RestEmote";
            this.soundString = "";
            this.animDuration = 1.5f;
            base.OnEnter();
        }
    }
}