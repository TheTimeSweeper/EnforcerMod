using BepInEx;
using BepInEx.Configuration;
using EnforcerPlugin.Modules;
using EntityStates;
using EntityStates.Enforcer;
using EntityStates.Enforcer.NeutralSpecial;
using IL.RoR2.ContentManagement;
using KinematicCharacterController;
using R2API;
using R2API.Utils;
using RoR2;
using RoR2.CharacterAI;
using RoR2.Projectile;
using RoR2.Skills;
using RoR2.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace EnforcerPlugin {

    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.DestroyedClone.AncientScepter", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.KomradeSpectre.Aetherium", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.Sivelos.SivsItems", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.K1454.SupplyDrop", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.TeamMoonstorm.Starstorm2", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.cwmlolzlz.skills", BepInDependency.DependencyFlags.SoftDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, "Enforcer", "3.1.2")]
    [R2APISubmoduleDependency(new string[]
    {
        "PrefabAPI",
        "LanguageAPI",
        "SoundAPI",
        "UnlockableAPI"
    })]

    public class EnforcerModPlugin : BaseUnityPlugin
    {
        public const string MODUID = "com.EnforcerGang.Enforcer";

        public const string characterName = "Enforcer";
        public const string characterSubtitle = "Unwavering Bastion";
        public const string characterOutro = "..and so he left, unsure of his title as protector.";
        public const string characterOutroFailure = "..and so he vanished, the planet's minorities finally at peace.";
        public const string characterLore = "\n<style=cMono>\"You don't have to do this.\"</style>\r\n\r\nThe words echoed in his head, yet he pushed forward. The pod was only a few steps away — he had a chance to leave — but something in his core kept him moving. He didn't know what it was, but he didn't question it. It was a natural force: the same force that always drove him to follow orders.\n\nThis time, however, it didn't seem so natural. There were no orders. The heavy trigger and its rhythmic thunder were his — and his alone.";

        public static EnforcerModPlugin instance;

        public static bool nemesisEnabled = true;

        internal static List<GameObject> bodyPrefabs = new List<GameObject>();
        internal static List<GameObject> masterPrefabs = new List<GameObject>();
        internal static List<GameObject> projectilePrefabs = new List<GameObject>();
        internal static List<SurvivorDef> survivorDefs = new List<SurvivorDef>();

        //i didn't want this to be static considering we're using an instance now but it throws 23 errors if i remove the static modifier 
        //i'm not dealing with that
        public static GameObject characterPrefab;
        public static GameObject characterDisplay;

        public static GameObject needlerCrosshair;

        public static GameObject nemesisSpawnEffect;

        public static GameObject bulletTracer;
        public static GameObject bulletTracerSSG;
        public static GameObject laserTracer;
        public static GameObject bungusTracer = Resources.Load<GameObject>("Prefabs/Effects/Tracers/TracerEngiTurret");
        public static GameObject minigunTracer;

        public static Material bungusMat;

        public static GameObject tearGasProjectilePrefab;
        public GameObject tearGasPrefab;

        public static GameObject damageGasProjectile;
        public GameObject damageGasEffect;

        public static GameObject stunGrenade;
        public static GameObject shockGrenade;

        public static GameObject blockEffectPrefab;
        public static GameObject heavyBlockEffectPrefab;
        public static GameObject hammerSlamEffect;

        public GameObject doppelganger;

        public static readonly Color characterColor = new Color(0.26f, 0.27f, 0.46f);

        public static SkillDef shieldDownDef;//skilldef used while shield is down
        public static SkillDef shieldUpDef;//skilldef used while shield is up
        public static SkillDef shieldOffDef;//skilldef used while shield is off
        public static SkillDef shieldOnDef;//skilldef used while shield is on

        public static SkillDef boardDownDef;
        public static SkillDef boardUpDef;

        public static SkillDef tearGasScepterDef;
        public static SkillDef shockGrenadeDef;

        public static bool cum; //don't ask
        public static bool aetheriumInstalled = false;
        public static bool sivsItemsInstalled = false;
        public static bool supplyDropInstalled = false;
        public static bool starstormInstalled = false;
        public static bool skillsPlusInstalled = false;

        //public static uint doomGuyIndex = 2;
        //public static uint engiIndex = 3;
        //public static uint stormtrooperIndex = 4;
        //public static uint frogIndex = 7;

        public static ConfigEntry<bool> forceUnlock;
        public static ConfigEntry<bool> classicShotgun;
        public static ConfigEntry<bool> classicIcons;
        public static ConfigEntry<float> headSize;
        public static ConfigEntry<bool> sprintShieldCancel;
        public static ConfigEntry<bool> sirenOnDeflect;
        public static ConfigEntry<bool> useNeedlerCrosshair;
        public static ConfigEntry<bool> cursed;
        public static ConfigEntry<bool> hateFun;
        //public static ConfigEntry<bool> femSkin;
        public static ConfigEntry<bool> shellSounds;
        public static ConfigEntry<bool> globalInvasion;
        public static ConfigEntry<bool> multipleInvasions;
        public static ConfigEntry<bool> kingDededeBoss;

        public static ConfigEntry<KeyCode> defaultDanceKey;
        public static ConfigEntry<KeyCode> flossKey;
        public static ConfigEntry<KeyCode> earlKey;
        public static ConfigEntry<KeyCode> sirensKey;

        //i don't wanna fucking buff him so i have no choice but to do this
        public static ConfigEntry<float> baseHealth;
        public static ConfigEntry<float> healthGrowth;
        public static ConfigEntry<float> baseDamage;
        public static ConfigEntry<float> damageGrowth;
        public static ConfigEntry<float> baseArmor;
        public static ConfigEntry<float> armorGrowth;
        public static ConfigEntry<float> baseMovementSpeed;
        public static ConfigEntry<float> baseCrit;
        public static ConfigEntry<float> baseRegen;
        public static ConfigEntry<float> regenGrowth;

        public static ConfigEntry<float> shotgunDamage;
        public static ConfigEntry<int> shotgunBulletCount;
        public static ConfigEntry<float> shotgunProcCoefficient;
        public static ConfigEntry<float> shotgunRange;
        public static ConfigEntry<float> shotgunSpread;

        public static ConfigEntry<float> rifleDamage;
        public static ConfigEntry<int> rifleBaseBulletCount;
        public static ConfigEntry<float> rifleProcCoefficient;
        public static ConfigEntry<float> rifleRange;
        public static ConfigEntry<float> rifleSpread;

        public static ConfigEntry<float> superDamage;
        public static ConfigEntry<float> superSpread;
        public static ConfigEntry<float> superDuration;
        public static ConfigEntry<float> superBeef;

        public static ConfigEntry<bool> balancedShieldBash;
        public static ConfigEntry<bool> stupidShieldBash;

        // a blacklist for teleporter particles- the fix for it is retarded so we just disable them on certain characters.
        private static List<string> _tpParticleBlacklist = new List<string>
        {
            "PALADIN_NAME",
            "LUNAR_KNIGHT_BODY_NAME",
            "NEMMANDO_NAME",
            "EXECUTIONER_NAME",
            "MINER_NAME"
        };


        private SkillLocator _skillLocator;
        private CharacterSelectSurvivorPreviewDisplayController _previewController;

        //更新许可证 DO WHAT THE FUCK YOU WANT TO

        //public EnforcerPlugin()
        //{
        //    //don't touch this
        //    // what does all this even do anyway?
        //    //its our plugin constructor
        //
        //i'm touching this. fuck you
        //
        //    //awake += EnforcerPlugin_Load;
        //    //start += EnforcerPlugin_LoadStart;
        //}

        private void Awake() {

            //touch this all you want tho
            ConfigShit();
            Modules.States.FixStates();
            Assets.PopulateAssets();
            SetupModCompat();

            MemeSetup();
            CreatePrefab();
            CreateDisplayPrefab();

            EnforcerUnlockables.RegisterUnlockables();
            RegisterCharacter();

            ItemDisplays.PopulateDisplays();
            Skins.RegisterSkins();

            Modules.Buffs.RegisterBuffs();
            RegisterProjectile();
            CreateDoppelganger();
            CreateCrosshair();

            if (nemesisEnabled) new NemforcerPlugin().Init();

            Hook();
            //new Modules.ContentPacks().CreateContentPack();
            RoR2.ContentManagement.ContentManager.collectContentPackProviders += ContentManager_collectContentPackProviders;
            RoR2.ContentManagement.ContentManager.onContentPacksAssigned += ContentManager_onContentPacksAssigned;

            gameObject.AddComponent<TestValueManager>();
        }

        private void SetupModCompat() {

            //aetherium item displays- dll won't compile without a reference to aetherium
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.KomradeSpectre.Aetherium")) {
                aetheriumInstalled = true;
            }
            //sivs item displays- dll won't compile without a reference
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.Sivelos.SivsItems")) {
                sivsItemsInstalled = true;
            }
            //supply drop item displays- dll won't compile without a reference
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.K1454.SupplyDrop")) {
                supplyDropInstalled = true;
            }
            //scepter stuff
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.DestroyedClone.AncientScepter")) {
                ScepterSkillSetup();
                ScepterSetup();
            }
            //shartstorm 2 xDDDD
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.TeamMoonstorm.Starstorm2")) {
                starstormInstalled = true;
            }
            //shartstorm 2 xDDDD
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.cwmlolzlz.skills")) {
                skillsPlusInstalled = true;
                SkillsPlusCompat.init();

            }
        }

        private void ContentManager_onContentPacksAssigned(HG.ReadOnlyArray<RoR2.ContentManagement.ReadOnlyContentPack> obj)
        {
            EnforcerItemDisplays.RegisterDisplays();
            if (nemesisEnabled) NemItemDisplays.RegisterDisplays();

        }

        private void ContentManager_collectContentPackProviders(RoR2.ContentManagement.ContentManager.AddContentPackProviderDelegate addContentPackProvider) {
            addContentPackProvider(new Modules.ContentPacks());
        }

        private void Start()
        {
            /*foreach(SurvivorDef i in SurvivorCatalog.survivorDefs)
            {
                Debug.Log(Language.GetString((i.displayNameToken), "EN_US") + " sort position: " + i.desiredSortPosition);
            }*/
        }

        private void ConfigShit() {
            forceUnlock = base.Config.Bind<bool>("01 - General Settings", "Force Unlock", false, "Makes Enforcer unlocked by default");
            classicShotgun = base.Config.Bind<bool>("01 - General Settings", "Classic Shotgun", false, "Use RoR1 shotgun sound");
            classicIcons = base.Config.Bind<bool>("01 - General Settings", "Classic Icons", false, "Use RoR1 skill icons");
            headSize = base.Config.Bind<float>("01 - General Settings", "Head Size", 1f, "Changes the size of Enforcer's head");
            sprintShieldCancel = base.Config.Bind<bool>("01 - General Settings", "Sprint Cancels Shield", true, "Allows Protect and Serve to be cancelled by pressing sprint rather than special again");
            sirenOnDeflect = base.Config.Bind<bool>("01 - General Settings", "Siren on Deflect", true, "Play siren sound upon deflecting a projectile");
            useNeedlerCrosshair = base.Config.Bind<bool>("01 - General Settings", "Visions Crosshair", true, "Gives every survivor the custom crosshair for Visions of Heresy");
            cursed = base.Config.Bind<bool>("01 - General Settings", "Cursed", false, "Enables extra/unfinished content. Enable at own risk.");
            //hateFun = base.Config.Bind<bool>("01 - General Settings", "I hate fun", false, "Overrides cursed. Further disables some extra content, namely skins and their achievements");
            //femSkin = base.Config.Bind<bool>("01 - General Settings", "Femforcer", false, "Enables femforcer skin. Not for good boys and girls.");
            shellSounds = base.Config.Bind<bool>("01 - General Settings", "Shell Sounds", true, "Play a sound when ejected shotgun shells hit the ground");
            globalInvasion = base.Config.Bind<bool>("01 - General Settings", "Global Invasion", false, "Allows invasions when playing any character, not just Enforcer. Purely for fun.");
            multipleInvasions = base.Config.Bind<bool>("01 - General Settings", "Multiple Invasion Bosses", false, "Allows multiple bosses to spawn from an invasion.");
            kingDededeBoss = base.Config.Bind<bool>("01 - General Settings", "King Dedede Boss", false, "Adds a King Dedede boss that spawns on Sky Meadow and post-loop Titanic Plains.");

            defaultDanceKey = base.Config.Bind<KeyCode>("02 - Keybinds", "Default Dance", KeyCode.Alpha1, "Key used to Chair");
            flossKey = base.Config.Bind<KeyCode>("02 - Keybinds", "Floss", KeyCode.Alpha2, "Key used to Salute");
            earlKey = base.Config.Bind<KeyCode>("02 - Keybinds", "Earl Run", KeyCode.Alpha3, "FLINT LOCKWOOD (when it works again)");
            sirensKey = base.Config.Bind<KeyCode>("02 - Keybinds", "Sirens", KeyCode.CapsLock, "Key used to toggle sirens");

            baseHealth = base.Config.Bind<float>(new ConfigDefinition("03 - Character Stats", "Base Health"), 160f, new ConfigDescription("", null, Array.Empty<object>()));
            healthGrowth = base.Config.Bind<float>(new ConfigDefinition("03 - Character Stats", "Health Growth"), 48f, new ConfigDescription("", null, Array.Empty<object>()));
            baseRegen = base.Config.Bind<float>(new ConfigDefinition("03 - Character Stats", "Base Health Regen"), 1f, new ConfigDescription("", null, Array.Empty<object>()));
            regenGrowth = base.Config.Bind<float>(new ConfigDefinition("03 - Character Stats", "Health Regen Growth"), 0.2f, new ConfigDescription("", null, Array.Empty<object>()));
            baseArmor = base.Config.Bind<float>(new ConfigDefinition("03 - Character Stats", "Base Armor"), 15f, new ConfigDescription("", null, Array.Empty<object>()));
            armorGrowth = base.Config.Bind<float>(new ConfigDefinition("03 - Character Stats", "Armor Growth"), 0f, new ConfigDescription("", null, Array.Empty<object>()));
            baseDamage = base.Config.Bind<float>(new ConfigDefinition("03 - Character Stats", "Base Damage"), 12f, new ConfigDescription("", null, Array.Empty<object>()));
            damageGrowth = base.Config.Bind<float>(new ConfigDefinition("03 - Character Stats", "Damage Growth"), 2.4f, new ConfigDescription("", null, Array.Empty<object>()));
            baseMovementSpeed = base.Config.Bind<float>(new ConfigDefinition("03 - Character Stats", "Base Movement Speed"), 7f, new ConfigDescription("", null, Array.Empty<object>()));
            baseCrit = base.Config.Bind<float>(new ConfigDefinition("03 - Character Stats", "Base Crit"), 1f, new ConfigDescription("", null, Array.Empty<object>()));

            shotgunDamage = base.Config.Bind<float>("04 - Riot Shotgun 3.1.0", "Damage Coefficient", 0.45f, "Damage of each pellet");
            shotgunProcCoefficient = base.Config.Bind<float>("04 - Riot Shotgun 3.1.0", "Proc Coefficient", 0.5f, "Proc Coefficient of each pellet");
            shotgunBulletCount = base.Config.Bind<int>("04 - Riot Shotgun 3.1.0", "Bullet Count", 8, "Amount of pellets fired");
            shotgunRange = base.Config.Bind<float>("04 - Riot Shotgun 3.1.0", "Range", 64f, "Maximum range");
            shotgunSpread = base.Config.Bind<float>("04 - Riot Shotgun 3.1.0", "Spread", 5.5f, "Maximum spread");

            superDamage = base.Config.Bind<float>("06 - Super Shotgun 3.1.0", "Damage Coefficient", 0.8f, "Damage of each pellet");
            superSpread = base.Config.Bind<float>("06 - Super Shotgun 3.1.0", "Max Spread", 6f, "your cheeks");
            superDuration = base.Config.Bind<float>("06 - Super Shotgun 3.1.0", "Duration Scale", 1f, $" Scale the duration of the attack (i.e. attack speed) by this value");
            superBeef = base.Config.Bind<float>("06 - Super Shotgun 3.1.0", "beef", 0.4f, "movement stop while shooting in shield. cannot go lower than 0.2 because I say so");

            /*rifleDamage = base.Config.Bind<float>(new ConfigDefinition("05 - Assault Rifle", "Damage Coefficient"), 0.85f, new ConfigDescription("Damage of each bullet", null, Array.Empty<object>()));
            rifleProcCoefficient = base.Config.Bind<float>(new ConfigDefinition("05 - Assault Rifle", "Proc Coefficient"), 0.75f, new ConfigDescription("Proc Coefficient of each bullet", null, Array.Empty<object>()));
            rifleBaseBulletCount = base.Config.Bind<int>(new ConfigDefinition("05 - Assault Rifle", "Base Bullet Count"), 3, new ConfigDescription("Bullets fired with each shot", null, Array.Empty<object>()));
            rifleRange = base.Config.Bind<float>(new ConfigDefinition("05 - Assault Rifle", "Range"), 256f, new ConfigDescription("Maximum range", null, Array.Empty<object>()));
            rifleSpread = base.Config.Bind<float>(new ConfigDefinition("05 - Assault Rifle", "Spread"), 5f, new ConfigDescription("Maximum spread", null, Array.Empty<object>()));*/

            balancedShieldBash = base.Config.Bind<bool>("07 - Shield Bash", "Balanced Knockback", false, "Applies a cap to knockback so bosses can no longer be thrown around.");
            stupidShieldBash = base.Config.Bind<bool>("07 - Shield Bash", "Ally Knockback", true, "Applies knockback to allies.");
        }
        
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private void ScepterSetup()
        {
            AncientScepter.AncientScepterItem.instance.RegisterScepterSkill(tearGasScepterDef, "EnforcerBody", SkillSlot.Utility, 0);
            AncientScepter.AncientScepterItem.instance.RegisterScepterSkill(shockGrenadeDef, "EnforcerBody", SkillSlot.Utility, 1);
        }

        private void Hook()
        {
            //add hooks here
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
            //On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnEnemyHit;
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
            On.RoR2.CharacterBody.Update += CharacterBody_Update;
            On.RoR2.CharacterBody.OnLevelUp += CharacterBody_OnLevelChanged;
            On.RoR2.CharacterMaster.OnInventoryChanged += CharacterMaster_OnInventoryChanged;
            On.RoR2.BodyCatalog.SetBodyPrefabs += BodyCatalog_SetBodyPrefabs;
            On.RoR2.SceneDirector.Start += SceneDirector_Start;
            On.EntityStates.BaseState.OnEnter += ParryState_OnEnter;
            On.RoR2.ArenaMissionController.BeginRound += ArenaMissionController_BeginRound;
            On.RoR2.ArenaMissionController.EndRound += ArenaMissionController_EndRound;
            On.RoR2.EscapeSequenceController.BeginEscapeSequence += EscapeSequenceController_BeginEscapeSequence;
            On.RoR2.UI.MainMenu.BaseMainMenuScreen.OnEnter += BaseMainMenuScreen_OnEnter;
            On.RoR2.CharacterSelectBarController.Start += CharacterSelectBarController_Start;
            On.RoR2.MapZone.TryZoneStart += MapZone_TryZoneStart;
            On.RoR2.HealthComponent.Suicide += HealthComponent_Suicide;
            //On.RoR2.TeleportOutController.OnStartClient += TeleportOutController_OnStartClient;
            //On.EntityStates.Global.Skills.LunarNeedle.FireLunarNeedle.OnEnter += FireLunarNeedle_OnEnter;

            On.RoR2.EventFunctions.DisableAllChildrenExcept += EventFunctions_DisableAllChildrenExcept;
        }

        private void EventFunctions_DisableAllChildrenExcept(On.RoR2.EventFunctions.orig_DisableAllChildrenExcept orig, EventFunctions self, GameObject objectToEnable) {
            orig(self, objectToEnable);

            for (int i = base.transform.childCount - 1; i >= 0; i--) {
                GameObject siblingObject = base.transform.GetChild(i).gameObject;
                Debug.LogWarning($"{self.gameObject.name}: {siblingObject.name}: {siblingObject == objectToEnable}");
            }
        }

        #region Hooks

        private bool isMonsoon()
        {
            bool flag = true;

            if (Run.instance.selectedDifficulty == DifficultyIndex.Easy || Run.instance.selectedDifficulty == DifficultyIndex.Normal) flag = false;

            return flag;
        }

        private void TeleportOutController_OnStartClient(On.RoR2.TeleportOutController.orig_OnStartClient orig, TeleportOutController self)
        {
            // fuck you hopoo
                // no. big particle funny
            if (self.target)
            {
                CharacterBody targetBody = self.target.GetComponent<CharacterBody>();
                if (targetBody)
                {
                    if (EnforcerModPlugin._tpParticleBlacklist.Contains(targetBody.baseNameToken))
                    {
                        self.bodyGlowParticles.Play();
                        return;
                    }
                }
            }

            orig(self);
        }

        private void MapZone_TryZoneStart(On.RoR2.MapZone.orig_TryZoneStart orig, MapZone self, Collider other)
        {
            if (other.gameObject)
            {
                CharacterBody body = other.GetComponent<CharacterBody>();
                if (body)
                {
                    if (body.baseNameToken == "NEMFORCER_NAME" || body.baseNameToken == "NEMFORCER_BOSS_NAME")
                    {
                        var teamComponent = body.teamComponent;
                        if (teamComponent)
                        {
                            if (teamComponent.teamIndex != TeamIndex.Player)
                            {
                                teamComponent.teamIndex = TeamIndex.Player;
                                orig(self, other);
                                teamComponent.teamIndex = TeamIndex.Monster;
                                return;
                            }
                        }
                    }
                }
            }
            orig(self, other);
        }

        private void ArenaMissionController_BeginRound(On.RoR2.ArenaMissionController.orig_BeginRound orig, ArenaMissionController self)
        {
            if (self.currentRound == 0)
            {
                if (isMonsoon() && Run.instance.stageClearCount >= 5)
                {
                    bool invasion = false;
                    for (int i = CharacterMaster.readOnlyInstancesList.Count - 1; i >= 0; i--)
                    {
                        CharacterMaster master = CharacterMaster.readOnlyInstancesList[i];
                        if (!globalInvasion.Value)
                        {
                            if (master.teamIndex == TeamIndex.Player && master.bodyPrefab == BodyCatalog.FindBodyPrefab("EnforcerBody"))
                            {
                                invasion = true;
                            }
                        }
                        else
                        {
                            if (master.teamIndex == TeamIndex.Player)
                            {
                                invasion = true;
                            }
                        }
                    }

                    if (invasion && NetworkServer.active)
                    {
                        ChatMessage.SendColored("You feel an overwhelming presence..", new Color(0.149f, 0.0039f, 0.2117f));
                    }
                }
            }

            orig(self);

            if (self.currentRound == 9)
            {
                if (isMonsoon() && Run.instance.stageClearCount >= 5)
                {
                    for (int i = CharacterMaster.readOnlyInstancesList.Count - 1; i >= 0; i--)
                    {
                        CharacterMaster master = CharacterMaster.readOnlyInstancesList[i];
                        if (!globalInvasion.Value)
                        {
                            if (multipleInvasions.Value)
                            {
                                if (master.teamIndex == TeamIndex.Player && master.bodyPrefab == BodyCatalog.FindBodyPrefab("EnforcerBody"))
                                {
                                    NemesisInvasionManager.PerformInvasion(new Xoroshiro128Plus(Run.instance.seed));

                                    master.gameObject.AddComponent<NemesisInvasion>().hasInvaded = true;
                                }
                            }
                            else
                            {
                                bool flag = false;
                                if (master.teamIndex == TeamIndex.Player && master.bodyPrefab == BodyCatalog.FindBodyPrefab("EnforcerBody"))
                                {
                                    flag = true;
                                    master.gameObject.AddComponent<NemesisInvasion>().hasInvaded = true;
                                }
                                if (flag) NemesisInvasionManager.PerformInvasion(new Xoroshiro128Plus(Run.instance.seed));
                            }
                        }
                        else
                        {
                            if (multipleInvasions.Value)
                            {
                                if (master.teamIndex == TeamIndex.Player && master.playerCharacterMasterController)
                                {
                                    NemesisInvasionManager.PerformInvasion(new Xoroshiro128Plus(Run.instance.seed));

                                    master.gameObject.AddComponent<NemesisInvasion>().hasInvaded = true;
                                }
                            }
                            else
                            {
                                bool flag = false;
                                if (master.teamIndex == TeamIndex.Player && master.playerCharacterMasterController)
                                {
                                    flag = true;
                                    master.gameObject.AddComponent<NemesisInvasion>().hasInvaded = true;
                                }
                                if (flag) NemesisInvasionManager.PerformInvasion(new Xoroshiro128Plus(Run.instance.seed));
                            }
                        }
                    }
                }
            }
        }

        private void ArenaMissionController_EndRound(On.RoR2.ArenaMissionController.orig_EndRound orig, ArenaMissionController self)
        {
            orig(self);

            if (self.currentRound == 9 || self.currentRound == 10)
            {
                if (isMonsoon() && Run.instance.stageClearCount < 5)
                {
                    bool pendingInvasion = false;

                    if (!globalInvasion.Value)
                    {
                        for (int i = CharacterMaster.readOnlyInstancesList.Count - 1; i >= 0; i--)
                        {
                            CharacterMaster master = CharacterMaster.readOnlyInstancesList[i];
                            if (master.teamIndex == TeamIndex.Player && master.bodyPrefab == BodyCatalog.FindBodyPrefab("EnforcerBody"))
                            {
                                master.gameObject.AddComponent<NemesisInvasion>().pendingInvasion = true;
                                pendingInvasion = true;
                            }
                        }
                    }
                    else
                    {
                        for (int i = CharacterMaster.readOnlyInstancesList.Count - 1; i >= 0; i--)
                        {
                            CharacterMaster master = CharacterMaster.readOnlyInstancesList[i];
                            if (master.teamIndex == TeamIndex.Player && master.playerCharacterMasterController)
                            {
                                master.gameObject.AddComponent<NemesisInvasion>().pendingInvasion = true;
                                pendingInvasion = true;
                            }
                        }
                    }


                    if (pendingInvasion && NetworkServer.active)
                    {
                        ChatMessage.SendColored("The void peers into you....", new Color(0.149f, 0.0039f, 0.2117f));
                    }
                }
            }
        }

        private void EscapeSequenceController_BeginEscapeSequence(On.RoR2.EscapeSequenceController.orig_BeginEscapeSequence orig, EscapeSequenceController self)
        {
            if (isMonsoon())
            {
                for (int i = CharacterMaster.readOnlyInstancesList.Count - 1; i >= 0; i--)
                {
                    CharacterMaster master = CharacterMaster.readOnlyInstancesList[i];
                    bool hasInvaded = false;

                    if (!globalInvasion.Value)
                    {
                        if (master.teamIndex == TeamIndex.Player && master.bodyPrefab == BodyCatalog.FindBodyPrefab("EnforcerBody") && master.GetBody())
                        {
                            var j = master.gameObject.GetComponent<NemesisInvasion>();
                            if (j)
                            {
                                if (j.pendingInvasion && !j.hasInvaded)
                                {
                                    j.pendingInvasion = false;
                                    j.hasInvaded = true;

                                    if (multipleInvasions.Value) NemesisInvasionManager.PerformInvasion(new Xoroshiro128Plus(Run.instance.seed));
                                    else if (!hasInvaded) NemesisInvasionManager.PerformInvasion(new Xoroshiro128Plus(Run.instance.seed));

                                    hasInvaded = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (master.teamIndex == TeamIndex.Player && master.playerCharacterMasterController && master.GetBody())
                        {
                            var j = master.gameObject.GetComponent<NemesisInvasion>();
                            if (j)
                            {
                                if (j.pendingInvasion && !j.hasInvaded)
                                {
                                    j.pendingInvasion = false;
                                    j.hasInvaded = true;

                                    if (multipleInvasions.Value) NemesisInvasionManager.PerformInvasion(new Xoroshiro128Plus(Run.instance.seed));
                                    else if (!hasInvaded) NemesisInvasionManager.PerformInvasion(new Xoroshiro128Plus(Run.instance.seed));

                                    hasInvaded = true;
                                }
                            }
                        }
                    }
                }
            }

            orig(self);
        }

        private void BodyCatalog_SetBodyPrefabs(On.RoR2.BodyCatalog.orig_SetBodyPrefabs orig, GameObject[] newBodyPrefabs)
        {
            //nicely done brother
            for (int i = 0; i < newBodyPrefabs.Length; i++)
            {
                if (newBodyPrefabs[i].name == "EnforcerBody" && newBodyPrefabs[i] != characterPrefab)
                {
                    newBodyPrefabs[i].name = "OldEnforcerBody";
                }
            }
            orig(newBodyPrefabs);
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);

            if (self)
            {
                if (self.HasBuff(Modules.Buffs.protectAndServeBuff))
                {
                    self.armor += 10f;
                    self.moveSpeed *= 0.35f;
                    self.maxJumpCount = 0;
                }

                if (self.HasBuff(Modules.Buffs.minigunBuff))
                {
                    self.armor += 60f;
                    self.moveSpeed *= 0.8f;
                }

                if (self.HasBuff(Modules.Buffs.energyShieldBuff))
                {
                    self.maxJumpCount = 0;
                    self.armor += 40f;
                    self.moveSpeed *= 0.65f;
                }

                if (self.HasBuff(Modules.Buffs.skateboardBuff)) {
                    self.characterMotor.airControl = 0.1f;
                }

                if (self.HasBuff(Modules.Buffs.impairedBuff))
                {
                    self.maxJumpCount = 0;
                    self.armor -= 20f;
                    self.moveSpeed *= 0.25f;
                    self.attackSpeed *= 0.75f;
                }

                if (self.HasBuff(Modules.Buffs.nemImpairedBuff))
                {
                    self.maxJumpCount = 0;
                    self.moveSpeed *= 0.25f;
                }

                if (self.HasBuff(Modules.Buffs.smallSlowBuff))
                {
                    self.armor += 10f;
                    self.moveSpeed *= 0.7f;
                }

                if (self.HasBuff(Modules.Buffs.bigSlowBuff))
                {
                    self.moveSpeed *= 0.2f;
                }

                //regen passive
                if (self.baseNameToken == "NEMFORCER_NAME" || self.baseNameToken == "NEMFORCER_BOSS_NAME")
                {
                    HealthComponent hp = self.healthComponent;
                    float regenValue = hp.fullCombinedHealth * NemforcerPlugin.passiveRegenBonus;
                    float regen = Mathf.SmoothStep(regenValue, 0, hp.combinedHealth / hp.fullCombinedHealth);

                    // reduce it while taking damage, scale it back up over time- only apply this to the normal boss and let ultra keep the bullshit regen
                    if (self.teamComponent.teamIndex == TeamIndex.Monster && self.baseNameToken == "NEMFORCER_NAME")
                    {
                        float maxRegenValue = regen;
                        float i = Mathf.Clamp(self.outOfDangerStopwatch, 0f, 5f);
                        regen = Util.Remap(i, 0f, 5f, 0f, maxRegenValue);
                    }

                    self.regen += regen;

                    if (self.teamComponent.teamIndex == TeamIndex.Monster)
                    {
                        self.regen *= 0.8f;
                        if (self.HasBuff(RoR2Content.Buffs.SuperBleed) || self.HasBuff(RoR2Content.Buffs.Bleeding)) self.regen = 0f;
                    }
                }
            }
        }

        private void CharacterMaster_OnInventoryChanged(On.RoR2.CharacterMaster.orig_OnInventoryChanged orig, CharacterMaster self)
        {
            orig(self);

            if (self.hasBody)
            {
                if (self.GetBody().baseNameToken == "ENFORCER_NAME")
                {
                    var weaponComponent = self.GetBody().GetComponent<EnforcerWeaponComponent>();
                    if (weaponComponent)
                    {
                        weaponComponent.DelayedResetWeapon();
                        weaponComponent.ModelCheck();
                    }
                }
                else
                {
                    if (self.GetBody().baseNameToken == "NEMFORCER_NAME")
                    {
                        var nemComponent = self.GetBody().GetComponent<NemforcerController>();
                        if (nemComponent)
                        {
                            nemComponent.DelayedResetWeapon();
                            nemComponent.ModelCheck();
                        }
                    }
                    else if (self.inventory && useNeedlerCrosshair.Value)
                    {
                        if (self.inventory.GetItemCount(RoR2Content.Items.LunarPrimaryReplacement) > 0)
                        {
                            self.GetBody().crosshairPrefab = needlerCrosshair;
                        }
                    }
                }
            }
        }

        private void CharacterBody_OnLevelChanged(On.RoR2.CharacterBody.orig_OnLevelUp orig, CharacterBody self)
        {
            orig(self);

            if (self.baseNameToken == "ENFORCER_NAME")
            {
                var lightController = self.GetComponent<EnforcerLightControllerAlt>();
                if (lightController)
                {
                    lightController.FlashLights(4);
                }
            }
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo info)
        {
            bool blocked = false;

            if ((info.damageType & DamageType.BarrierBlocked) != DamageType.Generic && self.body.baseNameToken == "ENFORCER_NAME")
            {
                blocked = true;
            }

            if (self.body.baseNameToken == "ENFORCER_NAME" && info.attacker)
            {
                CharacterBody cb = info.attacker.GetComponent<CharacterBody>();
                if (cb)
                {
                    //this is probably why this isn't networked
                    EnforcerComponent enforcerComponent = self.body.GetComponent<EnforcerComponent>();

                    ////ugly hack cause golems kept hitting past shield
                    //if (cb.baseNameToken == "GOLEM_BODY_NAME" && GetShieldBlock(self, info, enforcerComponent))
                    //{
                    //    blocked = self.body.HasBuff(Modules.Buffs.protectAndServeBuff);

                    //    if (enforcerComponent != null)
                    //    {
                    //        if (enforcerComponent.isDeflecting)
                    //        {
                    //            blocked = true;
                    //        }

                    //        Debug.LogWarning("firin mah layzor " + NetworkServer.active);
                    //        enforcerComponent.invokeOnLaserHitEvent();
                    //    }
                    //}

                    if (enforcerComponent)
                    {
                        enforcerComponent.AttackBlocked(blocked);
                    }
                }
            }

            if (blocked)
            {
                GameObject blockEffect = EnforcerModPlugin.blockEffectPrefab;
                if (info.procCoefficient >= 1) blockEffect = EnforcerModPlugin.heavyBlockEffectPrefab;

                EffectData effectData = new EffectData
                {
                    origin = info.position,
                    rotation = Util.QuaternionSafeLookRotation((info.force != Vector3.zero) ? info.force : UnityEngine.Random.onUnitSphere)
                };

                EffectManager.SpawnEffect(blockEffect, effectData, true);

                info.rejected = true;
            }

            if (self.body.name == "EnergyShield")
            {
                info.damage = info.procCoefficient;
            }

            orig(self, info);
        }

        private void ParryState_OnEnter(On.EntityStates.BaseState.orig_OnEnter orig, BaseState self)
        {
            orig(self);

            if (self.outer.customName == "EnforcerParry")
            {
                self.damageStat = self.outer.commonComponents.characterBody.damage * 5f;
            }
        }

        private void FireLunarNeedle_OnEnter(On.EntityStates.GlobalSkills.LunarNeedle.FireLunarNeedle.orig_OnEnter orig, EntityStates.GlobalSkills.LunarNeedle.FireLunarNeedle self)
        {
            // this actually didn't work, hopefully someone else can figure it out bc needler shotgun sounds badass
            // don't forget to register the state if you do :^)
            if (self.outer.commonComponents.characterBody)
            {
                if (self.outer.commonComponents.characterBody.baseNameToken == "ENFORCER_NAME")
                {
                    self.outer.SetNextState(new FireNeedler());
                    return;
                }
            }

            orig(self);
        }

        private void SceneDirector_Start(On.RoR2.SceneDirector.orig_Start orig, SceneDirector self)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "moon")
            {
                //null checks to hell and back
                if (GameObject.Find("EscapeSequenceController")) {
                    if (GameObject.Find("EscapeSequenceController").transform.Find("EscapeSequenceObjects")) {
                        if (GameObject.Find("EscapeSequenceController").transform.Find("EscapeSequenceObjects").transform.Find("SmoothFrog")) {
                            GameObject.Find("EscapeSequenceController").transform.Find("EscapeSequenceObjects").transform.Find("SmoothFrog").gameObject.AddComponent<EnforcerFrogComponent>();
                        }
                    }
                }
            }

            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "bazaar")
            {
                if (DifficultyIndex.Hard <= Run.instance.selectedDifficulty && Run.instance.stageClearCount >= 5)
                {
                    bool conditionsMet = false;
                    for (int i = CharacterMaster.readOnlyInstancesList.Count - 1; i >= 0; i--)
                    {
                        CharacterMaster master = CharacterMaster.readOnlyInstancesList[i];
                        if (master.teamIndex == TeamIndex.Player && master.bodyPrefab == BodyCatalog.FindBodyPrefab("EnforcerBody"))
                        {
                            var j = master.GetComponent<NemesisInvasion>();
                            if (!j) conditionsMet = true;
                            else if (!j.hasInvaded && !j.pendingInvasion) conditionsMet = true;
                        }
                    }

                    if (conditionsMet && NetworkServer.active)
                    {
                        ChatMessage.SendColored("An unusual energy emanates from below..", new Color(0.149f, 0.0039f, 0.2117f));
                    }
                }
            }
            orig(self);
        }

        private void BaseMainMenuScreen_OnEnter(On.RoR2.UI.MainMenu.BaseMainMenuScreen.orig_OnEnter orig, RoR2.UI.MainMenu.BaseMainMenuScreen self, RoR2.UI.MainMenu.MainMenuController menuController)
        {
            orig(self, menuController);

            if (UnityEngine.Random.value <= 0.1f)
            {
                GameObject hammer = Instantiate(Assets.nemesisHammer);
                hammer.transform.position = new Vector3(35, 4.5f, 21);
                hammer.transform.rotation = Quaternion.Euler(new Vector3(45, 270, 0));
                hammer.transform.localScale = new Vector3(12, 12, 340);
            }
        }


        private void CharacterSelectBarController_Start(On.RoR2.CharacterSelectBarController.orig_Start orig, CharacterSelectBarController self) {
            
            string bodyName = NemforcerPlugin.characterPrefab.GetComponent<CharacterBody>().baseNameToken;

            bool unlocked = LocalUserManager.readOnlyLocalUsersList.Any((LocalUser localUser) => localUser.userProfile.HasUnlockable(EnforcerUnlockables.nemesisUnlockableDef));

            SurvivorCatalog.FindSurvivorDefFromBody(NemforcerPlugin.characterPrefab).hidden = true;// !unlocked;

            orig(self);
        }

        private void HealthComponent_Suicide(On.RoR2.HealthComponent.orig_Suicide orig, HealthComponent self, GameObject killerOverride, GameObject inflictorOverride, DamageType damageType) {

            if (damageType == DamageType.VoidDeath) {
                //Debug.LogWarning("voidDeath");
                if (self.body.baseNameToken == "NEMFORCER_NAME" || self.body.baseNameToken == "NEMFORCER_BOSS_NAME") {
                    //Debug.LogWarning("nemmememme");
                    if (self.body.teamComponent.teamIndex != TeamIndex.Player) {
                        //Debug.LogWarning("spookyscary");
                        return;
                    }
                }
            }
            orig(self, killerOverride, inflictorOverride, damageType);
        }

        private bool GetShieldBlock(HealthComponent self, DamageInfo info, EnforcerComponent shieldComponent)
        {
            CharacterBody charB = self.GetComponent<CharacterBody>();
            Ray aimRay = shieldComponent.aimRay;
            Vector3 relativePosition = info.attacker.transform.position - aimRay.origin;
            float angle = Vector3.Angle(shieldComponent.shieldDirection, relativePosition);

            return angle < 55;
        }

        /*private void GlobalEventManager_OnEnemyHit(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo info, GameObject victim)
        {
            ShieldComponent shieldComponent = self.GetComponent<ShieldComponent>();
            if (shieldComponent && info.attacker && victim.GetComponent<CharacterBody>().HasBuff(jackBoots))
            {
                bool canBlock = GetShieldDebuffBlock(victim, info, shieldComponent);

                if (canBlock)
                {
                    //this is gross and i don't even know if it works but i'm too tired to test it rn
                    // yeah ok it literally doesn't work, ig ive up, we'll call it a feature if no one else can fix it
                    if (info.damageType.HasFlag(DamageType.IgniteOnHit) || info.damageType.HasFlag(DamageType.PercentIgniteOnHit) || info.damageType.HasFlag(DamageType.BleedOnHit) || info.damageType.HasFlag(DamageType.ClayGoo) || info.damageType.HasFlag(DamageType.Nullify) || info.damageType.HasFlag(DamageType.SlowOnHit)) info.damageType = DamageType.Generic;

                    return;
                }
            }

            orig(self, info, victim);
        }*/

        /*private bool GetShieldDebuffBlock(GameObject self, DamageInfo info, ShieldComponent shieldComponent)
        {
            CharacterBody charB = self.GetComponent<CharacterBody>();
            Ray aimRay = shieldComponent.aimRay;
            Vector3 relativePosition = info.attacker.transform.position - aimRay.origin;
            float angle = Vector3.Angle(shieldComponent.shieldDirection, relativePosition);

            return angle < ShieldBlockAngle;
        }*/

        private void CharacterBody_Update(On.RoR2.CharacterBody.orig_Update orig, CharacterBody self)
        {
            if (self.name == "EnergyShield")
            {
                return;
            }
            orig(self);
        }
        #endregion

        private static GameObject CreateBodyModel(GameObject main)
        {
            Destroy(main.transform.Find("ModelBase").gameObject);
            Destroy(main.transform.Find("CameraPivot").gameObject);
            Destroy(main.transform.Find("AimOrigin").gameObject);

            return Assets.MainAssetBundle.LoadAsset<GameObject>("mdlEnforcer");
        }

        private static GameObject CreateDisplayModel()
        {
            return Assets.MainAssetBundle.LoadAsset<GameObject>("EnforcerDisplay");
        }

        private static void CreateDisplayPrefab()
        {
            GameObject model = CreateDisplayModel();

            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            CharacterModel characterModel = model.AddComponent<CharacterModel>();
            characterModel.body = null;

            characterModel.baseRendererInfos = new CharacterModel.RendererInfo[]
            {
                new CharacterModel.RendererInfo
                {
                    defaultMaterial = Assets.CreateMaterial("matEnforcerShield", 0f, Color.black, 1f),
                    renderer = childLocator.FindChild("ShieldModel").gameObject.GetComponent<SkinnedMeshRenderer>(),
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false
                },
                new CharacterModel.RendererInfo
                {
                    //not hotpoo shader for transparency
                    defaultMaterial = childLocator.FindChild("ShieldGlassModel").gameObject.GetComponent<SkinnedMeshRenderer>().material, //Assets.CreateMaterial("matSexforcerShieldGlass", 0f, Color.black, 1f),
                    renderer = childLocator.FindChild("ShieldGlassModel").gameObject.GetComponent<SkinnedMeshRenderer>(),
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = true
                },
                new CharacterModel.RendererInfo
                {
                    defaultMaterial = Assets.CreateMaterial("matEnforcerBoard", 0f, Color.black, 1f),
                    renderer = childLocator.FindChild("SkamteBordModel").gameObject.GetComponent<SkinnedMeshRenderer>(),
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false
                },
                new CharacterModel.RendererInfo
                {
                    defaultMaterial = Assets.CreateMaterial("matEnforcerGun", 0f, Color.black, 0f),
                    renderer = childLocator.FindChild("GunModel").gameObject.GetComponent<SkinnedMeshRenderer>(),
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false
                },
                new CharacterModel.RendererInfo
                {
                    defaultMaterial = Assets.CreateMaterial("matClassicGunSuper", 0f, Color.black, 0f),
                    renderer = childLocator.FindChild("SuperGunModel").gameObject.GetComponent<SkinnedMeshRenderer>(),
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false
                },
                new CharacterModel.RendererInfo
                {
                    defaultMaterial = Assets.CreateMaterial("matClassicGunHMG", 0f, Color.black, 0f),
                    renderer = childLocator.FindChild("HMGModel").gameObject.GetComponent<SkinnedMeshRenderer>(),
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false
                },
                new CharacterModel.RendererInfo
                {
                    defaultMaterial = Assets.CreateMaterial("matEnforcerHammer", 0f, Color.black, 0f),
                    renderer = childLocator.FindChild("HammerModel").gameObject.GetComponent<SkinnedMeshRenderer>(),
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false
                },
                new CharacterModel.RendererInfo
                {
                    defaultMaterial = Assets.CreateMaterial("matEnforcer", 1f, Color.white, 0f),
                    renderer = childLocator.FindChild("PauldronModel").gameObject.GetComponent<SkinnedMeshRenderer>(),
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false
                },
                new CharacterModel.RendererInfo
                {
                    defaultMaterial = Assets.CreateMaterial("matEnforcer", 1f, Color.white, 0f),
                    renderer = childLocator.FindChild("Model").gameObject.GetComponent<SkinnedMeshRenderer>(),
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false
                }
            };

            characterModel.autoPopulateLightInfos = true;
            characterModel.invisibilityCount = 0;
            characterModel.temporaryOverlays = new List<TemporaryOverlay>();

            characterModel.mainSkinnedMeshRenderer = characterModel.baseRendererInfos[8].renderer.gameObject.GetComponent<SkinnedMeshRenderer>();

            characterDisplay = PrefabAPI.InstantiateClone(model, "EnforcerDisplay", true);

            characterDisplay.AddComponent<MenuSound>();
            characterDisplay.AddComponent<EnforcerLightController>();
            characterDisplay.AddComponent<EnforcerLightControllerAlt>();

            childLocator.FindChild("Head").transform.localScale = Vector3.one * headSize.Value;

            //i really wish this was set up in code rather than in the editor so we wouldn't have to build a new assetbundle and redo the components/events every time something on the prefab changes
            //it's seriously tedious as fuck.
            // just make it not tedious 4head
            //   turns out addlistener doesn't even fuckin work so I actually can't set it up in code even if when I wanted to try the inferior way
            //pain.
            //but yea i tried it too and gave up so understandable

            CharacterSelectSurvivorPreviewDisplayController displayController = characterDisplay.GetComponent<CharacterSelectSurvivorPreviewDisplayController>();

            if (displayController) displayController.bodyPrefab = characterPrefab;
        }

        private static void CreatePrefab()
        {
            //...what?
            // https://youtu.be/zRXl8Ow2bUs

            #region add all the things
            characterPrefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody"), "EnforcerBody");

            characterPrefab.GetComponent<NetworkIdentity>().localPlayerAuthority = true;

            GameObject model = CreateBodyModel(characterPrefab);

            GameObject modelBase = new GameObject("ModelBase");
            modelBase.transform.parent = characterPrefab.transform;
            modelBase.transform.localPosition = new Vector3(0f, -0.91f, 0f);
            modelBase.transform.localRotation = Quaternion.identity;
            modelBase.transform.localScale = Vector3.one;


            GameObject cameraPivot = new GameObject("CameraPivot");
            cameraPivot.transform.parent = modelBase.transform;
            cameraPivot.transform.localPosition = new Vector3(0f, 0f, 0f);    //1.6
            cameraPivot.transform.localRotation = Quaternion.identity;
            cameraPivot.transform.localScale = Vector3.one;

            GameObject aimOirign = new GameObject("AimOrigin");
            aimOirign.transform.parent = modelBase.transform;
            aimOirign.transform.localPosition = new Vector3(0f, 3f, 0f);    //1.8
            aimOirign.transform.localRotation = Quaternion.identity;
            aimOirign.transform.localScale = Vector3.one;

            CharacterDirection characterDirection = characterPrefab.GetComponent<CharacterDirection>();
            characterDirection.moveVector = Vector3.zero;
            characterDirection.targetTransform = modelBase.transform;
            characterDirection.overrideAnimatorForwardTransform = null;
            characterDirection.rootMotionAccumulator = null;
            characterDirection.modelAnimator = model.GetComponentInChildren<Animator>();
            characterDirection.driveFromRootRotation = false;
            characterDirection.turnSpeed = 720f;

            CharacterBody bodyComponent = characterPrefab.GetComponent<CharacterBody>();
            bodyComponent.name = "EnforcerBody";
            bodyComponent.baseNameToken = "ENFORCER_NAME";
            bodyComponent.subtitleNameToken = "ENFORCER_SUBTITLE";
            bodyComponent.bodyFlags = CharacterBody.BodyFlags.ImmuneToExecutes;
            bodyComponent.rootMotionInMainState = false;
            bodyComponent.mainRootSpeed = 0;
            bodyComponent.baseMaxHealth = baseHealth.Value;
            bodyComponent.levelMaxHealth = healthGrowth.Value;
            bodyComponent.baseRegen = baseRegen.Value;
            bodyComponent.levelRegen = regenGrowth.Value;
            bodyComponent.baseMaxShield = 0;
            bodyComponent.levelMaxShield = 0;
            bodyComponent.baseMoveSpeed = baseMovementSpeed.Value;
            bodyComponent.levelMoveSpeed = 0;
            bodyComponent.baseAcceleration = 80;
            bodyComponent.baseJumpPower = 15;
            bodyComponent.levelJumpPower = 0;
            bodyComponent.baseDamage = baseDamage.Value;
            bodyComponent.levelDamage = damageGrowth.Value;
            bodyComponent.baseAttackSpeed = 1;
            bodyComponent.levelAttackSpeed = 0;
            bodyComponent.baseCrit = baseCrit.Value;
            bodyComponent.levelCrit = 0;
            bodyComponent.baseArmor = baseArmor.Value;
            bodyComponent.levelArmor = armorGrowth.Value;
            bodyComponent.baseJumpCount = 1;
            bodyComponent.sprintingSpeedMultiplier = 1.45f;
            bodyComponent.wasLucky = false;
            bodyComponent.hideCrosshair = false;
            bodyComponent.crosshairPrefab = Resources.Load<GameObject>("Prefabs/Crosshair/SMGCrosshair");
            bodyComponent.aimOriginTransform = aimOirign.transform;
            bodyComponent.hullClassification = HullClassification.Human;
            bodyComponent.portraitIcon = Assets.charPortrait;
            bodyComponent.isChampion = false;
            bodyComponent.currentVehicle = null;
            bodyComponent.skinIndex = 0U;
            bodyComponent.bodyColor = characterColor;
            bodyComponent.spreadBloomDecayTime = 0.7f;

            Modules.States.AddSkill(typeof(EnforcerMain));

            EntityStateMachine stateMachine = bodyComponent.GetComponent<EntityStateMachine>();
            stateMachine.mainStateType = new SerializableEntityStateType(typeof(EnforcerMain));

                                                           //why
                                                           //in the god damn fuck
                                                           //does AddComponent default to an extension coming from the fucking starstorm dll
            EntityStateMachine octagonapus = bodyComponent.gameObject.AddComponent<EntityStateMachine>();

            octagonapus.customName = "EnforcerParry";
            bodyComponent.GetComponent<NetworkStateMachine>().stateMachines.Append(octagonapus);

            SerializableEntityStateType idleState = new SerializableEntityStateType(typeof(Idle));
            octagonapus.initialStateType = idleState;
            octagonapus.mainStateType = idleState;

            CharacterMotor characterMotor = characterPrefab.GetComponent<CharacterMotor>();
            characterMotor.walkSpeedPenaltyCoefficient = 1f;
            characterMotor.characterDirection = characterDirection;
            characterMotor.muteWalkMotion = false;
            characterMotor.mass = 200f;
            characterMotor.airControl = 0.25f;
            characterMotor.disableAirControlUntilCollision = false;
            characterMotor.generateParametersOnAwake = true;

            CameraTargetParams cameraTargetParams = characterPrefab.GetComponent<CameraTargetParams>();
            cameraTargetParams.cameraParams = ScriptableObject.CreateInstance<CharacterCameraParams>();
            cameraTargetParams.cameraParams.maxPitch = 70;
            cameraTargetParams.cameraParams.minPitch = -70;
            cameraTargetParams.cameraParams.wallCushion = 0.1f;
            cameraTargetParams.cameraParams.pivotVerticalOffset = 1.37f;
            cameraTargetParams.cameraParams.standardLocalCameraPos = EnforcerMain.standardCameraPosition;

            cameraTargetParams.cameraPivotTransform = null;
            cameraTargetParams.aimMode = CameraTargetParams.AimType.Standard;
            cameraTargetParams.recoil = Vector2.zero;
            cameraTargetParams.idealLocalCameraPos = Vector3.zero;
            cameraTargetParams.dontRaycastToPivot = false;

            model.transform.parent = modelBase.transform;
            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.identity;

            ModelLocator modelLocator = characterPrefab.GetComponent<ModelLocator>();
            modelLocator.modelTransform = model.transform;
            modelLocator.modelBaseTransform = modelBase.transform;

            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            //bubble shield stuff

            /*GameObject engiShieldObj = Resources.Load<GameObject>("Prefabs/Projectiles/EngiBubbleShield");

            Material shieldFillMat = UnityEngine.Object.Instantiate<Material>(engiShieldObj.transform.Find("Collision").Find("ActiveVisual").GetComponent<MeshRenderer>().material);
            childLocator.FindChild("BungusShieldFill").GetComponent<MeshRenderer>().material = shieldFillMat;

            Material shieldOuterMat = UnityEngine.Object.Instantiate<Material>(engiShieldObj.transform.Find("Collision").Find("ActiveVisual").Find("Edge").GetComponent<MeshRenderer>().material);
            childLocator.FindChild("BungusShieldOutline").GetComponent<MeshRenderer>().material = shieldOuterMat;*/

            /*Material marauderShieldFillMat = UnityEngine.Object.Instantiate<Material>(shieldFillMat);
            marauderShieldFillMat.SetTexture("_MainTex", Assets.MainAssetBundle.LoadAsset<Material>("matMarauderShield").GetTexture("_MainTex"));
            marauderShieldFillMat.SetTexture("_EmTex", Assets.MainAssetBundle.LoadAsset<Material>("matMarauderShield").GetTexture("_EmissionMap"));
            childLocator.FindChild("MarauderShieldFill").GetComponent<MeshRenderer>().material = marauderShieldFillMat;

            Material marauderShieldOuterMat = UnityEngine.Object.Instantiate<Material>(shieldOuterMat);
            marauderShieldOuterMat.SetTexture("_MainTex", Assets.MainAssetBundle.LoadAsset<Material>("matMarauderShield").GetTexture("_MainTex"));
            marauderShieldOuterMat.SetTexture("_EmTex", Assets.MainAssetBundle.LoadAsset<Material>("matMarauderShield").GetTexture("_EmissionMap"));
            childLocator.FindChild("MarauderShieldOutline").GetComponent<MeshRenderer>().material = marauderShieldOuterMat;*/

            /*var stuff1 = childLocator.FindChild("BungusShieldFill").gameObject.AddComponent<AnimateShaderAlpha>();
            var stuff2 = engiShieldObj.transform.Find("Collision").Find("ActiveVisual").GetComponent<AnimateShaderAlpha>();
            stuff1.alphaCurve = stuff2.alphaCurve;
            stuff1.decal = stuff2.decal;
            stuff1.destroyOnEnd = false;
            stuff1.disableOnEnd = false;
            stuff1.time = 0;
            stuff1.timeMax = 0.6f;*/

            //childLocator.FindChild("BungusShieldFill").gameObject.AddComponent<ObjectScaleCurve>().timeMax = 0.3f;
            CharacterModel characterModel = model.AddComponent<CharacterModel>();
            characterModel.body = bodyComponent;
            characterModel.baseRendererInfos = new CharacterModel.RendererInfo[]
            {
                new CharacterModel.RendererInfo
                {
                    defaultMaterial = Assets.CreateMaterial("matEnforcerShield", 0f, Color.black, 1f),
                    renderer = childLocator.FindChild("ShieldModel").gameObject.GetComponent<SkinnedMeshRenderer>(),
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false
                },
                new CharacterModel.RendererInfo
                {
                    //not hotpoo shader for transparency
                    defaultMaterial = childLocator.FindChild("ShieldGlassModel").gameObject.GetComponent<SkinnedMeshRenderer>().material, //Assets.CreateMaterial("matSexforcerShieldGlass", 0f, Color.black, 1f),
                    renderer = childLocator.FindChild("ShieldGlassModel").gameObject.GetComponent<SkinnedMeshRenderer>(),
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = true
                },
                new CharacterModel.RendererInfo
                {
                    defaultMaterial = Assets.CreateMaterial("matEnforcerBoard", 0f, Color.black, 1f),
                    renderer = childLocator.FindChild("SkamteBordModel").gameObject.GetComponent<SkinnedMeshRenderer>(),
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false
                },
                new CharacterModel.RendererInfo
                {
                    defaultMaterial = Assets.CreateMaterial("matEnforcerGun", 0f, Color.black, 0f),
                    renderer = childLocator.FindChild("GunModel").gameObject.GetComponent<SkinnedMeshRenderer>(),
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false
                },
                new CharacterModel.RendererInfo
                {
                    defaultMaterial = Assets.CreateMaterial("matClassicGunSuper", 0f, Color.black, 0f),
                    renderer = childLocator.FindChild("SuperGunModel").gameObject.GetComponent<SkinnedMeshRenderer>(),
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false
                },
                new CharacterModel.RendererInfo
                {
                    defaultMaterial = Assets.CreateMaterial("matClassicGunHMG", 0f, Color.black, 0f),
                    renderer = childLocator.FindChild("HMGModel").gameObject.GetComponent<SkinnedMeshRenderer>(),
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false
                },
                new CharacterModel.RendererInfo
                {
                    defaultMaterial = Assets.CreateMaterial("matEnforcerHammer", 0f, Color.black, 0f),
                    renderer = childLocator.FindChild("HammerModel").gameObject.GetComponent<SkinnedMeshRenderer>(),
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false
                },
                new CharacterModel.RendererInfo
                {
                    defaultMaterial = Assets.CreateMaterial("matEnforcer", 1f, Color.white, 0f),
                    renderer = childLocator.FindChild("PauldronModel").gameObject.GetComponent<SkinnedMeshRenderer>(),
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false
                },
                new CharacterModel.RendererInfo
                {
                    defaultMaterial = Assets.CreateMaterial("matEnforcer", 1f, Color.white, 0f),
                    renderer = childLocator.FindChild("Model").gameObject.GetComponent<SkinnedMeshRenderer>(),
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false
                }
            };
            characterModel.autoPopulateLightInfos = true;
            characterModel.invisibilityCount = 0;
            characterModel.temporaryOverlays = new List<TemporaryOverlay>();

            characterModel.mainSkinnedMeshRenderer = childLocator.FindChild("Model").gameObject.GetComponent<SkinnedMeshRenderer>();

            childLocator.FindChild("Chair").GetComponent<MeshRenderer>().material = Assets.CreateMaterial("matChair", 0f, Color.black, 0f);

            //fuck man
            childLocator.FindChild("Head").transform.localScale = Vector3.one * headSize.Value;
            
            HealthComponent healthComponent = characterPrefab.GetComponent<HealthComponent>();
            healthComponent.health = 160f;
            healthComponent.shield = 0f;
            healthComponent.barrier = 0f;
            healthComponent.magnetiCharge = 0f;
            healthComponent.body = null;
            healthComponent.dontShowHealthbar = false;
            healthComponent.globalDeathEventChanceCoefficient = 1f;

            CharacterDeathBehavior characterDeathBehavior = characterPrefab.GetComponent<CharacterDeathBehavior>();
            characterDeathBehavior.deathStateMachine = characterPrefab.GetComponent<EntityStateMachine>();
            //characterDeathBehavior.deathState = new SerializableEntityStateType(typeof(GenericCharacterDeath));

            SfxLocator sfxLocator = characterPrefab.GetComponent<SfxLocator>();
            sfxLocator.deathSound = Sounds.DeathSound;
            sfxLocator.barkSound = "";
            sfxLocator.openSound = "";
            sfxLocator.landingSound = "Play_char_land";
            sfxLocator.fallDamageSound = "Play_char_land_fall_damage";
            sfxLocator.aliveLoopStart = "";
            sfxLocator.aliveLoopStop = "";

            Rigidbody rigidbody = characterPrefab.GetComponent<Rigidbody>();
            rigidbody.mass = 200f;
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

            HurtBox mainHurtbox = model.transform.Find("MainHurtbox").GetComponent<CapsuleCollider>().gameObject.AddComponent<HurtBox>();
            mainHurtbox.gameObject.layer = LayerIndex.entityPrecise.intVal;
            mainHurtbox.healthComponent = healthComponent;
            mainHurtbox.isBullseye = true;
            mainHurtbox.damageModifier = HurtBox.DamageModifier.Normal;
            mainHurtbox.hurtBoxGroup = hurtBoxGroup;
            mainHurtbox.indexInGroup = 0;

            //make a hurtbox for the shield since this works apparently !
            HurtBox shieldHurtbox = childLocator.FindChild("ShieldHurtbox").gameObject.AddComponent<HurtBox>();
            shieldHurtbox.gameObject.layer = LayerIndex.entityPrecise.intVal;
            shieldHurtbox.healthComponent = healthComponent;
            shieldHurtbox.isBullseye = false;
            shieldHurtbox.damageModifier = HurtBox.DamageModifier.Barrier;
            shieldHurtbox.hurtBoxGroup = hurtBoxGroup;
            shieldHurtbox.indexInGroup = 1;
            shieldHurtbox.gameObject.SetActive(false);

            hurtBoxGroup.hurtBoxes = new HurtBox[]
            {
                mainHurtbox,
                shieldHurtbox
            };

            hurtBoxGroup.mainHurtBox = mainHurtbox;
            hurtBoxGroup.bullseyeCount = 1;

            //make a hitbox for shoulder bash
            HitBoxGroup hitBoxGroup = model.AddComponent<HitBoxGroup>();

            GameObject chargeHitbox = childLocator.FindChild("ChargeHitbox").gameObject;
            HitBox hitBox = chargeHitbox.AddComponent<HitBox>();
            chargeHitbox.layer = LayerIndex.projectile.intVal;

            hitBoxGroup.hitBoxes = new HitBox[]
            {
                hitBox
            };

            hitBoxGroup.groupName = "Charge";

            //hammer hitbox
            HitBoxGroup hammerHitBoxGroup = model.AddComponent<HitBoxGroup>();

            GameObject hammerHitbox = childLocator.FindChild("ActualHammerHitbox").gameObject;

            HitBox hammerHitBox = hammerHitbox.AddComponent<HitBox>();
            hammerHitbox.layer = LayerIndex.projectile.intVal;

            hammerHitBoxGroup.hitBoxes = new HitBox[] 
            {
                hammerHitBox
            };

            hammerHitBoxGroup.groupName = "Hammer";

            //hammer hitbox2
            HitBoxGroup hammerHitBoxGroup2 = model.AddComponent<HitBoxGroup>();

            GameObject hammerHitbox2 = childLocator.FindChild("HammerHitboxBig").gameObject;
            
            HitBox hammerHitBox2 = hammerHitbox2.AddComponent<HitBox>();
            hammerHitbox2.layer = LayerIndex.projectile.intVal;

            hammerHitBoxGroup2.hitBoxes = new HitBox[]
            {
                hammerHitBox2
            };

            hammerHitBoxGroup2.groupName = "HammerBig";

            FootstepHandler footstepHandler = model.AddComponent<FootstepHandler>();
            footstepHandler.baseFootstepString = "Play_player_footstep";
            footstepHandler.sprintFootstepOverrideString = "";
            footstepHandler.enableFootstepDust = true;
            footstepHandler.footstepDustPrefab = Resources.Load<GameObject>("Prefabs/GenericFootstepDust");

            RagdollController ragdollController = model.GetComponent<RagdollController>();

            PhysicMaterial physicMat = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<RagdollController>().bones[1].GetComponent<Collider>().material;

            foreach (Transform i in ragdollController.bones)
            {
                if (i)
                {
                    i.gameObject.layer = LayerIndex.ragdoll.intVal;

                    Collider j = i.GetComponent<Collider>();
                    if (j)
                    {
                        j.material = physicMat;
                        j.sharedMaterial = physicMat;
                    }
                }
            }

            AimAnimator aimAnimator = model.AddComponent<AimAnimator>();
            aimAnimator.directionComponent = characterDirection;
            aimAnimator.pitchRangeMax = 60f;
            aimAnimator.pitchRangeMin = -60f;
            aimAnimator.yawRangeMin = -90f;
            aimAnimator.yawRangeMax = 90f;
            aimAnimator.pitchGiveupRange = 30f;
            aimAnimator.yawGiveupRange = 10f;
            aimAnimator.giveupDuration = 3f;
            aimAnimator.inputBank = characterPrefab.GetComponent<InputBankTest>();

            characterPrefab.AddComponent<EnforcerComponent>();
            characterPrefab.AddComponent<EnforcerWeaponComponent>();
            characterPrefab.AddComponent<EnforcerLightController>();
            characterPrefab.AddComponent<EnforcerLightControllerAlt>();

            #endregion
        }

        private void RegisterCharacter()
        {
            string desc = "The Enforcer is a defensive juggernaut who can take and give a beating.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Riot Shotgun can pierce through many enemies at once." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Batting away enemies with Shield Bash guarantees you will keep enemies at a safe range." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Use Tear Gas to weaken large crowds of enemies, then get in close and crush them." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > When you can, use Protect and Serve against walls to prevent enemies from flanking you." + Environment.NewLine + Environment.NewLine;

            string outro = characterOutro;
            if (forceUnlock.Value) outro = "..and so he left, having cheated not only the game, but himself. He didn't grow. He didn't improve. He took a shortcut and gained nothing. He experienced a hollow victory. Nothing was risked and nothing was rained.";

            LanguageAPI.Add("ENFORCER_NAME", characterName);
            LanguageAPI.Add("ENFORCER_DESCRIPTION", desc);
            LanguageAPI.Add("ENFORCER_SUBTITLE", characterSubtitle);
            //LanguageAPI.Add("ENFORCER_LORE", "I'M FUCKING INVINCIBLE");
            LanguageAPI.Add("ENFORCER_LORE", characterLore);
            LanguageAPI.Add("ENFORCER_OUTRO_FLAVOR", outro);
            LanguageAPI.Add("ENFORCER_OUTRO_FAILURE", characterOutroFailure);

            characterDisplay.AddComponent<NetworkIdentity>();

            Modules.Survivors.RegisterNewSurvivor(characterPrefab,
                                                  characterDisplay, 
                                                  "ENFORCER",
                                                  EnforcerUnlockables.enforcerUnlockableDef, 
                                                  4.005f);

            SkillSetup();
            
            bodyPrefabs.Add(characterPrefab);
        }

        private void RegisterProjectile()
        {
            //i'm the treasure, baby, i'm the prize
            
            stunGrenade = Resources.Load<GameObject>("Prefabs/Projectiles/CommandoGrenadeProjectile").InstantiateClone("EnforcerStunGrenade", true);

            ProjectileController stunGrenadeController = stunGrenade.GetComponent<ProjectileController>();
            ProjectileImpactExplosion stunGrenadeImpact = stunGrenade.GetComponent<ProjectileImpactExplosion>();

            GameObject stunGrenadeModel = Assets.stunGrenadeModel.InstantiateClone("StunGrenadeGhost", true);
            stunGrenadeModel.AddComponent<NetworkIdentity>();
            stunGrenadeModel.AddComponent<ProjectileGhostController>();

            stunGrenadeController.ghostPrefab = stunGrenadeModel;

            stunGrenadeImpact.lifetimeExpiredSoundString = "";
            stunGrenadeImpact.explosionSoundString = Sounds.StunExplosion;
            stunGrenadeImpact.offsetForLifetimeExpiredSound = 1;
            stunGrenadeImpact.destroyOnEnemy = false;
            stunGrenadeImpact.destroyOnWorld = false;
            stunGrenadeImpact.timerAfterImpact = true;
            stunGrenadeImpact.falloffModel = BlastAttack.FalloffModel.None;
            stunGrenadeImpact.lifetimeAfterImpact = 0f;
            stunGrenadeImpact.lifetimeRandomOffset = 0;
            stunGrenadeImpact.blastRadius = 8;
            stunGrenadeImpact.blastDamageCoefficient = 1;
            stunGrenadeImpact.blastProcCoefficient = 1f;
            stunGrenadeImpact.fireChildren = false;
            stunGrenadeImpact.childrenCount = 0;
            stunGrenadeImpact.bonusBlastForce = -2000f * Vector3.up;
            stunGrenadeController.procCoefficient = 1;

            shockGrenade = Resources.Load<GameObject>("Prefabs/Projectiles/CommandoGrenadeProjectile").InstantiateClone("EnforcerShockGrenade", true);

            ProjectileController shockGrenadeController = shockGrenade.GetComponent<ProjectileController>();
            ProjectileImpactExplosion shockGrenadeImpact = shockGrenade.GetComponent<ProjectileImpactExplosion>();

            GameObject shockGrenadeModel = Assets.stunGrenadeModelAlt.InstantiateClone("ShockGrenadeGhost", true);
            shockGrenadeModel.AddComponent<NetworkIdentity>();
            shockGrenadeModel.AddComponent<ProjectileGhostController>();

            shockGrenadeController.ghostPrefab = shockGrenadeModel;

            shockGrenadeImpact.lifetimeExpiredSoundString = "";
            shockGrenadeImpact.explosionSoundString = "Play_mage_m2_impact";
            shockGrenadeImpact.offsetForLifetimeExpiredSound = 1;
            shockGrenadeImpact.destroyOnEnemy = false;
            shockGrenadeImpact.destroyOnWorld = false;                          
            shockGrenadeImpact.timerAfterImpact = true;
            shockGrenadeImpact.falloffModel = BlastAttack.FalloffModel.None;
            shockGrenadeImpact.lifetimeAfterImpact = 0f;
            shockGrenadeImpact.lifetimeRandomOffset = 0;
            shockGrenadeImpact.blastRadius = 14f;
            shockGrenadeImpact.blastDamageCoefficient = 1;
            shockGrenadeImpact.blastProcCoefficient = 1f;
            shockGrenadeImpact.fireChildren = false;
            shockGrenadeImpact.childrenCount = 0;
            shockGrenadeImpact.bonusBlastForce = -2000f * Vector3.up;
            shockGrenadeImpact.impactEffect = CreateShockGrenadeEffect();
            shockGrenadeController.procCoefficient = 1;

            tearGasProjectilePrefab = Resources.Load<GameObject>("Prefabs/Projectiles/CommandoGrenadeProjectile").InstantiateClone("EnforcerTearGasGrenade", true);
            tearGasPrefab = Resources.Load<GameObject>("Prefabs/Projectiles/SporeGrenadeProjectileDotZone").InstantiateClone("TearGasDotZone", true);

            ProjectileController grenadeController = tearGasProjectilePrefab.GetComponent<ProjectileController>();
            ProjectileController tearGasController = tearGasPrefab.GetComponent<ProjectileController>();

            ProjectileDamage grenadeDamage = tearGasProjectilePrefab.GetComponent<ProjectileDamage>();
            ProjectileDamage tearGasDamage = tearGasPrefab.GetComponent<ProjectileDamage>();

            ProjectileSimple simple = tearGasProjectilePrefab.GetComponent<ProjectileSimple>();

            TeamFilter filter = tearGasPrefab.GetComponent<TeamFilter>();

            ProjectileImpactExplosion grenadeImpact = tearGasProjectilePrefab.GetComponent<ProjectileImpactExplosion>();

            Destroy(tearGasPrefab.GetComponent<ProjectileDotZone>());

            BuffWard buffWard = tearGasPrefab.AddComponent<BuffWard>();

            filter.teamIndex = TeamIndex.Player;

            GameObject grenadeModel = Assets.tearGasGrenadeModel.InstantiateClone("TearGasGhost", true);
            grenadeModel.AddComponent<NetworkIdentity>();
            grenadeModel.AddComponent<ProjectileGhostController>();

            grenadeController.ghostPrefab = grenadeModel;
            //tearGasController.ghostPrefab = Assets.tearGasEffectPrefab;

            grenadeImpact.lifetimeExpiredSoundString = "";
            grenadeImpact.explosionSoundString = Sounds.GasExplosion;
            grenadeImpact.offsetForLifetimeExpiredSound = 1;
            grenadeImpact.destroyOnEnemy = false;
            grenadeImpact.destroyOnWorld = false;
            grenadeImpact.timerAfterImpact = true;
            grenadeImpact.falloffModel = BlastAttack.FalloffModel.SweetSpot;
            grenadeImpact.lifetime = 18;
            grenadeImpact.lifetimeAfterImpact = 0.5f;
            grenadeImpact.lifetimeRandomOffset = 0;
            grenadeImpact.blastRadius = 6;
            grenadeImpact.blastDamageCoefficient = 1;
            grenadeImpact.blastProcCoefficient = 1;
            grenadeImpact.fireChildren = true;
            grenadeImpact.childrenCount = 1;
            grenadeImpact.childrenProjectilePrefab = tearGasPrefab;
            grenadeImpact.childrenDamageCoefficient = 0;
            grenadeImpact.impactEffect = null;

            grenadeController.startSound = "";
            grenadeController.procCoefficient = 1;
            tearGasController.procCoefficient = 0;

            grenadeDamage.crit = false;
            grenadeDamage.damage = 0f;
            grenadeDamage.damageColorIndex = DamageColorIndex.Default;
            grenadeDamage.damageType = DamageType.Stun1s;
            grenadeDamage.force = 0;

            tearGasDamage.crit = false;
            tearGasDamage.damage = 0;
            tearGasDamage.damageColorIndex = DamageColorIndex.WeakPoint;
            tearGasDamage.damageType = DamageType.Stun1s;
            tearGasDamage.force = -1000;

            buffWard.radius = 18;
            buffWard.interval = 1;
            buffWard.rangeIndicator = null;
            buffWard.buffDef = Modules.Buffs.impairedBuff;
            buffWard.buffDuration = 1.5f;
            buffWard.floorWard = false;
            buffWard.expires = false;
            buffWard.invertTeamFilter = true;
            buffWard.expireDuration = 0;
            buffWard.animateRadius = false;

            //this is weird but it works

            Destroy(tearGasPrefab.transform.GetChild(0).gameObject);
            GameObject gasFX = Assets.tearGasEffectPrefab.InstantiateClone("FX", false);
            gasFX.AddComponent<TearGasComponent>();
            gasFX.AddComponent<DestroyOnTimer>().duration = 18f;
            gasFX.transform.parent = tearGasPrefab.transform;
            gasFX.transform.localPosition = Vector3.zero;

            //i have this really big cut on my shin and it's bleeding but i'm gonna code instead of doing something about it
            // that's the spirit, champ

            tearGasPrefab.AddComponent<DestroyOnTimer>().duration = 18;

            //scepter stuff.........
            //damageGasProjectile = PrefabAPI.InstantiateClone(projectilePrefab, "DamageGasGrenade", true);
            damageGasProjectile = Resources.Load<GameObject>("Prefabs/Projectiles/CommandoGrenadeProjectile").InstantiateClone("EnforcerTearGasScepterGrenade", true);
            damageGasEffect = Resources.Load<GameObject>("Prefabs/Projectiles/SporeGrenadeProjectileDotZone").InstantiateClone("TearGasScepterDotZone", true);

            ProjectileController scepterGrenadeController = damageGasProjectile.GetComponent<ProjectileController>();
            ProjectileController scepterTearGasController = damageGasEffect.GetComponent<ProjectileController>();
            ProjectileDamage scepterGrenadeDamage = damageGasProjectile.GetComponent<ProjectileDamage>();
            ProjectileDamage scepterTearGasDamage = damageGasEffect.GetComponent<ProjectileDamage>();
            ProjectileImpactExplosion scepterGrenadeImpact = damageGasProjectile.GetComponent<ProjectileImpactExplosion>();
            ProjectileDotZone dotZone = damageGasEffect.GetComponent<ProjectileDotZone>();

            dotZone.damageCoefficient = 2f;
            dotZone.fireFrequency = 4f;
            dotZone.forceVector = Vector3.zero;
            dotZone.impactEffect = null;
            dotZone.lifetime = 18f;
            dotZone.overlapProcCoefficient = 0.05f;
            dotZone.transform.localScale = Vector3.one * 28;

            HitBoxGroup gasHitboxGroup = dotZone.GetComponent<HitBoxGroup>();
            gasHitboxGroup.hitBoxes = new HitBox[] { gasHitboxGroup.gameObject.AddComponent<HitBox>() };

            GameObject scepterGrenadeModel = Assets.tearGasGrenadeModelAlt.InstantiateClone("TearGasScepterGhost", true);
            scepterGrenadeModel.AddComponent<NetworkIdentity>();
            scepterGrenadeModel.AddComponent<ProjectileGhostController>();

            scepterGrenadeController.ghostPrefab = scepterGrenadeModel;
            //tearGasController.ghostPrefab = Assets.tearGasEffectPrefab;

            scepterGrenadeImpact.lifetimeExpiredSoundString = "";
            scepterGrenadeImpact.explosionSoundString = Sounds.GasExplosion;
            scepterGrenadeImpact.offsetForLifetimeExpiredSound = 1;
            scepterGrenadeImpact.destroyOnEnemy = false;
            scepterGrenadeImpact.destroyOnWorld = false;
            scepterGrenadeImpact.timerAfterImpact = true;
            scepterGrenadeImpact.falloffModel = BlastAttack.FalloffModel.SweetSpot;
            scepterGrenadeImpact.lifetime = 18;
            scepterGrenadeImpact.lifetimeAfterImpact = 0.5f;
            scepterGrenadeImpact.lifetimeRandomOffset = 0;
            scepterGrenadeImpact.blastRadius = 6;
            scepterGrenadeImpact.blastDamageCoefficient = 1;
            scepterGrenadeImpact.blastProcCoefficient = 1;
            scepterGrenadeImpact.fireChildren = true;
            scepterGrenadeImpact.childrenCount = 1;
            scepterGrenadeImpact.childrenProjectilePrefab = damageGasEffect;
            scepterGrenadeImpact.childrenDamageCoefficient = 0.5f;
            scepterGrenadeImpact.impactEffect = null;

            scepterGrenadeController.startSound = "";
            scepterGrenadeController.procCoefficient = 1;
            scepterTearGasController.procCoefficient = 0;

            scepterGrenadeDamage.crit = false;
            scepterGrenadeDamage.damage = 0f;
            scepterGrenadeDamage.damageColorIndex = DamageColorIndex.Default;
            scepterGrenadeDamage.damageType = DamageType.Stun1s;
            scepterGrenadeDamage.force = 0;

            scepterTearGasDamage.crit = false;
            scepterTearGasDamage.damage = 1f;
            scepterTearGasDamage.damageColorIndex = DamageColorIndex.WeakPoint;
            scepterTearGasDamage.damageType = DamageType.Generic;
            scepterTearGasDamage.force = -10;

            Destroy(damageGasEffect.transform.GetChild(0).gameObject);
            GameObject scepterGasFX = Assets.tearGasEffectPrefabAlt.InstantiateClone("FX", false);
            scepterGasFX.AddComponent<TearGasComponent>();
            scepterGasFX.AddComponent<DestroyOnTimer>().duration = 18f;
            scepterGasFX.transform.parent = damageGasEffect.transform;
            scepterGasFX.transform.localPosition = Vector3.zero;

            damageGasEffect.AddComponent<DestroyOnTimer>().duration = 18;

            BuffWard buffWard2 = damageGasEffect.AddComponent<BuffWard>();

            buffWard2.radius = 18;
            buffWard2.interval = 1;
            buffWard2.rangeIndicator = null;
            buffWard2.buffDef = Modules.Buffs.impairedBuff;
            buffWard2.buffDuration = 1.5f;
            buffWard2.floorWard = false;
            buffWard2.expires = false;
            buffWard2.invertTeamFilter = true;
            buffWard2.expireDuration = 0;
            buffWard2.animateRadius = false;

            //bullet tracers
            bulletTracer = Resources.Load<GameObject>("Prefabs/Effects/Tracers/TracerCommandoShotgun").InstantiateClone("EnforcerBulletTracer", true);

            if (!bulletTracer.GetComponent<EffectComponent>()) bulletTracer.AddComponent<EffectComponent>();
            if (!bulletTracer.GetComponent<VFXAttributes>()) bulletTracer.AddComponent<VFXAttributes>();
            if (!bulletTracer.GetComponent<NetworkIdentity>()) bulletTracer.AddComponent<NetworkIdentity>();

            Material bulletMat = null;

            foreach (LineRenderer i in bulletTracer.GetComponentsInChildren<LineRenderer>())
            {
                if (i)
                {
                    bulletMat = UnityEngine.Object.Instantiate<Material>(i.material);
                    bulletMat.SetColor("_TintColor", new Color(0.68f, 0.58f, 0.05f));
                    i.material = bulletMat;
                    i.startColor = new Color(0.68f, 0.58f, 0.05f);
                    i.endColor = new Color(0.68f, 0.58f, 0.05f);
                }
            }

            bulletTracerSSG = Resources.Load<GameObject>("Prefabs/Effects/Tracers/TracerCommandoShotgun").InstantiateClone("EnforcerBulletTracer", true);

            if (!bulletTracerSSG.GetComponent<EffectComponent>()) bulletTracerSSG.AddComponent<EffectComponent>();
            if (!bulletTracerSSG.GetComponent<VFXAttributes>()) bulletTracerSSG.AddComponent<VFXAttributes>();
            if (!bulletTracerSSG.GetComponent<NetworkIdentity>()) bulletTracerSSG.AddComponent<NetworkIdentity>();

            foreach (LineRenderer i in bulletTracerSSG.GetComponentsInChildren<LineRenderer>())
            {
                if (i)
                {
                    Material material = UnityEngine.Object.Instantiate<Material>(i.material);
                    material.SetColor("_TintColor", Color.yellow);
                    i.material = material;
                    i.startColor = new Color(0.8f, 0.24f, 0f);
                    i.endColor = new Color(0.8f, 0.24f, 0f);
                }
            }

            laserTracer = Resources.Load<GameObject>("Prefabs/Effects/Tracers/TracerCommandoShotgun").InstantiateClone("EnforcerLaserTracer", true);

            if (!laserTracer.GetComponent<EffectComponent>()) laserTracer.AddComponent<EffectComponent>();
            if (!laserTracer.GetComponent<VFXAttributes>()) laserTracer.AddComponent<VFXAttributes>();
            if (!laserTracer.GetComponent<NetworkIdentity>()) laserTracer.AddComponent<NetworkIdentity>();

            foreach (LineRenderer i in laserTracer.GetComponentsInChildren<LineRenderer>())
            {
                if (i)
                {
                    Material material = UnityEngine.Object.Instantiate<Material>(i.material);
                    material.SetColor("_TintColor", Color.red);
                    i.material = material;
                    i.startColor = new Color(0.8f, 0.19f, 0.19f);
                    i.endColor = new Color(0.8f, 0.19f, 0.19f);
                }
            }

            minigunTracer = Resources.Load<GameObject>("Prefabs/Effects/Tracers/TracerClayBruiserMinigun").InstantiateClone("NemforcerMinigunTracer", true);

            var line = minigunTracer.GetComponent<LineRenderer>();
            line.material = bulletMat;
            line.startColor = new Color(0.68f, 0.58f, 0.05f);
            line.endColor = new Color(0.68f, 0.58f, 0.05f);
            line.startWidth = 0.2f;
            line.endWidth = 0.2f;

            if (!minigunTracer.GetComponent<EffectComponent>()) minigunTracer.AddComponent<EffectComponent>();
            if (!minigunTracer.GetComponent<VFXAttributes>()) minigunTracer.AddComponent<VFXAttributes>();
            if (!minigunTracer.GetComponent<NetworkIdentity>()) minigunTracer.AddComponent<NetworkIdentity>();

            //block effect
            blockEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/BearProc").InstantiateClone("EnforcerBlockEffect", true);

            blockEffectPrefab.GetComponent<EffectComponent>().soundName = Sounds.ShieldBlockLight;
            if (!blockEffectPrefab.GetComponent<NetworkIdentity>()) blockEffectPrefab.AddComponent<NetworkIdentity>();

            //heavy block effect
            heavyBlockEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/BearProc").InstantiateClone("EnforcerHeavyBlockEffect", true);

            heavyBlockEffectPrefab.GetComponent<EffectComponent>().soundName = Sounds.ShieldBlockHeavy;
            if (!heavyBlockEffectPrefab.GetComponent<NetworkIdentity>()) heavyBlockEffectPrefab.AddComponent<NetworkIdentity>();

            //hammer slam effect for enforcer m1 and nemforcer m2
            hammerSlamEffect = Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/ParentSlamEffect").InstantiateClone("EnforcerHammerSlamEffect");
            hammerSlamEffect.GetComponent<EffectComponent>().applyScale = true;

            Transform dust = hammerSlamEffect.transform.Find("Dust, Directional");
            if(dust) dust.localScale = new Vector3(1, 0.7f, 1);

            Transform nova = hammerSlamEffect.transform.Find("Nova Sphere");
            if(nova) nova.localScale = new Vector3(8, 8, 8);

            if (!hammerSlamEffect.GetComponent<NetworkIdentity>()) hammerSlamEffect.AddComponent<NetworkIdentity>();

            projectilePrefabs.Add(tearGasProjectilePrefab);
            projectilePrefabs.Add(damageGasProjectile);
            projectilePrefabs.Add(tearGasPrefab);
            projectilePrefabs.Add(damageGasEffect);
            projectilePrefabs.Add(stunGrenade);
            projectilePrefabs.Add(shockGrenade);

            Modules.Effects.AddEffect(bulletTracer);
            Modules.Effects.AddEffect(bulletTracerSSG);
            Modules.Effects.AddEffect(laserTracer);
            Modules.Effects.AddEffect(minigunTracer);
            Modules.Effects.AddEffect(blockEffectPrefab, Sounds.ShieldBlockLight);
            Modules.Effects.AddEffect(heavyBlockEffectPrefab, Sounds.ShieldBlockHeavy);
            Modules.Effects.AddEffect(hammerSlamEffect);
        }

        private GameObject CreateShockGrenadeEffect()
        {
            GameObject effect = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/effects/lightningstakenova"), "EnforcerShockGrenadeExplosionEffect", false);
            EffectComponent ec = effect.GetComponent<EffectComponent>();
            ec.applyScale = true;
            ec.soundName = "Play_item_use_lighningArm"; //This typo is in the game.
            Modules.Effects.effectDefs.Add(new EffectDef(effect));

            return effect;
        }

        private void CreateCrosshair()
        {
            needlerCrosshair = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Crosshair/LoaderCrosshair"), "NeedlerCrosshair", true);
            needlerCrosshair.AddComponent<NetworkIdentity>();
            Destroy(needlerCrosshair.GetComponent<LoaderHookCrosshairController>());

            needlerCrosshair.GetComponent<RawImage>().enabled = false;

            var control = needlerCrosshair.GetComponent<CrosshairController>();

            control.maxSpreadAlpha = 0;
            control.maxSpreadAngle = 3;
            control.minSpreadAlpha = 0;
            control.spriteSpreadPositions = new CrosshairController.SpritePosition[]
            {
                new CrosshairController.SpritePosition
                {
                    target = needlerCrosshair.transform.GetChild(2).GetComponent<RectTransform>(),
                    zeroPosition = new Vector3(-20f, 0, 0),
                    onePosition = new Vector3(-48f, 0, 0)
                },
                new CrosshairController.SpritePosition
                {
                    target = needlerCrosshair.transform.GetChild(3).GetComponent<RectTransform>(),
                    zeroPosition = new Vector3(20f, 0, 0),
                    onePosition = new Vector3(48f, 0, 0)
                }
            };

            Destroy(needlerCrosshair.transform.GetChild(0).gameObject);
            Destroy(needlerCrosshair.transform.GetChild(1).gameObject);
        }

        private void CreateDoppelganger()
        {
            doppelganger = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/CharacterMasters/MercMonsterMaster"), "EnforcerMonsterMaster");

            foreach (AISkillDriver ai in doppelganger.GetComponentsInChildren<AISkillDriver>())
            {
                BaseUnityPlugin.DestroyImmediate(ai);
            }

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
            exitShieldDriver.requiredSkill = shieldUpDef;

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
            shoulderBashDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;
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
            swapToMinigunDriver.requiredSkill = shieldDownDef;

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

            masterPrefabs.Add(doppelganger);

            CharacterMaster master = doppelganger.GetComponent<CharacterMaster>();
            master.bodyPrefab = characterPrefab;
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

            _skillLocator = characterPrefab.GetComponent<SkillLocator>();
            _previewController = characterDisplay.GetComponent<CharacterSelectSurvivorPreviewDisplayController>();

            PrimarySetup();
            SecondarySetup();
            UtilitySetup();
            SpecialSetup();

            CSSPreviewSetup();
        }

        #region skillSetups

        private void PrimarySetup()
        {

            SkillDef primaryDef1 = PrimarySkillDef_RiotShotgun();
            Modules.Skills.RegisterSkillDef(primaryDef1, typeof(RiotShotgun));

            SkillDef primaryDef2 = PrimarySkillDef_SuperShotgun();
            Modules.Skills.RegisterSkillDef(primaryDef2, typeof(SuperShotgun2));

            SkillDef primaryDef3 = PrimarySkillDef_AssaultRifle();
            Modules.Skills.RegisterSkillDef(primaryDef3, typeof(FireMachineGun));

            SkillDef primaryDef4 = PrimarySkillDef_Hammer();
            Modules.Skills.RegisterSkillDef(primaryDef4, typeof(HammerSwing));

            SkillFamily.Variant primaryVariant1 = Modules.Skills.SetupSkillVariant(primaryDef1);
            SkillFamily.Variant primaryVariant2 = Modules.Skills.SetupSkillVariant(primaryDef2, EnforcerUnlockables.enforcerDoomUnlockableDef);
            SkillFamily.Variant primaryVariant3 = Modules.Skills.SetupSkillVariant(primaryDef3, EnforcerUnlockables.enforcerARUnlockableDef);
            SkillFamily.Variant primaryVariant4 = Modules.Skills.SetupSkillVariant(primaryDef4);

            _skillLocator.primary = Modules.Skills.RegisterSkillsToFamily(characterPrefab, "EnforcerPrimary", primaryVariant1, primaryVariant2, primaryVariant3);

            _previewController.skillChangeResponses[0].triggerSkillFamily = _skillLocator.primary.skillFamily;
            _previewController.skillChangeResponses[0].triggerSkill = primaryDef1;
            _previewController.skillChangeResponses[1].triggerSkillFamily = _skillLocator.primary.skillFamily;
            _previewController.skillChangeResponses[1].triggerSkill = primaryDef2;
            _previewController.skillChangeResponses[2].triggerSkillFamily = _skillLocator.primary.skillFamily;
            _previewController.skillChangeResponses[2].triggerSkill = primaryDef3;

            //cursed

            if (cursed.Value)
            {
                Modules.Skills.RegisterAdditionalSkills(_skillLocator.primary, primaryVariant4);

                _previewController.skillChangeResponses[3].triggerSkillFamily = _skillLocator.primary.skillFamily;
                _previewController.skillChangeResponses[3].triggerSkill = primaryDef4;
            }
        }

        private void SecondarySetup() {

            SkillDef secondaryDef1 = SecondarySkillDef_Bash();
            Modules.Skills.RegisterSkillDef(secondaryDef1, typeof(ShieldBash), typeof(ShoulderBash), typeof(ShoulderBashImpact));

            SkillFamily.Variant secondaryVariant1 = Modules.Skills.SetupSkillVariant(secondaryDef1);

            _skillLocator.secondary = Modules.Skills.RegisterSkillsToFamily(characterPrefab, "EnforcerSecondary", secondaryVariant1);
        }

        private void UtilitySetup()
        {
            SkillDef utilityDef1 = UtilitySkillDef_TearGas();
            Modules.Skills.RegisterSkillDef(utilityDef1, typeof(AimTearGas), typeof(TearGas));

            SkillDef utilityDef2 = UtilitySkillDef_StunGrenade();
            Modules.Skills.RegisterSkillDef(utilityDef2, typeof(StunGrenade));

            SkillFamily.Variant utilityVariant1 = Modules.Skills.SetupSkillVariant(utilityDef1);
            SkillFamily.Variant utilityVariant2 = Modules.Skills.SetupSkillVariant(utilityDef2, null);

            _skillLocator.utility = Modules.Skills.RegisterSkillsToFamily(characterPrefab, "EnforcerUtility", utilityVariant1, utilityVariant2);
        }

        private void SpecialSetup()
        {
            //shield
            SkillDef specialDef1 = SpecialSkillDef_ProtectAndServe();
            Modules.Skills.RegisterSkillDef(specialDef1, typeof(ProtectAndServe));
            SkillDef specialDef1Down = SpecialSkillDef_ShieldDown();
            Modules.Skills.RegisterSkillDef(specialDef1Down);

            shieldDownDef = specialDef1;
            shieldUpDef = specialDef1Down;

            //cursed
            //energy shield (lol)
            SkillDef specialDef2 = SpecialSkillDef_EnergyShield();
            Modules.Skills.RegisterSkillDef(specialDef2, typeof(EnergyShield));
            SkillDef specialDef2Down = SpecialSkillDef_EnergyShieldDown();
            Modules.Skills.RegisterSkillDef(specialDef2Down);

            shieldOffDef = specialDef2;
            shieldOnDef = specialDef2Down;

            //skateboard
            SkillDef specialDef3 = SpecialSkillDef_SkamteBord();
            Modules.Skills.RegisterSkillDef(specialDef3, typeof(Skateboard));
            SkillDef specialDef3Down = SpecialSkillDef_SkamteBordDown();
            Modules.Skills.RegisterSkillDef(specialDef3Down);

            boardDownDef = specialDef3;
            boardUpDef = specialDef3Down;

            //setup
            SkillFamily.Variant specialVariant1 = Modules.Skills.SetupSkillVariant(specialDef1);
            SkillFamily.Variant specialVariant2 = Modules.Skills.SetupSkillVariant(specialDef2);
            SkillFamily.Variant specialVariant3 = Modules.Skills.SetupSkillVariant(specialDef3);

            _skillLocator.special = Modules.Skills.RegisterSkillsToFamily(characterPrefab, "EnforcerSpecial", specialVariant1);

            _previewController.skillChangeResponses[4].triggerSkillFamily = _skillLocator.special.skillFamily;
            _previewController.skillChangeResponses[4].triggerSkill = specialDef1;

            if (cursed.Value)
            {

                Modules.Skills.RegisterAdditionalSkills(_skillLocator.special, specialVariant3);

                _previewController.skillChangeResponses[5].triggerSkillFamily = _skillLocator.special.skillFamily;
                _previewController.skillChangeResponses[5].triggerSkill = specialDef3;

                ////rip energy shield lol
                ////previewController.skillChangeResponses[6].triggerSkillFamily = skillLocator.special.skillFamily;
                ////previewController.skillChangeResponses[6].triggerSkill = specialDef2;
            }
        }

        #region skilldefs
        private SkillDef PrimarySkillDef_RiotShotgun()
        {
            string desc = "Fire a short-range blast that <style=cIsUtility>pierces</style> for <style=cIsDamage>" + shotgunBulletCount.Value + "x" + 100f * shotgunDamage.Value + "% damage.";

            LanguageAPI.Add("ENFORCER_PRIMARY_SHOTGUN_NAME", "Riot Shotgun");
            LanguageAPI.Add("ENFORCER_PRIMARY_SHOTGUN_DESCRIPTION", desc);

            SkillDef skillDefRiotShotgun = ScriptableObject.CreateInstance<SkillDef>();
            skillDefRiotShotgun.activationState = new SerializableEntityStateType(typeof(RiotShotgun));
            skillDefRiotShotgun.activationStateMachineName = "Weapon";
            skillDefRiotShotgun.baseMaxStock = 1;
            skillDefRiotShotgun.baseRechargeInterval = 0f;
            skillDefRiotShotgun.beginSkillCooldownOnSkillEnd = false;
            skillDefRiotShotgun.canceledFromSprinting = false;
            skillDefRiotShotgun.fullRestockOnAssign = true;
            skillDefRiotShotgun.interruptPriority = InterruptPriority.Any;
            skillDefRiotShotgun.resetCooldownTimerOnUse = false;
            skillDefRiotShotgun.isCombatSkill = true;
            skillDefRiotShotgun.mustKeyPress = false;
            skillDefRiotShotgun.cancelSprintingOnActivation = true;
            skillDefRiotShotgun.rechargeStock = 1;
            skillDefRiotShotgun.requiredStock = 1;
            skillDefRiotShotgun.stockToConsume = 1;
            skillDefRiotShotgun.icon = Assets.icon10Shotgun;
            skillDefRiotShotgun.skillDescriptionToken = "ENFORCER_PRIMARY_SHOTGUN_DESCRIPTION";
            skillDefRiotShotgun.skillName = "ENFORCER_PRIMARY_SHOTGUN_NAME";
            skillDefRiotShotgun.skillNameToken = "ENFORCER_PRIMARY_SHOTGUN_NAME";

            return skillDefRiotShotgun;
        }

        private SkillDef PrimarySkillDef_SuperShotgun()
        {
            string desc = "Fire up to 2 shotgun blasts for <style=cIsDamage>" + SuperShotgun2.bulletCount/2 + "x" + 100f * superDamage.Value + "% damage</style>.\nWhile using <style=cIsUtility>Protect and Serve</style>, fire <style=cIsDamage>both barrels at once.</style>";

            LanguageAPI.Add("ENFORCER_PRIMARY_SUPERSHOTGUN_NAME", "Super Shotgun");
            LanguageAPI.Add("ENFORCER_PRIMARY_SUPERSHOTGUN_DESCRIPTION", desc);

            SkillDef skillDefSuperShotgun = ScriptableObject.CreateInstance<SkillDef>();
            skillDefSuperShotgun.activationState = new SerializableEntityStateType(typeof(SuperShotgun2));
            skillDefSuperShotgun.activationStateMachineName = "Weapon";
            skillDefSuperShotgun.baseMaxStock = 1;
            skillDefSuperShotgun.baseRechargeInterval = 0f;
            skillDefSuperShotgun.beginSkillCooldownOnSkillEnd = false;
            skillDefSuperShotgun.canceledFromSprinting = false;
            skillDefSuperShotgun.fullRestockOnAssign = true;
            skillDefSuperShotgun.interruptPriority = InterruptPriority.Any;
            skillDefSuperShotgun.resetCooldownTimerOnUse = false;
            skillDefSuperShotgun.isCombatSkill = true;
            skillDefSuperShotgun.mustKeyPress = false;
            skillDefSuperShotgun.cancelSprintingOnActivation = true;
            skillDefSuperShotgun.rechargeStock = 1;
            skillDefSuperShotgun.requiredStock = 1;
            skillDefSuperShotgun.stockToConsume = 1;
            skillDefSuperShotgun.icon = Assets.icon11SuperShotgun;
            skillDefSuperShotgun.skillDescriptionToken = "ENFORCER_PRIMARY_SUPERSHOTGUN_DESCRIPTION";
            skillDefSuperShotgun.skillName = "ENFORCER_PRIMARY_SUPERSHOTGUN_NAME";
            skillDefSuperShotgun.skillNameToken = "ENFORCER_PRIMARY_SUPERSHOTGUN_NAME";

            return skillDefSuperShotgun;
        }

        private SkillDef PrimarySkillDef_AssaultRifle()
        {
            //string damage = $"<style=cIsDamage>{FireBurstRifle.projectileCount}x{100f * FireBurstRifle.damageCoefficient}% damage</style>";
            //string desc = $"Fire a burst of bullets dealing {damage}. <style=cIsUtility>During Protect and Serve</style>, fires <style=cIsDamage>{2 * FireBurstRifle.projectileCount} bullets</style> instead.";

            string damage = $"<style=cIsDamage>{100f * FireMachineGun.damageCoefficient}% damage</style>";

            string desc = $"Unload a barrage of bullets for {damage}.\nWhile using <style=cIsUtility>Protect and Serve</style>, has <style=cIsDamage>increased accuracy</style>, but <style=cIsHealth>slower movement speed</style>.";

            LanguageAPI.Add("ENFORCER_PRIMARY_RIFLE_NAME", "Heavy Machine Gun");
            LanguageAPI.Add("ENFORCER_PRIMARY_RIFLE_DESCRIPTION", desc);

            SkillDef skillDefAssaultRifle = ScriptableObject.CreateInstance<SkillDef>();
            //skillDefAssaultRifle.activationState = new SerializableEntityStateType(typeof(FireBurstRifle));
            skillDefAssaultRifle.activationState = new SerializableEntityStateType(typeof(FireMachineGun));
            skillDefAssaultRifle.activationStateMachineName = "Weapon";
            skillDefAssaultRifle.baseMaxStock = 1;
            skillDefAssaultRifle.baseRechargeInterval = 0f;
            skillDefAssaultRifle.beginSkillCooldownOnSkillEnd = false;
            skillDefAssaultRifle.canceledFromSprinting = false;
            skillDefAssaultRifle.fullRestockOnAssign = true;
            skillDefAssaultRifle.interruptPriority = InterruptPriority.Any;
            skillDefAssaultRifle.resetCooldownTimerOnUse = false;
            skillDefAssaultRifle.isCombatSkill = true;
            skillDefAssaultRifle.mustKeyPress = false;
            skillDefAssaultRifle.cancelSprintingOnActivation = true;
            skillDefAssaultRifle.rechargeStock = 1;
            skillDefAssaultRifle.requiredStock = 1;
            skillDefAssaultRifle.stockToConsume = 1;
            skillDefAssaultRifle.icon = Assets.icon12AssaultRifle;
            skillDefAssaultRifle.skillDescriptionToken = "ENFORCER_PRIMARY_RIFLE_DESCRIPTION";
            skillDefAssaultRifle.skillName = "ENFORCER_PRIMARY_RIFLE_NAME";
            skillDefAssaultRifle.skillNameToken = "ENFORCER_PRIMARY_RIFLE_NAME";

            return skillDefAssaultRifle;
        }

        private SkillDef PrimarySkillDef_Hammer()
        {
            string damage = $"<style=cIsDamage>{ 100f * HammerSwing.damageCoefficient}% damage</style>";
            string shieldDamage = $"<style=cIsDamage>{ 100f * HammerSwing.shieldDamageCoefficient}% damage</style>";
            string desc = $"Swing your hammer for {damage}.\nWhile using Protect and Serve, swing in a <style=cIsUtility>larger area</style>, for {shieldDamage} instead.";

            LanguageAPI.Add("ENFORCER_PRIMARY_HAMMER_NAME", "Breaching Hammer");
            LanguageAPI.Add("ENFORCER_PRIMARY_HAMMER_DESCRIPTION", desc);

            SkillDef skillDefHammer = ScriptableObject.CreateInstance<SkillDef>();
            skillDefHammer.activationState = new SerializableEntityStateType(typeof(HammerSwing));
            skillDefHammer.activationStateMachineName = "Weapon";
            skillDefHammer.baseMaxStock = 1;
            skillDefHammer.baseRechargeInterval = 0f;
            skillDefHammer.beginSkillCooldownOnSkillEnd = false;
            skillDefHammer.canceledFromSprinting = false;
            skillDefHammer.fullRestockOnAssign = true;
            skillDefHammer.interruptPriority = InterruptPriority.Any;
            skillDefHammer.resetCooldownTimerOnUse = false;
            skillDefHammer.isCombatSkill = true;
            skillDefHammer.mustKeyPress = false;
            skillDefHammer.cancelSprintingOnActivation = true;
            skillDefHammer.rechargeStock = 1;
            skillDefHammer.requiredStock = 1;
            skillDefHammer.stockToConsume = 1;
            skillDefHammer.icon = Assets.icon13Hammer;
            skillDefHammer.skillDescriptionToken = "ENFORCER_PRIMARY_HAMMER_DESCRIPTION";
            skillDefHammer.skillName = "ENFORCER_PRIMARY_HAMMER_NAME";
            skillDefHammer.skillNameToken = "ENFORCER_PRIMARY_HAMMER_NAME";

            return skillDefHammer;
        }

        private SkillDef SecondarySkillDef_Bash()
        {
            LanguageAPI.Add("KEYWORD_BASH", "<style=cKeywordName>Bash</style><style=cSub>Applies <style=cIsDamage>stun</style> and <style=cIsUtility>heavy knockback</style>.");

            LanguageAPI.Add("KEYWORD_SPRINTBASH", $"<style=cKeywordName>Shoulder Bash</style><style=cSub><style=cIsDamage>Stunning.</style> A short charge that deals <style=cIsDamage>{100f * ShoulderBash.chargeDamageCoefficient}% damage</style>.\nHitting <style=cIsDamage>heavier enemies</style> deals <style=cIsDamage>{ShoulderBash.knockbackDamageCoefficient * 100f}% damage</style>.");

            //string desc = $"<style=cIsDamage>Bash</style> nearby enemies for <style=cIsDamage>{100f * ShieldBash.damageCoefficient}% damage</style>. <style=cIsUtility>Deflects projectiles</style>. Use while <style=cIsUtility>sprinting</style> to perform a <style=cIsDamage>Shoulder Bash</style> for <style=cIsDamage>{100f * ShoulderBash.chargeDamageCoefficient}-{100f * ShoulderBash.knockbackDamageCoefficient}% damage</style> instead.";
            string desc = $"<style=cIsDamage>Stunning</style>. Knock back enemies for <style=cIsDamage>{100f * ShieldBash.damageCoefficient}% damage</style> and <style=cIsUtility>deflect projectiles</style>.";
            desc += $"\nWhile <style=cIsUtility>sprinting</style>, perform a <style=cIsDamage>Shoulder Bash</style> instead.";
            //desc += $" Deals <style=cIsDamage>{100f * ShoulderBash.chargeDamageCoefficient}% damage</style> while sprinting.";

            LanguageAPI.Add("ENFORCER_SECONDARY_BASH_NAME", "Shield Bash");
            LanguageAPI.Add("ENFORCER_SECONDARY_BASH_DESCRIPTION", desc);

            SkillDef mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(ShieldBash));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 6f;
            mySkillDef.beginSkillCooldownOnSkillEnd = false;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Skill;
            mySkillDef.resetCooldownTimerOnUse = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.cancelSprintingOnActivation = false;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Assets.icon20ShieldBash;
            mySkillDef.skillDescriptionToken = "ENFORCER_SECONDARY_BASH_DESCRIPTION";
            mySkillDef.skillName = "ENFORCER_SECONDARY_BASH_NAME";
            mySkillDef.skillNameToken = "ENFORCER_SECONDARY_BASH_NAME";
            mySkillDef.keywordTokens = new string[] {
                "KEYWORD_STUNNING",
                "KEYWORD_SPRINTBASH"
            };
              //"KEYWORD_BASH",
              //"KEYWORD_SPRINTBASH"

            return mySkillDef;
        }

        private SkillDef UtilitySkillDef_TearGas()
        {
            LanguageAPI.Add("KEYWORD_BLINDED", "<style=cKeywordName>Impaired</style><style=cSub>Lowers <style=cIsDamage>movement speed</style> by <style=cIsDamage>75%</style>, <style=cIsDamage>attack speed</style> by <style=cIsDamage>25%</style> and <style=cIsHealth>armor</style> by <style=cIsDamage>20</style>.</style></style>");

            LanguageAPI.Add("ENFORCER_UTILITY_TEARGAS_NAME", "Tear Gas");
            LanguageAPI.Add("ENFORCER_UTILITY_TEARGAS_DESCRIPTION", "Toss a grenade that covers an area in <style=cIsDamage>Impairing</style> gas.");

            SkillDef tearGasDef = ScriptableObject.CreateInstance<SkillDef>();
            tearGasDef.activationState = new SerializableEntityStateType(typeof(AimTearGas));
            tearGasDef.activationStateMachineName = "Weapon";
            tearGasDef.baseMaxStock = 1;
            tearGasDef.baseRechargeInterval = 24;
            tearGasDef.beginSkillCooldownOnSkillEnd = true;
            tearGasDef.canceledFromSprinting = false;
            tearGasDef.fullRestockOnAssign = true;
            tearGasDef.interruptPriority = InterruptPriority.Skill;
            tearGasDef.resetCooldownTimerOnUse = false;
            tearGasDef.isCombatSkill = true;
            tearGasDef.mustKeyPress = true;
            tearGasDef.cancelSprintingOnActivation = true;
            tearGasDef.rechargeStock = 1;
            tearGasDef.requiredStock = 1;
            tearGasDef.stockToConsume = 1;
            tearGasDef.icon = Assets.icon30TearGas;
            tearGasDef.skillDescriptionToken = "ENFORCER_UTILITY_TEARGAS_DESCRIPTION";
            tearGasDef.skillName = "ENFORCER_UTILITY_TEARGAS_NAME";
            tearGasDef.skillNameToken = "ENFORCER_UTILITY_TEARGAS_NAME";
            tearGasDef.keywordTokens = new string[] {
                "KEYWORD_BLINDED"
            };

            return tearGasDef;
        }

        private SkillDef UtilitySkillDef_StunGrenade()
        {
            LanguageAPI.Add("ENFORCER_UTILITY_STUNGRENADE_NAME", "Stun Grenade");
            LanguageAPI.Add("ENFORCER_UTILITY_STUNGRENADE_DESCRIPTION", "<style=cIsDamage>Stunning</style>. Launch a grenade that concusses enemies for <style=cIsDamage>" + 100f * StunGrenade.damageCoefficient + "% damage</style>. Hold up to 3.");

            SkillDef stunGrenadeDef = ScriptableObject.CreateInstance<SkillDef>();
            stunGrenadeDef.activationState = new SerializableEntityStateType(typeof(StunGrenade));
            stunGrenadeDef.activationStateMachineName = "Weapon";
            stunGrenadeDef.baseMaxStock = 3;
            stunGrenadeDef.baseRechargeInterval = 6f;
            stunGrenadeDef.beginSkillCooldownOnSkillEnd = false;
            stunGrenadeDef.canceledFromSprinting = false;
            stunGrenadeDef.fullRestockOnAssign = true;
            stunGrenadeDef.interruptPriority = InterruptPriority.Skill;
            stunGrenadeDef.resetCooldownTimerOnUse = false;
            stunGrenadeDef.isCombatSkill = true;
            stunGrenadeDef.mustKeyPress = false;
            stunGrenadeDef.cancelSprintingOnActivation = true;
            stunGrenadeDef.rechargeStock = 1;
            stunGrenadeDef.requiredStock = 1;
            stunGrenadeDef.stockToConsume = 1;
            stunGrenadeDef.icon = Assets.icon31StunGrenade;
            stunGrenadeDef.skillDescriptionToken = "ENFORCER_UTILITY_STUNGRENADE_DESCRIPTION";
            stunGrenadeDef.skillName = "ENFORCER_UTILITY_STUNGRENADE_NAME";
            stunGrenadeDef.skillNameToken = "ENFORCER_UTILITY_STUNGRENADE_NAME";
            stunGrenadeDef.keywordTokens = new string[] {
                "KEYWORD_STUNNING"
            };

            return stunGrenadeDef;
        }

        private SkillDef SpecialSkillDef_ProtectAndServe()
        {
            LanguageAPI.Add("ENFORCER_SPECIAL_SHIELDUP_NAME", "Protect and Serve");
            LanguageAPI.Add("ENFORCER_SPECIAL_SHIELDUP_DESCRIPTION", "Take a defensive stance, <style=cIsUtility>blocking all damage from the front</style>. <style=cIsUtility>Enhances primary fire</style>, but <style=cIsHealth>prevents sprinting and jumping</style>.");

            SkillDef mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(ProtectAndServe));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 0f;
            mySkillDef.beginSkillCooldownOnSkillEnd = false;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.PrioritySkill;
            mySkillDef.resetCooldownTimerOnUse = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = true;
            mySkillDef.cancelSprintingOnActivation = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Assets.icon40Shield;
            mySkillDef.skillDescriptionToken = "ENFORCER_SPECIAL_SHIELDUP_DESCRIPTION";
            mySkillDef.skillName = "ENFORCER_SPECIAL_SHIELDUP_NAME";
            mySkillDef.skillNameToken = "ENFORCER_SPECIAL_SHIELDUP_NAME";

            return mySkillDef;
        }
        private SkillDef SpecialSkillDef_ShieldDown()
        {
            LanguageAPI.Add("ENFORCER_SPECIAL_SHIELDDOWN_NAME", "Protect and Serve");
            LanguageAPI.Add("ENFORCER_SPECIAL_SHIELDDOWN_DESCRIPTION", "Take a defensive stance, <style=cIsUtility>blocking all damage from the front</style>. <style=cIsDamage>Increases attack speed</style>, but <style=cIsHealth>prevents sprinting and jumping</style>.");

            SkillDef mySkillDef2 = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef2.activationState = new SerializableEntityStateType(typeof(ProtectAndServe));
            mySkillDef2.activationStateMachineName = "Weapon";
            mySkillDef2.baseMaxStock = 1;
            mySkillDef2.baseRechargeInterval = 0f;
            mySkillDef2.beginSkillCooldownOnSkillEnd = false;
            mySkillDef2.canceledFromSprinting = false;
            mySkillDef2.fullRestockOnAssign = true;
            mySkillDef2.interruptPriority = InterruptPriority.PrioritySkill;
            mySkillDef2.resetCooldownTimerOnUse = false;
            mySkillDef2.isCombatSkill = true;
            mySkillDef2.mustKeyPress = true;
            mySkillDef2.cancelSprintingOnActivation = false;
            mySkillDef2.rechargeStock = 1;
            mySkillDef2.requiredStock = 1;
            mySkillDef2.stockToConsume = 1;
            mySkillDef2.icon = Assets.icon40ShieldOff;
            mySkillDef2.skillDescriptionToken = "ENFORCER_SPECIAL_SHIELDDOWN_DESCRIPTION";
            mySkillDef2.skillName = "ENFORCER_SPECIAL_SHIELDDOWN_NAME";
            mySkillDef2.skillNameToken = "ENFORCER_SPECIAL_SHIELDDOWN_NAME";

            return mySkillDef2;
        }

        private SkillDef SpecialSkillDef_EnergyShield()
        {
            LanguageAPI.Add("ENFORCER_SPECIAL_SHIELDON_NAME", "Project and Swerve");
            LanguageAPI.Add("ENFORCER_SPECIAL_SHIELDON_DESCRIPTION", "Take a defensive stance, <style=cIsUtility>projecting an Energy Shield in front of you</style>. <style=cIsDamage>Increases your rate of fire</style>, but <style=cIsUtility>prevents sprinting and jumping</style>.");

            SkillDef mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(EnergyShield));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 0f;
            mySkillDef.beginSkillCooldownOnSkillEnd = false;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.PrioritySkill;
            mySkillDef.resetCooldownTimerOnUse = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = true;
            mySkillDef.cancelSprintingOnActivation = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Assets.testIcon;
            mySkillDef.skillDescriptionToken = "ENFORCER_SPECIAL_SHIELDON_DESCRIPTION";
            mySkillDef.skillName = "ENFORCER_SPECIAL_SHIELDON_NAME";
            mySkillDef.skillNameToken = "ENFORCER_SPECIAL_SHIELDON_NAME";

            return mySkillDef;
        }
        private SkillDef SpecialSkillDef_EnergyShieldDown()
        {
            LanguageAPI.Add("ENFORCER_SPECIAL_SHIELDOFF_NAME", "Project and Swerve");
            LanguageAPI.Add("ENFORCER_SPECIAL_SHIELDOFF_DESCRIPTION", "Take a defensive stance, <style=cIsUtility>projecting an Energy Shield in front of you</style>. <style=cIsDamage>Increases your rate of fire</style>, but <style=cIsUtility>prevents sprinting and jumping</style>.");

            SkillDef mySkillDef2 = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef2.activationState = new SerializableEntityStateType(typeof(EnergyShield));
            mySkillDef2.activationStateMachineName = "Weapon";
            mySkillDef2.baseMaxStock = 1;
            mySkillDef2.baseRechargeInterval = 0f;
            mySkillDef2.beginSkillCooldownOnSkillEnd = false;
            mySkillDef2.canceledFromSprinting = false;
            mySkillDef2.fullRestockOnAssign = true;
            mySkillDef2.interruptPriority = InterruptPriority.PrioritySkill;
            mySkillDef2.resetCooldownTimerOnUse = false;
            mySkillDef2.isCombatSkill = true;
            mySkillDef2.mustKeyPress = true;
            mySkillDef2.cancelSprintingOnActivation = false;
            mySkillDef2.rechargeStock = 1;
            mySkillDef2.requiredStock = 1;
            mySkillDef2.stockToConsume = 1;
            mySkillDef2.icon = Assets.testIcon;
            mySkillDef2.skillDescriptionToken = "ENFORCER_SPECIAL_SHIELDOFF_DESCRIPTION";
            mySkillDef2.skillName = "ENFORCER_SPECIAL_SHIELDOFF_NAME";
            mySkillDef2.skillNameToken = "ENFORCER_SPECIAL_SHIELDOFF_NAME";

            return mySkillDef2;
        }

        private SkillDef SpecialSkillDef_SkamteBord()
        {
            LanguageAPI.Add("ENFORCER_SPECIAL_BOARDUP_NAME", "Skateboard");
            LanguageAPI.Add("ENFORCER_SPECIAL_BOARDUP_DESCRIPTION", "Swag.");

            SkillDef mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(Skateboard));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 0f;
            mySkillDef.beginSkillCooldownOnSkillEnd = false;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.PrioritySkill;
            mySkillDef.resetCooldownTimerOnUse = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = true;
            mySkillDef.cancelSprintingOnActivation = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Assets.icon42SkateBoard;
            mySkillDef.skillDescriptionToken = "ENFORCER_SPECIAL_BOARDUP_DESCRIPTION";
            mySkillDef.skillName = "ENFORCER_SPECIAL_BOARDUP_NAME";
            mySkillDef.skillNameToken = "ENFORCER_SPECIAL_BOARDUP_NAME";

            return mySkillDef;
        }
        private SkillDef SpecialSkillDef_SkamteBordDown()
        {
            LanguageAPI.Add("ENFORCER_SPECIAL_BOARDDOWN_NAME", "Skateboard");
            LanguageAPI.Add("ENFORCER_SPECIAL_BOARDDOWN_DESCRIPTION", "Unswag.");

            SkillDef mySkillDef2 = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef2.activationState = new SerializableEntityStateType(typeof(Skateboard));
            mySkillDef2.activationStateMachineName = "Weapon";
            mySkillDef2.baseMaxStock = 1;
            mySkillDef2.baseRechargeInterval = 0f;
            mySkillDef2.beginSkillCooldownOnSkillEnd = false;
            mySkillDef2.canceledFromSprinting = false;
            mySkillDef2.fullRestockOnAssign = true;
            mySkillDef2.interruptPriority = InterruptPriority.PrioritySkill;
            mySkillDef2.resetCooldownTimerOnUse = false;
            mySkillDef2.isCombatSkill = true;
            mySkillDef2.mustKeyPress = true;
            mySkillDef2.cancelSprintingOnActivation = false;
            mySkillDef2.rechargeStock = 1;
            mySkillDef2.requiredStock = 1;
            mySkillDef2.stockToConsume = 1;
            mySkillDef2.icon = Assets.icon42SkateBoardOff;
            mySkillDef2.skillDescriptionToken = "ENFORCER_SPECIAL_BOARDDOWN_DESCRIPTION";
            mySkillDef2.skillName = "ENFORCER_SPECIAL_BOARDDOWN_NAME";
            mySkillDef2.skillNameToken = "ENFORCER_SPECIAL_BOARDDOWN_NAME";

            return mySkillDef2;
        }
        #endregion

        private void ScepterSkillSetup()
        {
            Modules.States.AddSkill(typeof(AimDamageGas));

            LanguageAPI.Add("ENFORCER_UTILITY_TEARGASSCEPTER_NAME", "Mustard Gas");
            LanguageAPI.Add("ENFORCER_UTILITY_TEARGASSCEPTER_DESCRIPTION", "Toss a grenade that covers an area in <style=cIsDamage>Impairing</style> gas, choking enemies for <style=cIsDamage>200% damage per second</style>.");

            tearGasScepterDef = ScriptableObject.CreateInstance<SkillDef>();
            tearGasScepterDef.activationState = new SerializableEntityStateType(typeof(AimDamageGas));
            tearGasScepterDef.activationStateMachineName = "Weapon";
            tearGasScepterDef.baseMaxStock = 1;
            tearGasScepterDef.baseRechargeInterval = 24;
            tearGasScepterDef.beginSkillCooldownOnSkillEnd = true;
            tearGasScepterDef.canceledFromSprinting = false;
            tearGasScepterDef.fullRestockOnAssign = true;
            tearGasScepterDef.interruptPriority = InterruptPriority.Skill;
            tearGasScepterDef.resetCooldownTimerOnUse = false;
            tearGasScepterDef.isCombatSkill = true;
            tearGasScepterDef.mustKeyPress = true;
            tearGasScepterDef.cancelSprintingOnActivation = true;
            tearGasScepterDef.rechargeStock = 1;
            tearGasScepterDef.requiredStock = 1;
            tearGasScepterDef.stockToConsume = 1;
            tearGasScepterDef.icon = Assets.icon30TearGasScepter;
            tearGasScepterDef.skillDescriptionToken = "ENFORCER_UTILITY_TEARGASSCEPTER_DESCRIPTION";
            tearGasScepterDef.skillName = "ENFORCER_UTILITY_TEARGASSCEPTER_NAME";
            tearGasScepterDef.skillNameToken = "ENFORCER_UTILITY_TEARGASSCEPTER_NAME";
            tearGasScepterDef.keywordTokens = new string[] {
                "KEYWORD_BLINDED"
            };

            Modules.States.AddSkillDef(tearGasScepterDef);

            Modules.States.AddSkill(typeof(ShockGrenade));

            LanguageAPI.Add("ENFORCER_UTILITY_SHOCKGRENADE_NAME", "Shock Grenade");
            LanguageAPI.Add("ENFORCER_UTILITY_SHOCKGRENADE_DESCRIPTION", "<style=cIsDamage>Shocking</style>. Launch a grenade that electrocutes enemies for <style=cIsDamage>" + 100f * ShockGrenade.damageCoefficient + "% damage</style>. Hold up to 3.");

            shockGrenadeDef = ScriptableObject.CreateInstance<SkillDef>();
            shockGrenadeDef.activationState = new SerializableEntityStateType(typeof(ShockGrenade));
            shockGrenadeDef.activationStateMachineName = "Weapon";
            shockGrenadeDef.baseMaxStock = 3;
            shockGrenadeDef.baseRechargeInterval = 6f;
            shockGrenadeDef.beginSkillCooldownOnSkillEnd = false;
            shockGrenadeDef.canceledFromSprinting = false;
            shockGrenadeDef.fullRestockOnAssign = true;
            shockGrenadeDef.interruptPriority = InterruptPriority.Skill;
            shockGrenadeDef.resetCooldownTimerOnUse = false;
            shockGrenadeDef.isCombatSkill = true;
            shockGrenadeDef.mustKeyPress = false;
            shockGrenadeDef.cancelSprintingOnActivation = true;
            shockGrenadeDef.rechargeStock = 1;
            shockGrenadeDef.requiredStock = 1;
            shockGrenadeDef.stockToConsume = 1;
            shockGrenadeDef.icon = Assets.icon31StunGrenadeScepter;
            shockGrenadeDef.skillDescriptionToken = "ENFORCER_UTILITY_SHOCKGRENADE_DESCRIPTION";
            shockGrenadeDef.skillName = "ENFORCER_UTILITY_SHOCKGRENADE_NAME";
            shockGrenadeDef.skillNameToken = "ENFORCER_UTILITY_SHOCKGRENADE_NAME";
            shockGrenadeDef.keywordTokens = new string[] {
                "KEYWORD_SHOCKING"
            };

            Modules.States.AddSkillDef(shockGrenadeDef);
        }

        private void CSSPreviewSetup()
        {
            //something broke here i don't really understand it
            //  that's because holy shit i wrote this like a fucking ape. do not forgive me for this

            //// NULLCHECK YOUR SHIT FOR FUCKS SAKE
            if (_previewController)
            {
                List<int> emptyIndices = new List<int>();
                for (int i = 0; i < _previewController.skillChangeResponses.Length; i++)
                {
                    if (_previewController.skillChangeResponses[i].triggerSkillFamily == null ||
                        _previewController.skillChangeResponses[i].triggerSkillFamily == null)
                    {
                        emptyIndices.Add(i);
                    }
                }

                if (emptyIndices.Count == 0)
                    return;

                List<CharacterSelectSurvivorPreviewDisplayController.SkillChangeResponse> responsesList = _previewController.skillChangeResponses.ToList();
                for (int i = emptyIndices.Count - 1; i >= 0; i--)
                {
                    responsesList.RemoveAt(emptyIndices[i]);
                }

                _previewController.skillChangeResponses = responsesList.ToArray();
            }
        }

        private void MemeSetup()
        {
            Type[] memes = new Type[]
            {
                typeof(DefaultDance),
                typeof(Floss),
                typeof(FLINTLOCKWOOD),
                typeof(SirenToggle),
                typeof(NemesisRest),
                typeof(EntityStates.Nemforcer.Emotes.Salute),
                typeof(Enforcer.Emotes.EnforcerSalute)
            };
              
            for (int i = 0; i < memes.Length; i++)
            {
                Modules.States.AddSkill(memes[i]);
            }
        }
    }
    #endregion

    public class MenuSound : MonoBehaviour
    {
        private uint playID;

        private void OnEnable()
        {
            //this.playID = Util.PlaySound(Sounds.CharSelect, base.gameObject);

            var i = GetComponentInChildren<EnforcerLightController>();
            if (i)
            {
                i.FlashLights(3);
            }

            var j = GetComponentInChildren<EnforcerLightControllerAlt>();
            if (j)
            {
                j.ToggleSiren();
            }
        }

        private void OnDestroy()
        {
            if (this.playID != 0) AkSoundEngine.StopPlayingID(this.playID);
        }
    }

    public class EnforcerFrogComponent : MonoBehaviour
    {
        public static event Action<bool> FrogGet = delegate { };
        
        private void Awake()
        {
            InvokeRepeating("Sex", 0.5f, 0.5f);
        }

        private void Sex()
        {
            Collider[] array = Physics.OverlapSphere(transform.position, 16, LayerIndex.defaultLayer.mask);
            for (int i = 0; i < array.Length; i++)
            {
                CharacterBody component = array[i].GetComponent<CharacterBody>();
                if (component)
                {
                    if (component.baseNameToken == "ENFORCER_NAME") FrogGet(true);
                }
            }
        }
    }

    public class ParticleFuckingShitComponent : MonoBehaviour
    {
        private void Start()
        {
            this.transform.parent = null;
            this.gameObject.AddComponent<DestroyOnTimer>().duration = 8;
        }
    }

    public class TearGasComponent : MonoBehaviour
    {
        private int count;
        private int lastCount;
        private uint playID;

        public static event Action<int> GasCheck = delegate { };

        private void Awake()
        {
            playID = Util.PlaySound(Sounds.GasContinuous, base.gameObject);

            InvokeRepeating("Fuck", 0.25f, 0.25f);
        }

        private void Fuck()
        {
            //this is gross and hacky pls someone do this a different way eventually

            count = 0;

            foreach(CharacterBody i in GameObject.FindObjectsOfType<CharacterBody>())
            {
                if (i && i.HasBuff(Modules.Buffs.impairedBuff)) count++;
            }

            if (lastCount != count) GasCheck(count);

            lastCount = count;
        }

        private void OnDestroy()
        {
            AkSoundEngine.StopPlayingID(playID);
        }
    }

    public static class Sounds
    {
        public static readonly string CharSelect = "Play_Enforcer_CharSelect";

        public static readonly string FireShotgun = "Play_RiotShotgun_shoot"; //Shotgun_shot
        public static readonly string FireShotgunCrit = "Play_RiotShotgun_Crit"; //Shotgun_shot_crit
        public static readonly string FireClassicShotgun = "Ror1_Shotgun";

        public static readonly string FireSuperShotgun = "Super_Shotgun";
        public static readonly string FireSuperShotgunCrit = "Super_Shotgun_crit";
        public static readonly string FireSuperShotgunDOOM = "Doom_2_Super_Shotgun";
        public static readonly string FireSuperShotgunSingle = "Play_SSG_single";
        public static readonly string FireSuperShotgunSingleCrit = "Play_SSG_single_crit";

        public static readonly string FireAssaultRifleSlow = "Assault_Shots_1";
        public static readonly string FireAssaultRifleFast = "Assault_Shots_2";

        public static readonly string FireBlasterShotgun = "Blaster_Shotgun";
        public static readonly string FireBlasterRifle = "Blaster_Rifle";

        public static readonly string FireBungusShotgun = "Bungus_Riot";
        public static readonly string FireBungusSSG = "Bungus_SSg";
        public static readonly string FireBungusRifle = "Bungus_AR";

        public static readonly string ShieldBash = "Bash";
        public static readonly string BashHitEnemy = "Bash_Hit_Enemy";
        public static readonly string BashDeflect = "Bash_Deflect"; //"Play_Reflect_Ding"
        public static readonly string SirenDeflect = "Play_Siren_Reflect";

        public static readonly string ShoulderBashHit = "Shoulder_Bash_Hit";

        public static readonly string LaunchStunGrenade = "Launch_Stun";
        public static readonly string StunExplosion = "Stun_Explosion";

        public static readonly string LaunchTearGas = "Launch_Gas";
        public static readonly string GasExplosion = "Gas_Explosion";
        public static readonly string GasContinuous = "Gas_Continous";

        public static readonly string ShieldUp = "R_up";
        public static readonly string ShieldDown = "R_down";

        public static readonly string ShieldBlockLight = "Shield_Block_light";
        public static readonly string ShieldBlockHeavy = "Shield_Block_heavy";

        public static readonly string EnergyShieldUp = "Energy_R_Up";
        public static readonly string EnergyShieldDown = "Energy_R_down";

        public static readonly string ShellHittingFloor = "Shell_Hitting_floor";
        public static readonly string ShellHittingFloorFast = "Shell_Hitting_Floor_Fast";
        public static readonly string ShellHittingFloorSlow = "Shell_Hitting_Floor_Slow";

        public static readonly string NemesisSwing = "Play_Heavy_Swing";
        public static readonly string NemesisImpact = "Play_Heavy_Swing_Hit";
        public static readonly string NemesisSwing2 = "Play_HammerswingNewL";
        public static readonly string NemesisImpact2 = "Play_NemHammerImpact";
        public static readonly string NemesisSwingSecondary = "Play_NemSwingSecondary";

        public static readonly string NemesisSwingAxe = "NemforcerAxeSwing";
        public static readonly string NemesisImpactAxe = "NemforcerAxeHit";
        
        public static readonly string NemesisStartCharge = "Play_chargeStart";
        public static readonly string NemesisMaxCharge = "Play_chargeMax";
        public static readonly string NemesisFlameLoop = "Play_HammerFlameLoop";
        public static readonly string NemesisFlameBurst = "Play_Hammer_Slam";
        public static readonly string NemesisSwingL = "Play_Heavy_Swing_L";
        public static readonly string NemesisSmash = "Play_Hammer_Smash";

        public static readonly string NemesisGrenadeThrow = "Play_GrenadeThrow";

        public static readonly string NemesisMinigunSheathe = "Play_MinigunSheathe";
        public static readonly string NemesisMinigunUnsheathe = "Play_MinigunUnsheathe";
        public static readonly string NemesisMinigunWindDown = "Play_minigun_wind_down";
        public static readonly string NemesisMinigunWindUp = "Play_minigun_wind_up";
        public static readonly string NemesisMinigunShooting = "Play_Minigun_Shoot";

        public static readonly string NemesisMinigunSpinUp = "NemforcerMinigunSpinUp";
        public static readonly string NemesisMinigunSpinDown = "NemforcerMinigunSpinDown";
        public static readonly string NemesisMinigunLoop = "NemforcerMinigunLoop";

        public static readonly string DeathSound = "Death_Siren";
        public static readonly string SirenButton = "Siren_Button";
        public static readonly string SirenSpawn = "Siren_Spawn";
        public static readonly string Croak = "Croak_siren";
        public static readonly string HomeRun = "Play_Home_Run_Bat_Hit";
        public static readonly string Bonk = "Play_Bonk";

        public static readonly string DefaultDance = "Default_forcer";
        public static readonly string Floss = "Flossforcer";
        public static readonly string InfiniteDab = "Infiniforcer";
        public static readonly string DOOM = "DOOM";
        public static readonly string MioHonda = "Mio_Honda";

        public static readonly string SkateGrind = "grindmetal03";
        public static readonly string SkateLand = "Landing";
        public static readonly string SkateOllie = "Ollie";
        public static readonly string Skate180 = "Play_180";
        public static readonly string SkateRoll = "rollconcrete02";
        //rtcp - Skateboard_Speed

        public static readonly string SkamteJumpGap = "HUD_jumpgap";
        public static readonly string SkamteScore = "HUD_score";
        public static readonly string SkamtePerfectTrick = "HUD_perfecttrick";
        public static readonly string SkamteSpecialTrick = "HUD_specialtrick";

        public static readonly string DededeSwing = "Play_se_dedede_hammer_swing_m";
        public static readonly string DededeImpactS = "Play_dedede_hammer_attack_m";
        public static readonly string DededeImpactL = "Play_dedede_hammer_attack_l";

        public static readonly string HMGShoot = "Play_HMG_shoot";
        public static readonly string HMGCrit = "Play_HMG_crit";
    }
}