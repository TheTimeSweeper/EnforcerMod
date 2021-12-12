using System.Reflection;
using R2API;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using RoR2;
using RoR2.Audio;
using System.Collections.Generic;
using EnforcerPlugin.Modules;

namespace EnforcerPlugin {
    public static class Assets
    {
        public static AssetBundle MainAssetBundle = null;

        public static Material commandoMat;

        public static Texture charPortrait;

        public static Texture nemCharPortrait;
        public static Texture nemBossPortrait;

        //why the fuck were these names so cryptic
        //fucking pleb ass baby coders
        public static Sprite iconP; //idevenfk //oh passive
        public static Sprite icon10Shotgun;//shotgun
        public static Sprite icon11SuperShotgun;//super shotgun
        public static Sprite icon12AssaultRifle;//assault rifle
        public static Sprite icon13Hammer;//hammer
        public static Sprite icon20ShieldBash;//shield bash
        public static Sprite icon30TearGas;//tear gas
        public static Sprite icon30TearGasScepter;//tear gas(scepter)
        public static Sprite icon31StunGrenade;//stun grenade
        public static Sprite icon31StunGrenadeScepter;//stun grenade(scepter)
        public static Sprite icon40Shield;//protect and serve
        public static Sprite icon40ShieldOff;//protect and serve cancel
        public static Sprite icon42SkateBoard;//skateboard
        public static Sprite icon42SkateBoardOff;//skateboard cancel

        public static Sprite nemIconPassive;
        public static Sprite nIcon1;
        public static Sprite nIcon1B;
        public static Sprite nIcon1C;
        public static Sprite nIcon2;
        public static Sprite nIcon2B;
        public static Sprite nIcon3;
        public static Sprite nIcon3B;
        public static Sprite nIcon3C;
        public static Sprite nIcon4;
        public static Sprite nIcon4B;

        public static Sprite testIcon;// for wip skills

        public static GameObject nemesisHammer;

        public static GameObject tearGasGrenadeModel;
        public static GameObject tearGasEffectPrefab;

        public static GameObject tearGasGrenadeModelAlt;
        public static GameObject tearGasEffectPrefabAlt;

        public static GameObject nemGasGrenadeModel;
        public static GameObject nemGasEffectPrefab;

        public static GameObject stunGrenadeModel;

        public static GameObject stunGrenadeModelAlt;

        public static GameObject hammerProjectileModel;
        public static GameObject gordoProjectileModel;

        public static GameObject shotgunShell;
        public static GameObject superShotgunShell;

        public static GameObject shieldBashFX;
        public static GameObject shoulderBashFX;

        public static GameObject hammerSwingFX;
        public static GameObject hammerImpactFX;

        public static GameObject nemSwingFX;
        public static GameObject nemUppercutSwingFX;
        public static GameObject nemSlamSwingFX;
        public static GameObject nemSlamDownFX;
        public static GameObject nemDashFX;
        public static GameObject nemImpactFX;
        public static GameObject nemHeavyImpactFX;
        public static GameObject nemAxeImpactFX;
        public static GameObject nemAxeImpactFXVertical;

        public static GameObject gatDrone;

        public static Material mainMat;

        public static Mesh gmMesh;
        public static Mesh stormtrooperMesh;
        public static Mesh engiMesh;
        public static Mesh desperadoMesh;
        public static Mesh zeroSuitMesh;
        public static Mesh classicMesh;
        public static Mesh sexMesh;
        public static Mesh femMesh;
        public static Mesh fuckingSteveMesh;

        public static Mesh nemClassicMesh;
        public static Mesh nemClassicHammerMesh;
        public static Mesh nemMeshGM;
        public static Mesh nemHammerMeshGM;
        public static Mesh nemAltMesh;
        public static Mesh nemDripMesh;
        public static Mesh nemDripHammerMesh;
        public static Mesh dededeMesh;
        public static Mesh dededeHammerMesh;
        public static Mesh dededeBossMesh;
        public static Mesh minecraftNemMesh;
        public static Mesh minecraftHammerMesh;

        internal static NetworkSoundEventDef hammerHitSoundEvent;
        internal static NetworkSoundEventDef nemHammerHitSoundEvent;
        internal static NetworkSoundEventDef nemAxeHitSoundEvent;

        internal static List<NetworkSoundEventDef> networkSoundEventDefs = new List<NetworkSoundEventDef>();

        public static void PopulateAssets()
        {
            commandoMat = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial;

            if (MainAssetBundle == null)
            {
                using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Enforcer.enforcer"))
                {
                    MainAssetBundle = AssetBundle.LoadFromStream(assetStream);
                }
            }

            //fuck whoever wrote this code and fuck you
            // comment out the soundbank shit and then wonder why sounds aren't working you're literally fucking retarded holy hell
            using (Stream manifestResourceStream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream("Enforcer.EnforcerBank.bnk"))
            {
                byte[] array = new byte[manifestResourceStream2.Length];
                manifestResourceStream2.Read(array, 0, array.Length);
                SoundAPI.SoundBanks.Add(array);
            }

            using (Stream manifestResourceStream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream("Enforcer.NemforcerBank2.bnk"))
            {
                byte[] array = new byte[manifestResourceStream2.Length];
                manifestResourceStream2.Read(array, 0, array.Length);
                SoundAPI.SoundBanks.Add(array);
            }

            Shader hotpoo = Resources.Load<Shader>("Shaders/Deferred/hgstandard");

            string portrait = Random.value > 0.05f ? "texEnforcerIcon" : "texEnforcerIconEpic";
            charPortrait = MainAssetBundle.LoadAsset<Sprite>(portrait).texture;
            nemCharPortrait = MainAssetBundle.LoadAsset<Sprite>("nemIconBlu").texture;
            nemBossPortrait = MainAssetBundle.LoadAsset<Sprite>("nemIconRed").texture;

            iconP = MainAssetBundle.LoadAsset<Sprite>("TestIcon");
            icon10Shotgun = MainAssetBundle.LoadAsset<Sprite>("RiotShotgunIcon");
            icon11SuperShotgun = MainAssetBundle.LoadAsset<Sprite>("SuperShotgunIcon");
            icon12AssaultRifle = MainAssetBundle.LoadAsset<Sprite>("AssaultRifleIcon");
            icon13Hammer = MainAssetBundle.LoadAsset<Sprite>("BreachingHammerIcon");
            icon20ShieldBash = MainAssetBundle.LoadAsset<Sprite>("ShieldBashIcon");
            icon30TearGas = MainAssetBundle.LoadAsset<Sprite>("TearGasIcon");
            icon30TearGasScepter = MainAssetBundle.LoadAsset<Sprite>("TearGasScepterIcon");
            icon31StunGrenade = MainAssetBundle.LoadAsset<Sprite>("StunGrenadeIcon");
            icon31StunGrenadeScepter = MainAssetBundle.LoadAsset<Sprite>("StunGrenadeScepterIcon");
            icon40Shield = MainAssetBundle.LoadAsset<Sprite>("ShieldUpIcon");
            icon40ShieldOff = MainAssetBundle.LoadAsset<Sprite>("ShieldDownIcon");

            if (Config.classicIcons.Value) {

                iconP = MainAssetBundle.LoadAsset<Sprite>("TestIcon");
                icon10Shotgun = MainAssetBundle.LoadAsset<Sprite>("Skill1Icon");
                //icon11SuperShotgun = MainAssetBundle.LoadAsset<Sprite>("Skill1Icon");
                //icon12AssaultRifle = MainAssetBundle.LoadAsset<Sprite>("Skill1Icon");
                //icon13Hammer = MainAssetBundle.LoadAsset<Sprite>("Skill1Icon");
                icon20ShieldBash = MainAssetBundle.LoadAsset<Sprite>("Skill2Icon");
                icon30TearGas = MainAssetBundle.LoadAsset<Sprite>("Skill3Icon");
                //icon30TearGasScepter = MainAssetBundle.LoadAsset<Sprite>("Skill3Icon");
                icon31StunGrenade = MainAssetBundle.LoadAsset<Sprite>("Skill3BIcon");
                //icon31StunGrenadeScepter = MainAssetBundle.LoadAsset<Sprite>("Skill3BIcon");
                icon40Shield = MainAssetBundle.LoadAsset<Sprite>("Skill4Icon");
                icon40ShieldOff = MainAssetBundle.LoadAsset<Sprite>("Skill4BIcon");
            }

            icon42SkateBoard = MainAssetBundle.LoadAsset<Sprite>("SkamteOffIcon");
            icon42SkateBoardOff = MainAssetBundle.LoadAsset<Sprite>("SkamteOnIcon");

            testIcon = MainAssetBundle.LoadAsset<Sprite>("TestIcon");

            nemIconPassive = MainAssetBundle.LoadAsset<Sprite>("PassiveIcon");
            nIcon1 = MainAssetBundle.LoadAsset<Sprite>("HammerSwingIcon");
            nIcon1B = MainAssetBundle.LoadAsset<Sprite>("MinigunFireIcon");
            nIcon1C = MainAssetBundle.LoadAsset<Sprite>("HammerThrowIcon");
            nIcon2 = MainAssetBundle.LoadAsset<Sprite>("HammerChargeIcon");
            nIcon2B = MainAssetBundle.LoadAsset<Sprite>("HammerSlamIcon");
            nIcon3 = MainAssetBundle.LoadAsset<Sprite>("NemGasGrenadeIcon");
            nIcon3B = MainAssetBundle.LoadAsset<Sprite>("NemStunGrenadeIcon");
            nIcon3C = MainAssetBundle.LoadAsset<Sprite>("HeatCrashIcon");
            nIcon4 = MainAssetBundle.LoadAsset<Sprite>("MinigunStanceIcon");
            nIcon4B = MainAssetBundle.LoadAsset<Sprite>("HammerStanceIcon");

            nemesisHammer = MainAssetBundle.LoadAsset<GameObject>("NemesisHammer");
            nemesisHammer.GetComponentInChildren<MeshRenderer>().material.shader = hotpoo;

            tearGasGrenadeModel = MainAssetBundle.LoadAsset<GameObject>("TearGasGrenade");
            tearGasEffectPrefab = MainAssetBundle.LoadAsset<GameObject>("TearGasEffect");

            tearGasGrenadeModel.GetComponentInChildren<MeshRenderer>().material.shader = hotpoo;
            tearGasEffectPrefab.GetComponentInChildren<MeshRenderer>().material.shader = hotpoo;

            tearGasEffectPrefab.GetComponentInChildren<Rigidbody>().gameObject.layer = LayerIndex.debris.intVal;

            tearGasGrenadeModelAlt = MainAssetBundle.LoadAsset<GameObject>("TearGasGrenadeAlt");
            tearGasEffectPrefabAlt = MainAssetBundle.LoadAsset<GameObject>("TearGasEffectAlt");

            tearGasGrenadeModelAlt.GetComponentInChildren<MeshRenderer>().material.shader = hotpoo;
            tearGasEffectPrefabAlt.GetComponentInChildren<MeshRenderer>().material.shader = hotpoo;

            tearGasEffectPrefabAlt.GetComponentInChildren<Rigidbody>().gameObject.layer = LayerIndex.debris.intVal;

            nemGasGrenadeModel = MainAssetBundle.LoadAsset<GameObject>("NemGasGrenade");
            nemGasEffectPrefab = MainAssetBundle.LoadAsset<GameObject>("NemGasEffect");

            Material tempMat = nemGasGrenadeModel.GetComponentInChildren<MeshRenderer>().material;
            tempMat.shader = hotpoo;
            tempMat.SetFloat("_EmPower", 50f);
            tempMat.SetTexture("_EmTex", MainAssetBundle.LoadAsset<Material>("matNemGrenade").GetTexture("_EmissionMap"));
            nemGasEffectPrefab.GetComponentInChildren<MeshRenderer>().material.shader = hotpoo;

            nemGasEffectPrefab.GetComponentInChildren<Rigidbody>().gameObject.layer = LayerIndex.debris.intVal;

            stunGrenadeModel = MainAssetBundle.LoadAsset<GameObject>("StunGrenade");
            stunGrenadeModel.GetComponentInChildren<MeshRenderer>().material.shader = hotpoo;
            stunGrenadeModel.transform.GetChild(0).GetChild(0).Find("Smoke").gameObject.AddComponent<ParticleFollowerController>();

            stunGrenadeModelAlt = MainAssetBundle.LoadAsset<GameObject>("ShockGrenade");
            //replace the texture here bc the emission is important
            MeshRenderer shockGrenadeMesh = stunGrenadeModelAlt.GetComponentInChildren<MeshRenderer>();
            Material shockGrenadeMaterial = UnityEngine.Object.Instantiate<Material>(Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial);
            shockGrenadeMaterial.SetColor("_Color", Color.white);
            shockGrenadeMaterial.SetTexture("_MainTex", Assets.MainAssetBundle.LoadAsset<Material>("matShockGrenade").GetTexture("_MainTex"));
            shockGrenadeMaterial.SetColor("_EmColor", Color.white);
            shockGrenadeMaterial.SetFloat("_EmPower", 30f);
            shockGrenadeMaterial.SetTexture("_EmTex", Assets.MainAssetBundle.LoadAsset<Material>("matShockGrenade").GetTexture("_EmissionMap"));
            shockGrenadeMaterial.SetFloat("_NormalStrength", 0);

            shockGrenadeMesh.material = shockGrenadeMaterial;

            hammerProjectileModel = MainAssetBundle.LoadAsset<GameObject>("HammerProjectile");
            hammerProjectileModel.GetComponentInChildren<MeshRenderer>().material.shader = hotpoo;

            gordoProjectileModel = MainAssetBundle.LoadAsset<GameObject>("HammerProjectileGordo");
            //haven't used transform.find since i was like in college damn
            gordoProjectileModel.transform.Find("model/Gordo/polygon0").GetComponent<MeshRenderer>().material.shader = hotpoo;
            gordoProjectileModel.transform.Find("model/Gordo/polygon1").GetComponent<MeshRenderer>().material.shader = hotpoo;

            shotgunShell = MainAssetBundle.LoadAsset<GameObject>("ShotgunShell");
            superShotgunShell = MainAssetBundle.LoadAsset<GameObject>("SuperShotgunShell");

            shotgunShell.AddComponent<Enforcer.EnforcerShellController>();
            superShotgunShell.AddComponent<Enforcer.EnforcerShellController>();

            shieldBashFX = Assets.LoadEffect("ShieldBashFX", "", MainAssetBundle);
            shoulderBashFX = Assets.LoadEffect("ShoulderBashFX", "", MainAssetBundle);

            shieldBashFX.transform.Find("cum").Find("piss").gameObject.AddComponent<ParticleFuckingShitComponent>();

            shoulderBashFX.transform.Find("cum").Find("poop").gameObject.AddComponent<ParticleFuckingShitComponent>();

            hammerSwingFX = Assets.LoadEffect("EnforcerSwing", "", MainAssetBundle);
            hammerImpactFX = Assets.LoadEffect("ImpactEnforcer", "", MainAssetBundle);

            nemSwingFX = Assets.LoadEffect("NemforcerSwing", "", MainAssetBundle);
            nemUppercutSwingFX = Assets.LoadEffect("NemforcerSwingUppercut", "", MainAssetBundle);
            nemSlamSwingFX = Assets.LoadEffect("NemforcerSwingSlam", "", MainAssetBundle);
            nemSlamDownFX = Assets.LoadEffect("HammerAirSlamFX", "", MainAssetBundle);
            nemSlamDownFX.transform.GetChild(0).localScale /= 7f;
            nemSlamDownFX.transform.GetChild(0).localPosition = Vector3.zero;
            nemDashFX = Assets.LoadEffect("HammerDashFX", "", MainAssetBundle);
            nemDashFX.transform.GetChild(0).localScale /= 7f;
            nemDashFX.transform.GetChild(0).localPosition = Vector3.zero;
            nemImpactFX = Assets.LoadEffect("ImpactNemforcer", "", MainAssetBundle);
            nemHeavyImpactFX = Assets.LoadEffect("HeavyImpactNemforcer", "", MainAssetBundle);
            nemAxeImpactFX = Assets.LoadEffect("ImpactNemforcerAxe", "", MainAssetBundle);
            nemAxeImpactFXVertical = Assets.LoadEffect("ImpactNemforcerAxe_Vertical", "", MainAssetBundle);

            gatDrone = MainAssetBundle.LoadAsset<GameObject>("GatDrone");

            mainMat = MainAssetBundle.LoadAsset<Material>("matEnforcerAlt");

            //gmMesh = MainAssetBundle.LoadAsset<Mesh>("meshEnforcerGM");
            //stormtrooperMesh = MainAssetBundle.LoadAsset<Mesh>("StormtrooperMesh");
            //engiMesh = MainAssetBundle.LoadAsset<Mesh>("EngiforcerMesh");
            //desperadoMesh = MainAssetBundle.LoadAsset<Mesh>("EnforcerMesh");
            //zeroSuitMesh = MainAssetBundle.LoadAsset<Mesh>("ZeroSuitMesh");
            //classicMesh = MainAssetBundle.LoadAsset<Mesh>("EnforcerMesh");
            //sexMesh = MainAssetBundle.LoadAsset<Mesh>("SexforcerMesh");
            //femMesh = MainAssetBundle.LoadAsset<Mesh>("FemforcerMesh");
            //fuckingSteveMesh = MainAssetBundle.LoadAsset<Mesh>("FuckingSteveMesh");

            nemClassicMesh = MainAssetBundle.LoadAsset<Mesh>("meshNemforcerClassic");
            nemClassicHammerMesh = MainAssetBundle.LoadAsset<Mesh>("MeshClassicHammer");
            nemMeshGM = MainAssetBundle.LoadAsset<Mesh>("meshNemforcerGM");
            nemHammerMeshGM = MainAssetBundle.LoadAsset<Mesh>("meshHammerGM");
            nemAltMesh = MainAssetBundle.LoadAsset<Mesh>("MeshNemforcerAlt");
            nemDripMesh = MainAssetBundle.LoadAsset<Mesh>("MeshDripforcer");
            nemDripHammerMesh = MainAssetBundle.LoadAsset<Mesh>("MeshDripforcerHammer");
            dededeMesh = MainAssetBundle.LoadAsset<Mesh>("meshDedede");
            dededeHammerMesh = MainAssetBundle.LoadAsset<Mesh>("meshDededeHammer");
            dededeBossMesh = MainAssetBundle.LoadAsset<Mesh>("meshDededeBoss");
            minecraftNemMesh = MainAssetBundle.LoadAsset<Mesh>("meshMinecraftNem");
            minecraftHammerMesh = MainAssetBundle.LoadAsset<Mesh>("meshMinecraftHammer");

            hammerHitSoundEvent = CreateNetworkSoundEventDef(Sounds.NemesisImpact);
            nemHammerHitSoundEvent = CreateNetworkSoundEventDef(Sounds.NemesisImpact2);
            nemAxeHitSoundEvent = CreateNetworkSoundEventDef(Sounds.NemesisImpactAxe);
        }

        internal static NetworkSoundEventDef CreateNetworkSoundEventDef(string eventName)
        {
            NetworkSoundEventDef networkSoundEventDef = ScriptableObject.CreateInstance<NetworkSoundEventDef>();
            networkSoundEventDef.akId = AkSoundEngine.GetIDFromString(eventName);
            networkSoundEventDef.eventName = eventName;

            networkSoundEventDefs.Add(networkSoundEventDef);

            return networkSoundEventDef;
        }

        private static GameObject LoadEffect(string resourceName, string soundName, AssetBundle bundle)
        {
            GameObject newEffect = bundle.LoadAsset<GameObject>(resourceName);

            newEffect.AddComponent<DestroyOnTimer>().duration = 12;
            newEffect.AddComponent<NetworkIdentity>();
            newEffect.AddComponent<VFXAttributes>().vfxPriority = VFXAttributes.VFXPriority.Always;
            var effect = newEffect.AddComponent<EffectComponent>();
            effect.applyScale = false;
            effect.effectIndex = EffectIndex.Invalid;
            effect.parentToReferencedTransform = true;
            effect.positionAtReferencedTransform = true;
            effect.soundName = soundName;

            Modules.Effects.AddEffect(newEffect);

            return newEffect;
        }
        public static Material CreateMaterial(string materialName) {

            return CreateMaterial(Assets.MainAssetBundle, materialName, 0, Color.black, 0);
        }

        public static Material CreateMaterial(string materialName, float emission, Color emissionColor, float normalStrength) {
            return CreateMaterial(Assets.MainAssetBundle, materialName, emission, emissionColor, normalStrength);
        }

        public static Material CreateMaterial(AssetBundle assetbundle, string materialName, float emission, Color emissionColor, float normalStrength)
        {
            if (!commandoMat) commandoMat = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial;

            Material tempMat = assetbundle.LoadAsset<Material>(materialName);
            if (!tempMat) {
                return commandoMat;
            }

            Material mat = UnityEngine.Object.Instantiate<Material>(commandoMat);
            mat.name = materialName;

            mat.SetColor("_Color", tempMat.GetColor("_Color"));
            mat.SetTexture("_MainTex", tempMat.GetTexture("_MainTex"));
            mat.SetColor("_EmColor", emissionColor);
            mat.SetFloat("_EmPower", emission);
            mat.SetTexture("_EmTex", tempMat.GetTexture("_EmissionMap"));
            mat.SetFloat("_NormalStrength", normalStrength);

            return mat;
        }

        public static Material CreateNemMaterial(string materialName)
        {
            return CreateMaterial(Assets.MainAssetBundle, materialName, 0, Color.black, 0);
        }

        public static Material CreateNemMaterial(string materialName, float emission, Color emissionColor, float normalStrength) {

            return CreateMaterial(Assets.MainAssetBundle, materialName, emission, emissionColor, normalStrength);
        }
    }

    public static class Sounds {
        public static readonly string CharSelect = "Play_Enforcer_CharSelect";

        public static readonly string FireShotgun = "Play_RiotShotgun_shoot";
        public static readonly string FireShotgunCrit = "Play_RiotShotgun_Crit";
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