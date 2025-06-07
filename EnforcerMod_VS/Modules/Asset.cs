﻿using System.Reflection;
using R2API;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using RoR2;
using RoR2.Audio;
using System.Collections.Generic;
using System.Linq;
using System;
using EnforcerPlugin;
using UnityEngine.AddressableAssets;

namespace Modules {

    internal static class Asset {

        public static AssetBundle MainAssetBundle = null;
        public static AssetBundle VRAssetBundle = null;

        internal static Shader hotpoo;
        #region why did we do it this way
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

        public static GameObject vrEnforcerDominantHand; //Shotgun hand
        public static GameObject vrEnforcerNonDominantHand; //Shield hand
        public static GameObject vrNemforcerDominantHand; //Shotgun hand
        public static GameObject vrNemforcerNonDominantHand; //Shield hand

        public static Material mainMat;

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
        public static Mesh femNemMesh;
        public static Mesh femHammerMesh;

        #endregion why did we do it this way
        internal static NetworkSoundEventDef hammerHitSoundEvent;
        internal static NetworkSoundEventDef nemHammerHitSoundEvent;
        internal static NetworkSoundEventDef nemAxeHitSoundEvent;

        public static void Initialize() {

            hotpoo = Addressables.LoadAssetAsync<Shader>(RoR2BepInExPack.GameAssetPaths.RoR2_Base_Shaders.HGStandard_shader).WaitForCompletion();

            PopulateBundles();

            PopulateAssets();//and boy are there assets
        }

        private static void PopulateBundles() {

            if (MainAssetBundle == null) {
                MainAssetBundle = AssetBundle.LoadFromFile(Files.GetPathToFile("AssetBundles", "enforcer"));
            }

            ////fuck whoever wrote this code and fuck you
            //// comment out the soundbank shit and then wonder why sounds aren't working you're literally fucking retarded holy hell
            //using (Stream manifestResourceStream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream("Enforcer.EnforcerBank.bnk")) {
            //    byte[] array = new byte[manifestResourceStream2.Length];
            //    manifestResourceStream2.Read(array, 0, array.Length);
            //    SoundAPI.SoundBanks.Add(array);
            //}

            //// it's 2022 and I just fucking did it again
            //using (Stream manifestResourceStream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream("Enforcer.Nemforcer.bnk")) {
            //    byte[] array = new byte[manifestResourceStream2.Length];
            //    manifestResourceStream2.Read(array, 0, array.Length);
            //    SoundAPI.SoundBanks.Add(array);
            //};
            //deleting these memories is punishable by cum
        }

        private static void PopulateAssets() {
            string portrait = UnityEngine.Random.value > 0.05f ? "texEnforcerIcon" : "texEnforcerIconEpic";
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
            Material shockGrenadeMaterial = new Material(hotpoo);
            shockGrenadeMaterial.SetColor("_Color", Color.white);
            shockGrenadeMaterial.SetTexture("_MainTex", MainAssetBundle.LoadAsset<Material>("matShockGrenade").GetTexture("_MainTex"));
            shockGrenadeMaterial.SetColor("_EmColor", Color.white);
            shockGrenadeMaterial.SetFloat("_EmPower", 30f);
            shockGrenadeMaterial.SetTexture("_EmTex", MainAssetBundle.LoadAsset<Material>("matShockGrenade").GetTexture("_EmissionMap"));
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

            shotgunShell.AddComponent<Enforcer.ShotgunShellController>();
            superShotgunShell.AddComponent<Enforcer.ShotgunShellController>();

            shieldBashFX = LoadEffect("ShieldBashFX", "", MainAssetBundle);
            shoulderBashFX = LoadEffect("ShoulderBashFX", "", MainAssetBundle);

            shieldBashFX.transform.Find("cum").Find("piss").gameObject.AddComponent<ParticleFuckingShitComponent>();

            shoulderBashFX.transform.Find("cum").Find("poop").gameObject.AddComponent<ParticleFuckingShitComponent>();

            hammerSwingFX = LoadEffect("EnforcerSwing", "", MainAssetBundle);
            hammerImpactFX = LoadEffect("ImpactEnforcer", "", MainAssetBundle);

            nemSwingFX = LoadEffect("NemforcerSwing", "", MainAssetBundle);
            nemUppercutSwingFX = LoadEffect("NemforcerSwingUppercut", "", MainAssetBundle);
            nemSlamSwingFX = LoadEffect("NemforcerSwingSlam", "", MainAssetBundle);
            nemSlamDownFX = LoadEffect("HammerAirSlamFX", "", MainAssetBundle);
            nemSlamDownFX.transform.GetChild(0).localScale /= 7f;
            nemSlamDownFX.transform.GetChild(0).localPosition = Vector3.zero;
            nemDashFX = LoadEffect("HammerDashFX", "", MainAssetBundle);
            nemDashFX.transform.GetChild(0).localScale /= 7f;
            nemDashFX.transform.GetChild(0).localPosition = Vector3.zero;
            nemImpactFX = LoadEffect("ImpactNemforcer", "", MainAssetBundle);
            nemHeavyImpactFX = LoadEffect("HeavyImpactNemforcer", "", MainAssetBundle);
            nemAxeImpactFX = LoadEffect("ImpactNemforcerAxe", "", MainAssetBundle);
            nemAxeImpactFXVertical = LoadEffect("ImpactNemforcerAxe_Vertical", "", MainAssetBundle);

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
            femNemMesh = MainAssetBundle.LoadAsset<Mesh>("meshNemforcerFemBody");
            femHammerMesh = MainAssetBundle.LoadAsset<Mesh>("meshNemforcerFemHammer");

            hammerHitSoundEvent = CreateNetworkSoundEventDef(Sounds.NemesisImpact);
            nemHammerHitSoundEvent = CreateNetworkSoundEventDef(Sounds.NemesisImpact2);
            nemAxeHitSoundEvent = CreateNetworkSoundEventDef(Sounds.NemesisImpactAxe);
        }

        public static void loadVRBundle() {

            if (VRAssetBundle == null) {
                VRAssetBundle = AssetBundle.LoadFromFile(Files.GetPathToFile("AssetBundles", "enforcervr"));
            }

            vrEnforcerDominantHand = VRAssetBundle.LoadAsset<GameObject>("EnforcerShotgunHand").ConvertAllRenderersToHopooShader();
            vrEnforcerNonDominantHand = VRAssetBundle.LoadAsset<GameObject>("EnforcerShieldHand");

            //keep sex shield glass out of hopoo shader-izing
            //one day I'll learn nice hopoo shader transparency
            Renderer sexGlassRenderer = vrEnforcerNonDominantHand.transform.Find("Visuals/Enfucker_VR_LeftHand_Armature /Shields/meshSexforcerShield/meshShieldGlass")?.GetComponent<Renderer>();
            Material sexGlassMat = sexGlassRenderer?.material;

            vrEnforcerNonDominantHand.ConvertAllRenderersToHopooShader();
            
            if (sexGlassMat) sexGlassRenderer.material = sexGlassMat;

            vrNemforcerDominantHand = VRAssetBundle.LoadAsset<GameObject>("NemforcerHammerHand").ConvertAllRenderersToHopooShader();
            vrNemforcerNonDominantHand = VRAssetBundle.LoadAsset<GameObject>("NemforcerNonDomHand").ConvertAllRenderersToHopooShader();
        }

        #region helpers

        internal static Sprite LoadBuffSprite(string path) {
            return Addressables.LoadAssetAsync<BuffDef>(path).WaitForCompletion().iconSprite;
        }

        public static GameObject ConvertAllRenderersToHopooShader(this GameObject objectToConvert) {
            if (!objectToConvert) return objectToConvert;

            foreach (MeshRenderer i in objectToConvert.GetComponentsInChildren<MeshRenderer>()) {
                if (i?.sharedMaterial != null) {
                    i.sharedMaterial.SetHotpooMaterial();
                }
            }

            foreach (SkinnedMeshRenderer i in objectToConvert.GetComponentsInChildren<SkinnedMeshRenderer>()) {
                if (i?.sharedMaterial != null) {
                    i.sharedMaterial.SetHotpooMaterial();
                }
            }

            return objectToConvert;
        }

        internal static NetworkSoundEventDef CreateNetworkSoundEventDef(string eventName) {
            NetworkSoundEventDef networkSoundEventDef = ScriptableObject.CreateInstance<NetworkSoundEventDef>();
            networkSoundEventDef.akId = AkSoundEngine.GetIDFromString(eventName);
            networkSoundEventDef.eventName = eventName;

            Modules.Content.AddNetworkSoundEventDef(networkSoundEventDef);

            return networkSoundEventDef;
        }

        public static T LoadAsset<T>(string name) where T : UnityEngine.Object {
            //handle dynamically loading from additional bundle if we want to here
            return MainAssetBundle.LoadAsset<T>(name);
        }

        private static GameObject LoadEffect(string resourceName, string soundName, AssetBundle bundle) {
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

        public static GameObject LoadCrosshair(string crosshairName) {
            if (Asset.LoadAsset<GameObject>("Prefabs/Crosshair/" + crosshairName + "Crosshair") == null) return Asset.LoadAsset<GameObject>("Prefabs/Crosshair/StandardCrosshair");
            return Asset.LoadAsset<GameObject>("Prefabs/Crosshair/" + crosshairName + "Crosshair");
        }

        internal static GameObject LoadSurvivorModel(string modelName) {
            GameObject model = Modules.Asset.LoadAsset<GameObject>(modelName);
            if (model == null) {
                Debug.LogError("Trying to load a null model- check to see if the name in your code matches the name of the object in Unity");
                return null;
            }

            return PrefabAPI.InstantiateClone(model, model.name, false);
        }

        #region materials(old)
        private const string obsolete = "use `Materials.CreateMaterial` instead, or use the extension `Material.SetHotpooMaterial` directly on a material";
        [Obsolete(obsolete)]
        public static Material CreateMaterial(string materialName) => Materials.CreateHotpooMaterial(materialName);
        [Obsolete(obsolete)]
        public static Material CreateMaterial(string materialName, float emission) => Materials.CreateHotpooMaterial(materialName);
        [Obsolete(obsolete)]
        public static Material CreateMaterial(string materialName, float emission, Color emissionColor) => CreateMaterial(materialName, emission, emissionColor, 0f);
        [Obsolete(obsolete)]
        public static Material CreateMaterial(string materialName, float emission, Color emissionColor, float normalStrength) {
            return Materials.CreateHotpooMaterial(materialName)
                            .MakeUnique()
                            .SetEmission(emission, emissionColor)
                            .SetNormal(normalStrength);
        }
        #endregion materials(old)

#endregion helpers

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
        public static readonly string FireBlasterSSG = "Blaster_SSG";
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
        public static readonly string NemesisMinigunWindDown = "Play_minigun_wind_down";   //unused
        public static readonly string NemesisMinigunWindUp = "Play_minigun_wind_up";       //unused
        public static readonly string NemesisMinigunShooting = "Play_Minigun_Shoot";       //unused

        public static readonly string NemesisMinigunSpinUp = "NemforcerMinigunSpinUp";
        public static readonly string NemesisMinigunSpinDown = "NemforcerMinigunSpinDown";
        public static readonly string NemesisMinigunLoop = "Play_Minigun_Shoot2";

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