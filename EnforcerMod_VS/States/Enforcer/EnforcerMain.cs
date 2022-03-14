using Enforcer.Emotes;
using EntityStates.Enforcer.NeutralSpecial;
using EntityStates.Nemforcer.Emotes;
using Modules;
using Modules.Characters;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;
using static RoR2.CameraTargetParams;

namespace EntityStates.Enforcer {

    public class EnforcerMain : GenericCharacterMain {
    
        public static event Action<bool> onDance = delegate { };

        public static bool shotgunToggle = false;
        public bool dance = false;

        public Transform origOrigin;

        private EnforcerWeaponComponent weaponComponent;
        private EnforcerComponent enforcerComponent;

        private float initialTime;
        private float skateSpeedMultiplier = 0.5f;
        private float bungusStopwatch;
        private ChildLocator childLocator;        
        private Animator animator;
        private bool sprintCancelEnabled;
        private bool hasSprintCancelled;
        private Vector3 idealDirection;
        private uint skatePlayID;
        private EnforcerLightController lightController;
        private EnforcerLightControllerAlt lightControllerAlt;
        private EntityStateMachine sirenStateMachine;

        private bool skateJump { 
            get => EnforcerComponent.skateJump; 
            set => EnforcerComponent.skateJump = value; 
        }

        private AnimationCurve primarySpreadCurve = null;

        public static event Action<float> Bungus = delegate { };

        public override void OnEnter()
        {
            base.OnEnter();
            this.childLocator = base.GetModelChildLocator();
            this.animator = base.GetModelAnimator();

            this.lightController = base.characterBody.GetComponent<EnforcerLightController>();
            this.lightControllerAlt = base.characterBody.GetComponent<EnforcerLightControllerAlt>();
            this.weaponComponent = base.characterBody.GetComponent<EnforcerWeaponComponent>();
            this.enforcerComponent = base.characterBody.GetComponent<EnforcerComponent>();
            this.enforcerComponent.origOrigin = base.characterBody.aimOriginTransform;

            //Debug.LogWarning("EnforcerMain.OnEnter()");

            //bool hasParryStateMachine = false;

            foreach(EntityStateMachine i in base.gameObject.GetComponents<EntityStateMachine>()) {

                if (i.customName == "Slide") {
                    this.sirenStateMachine = i;
                }
            }

            //if (!EnforcerPlugin.EnforcerPlugin.cum && base.characterBody.skinIndex == EnforcerPlugin.EnforcerPlugin.doomGuyIndex)
            //{
            //    EnforcerPlugin.EnforcerPlugin.cum = true;
            //    Util.PlaySound(EnforcerPlugin.Sounds.DOOM, base.gameObject);
            //}

            //disable the shield when energy shield is selected
            if (base.characterBody.skillLocator.special.skillNameToken == "ENFORCER_SPECIAL_SHIELDON_NAME" || base.characterBody.skillLocator.special.skillNameToken == "ENFORCER_SPECIAL_SHIELDOFF_NAME")
            {
                if (this.childLocator.FindChild("ShieldModel")) 
                    this.childLocator.FindChild("ShieldModel").gameObject.SetActive(false);
            }

            //skamtebord
            if (base.characterBody.skillLocator.special.skillNameToken == "ENFORCER_SPECIAL_BOARDUP_NAME" || base.characterBody.skillLocator.special.skillNameToken == "ENFORCER_SPECIAL_BOARDDOWN_NAME")
            {
                if (this.childLocator.FindChild("ShieldModel")) 
                    this.childLocator.FindChild("ShieldModel").gameObject.SetActive(false);
                if (this.childLocator.FindChild("SkamteBordModel"))
                    this.childLocator.FindChild("SkamteBordModel").gameObject.SetActive(true);
            }

            if (base.isGrounded && base.HasBuff(Buffs.skateboardBuff))
            {
                this.skatePlayID = Util.PlaySound(Sounds.SkateRoll, base.gameObject);
            }
                
            onDance(false);

            this.sprintCancelEnabled = Config.sprintShieldCancel.Value;

            /*AnimationCurve commandoCurve = RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/characterbodies/commandobody").GetComponent<CharacterBody>().spreadBloomCurve;
            foreach (Keyframe k in commandoCurve.keys)
            {
                Debug.Log("\n time: " + k.time + ", value: " + k.value);
            }*/

            //Handle Spread
            if (base.characterBody)
            {
                switch (base.skillLocator.primary.skillDef.skillNameToken)
                {
                    case "ENFORCER_PRIMARY_RIFLE_NAME":

                        if (primarySpreadCurve == null)
                        {
                            primarySpreadCurve = new AnimationCurve();
                            primarySpreadCurve.AddKey(0.3f, 0.3f);
                            primarySpreadCurve.AddKey(1f, FireMachineGun.baseMaxSpread);
                            base.characterBody.spreadBloomCurve = primarySpreadCurve;
                        }

                        /*float maxSpread = (isShielded ? FireMachineGun.shieldSpreadMult * FireMachineGun.baseMaxSpread : FireMachineGun.baseMaxSpread);
                        if (base.characterBody.spreadBloomAngle > maxSpread)
                        {
                            base.characterBody.SetSpreadBloom(maxSpread, false);
                            base.characterBody.spreadBloomInternal = maxSpread;
                        }*/
                        break;
                    case "ENFORCER_PRIMARY_SHOTGUN_NAME":

                        if (primarySpreadCurve == null)
                        {
                            primarySpreadCurve = new AnimationCurve();
                            primarySpreadCurve.AddKey(0.3f, RiotShotgun.bulletSpread);
                            primarySpreadCurve.AddKey(1f, RiotShotgun.bulletSpread * 1.5f);
                            base.characterBody.spreadBloomCurve = primarySpreadCurve;
                        }

                        break;
                    case "ENFORCER_PRIMARY_SUPERSHOTGUN_NAME":

                        if (primarySpreadCurve == null)
                        {
                            primarySpreadCurve = new AnimationCurve();
                            primarySpreadCurve.AddKey(0.3f, Config.superSpread.Value);
                            primarySpreadCurve.AddKey(1f, Config.superSpread.Value * 1.5f);
                            base.characterBody.spreadBloomCurve = primarySpreadCurve;
                        }

                        break;
                    default:
                        break;
                }
            }
        }

        public override void Update()
        {
            base.Update();

            bool shieldIsUp = (base.characterBody.HasBuff(Buffs.protectAndServeBuff) || base.characterBody.HasBuff(Buffs.minigunBuff) || base.characterBody.HasBuff(Buffs.skateboardBuff));

            //emotes
            if (base.isAuthority && base.characterMotor.isGrounded && !shieldIsUp)
            {
                if (Input.GetKeyDown(Config.restKey.Value))
                {
                    this.outer.SetInterruptState(new Rest(), InterruptPriority.Any);
                    return;
                }
                else if (Input.GetKey(Config.saluteKey.Value))
                {
                    this.outer.SetInterruptState(new EnforcerSalute(), InterruptPriority.Any);
                    return;
                }
                if (Input.GetKey(Config.danceKey.Value))
                {
                    onDance(true);
                    this.outer.SetInterruptState(new DefaultDance(), InterruptPriority.Any);
                    return;
                }
                else if (Input.GetKey(Config.runKey.Value))
                {
                    this.outer.SetInterruptState(new FLINTLOCKWOOD(), InterruptPriority.Any);
                    return;
                }
            }        

            //sirens
            if (base.isAuthority && Input.GetKeyDown(Config.sirensKey.Value))
            {
                this.sirenStateMachine.SetInterruptState(new SirenToggle(), InterruptPriority.Any);
                return;
            }

            /*if (base.isAuthority && Input.GetKeyDown("z"))
            {
                EnforcerPlugin.NemesisInvasionManager.PerformInvasion(new Xoroshiro128Plus(Run.instance.seed));
            }*/
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (this.enforcerComponent) this.enforcerComponent.aimRay = base.GetAimRay();

            bool isShielded = base.characterBody.HasBuff(Buffs.protectAndServeBuff) || base.characterBody.HasBuff(Buffs.energyShieldBuff);

            if (isShielded)
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
            if (base.isAuthority && base.hasCharacterMotor)
            {
                //bool flag = false;

                /*if (base.characterMotor.velocity == Vector3.zero && base.characterMotor.isGrounded)
                {
                    int bungusCount = base.characterBody.master.inventory.GetItemCount(RoR2Content.Items.Mushroom);
                    if (bungusCount > 0)
                    {
                        flag = true;
                        float bungusMult = bungusCount * 0.35f;
                        this.bungusStopwatch += (1 + bungusMult) * Time.fixedDeltaTime;

                        Bungus(this.bungusStopwatch);
                    }
                }

                if (!flag) this.bungusStopwatch = 0;*/


                //sprint shield cancel
                if (base.isAuthority && NetworkServer.active && this.sprintCancelEnabled && base.inputBank)
                {
                    if (base.HasBuff(Buffs.protectAndServeBuff) && base.inputBank.sprint.down)
                    {
                        if (base.skillLocator)
                        {
                            if (base.skillLocator.special.CanExecute()) this.hasSprintCancelled = true;
                            base.skillLocator.special.ExecuteIfReady();
                        }
                    }
                }
            }

            //for idle anim
            //if (this.animator) this.animator.SetBool("inCombat", !base.characterBody.outOfCombat);
                
            //skateboard
            if (base.characterBody.HasBuff(Buffs.skateboardBuff))
            {
                if (base.isAuthority)
                {
                    base.characterBody.isSprinting = true;

                    this.UpdateSkateDirection();


                    if (base.characterDirection)
                    {
                        base.characterDirection.moveVector = this.idealDirection;
                        if (base.characterMotor && !(base.characterMotor.disableAirControlUntilCollision))
                        {
                            base.characterMotor.rootMotion += this.GetIdealVelocity() * Time.fixedDeltaTime;
                        }
                    }
                    if (base.isGrounded)
                    {
                        //slope shit
                        //Vector3 dir = modelLocator.modelTransform.up;
                        //base.characterMotor.ApplyForce(dir * skateGravity);
                    }
                }

                //sound
                if (base.isGrounded) {

                    if (this.skatePlayID == 0)
                    {
                        this.skatePlayID = Util.PlaySound(Sounds.SkateRoll, base.gameObject);
                    }

                    AkSoundEngine.SetRTPCValue("Skateboard_Speed", Util.Remap(base.characterMotor.velocity.magnitude, 7f, 60f, 1f, 4f));
                }
                else
                {
                    if (this.skatePlayID != 0)
                    {
                        if (base.characterMotor.velocity.y >= 0.1f) Util.PlaySound(Sounds.SkateOllie, base.gameObject);
                        AkSoundEngine.StopPlayingID(this.skatePlayID);
                        this.skatePlayID = 0;
                    }
                }
            }
            else
            {
                if (this.skatePlayID != 0)
                {
                    AkSoundEngine.StopPlayingID(this.skatePlayID);
                    this.skatePlayID = 0;
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

            // im fucking retarded lets goooo
            if (this.animator)
            {
                bool sirensOn = this.lightController.sirenToggle;
                this.animator.SetFloat("shitpost", sirensOn ? 1 : 0, 0.1f, Time.fixedDeltaTime);
            }
        }

        public override void ProcessJump() {
            base.ProcessJump();
            skateJump = true;
            //base.characterMotor.disableAirControlUntilCollision |= true;
        }

        public override void UpdateAnimationParameters() {
            base.UpdateAnimationParameters();

            if (enforcerComponent.beefStop) {
                this.modelAnimator.SetFloat(AnimationParameters.walkSpeed, 0);
            }
        }

        private void UpdateSkateDirection()
        {
            if (base.inputBank)
            {
                Vector2 vector = Util.Vector3XZToVector2XY(base.inputBank.moveVector);
                if (vector != Vector2.zero)
                {
                    vector.Normalize();
                    this.idealDirection = new Vector3(vector.x, 0f, vector.y).normalized;
                }
            }
        }

        private Vector3 GetIdealVelocity()
        {
            return base.characterDirection.forward * base.characterBody.moveSpeed * this.skateSpeedMultiplier;
        }

        public override void OnExit()
        {
            base.OnExit();

            AkSoundEngine.StopPlayingID(this.skatePlayID);
            this.skatePlayID = 0;
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