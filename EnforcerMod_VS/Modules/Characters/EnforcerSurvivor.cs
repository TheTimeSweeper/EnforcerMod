using BepInEx.Configuration;
using RoR2;
using System;
using UnityEngine;
using Modules;
using Modules.Characters;
using EntityStates.Enforcer;
using EntityStates;
using System.Linq;
using RoR2.Skills;
using EntityStates.Enforcer.NeutralSpecial;
using EnforcerPlugin;
using R2API;
using System.Runtime.CompilerServices;
using RoR2.CharacterAI;

namespace Modules.Characters {
    public class EnforcerSurvivor : SurvivorBase {

        public override string characterName => "Enforcer";
        public const string ENFORCER_PREFIX = "ENFORCER_";
        public override string survivorTokenPrefix => ENFORCER_PREFIX;

        public override BodyInfo bodyInfo => new BodyInfo {
            bodyName = "EnforcerBody",
            bodyNameToken = ENFORCER_PREFIX + "NAME",
            subtitleNameToken = ENFORCER_PREFIX + "SUBTITLE",
            bodyColor = new Color(0.26f, 0.27f, 0.46f),
            characterPortrait = Assets.charPortrait,
            sortPosition = 5.1f,

            crosshair = Modules.Assets.LoadCrosshair("SMG"),
            podPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            //stats
            maxHealth = Modules.Config.baseHealth.Value,
            healthRegen = Modules.Config.baseRegen.Value,
            armor = Modules.Config.baseArmor.Value,

            damage = Modules.Config.baseDamage.Value,
            crit = Modules.Config.baseCrit.Value,

            moveSpeed = Modules.Config.baseMovementSpeed.Value,
            jumpCount = 1,

            //level stats
            healthGrowth = Modules.Config.healthGrowth.Value,
            regenGrowth = Modules.Config.regenGrowth.Value,
            armorGrowth = Modules.Config.armorGrowth.Value,

            damageGrowth = Modules.Config.damageGrowth.Value,
        };

        public override CustomRendererInfo[] customRendererInfos {
            get => new CustomRendererInfo[] {
            new CustomRendererInfo {
                childName = "ShieldModel",
                //material = Materials.CreateHotpooMaterial("matEnforcerShield"),
            },
            new CustomRendererInfo {
                childName = "ShieldGlassModel",
                dontHotpoo = true,
                ignoreOverlays = true,
            },
            new CustomRendererInfo {
                childName = "SkamteBordModel",
            },

            new CustomRendererInfo {
                childName = "GunModel",
            },
            new CustomRendererInfo {
                childName = "SuperGunModel",
            },
            new CustomRendererInfo {
                childName = "HMGModel",
            },
            new CustomRendererInfo {
                childName = "HammerModel",
            },

            new CustomRendererInfo {
                childName = "PauldronModel",
            },
            new CustomRendererInfo {
                childName = "Model",
            },
        };}

        public override Type characterMainState => typeof(EnforcerMain);

        public override ItemDisplaysBase itemDisplays => null; //item displays handled by our own script instead of the new system

        public override UnlockableDef characterUnlockableDef => EnforcerUnlockables.enforcerUnlockableDef;

        public override ConfigEntry<bool> characterEnabledConfig => null;

        //SOC SOC
        public static SkillDef shieldEnterDef;
        public static SkillDef shieldExitDef;

        public static SkillDef energyShieldEnterDef;
        public static SkillDef energyShieldExitDef;

        public static SkillDef boardEnterDef;
        public static SkillDef boardExitDef;

        public override void Initialize() {
            base.Initialize();
            Hooks();
        }

        private void Hooks() {
        }

        #region characterbody stuff
        protected override void InitializeCharacterBodyAndModel() {
            base.InitializeCharacterBodyAndModel();

            //...what?
            // https://youtu.be/zRXl8Ow2bUs
            
            bodyPrefab.GetComponent<SfxLocator>().deathSound = Sounds.DeathSound;

            characterBodyModel.GetComponent<ChildLocator>().FindChild("Chair").GetComponent<MeshRenderer>().material.SetHotpooMaterial();

            //childLocator.FindChild("BungusShieldFill").gameObject.AddComponent<ObjectScaleCurve>().timeMax = 0.3f;

            bodyPrefab.gameObject.AddComponent<EnforcerComponent>();
            bodyPrefab.gameObject.AddComponent<EnforcerWeaponComponent>();
            bodyPrefab.gameObject.AddComponent<EnforcerNetworkComponent>();
            bodyPrefab.gameObject.AddComponent<EnforcerLightController>();
            bodyPrefab.gameObject.AddComponent<EnforcerLightControllerAlt>();

            if (EnforcerPlugin.EnforcerModPlugin.IDPHelperInstalled) {
                characterBodyModel.gameObject.AddComponent<EnforcerItemDisplayEditorComponent>();
            }
        }

        protected override void InitializeDisplayPrefab() {
            base.InitializeDisplayPrefab();

            displayPrefab.AddComponent<MenuSoundComponent>();
            displayPrefab.AddComponent<EnforcerLightController>();
            displayPrefab.AddComponent<EnforcerLightControllerAlt>();
        }

        public override void InitializeHurtboxes(HealthComponent healthComponent) {

            HurtBoxGroup mainHurtboxGroup = characterBodyModel.gameObject.GetComponent<HurtBoxGroup>();

            ChildLocator childLocator = characterBodyModel.GetComponent<ChildLocator>();

            //make a hurtbox for the shield since this works apparently !
            HurtBox shieldHurtbox = childLocator.FindChild("ShieldHurtbox").gameObject.AddComponent<HurtBox>();
            shieldHurtbox.gameObject.layer = LayerIndex.entityPrecise.intVal;
            shieldHurtbox.healthComponent = healthComponent;
            shieldHurtbox.isBullseye = false;
            shieldHurtbox.damageModifier = HurtBox.DamageModifier.Barrier;
            shieldHurtbox.hurtBoxGroup = mainHurtboxGroup;
            shieldHurtbox.indexInGroup = 1;

            HurtBox shieldHurtbox2 = childLocator.FindChild("ShieldHurtbox2").gameObject.AddComponent<HurtBox>();
            shieldHurtbox2.gameObject.layer = LayerIndex.entityPrecise.intVal;
            shieldHurtbox2.healthComponent = healthComponent;
            shieldHurtbox2.isBullseye = false;
            shieldHurtbox2.damageModifier = HurtBox.DamageModifier.Barrier;
            shieldHurtbox2.hurtBoxGroup = mainHurtboxGroup;
            shieldHurtbox2.indexInGroup = 1;

            childLocator.FindChild("ShieldHurtboxParent").gameObject.SetActive(false);

            mainHurtboxGroup.hurtBoxes.Concat(new HurtBox[] {
                shieldHurtbox,
                shieldHurtbox2
            }).ToArray();
        }

        public override void InitializeHitboxes() {

            ChildLocator childLocator = characterBodyModel.GetComponent<ChildLocator>();

            //make a hitbox for shoulder bash
            HitBoxGroup hitBoxGroup = characterBodyModel.gameObject.AddComponent<HitBoxGroup>();

            GameObject chargeHitbox = childLocator.FindChild("ChargeHitbox").gameObject;
            HitBox hitBox = chargeHitbox.AddComponent<HitBox>();
            chargeHitbox.layer = LayerIndex.projectile.intVal;

            hitBoxGroup.hitBoxes = new HitBox[]
            {
                hitBox
            };

            hitBoxGroup.groupName = "Charge";

            //hammer hitbox
            HitBoxGroup hammerHitBoxGroup = characterBodyModel.gameObject.AddComponent<HitBoxGroup>();

            GameObject hammerHitbox = childLocator.FindChild("ActualHammerHitbox").gameObject;

            HitBox hammerHitBox = hammerHitbox.AddComponent<HitBox>();
            hammerHitbox.layer = LayerIndex.projectile.intVal;

            hammerHitBoxGroup.hitBoxes = new HitBox[]
            {
                hammerHitBox
            };

            hammerHitBoxGroup.groupName = "Hammer";

            //hammer hitbox2
            HitBoxGroup hammerHitBoxGroup2 = characterBodyModel.gameObject.AddComponent<HitBoxGroup>();

            GameObject hammerHitbox2 = childLocator.FindChild("HammerHitboxBig").gameObject;

            HitBox hammerHitBox2 = hammerHitbox2.AddComponent<HitBox>();
            hammerHitbox2.layer = LayerIndex.projectile.intVal;

            hammerHitBoxGroup2.hitBoxes = new HitBox[]
            {
                hammerHitBox2
            };

            hammerHitBoxGroup2.groupName = "HammerBig";

            FootstepHandler footstepHandler = characterBodyModel.gameObject.AddComponent<FootstepHandler>();
            footstepHandler.baseFootstepString = "Play_player_footstep";
            footstepHandler.sprintFootstepOverrideString = "";
            footstepHandler.enableFootstepDust = true;
            footstepHandler.footstepDustPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/GenericFootstepDust");

        }

        protected override void InitializeEntityStateMachine() {
            base.InitializeEntityStateMachine();

            EntityStateMachine octagonapus = bodyPrefab.AddComponent<EntityStateMachine>();

            octagonapus.customName = "EnforcerParry";

            NetworkStateMachine networkStateMachine = bodyPrefab.GetComponent<NetworkStateMachine>();
            networkStateMachine.stateMachines = networkStateMachine.stateMachines.Append(octagonapus).ToArray();

            SerializableEntityStateType idleState = new SerializableEntityStateType(typeof(Idle));
            octagonapus.initialStateType = idleState;
            octagonapus.mainStateType = idleState;
        }
        #endregion characterbody stuff

        #region skills

        //add modifiers to your voids please 
        // no go fuck yourself :^)
        // suck my dick 

        public override void InitializeSkills() {

            Modules.Skills.CreateSkillFamilies(bodyPrefab);

            InitializePrimarySkills();

            InitializeSecondarySkills();

            InitializeUtilitySkills();

            InitializeSpecialSkills();

            if (EnforcerModPlugin.ScepterInstalled) {
                InitializeScepterSkills();
            }

            FinalizeCSSPreviewDisplayController();

            //todo: fancy emote system/genericEmote
            MemeSetup();
        }

        private void MemeSetup() {
            Type[] memes = new Type[]
            {
                typeof(SirenToggle),
                typeof(DefaultDance),
                typeof(FLINTLOCKWOOD),
                typeof(Rest),
                typeof(Enforcer.Emotes.EnforcerSalute),
                typeof(EntityStates.Nemforcer.Emotes.Salute),
            };

            for (int i = 0; i < memes.Length; i++) {
                Modules.Content.AddEntityState(memes[i]);
            }
        }

        private void InitializePrimarySkills() {

            Content.AddEntityState(typeof(FireNeedler),
                                   typeof(RiotShotgun),
                                   typeof(SuperShotgun2),
                                   typeof(FireMachineGun),
                                   typeof(HammerSwing));

            SkillDef primarySkillDef1 = EnforcerSkillDefs.PrimarySkillDef_RiotShotgun();
            SkillDef primarySkillDef2 = EnforcerSkillDefs.PrimarySkillDef_SuperShotgun();
            SkillDef primarySkillDef3 = EnforcerSkillDefs.PrimarySkillDef_AssaultRifle();
            SkillDef primarySkillDef4 = EnforcerSkillDefs.PrimarySkillDef_Hammer();
            
            Modules.Skills.AddPrimarySkills(bodyPrefab, primarySkillDef1, primarySkillDef2, primarySkillDef3);

            Modules.Skills.AddUnlockablesToFamily(bodyPrefab.GetComponent<SkillLocator>().primary.skillFamily, 
                                                  null,
                                                  EnforcerUnlockables.enforcerDoomUnlockableDef,
                                                  EnforcerUnlockables.enforcerARUnlockableDef);

            SkillLocator skillLocator = bodyPrefab.GetComponent<SkillLocator>();
            CharacterSelectSurvivorPreviewDisplayController previewController = displayPrefab.GetComponent<CharacterSelectSurvivorPreviewDisplayController>();

            previewController.skillChangeResponses[0].triggerSkillFamily = skillLocator.primary.skillFamily;
            previewController.skillChangeResponses[0].triggerSkill = primarySkillDef1;
            previewController.skillChangeResponses[1].triggerSkillFamily = skillLocator.primary.skillFamily;
            previewController.skillChangeResponses[1].triggerSkill = primarySkillDef2;
            previewController.skillChangeResponses[2].triggerSkillFamily = skillLocator.primary.skillFamily;
            previewController.skillChangeResponses[2].triggerSkill = primarySkillDef3;

            if (Modules.Config.cursed.Value) {
                Modules.Skills.AddSkillToFamily(skillLocator.primary.skillFamily, primarySkillDef4);

                previewController.skillChangeResponses[3].triggerSkillFamily = skillLocator.primary.skillFamily;
                previewController.skillChangeResponses[3].triggerSkill = primarySkillDef4;
            }
        }

        private void InitializeSecondarySkills() {

            Content.AddEntityState(typeof(ShieldBash));
            Content.AddEntityState(typeof(ShoulderBash));
            Content.AddEntityState(typeof(ShoulderBashImpact));

            SkillDef secondarySkilldef1 = EnforcerSkillDefs.SecondarySkillDef_Bash();

            Modules.Skills.AddSecondarySkills(bodyPrefab, secondarySkilldef1);
        }

        private void InitializeUtilitySkills() {

            Content.AddEntityState(typeof(AimTearGas));
            Content.AddEntityState(typeof(TearGas));
            Content.AddEntityState(typeof(StunGrenade));

            SkillDef utilitySkillDef1 = EnforcerSkillDefs.UtilitySkillDef_TearGas();
            SkillDef utilitySkillDef2 = EnforcerSkillDefs.UtilitySkillDef_StunGrenade();

            Modules.Skills.AddUtilitySkills(bodyPrefab, utilitySkillDef1, utilitySkillDef2);

            Modules.Skills.AddUnlockablesToFamily(bodyPrefab.GetComponent<SkillLocator>().utility.skillFamily,
                                                  null,
                                                  EnforcerUnlockables.enforcerStunGrenadeUnlockableDef);
        }

        private void InitializeSpecialSkills() {

            Content.AddEntityState(typeof(ProtectAndServe));
            Content.AddEntityState(typeof(EnergyShield));
            Content.AddEntityState(typeof(Skateboard));

            shieldEnterDef = EnforcerSkillDefs.SpecialSkillDef_ProtectAndServe();
            shieldExitDef = EnforcerSkillDefs.SpecialSkillDef_ShieldDown();

            energyShieldEnterDef = EnforcerSkillDefs.SpecialSkillDef_EnergyShield();
            energyShieldExitDef = EnforcerSkillDefs.SpecialSkillDef_EnergyShieldDown();

            boardEnterDef = EnforcerSkillDefs.SpecialSkillDef_SkamteBord();
            boardExitDef = EnforcerSkillDefs.SpecialSkillDef_SkamteBordDown();

            Modules.Skills.AddSpecialSkills(bodyPrefab, shieldEnterDef);

            //CSSPDC
            SkillLocator skillLocator = bodyPrefab.GetComponent<SkillLocator>();
            CharacterSelectSurvivorPreviewDisplayController previewController = displayPrefab.GetComponent<CharacterSelectSurvivorPreviewDisplayController>();

            previewController.skillChangeResponses[4].triggerSkillFamily = skillLocator.special.skillFamily;
            previewController.skillChangeResponses[4].triggerSkill = shieldEnterDef;


            if (Modules.Config.cursed.Value) {

                Modules.Skills.AddSkillToFamily(skillLocator.special.skillFamily, boardEnterDef);

                //CSSPDC
                previewController.skillChangeResponses[5].triggerSkillFamily = skillLocator.special.skillFamily;
                previewController.skillChangeResponses[5].triggerSkill = boardEnterDef;

                ////rip energy shield lol
                ////previewController.skillChangeResponses[6].triggerSkillFamily = skillLocator.special.skillFamily;
                ////previewController.skillChangeResponses[6].triggerSkill = energyShieldEnterDef;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private void InitializeScepterSkills() {

            SkillDef tearGasScepterDef = EnforcerSkillDefs.UtilitySkillDef_TearGas();
            tearGasScepterDef.activationState = new SerializableEntityStateType(typeof(AimDamageGas));
            tearGasScepterDef.icon = Assets.icon30TearGasScepter;
            tearGasScepterDef.skillDescriptionToken = "ENFORCER_UTILITY_TEARGASSCEPTER_DESCRIPTION";
            tearGasScepterDef.skillName = "ENFORCER_UTILITY_TEARGASSCEPTER_NAME";
            tearGasScepterDef.skillNameToken = "ENFORCER_UTILITY_TEARGASSCEPTER_NAME";
            tearGasScepterDef.keywordTokens = new string[] {
                "KEYWORD_BLINDED"
            };

            Modules.Content.AddSkillDef(tearGasScepterDef);
            Modules.Content.AddEntityState(typeof(AimDamageGas));

            SkillDef shockGrenadeDef = EnforcerSkillDefs.UtilitySkillDef_StunGrenade();
            shockGrenadeDef.activationState = new SerializableEntityStateType(typeof(ShockGrenade));
            shockGrenadeDef.icon = Assets.icon31StunGrenadeScepter;
            shockGrenadeDef.skillDescriptionToken = "ENFORCER_UTILITY_SHOCKGRENADE_DESCRIPTION";
            shockGrenadeDef.skillName = "ENFORCER_UTILITY_SHOCKGRENADE_NAME";
            shockGrenadeDef.skillNameToken = "ENFORCER_UTILITY_SHOCKGRENADE_NAME";
            shockGrenadeDef.keywordTokens = new string[] {
                "KEYWORD_SHOCKING"
            };

            Modules.Content.AddSkillDef(shockGrenadeDef);
            Modules.Content.AddEntityState(typeof(ShockGrenade));

            AncientScepter.AncientScepterItem.instance.RegisterScepterSkill(tearGasScepterDef, "EnforcerBody", SkillSlot.Utility, 0);
            AncientScepter.AncientScepterItem.instance.RegisterScepterSkill(shockGrenadeDef, "EnforcerBody", SkillSlot.Utility, 1);
        }

        #endregion skills

        #region ai
        public override void InitializeDoppelganger() {

            GameObject doppelganger = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterMasters/MercMonsterMaster"), "EnforcerMonsterMaster");

            foreach (AISkillDriver ai in doppelganger.GetComponentsInChildren<AISkillDriver>()) {
                UnityEngine.Object.DestroyImmediate(ai);
            }

            BaseAI baseAI = doppelganger.GetComponent<BaseAI>();
            baseAI.aimVectorMaxSpeed = 40;
            baseAI.aimVectorDampTime = 0.2f;

            AISkillDriver exitShieldDriver = doppelganger.AddComponent<AISkillDriver>();
            exitShieldDriver.customName = "ExitShield";
            exitShieldDriver.movementType = AISkillDriver.MovementType.Stop;
            exitShieldDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            exitShieldDriver.activationRequiresAimConfirmation = false;
            exitShieldDriver.activationRequiresTargetLoS = false;
            exitShieldDriver.selectionRequiresTargetLoS = false;
            exitShieldDriver.maxDistance = 512f;
            exitShieldDriver.minDistance = 45f;
            exitShieldDriver.requireSkillReady = true;
            exitShieldDriver.aimType = AISkillDriver.AimType.MoveDirection;
            exitShieldDriver.ignoreNodeGraph = false;
            exitShieldDriver.moveInputScale = 1f;
            exitShieldDriver.driverUpdateTimerOverride = 0.5f;
            exitShieldDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            exitShieldDriver.minTargetHealthFraction = Mathf.NegativeInfinity;
            exitShieldDriver.maxTargetHealthFraction = Mathf.Infinity;
            exitShieldDriver.minUserHealthFraction = Mathf.NegativeInfinity;
            exitShieldDriver.maxUserHealthFraction = Mathf.Infinity;
            exitShieldDriver.skillSlot = SkillSlot.Special;
            exitShieldDriver.requiredSkill = shieldExitDef;

            /*AISkillDriver grenadeDriver = doppelganger.AddComponent<AISkillDriver>();
            grenadeDriver.customName = "ThrowGrenade";
            grenadeDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            grenadeDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            grenadeDriver.activationRequiresAimConfirmation = true;
            grenadeDriver.activationRequiresTargetLoS = false;
            grenadeDriver.selectionRequiresTargetLoS = true;
            grenadeDriver.requireSkillReady = true;
            grenadeDriver.maxDistance = 64f;
            grenadeDriver.minDistance = 0f;
            grenadeDriver.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            grenadeDriver.ignoreNodeGraph = false;
            grenadeDriver.moveInputScale = 1f;
            grenadeDriver.driverUpdateTimerOverride = 0.5f;
            grenadeDriver.buttonPressType = AISkillDriver.ButtonPressType.TapContinuous;
            grenadeDriver.minTargetHealthFraction = Mathf.NegativeInfinity;
            grenadeDriver.maxTargetHealthFraction = Mathf.Infinity;
            grenadeDriver.minUserHealthFraction = Mathf.NegativeInfinity;
            grenadeDriver.maxUserHealthFraction = Mathf.Infinity;
            grenadeDriver.skillSlot = SkillSlot.Utility;*/

            AISkillDriver shoulderBashDriver = doppelganger.AddComponent<AISkillDriver>();
            shoulderBashDriver.customName = "ShoulderBash";
            shoulderBashDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            shoulderBashDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            shoulderBashDriver.activationRequiresAimConfirmation = true;
            shoulderBashDriver.activationRequiresTargetLoS = false;
            shoulderBashDriver.selectionRequiresTargetLoS = true;
            shoulderBashDriver.maxDistance = 6f;
            shoulderBashDriver.minDistance = 0f;
            shoulderBashDriver.requireSkillReady = true;
            shoulderBashDriver.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            shoulderBashDriver.ignoreNodeGraph = true;
            shoulderBashDriver.moveInputScale = 1f;
            shoulderBashDriver.driverUpdateTimerOverride = 2f;
            shoulderBashDriver.buttonPressType = AISkillDriver.ButtonPressType.TapContinuous;
            shoulderBashDriver.minTargetHealthFraction = Mathf.NegativeInfinity;
            shoulderBashDriver.maxTargetHealthFraction = Mathf.Infinity;
            shoulderBashDriver.minUserHealthFraction = Mathf.NegativeInfinity;
            shoulderBashDriver.maxUserHealthFraction = Mathf.Infinity;
            shoulderBashDriver.skillSlot = SkillSlot.Secondary;
            //shoulderBashDriver.requiredSkill = shieldDownDef;
            shoulderBashDriver.shouldSprint = true;

            /*AISkillDriver shoulderBashPrepDriver = doppelganger.AddComponent<AISkillDriver>();
            shoulderBashPrepDriver.customName = "ShoulderBashPrep";
            shoulderBashPrepDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            shoulderBashPrepDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            shoulderBashPrepDriver.activationRequiresAimConfirmation = true;
            shoulderBashPrepDriver.activationRequiresTargetLoS = false;
            shoulderBashPrepDriver.selectionRequiresTargetLoS = false;
            shoulderBashPrepDriver.maxDistance = 12f;
            shoulderBashPrepDriver.minDistance = 0f;
            shoulderBashPrepDriver.requireSkillReady = true;
            shoulderBashPrepDriver.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            shoulderBashPrepDriver.ignoreNodeGraph = true;
            shoulderBashPrepDriver.moveInputScale = 1f;
            shoulderBashPrepDriver.driverUpdateTimerOverride = -1f;
            shoulderBashPrepDriver.buttonPressType = AISkillDriver.ButtonPressType.Abstain;
            shoulderBashPrepDriver.minTargetHealthFraction = Mathf.NegativeInfinity;
            shoulderBashPrepDriver.maxTargetHealthFraction = Mathf.Infinity;
            shoulderBashPrepDriver.minUserHealthFraction = Mathf.NegativeInfinity;
            shoulderBashPrepDriver.maxUserHealthFraction = Mathf.Infinity;
            shoulderBashPrepDriver.skillSlot = SkillSlot.Secondary;
            //shoulderBashPrepDriver.requiredSkill = shieldDownDef;
            shoulderBashPrepDriver.shouldSprint = true;*/

            AISkillDriver swapToMinigunDriver = doppelganger.AddComponent<AISkillDriver>();
            swapToMinigunDriver.customName = "EnterShield";
            swapToMinigunDriver.movementType = AISkillDriver.MovementType.Stop;
            swapToMinigunDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            swapToMinigunDriver.activationRequiresAimConfirmation = false;
            swapToMinigunDriver.activationRequiresTargetLoS = false;
            swapToMinigunDriver.selectionRequiresTargetLoS = false;
            swapToMinigunDriver.maxDistance = 30f;
            swapToMinigunDriver.minDistance = 0f;
            swapToMinigunDriver.requireSkillReady = true;
            swapToMinigunDriver.aimType = AISkillDriver.AimType.MoveDirection;
            swapToMinigunDriver.ignoreNodeGraph = true;
            swapToMinigunDriver.moveInputScale = 1f;
            swapToMinigunDriver.driverUpdateTimerOverride = -1f;
            swapToMinigunDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            swapToMinigunDriver.minTargetHealthFraction = Mathf.NegativeInfinity;
            swapToMinigunDriver.maxTargetHealthFraction = Mathf.Infinity;
            swapToMinigunDriver.minUserHealthFraction = Mathf.NegativeInfinity;
            swapToMinigunDriver.maxUserHealthFraction = Mathf.Infinity;
            swapToMinigunDriver.skillSlot = SkillSlot.Special;
            swapToMinigunDriver.requiredSkill = shieldEnterDef;

            AISkillDriver shieldBashDriver = doppelganger.AddComponent<AISkillDriver>();
            shieldBashDriver.customName = "ShieldBash";
            shieldBashDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            shieldBashDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            shieldBashDriver.activationRequiresAimConfirmation = false;
            shieldBashDriver.activationRequiresTargetLoS = false;
            shieldBashDriver.selectionRequiresTargetLoS = false;
            shieldBashDriver.maxDistance = 6f;
            shieldBashDriver.minDistance = 0f;
            shieldBashDriver.requireSkillReady = true;
            shieldBashDriver.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            shieldBashDriver.ignoreNodeGraph = true;
            shieldBashDriver.moveInputScale = 1f;
            shieldBashDriver.driverUpdateTimerOverride = -1f;
            shieldBashDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            shieldBashDriver.minTargetHealthFraction = Mathf.NegativeInfinity;
            shieldBashDriver.maxTargetHealthFraction = Mathf.Infinity;
            shieldBashDriver.minUserHealthFraction = Mathf.NegativeInfinity;
            shieldBashDriver.maxUserHealthFraction = Mathf.Infinity;
            shieldBashDriver.skillSlot = SkillSlot.Secondary;
            //shieldBashDriver.requiredSkill = shieldUpDef;

            AISkillDriver shieldFireDriver = doppelganger.AddComponent<AISkillDriver>();
            shieldFireDriver.customName = "StandAndShoot";
            shieldFireDriver.movementType = AISkillDriver.MovementType.Stop;
            shieldFireDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            shieldFireDriver.activationRequiresAimConfirmation = true;
            shieldFireDriver.activationRequiresTargetLoS = false;
            shieldFireDriver.selectionRequiresTargetLoS = false;
            shieldFireDriver.maxDistance = 16f;
            shieldFireDriver.minDistance = 0f;
            shieldFireDriver.requireSkillReady = true;
            shieldFireDriver.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            shieldFireDriver.ignoreNodeGraph = true;
            shieldFireDriver.moveInputScale = 1f;
            shieldFireDriver.driverUpdateTimerOverride = -1f;
            shieldFireDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            shieldFireDriver.minTargetHealthFraction = Mathf.NegativeInfinity;
            shieldFireDriver.maxTargetHealthFraction = Mathf.Infinity;
            shieldFireDriver.minUserHealthFraction = Mathf.NegativeInfinity;
            shieldFireDriver.maxUserHealthFraction = Mathf.Infinity;
            shieldFireDriver.skillSlot = SkillSlot.Primary;
            //shieldFireDriver.requiredSkill = shieldUpDef;

            AISkillDriver noShieldFireDriver = doppelganger.AddComponent<AISkillDriver>();
            noShieldFireDriver.customName = "StrafeAndShoot";
            noShieldFireDriver.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            noShieldFireDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            noShieldFireDriver.activationRequiresAimConfirmation = true;
            noShieldFireDriver.activationRequiresTargetLoS = false;
            noShieldFireDriver.selectionRequiresTargetLoS = false;
            noShieldFireDriver.maxDistance = 40f;
            noShieldFireDriver.minDistance = 8f;
            noShieldFireDriver.requireSkillReady = true;
            noShieldFireDriver.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            noShieldFireDriver.ignoreNodeGraph = false;
            noShieldFireDriver.moveInputScale = 1f;
            noShieldFireDriver.driverUpdateTimerOverride = -1f;
            noShieldFireDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            noShieldFireDriver.minTargetHealthFraction = Mathf.NegativeInfinity;
            noShieldFireDriver.maxTargetHealthFraction = Mathf.Infinity;
            noShieldFireDriver.minUserHealthFraction = Mathf.NegativeInfinity;
            noShieldFireDriver.maxUserHealthFraction = Mathf.Infinity;
            noShieldFireDriver.skillSlot = SkillSlot.Primary;
            //noShieldFireDriver.requiredSkill = shieldDownDef;

            AISkillDriver followDriver = doppelganger.AddComponent<AISkillDriver>();
            followDriver.customName = "Chase";
            followDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            followDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            followDriver.activationRequiresAimConfirmation = false;
            followDriver.activationRequiresTargetLoS = false;
            followDriver.maxDistance = Mathf.Infinity;
            followDriver.minDistance = 0f;
            followDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            followDriver.ignoreNodeGraph = false;
            followDriver.moveInputScale = 1f;
            followDriver.driverUpdateTimerOverride = -1f;
            followDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            followDriver.minTargetHealthFraction = Mathf.NegativeInfinity;
            followDriver.maxTargetHealthFraction = Mathf.Infinity;
            followDriver.minUserHealthFraction = Mathf.NegativeInfinity;
            followDriver.maxUserHealthFraction = Mathf.Infinity;
            followDriver.skillSlot = SkillSlot.None;

            Modules.Content.AddMasterPrefab(doppelganger);

            CharacterMaster master = doppelganger.GetComponent<CharacterMaster>();
            master.bodyPrefab = bodyPrefab;
        }
        #endregion

        #region content
        public override void InitializeUnlockables() {

            //moved to awake
            //EnforcerUnlockables.RegisterUnlockables();
        }

        public override void InitializeSkins() {
            Skins.RegisterSkins();
        }

        #endregion
    }
}
