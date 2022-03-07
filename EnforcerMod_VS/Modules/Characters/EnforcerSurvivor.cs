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
            podPrefab = Assets.LoadAsset<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

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

        public override ItemDisplaysBase itemDisplays => null; //todo CUM2

        public override UnlockableDef characterUnlockableDef => EnforcerUnlockables.enforcerUnlockableDef;

        public override ConfigEntry<bool> characterEnabledConfig => null;

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

            characterBodyModel.gameObject.AddComponent<EnforcerComponent>();
            characterBodyModel.gameObject.AddComponent<EnforcerWeaponComponent>();
            //characterBodyModel.gameObject.AddComponent<EnforcerNetworkComponent>();
            characterBodyModel.gameObject.AddComponent<EnforcerLightController>();
            characterBodyModel.gameObject.AddComponent<EnforcerLightControllerAlt>();

            if (EnforcerPlugin.EnforcerModPlugin.IDPHelperInstalled) {
                characterBodyModel.gameObject.AddComponent<EnforcerItemDisplayEditorComponent>();
            }
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
                Modules.States.AddSkill(memes[i]);
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

            Modules.Skills.AddUnlockablesToFamily(bodyPrefab.GetComponent<SkillLocator>().primary.skillFamily,
                                                  null,
                                                  EnforcerUnlockables.enforcerStunGrenadeUnlockableDef);
        }

        public static SkillDef shielEnterDef;
        public static SkillDef shieldExitDef;

        public static SkillDef energyShieldEnterDef;
        public static SkillDef energyShieldExitDef;

        public static SkillDef boardEnterDef;
        public static SkillDef boardExitDef;

        private void InitializeSpecialSkills() {

            Content.AddEntityState(typeof(ProtectAndServe));
            Content.AddEntityState(typeof(EnergyShield));
            Content.AddEntityState(typeof(Skateboard));

            shielEnterDef = EnforcerSkillDefs.SpecialSkillDef_ProtectAndServe();
            shieldExitDef = EnforcerSkillDefs.SpecialSkillDef_ShieldDown();

            energyShieldEnterDef = EnforcerSkillDefs.SpecialSkillDef_EnergyShield();
            energyShieldExitDef = EnforcerSkillDefs.SpecialSkillDef_EnergyShieldDown();

            boardEnterDef = EnforcerSkillDefs.SpecialSkillDef_SkamteBord();
            boardExitDef = EnforcerSkillDefs.SpecialSkillDef_SkamteBordDown();

            Modules.Skills.AddSpecialSkills(bodyPrefab, shielEnterDef);

            //CSSPDC
            SkillLocator skillLocator = bodyPrefab.GetComponent<SkillLocator>();
            CharacterSelectSurvivorPreviewDisplayController previewController = displayPrefab.GetComponent<CharacterSelectSurvivorPreviewDisplayController>();

            previewController.skillChangeResponses[4].triggerSkillFamily = skillLocator.special.skillFamily;
            previewController.skillChangeResponses[4].triggerSkill = shielEnterDef;


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

        #region oh god

        protected override void InitializeDisplayPrefab() {
            base.InitializeDisplayPrefab();

            displayPrefab.AddComponent<MenuSoundComponent>();
            displayPrefab.AddComponent<EnforcerLightController>();
            displayPrefab.AddComponent<EnforcerLightControllerAlt>();
        }
        #endregion
    }
}
