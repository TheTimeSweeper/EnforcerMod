using System.Reflection;
using R2API;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using RoR2;
using RoR2.Audio;
using System.Collections.Generic;

namespace EnforcerPlugin
{
    public static class Assets
    {
        public static AssetBundle MainAssetBundle = null;

        public static Material commandoMat;

        public static Texture charPortrait;

        public static Texture nemCharPortrait;
        public static Texture nemBossPortrait;

        public static Sprite iconP;
        public static Sprite icon1;//shotgun
        public static Sprite icon1B;//super shotgun
        public static Sprite icon1C;//assault rifle
        public static Sprite icon1D;//hammer
        public static Sprite icon2;//shield bash
        public static Sprite icon3;//tear gas
        public static Sprite icon3S;//tear gas(scepter)
        public static Sprite icon3B;//stun grenade
        public static Sprite icon3BS;//stun grenade(scepter)
        public static Sprite icon4;//protect and serve
        public static Sprite icon4B;//protect and serve cancel
        public static Sprite icon4C;//skateboard
        public static Sprite icon4D;//skateboard cancel

        public static Sprite nIconP;
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
        public static Mesh sneedMesh;
        public static Mesh sneedHammerMesh;
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

            charPortrait = MainAssetBundle.LoadAsset<Sprite>("texEnforcerIcon").texture;
            nemCharPortrait = MainAssetBundle.LoadAsset<Sprite>("nemIconBlu").texture;
            nemBossPortrait = MainAssetBundle.LoadAsset<Sprite>("nemIconRed").texture;

            if (EnforcerPlugin.classicIcons.Value)
            {
                iconP = MainAssetBundle.LoadAsset<Sprite>("TestIcon");
                icon1 = MainAssetBundle.LoadAsset<Sprite>("Skill1Icon");
                icon1B = MainAssetBundle.LoadAsset<Sprite>("Skill1Icon");
                icon1C = MainAssetBundle.LoadAsset<Sprite>("Skill1Icon");
                icon1D = MainAssetBundle.LoadAsset<Sprite>("Skill1Icon");
                icon2 = MainAssetBundle.LoadAsset<Sprite>("Skill2Icon");
                icon3 = MainAssetBundle.LoadAsset<Sprite>("Skill3Icon");
                icon3S = MainAssetBundle.LoadAsset<Sprite>("Skill3Icon");
                icon3B = MainAssetBundle.LoadAsset<Sprite>("Skill3BIcon");
                icon3BS = MainAssetBundle.LoadAsset<Sprite>("Skill3BIcon");
                icon4 = MainAssetBundle.LoadAsset<Sprite>("Skill4Icon");
                icon4B = MainAssetBundle.LoadAsset<Sprite>("Skill4BIcon");
            }
            else
            {
                iconP = MainAssetBundle.LoadAsset<Sprite>("TestIcon");
                icon1 = MainAssetBundle.LoadAsset<Sprite>("RiotShotgunIcon");
                icon1B = MainAssetBundle.LoadAsset<Sprite>("SuperShotgunIcon");
                icon1C = MainAssetBundle.LoadAsset<Sprite>("AssaultRifleIcon");
                icon1D = MainAssetBundle.LoadAsset<Sprite>("BreachingHammerIcon");
                icon2 = MainAssetBundle.LoadAsset<Sprite>("ShieldBashIcon");
                icon3 = MainAssetBundle.LoadAsset<Sprite>("TearGasIcon");
                icon3S = MainAssetBundle.LoadAsset<Sprite>("TearGasScepterIcon");
                icon3B = MainAssetBundle.LoadAsset<Sprite>("StunGrenadeIcon");
                icon3BS = MainAssetBundle.LoadAsset<Sprite>("StunGrenadeScepterIcon");
                icon4 = MainAssetBundle.LoadAsset<Sprite>("ShieldUpIcon");
                icon4B = MainAssetBundle.LoadAsset<Sprite>("ShieldDownIcon");
            }

            icon4C = MainAssetBundle.LoadAsset<Sprite>("SkamteOffIcon");
            icon4D = MainAssetBundle.LoadAsset<Sprite>("SkamteOnIcon");

            testIcon = MainAssetBundle.LoadAsset<Sprite>("TestIcon");

            nIconP = MainAssetBundle.LoadAsset<Sprite>("PassiveIcon");
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

            gatDrone = MainAssetBundle.LoadAsset<GameObject>("GatDrone");

            mainMat = MainAssetBundle.LoadAsset<Material>("matEnforcerAlt");

            gmMesh = MainAssetBundle.LoadAsset<Mesh>("meshEnforcerGM");
            stormtrooperMesh = MainAssetBundle.LoadAsset<Mesh>("StormtrooperMesh");
            engiMesh = MainAssetBundle.LoadAsset<Mesh>("EngiforcerMesh");
            desperadoMesh = MainAssetBundle.LoadAsset<Mesh>("EnforcerMesh");
            zeroSuitMesh = MainAssetBundle.LoadAsset<Mesh>("ZeroSuitMesh");
            classicMesh = MainAssetBundle.LoadAsset<Mesh>("EnforcerMesh");
            sexMesh = MainAssetBundle.LoadAsset<Mesh>("SexforcerMesh");
            femMesh = MainAssetBundle.LoadAsset<Mesh>("FemforcerMesh");
            fuckingSteveMesh = MainAssetBundle.LoadAsset<Mesh>("FuckingSteveMesh");

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
            sneedMesh = MainAssetBundle.LoadAsset<Mesh>("meshSneed");
            sneedHammerMesh = MainAssetBundle.LoadAsset<Mesh>("meshMallet");
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

        public static Material CreateMaterial(string materialName, float emission, Color emissionColor, float normalStrength)
        {
            if (!commandoMat) commandoMat = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial;

            Material tempMat = Assets.MainAssetBundle.LoadAsset<Material>(materialName);
            if (!tempMat)
            {
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
            return CreateNemMaterial(materialName, 0, Color.black, 0);
        }

        public static Material CreateNemMaterial(string materialName, float emission, Color emissionColor, float normalStrength)
        {
            if (!commandoMat) commandoMat = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial;

            Material tempMat = Assets.MainAssetBundle.LoadAsset<Material>(materialName);
            if (!tempMat)
            {
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
    }
}