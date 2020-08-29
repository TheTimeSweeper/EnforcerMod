using RoR2;
using System;
using UnityEngine;

namespace EntityStates.Enforcer
{
    public class EnforcerMain : GenericCharacterMain
    {
        public static float lightFlashInterval = 0.5f;

        private ShieldComponent shieldComponent;
        private EnforcerLightController lightComponent;
        private bool wasShielding = false;
        private float initialTime;

        private uint sirenPlayID;
        private float flashStopwatch;
        private float bungusStopwatch;

        public static bool shotgunToggle = false;
        private bool sirenToggle;
        private ChildLocator childLocator;
        private bool sprintCancelEnabled;
        private bool hasSprintCancelled;

        public static event Action<float> Bungus = delegate { };

        public override void OnEnter()
        {
            base.OnEnter();
            this.shieldComponent = base.characterBody.GetComponent<ShieldComponent>();
            this.lightComponent = base.characterBody.GetComponent<EnforcerLightController>();
            this.childLocator = base.GetModelTransform().GetComponent<ChildLocator>();

            this.childLocator.FindChild("ShieldHurtbox").gameObject.SetActive(false);

            if (!EnforcerPlugin.EnforcerPlugin.cum && base.characterBody.skinIndex == 2)
            {
                EnforcerPlugin.EnforcerPlugin.cum = true;
                Util.PlaySound(EnforcerPlugin.Sounds.DOOM, base.gameObject);
            }

            //disable the shield when energy shield is selected
            if (base.characterBody.skillLocator.special.skillNameToken == "ENFORCER_SPECIAL_SHIELDON_NAME" || base.characterBody.skillLocator.special.skillNameToken == "ENFORCER_SPECIAL_SHIELDOFF_NAME")
            {
                if (this.childLocator) this.childLocator.FindChild("Shield").gameObject.SetActive(false);
            }

            this.sprintCancelEnabled = EnforcerPlugin.EnforcerPlugin.sprintShieldCancel.Value;
        }

        public override void Update()
        {
            base.Update();

            //for ror1 shotgun sounds
            /*if (Input.GetKeyDown(KeyCode.X))
            {
                this.ToggleShotgun();
            }*/

            //default dance
            if (base.isAuthority && Input.GetKeyDown(KeyCode.Z))
            {
                if (base.characterMotor.isGrounded)
                {
                    this.outer.SetNextState(new DefaultDance());
                    return;
                }
            }

            //sirens
            if (base.isAuthority && Input.GetKeyDown(KeyCode.CapsLock))
            {
                this.ToggleSirens();
            }

            //shield mode camera stuff
            if (this.shieldComponent.isShielding != this.wasShielding)
            {
                this.wasShielding = this.shieldComponent.isShielding;
                this.initialTime = Time.fixedTime;
            }

            if (this.shieldComponent.isShielding)
            {
                CameraTargetParams ctp = base.cameraTargetParams;
                float denom = (1 + Time.fixedTime - this.initialTime);
                float smoothFactor = 8 / Mathf.Pow(denom, 2);
                Vector3 smoothVector = new Vector3(-3 /20, 1 / 16, -1);
                ctp.idealLocalCameraPos = new Vector3(1.8f, -0.5f, -6f) + smoothFactor * smoothVector;
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.shieldComponent.aimRay = base.GetAimRay();

            if (base.characterBody.HasBuff(EnforcerPlugin.EnforcerPlugin.jackBoots) || base.characterBody.HasBuff(EnforcerPlugin.EnforcerPlugin.energyShieldBuff))
            {
                base.characterBody.isSprinting = false;
                base.characterBody.SetAimTimer(0.2f);
            }

            if (this.sirenToggle)
            {
                this.flashStopwatch -= Time.fixedDeltaTime;
                if (this.flashStopwatch <= 0)
                {
                    this.flashStopwatch = EnforcerMain.lightFlashInterval;
                    this.FlashLights();
                }
            }

            if (this.hasSprintCancelled)
            {
                this.hasSprintCancelled = false;
                base.characterBody.isSprinting = true;
            }

            //bungus achievement
            if (base.isAuthority && base.hasCharacterMotor)
            {
                bool flag = false;

                if (base.characterMotor.velocity == Vector3.zero && base.characterMotor.isGrounded)
                {
                    if (base.characterBody.master.inventory.GetItemCount(ItemIndex.Mushroom) > 0)
                    {
                        flag = true;
                        this.bungusStopwatch += Time.fixedDeltaTime;

                        Bungus(this.bungusStopwatch);
                    }
                }

                if (!flag) this.bungusStopwatch = 0;


                //sprint shield cancel
                if (this.sprintCancelEnabled && base.inputBank)
                {
                    if (base.HasBuff(EnforcerPlugin.EnforcerPlugin.jackBoots) && base.inputBank.sprint.down)
                    {
                        if (base.skillLocator)
                        {
                            if (base.skillLocator.special.CanExecute()) this.hasSprintCancelled = true;
                            base.skillLocator.special.ExecuteIfReady();
                        }
                    }
                }
            }

            /*if (base.characterBody.skillLocator.special.skillNameToken == "ENFORCER_SPECIAL_SHIELDOFF_NAME")
            {
                if (this.shieldComponent.shieldHealth <= 0 && this.shieldComponent.isShielding)
                {
                    //this isn't working, shield health is always 0
                    //outer.SetNextState(new EnergyShield());
                    //return;
                }
            }*/
        }

        public override void OnExit()
        {
            base.OnExit();

            if (this.sirenPlayID != 0) AkSoundEngine.StopPlayingID(this.sirenPlayID);
        }

        private void ToggleShotgun()
        {
            EnforcerMain.shotgunToggle = !EnforcerMain.shotgunToggle;

            if (EnforcerMain.shotgunToggle)
            {
                Chat.AddMessage("Using classic shotgun sounds");
            }
            else
            {
                Chat.AddMessage("Using modern shotgun sounds");
            }
        }

        private void ToggleSirens()
        {
            this.sirenToggle = !this.sirenToggle;

            if (this.sirenToggle)
            {
                string sound = EnforcerPlugin.Sounds.SirenButton;
                if (base.characterBody.skinIndex == EnforcerPlugin.EnforcerPlugin.frogIndex) sound = EnforcerPlugin.Sounds.Croak;
                this.sirenPlayID = Util.PlaySound(sound, base.gameObject);
                this.flashStopwatch = 0;
            }
            else
            {
                if (this.sirenPlayID != 0) AkSoundEngine.StopPlayingID(this.sirenPlayID);
            }
        }

        private void FlashLights()
        {
            if (this.lightComponent) this.lightComponent.FlashLights(1);
        }
    }
}