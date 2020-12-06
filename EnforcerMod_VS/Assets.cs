using System.Reflection;
using R2API;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using RoR2;

namespace EnforcerPlugin
{
    public static class Assets
    {
        public static AssetBundle MainAssetBundle = null;
        public static AssetBundle NemAssetBundle = null;

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
        public static GameObject nemImpactFX;
        public static GameObject nemHeavyImpactFX;

        public static GameObject gatDrone;

        public static Material mainMat;

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

        public static void PopulateAssets()
        {
            if (MainAssetBundle == null)
            {
                using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Enforcer.enforcer"))
                {
                    MainAssetBundle = AssetBundle.LoadFromStream(assetStream);
                    var provider = new AssetBundleResourcesProvider("@Enforcer", MainAssetBundle);
                    ResourcesAPI.AddProvider(provider);
                }
            }

            if (NemAssetBundle == null)
            {
                using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Enforcer.nemforcer"))
                {
                    NemAssetBundle = AssetBundle.LoadFromStream(assetStream);
                    var provider = new AssetBundleResourcesProvider("@Nemforcer", NemAssetBundle);
                    ResourcesAPI.AddProvider(provider);
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

            Shader hotpoo = Resources.Load<Shader>("Shaders/Deferred/hgstandard");

            charPortrait = MainAssetBundle.LoadAsset<Sprite>("texEnforcerIcon").texture;
            nemCharPortrait = NemAssetBundle.LoadAsset<Sprite>("nemIconBlu").texture;
            nemBossPortrait = NemAssetBundle.LoadAsset<Sprite>("nemIconRed").texture;

            if (EnforcerPlugin.classicIcons.Value)
            {
                iconP = MainAssetBundle.LoadAsset<Sprite>("TestIcon");
                icon1 = MainAssetBundle.LoadAsset<Sprite>("Skill1Icon");
                icon1B = MainAssetBundle.LoadAsset<Sprite>("Skill1Icon");
                icon1C = MainAssetBundle.LoadAsset<Sprite>("Skill1Icon");
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

            nIconP = NemAssetBundle.LoadAsset<Sprite>("PassiveIcon");
            nIcon1 = NemAssetBundle.LoadAsset<Sprite>("HammerSwingIcon");
            nIcon1B = NemAssetBundle.LoadAsset<Sprite>("MinigunFireIcon");
            nIcon1C = NemAssetBundle.LoadAsset<Sprite>("HammerThrowIcon");
            nIcon2 = NemAssetBundle.LoadAsset<Sprite>("HammerChargeIcon");
            nIcon2B = NemAssetBundle.LoadAsset<Sprite>("HammerSlamIcon");
            nIcon3 = NemAssetBundle.LoadAsset<Sprite>("GasGrenadeIcon");
            nIcon4 = NemAssetBundle.LoadAsset<Sprite>("MinigunStanceIcon");
            nIcon4B = NemAssetBundle.LoadAsset<Sprite>("HammerStanceIcon");

            nemesisHammer = NemAssetBundle.LoadAsset<GameObject>("NemesisHammer");
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

            nemGasGrenadeModel = NemAssetBundle.LoadAsset<GameObject>("NemGasGrenade");
            nemGasEffectPrefab = NemAssetBundle.LoadAsset<GameObject>("NemGasEffect");

            Material tempMat = nemGasGrenadeModel.GetComponentInChildren<MeshRenderer>().material;
            tempMat.shader = hotpoo;
            tempMat.SetFloat("_EmPower", 50f);
            tempMat.SetTexture("_EmTex", NemAssetBundle.LoadAsset<Material>("matNemGrenade").GetTexture("_EmissionMap"));
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

            hammerProjectileModel = NemAssetBundle.LoadAsset<GameObject>("HammerProjectile");

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

            nemSwingFX = Assets.LoadEffect("NemforcerSwing", "", NemAssetBundle);
            nemUppercutSwingFX = Assets.LoadEffect("NemforcerSwingUppercut", "", NemAssetBundle);
            nemSlamSwingFX = Assets.LoadEffect("NemforcerSwingSlam", "", NemAssetBundle);
            nemImpactFX = Assets.LoadEffect("ImpactNemforcer", "", NemAssetBundle);
            nemHeavyImpactFX = Assets.LoadEffect("HeavyImpactNemforcer", "", NemAssetBundle);

            gatDrone = MainAssetBundle.LoadAsset<GameObject>("GatDrone");

            mainMat = MainAssetBundle.LoadAsset<Material>("matEnforcerAlt");

            stormtrooperMesh = MainAssetBundle.LoadAsset<Mesh>("StormtrooperMesh");
            engiMesh = MainAssetBundle.LoadAsset<Mesh>("EngiforcerMesh");
            desperadoMesh = MainAssetBundle.LoadAsset<Mesh>("EnforcerMesh");
            zeroSuitMesh = MainAssetBundle.LoadAsset<Mesh>("ZeroSuitMesh");
            classicMesh = MainAssetBundle.LoadAsset<Mesh>("EnforcerMesh");
            sexMesh = MainAssetBundle.LoadAsset<Mesh>("SexforcerMesh");
            femMesh = MainAssetBundle.LoadAsset<Mesh>("FemforcerMesh");
            fuckingSteveMesh = MainAssetBundle.LoadAsset<Mesh>("FuckingSteveMesh");

            nemClassicMesh = NemAssetBundle.LoadAsset<Mesh>("MeshClassic");
            nemClassicHammerMesh = NemAssetBundle.LoadAsset<Mesh>("MeshClassicHammer");
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

            EffectAPI.AddEffect(newEffect);

            return newEffect;
        }
    }
}