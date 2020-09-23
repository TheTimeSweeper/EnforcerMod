using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace EntityStates.Enforcer
{
    public class EnforcerMain : GenericCharacterMain
    {
        public static event Action<bool> onDance = delegate { };

        public static bool shotgunToggle = false;

        public Transform origOrigin;

        private ShieldComponent shieldComponent;
        private bool wasShielding = false;
        private float initialTime;

        private float bungusStopwatch;
        private ChildLocator childLocator;
        private Animator animator;
        private bool sprintCancelEnabled;
        private bool hasSprintCancelled;
        private bool isNemesis;

        public static event Action<float> Bungus = delegate { };

        public override void OnEnter()
        {
            base.OnEnter();
            this.shieldComponent = base.characterBody.GetComponent<ShieldComponent>();
            this.childLocator = base.GetModelChildLocator();
            this.animator = base.GetModelAnimator();

            shieldComponent.origOrigin = characterBody.aimOriginTransform;

            if (base.characterBody.skillLocator.special.skillNameToken == "NEMFORCER_SPECIAL_MINIGUNUP_NAME") this.isNemesis = true;
            else this.isNemesis = false;

            if (!this.isNemesis)
            {
                EntityStateMachine drOctagonapus = characterBody.gameObject.AddComponent<EntityStateMachine>();
                drOctagonapus.customName = "EnforcerParry";

                SerializableEntityStateType idleState = new SerializableEntityStateType(typeof(Idle));
                drOctagonapus.initialStateType = idleState;
                drOctagonapus.mainStateType = idleState;

                this.shieldComponent.drOctagonapus = drOctagonapus;
                drOctagonapus.mainStateType = new SerializableEntityStateType(typeof(Idle));
                this.shieldComponent.drOctagonapus = drOctagonapus;

                if (!EnforcerPlugin.EnforcerPlugin.cum && base.characterBody.skinIndex == 2)
                {
                    EnforcerPlugin.EnforcerPlugin.cum = true;
                    Util.PlaySound(EnforcerPlugin.Sounds.DOOM, base.gameObject);
                }

                //disable the shield when energy shield is selected
                if (base.characterBody.skillLocator.special.skillNameToken == "ENFORCER_SPECIAL_SHIELDON_NAME" || base.characterBody.skillLocator.special.skillNameToken == "ENFORCER_SPECIAL_SHIELDOFF_NAME")
                {
                    if (this.childLocator.FindChild("Shield")) this.childLocator.FindChild("Shield").gameObject.SetActive(false);
                }
            }

            onDance(false);

            this.sprintCancelEnabled = EnforcerPlugin.EnforcerPlugin.sprintShieldCancel.Value;
        }

        public override void Update()
        {
            base.Update();

            bool shieldIsUp = (base.characterBody.HasBuff(EnforcerPlugin.EnforcerPlugin.jackBoots) || base.characterBody.HasBuff(EnforcerPlugin.EnforcerPlugin.minigunBuff));

            /*if (Input.GetKeyDown(KeyCode.G)) {
                RiotShotgun.spreadSpread = !RiotShotgun.spreadSpread;
                Chat.AddMessage($"Spreading: {RiotShotgun.spreadSpread}");
            }*/

            //for ror1 shotgun sounds
            /*if (Input.GetKeyDown(KeyCode.X))
            {
                this.ToggleShotgun();
            }*/

            //default dance
            if (base.isAuthority && base.characterMotor.isGrounded && !shieldIsUp)
            {
                if (Input.GetKeyDown(EnforcerPlugin.EnforcerPlugin.dance1Key.Value))
                {
                    onDance(true);
                    this.outer.SetInterruptState(EntityState.Instantiate(new SerializableEntityStateType(typeof(DefaultDance))), InterruptPriority.Any);
                    return;
                }
                else if (Input.GetKeyDown(EnforcerPlugin.EnforcerPlugin.dance2Key.Value))
                {
                    onDance(true);
                    this.outer.SetInterruptState(EntityState.Instantiate(new SerializableEntityStateType(typeof(Floss))), InterruptPriority.Any);
                    return;
                }
            }

            //sirens
            if (base.isAuthority && Input.GetKeyDown(EnforcerPlugin.EnforcerPlugin.sirensKey.Value))
            {
                this.outer.SetInterruptState(EntityState.Instantiate(new SerializableEntityStateType(typeof(SirenToggle))), InterruptPriority.Any);
                return;
            }

            //shield mode camera stuff
            if (shieldIsUp != this.wasShielding)
            {
                this.wasShielding = shieldIsUp;
                this.initialTime = Time.fixedTime;
            }

            if (shieldIsUp)
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
            if (this.shieldComponent) this.shieldComponent.aimRay = base.GetAimRay();

            if (base.characterBody.HasBuff(EnforcerPlugin.EnforcerPlugin.jackBoots) || base.characterBody.HasBuff(EnforcerPlugin.EnforcerPlugin.energyShieldBuff) || base.characterBody.HasBuff(EnforcerPlugin.EnforcerPlugin.minigunBuff))
            {
                base.characterBody.isSprinting = false;
                base.characterBody.SetAimTimer(0.2f);
            }

            if (this.hasSprintCancelled)
            {
                this.hasSprintCancelled = false;
                base.characterBody.isSprinting = true;
            }

            //bungus achievement
            if (base.isAuthority && base.hasCharacterMotor && !this.isNemesis)
            {
                bool flag = false;

                if (base.characterMotor.velocity == Vector3.zero && base.characterMotor.isGrounded)
                {
                    int bungusCount = base.characterBody.master.inventory.GetItemCount(ItemIndex.Mushroom);
                    if (bungusCount > 0)
                    {
                        flag = true;
                        float bungusMult = bungusCount * 0.01f;
                        this.bungusStopwatch += (1 + bungusMult) * Time.fixedDeltaTime;

                        Bungus(this.bungusStopwatch);
                    }
                }

                if (!flag) this.bungusStopwatch = 0;


                //sprint shield cancel
                if (base.isAuthority && NetworkServer.active && this.sprintCancelEnabled && base.inputBank)
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

            if (this.animator) this.animator.SetBool("inCombat", !base.characterBody.outOfCombat);

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
    }
}