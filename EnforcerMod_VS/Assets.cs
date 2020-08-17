using System.Reflection;
using R2API;
using UnityEngine;
using System.IO;

namespace EnforcerPlugin {
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

        public static GameObject tearGasGrenadeModel;
        public static GameObject tearGasEffectPrefab;

        public static GameObject stunGrenadeModel;

        public static Mesh stormtrooperMesh;
        public static Mesh engiMesh;

        public static void PopulateAssets()
        {
            if (MainAssetBundle == null)
            {
                using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Enforcer.enforcer"))
                {
                    MainAssetBundle = AssetBundle.LoadFromStream(assetStream);
                }
            }

            //fuck whoever wrote this code and fuck you
            // comment out the soundbank shit and then wonder why sounds aren't working you're literally fucking retarded holy hell
            using(Stream manifestResourceStream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream("Enforcer.EnforcerBank.bnk"))
            {
                byte[] array = new byte[manifestResourceStream2.Length];
                manifestResourceStream2.Read(array, 0, array.Length);
                SoundAPI.SoundBanks.Add(array);
            }

            charPortrait = MainAssetBundle.LoadAsset<Sprite>("EnforcerBody").texture;

            //iconP = MainAssetBundle.LoadAsset<Sprite>("PassiveIcon");
            icon1 = MainAssetBundle.LoadAsset<Sprite>("Skill1Icon");
            icon2 = MainAssetBundle.LoadAsset<Sprite>("Skill2Icon");
            icon3 = MainAssetBundle.LoadAsset<Sprite>("Skill3Icon");
            icon3B = MainAssetBundle.LoadAsset<Sprite>("Skill3BIcon");
            icon4 = MainAssetBundle.LoadAsset<Sprite>("Skill4Icon");
            icon4B = MainAssetBundle.LoadAsset<Sprite>("Skill4BIcon");

            //grenade = TempAssetBundle.LoadAsset<GameObject>("Grenade");
            tearGasGrenadeModel = MainAssetBundle.LoadAsset<GameObject>("TearGasGrenade");
            tearGasEffectPrefab = MainAssetBundle.LoadAsset<GameObject>("TearGasEffect");

            stunGrenadeModel = MainAssetBundle.LoadAsset<GameObject>("StunGrenade");

            //add vfx shit so nothing breaks
            //tearGasEffectPrefab.AddComponent<VFXAttributes>().vfxPriority = VFXAttributes.VFXPriority.Always;
            //tearGasEffectPrefab.AddComponent<EffectComponent>().applyScale = false;
            //actually this wasn't even needed

            stormtrooperMesh = MainAssetBundle.LoadAsset<Mesh>("StormtrooperMesh");
            engiMesh = MainAssetBundle.LoadAsset<Mesh>("EngiforcerMesh");
        }
    }
}