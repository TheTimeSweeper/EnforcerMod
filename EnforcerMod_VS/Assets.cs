using System.Reflection;
using R2API;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using RoR2;

namespace EnforcerPlugin {
    public static class Assets
    {
        public static AssetBundle MainAssetBundle = null;

        public static Texture charPortrait;

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

        public static Sprite testIcon;// for wip skills

        public static GameObject tearGasGrenadeModel;
        public static GameObject tearGasEffectPrefab;

        public static GameObject tearGasGrenadeModelAlt;
        public static GameObject tearGasEffectPrefabAlt;

        public static GameObject stunGrenadeModel;

        public static GameObject stunGrenadeModelAlt;

        public static GameObject shotgunShell;
        public static GameObject superShotgunShell;

        public static GameObject shieldBashFX;
        public static GameObject shoulderBashFX;

        public static GameObject gatDrone;

        public static Mesh stormtrooperMesh;
        public static Mesh engiMesh;
        public static Mesh desperadoMesh;
        public static Mesh zeroSuitMesh;
        public static Mesh classicMesh;
        public static Mesh sexMesh;
        public static Mesh femMesh;

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

            //fuck whoever wrote this code and fuck you
            // comment out the soundbank shit and then wonder why sounds aren't working you're literally fucking retarded holy hell
            using (Stream manifestResourceStream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream("Enforcer.EnforcerBank.bnk"))
            {
                byte[] array = new byte[manifestResourceStream2.Length];
                manifestResourceStream2.Read(array, 0, array.Length);
                SoundAPI.SoundBanks.Add(array);
            }

            charPortrait = MainAssetBundle.LoadAsset<Sprite>("texEnforcerIcon").texture;

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

            testIcon = MainAssetBundle.LoadAsset<Sprite>("TestIcon");

            tearGasGrenadeModel = MainAssetBundle.LoadAsset<GameObject>("TearGasGrenade");
            tearGasEffectPrefab = MainAssetBundle.LoadAsset<GameObject>("TearGasEffect");

            tearGasGrenadeModelAlt = MainAssetBundle.LoadAsset<GameObject>("TearGasGrenadeAlt");
            tearGasEffectPrefabAlt = MainAssetBundle.LoadAsset<GameObject>("TearGasEffectAlt");

            stunGrenadeModel = MainAssetBundle.LoadAsset<GameObject>("StunGrenade");

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

            shotgunShell = MainAssetBundle.LoadAsset<GameObject>("ShellController");
            superShotgunShell = MainAssetBundle.LoadAsset<GameObject>("SuperShellController");

            shieldBashFX = MainAssetBundle.LoadAsset<GameObject>("ShieldBashFX");
            shoulderBashFX = MainAssetBundle.LoadAsset<GameObject>("ShoulderBashFX");

            shieldBashFX.AddComponent<DestroyOnTimer>().duration = 5;
            shieldBashFX.AddComponent<NetworkIdentity>();
            shieldBashFX.AddComponent<VFXAttributes>().vfxPriority = VFXAttributes.VFXPriority.Always;
            var effect = shieldBashFX.AddComponent<EffectComponent>();
            effect.applyScale = false;
            effect.effectIndex = EffectIndex.Invalid;
            effect.parentToReferencedTransform = true;
            effect.positionAtReferencedTransform = true;
            effect.soundName = "";

            shieldBashFX.transform.Find("cum").Find("piss").gameObject.AddComponent<ParticleFuckingShitComponent>();

            shoulderBashFX.AddComponent<DestroyOnTimer>().duration = 5;
            shoulderBashFX.AddComponent<NetworkIdentity>();
            shoulderBashFX.AddComponent<VFXAttributes>().vfxPriority = VFXAttributes.VFXPriority.Always;
            effect = shoulderBashFX.AddComponent<EffectComponent>();
            effect.applyScale = false;
            effect.effectIndex = EffectIndex.Invalid;
            effect.parentToReferencedTransform = true;
            effect.positionAtReferencedTransform = true;
            effect.soundName = "";

            shoulderBashFX.transform.Find("cum").Find("poop").gameObject.AddComponent<ParticleFuckingShitComponent>();

            EffectAPI.AddEffect(shieldBashFX);
            EffectAPI.AddEffect(shoulderBashFX);

            gatDrone = MainAssetBundle.LoadAsset<GameObject>("GatDrone");

            stormtrooperMesh = MainAssetBundle.LoadAsset<Mesh>("StormtrooperMesh");
            engiMesh = MainAssetBundle.LoadAsset<Mesh>("EngiforcerMesh");
            desperadoMesh = MainAssetBundle.LoadAsset<Mesh>("EnforcerMesh");
            zeroSuitMesh = MainAssetBundle.LoadAsset<Mesh>("ZeroSuitMesh");
            classicMesh = MainAssetBundle.LoadAsset<Mesh>("EnforcerMesh");
            sexMesh = MainAssetBundle.LoadAsset<Mesh>("SexforcerMesh");
            femMesh = MainAssetBundle.LoadAsset<Mesh>("FemforcerMesh");
        }
    }
}