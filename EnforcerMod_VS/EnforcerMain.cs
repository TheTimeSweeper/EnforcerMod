using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace EntityStates.Enforcer
{
    public class EnforcerMain : GenericCharacterMain
    {
        public static float lightFlashInterval = 0.6f;

        private ShieldComponent shieldComponent;
        private EnforcerLightController lightComponent;
        private bool toggle = false;
        private bool wasShielding = false;
        private float initialTime;

        private uint sirenPlayID;
        private float flashStopwatch;

        public static bool shotgunToggle = false;
        private bool sirenToggle;
        private ChildLocator childLocator;

        public override void OnEnter()
        {
            base.OnEnter();
            this.shieldComponent = base.characterBody.GetComponent<ShieldComponent>();
            this.lightComponent = base.characterBody.GetComponent<EnforcerLightController>();
            this.childLocator = base.GetModelTransform().GetComponent<ChildLocator>();

            if (!EnforcerPlugin.EnforcerPlugin.cum && base.characterBody.skinIndex == 1)
            {
                EnforcerPlugin.EnforcerPlugin.cum = true;
                Util.PlaySound(EnforcerPlugin.Sounds.DOOM, base.gameObject);
            }

            //disable the shield when energy shield is selected
            if (base.characterBody.skillLocator.special.skillNameToken == "ENFORCER_SPECIAL_SHIELDON_NAME" || base.characterBody.skillLocator.special.skillNameToken == "ENFORCER_SPECIAL_SHIELDOFF_NAME")
            {
                if (this.childLocator) this.childLocator.FindChild("Shield").gameObject.SetActive(false);
            }
        }

        public override void Update()
        {
            base.Update();
            this.shieldComponent.aimRay = base.GetAimRay();

            //for ror1 shotgun sounds
            if (Input.GetKeyDown(KeyCode.X))
            {
                this.ToggleShotgun();
            }

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
            if (base.isAuthority && Input.GetKeyDown(KeyCode.C))
            {
                this.ToggleSirens();
            }

            //close camera
            if (this.shieldComponent.isShielding != this.wasShielding)
            {
                this.wasShielding = shieldComponent.isShielding;
                initialTime = Time.fixedTime;
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

            if (base.characterBody.skillLocator.special.skillNameToken == "ENFORCER_SPECIAL_SHIELDOFF_NAME")
            {
                if (this.shieldComponent.shieldHealth <= 0 && this.shieldComponent.isShielding)
                {
                    //this isn't working, shield health is always 0
                    //outer.SetNextState(new EnergyShield());
                    //return;
                }
            }
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
                this.sirenPlayID = Util.PlaySound(EnforcerPlugin.Sounds.SirenButton, base.gameObject);
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