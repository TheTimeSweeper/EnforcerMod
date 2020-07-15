using System;
using System.Reflection;
using System.Collections.Generic;
using BepInEx;
using R2API;
using R2API.Utils;
using EntityStates;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.Networking;
using KinematicCharacterController;
using EntityStates.Enforcer;
using RoR2.Projectile;
using System.Collections;
using System.Xml.Schema;

namespace EnforcerPlugin
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin(MODUID, "Enforcer", "0.0.1")]
    [R2APISubmoduleDependency(new string[]
    {
        "PrefabAPI",
        "SurvivorAPI",
        "LoadoutAPI",
        "BuffAPI",
        "LanguageAPI"
    })]

    public class EnforcerPlugin : BaseUnityPlugin
    {
        public const string MODUID = "com.ok.Enforcer";

        public static EnforcerPlugin instance;

        //i didn't want this to be static considering we're using an instance now but it throws 23 errors if i remove the static modifier 
        //i'm not dealing with that
        public static GameObject characterPrefab;
        public static GameObject projectilePrefab;
        public GameObject tearGasPrefab;
        public GameObject characterDisplay;
        public GameObject doppelganger;
        
        public static event Action awake;
        //public static event Action start;

        public static readonly Color characterColor = new Color(0.26f, 0.27f, 0.46f);

        public static BuffIndex jackBoots;

        //更新许可证 DO WHAT THE FUCK YOU WANT TO

        public SkillLocator skillLocator;

        public EnforcerPlugin() {
            //don't touch this
            // what does all this even do anyway?
            //its our plugin constructor
            awake += EnforcerPlugin_Load;
        }

        private void EnforcerPlugin_Load()
        {
            //touch this all you want tho
            Assets.PopulateAssets();
            CreatePrefab();
            RegisterCharacter();
            Skins.RegisterSkins();
            RegisterBuffs();
            RegisterProjectile();
            CreateDoppelganger();
            Hook();
        }

        public void Awake()
        {
            Action awake = EnforcerPlugin.awake;
            if (awake == null)
            {
                return;
            }
            awake();
        }
        private void Hook() {
            //add hooks here
            //using this approach means we'll only ever have to comment one line if we don't want a hook to fire
            //it's much simpler this way, trust me
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
        }
        #region Hooks
        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);
            if (self && self.HasBuff(jackBoots))
            {
                R2API.Utils.Reflection.SetPropertyValue<int>(self, "maxJumpCount", 0);
                R2API.Utils.Reflection.SetPropertyValue<float>(self, "armor", self.armor + 20);
                R2API.Utils.Reflection.SetPropertyValue<float>(self, "moveSpeed", self.moveSpeed * 0.5f);
            }
        }
        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo info)
        {
            ShieldComponent shieldComponent = self.GetComponent<ShieldComponent>();
            if (shieldComponent && info.attacker && shieldComponent.isShielding)
            {
                CharacterBody charB = self.GetComponent<CharacterBody>();
                Ray aimRay = shieldComponent.aimRay;
                Vector3 relativePosition = info.attacker.transform.position - aimRay.origin;
                float angle = Vector3.Angle(aimRay.direction, relativePosition);
                if (angle < 45)
                {
                    return;
                }
            }
            orig(self, info);
        }
        #endregion
        private static GameObject CreateModel(GameObject main)
        {
            Destroy(main.transform.Find("ModelBase").gameObject);
            Destroy(main.transform.Find("CameraPivot").gameObject);
            Destroy(main.transform.Find("AimOrigin").gameObject);

            GameObject model = Assets.MainAssetBundle.LoadAsset<GameObject>("mdlEnforcer");

            return model;
        }
        private static void CreatePrefab()
        {
            //...what?
            // https://youtu.be/zRXl8Ow2bUs

            #region add all the things
            characterPrefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody"), "EnforcerEpicBody");

            characterPrefab.GetComponent<NetworkIdentity>().localPlayerAuthority = true;

            GameObject model = CreateModel(characterPrefab);

            GameObject gameObject = new GameObject("ModelBase");
            gameObject.transform.parent = characterPrefab.transform;
            gameObject.transform.localPosition = new Vector3(0f, -0.81f, 0f);
            gameObject.transform.localRotation = Quaternion.identity;
            gameObject.transform.localScale = new Vector3(1f, 1f, 1f);

            GameObject gameObject2 = new GameObject("CameraPivot");
            gameObject2.transform.parent = gameObject.transform;
            gameObject2.transform.localPosition = new Vector3(0f, 1.6f, 0f);
            gameObject2.transform.localRotation = Quaternion.identity;
            gameObject2.transform.localScale = Vector3.one;

            GameObject gameObject3 = new GameObject("AimOrigin");
            gameObject3.transform.parent = gameObject.transform;
            gameObject3.transform.localPosition = new Vector3(0f, 1.4f, 0f);
            gameObject3.transform.localRotation = Quaternion.identity;
            gameObject3.transform.localScale = Vector3.one;

            Transform transform = model.transform;
            transform.parent = gameObject.transform;
            transform.localPosition = Vector3.zero;
            //transform.localScale = new Vector3(0.14f, 0.14f, 0.14f);
            transform.localRotation = Quaternion.identity;

            CharacterDirection characterDirection = characterPrefab.GetComponent<CharacterDirection>();
            characterDirection.moveVector = Vector3.zero;
            characterDirection.targetTransform = gameObject.transform;
            characterDirection.overrideAnimatorForwardTransform = null;
            characterDirection.rootMotionAccumulator = null;
            characterDirection.modelAnimator = model.GetComponentInChildren<Animator>();
            characterDirection.driveFromRootRotation = false;
            characterDirection.turnSpeed = 720f;

            CharacterBody bodyComponent = characterPrefab.GetComponent<CharacterBody>();
            bodyComponent.bodyIndex = -1;
            bodyComponent.baseNameToken = "ENFORCER_NAME";
            bodyComponent.subtitleNameToken = "ENFORCER_SUBTITLE";
            bodyComponent.bodyFlags = CharacterBody.BodyFlags.ImmuneToExecutes;
            bodyComponent.rootMotionInMainState = false;
            bodyComponent.mainRootSpeed = 0;
            bodyComponent.baseMaxHealth = 160;
            bodyComponent.levelMaxHealth = 48;
            bodyComponent.baseRegen = 2.5f;
            bodyComponent.levelRegen = 0.5f;
            bodyComponent.baseMaxShield = 0;
            bodyComponent.levelMaxShield = 0;
            bodyComponent.baseMoveSpeed = 7;
            bodyComponent.levelMoveSpeed = 0;
            bodyComponent.baseAcceleration = 80;
            bodyComponent.baseJumpPower = 15;
            bodyComponent.levelJumpPower = 0;
            bodyComponent.baseDamage = 12;
            bodyComponent.levelDamage = 2.4f;
            bodyComponent.baseAttackSpeed = 1;
            bodyComponent.levelAttackSpeed = 0;
            bodyComponent.baseCrit = 1;
            bodyComponent.levelCrit = 0;
            bodyComponent.baseArmor = 0;
            bodyComponent.levelArmor = 0;
            bodyComponent.baseJumpCount = 1;
            bodyComponent.sprintingSpeedMultiplier = 1.45f;
            bodyComponent.wasLucky = false;
            bodyComponent.hideCrosshair = false;
            bodyComponent.crosshairPrefab = Resources.Load<GameObject>("Prefabs/Crosshair/SMGCrosshair");
            bodyComponent.aimOriginTransform = gameObject3.transform;
            bodyComponent.hullClassification = HullClassification.Human;
            bodyComponent.portraitIcon = Assets.charPortrait;
            bodyComponent.isChampion = false;
            bodyComponent.currentVehicle = null;
            bodyComponent.skinIndex = 0U;

            LoadoutAPI.AddSkill(typeof(EnforcerMain));

            var stateMachine = bodyComponent.GetComponent<EntityStateMachine>();
            stateMachine.mainStateType = new SerializableEntityStateType(typeof(EnforcerMain));

            CharacterMotor characterMotor = characterPrefab.GetComponent<CharacterMotor>();
            characterMotor.walkSpeedPenaltyCoefficient = 1f;
            characterMotor.characterDirection = characterDirection;
            characterMotor.muteWalkMotion = false;
            characterMotor.mass = 100f;
            characterMotor.airControl = 0.25f;
            characterMotor.disableAirControlUntilCollision = false;
            characterMotor.generateParametersOnAwake = true;
            characterMotor.useGravity = true;
            characterMotor.isFlying = false;

            InputBankTest inputBankTest = characterPrefab.GetComponent<InputBankTest>();
            inputBankTest.moveVector = Vector3.zero;

            CameraTargetParams cameraTargetParams = characterPrefab.GetComponent<CameraTargetParams>();
            cameraTargetParams.cameraParams = Resources.Load<GameObject>("Prefabs/CharacterBodies/LoaderBody").GetComponent<CameraTargetParams>().cameraParams;
            cameraTargetParams.cameraPivotTransform = null;
            cameraTargetParams.aimMode = CameraTargetParams.AimType.Standard;
            cameraTargetParams.recoil = Vector2.zero;
            cameraTargetParams.idealLocalCameraPos = Vector3.zero;
            cameraTargetParams.dontRaycastToPivot = false;

            ModelLocator modelLocator = characterPrefab.GetComponent<ModelLocator>();
            modelLocator.modelTransform = transform;
            modelLocator.modelBaseTransform = gameObject.transform;
            modelLocator.dontReleaseModelOnDeath = false;
            modelLocator.autoUpdateModelTransform = true;
            modelLocator.dontDetatchFromParent = false;
            modelLocator.noCorpse = false;
            modelLocator.normalizeToFloor = false;
            modelLocator.preserveModel = false;

            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            CharacterModel characterModel = model.AddComponent<CharacterModel>();
            characterModel.body = bodyComponent;
            characterModel.baseRendererInfos = new CharacterModel.RendererInfo[]
            {
                new CharacterModel.RendererInfo
                {
                    defaultMaterial = model.GetComponentInChildren<SkinnedMeshRenderer>().material,
                    renderer = model.GetComponentInChildren<SkinnedMeshRenderer>(),
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false
                },
                new CharacterModel.RendererInfo
                {
                    defaultMaterial = model.GetComponent<ChildLocator>().FindChild("Shotgun").GetComponentInChildren<MeshRenderer>().material,
                    renderer = model.GetComponent<ChildLocator>().FindChild("Shotgun").GetComponentInChildren<MeshRenderer>(),
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false
                },
                new CharacterModel.RendererInfo
                {
                    defaultMaterial = model.GetComponent<ChildLocator>().FindChild("Shield").GetComponentInChildren<MeshRenderer>().material,
                    renderer = model.GetComponent<ChildLocator>().FindChild("Shield").GetComponentInChildren<MeshRenderer>(),
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false
                }
            };

            characterModel.autoPopulateLightInfos = true;
            characterModel.invisibilityCount = 0;
            characterModel.temporaryOverlays = new List<TemporaryOverlay>();

            characterModel.SetFieldValue("mainSkinnedMeshRenderer", characterModel.baseRendererInfos[0].renderer.gameObject.GetComponent<SkinnedMeshRenderer>());

            TeamComponent teamComponent = null;
            if (characterPrefab.GetComponent<TeamComponent>() != null) teamComponent = characterPrefab.GetComponent<TeamComponent>();
            else teamComponent = characterPrefab.GetComponent<TeamComponent>();
            teamComponent.hideAllyCardDisplay = false;
            teamComponent.teamIndex = TeamIndex.None;

            HealthComponent healthComponent = characterPrefab.GetComponent<HealthComponent>();
            healthComponent.health = 160f;
            healthComponent.shield = 0f;
            healthComponent.barrier = 0f;
            healthComponent.magnetiCharge = 0f;
            healthComponent.body = null;
            healthComponent.dontShowHealthbar = false;
            healthComponent.globalDeathEventChanceCoefficient = 1f;

            characterPrefab.GetComponent<Interactor>().maxInteractionDistance = 3f;
            characterPrefab.GetComponent<InteractionDriver>().highlightInteractor = true;

            CharacterDeathBehavior characterDeathBehavior = characterPrefab.GetComponent<CharacterDeathBehavior>();
            characterDeathBehavior.deathStateMachine = characterPrefab.GetComponent<EntityStateMachine>();
            characterDeathBehavior.deathState = new SerializableEntityStateType(typeof(GenericCharacterDeath));

            SfxLocator sfxLocator = characterPrefab.GetComponent<SfxLocator>();
            sfxLocator.deathSound = "Play_ui_player_death";
            sfxLocator.barkSound = "";
            sfxLocator.openSound = "";
            sfxLocator.landingSound = "Play_char_land";
            sfxLocator.fallDamageSound = "Play_char_land_fall_damage";
            sfxLocator.aliveLoopStart = "";
            sfxLocator.aliveLoopStop = "";

            Rigidbody rigidbody = characterPrefab.GetComponent<Rigidbody>();
            rigidbody.mass = 100f;
            rigidbody.drag = 0f;
            rigidbody.angularDrag = 0f;
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
            rigidbody.interpolation = RigidbodyInterpolation.None;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            rigidbody.constraints = RigidbodyConstraints.None;

            CapsuleCollider capsuleCollider = characterPrefab.GetComponent<CapsuleCollider>();
            capsuleCollider.isTrigger = false;
            capsuleCollider.material = null;
            capsuleCollider.center = new Vector3(0f, 0f, 0f);
            capsuleCollider.radius = 0.5f;
            capsuleCollider.height = 1.82f;
            capsuleCollider.direction = 1;

            KinematicCharacterMotor kinematicCharacterMotor = characterPrefab.GetComponent<KinematicCharacterMotor>();
            kinematicCharacterMotor.CharacterController = characterMotor;
            kinematicCharacterMotor.Capsule = capsuleCollider;
            kinematicCharacterMotor.Rigidbody = rigidbody;

            kinematicCharacterMotor.DetectDiscreteCollisions = false;
            kinematicCharacterMotor.GroundDetectionExtraDistance = 0f;
            kinematicCharacterMotor.MaxStepHeight = 0.2f;
            kinematicCharacterMotor.MinRequiredStepDepth = 0.1f;
            kinematicCharacterMotor.MaxStableSlopeAngle = 55f;
            kinematicCharacterMotor.MaxStableDistanceFromLedge = 0.5f;
            kinematicCharacterMotor.PreventSnappingOnLedges = false;
            kinematicCharacterMotor.MaxStableDenivelationAngle = 55f;
            kinematicCharacterMotor.RigidbodyInteractionType = RigidbodyInteractionType.None;
            kinematicCharacterMotor.PreserveAttachedRigidbodyMomentum = true;
            kinematicCharacterMotor.HasPlanarConstraint = false;
            kinematicCharacterMotor.PlanarConstraintAxis = Vector3.up;
            kinematicCharacterMotor.StepHandling = StepHandlingMethod.None;
            kinematicCharacterMotor.LedgeHandling = true;
            kinematicCharacterMotor.InteractiveRigidbodyHandling = true;
            kinematicCharacterMotor.SafeMovement = false;

            HurtBoxGroup hurtBoxGroup = model.AddComponent<HurtBoxGroup>();

            HurtBox componentInChildren = model.GetComponentInChildren<CapsuleCollider>().gameObject.AddComponent<HurtBox>();
            componentInChildren.gameObject.layer = LayerIndex.entityPrecise.intVal;
            componentInChildren.healthComponent = healthComponent;
            componentInChildren.isBullseye = true;
            componentInChildren.damageModifier = HurtBox.DamageModifier.Normal;
            componentInChildren.hurtBoxGroup = hurtBoxGroup;
            componentInChildren.indexInGroup = 0;

            hurtBoxGroup.hurtBoxes = new HurtBox[]
            {
                componentInChildren
            };

            hurtBoxGroup.mainHurtBox = componentInChildren;
            hurtBoxGroup.bullseyeCount = 1;

            //make a hitbox for shoulder bash
            HitBoxGroup hitBoxGroup = model.AddComponent<HitBoxGroup>();

            GameObject chargeHitbox = new GameObject("ChargeHitbox");
            chargeHitbox.transform.parent = characterPrefab.transform;
            chargeHitbox.transform.localPosition = new Vector3(0f, 0f, 0f);
            chargeHitbox.transform.localRotation = Quaternion.identity;
            chargeHitbox.transform.localScale = new Vector3(8f, 8f, 8f);

            HitBox hitBox = chargeHitbox.AddComponent<HitBox>();
            chargeHitbox.layer = LayerIndex.projectile.intVal;

            hitBoxGroup.hitBoxes = new HitBox[]
            {
                hitBox
            };

            hitBoxGroup.groupName = "Charge";

            FootstepHandler footstepHandler = model.AddComponent<FootstepHandler>();
            footstepHandler.baseFootstepString = "Play_player_footstep";
            footstepHandler.sprintFootstepOverrideString = "";
            footstepHandler.enableFootstepDust = true;
            footstepHandler.footstepDustPrefab = Resources.Load<GameObject>("Prefabs/GenericFootstepDust");

            RagdollController ragdollController = model.AddComponent<RagdollController>();
            ragdollController.bones = null;
            ragdollController.componentsToDisableOnRagdoll = null;

            AimAnimator aimAnimator = model.AddComponent<AimAnimator>();
            aimAnimator.inputBank = inputBankTest;
            aimAnimator.directionComponent = characterDirection;
            aimAnimator.pitchRangeMax = 55f;
            aimAnimator.pitchRangeMin = -50f;
            aimAnimator.yawRangeMin = -44f;
            aimAnimator.yawRangeMax = 44f;
            aimAnimator.pitchGiveupRange = 30f;
            aimAnimator.yawGiveupRange = 10f;
            aimAnimator.giveupDuration = 8f;

            //why cache it if we're not gonna set it?
            // i dunno honestly
            /*ShieldComponent shieldComponent =*/
            characterPrefab.AddComponent<ShieldComponent>();

#endregion
        }

        private void RegisterCharacter()
        {
            characterDisplay = PrefabAPI.InstantiateClone(characterPrefab.GetComponent<ModelLocator>().modelBaseTransform.gameObject, "EnforcerDisplay");
            characterDisplay.AddComponent<NetworkIdentity>();
            characterDisplay.AddComponent<MenuAnimator>();

            string desc = "The Enforcer is a slow but powerful character.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Batting away enemies with Shield Bash guarantees you will keep enemies at a safe range." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Make sure to use Protect and Serve against walls to prevent enemies from flanking you." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Sample Text 3." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Sample Text 4.</color>" + Environment.NewLine + Environment.NewLine;

            LanguageAPI.Add("ENFORCER_NAME", "Enforcer");
            LanguageAPI.Add("ENFORCER_DESCRIPTION", desc);
            LanguageAPI.Add("ENFORCER_SUBTITLE", "Mutated Beyond Recognition");
            LanguageAPI.Add("ENFORCER_LORE", "I'M FUCKING INVINCIBLE");

            SurvivorDef survivorDef = new SurvivorDef
            {
                name = "ENFORCER_NAME",
                unlockableName = "",
                descriptionToken = "ENFORCER_DESCRIPTION",
                primaryColor = characterColor,
                bodyPrefab = characterPrefab,
                displayPrefab = characterDisplay
            };


            SurvivorAPI.AddSurvivor(survivorDef);

            SkillSetup();

            BodyCatalog.getAdditionalEntries += delegate (List<GameObject> list)
            {
                list.Add(characterPrefab);
            };
        }

        private void RegisterBuffs() {
            BuffDef jackBootsDef = new BuffDef {
                name = "Heavyweight",
                iconPath = "Textures/BuffIcons/texBuffTempestSpeedIcon",
                buffColor = characterColor,
                canStack = false,
                isDebuff = false,
                eliteIndex = EliteIndex.None
            };
            CustomBuff jackBoots = new CustomBuff(jackBootsDef);
            EnforcerPlugin.jackBoots = BuffAPI.Add(jackBoots);
        }

        private void RegisterProjectile()
        {
            //i'm the treasure, baby, i'm the prize

            projectilePrefab = Resources.Load<GameObject>("prefabs/projectiles/CommandoGrenadeProjectile").InstantiateClone("EnforcerTearGasGrenade", true);
            tearGasPrefab = Resources.Load<GameObject>("prefabs/projectiles/SporeGrenadeProjectileDotZone").InstantiateClone("TearGasDotZone", true);

            ProjectileController grenadeController = projectilePrefab.GetComponent<ProjectileController>();
            ProjectileController tearGasController = tearGasPrefab.GetComponent<ProjectileController>();

            ProjectileDamage grenadeDamage = projectilePrefab.GetComponent<ProjectileDamage>();
            ProjectileDamage tearGasDamage = tearGasPrefab.GetComponent<ProjectileDamage>();

            ProjectileImpactExplosion grenadeImpact = projectilePrefab.GetComponent<ProjectileImpactExplosion>();

            Destroy(tearGasPrefab.GetComponent<ProjectileDotZone>());

            EProjectileDotZone dotZone = tearGasPrefab.AddComponent<EProjectileDotZone>();

            grenadeController.procCoefficient = 1;
            tearGasController.procCoefficient = 0;

            grenadeDamage.crit = false;
            grenadeDamage.damage = 1.5f;
            grenadeDamage.damageColorIndex = DamageColorIndex.Default;
            grenadeDamage.damageType = DamageType.Stun1s;
            grenadeDamage.force = -1000;

            tearGasDamage.crit = false;
            tearGasDamage.damage = 0;
            tearGasDamage.damageColorIndex = DamageColorIndex.WeakPoint;
            tearGasDamage.damageType = DamageType.WeakOnHit;
            tearGasDamage.force = -1000;

            dotZone.damageCoefficient = 0f;
            dotZone.iEffect = Resources.Load<GameObject>("prefabs/effects/impacteffects/SporeGrenadeRepeatHitImpact");
            dotZone.forceVector = new Vector3(0, 0, 0);
            dotZone.overlapProcCoefficient = 0f;
            dotZone.fireFrequency = 5;
            dotZone.resetFrequency = 20;
            dotZone.lifetime = 7;
            dotZone.onBegin = new UnityEngine.Events.UnityEvent();
            dotZone.onEnd = new UnityEngine.Events.UnityEvent();

            //change "Play_commando_M2_grenade_bounce" to whatever the event name of enforcer's tear gas grenade is
            grenadeImpact.lifetimeExpiredSoundString = "Play_commando_M2_grenade_bounce";
            grenadeImpact.offsetForLifetimeExpiredSound = 1;
            grenadeImpact.destroyOnEnemy = false;
            grenadeImpact.destroyOnWorld = false;
            grenadeImpact.timerAfterImpact = true;
            grenadeImpact.falloffModel = BlastAttack.FalloffModel.SweetSpot;
            grenadeImpact.lifetime = 10;
            grenadeImpact.lifetimeAfterImpact = 0;
            grenadeImpact.lifetimeRandomOffset = 0;
            grenadeImpact.blastRadius = 12;
            grenadeImpact.blastDamageCoefficient = 1;
            grenadeImpact.blastProcCoefficient = 1;
            grenadeImpact.fireChildren = true;
            grenadeImpact.childrenCount = 1;
            grenadeImpact.childrenProjectilePrefab = tearGasPrefab;
            grenadeImpact.childrenDamageCoefficient = 0;

            /*ProjectileController controller = projectilePrefab.GetComponent<ProjectileController>();
            ProjectileController otherController = projectilePrefab.GetComponent<ProjectileController>();
            ProjectileImpactExplosion impactExplosion = projectilePrefab.GetComponent<ProjectileImpactExplosion>();
            ProjectileDamage damage = projectilePrefab.GetComponent<ProjectileDamage>();


            ProjectileOverlapAttack SHITTYATTACK = tearGasPrefab.GetComponent<EProjectileDotZone>();
            //NOWNOWNOWNOWNOWNOWNOWNOWNOWNOWNOWNOWNOWNOWNOWNOWNOWNOWNOWNOWNOWNOWNOWNOWNOWNOWNOWNOWNOWNOWNOWNOWNOWNOWNOWNOW
            Destroy(SHITTYATTACK);
            EProjectileDotZone CHADATTACK = tearGasPrefab.AddComponent<EProjectileDotZone>();
            ....what the hell happened here??


            ProjectileDamage otherDamage = tearGasPrefab.GetComponent<ProjectileDamage>();

            //damage.force = -1000;
            damage.damage = 0;
            damage.damageType = DamageType.WeakOnHit;
            otherDamage.damageType = DamageType.WeakOnHit;
            otherDamage.damage = 0;
            otherDamage.force = -5000f; 
            otherController.procCoefficient = 0;

            //uncomment this line for funny
            //controller.ghostPrefab = Assets.MainAssetBundle.LoadAsset<GameObject>("mdlEnforcer"); 

            //uncomment this line when we have sfx
            //impactExplosion.lifetimeExpiredSoundString = "Play_commando_M2_grenade_bounce";
            impactExplosion.offsetForLifetimeExpiredSound = 1;
            impactExplosion.destroyOnEnemy = false;
            impactExplosion.destroyOnWorld = false;
            impactExplosion.timerAfterImpact = true;
            impactExplosion.falloffModel = BlastAttack.FalloffModel.SweetSpot;
            impactExplosion.lifetime = 10;
            impactExplosion.lifetimeAfterImpact = 0;
            impactExplosion.lifetimeRandomOffset = 0;
            impactExplosion.blastRadius = 12;
            impactExplosion.blastDamageCoefficient = 1;
            impactExplosion.blastProcCoefficient = 1;
            impactExplosion.fireChildren = true;
            impactExplosion.childrenCount = 1;
            impactExplosion.childrenProjectilePrefab = tearGasPrefab;
            impactExplosion.childrenDamageCoefficient = 0*/



            ProjectileCatalog.getAdditionalEntries += delegate (List<GameObject> list) {
                list.Add(projectilePrefab);
                list.Add(tearGasPrefab);
            };
        }


        private void CreateDoppelganger()
        {
            // commando ai for now
            doppelganger = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/CharacterMasters/CommandoMonsterMaster"), "EnforcerMonsterMaster");

            MasterCatalog.getAdditionalEntries += delegate (List<GameObject> list)
            {
                list.Add(doppelganger);
            };

            CharacterMaster component = doppelganger.GetComponent<CharacterMaster>();
            component.bodyPrefab = characterPrefab;
        }

        //add modifiers to your voids please 
        // no go fuck yourself :^)
        // suck my dick 

        private void SkillSetup()
        {
            foreach (GenericSkill obj in characterPrefab.GetComponentsInChildren<GenericSkill>())
            {
                BaseUnityPlugin.DestroyImmediate(obj);
            }

            skillLocator = characterPrefab.GetComponent<SkillLocator>();

            PrimarySetup();
            SecondarySetup();
            UtilitySetup();
            SpecialSetup();
        }

        private void PrimarySetup()
        {
            LoadoutAPI.AddSkill(typeof(RiotShotgun));

            string desc = "Fire a short range <style=cIsUtility>piercing blast</style> for <style=cIsDamage>" + RiotShotgun.projectileCount + "x" + 100f * RiotShotgun.damageCoefficient + "% damage.";

            LanguageAPI.Add("ENFORCER_PRIMARY_SHOTGUN_NAME", "Riot Shotgun");
            LanguageAPI.Add("ENFORCER_PRIMARY_SHOTGUN_DESCRIPTION", desc);

            SkillDef mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(RiotShotgun));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 0f;
            mySkillDef.beginSkillCooldownOnSkillEnd = false;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Any;
            mySkillDef.isBullets = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.noSprint = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.shootDelay = 0f;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Assets.icon1;
            mySkillDef.skillDescriptionToken = "ENFORCER_PRIMARY_SHOTGUN_DESCRIPTION";
            mySkillDef.skillName = "ENFORCER_PRIMARY_SHOTGUN_NAME";
            mySkillDef.skillNameToken = "ENFORCER_PRIMARY_SHOTGUN_NAME";

            LoadoutAPI.AddSkillDef(mySkillDef);

            skillLocator.primary = characterPrefab.AddComponent<GenericSkill>();
            SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
            newFamily.variants = new SkillFamily.Variant[1];
            LoadoutAPI.AddSkillFamily(newFamily);
            skillLocator.primary.SetFieldValue("_skillFamily", newFamily);
            SkillFamily skillFamily = skillLocator.primary.skillFamily;

            skillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };
        }

        private void SecondarySetup()
        {
            LoadoutAPI.AddSkill(typeof(ShieldBash));
            LoadoutAPI.AddSkill(typeof(ExperimentalShieldBash));
            LoadoutAPI.AddSkill(typeof(ShoulderBash));
            LoadoutAPI.AddSkill(typeof(ShoulderBashImpact));

            string desc = "Smash nearby enemies for <style=cIsDamage>" + 100f * ShieldBash.damageCoefficient + " damage, stunning</style> and <style=cIsUtility>knocking them back</style>. <style=cIsUtility>Deflects projectiles.</style>";

            LanguageAPI.Add("ENFORCER_SECONDARY_BASH_NAME", "Shield Bash");
            LanguageAPI.Add("ENFORCER_SECONDARY_BASH_DESCRIPTION", desc);

            SkillDef mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(ExperimentalShieldBash));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 6f;
            mySkillDef.beginSkillCooldownOnSkillEnd = false;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Skill;
            mySkillDef.isBullets = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.noSprint = false;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.shootDelay = 0f;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Assets.icon2;
            mySkillDef.skillDescriptionToken = "ENFORCER_SECONDARY_BASH_DESCRIPTION";
            mySkillDef.skillName = "ENFORCER_SECONDARY_BASH_NAME";
            mySkillDef.skillNameToken = "ENFORCER_SECONDARY_BASH_NAME";

            LoadoutAPI.AddSkillDef(mySkillDef);

            skillLocator.secondary = characterPrefab.AddComponent<GenericSkill>();
            SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
            newFamily.variants = new SkillFamily.Variant[1];
            LoadoutAPI.AddSkillFamily(newFamily);
            skillLocator.secondary.SetFieldValue("_skillFamily", newFamily);
            SkillFamily skillFamily = skillLocator.secondary.skillFamily;

            skillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };
        }

        private void UtilitySetup()
        {
            //these currently are the same skill, just getting the skilldefs out of the way for now

            LoadoutAPI.AddSkill(typeof(StunGrenade));

            LanguageAPI.Add("ENFORCER_UTILITY_TEARGAS_NAME", "Tear Gas");
            LanguageAPI.Add("ENFORCER_UTILITY_TEARGAS_DESCRIPTION", "Throw a grenade that deals <style=cIsDamage>150% damage</style> which explodes into tear gas that <style=cIsDamage>weakens enemies</style>.");

            SkillDef mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(StunGrenade));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 12;
            mySkillDef.beginSkillCooldownOnSkillEnd = false;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Skill;
            mySkillDef.isBullets = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.noSprint = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.shootDelay = 0f;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Assets.icon3;
            mySkillDef.skillDescriptionToken = "ENFORCER_UTILITY_TEARGAS_DESCRIPTION";
            mySkillDef.skillName = "ENFORCER_UTILITY_TEARGAS_NAME";
            mySkillDef.skillNameToken = "ENFORCER_UTILITY_TEARGAS_NAME";

            LoadoutAPI.AddSkillDef(mySkillDef);

            skillLocator.utility = characterPrefab.AddComponent<GenericSkill>();
            SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
            newFamily.variants = new SkillFamily.Variant[1];
            LoadoutAPI.AddSkillFamily(newFamily);
            skillLocator.utility.SetFieldValue("_skillFamily", newFamily);
            SkillFamily skillFamily = skillLocator.utility.skillFamily;

            skillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };

            //LoadoutAPI.AddSkill(typeof(StunGrenade));

            LanguageAPI.Add("ENFORCER_UTILITY_STUNGRENADE_NAME", "Stun Grenade");
            LanguageAPI.Add("ENFORCER_UTILITY_STUNGRENADE_DESCRIPTION", "Launch a stun grenade, stunning enemies in a huge radius for 150% damage. Holds up to 6 stock. Can bounce at shallow angles.");

            mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(StunGrenade));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 6;
            mySkillDef.baseRechargeInterval = 8f;
            mySkillDef.beginSkillCooldownOnSkillEnd = false;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Skill;
            mySkillDef.isBullets = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.noSprint = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.shootDelay = 0f;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Assets.icon3B;
            mySkillDef.skillDescriptionToken = "ENFORCER_UTILITY_STUNGRENADE_DESCRIPTION";
            mySkillDef.skillName = "ENFORCER_UTILITY_STUNGRENADE_NAME";
            mySkillDef.skillNameToken = "ENFORCER_UTILITY_STUNGRENADE_NAME";

            LoadoutAPI.AddSkillDef(mySkillDef);

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };
        }

        private void SpecialSetup()
        {
            LoadoutAPI.AddSkill(typeof(ProtectAndServe));

            LanguageAPI.Add("ENFORCER_SPECIAL_SHIELDUP_NAME", "Protect and Serve");
            LanguageAPI.Add("ENFORCER_SPECIAL_SHIELDUP_DESCRIPTION", "Take a defensive stance, <style=cIsUtility>blocking all damage from the front</style>. <style=cIsDamage>Increases your rate of fire</style>, but prevents sprinting and jumping.");

            SkillDef mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(ProtectAndServe));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 0f;
            mySkillDef.beginSkillCooldownOnSkillEnd = false;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.PrioritySkill;
            mySkillDef.isBullets = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.noSprint = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.shootDelay = 0f;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Assets.icon4;
            mySkillDef.skillDescriptionToken = "ENFORCER_SPECIAL_SHIELDUP_DESCRIPTION";
            mySkillDef.skillName = "ENFORCER_SPECIAL_SHIELDUP_NAME";
            mySkillDef.skillNameToken = "ENFORCER_SPECIAL_SHIELDUP_NAME";

            LoadoutAPI.AddSkillDef(mySkillDef);

            skillLocator.special = characterPrefab.AddComponent<GenericSkill>();
            SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
            newFamily.variants = new SkillFamily.Variant[1];
            LoadoutAPI.AddSkillFamily(newFamily);
            skillLocator.special.SetFieldValue("_skillFamily", newFamily);
            SkillFamily skillFamily = skillLocator.special.skillFamily;

            skillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };
        }
    }

    //this is temporary and not networked, i'll add a custom animator later on
    public class MenuAnimator : MonoBehaviour
    {
        internal void OnEnable()
        {
            bool flag = base.gameObject.transform.parent.gameObject.name == "CharacterPad";
            if (flag)
            {
                base.StartCoroutine(this.SpawnAnimation());
            }
        }

        private IEnumerator SpawnAnimation()
        {
            Animator animator = base.GetComponentInChildren<Animator>();

            Util.PlayScaledSound(EntityStates.ScavMonster.FindItem.sound, base.gameObject, 0.75f);
            PlayAnimation("FullBody, Override", "Menu", "", 1, animator);

            yield break;
        }

        private void PlayAnimation(string layerName, string animationStateName, string playbackRateParam, float duration, Animator animator)
        {
            int layerIndex = animator.GetLayerIndex(layerName);
            animator.SetFloat(playbackRateParam, 1f);
            animator.PlayInFixedTime(animationStateName, layerIndex, 0f);
            animator.Update(0f);
            float length = animator.GetCurrentAnimatorStateInfo(layerIndex).length;
            animator.SetFloat(playbackRateParam, length / duration);
        }
    }

    public static class Assets
    {
        public static AssetBundle MainAssetBundle = null;

        public static AssetBundle TempAssetBundle = null;

        public static Texture charPortrait;

        //public static Sprite iconP;
        public static Sprite icon1;//shotgun
        public static Sprite icon2;//shield bash
        public static Sprite icon3;//tear gas
        public static Sprite icon3B;//stun grenade
        public static Sprite icon4;//protect and serve
        public static Sprite icon4B;//protect and serve cancel

        public static GameObject grenade;

        public static void PopulateAssets()
        {
            if (MainAssetBundle == null)
            {
                using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Enforcer.enforcer"))
                {
                    MainAssetBundle = AssetBundle.LoadFromStream(assetStream);
                }
            }

            //this shit doesn't fucking work i give up i'm going to bed fuck this
            //get fucked kiddo
            /*if (TempAssetBundle == null)
            {
                using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Enforcer.grenadeBundle"))
                {
                    TempAssetBundle = AssetBundle.LoadFromStream(assetStream);
                }
            }*/

            // for the soundbank later
            /*using (Stream manifestResourceStream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream("Enforcer.OurReallyCoolFuckingSoundbank.bnk"))
            {
                byte[] array = new byte[manifestResourceStream2.Length];
                manifestResourceStream2.Read(array, 0, array.Length);
                SoundAPI.SoundBanks.Add(array);
            }*/

            charPortrait = MainAssetBundle.LoadAsset<Sprite>("EnforcerBody").texture;

            //iconP = MainAssetBundle.LoadAsset<Sprite>("PassiveIcon");
            icon1 = MainAssetBundle.LoadAsset<Sprite>("Skill1Icon");
            icon2 = MainAssetBundle.LoadAsset<Sprite>("Skill2Icon");
            icon3 = MainAssetBundle.LoadAsset<Sprite>("Skill3Icon");
            icon3B = MainAssetBundle.LoadAsset<Sprite>("Skill3BIcon");
            icon4 = MainAssetBundle.LoadAsset<Sprite>("Skill4Icon");
            icon4B = MainAssetBundle.LoadAsset<Sprite>("Skill4BIcon");

            //grenade = TempAssetBundle.LoadAsset<GameObject>("Grenade");
        }
    }
}