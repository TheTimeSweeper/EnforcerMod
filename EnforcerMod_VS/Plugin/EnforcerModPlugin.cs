using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using EntityStates;
using EntityStates.Enforcer;
using EntityStates.Enforcer.NeutralSpecial;
using IL.RoR2.ContentManagement;
using KinematicCharacterController;
using Modules;
using Modules.Characters;
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
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace EnforcerPlugin {

    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.DrBibop.VRAPI", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.DestroyedClone.AncientScepter", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.KomradeSpectre.Aetherium", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.Sivelos.SivsItems", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.K1454.SupplyDrop", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.TeamMoonstorm.Starstorm2", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.cwmlolzlz.skills", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.KingEnderBrine.ItemDisplayPlacementHelper", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.Moffein.RiskyArtifacts", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.bepis.r2api.items", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("HIFU.Inferno", BepInDependency.DependencyFlags.SoftDependency)][BepInDependency("com.johnedwa.RTAutoSprintEx", BepInDependency.DependencyFlags.SoftDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, "Enforcer", "3.7.2")]
    public class EnforcerModPlugin : BaseUnityPlugin
    {
        public const string MODUID = "com.EnforcerGang.Enforcer";
        
        public static EnforcerModPlugin instance;

        public static bool holdonasec = true;
        
        //i didn't want this to be static considering we're using an instance now but it throws 23 errors if i remove the static modifier 
        //i'm not dealing with that
        //public static GameObject characterBodyPrefab;
        //public static GameObject characterDisplay;
        
        public static GameObject needlerCrosshair;

        public static GameObject nemesisSpawnEffect;

        public static GameObject bulletTracer;
        public static GameObject bulletTracerSSG;
        public static GameObject laserTracer;
        public static GameObject bungusTracer = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerEngiTurret");
        public static GameObject minigunTracer;

        public static GameObject tearGasProjectilePrefab;
        public GameObject tearGasPrefab;

        public static GameObject damageGasProjectile;
        public GameObject damageGasEffect;

        public static GameObject stunGrenade;
        public static GameObject shockGrenade;

        public static GameObject blockEffectPrefab;
        public static GameObject heavyBlockEffectPrefab;
        public static GameObject hammerSlamEffect;
        public static GameObject hammerSlamEffectShield;

        //These bodies ignore the Guaranteed Block Cone
        private static List<BodyIndex> GuaranteedBlockBlacklist = new List<BodyIndex>();
        //add enemy's bodyname to this list if you don't want them to be blocked by enforcer's extra blocking code (they will still be blocked by the shield hurtbox)
        //for example, large melee attacks you want to go through the shield (mithrix, beetle guard), or characters with weird characterbody.corepositions (worms).
            //if you're worried about projectile weirdness, the cone only checks if attacker == inflictor, so projectiles won't be included in the code check
        public static List<string> GuaranteedBlockBlacklistBodyNames = new List<string> {
            "BrotherBody",
            "MagmaWormBody",
            "ElectricWormBody",
            "BeetleGuardBody",
            "VerminBody"
        };
        public static BodyIndex EnforcerBodyIndex;
        public static BodyIndex NemesisEnforcerBodyIndex;
        public static BodyIndex NemesisEnforcerBossBodyIndex;

        public static readonly Color characterColor = new Color(0.26f, 0.27f, 0.46f);

        public static bool cum; //don't ask

        public static bool ScepterInstalled = false;
        public static bool aetheriumInstalled = false;
        public static bool sivsItemsInstalled = false;
        public static bool supplyDropInstalled = false;
        public static bool starstormInstalled = false;
        public static bool skillsPlusInstalled = false;
        public static bool IDPHelperInstalled = false;
        public static bool VREnabled = false;
        public static bool RiskyArtifactsInstalled = false;
        public static bool autoSprintInstalled = false;

        public static DamageAPI.ModdedDamageType barrierDamageType;

        //private SkillLocator _skillLocator;
        //private CharacterSelectSurvivorPreviewDisplayController _previewController;

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
        internal static ManualLogSource StaticLogger;

        void Awake()
        {
            StaticLogger = Logger;

            Files.Init(Info);

            Modules.Config.ConfigShit(this);

            Assets.Initialize();

            Tokens.RegisterTokens();
        }

        private void Start() {
            Logger.LogInfo("[Initializing Enforcer]");

            SoundBanks.Init();

            SetupModCompat();

            EnforcerUnlockables.RegisterUnlockables();

            new EnforcerSurvivor().Initialize();

            barrierDamageType = DamageAPI.ReserveDamageType();

            ItemDisplays.PopulateDisplays();

            Modules.Buffs.RegisterBuffs();
            RegisterProjectile();
            CreateCrosshair();

            new NemforcerPlugin().Init();

            Hook();
            //new Modules.ContentPacks().CreateContentPack();
            RoR2.ContentManagement.ContentManager.collectContentPackProviders += ContentManager_collectContentPackProviders;
            RoR2.ContentManagement.ContentManager.onContentPacksAssigned += ContentManager_onContentPacksAssigned;
            //RoR2.RoR2Application.onLoad += LateSetupItemDisplays;

            gameObject.AddComponent<TestValueManager>();
            
            Logger.LogInfo("[Initialized]");
        }

        private void SetupModCompat() {

            //aetherium item displays- dll won't compile without a reference to aetherium
            aetheriumInstalled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.KomradeSpectre.Aetherium");
            sivsItemsInstalled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.Sivelos.SivsItems");
            supplyDropInstalled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.K1454.SupplyDrop");

            //scepter stuff
            ScepterInstalled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.DestroyedClone.AncientScepter");
            //shartstorm 2 xDDDD
            starstormInstalled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.TeamMoonstorm.Starstorm2");

            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.cwmlolzlz.skills")) {
                skillsPlusInstalled = true;
                SkillsPlusCompat.init();
            }

            IDPHelperInstalled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.KingEnderBrine.ItemDisplayPlacementHelper");

            RiskyArtifactsInstalled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.Moffein.RiskyArtifacts");

            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.DrBibop.VRAPI")) {
                SetupVR();
            }

            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.xoxfaby.BetterUI")) {
                BetterUICompat.init();
            }

            autoSprintInstalled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.johnedwa.RTAutoSprintEx");

            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.bepis.r2api.items")) {
                FixItemDisplays();
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static void FixItemDisplays() {
            string[] bods = new string[]
            {
                "NemesisEnforcerBody",
                "MinerBody",
                "CHEF",
                "ExecutionerBody",
                "NemmandoBody"
            };

            for (int i = 0; i < bods.Length; i++) {
                ItemAPI.DoNotAutoIDRSFor(bods[i]);
            }
        }

        private void ContentManager_onContentPacksAssigned(HG.ReadOnlyArray<RoR2.ContentManagement.ReadOnlyContentPack> obj) {//LateSetupItemDisplays() {
            EnforcerItemDisplays.RegisterDisplays();
            NemItemDisplays.RegisterDisplays();
        }

        private void ContentManager_collectContentPackProviders(RoR2.ContentManagement.ContentManager.AddContentPackProviderDelegate addContentPackProvider) {
            addContentPackProvider(new Modules.ContentPacks());
        }

        //[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        //private void ScepterSetup()
        //{
        //    AncientScepter.AncientScepterItem.instance.RegisterScepterSkill(tearGasScepterDef, "EnforcerBody", SkillSlot.Utility, 0);
        //    AncientScepter.AncientScepterItem.instance.RegisterScepterSkill(shockGrenadeDef, "EnforcerBody", SkillSlot.Utility, 1);
        //}

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private void SetupVR()
        {
            if (!VRAPI.VR.enabled || !VRAPI.MotionControls.enabled) return;

            VREnabled = true;
            Assets.loadVRBundle();
            VRAPI.MotionControls.AddHandPrefab(Assets.vrEnforcerDominantHand);
            VRAPI.MotionControls.AddHandPrefab(Assets.vrEnforcerNonDominantHand);
            VRAPI.MotionControls.AddHandPrefab(Assets.vrNemforcerDominantHand);
            VRAPI.MotionControls.AddHandPrefab(Assets.vrNemforcerNonDominantHand);
            VRAPI.MotionControls.AddSkillBindingOverride("EnforcerBody", SkillSlot.Primary, SkillSlot.Secondary, SkillSlot.Special, SkillSlot.Utility);
            VRAPI.MotionControls.AddSkillBindingOverride("NemesisEnforcerBody", SkillSlot.Primary, SkillSlot.Utility, SkillSlot.Special, SkillSlot.Secondary);
        }

        private void Hook()
        {
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
            //add hooks here
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
            On.EntityStates.GolemMonster.FireLaser.OnEnter += FireLaser_OnEnter;
            //uncomment this when nebby's variants return
            //although truthfully rewrite this shit it's terrible
            //On.EntityStates.BaseState.OnEnter += BaseState_OnEnter;
            //On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnEnemyHit;
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
            On.RoR2.CharacterBody.Update += CharacterBody_Update;
            On.RoR2.CharacterBody.OnLevelUp += CharacterBody_OnLevelChanged;
            On.RoR2.CharacterMaster.OnInventoryChanged += CharacterMaster_OnInventoryChanged;
            //On.RoR2.BodyCatalog.SetBodyPrefabs += BodyCatalog_SetBodyPrefabs;
            On.RoR2.ContentManagement.ContentManager.SetContentPacks += ContentManager_SetContentPacks;
            On.RoR2.SceneDirector.Start += SceneDirector_Start;
            On.RoR2.ArenaMissionController.BeginRound += ArenaMissionController_BeginRound;
            On.RoR2.ArenaMissionController.EndRound += ArenaMissionController_EndRound;
            On.RoR2.EscapeSequenceController.BeginEscapeSequence += EscapeSequenceController_BeginEscapeSequence;
            On.RoR2.UI.MainMenu.BaseMainMenuScreen.OnEnter += BaseMainMenuScreen_OnEnter;
            On.RoR2.CharacterSelectBarController.Awake += CharacterSelectBarController_Awake;
            On.RoR2.MapZone.TryZoneStart += MapZone_TryZoneStart;
            On.RoR2.HealthComponent.Suicide += HealthComponent_Suicide;
            //On.RoR2.TeleportOutController.OnStartClient += TeleportOutController_OnStartClient;
            On.EntityStates.GlobalSkills.LunarNeedle.FireLunarNeedle.OnEnter += FireLunarNeedle_OnEnter;
            On.RoR2.EntityStateMachine.SetState += EntityStateMachine_SetState;
            On.RoR2.DamageInfo.ModifyDamageInfo += DamageInfo_ModifyDamageInfo;
            Run.onRunStartGlobal += Run_onRunStartGlobal;
            On.RoR2.BodyCatalog.Init += BodyCatalog_Init;
        }

        private void BodyCatalog_Init(On.RoR2.BodyCatalog.orig_Init orig) {

            orig();

            for (int i = 0; i < GuaranteedBlockBlacklistBodyNames.Count; i++) {
                AddBodyToBlockBlacklist(GuaranteedBlockBlacklistBodyNames[i]);
            }

            EnforcerBodyIndex = BodyCatalog.FindBodyIndex("EnforcerBody");
            NemesisEnforcerBodyIndex = BodyCatalog.FindBodyIndex("NemesisEnforcerBody");
            NemesisEnforcerBossBodyIndex = BodyCatalog.FindBodyIndex("NemesisEnforcerBossBody");
        }

        public static void AddBodyToBlockBlacklist(string bodyName) {
            BodyIndex index = BodyCatalog.FindBodyIndex(bodyName);
            if (index != BodyIndex.None) GuaranteedBlockBlacklist.Add(index);
        }

        private void Run_onRunStartGlobal(Run obj) {
            //it's for doom music
            //no I'm not renaming it
            EnforcerModPlugin.cum = false;
        }

        private void DamageInfo_ModifyDamageInfo(On.RoR2.DamageInfo.orig_ModifyDamageInfo orig, DamageInfo self, HurtBox.DamageModifier damageModifier) {
            orig(self, damageModifier);
            if(damageModifier == HurtBox.DamageModifier.Barrier) {
                self.AddModdedDamageType(barrierDamageType);
            }
        }
        #region Hooks

        //TODO TEST THIS
        private bool isMonsoon()
        {
            DifficultyDef runDifficulty = DifficultyCatalog.GetDifficultyDef(Run.instance.ruleBook.FindDifficulty());

            return runDifficulty.countsAsHardMode && runDifficulty.scalingValue >= 3;
        }

        private void MapZone_TryZoneStart(On.RoR2.MapZone.orig_TryZoneStart orig, MapZone self, Collider other)
        {
            if (other.gameObject)
            {
                CharacterBody body = other.GetComponent<CharacterBody>();
                if (body)
                {
                    if (body.bodyIndex == EnforcerModPlugin.NemesisEnforcerBodyIndex || body.bodyIndex == EnforcerModPlugin.NemesisEnforcerBossBodyIndex)
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
                        if (!Modules.Config.globalInvasion.Value)
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
                        if (!Modules.Config.globalInvasion.Value)
                        {
                            if (Modules.Config.multipleInvasions.Value)
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
                            if (Modules.Config.multipleInvasions.Value)
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

                    if (!Modules.Config.globalInvasion.Value)
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

                    if (!Modules.Config.globalInvasion.Value)
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

                                    if (Modules.Config.multipleInvasions.Value) NemesisInvasionManager.PerformInvasion(new Xoroshiro128Plus(Run.instance.seed));
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

                                    if (Modules.Config.multipleInvasions.Value) NemesisInvasionManager.PerformInvasion(new Xoroshiro128Plus(Run.instance.seed));
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
                if (newBodyPrefabs[i].name == "EnforcerBody" && newBodyPrefabs[i] != EnforcerSurvivor.instance.bodyPrefab)
                {
                    newBodyPrefabs[i].name = "OldEnforcerBody";
                }
            }
            orig(newBodyPrefabs);
        }

        private void ContentManager_SetContentPacks(On.RoR2.ContentManagement.ContentManager.orig_SetContentPacks orig, List<RoR2.ContentManagement.ReadOnlyContentPack> newContentPacks) {

            for (int i = 0; i < newContentPacks.Count; i++) {
                var contentPack = newContentPacks[i];
                if (contentPack.identifier == "RoR2.Junk") {
                    
                    GameObject body = contentPack.bodyPrefabs.Find("EnforcerBody");
                    body.name = "OldEnforcerBody";
                }
            }
            orig(newContentPacks);
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender.HasBuff(Modules.Buffs.protectAndServeBuff))
            {
                args.armorAdd += 10f;
                args.moveSpeedReductionMultAdd += 1.8f;//self.moveSpeed *= 0.35
            }

            if (sender.HasBuff(Modules.Buffs.minigunBuff))
            {
                args.armorAdd += 60f;
                args.moveSpeedReductionMultAdd += 0.25f;//self.moveSpeed *= 0.8f;
            }

            if (sender.HasBuff(Modules.Buffs.energyShieldBuff))
            {
                args.armorAdd += 40f;
                args.moveSpeedReductionMultAdd += 0.5f; //self.moveSpeed *= 0.65f;
            }

            if (sender.HasBuff(Modules.Buffs.impairedBuff))
            {
                args.armorAdd -= 20f;
                args.moveSpeedReductionMultAdd += 3f; //self.moveSpeed *= 0.25f;
                sender.attackSpeed *= 0.75f;    //TODO: Convert to attackSpeedReductionMultAdd if that gets added to R2API.
            }

            if (sender.HasBuff(Modules.Buffs.nemImpairedBuff))
            {
                //sender.maxJumpCount = 0; //doesn't work with recalcapi
                args.moveSpeedReductionMultAdd += 3f; //self.moveSpeed *= 0.25f;
                if (!sender.characterMotor.isGrounded)
                {
                    sender.characterMotor.velocity.y -= 10;
                }
            }

            if (sender.HasBuff(Modules.Buffs.smallSlowBuff))
            {
                args.armorAdd += 10f;
                args.moveSpeedReductionMultAdd += 0.4f; //self.moveSpeed *= 0.7f;
            }

            if (sender.HasBuff(Modules.Buffs.bigSlowBuff))
            {
                args.moveSpeedReductionMultAdd += 4f; //self.moveSpeed *= 0.2f;
            }

            //regen passive
            //Added isPlayerControlled check because regen on enemies simply turns them into a DPS check that can't even be whittled down.
            //Regen passive is too forgiving, but I don't play enough NemForcer to think of an alternative.
            if (sender.isPlayerControlled && (sender.bodyIndex == EnforcerModPlugin.NemesisEnforcerBodyIndex || sender.bodyIndex == EnforcerModPlugin.NemesisEnforcerBossBodyIndex)) //Use BodyIndex instead.
            {
                HealthComponent hp = sender.healthComponent;
                float regenValue = hp.fullCombinedHealth * NemforcerPlugin.passiveRegenBonus;
                float regen = Mathf.SmoothStep(regenValue, 0, hp.combinedHealth / hp.fullCombinedHealth);

                // reduce it while taking damage, scale it back up over time- only apply this to the normal boss and let ultra keep the bullshit regen
                /*if (sender.teamComponent.teamIndex == TeamIndex.Monster && sender.baseNameToken == "NEMFORCER_NAME")
                {
                    float maxRegenValue = regen;
                    float i = Mathf.Clamp(sender.outOfDangerStopwatch, 0f, 5f);
                    regen = Util.Remap(i, 0f, 5f, 0f, maxRegenValue);
                }*/

                args.baseRegenAdd += regen;

                /*if (sender.teamComponent.teamIndex == TeamIndex.Monster)
                {
                    sender.regen *= 0.8f;   //Would rather do this via args
                    if (sender.HasBuff(RoR2Content.Buffs.SuperBleed) || sender.HasBuff(RoR2Content.Buffs.Bleeding) || sender.HasBuff(RoR2Content.Buffs.OnFire)) sender.regen = 0f;
                }*/
            }
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);

            if (self) 
            {
                //doesn't work in recalculatestatsapi for some reason
                if (self.HasBuff(Modules.Buffs.protectAndServeBuff)) {
                    self.maxJumpCount = 0;
                }
                if (self.HasBuff(Modules.Buffs.energyShieldBuff)) {
                    self.maxJumpCount = 0;
                }
                if (self.HasBuff(Modules.Buffs.impairedBuff)) {
                    self.maxJumpCount = 0;
                }
                if (self.HasBuff(Modules.Buffs.nemImpairedBuff)) {
                    self.maxJumpCount = 0;
                }

                if (self.HasBuff(Modules.Buffs.skateboardBuff)) {
                    self.characterMotor.airControl = 0.1f;
                }
            }
        }

        //todo move this to respective components and subscribe to characterbody.inventory.onvinentorychanged instead of using a hook and checking names
        private void CharacterMaster_OnInventoryChanged(On.RoR2.CharacterMaster.orig_OnInventoryChanged orig, CharacterMaster self)
        {
            orig(self);

            if (self.hasBody) {
                if (self.inventory && Modules.Config.useNeedlerCrosshair.Value) {
                    if (self.inventory.GetItemCount(RoR2Content.Items.LunarPrimaryReplacement) > 0) {
                        self.GetBody()._defaultCrosshairPrefab = needlerCrosshair;

                        //CrosshairUtils.RequestOverrideForBody(self.GetBody(), needlerCrosshair, CrosshairUtils.OverridePriority.Skill);
                    }
                }
            }
        }

        private void CharacterBody_OnLevelChanged(On.RoR2.CharacterBody.orig_OnLevelUp orig, CharacterBody self)
        {
            orig(self);

            if (self.bodyIndex == EnforcerModPlugin.EnforcerBodyIndex)
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
            //Tear Gas damage numbers
            if (info.damageColorIndex == DamageColorIndex.Default && self.body.HasBuff(Buffs.impairedBuff)) {
                info.damageColorIndex = DamageColorIndex.WeakPoint;
            }

            bool blocked = false;
            bool isEnforcer = self.body.bodyIndex == EnforcerModPlugin.EnforcerBodyIndex;

            if (DamageAPI.HasModdedDamageType(info,barrierDamageType) && isEnforcer) { 
                blocked = true;
            }

            if (isEnforcer && info.attacker)
            {
                //uncomment this if barrier blocking isnt enough and you need to check facing direction like old days
                CharacterBody attackerBody = info.attacker.GetComponent<CharacterBody>();
                if (attackerBody) {

                    EnforcerComponent enforcerComponent = self.body.GetComponent<EnforcerComponent>();

                    if (enforcerComponent)
                    {
                        //ugly hack cause golems kept hitting past shield
                        //actually they're just not anymore? probably cause shield isn't parented anymroe
                        //code stays for deflecting tho
                        if (attackerBody.bodyIndex == BodyCatalog.FindBodyIndex("GolemBody") && info.attacker && enforcerComponent.GetShieldBlock(attackerBody.corePosition, 55f))
                        {
                            blocked = self.body.HasBuff(Modules.Buffs.protectAndServeBuff);

                            if (enforcerComponent.isDeflecting)
                            {
                                blocked = true;
                            }
                        }
                        
                        //Hack to get melee enemies to stop penetrating the shield at certain angles.
                        //info.attacker is already guaranteed notnull
                        if (!blocked
                            && !info.damageType.HasFlag(DamageType.DoT) && !info.damageType.HasFlag(DamageType.BypassBlock)
                            && info.attacker == info.inflictor && !GuaranteedBlockBlacklist.Contains(attackerBody.bodyIndex))
                        {
                            if (enforcerComponent.isShielding && enforcerComponent.GetShieldBlock(attackerBody.corePosition, 55f))
                            {
                                blocked = true;
                            }
                        }
                    }

                    if (enforcerComponent && blocked) {
                        enforcerComponent.AttackBlocked(self.body.gameObject);
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

                info.damage = 0f;
                info.rejected = true;
            }

            if (self.body.name == "EnergyShield")
            {
                info.damage = info.procCoefficient;
            }

            orig(self, info);
        }

        private void BaseState_OnEnter(On.EntityStates.BaseState.orig_OnEnter orig, BaseState self) {
            orig(self);

            if (self.outer.customName == "EnforcerParry") {
                self.damageStat *= 5f;
            }

            List<string> absolutelydisgustinghackynamecheck = new List<string> {
                "NebbysWrath.VariantEntityStates.LesserWisp.FireStoneLaser",
                "NebbysWrath.VariantEntityStates.GreaterWisp.FireDoubleStoneLaser",
            };

            if (absolutelydisgustinghackynamecheck.Contains(self.GetType().ToString())) {

                CheckEnforcerParry(self.GetAimRay());
            }
        }

        private void FireLaser_OnEnter(On.EntityStates.GolemMonster.FireLaser.orig_OnEnter orig, EntityStates.GolemMonster.FireLaser self)
        {
            orig(self);

            Ray ray = self.modifiedAimRay;

            CheckEnforcerParry(ray);
        }

        private static void CheckEnforcerParry(Ray ray) {

            RaycastHit raycastHit;

            if (Physics.Raycast(ray, out raycastHit, 1000f, LayerIndex.world.mask | LayerIndex.defaultLayer.mask | LayerIndex.entityPrecise.mask)) {
                                                                             //do I have this power?
                GameObject gob = raycastHit.transform.GetComponent<HurtBox>()?.healthComponent.gameObject;

                if (!gob) {
                    gob = raycastHit.transform.GetComponent<HealthComponent>()?.gameObject;
                }
                                                //I believe I do. it makes the decompiled version look mad ugly tho
                EnforcerComponent enforcer = gob?.GetComponent<EnforcerComponent>();

                //Debug.LogWarning($"tran {raycastHit.transform}, " +
                //    $"hurt {raycastHit.transform.GetComponent<HurtBox>()}, " +
                //    $"health {raycastHit.transform.GetComponent<HurtBox>()?.healthComponent.gameObject}, " +
                //    $"{gob?.GetComponent<EnforcerComponent>()}");

                if (enforcer) {
                    enforcer.invokeOnLaserHitEvent();
                }
            }
        }

        private void FireLunarNeedle_OnEnter(On.EntityStates.GlobalSkills.LunarNeedle.FireLunarNeedle.orig_OnEnter orig, EntityStates.GlobalSkills.LunarNeedle.FireLunarNeedle self)
        {
            // this actually didn't work, hopefully someone else can figure it out bc needler shotgun sounds badass
            // don't forget to register the state if you do :^)
            if (false)//self.outer.commonComponents.characterBody)
            {
                if (self.outer.commonComponents.characterBody.bodyIndex == EnforcerModPlugin.EnforcerBodyIndex)
                {
                    self.outer.SetNextState(new FireNeedler());
                    return;
                }
            }
            
            orig(self);
        }

        //this did work c:
        private void EntityStateMachine_SetState(On.RoR2.EntityStateMachine.orig_SetState orig, EntityStateMachine self, EntityState newState) {

            if (self.commonComponents.characterBody?.bodyIndex == EnforcerModPlugin.EnforcerBodyIndex) {
                if (newState is EntityStates.GlobalSkills.LunarNeedle.FireLunarNeedle)
                    newState = new FireNeedler();
            }
            orig(self, newState);
        }

        private void SceneDirector_Start(On.RoR2.SceneDirector.orig_Start orig, SceneDirector self)
        {
            //good riddance gameobject.finds
            //if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "moon")
            //{
            //    //null checks to hell and back
            //    if (GameObject.Find("EscapeSequenceController")) {
            //        if (GameObject.Find("EscapeSequenceController").transform.Find("EscapeSequenceObjects")) {
            //            if (GameObject.Find("EscapeSequenceController").transform.Find("EscapeSequenceObjects").transform.Find("SmoothFrog")) {
            //                GameObject.Find("EscapeSequenceController").transform.Find("EscapeSequenceObjects").transform.Find("SmoothFrog").gameObject.AddComponent<FrogComponent>();
            //            }
            //        }
            //    }
            //}

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


        private void CharacterSelectBarController_Awake(On.RoR2.CharacterSelectBarController.orig_Awake orig, CharacterSelectBarController self) {

            string bodyName = NemforcerPlugin.characterBodyPrefab.GetComponent<CharacterBody>().baseNameToken;

            bool unlocked = LocalUserManager.readOnlyLocalUsersList.Any((LocalUser localUser) => localUser.userProfile.HasUnlockable(EnforcerUnlockables.nemesisUnlockableDef));

            SurvivorCatalog.FindSurvivorDefFromBody(NemforcerPlugin.characterBodyPrefab).hidden = !unlocked;

            orig(self);
        }

        private void HealthComponent_Suicide(On.RoR2.HealthComponent.orig_Suicide orig, HealthComponent self, GameObject killerOverride, GameObject inflictorOverride, DamageType damageType) {

            if (damageType == DamageType.VoidDeath) {
                //Debug.LogWarning("voidDeath");
                if (self.body.bodyIndex == EnforcerModPlugin.NemesisEnforcerBodyIndex || self.body.bodyIndex == EnforcerModPlugin.NemesisEnforcerBossBodyIndex) {
                    //Debug.LogWarning("nemmememme");
                    if (self.body.teamComponent.teamIndex != TeamIndex.Player) {
                        //Debug.LogWarning("spookyscary");
                        return;
                    }
                }
            }
            orig(self, killerOverride, inflictorOverride, damageType);
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


        #region projectiles and effects
        private void RegisterProjectile()
        {
            //i'm the treasure, baby, i'm the prize, i'm yours forever
            
            stunGrenade = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/CommandoGrenadeProjectile").InstantiateClone("EnforcerStunGrenade", true);

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

            shockGrenade = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/CommandoGrenadeProjectile").InstantiateClone("EnforcerShockGrenade", true);

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

            tearGasProjectilePrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/CommandoGrenadeProjectile").InstantiateClone("EnforcerTearGasGrenade", true);
            tearGasPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/SporeGrenadeProjectileDotZone").InstantiateClone("TearGasDotZone", true);

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
            grenadeImpact.lifetime = 12;
            grenadeImpact.lifetimeAfterImpact = 0.2f;
            grenadeImpact.lifetimeRandomOffset = 0;
            grenadeImpact.blastRadius = 8;
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
            gasFX.AddComponent<DestroyOnTimer>().duration = 12f;
            gasFX.transform.parent = tearGasPrefab.transform;
            gasFX.transform.localPosition = Vector3.zero;

            //i have this really big cut on my shin and it's bleeding but i'm gonna code instead of doing something about it
            // that's the spirit, champ

            tearGasPrefab.AddComponent<DestroyOnTimer>().duration = 12;

            //scepter stuff.........
            //damageGasProjectile = PrefabAPI.InstantiateClone(projectilePrefab, "DamageGasGrenade", true);
            damageGasProjectile = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/CommandoGrenadeProjectile").InstantiateClone("EnforcerTearGasScepterGrenade", true);
            damageGasEffect = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/SporeGrenadeProjectileDotZone").InstantiateClone("TearGasScepterDotZone", true);

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
            dotZone.lifetime = 12f;
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
            scepterGrenadeImpact.lifetime = 12;
            scepterGrenadeImpact.lifetimeAfterImpact = 0.2f;
            scepterGrenadeImpact.lifetimeRandomOffset = 0;
            scepterGrenadeImpact.blastRadius = 8;
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
            scepterGasFX.AddComponent<DestroyOnTimer>().duration = 12f;
            scepterGasFX.transform.parent = damageGasEffect.transform;
            scepterGasFX.transform.localPosition = Vector3.zero;

            damageGasEffect.AddComponent<DestroyOnTimer>().duration = 12;

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
            bulletTracer = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerCommandoShotgun").InstantiateClone("EnforcerBulletTracer", true);

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

            bulletTracerSSG = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerCommandoShotgun").InstantiateClone("EnforcerBulletTracer", true);

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

            laserTracer = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerCommandoShotgun").InstantiateClone("EnforcerLaserTracer", true);

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

            minigunTracer = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerClayBruiserMinigun").InstantiateClone("NemforcerMinigunTracer", true);

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
            blockEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/BearProc").InstantiateClone("EnforcerBlockEffect", true);

            blockEffectPrefab.GetComponent<EffectComponent>().soundName = Sounds.ShieldBlockLight;
            if (!blockEffectPrefab.GetComponent<NetworkIdentity>()) blockEffectPrefab.AddComponent<NetworkIdentity>();

            //heavy block effect
            heavyBlockEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/BearProc").InstantiateClone("EnforcerHeavyBlockEffect", true);

            heavyBlockEffectPrefab.GetComponent<EffectComponent>().soundName = Sounds.ShieldBlockHeavy;
            if (!heavyBlockEffectPrefab.GetComponent<NetworkIdentity>()) heavyBlockEffectPrefab.AddComponent<NetworkIdentity>();

            //hammer slam effect for enforcer m1
            hammerSlamEffect = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/ParentSlamEffect").InstantiateClone("EnforcerHammerSlamEffect");
            hammerSlamEffect.GetComponent<EffectComponent>().applyScale = true;

            Transform dust = hammerSlamEffect.transform.Find("Dust, Directional");
            if(dust) dust.localScale = new Vector3(1, 0.7f, 1);

            Transform nova = hammerSlamEffect.transform.Find("Nova Sphere");
            if(nova) nova.localScale = new Vector3(8, 8, 8);

            if (!hammerSlamEffect.GetComponent<NetworkIdentity>()) hammerSlamEffect.AddComponent<NetworkIdentity>();

            Modules.Content.AddProjectilePrefab(tearGasProjectilePrefab,
                                                damageGasProjectile,
                                                tearGasPrefab,
                                                damageGasEffect,
                                                stunGrenade,
                                                shockGrenade);


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
            GameObject effect = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/effects/lightningstakenova"), "EnforcerShockGrenadeExplosionEffect", false);
            EffectComponent ec = effect.GetComponent<EffectComponent>();
            ec.applyScale = true;
            ec.soundName = "Play_item_use_lighningArm"; //This typo is in the game.
            Modules.Content.AddEffectDef(new EffectDef(effect));

            return effect;
        }

        private void CreateCrosshair()
        {
            needlerCrosshair = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/LoaderCrosshair"), "NeedlerCrosshair", true);
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
        #endregion projectiles and effects
    }
}