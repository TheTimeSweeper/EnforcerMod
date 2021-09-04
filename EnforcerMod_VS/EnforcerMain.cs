using Enforcer.Emotes;
using EnforcerPlugin;
using EntityStates.Enforcer.NeutralSpecial;
using EntityStates.Nemforcer.Emotes;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace EntityStates.Enforcer
{
    public class EnforcerMain : GenericCharacterMain
    {
        public static event Action<bool> onDance = delegate { };
        public static Vector3 shieldCameraPosition = new Vector3(1.8f, -2.4f, -6f);
        public static Vector3 standardCameraPosition = new Vector3(0f, -1.3f, -12f);
        public static bool shotgunToggle = false;

        public Transform origOrigin;

        private EnforcerWeaponComponent weaponComponent;
        private ShieldComponent shieldComponent;

        private bool wasShielding = false;
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
            this.shieldComponent = base.characterBody.GetComponent<ShieldComponent>();
            this.shieldComponent.origOrigin = base.characterBody.aimOriginTransform;

            bool hasParryStateMachine = false;

            foreach(EntityStateMachine i in base.gameObject.GetComponents<EntityStateMachine>())
            {
                if (i.customName == "EnforcerParry")
                {
                    hasParryStateMachine = true;
                }

                if (i.customName == "Slide")
                {
                    this.sirenStateMachine = i;
                }
            }

            if (!hasParryStateMachine)
            {
                EntityStateMachine drOctagonapus = characterBody.gameObject.AddComponent<EntityStateMachine>();
                drOctagonapus.customName = "EnforcerParry";

                SerializableEntityStateType idleState = new SerializableEntityStateType(typeof(Idle));
                drOctagonapus.initialStateType = idleState;
                drOctagonapus.mainStateType = idleState;

                this.shieldComponent.drOctagonapus = drOctagonapus;
                drOctagonapus.mainStateType = new SerializableEntityStateType(typeof(Idle));
                this.shieldComponent.drOctagonapus = drOctagonapus;
            }

            //if (!EnforcerPlugin.EnforcerPlugin.cum && base.characterBody.skinIndex == EnforcerPlugin.EnforcerPlugin.doomGuyIndex)
            //{
            //    EnforcerPlugin.EnforcerPlugin.cum = true;
            //    Util.PlaySound(EnforcerPlugin.Sounds.DOOM, base.gameObject);
            //}

            //disable the shield when energy shield is selected
            if (base.characterBody.skillLocator.special.skillNameToken == "ENFORCER_SPECIAL_SHIELDON_NAME" || base.characterBody.skillLocator.special.skillNameToken == "ENFORCER_SPECIAL_SHIELDOFF_NAME")
            {
                if (this.childLocator.FindChild("Shield")) this.childLocator.FindChild("Shield").gameObject.SetActive(false);
            }

            //skamtebord
            if (base.characterBody.skillLocator.special.skillNameToken == "ENFORCER_SPECIAL_BOARDUP_NAME" || base.characterBody.skillLocator.special.skillNameToken == "ENFORCER_SPECIAL_BOARDDOWN_NAME")
            {
                //if (this.childLocator.FindChild("Shield")) this.childLocator.FindChild("Shield").gameObject.SetActive(false);
                //if (this.childLocator.FindChild("Skateboard")) this.childLocator.FindChild("Skateboard").gameObject.SetActive(true);
            }

            if (base.isGrounded && base.HasBuff(EnforcerPlugin.Modules.Buffs.skateboardBuff))
            {
                //this.skatePlayID = Util.PlaySound(EnforcerPlugin.Sounds.SkateRoll, base.gameObject);
            }
                
            onDance(false);

            this.sprintCancelEnabled = EnforcerPlugin.EnforcerModPlugin.sprintShieldCancel.Value;

            /*AnimationCurve commandoCurve = Resources.Load<GameObject>("prefabs/characterbodies/commandobody").GetComponent<CharacterBody>().spreadBloomCurve;
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
                            primarySpreadCurve.AddKey(0.3f, EnforcerModPlugin.superSpread.Value);
                            primarySpreadCurve.AddKey(1f, EnforcerModPlugin.superSpread.Value * 1.5f);
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

            bool shieldIsUp = (base.characterBody.HasBuff(EnforcerPlugin.Modules.Buffs.protectAndServeBuff) || base.characterBody.HasBuff(EnforcerPlugin.Modules.Buffs.minigunBuff) || base.characterBody.HasBuff(EnforcerPlugin.Modules.Buffs.skateboardBuff));

            //emotes
            if (base.isAuthority && base.characterMotor.isGrounded && !shieldIsUp)
            {
                if (Input.GetKeyDown(EnforcerPlugin.EnforcerModPlugin.defaultDanceKey.Value))
                {
                    onDance(true);
                    this.outer.SetInterruptState(new NemesisRest(), InterruptPriority.Any);
                    return;
                }
                else if (Input.GetKeyDown(EnforcerPlugin.EnforcerModPlugin.flossKey.Value))
                {
                    onDance(true);
                    this.outer.SetInterruptState(new EnforcerSalute(), InterruptPriority.Any);
                    return;
                }
                else if (Input.GetKeyDown(EnforcerPlugin.EnforcerModPlugin.earlKey.Value))
                {
                    //onDance(true);
                    //this.outer.SetInterruptState(new FLINTLOCKWOOD(), InterruptPriority.Any);
                    //return;
                }
            }

            //sirens
            if (base.isAuthority && Input.GetKeyDown(EnforcerPlugin.EnforcerModPlugin.sirensKey.Value))
            {
                this.sirenStateMachine.SetInterruptState(new SirenToggle(), InterruptPriority.Any);
                return;
            }

            /*if (base.isAuthority && Input.GetKeyDown("z"))
            {
                EnforcerPlugin.NemesisInvasionManager.PerformInvasion(new Xoroshiro128Plus(Run.instance.seed));
            }*/

            //shield mode camera stuff
            if (this.weaponComponent.isMultiplayer)
            {
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
                    Vector3 smoothVector = new Vector3(-3 / 20, 1 / 16, -1);
                    ctp.idealLocalCameraPos = shieldCameraPosition + smoothFactor * smoothVector;
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (this.shieldComponent) this.shieldComponent.aimRay = base.GetAimRay();

            bool isShielded = base.characterBody.HasBuff(EnforcerPlugin.Modules.Buffs.protectAndServeBuff) || base.characterBody.HasBuff(EnforcerPlugin.Modules.Buffs.energyShieldBuff);

            if (isShielded || base.characterBody.HasBuff(EnforcerPlugin.Modules.Buffs.minigunBuff))
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
                    if (base.HasBuff(EnforcerPlugin.Modules.Buffs.protectAndServeBuff) && base.inputBank.sprint.down)
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

            //visions anim
            if (base.hasSkillLocator)
            {
                if (base.skillLocator.primary.skillDef.skillNameToken == "SKILL_LUNAR_PRIMARY_REPLACEMENT_NAME")
                {
                    if (base.inputBank.skill1.down)
                    {
                        if (isShielded)
                        {
                            base.PlayAnimation("Gesture, Override", "FireShotgun", "FireShotgun.playbackRate", this.attackSpeedStat);
                        }
                        else
                        {
                            base.PlayAnimation("Gesture, Override", "FireShotgun", "FireShotgun.playbackRate", this.attackSpeedStat);
                        }
                    }
                }
            }
                
            //skateboard
            if (base.characterBody.HasBuff(EnforcerPlugin.Modules.Buffs.skateboardBuff))
            {
                if (base.isAuthority)
                {
                    base.characterBody.isSprinting = true;

                    this.UpdateSkateDirection();

                    if (base.characterDirection)
                    {
                        base.characterDirection.moveVector = this.idealDirection;
                        if (base.characterMotor && !base.characterMotor.disableAirControlUntilCollision)
                        {
                            base.characterMotor.rootMotion += this.GetIdealVelocity() * Time.fixedDeltaTime;
                        }
                    }

                    /*if (base.isGrounded)
                    {
                        //slope shit
                        Vector3 dir = modelLocator.modelTransform.up;
                        base.characterMotor.ApplyForce(dir * skateGravity);
                    }*/
                }

                //sound
                if (base.isGrounded)
                {
                    if (this.skatePlayID == 0)
                    {
                        this.skatePlayID = Util.PlaySound(EnforcerPlugin.Sounds.SkateRoll, base.gameObject);
                    }

                    AkSoundEngine.SetRTPCValue("Skateboard_Speed", Util.Remap(base.characterMotor.velocity.magnitude, 7f, 60f, 1f, 4f));
                }
                else
                {
                    if (this.skatePlayID != 0)
                    {
                        if (base.characterMotor.velocity.y >= 0.1f) Util.PlaySound(EnforcerPlugin.Sounds.SkateOllie, base.gameObject);
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