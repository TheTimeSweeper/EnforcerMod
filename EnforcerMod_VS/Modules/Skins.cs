using System;
using UnityEngine;
using R2API;
using RoR2;
using R2API.Utils;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Modules.Characters;
using EnforcerPlugin;

namespace Modules {

    internal static class Skins {

        public enum EnforcerSkin
        {
            NONE = -1,
            DEFAULT,
            MASTERY,
            GRANDMASTERY,
            ROBIT,
            CLASSIC,
            RECOLORNEMESIS,
            RECOLORDESPERADO,
            RECOLORENGI,
            RECOLORENGIBUNG,
            RECOLORSTORM,
            RECOLORDOOM,
            FUCKINGSTEVE,
            FEMFORCER
        }

        public static SkinDef engiBungusSkin;

        public static Dictionary<uint, string> SkinIdices = new Dictionary<uint, string>();

        public static List<SkinDef> skinDefs = new List<SkinDef>();

        //todo what the fuck
        public static bool isEnforcerCurrentSkin(CharacterBody characterbody, string skin) {

            return characterbody.baseNameToken == "ENFORCER_NAME" && SkinIdices.ContainsKey(characterbody.skinIndex) && SkinIdices[characterbody.skinIndex] == skin;
        }

        public static bool isEnforcerCurrentSkin(CharacterBody characterbody, EnforcerSkin skin)
        {
            return isEnforcerCurrentSkin(characterbody, GetFuckingSkinID(skin));
        }

        public static string GetFuckingSkinID(EnforcerSkin skin)
        {
            switch (skin)
            {
                default:
                case EnforcerSkin.DEFAULT:
                    return "ENFORCERBODY_DEFAULT_SKIN_NAME";
                case EnforcerSkin.MASTERY:
                    return "ENFORCERBODY_MASTERY_SKIN_NAME";
                case EnforcerSkin.GRANDMASTERY:
                    return "ENFORCERBODY_TYPHOON_SKIN_NAME";
                case EnforcerSkin.ROBIT:
                    return "ENFORCERBODY_BOT_SKIN_NAME";
                case EnforcerSkin.CLASSIC:
                    return "ENFORCERBODY_CLASSIC_SKIN_NAME";
                case EnforcerSkin.RECOLORDESPERADO:
                    return "ENFORCERBODY_DESPERADO_SKIN_NAME";
                case EnforcerSkin.RECOLORENGI:
                    return "ENFORCERBODY_ENGI_SKIN_NAME";
                case EnforcerSkin.RECOLORNEMESIS:
                    return "ENFORCERBODY_NEMESIS_SKIN_NAME";
                case EnforcerSkin.RECOLORENGIBUNG:
                    return "ENFORCERBODY_ENGIBUNGUS_SKIN_NAME";
                case EnforcerSkin.RECOLORSTORM:
                    return "ENFORCERBODY_STORM_SKIN_NAME";
                case EnforcerSkin.RECOLORDOOM:
                    return "ENFORCERBODY_DOOM_SKIN_NAME";
                case EnforcerSkin.FUCKINGSTEVE:
                    return "ENFORCERBODY_FUCKINGSTEVE_SKIN_NAME";
                case EnforcerSkin.FEMFORCER:
                    return "ENFORCERBODY_FEMFORCER_SKIN_NAME";
            }
        }

        public static void RegisterSkins() {
            GameObject bodyPrefab = CharacterBase.instance.bodyPrefab;
            GameObject modelTransform = bodyPrefab.GetComponent<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = modelTransform.GetComponent<CharacterModel>();
            ModelSkinController skinController = modelTransform.AddComponent<ModelSkinController>();
            ChildLocator childLocator = modelTransform.GetComponent<ChildLocator>();
            SkinnedMeshRenderer mainRenderer = characterModel.mainSkinnedMeshRenderer;
            skinDefs = new List<SkinDef>();

            #region GameObjectActivations

            GameObject sexforcerGlass = childLocator.FindChild("ShieldGlassModel").gameObject;
            //GameObject pauldrons = childLocator.FindChild("PauldronModel").gameObject;

            allGameObjectActivations = new List<GameObject> {
                sexforcerGlass,
            };

            #endregion

            #region old default rendererinfos for reference
            /*
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
                    defaultMaterial = Assets.CreateMaterial("matEnforcerGun", 1f, Color.white, 0f),
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
            */
            #endregion

            #region default
            SkinDefInfo defaultSkinDefInfo = new SkinDefInfo();
            defaultSkinDefInfo.Name = "ENFORCERBODY_DEFAULT_SKIN_NAME";
            defaultSkinDefInfo.NameToken = "ENFORCERBODY_DEFAULT_SKIN_NAME";
            defaultSkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texEnforcerAchievement");
            defaultSkinDefInfo.RootObject = modelTransform;

            defaultSkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            defaultSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            defaultSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            defaultSkinDefInfo.GameObjectActivations = getGameObjectActivations(sexforcerGlass);

            defaultSkinDefInfo.MeshReplacements = getMeshReplacements(characterModel.baseRendererInfos,
                "meshEnforcerShield",
                "meshEnforcerShieldGlass",
                "meshEnforcerSkamteBord",
                "meshEnforcerGun",
                "meshClassicGunSuper",
                "meshClassicGunHMG",
                "meshEnforcerHammer",
                "meshEnforcerPauldron",
                "meshEnforcer"
                );
            
            defaultSkinDefInfo.RendererInfos = characterModel.baseRendererInfos;
            //pauldron for lights
            defaultSkinDefInfo.RendererInfos[7].defaultMaterial.MakeUnique();
            
            SkinDef defaultSkinDef = CreateSkinDef(defaultSkinDefInfo);
            skinDefs.Add(defaultSkinDef);
            #endregion

            #region Mastery
            SkinDefInfo masterySkinDefInfo = new SkinDefInfo();
            masterySkinDefInfo.Name = "ENFORCERBODY_MASTERY_SKIN_NAME";
            masterySkinDefInfo.NameToken = "ENFORCERBODY_MASTERY_SKIN_NAME";
            masterySkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texSexforcerAchievement");
            masterySkinDefInfo.UnlockableDef = EnforcerUnlockables.enforcerMasteryUnlockableDef;
            masterySkinDefInfo.RootObject = modelTransform;

            masterySkinDefInfo.BaseSkins = new SkinDef[] { defaultSkinDef };
            masterySkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            masterySkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            masterySkinDefInfo.GameObjectActivations = getGameObjectActivations(sexforcerGlass);

            masterySkinDefInfo.MeshReplacements = getMeshReplacements(characterModel.baseRendererInfos,
                "meshSexforcerShield",
                "meshSexforcerShieldGlass",
                null,//board
                null,//"meshEnforcerGun",
                null,//"meshClassicGunSuper",
                null,//"meshClassicGunHMG",
                null,//"meshEnforcerHammer",
                "meshSexforcerPauldron",
                "meshSexforcer"
                );

            masterySkinDefInfo.RendererInfos = new CharacterModel.RendererInfo[defaultSkinDef.rendererInfos.Length];
            defaultSkinDef.rendererInfos.CopyTo(masterySkinDefInfo.RendererInfos, 0);

            masterySkinDefInfo.RendererInfos[0].defaultMaterial = Materials.CreateHotpooMaterial("matSexforcerShield");
            //take default
            //masterySkinDefInfo.RendererInfos[1].defaultMaterial = Assets.CreateMaterial("matSexforcerShieldGlass", 0f, Color.black, 0);
            masterySkinDefInfo.RendererInfos[2].defaultMaterial = Materials.CreateHotpooMaterial("matSexforcerBoard");
            //masterySkinDefInfo.RendererInfos[3].defaultMaterial = Assets.CreateMaterial("matEnforcerGun", 0f, Color.white, 0f);
            //masterySkinDefInfo.RendererInfos[4].defaultMaterial = Assets.CreateMaterial("matClassicGunSuper", 0f, Color.white, 0f);
            //masterySkinDefInfo.RendererInfos[5].defaultMaterial = Assets.CreateMaterial("matClassicGunHMG", 0f, Color.white, 0f);
            //masterySkinDefInfo.RendererInfos[6].defaultMaterial = Assets.CreateMaterial("matEnforcerHammer", 0f, Color.white, 0f);
            masterySkinDefInfo.RendererInfos[7].defaultMaterial = Materials.CreateHotpooMaterial("matSexforcer").MakeUnique();
            masterySkinDefInfo.RendererInfos[8].defaultMaterial = Materials.CreateHotpooMaterial("matSexforcer");

            SkinDef masterySkin = CreateSkinDef(masterySkinDefInfo);
            skinDefs.Add(masterySkin);
            #endregion

            #region GrandMastery
            SkinDefInfo grandMasterySkinDefInfo = new SkinDefInfo();
            grandMasterySkinDefInfo.Name = "ENFORCERBODY_TYPHOON_SKIN_NAME";
            grandMasterySkinDefInfo.NameToken = "ENFORCERBODY_TYPHOON_SKIN_NAME";
            grandMasterySkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texTyphoonAchievement");
            grandMasterySkinDefInfo.UnlockableDef = EnforcerUnlockables.enforcerGrandMasteryUnlockableDef;
            grandMasterySkinDefInfo.RootObject = modelTransform;

            grandMasterySkinDefInfo.BaseSkins = new SkinDef[] { defaultSkinDef };
            grandMasterySkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            grandMasterySkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            grandMasterySkinDefInfo.GameObjectActivations = getGameObjectActivations(sexforcerGlass);

            grandMasterySkinDefInfo.MeshReplacements = getMeshReplacements(characterModel.baseRendererInfos,
                "meshGMShield",
                "meshGMShieldGlass",
                null,//board
                null,//"meshEnforcerGun",
                null,//"meshClassicGunSuper",
                null,//"meshClassicGunHMG",
                null,//"meshEnforcerHammer",
                "meshGMPauldron",
                "meshGM"
                );

            grandMasterySkinDefInfo.RendererInfos = new CharacterModel.RendererInfo[defaultSkinDef.rendererInfos.Length];
            defaultSkinDef.rendererInfos.CopyTo(grandMasterySkinDefInfo.RendererInfos, 0);

            grandMasterySkinDefInfo.RendererInfos[0].defaultMaterial = Materials.CreateHotpooMaterial("matGMShield");
            //take default
            //grandMasterySkinDefInfo.RendererInfos[1].defaultMaterial = Assets.CreateMaterial("matSexforcerShieldGlass", 0f, Color.black, 0);
            //grandMasterySkinDefInfo.RendererInfos[2].defaultMaterial = Materials.CreateHotpooMaterial("matSexforcerBoard");
            grandMasterySkinDefInfo.RendererInfos[3].defaultMaterial = Materials.CreateHotpooMaterial("matGMGun");
            //grandMasterySkinDefInfo.RendererInfos[4].defaultMaterial = Assets.CreateMaterial("matClassicGunSuper", 0f, Color.white, 0f);
            //grandMasterySkinDefInfo.RendererInfos[5].defaultMaterial = Assets.CreateMaterial("matClassicGunHMG", 0f, Color.white, 0f);
            //grandMasterySkinDefInfo.RendererInfos[6].defaultMaterial = Assets.CreateMaterial("matEnforcerHammer", 0f, Color.white, 0f);
            grandMasterySkinDefInfo.RendererInfos[7].defaultMaterial = Materials.CreateHotpooMaterial("matGM").MakeUnique();
            grandMasterySkinDefInfo.RendererInfos[8].defaultMaterial = Materials.CreateHotpooMaterial("matGM");

            SkinDef grandMasterySkin = CreateSkinDef(grandMasterySkinDefInfo);
            skinDefs.Add(grandMasterySkin);
            #endregion

            #region robit
            SkinDefInfo robitSkinDefInfo = new SkinDefInfo();
            robitSkinDefInfo.Name = "ENFORCERBODY_BOT_SKIN_NAME";
            robitSkinDefInfo.NameToken = "ENFORCERBODY_BOT_SKIN_NAME";
            robitSkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texNemforcerEnforcer");
            robitSkinDefInfo.UnlockableDef = EnforcerUnlockables.enforcerRobitUnlockableDef;
            robitSkinDefInfo.RootObject = modelTransform;

            robitSkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            robitSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            robitSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            robitSkinDefInfo.GameObjectActivations = getGameObjectActivations();

            robitSkinDefInfo.MeshReplacements = getMeshReplacements(characterModel.baseRendererInfos,
                "meshN4CRShield",
                null,//sex shield glass
                "meshEnforcerSkamteBord",//board
                "meshN4CRGun",
                "meshN4CRGun",
                "meshN4CRGun",
                "meshEnforcerHammer",
                "meshN4CRPauldron",
                "meshN4CR"
                );

            robitSkinDefInfo.RendererInfos = new CharacterModel.RendererInfo[defaultSkinDef.rendererInfos.Length];
            defaultSkinDef.rendererInfos.CopyTo(robitSkinDefInfo.RendererInfos, 0);

            robitSkinDefInfo.RendererInfos[0].defaultMaterial = Materials.CreateHotpooMaterial("matN4CR");
            //[1] take default                                  
            //[2] take default                                  
            robitSkinDefInfo.RendererInfos[3].defaultMaterial = Materials.CreateHotpooMaterial("matN4CR");
            robitSkinDefInfo.RendererInfos[4].defaultMaterial = Materials.CreateHotpooMaterial("matN4CR");
            robitSkinDefInfo.RendererInfos[5].defaultMaterial = Materials.CreateHotpooMaterial("matN4CR");
            //[6] take default hammer                           
            robitSkinDefInfo.RendererInfos[7].defaultMaterial = Materials.CreateHotpooMaterial("matN4CR").MakeUnique();
            robitSkinDefInfo.RendererInfos[8].defaultMaterial = Materials.CreateHotpooMaterial("matN4CR");

            SkinDef robitSkinDef = CreateSkinDef(robitSkinDefInfo);
            skinDefs.Add(robitSkinDef);
            #endregion

            #region Classic
            SkinDefInfo classicSkinDefInfo = new SkinDefInfo();
            classicSkinDefInfo.Name = "ENFORCERBODY_CLASSIC_SKIN_NAME";
            classicSkinDefInfo.NameToken = "ENFORCERBODY_CLASSIC_SKIN_NAME";
            classicSkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texEnforcerAchievement");
            classicSkinDefInfo.UnlockableDef = EnforcerUnlockables.enforcerClassicUnlockableDef;
            classicSkinDefInfo.RootObject = modelTransform;

            classicSkinDefInfo.BaseSkins = new SkinDef[] { defaultSkinDef };
            classicSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            classicSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            classicSkinDefInfo.GameObjectActivations = getGameObjectActivations();

            classicSkinDefInfo.MeshReplacements = getMeshReplacements(characterModel.baseRendererInfos,
                null,//"meshEnforcerShield",
                null,//"meshEnforcerShieldGlass",//take default
                null,//board
                "meshClassicGun",
                "meshClassicGunSuper",
                "meshClassicGunHMG",
                null,//"meshEnforcerHammer",
                "meshClassicPauldron",
                "meshClassic"
                );

            classicSkinDefInfo.RendererInfos = new CharacterModel.RendererInfo[defaultSkinDef.rendererInfos.Length];
            defaultSkinDef.rendererInfos.CopyTo(classicSkinDefInfo.RendererInfos, 0);

            //classicSkinDefInfo.RendererInfos[0].defaultMaterial = Assets.CreateMaterial("matEnforcerShield", 0f, Color.black, 1f);
            //classicSkinDefInfo.RendererInfos[1].defaultMaterial = Assets.CreateMaterial("matSexforcerShieldGlass", 0f, Color.black, 0);
            //[2] default board
            classicSkinDefInfo.RendererInfos[3].defaultMaterial = Materials.CreateHotpooMaterial("matClassicGun");
            classicSkinDefInfo.RendererInfos[4].defaultMaterial = Materials.CreateHotpooMaterial("matClassicGunSuper");
            classicSkinDefInfo.RendererInfos[5].defaultMaterial = Materials.CreateHotpooMaterial("matClassicGunHMG");
            //classicSkinDefInfo.RendererInfos[6].defaultMaterial = Assets.CreateMaterial("matEnforcerHammer", 0f, Color.white, 0f);
            classicSkinDefInfo.RendererInfos[7].defaultMaterial = Materials.CreateHotpooMaterial("matClassic").MakeUnique();
            classicSkinDefInfo.RendererInfos[8].defaultMaterial = Materials.CreateHotpooMaterial("matClassic");

            SkinDef classicSkin = CreateSkinDef(classicSkinDefInfo);
            skinDefs.Add(classicSkin);
            #endregion

            #region RecolorNemesis
            SkinDefInfo nemesisSkinDefInfo = new SkinDefInfo();
            nemesisSkinDefInfo.Name = "ENFORCERBODY_NEMESIS_SKIN_NAME";
            nemesisSkinDefInfo.NameToken = "ENFORCERBODY_NEMESIS_SKIN_NAME";
            nemesisSkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texNemforcerAchievement");
            nemesisSkinDefInfo.UnlockableDef = EnforcerUnlockables.enforcerNemesisSkinUnlockableDef;
            nemesisSkinDefInfo.RootObject = modelTransform;

            nemesisSkinDefInfo.BaseSkins = new SkinDef[] { defaultSkinDef };
            nemesisSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            nemesisSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            nemesisSkinDefInfo.GameObjectActivations = getGameObjectActivations(sexforcerGlass);

            nemesisSkinDefInfo.MeshReplacements = getMeshReplacements(characterModel.baseRendererInfos,
                null,
                null,
                null,//board
                null,//"meshEnforcerGun",
                null,//"meshClassicGunSuper",
                null,//"meshClassicGunHMG",
                "meshRecolorNemHammer",//"meshEnforcerHammer",
                null,
                null
                );

            nemesisSkinDefInfo.RendererInfos = new CharacterModel.RendererInfo[defaultSkinDef.rendererInfos.Length];
            defaultSkinDef.rendererInfos.CopyTo(nemesisSkinDefInfo.RendererInfos, 0);

            nemesisSkinDefInfo.RendererInfos[0].defaultMaterial = Materials.CreateHotpooMaterial("matRecolorNemShield");
            nemesisSkinDefInfo.RendererInfos[1].defaultMaterial = Assets.LoadAsset<Material>("matRecolorDesperadoShieldGlass");
            //nemesisSkinDefInfo.RendererInfos[2].defaultMaterial = Materials.CreateHotpooMaterial("matSexforcerBoard");
            nemesisSkinDefInfo.RendererInfos[3].defaultMaterial = Materials.CreateHotpooMaterial("matRecolorNemGun");
            //nemesisSkinDefInfo.RendererInfos[4].defaultMaterial = Assets.CreateMaterial("matClassicGunSuper", 0f, Color.white, 0f);
            //nemesisSkinDefInfo.RendererInfos[5].defaultMaterial = Assets.CreateMaterial("matClassicGunHMG", 0f, Color.white, 0f);
            nemesisSkinDefInfo.RendererInfos[6].defaultMaterial = Materials.CreateHotpooMaterial("matNemforcerClassic");
            nemesisSkinDefInfo.RendererInfos[7].defaultMaterial = Materials.CreateHotpooMaterial("matRecolorNem").MakeUnique();
            nemesisSkinDefInfo.RendererInfos[8].defaultMaterial = Materials.CreateHotpooMaterial("matRecolorNem");

            SkinDef nemesisSkin = CreateSkinDef(nemesisSkinDefInfo);
            skinDefs.Add(nemesisSkin);
            #endregion

            if (!EnforcerModPlugin.holdonasec) {
                #region RecolorDesperado
                SkinDefInfo desperadoSkinDefInfo = new SkinDefInfo();
                desperadoSkinDefInfo.Name = "ENFORCERBODY_DESPERADO_SKIN_NAME";
                desperadoSkinDefInfo.NameToken = "ENFORCERBODY_DESPERADO_SKIN_NAME";
                desperadoSkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texDesperadoAchievement");
                desperadoSkinDefInfo.UnlockableDef = EnforcerUnlockables.enforcerDesperadoSkinUnlockableDef;
                desperadoSkinDefInfo.RootObject = modelTransform;

                desperadoSkinDefInfo.BaseSkins = new SkinDef[] { defaultSkinDef };
                desperadoSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
                desperadoSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

                desperadoSkinDefInfo.GameObjectActivations = getGameObjectActivations(sexforcerGlass);

                desperadoSkinDefInfo.MeshReplacements = getMeshReplacements(characterModel.baseRendererInfos,
                    null,
                    null,
                    null,//board
                    null,//"meshEnforcerGun",
                    null,//"meshClassicGunSuper",
                    null,//"meshClassicGunHMG",
                    null,//"meshEnforcerHammer",
                    null,
                    null//"meshRecolorDesperado"
                    );

                desperadoSkinDefInfo.RendererInfos = new CharacterModel.RendererInfo[defaultSkinDef.rendererInfos.Length];
                defaultSkinDef.rendererInfos.CopyTo(desperadoSkinDefInfo.RendererInfos, 0);

                desperadoSkinDefInfo.RendererInfos[0].defaultMaterial = Materials.CreateHotpooMaterial("matRecolorDesperadoShield");
                desperadoSkinDefInfo.RendererInfos[1].defaultMaterial = Assets.LoadAsset<Material>("matRecolorDesperadoShieldGlass");
                //desperadoSkinDefInfo.RendererInfos[2].defaultMaterial = Materials.CreateHotpooMaterial("matSexforcerBoard");
                desperadoSkinDefInfo.RendererInfos[3].defaultMaterial = Materials.CreateHotpooMaterial("matRecolorDesperadoGun");
                //desperadoSkinDefInfo.RendererInfos[4].defaultMaterial = Assets.CreateMaterial("matClassicGunSuper", 0f, Color.white, 0f);
                //desperadoSkinDefInfo.RendererInfos[5].defaultMaterial = Assets.CreateMaterial("matClassicGunHMG", 0f, Color.white, 0f);
                //desperadoSkinDefInfo.RendererInfos[6].defaultMaterial = Materials.CreateHotpooMaterial("matNemforcerClassic");
                desperadoSkinDefInfo.RendererInfos[7].defaultMaterial = Materials.CreateHotpooMaterial("matRecolorDesperado").MakeUnique();
                desperadoSkinDefInfo.RendererInfos[8].defaultMaterial = Materials.CreateHotpooMaterial("matRecolorDesperado");

                SkinDef desperadoSkin = CreateSkinDef(desperadoSkinDefInfo);
                skinDefs.Add(desperadoSkin);
                #endregion

                #region RecolorEngi
                SkinDefInfo engiSkinDefInfo = new SkinDefInfo();
                engiSkinDefInfo.Name = "ENFORCERBODY_ENGI_SKIN_NAME";
                engiSkinDefInfo.NameToken = "ENFORCERBODY_ENGI_SKIN_NAME";
                engiSkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texBungusAchievement");
                engiSkinDefInfo.UnlockableDef = EnforcerUnlockables.enforcerEngiSkinUnlockableDef;
                engiSkinDefInfo.RootObject = modelTransform;

                engiSkinDefInfo.BaseSkins = new SkinDef[] { defaultSkinDef };
                engiSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
                engiSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

                engiSkinDefInfo.GameObjectActivations = getGameObjectActivations(sexforcerGlass);

                engiSkinDefInfo.MeshReplacements = getMeshReplacements(characterModel.baseRendererInfos,
                    null,
                    null,
                    null,//board
                    null,//"meshEnforcerGun",
                    null,//"meshClassicGunSuper",
                    null,//"meshClassicGunHMG",
                    null,//"meshEnforcerHammer",
                    null,
                    "meshRecolorEngi"
                    );

                engiSkinDefInfo.RendererInfos = new CharacterModel.RendererInfo[defaultSkinDef.rendererInfos.Length];
                defaultSkinDef.rendererInfos.CopyTo(engiSkinDefInfo.RendererInfos, 0);

                engiSkinDefInfo.RendererInfos[0].defaultMaterial = Materials.CreateHotpooMaterial("matRecolorEngiShield");
                //take default
                //engiSkinDefInfo.RendererInfos[1].defaultMaterial = Assets.CreateMaterial("matSexforcerShieldGlass", 0f, Color.black, 0);
                //engiSkinDefInfo.RendererInfos[2].defaultMaterial = Materials.CreateHotpooMaterial("matSexforcerBoard");
                engiSkinDefInfo.RendererInfos[3].defaultMaterial = Materials.CreateHotpooMaterial("matRecolorEngiGun");
                //engiSkinDefInfo.RendererInfos[4].defaultMaterial = Assets.CreateMaterial("matClassicGunSuper", 0f, Color.white, 0f);
                //engiSkinDefInfo.RendererInfos[5].defaultMaterial = Assets.CreateMaterial("matClassicGunHMG", 0f, Color.white, 0f);
                //engiSkinDefInfo.RendererInfos[6].defaultMaterial = Materials.CreateHotpooMaterial("matNemforcerClassic");
                engiSkinDefInfo.RendererInfos[7].defaultMaterial = Materials.CreateHotpooMaterial("matRecolorEngi").MakeUnique();
                engiSkinDefInfo.RendererInfos[8].defaultMaterial = Materials.CreateHotpooMaterial("matRecolorEngi");

                SkinDef engiSkin = CreateSkinDef(engiSkinDefInfo);
                skinDefs.Add(engiSkin);
                #endregion

                #region RecolorEngiBung
                SkinDefInfo engiBungusSkinDefInfo = new SkinDefInfo();
                engiBungusSkinDefInfo.Name = "ENFORCERBODY_ENGIBUNGUS_SKIN_NAME";
                engiBungusSkinDefInfo.NameToken = "ENFORCERBODY_ENGIBUNGUS_SKIN_NAME";
                engiBungusSkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texBungusAchievement");
                engiBungusSkinDefInfo.UnlockableDef = null;
                engiBungusSkinDefInfo.RootObject = modelTransform;

                engiBungusSkinDefInfo.BaseSkins = new SkinDef[] { engiSkin };
                engiBungusSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
                engiBungusSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

                engiBungusSkinDefInfo.GameObjectActivations = getGameObjectActivations(sexforcerGlass);

                engiBungusSkinDefInfo.MeshReplacements = getMeshReplacements(characterModel.baseRendererInfos,
                    null,
                    null,
                    null,
                    "meshRecolorEngiBungusGun",
                    "meshRecolorEngiBungusSSG",
                    "meshRecolorEngiBungusHMG",
                    "meshRecolorEngiBungusHammer",
                    null,
                    null//"meshRecolorEngi"
                    );

                engiBungusSkinDefInfo.RendererInfos = new CharacterModel.RendererInfo[defaultSkinDef.rendererInfos.Length];
                engiSkin.rendererInfos.CopyTo(engiBungusSkinDefInfo.RendererInfos, 0);

                Material bungusMat = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<Material>("RoR2/Base/Mushroom/matMushroom.mat").WaitForCompletion();

                //engiBungusSkinDefInfo.RendererInfos[0].defaultMaterial = Materials.CreateHotpooMaterial("matRecolorEngiShield");
                //engiBungusSkinDefInfo.RendererInfos[1].defaultMaterial = Assets.CreateMaterial("matSexforcerShieldGlass", 0f, Color.black, 0);
                //engiBungusSkinDefInfo.RendererInfos[2].defaultMaterial = Materials.CreateHotpooMaterial("matSexforcerBoard");
                engiBungusSkinDefInfo.RendererInfos[3].defaultMaterial = bungusMat;
                engiBungusSkinDefInfo.RendererInfos[4].defaultMaterial = bungusMat;
                engiBungusSkinDefInfo.RendererInfos[5].defaultMaterial = bungusMat;
                engiBungusSkinDefInfo.RendererInfos[6].defaultMaterial = bungusMat;
                //engiBungusSkinDefInfo.RendererInfos[7].defaultMaterial = Materials.CreateHotpooMaterial("matRecolorEngi").MakeUnique();
                //engiBungusSkinDefInfo.RendererInfos[8].defaultMaterial = Materials.CreateHotpooMaterial("matRecolorEngi");

                engiBungusSkin = CreateSkinDef(engiBungusSkinDefInfo);
                #endregion

                #region RecolorStorm
                SkinDefInfo stormSkinDefInfo = new SkinDefInfo();
                stormSkinDefInfo.Name = "ENFORCERBODY_STORM_SKIN_NAME";
                stormSkinDefInfo.NameToken = "ENFORCERBODY_STORM_SKIN_NAME";
                stormSkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texStormtrooperAchievement");
                stormSkinDefInfo.UnlockableDef = EnforcerUnlockables.enforcerStormSkinUnlockableDef;
                stormSkinDefInfo.RootObject = modelTransform;

                stormSkinDefInfo.BaseSkins = new SkinDef[] { defaultSkinDef };
                stormSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
                stormSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

                stormSkinDefInfo.GameObjectActivations = getGameObjectActivations(sexforcerGlass);
                
                stormSkinDefInfo.MeshReplacements = getMeshReplacements(characterModel.baseRendererInfos,
                    null,
                    null,
                    null,//board
                    "meshRecolorStormGun",
                    "meshRecolorStormGun",
                    "meshRecolorStormGun",
                    null,//"meshEnforcerHammer",
                    null,
                    "meshRecolorStorm"
                    );

                stormSkinDefInfo.RendererInfos = new CharacterModel.RendererInfo[defaultSkinDef.rendererInfos.Length];
                defaultSkinDef.rendererInfos.CopyTo(stormSkinDefInfo.RendererInfos, 0);

                stormSkinDefInfo.RendererInfos[0].defaultMaterial = Materials.CreateHotpooMaterial("matRecolorStormShield");
                //take default
                //stormSkinDefInfo.RendererInfos[1].defaultMaterial = Assets.CreateMaterial("matSexforcerShieldGlass", 0f, Color.black, 0);
                //stormSkinDefInfo.RendererInfos[2].defaultMaterial = Materials.CreateHotpooMaterial("matSexforcerBoard");
                stormSkinDefInfo.RendererInfos[3].defaultMaterial = Materials.CreateHotpooMaterial("matRecolorStormGun");
                stormSkinDefInfo.RendererInfos[4].defaultMaterial = Materials.CreateHotpooMaterial("matRecolorStormGun");
                stormSkinDefInfo.RendererInfos[5].defaultMaterial = Materials.CreateHotpooMaterial("matRecolorStormGun");
                //stormSkinDefInfo.RendererInfos[6].defaultMaterial = Materials.CreateHotpooMaterial("matNemforcerClassic");
                stormSkinDefInfo.RendererInfos[7].defaultMaterial = Materials.CreateHotpooMaterial("matRecolorStorm").MakeUnique();
                stormSkinDefInfo.RendererInfos[8].defaultMaterial = Materials.CreateHotpooMaterial("matRecolorStorm");

                SkinDef stormSkin = CreateSkinDef(stormSkinDefInfo);
                skinDefs.Add(stormSkin);
                #endregion

                #region RecolorDoom
                SkinDefInfo doomSkinDefInfo = new SkinDefInfo();
                doomSkinDefInfo.Name = "ENFORCERBODY_DOOM_SKIN_NAME";
                doomSkinDefInfo.NameToken = "ENFORCERBODY_DOOM_SKIN_NAME";
                doomSkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texDoomAchievement");
                doomSkinDefInfo.UnlockableDef = EnforcerUnlockables.enforcerDoomSkinUnlockableDef;
                doomSkinDefInfo.RootObject = modelTransform;

                doomSkinDefInfo.BaseSkins = new SkinDef[] { defaultSkinDef };
                doomSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
                doomSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

                doomSkinDefInfo.GameObjectActivations = getGameObjectActivations(sexforcerGlass);

                doomSkinDefInfo.MeshReplacements = getMeshReplacements(characterModel.baseRendererInfos,
                    null,
                    null,
                    null,//board
                    null,//"meshEnforcerGun",
                    null,//"meshClassicGunSuper",
                    null,//"meshClassicGunHMG",
                    null,//"meshEnforcerHammer",
                    null,
                    null
                    );

                doomSkinDefInfo.RendererInfos = new CharacterModel.RendererInfo[defaultSkinDef.rendererInfos.Length];
                defaultSkinDef.rendererInfos.CopyTo(doomSkinDefInfo.RendererInfos, 0);

                doomSkinDefInfo.RendererInfos[0].defaultMaterial = Materials.CreateHotpooMaterial("matRecolorDoomShield");
                //take default
                //doomSkinDefInfo.RendererInfos[1].defaultMaterial = Assets.CreateMaterial("matSexforcerShieldGlass", 0f, Color.black, 0);
                //doomSkinDefInfo.RendererInfos[2].defaultMaterial = Materials.CreateHotpooMaterial("matSexforcerBoard");
                doomSkinDefInfo.RendererInfos[3].defaultMaterial = Materials.CreateHotpooMaterial("matRecolorDoomGun");
                //doomSkinDefInfo.RendererInfos[4].defaultMaterial = Assets.CreateMaterial("matClassicGunSuper", 0f, Color.white, 0f);
                //doomSkinDefInfo.RendererInfos[5].defaultMaterial = Assets.CreateMaterial("matClassicGunHMG", 0f, Color.white, 0f);
                //doomSkinDefInfo.RendererInfos[6].defaultMaterial = Materials.CreateHotpooMaterial("matNemforcerClassic");
                doomSkinDefInfo.RendererInfos[7].defaultMaterial = Materials.CreateHotpooMaterial("matRecolorDoom").MakeUnique();
                doomSkinDefInfo.RendererInfos[8].defaultMaterial = Materials.CreateHotpooMaterial("matRecolorDoom");

                SkinDef doomSkin = CreateSkinDef(doomSkinDefInfo);
                skinDefs.Add(doomSkin);
                #endregion
            }

            if(Config.cursed.Value) {

                #region If she don't play the craft
                SkinDefInfo minecraftSkinDefInfo = new SkinDefInfo();
                minecraftSkinDefInfo.Name = "ENFORCERBODY_FUCKINGSTEVE_SKIN_NAME";
                minecraftSkinDefInfo.NameToken = "ENFORCERBODY_FUCKINGSTEVE_SKIN_NAME";
                minecraftSkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texSbeveAchievement");
                minecraftSkinDefInfo.UnlockableDef = EnforcerUnlockables.enforcerSteveUnlockableDef;
                minecraftSkinDefInfo.RootObject = modelTransform;

                minecraftSkinDefInfo.BaseSkins = new SkinDef[] { defaultSkinDef };
                minecraftSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
                minecraftSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

                minecraftSkinDefInfo.GameObjectActivations = getGameObjectActivations().AddItem(new SkinDef.GameObjectActivation {
                    gameObject = childLocator.FindChild("PauldronModel").gameObject,
                    shouldActivate = false
                }).ToArray();

                minecraftSkinDefInfo.MeshReplacements = getMeshReplacements(characterModel.baseRendererInfos,
                    "meshFuckingSteve2Shield",
                    null,//"meshSexforcerShieldGlass",
                    null,//"meshSbeveBoard",
                    null,//"meshSbeveGun",
                    null,//"meshSbeveGunSuper",
                    null,//"meshSbeveGunHMG",
                    null,//"meshSbeveHammer",
                    null,//"meshSbevePauldron",
                    "meshFuckingSteve2"
                    );
                minecraftSkinDefInfo.RendererInfos = characterModel.baseRendererInfos;

                minecraftSkinDefInfo.RendererInfos = new CharacterModel.RendererInfo[defaultSkinDef.rendererInfos.Length];
                defaultSkinDef.rendererInfos.CopyTo(minecraftSkinDefInfo.RendererInfos, 0);

                minecraftSkinDefInfo.RendererInfos[0].defaultMaterial = Materials.CreateHotpooMaterial("matFuckingSteve2Shield");
                minecraftSkinDefInfo.RendererInfos[8].defaultMaterial = Materials.CreateHotpooMaterial("matFuckingSteve2");

                SkinDef minecraftSkin = CreateSkinDef(minecraftSkinDefInfo);

                //wait more model
                skinDefs.Add(minecraftSkin);
                #endregion she don't get the shaft

                #region fucking frog
                #endregion
            }

            if (Config.femSkin.Value) {
                #region fem
                SkinDefInfo femforcerSkinDefInfo = new SkinDefInfo();
                femforcerSkinDefInfo.Name = "ENFORCERBODY_FEMFORCER_SKIN_NAME";
                femforcerSkinDefInfo.NameToken = "ENFORCERBODY_FEMFORCER_SKIN_NAME";
                //femforcerSkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texSexforcerAchievement");
                femforcerSkinDefInfo.Icon = LoadoutAPI.CreateSkinIcon(new Color(0.31f, 0.49f, 0.69f), new Color(0.86f, 0.83f, 0.63f), new Color(0.1f, 0.07f, 0.06f), new Color(0.21f, 0.29f, 0.38f), new Color(0.9f, 0.6f, 0.69f));

                femforcerSkinDefInfo.UnlockableDef = EnforcerUnlockables.enforcerMasteryUnlockableDef;
                femforcerSkinDefInfo.RootObject = modelTransform;

                femforcerSkinDefInfo.BaseSkins = new SkinDef[] { defaultSkinDef };
                femforcerSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
                femforcerSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

                femforcerSkinDefInfo.GameObjectActivations = getGameObjectActivations(sexforcerGlass);

                femforcerSkinDefInfo.MeshReplacements = getMeshReplacements(characterModel.baseRendererInfos,
                    "meshFemforcer2Shield",
                    "meshFemforcer2ShieldGlass",
                    "meshFemforcer2Board",
                    null,//"meshEnforcerGun",
                    null,//"meshClassicGunSuper",
                    null,//"meshClassicGunHMG",
                    null,//"meshEnforcerHammer",
                    "meshFemforcer2Pauldron",
                    "meshFemforcer2"
                    );

                femforcerSkinDefInfo.RendererInfos = new CharacterModel.RendererInfo[defaultSkinDef.rendererInfos.Length];
                defaultSkinDef.rendererInfos.CopyTo(femforcerSkinDefInfo.RendererInfos, 0);

                femforcerSkinDefInfo.RendererInfos[0].defaultMaterial = Materials.CreateHotpooMaterial("matFemforcer2");
                //take default
                //femforcerSkinDefInfo.RendererInfos[1].defaultMaterial = Assets.CreateMaterial("matFemforcer2ShieldGlass", 0f, Color.black, 0);
                femforcerSkinDefInfo.RendererInfos[2].defaultMaterial = Materials.CreateHotpooMaterial("matFemforcer2Board");
                //femforcerSkinDefInfo.RendererInfos[3].defaultMaterial = Assets.CreateMaterial("matEnforcerGun", 0f, Color.white, 0f);
                //femforcerSkinDefInfo.RendererInfos[4].defaultMaterial = Assets.CreateMaterial("matClassicGunSuper", 0f, Color.white, 0f);
                //femforcerSkinDefInfo.RendererInfos[5].defaultMaterial = Assets.CreateMaterial("matClassicGunHMG", 0f, Color.white, 0f);
                //femforcerSkinDefInfo.RendererInfos[6].defaultMaterial = Assets.CreateMaterial("matEnforcerHammer", 0f, Color.white, 0f);
                femforcerSkinDefInfo.RendererInfos[7].defaultMaterial = Materials.CreateHotpooMaterial("matFemforcer2").MakeUnique();
                femforcerSkinDefInfo.RendererInfos[8].defaultMaterial = Materials.CreateHotpooMaterial("matFemforcer2");

                SkinDef femforcerSkin = CreateSkinDef(femforcerSkinDefInfo);
                skinDefs.Add(femforcerSkin);

                #endregion fem
            }
            // what are we gonna do about all this...........
            //      fuckin nothing that's what you're gonna do faggot
            #region FUCK
            /*
            #region GameObjects
            GameObject engiShield = childLocator.FindChild("EngiShield").gameObject;
            GameObject shotgunModel = childLocator.FindChild("ShotgunModel").gameObject;
            GameObject rifleModel = childLocator.FindChild("RifleModel").gameObject;
            GameObject blasterModel = childLocator.FindChild("Blaster").gameObject;
            GameObject blasterRifle = childLocator.FindChild("BlasterRifle").gameObject;
            GameObject superShotgun = childLocator.FindChild("SuperShotgunModel").gameObject;
            GameObject superBlaster = childLocator.FindChild("BlasterSuper").gameObject;
            GameObject shieldModel = childLocator.FindChild("ShieldModel").gameObject;
            GameObject sexShield = childLocator.FindChild("SexShieldModel").gameObject;
            GameObject marauderShield = childLocator.FindChild("MarauderArmShield").gameObject;
            GameObject bungusShield = childLocator.FindChild("BungusArmShield").gameObject;
            GameObject bungusShotgun = childLocator.FindChild("BungusShotgun").gameObject;
            GameObject bungusSSG = childLocator.FindChild("BungusSSG").gameObject;
            GameObject bungusRifle = childLocator.FindChild("BungusRifle").gameObject;
            GameObject cubeShield = childLocator.FindChild("CubeShield").gameObject;
            GameObject cubeShotgun = childLocator.FindChild("CubeShotgun").gameObject;
            GameObject cubeRifle = childLocator.FindChild("CubeRifle").gameObject;
            GameObject femShield = childLocator.FindChild("FemShield").gameObject;
            GameObject lightL = childLocator.FindChild("LightL").gameObject;
            GameObject lightR = childLocator.FindChild("LightR").gameObject;

            GameObject[] allObjects = new GameObject[] {
                engiShield,
                shotgunModel,
                rifleModel,
                blasterModel,
                blasterRifle,
                superShotgun,
                superBlaster,
                shieldModel,
                sexShield,
                marauderShield,
                bungusShield,
                bungusShotgun,
                bungusSSG,
                bungusRifle,
                cubeShield,
                cubeShotgun,
                cubeRifle,
                femShield,
                lightL,
                lightR
            };
            #endregion

            #region DefaultSkin
            LoadoutAPI.SkinDefInfo skinDefInfo = default(LoadoutAPI.SkinDefInfo);
            skinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            skinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            skinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            skinDefInfo.GameObjectActivations = getActivations(allObjects, shotgunModel, rifleModel, superShotgun, shieldModel, lightL, lightR);

            skinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texEnforcerAchievement");
            //skinDefInfo.Icon = LoadoutAPI.CreateSkinIcon(new Color(0.31f, 0.49f, 0.69f), new Color(0.86f, 0.83f, 0.63f), new Color(0.1f, 0.07f, 0.06f), new Color(0.21f, 0.29f, 0.38f));
            skinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    renderer = mainRenderer,
                    mesh = mainRenderer.sharedMesh
                }
            };
            skinDefInfo.Name = "ENFORCERBODY_DEFAULT_SKIN_NAME";
            skinDefInfo.NameToken = "ENFORCERBODY_DEFAULT_SKIN_NAME";
            skinDefInfo.RendererInfos = characterModel.baseRendererInfos;
            skinDefInfo.RootObject = model;
            skinDefInfo.UnlockableName = "";

            CharacterModel.RendererInfo[] rendererInfos = skinDefInfo.RendererInfos;
            CharacterModel.RendererInfo[] array = new CharacterModel.RendererInfo[rendererInfos.Length];
            rendererInfos.CopyTo(array, 0);

            //AGONY
            Material material = array[0].defaultMaterial;
            //body
            array[0].defaultMaterial = Assets.CreateMaterial("matEnforcerAlt", 2.5f, Color.white, 1);
            //shield
            array[2].defaultMaterial = Assets.CreateMaterial("matEquippedShield", 0f, Color.white, 1);
            //shotgun stuff
            array[1].defaultMaterial = Assets.CreateMaterial("matShotgun", 0f, Color.white, 0.5f);
            array[3].defaultMaterial = Assets.CreateMaterial("matShotgun", 0f, Color.white, 0.5f);
            array[4].defaultMaterial = Assets.CreateMaterial("matShotgun", 0f, Color.white, 0.5f);
            //assault rifle
            array[5].defaultMaterial = Assets.CreateMaterial("matRifle", 0f, Color.white, 0f);
            array[7].defaultMaterial = Assets.CreateMaterial("matShotgun", 0f, Color.white, 0.5f);
            //blaster
            array[8].defaultMaterial = Assets.CreateMaterial("matTemp", 0f, Color.white, 0f);
            array[9].defaultMaterial = Assets.CreateMaterial("matShotgun", 0f, Color.white, 0.5f);
            array[10].defaultMaterial = Assets.CreateMaterial("matTemp", 0f, Color.white, 0f);
            array[11].defaultMaterial = Assets.CreateMaterial("matShotgun", 0f, Color.white, 0.5f);
            //super shotgun
            array[12].defaultMaterial = Assets.CreateMaterial("matSuperShotgun", 0f, Color.white, 0f);
            array[13].defaultMaterial = Assets.CreateMaterial("matTemp", 0f, Color.white, 0f);
            array[14].defaultMaterial = Assets.CreateMaterial("matShotgun", 0f, Color.white, 0f);
            //not even sure at this point
            array[15].defaultMaterial = Assets.CreateMaterial("matShotgun", 0f, Color.white, 0f);
            array[16].defaultMaterial = Assets.CreateMaterial("matTemp", 0f, Color.white, 0f);
            array[17].defaultMaterial = Assets.CreateMaterial("matTemp", 0f, Color.white, 0f);
            //sexforcer shield
            array[18].defaultMaterial = Assets.CreateMaterial("matSexShield", 0f, Color.white, 0f);
            //needler
            array[20].defaultMaterial = Assets.CreateMaterial("matNeedler", 5f, Color.white, 0f);
            array[21].defaultMaterial = Assets.CreateMaterial("matShotgun", 0f, Color.white, 0f);
            //bungus
            array[22].defaultMaterial = EnforcerPlugin.bungusMat;
            //i think this is marauder shield
            material = array[24].defaultMaterial;
            material = UnityEngine.Object.Instantiate<Material>(Assets.commandoMat);
            material.SetColor("_Color", Color.black);
            material.SetTexture("_MainTex", null);
            material.SetFloat("_EmPower", 20);
            material.SetColor("_EmColor", Color.red);
            material.SetTexture("_EmTex", null);
            material.SetFloat("_NormalStrength", 0);
            array[24].defaultMaterial = material;
            //marauder shield arm attachment
            array[27].defaultMaterial = Assets.CreateMaterial("matMarauderArmShield", 20f, Color.white, 0f);
            //bungus shield arm attachment
            array[28].defaultMaterial = EnforcerPlugin.bungusMat;
            //femforcer shield
            array[29].defaultMaterial = Assets.CreateMaterial("matFemforcerShield", 0.69f, Color.white, 0f);
            //more bungus guns
            array[31].defaultMaterial = EnforcerPlugin.bungusMat;
            array[32].defaultMaterial = EnforcerPlugin.bungusMat;
            //pauldrons
            array[33].defaultMaterial = Assets.CreateMaterial("matEnforcerAlt", 1f, Color.white, 1);
            array[34].defaultMaterial = Assets.CreateMaterial("matEnforcerAlt", 1f, Color.white, 1);
            //skateboard
            array[35].defaultMaterial = Assets.CreateMaterial("matSkamtebord", 0f, Color.black, 0);
            //minecraft shield
            array[36].defaultMaterial = Assets.CreateMaterial("matCubeShield", 0f, Color.white, 0);
            //minecraft shotgun
            array[37].defaultMaterial = Assets.CreateMaterial("matCubeShotgun", 0f, Color.white, 0);
            //minecraft assault rifle
            array[38].defaultMaterial = Assets.CreateMaterial("matRifle", 0f, Color.white, 0);

            skinDefInfo.RendererInfos = array;

            SkinDef defaultSkin = LoadoutAPI.CreateNewSkinDef(skinDefInfo);
            #endregion

            #region Stormtrooper
            LoadoutAPI.SkinDefInfo spaceSkinDefInfo = default(LoadoutAPI.SkinDefInfo);
            spaceSkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            spaceSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            spaceSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            spaceSkinDefInfo.GameObjectActivations = getActivations(allObjects, blasterModel, blasterRifle, superBlaster, shieldModel, lightL, lightR);

            spaceSkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texStormtrooperAchievement");
            //spaceSkinDefInfo.Icon = LoadoutAPI.CreateSkinIcon(new Color(0.83f, 0.83f, 0.83f), new Color(0.64f, 0.64f, 0.64f), new Color(0.25f, 0.25f, 0.25f), new Color(0f, 0f, 0f));
            spaceSkinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    renderer = mainRenderer,
                    mesh = Assets.stormtrooperMesh
                }
            };
            spaceSkinDefInfo.Name = "ENFORCERBODY_SPACE_SKIN_NAME";
            spaceSkinDefInfo.NameToken = "ENFORCERBODY_SPACE_SKIN_NAME";
            spaceSkinDefInfo.RendererInfos = characterModel.baseRendererInfos;
            spaceSkinDefInfo.RootObject = model;
            spaceSkinDefInfo.UnlockableName = "ENFORCER_STORMTROOPERUNLOCKABLE_REWARD_ID";

            rendererInfos = skinDefInfo.RendererInfos;
            array = new CharacterModel.RendererInfo[rendererInfos.Length];
            rendererInfos.CopyTo(array, 0);

            array[0].defaultMaterial = Assets.CreateMaterial("matSpaceEnforcer", 0f, Color.black, 0f);
            array[2].defaultMaterial = Assets.CreateMaterial("matEquippedShieldWhite", 0f, Color.white, 0f);
            array[33].defaultMaterial = Assets.CreateMaterial("matSpaceEnforcer", 0f, Color.white, 0f);
            array[34].defaultMaterial = Assets.CreateMaterial("matSpaceEnforcer", 0f, Color.white, 0f);

            spaceSkinDefInfo.RendererInfos = array;

            SkinDef spaceSkin = LoadoutAPI.CreateNewSkinDef(spaceSkinDefInfo);
            #endregion

            #region Engi
            LoadoutAPI.SkinDefInfo engiSkinDefInfo = default(LoadoutAPI.SkinDefInfo);
            engiSkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            engiSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            engiSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            if (EnforcerPlugin.oldEngiShield.Value)
            {
                engiSkinDefInfo.GameObjectActivations = getActivations(allObjects, bungusShotgun, bungusRifle, bungusSSG, shieldModel, engiShield, lightL, lightR);
            }
            else
            {
                engiSkinDefInfo.GameObjectActivations = getActivations(allObjects, bungusShotgun, bungusRifle, bungusSSG, bungusShield, lightL, lightR);
            }

            engiSkinDefInfo.Icon = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/EngiBody").GetComponentInChildren<ModelSkinController>().skins[0].icon;
            engiSkinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    renderer = mainRenderer,
                    mesh = Assets.engiMesh
                }
            };
            engiSkinDefInfo.Name = "ENFORCERBODY_ENGI_SKIN_NAME";
            engiSkinDefInfo.NameToken = "ENFORCERBODY_ENGI_SKIN_NAME";
            engiSkinDefInfo.RendererInfos = characterModel.baseRendererInfos;
            engiSkinDefInfo.RootObject = model;
            engiSkinDefInfo.UnlockableName = "ENFORCER_BUNGUSUNLOCKABLE_REWARD_ID";

            rendererInfos = skinDefInfo.RendererInfos;
            array = new CharacterModel.RendererInfo[rendererInfos.Length];
            rendererInfos.CopyTo(array, 0);

            array[0].defaultMaterial = Assets.CreateMaterial("matEngiforcer", 1f, Color.white, 1f);
            array[2].defaultMaterial = Assets.CreateMaterial("matEquippedShieldEngi", 0f, Color.white, 0f);
            array[33].defaultMaterial = Assets.CreateMaterial("matEngiforcer", 3f, Color.white, 1f);
            array[34].defaultMaterial = Assets.CreateMaterial("matEngiforcer", 3f, Color.white, 1f);

            engiSkinDefInfo.RendererInfos = array;

            SkinDef engiSkin = LoadoutAPI.CreateNewSkinDef(engiSkinDefInfo);
            #endregion

            #region DoomGuy
            LoadoutAPI.SkinDefInfo doomSkinDefInfo = default(LoadoutAPI.SkinDefInfo);
            doomSkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            doomSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            doomSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            doomSkinDefInfo.GameObjectActivations = getActivations(allObjects, shotgunModel, rifleModel, superShotgun, marauderShield, lightL, lightR);

            doomSkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texDoomAchievement");
            //doomSkinDefInfo.Icon = LoadoutAPI.CreateSkinIcon(new Color(0.41f, 0.49f, 0.4f), new Color(0.14f, 0.18f, 0.16f), new Color(0.46f, 0.46f, 0.46f), new Color(0.64f, 0.64f, 0.64f));
            doomSkinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    renderer = mainRenderer,
                    mesh = mainRenderer.sharedMesh
                }
            };
            doomSkinDefInfo.Name = "ENFORCERBODY_DOOM_SKIN_NAME";
            doomSkinDefInfo.NameToken = "ENFORCERBODY_DOOM_SKIN_NAME";
            doomSkinDefInfo.RendererInfos = characterModel.baseRendererInfos;
            doomSkinDefInfo.RootObject = model;
            doomSkinDefInfo.UnlockableName = "ENFORCER_DOOMUNLOCKABLE_REWARD_ID";

            rendererInfos = skinDefInfo.RendererInfos;
            array = new CharacterModel.RendererInfo[rendererInfos.Length];
            rendererInfos.CopyTo(array, 0);

            array[0].defaultMaterial = Assets.CreateMaterial("matDoomEnforcer", 1f, Color.white, 1f);
            array[2].defaultMaterial = Assets.CreateMaterial("matEquippedShieldBlack", 0f, Color.white, 0f);
            array[33].defaultMaterial = Assets.CreateMaterial("matDoomEnforcer", 1f, Color.white, 1f);
            array[34].defaultMaterial = Assets.CreateMaterial("matDoomEnforcer", 1f, Color.white, 1f);

            doomSkinDefInfo.RendererInfos = array;

            SkinDef doomSkin = LoadoutAPI.CreateNewSkinDef(doomSkinDefInfo);
            #endregion

            #region Sexforcer
            LoadoutAPI.SkinDefInfo masterySkinDefInfo = default(LoadoutAPI.SkinDefInfo);
            masterySkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            masterySkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            masterySkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            masterySkinDefInfo.GameObjectActivations = getActivations(allObjects, shotgunModel, rifleModel, superShotgun, sexShield);

            masterySkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texEnforcerAchievement");
            //masterySkinDefInfo.Icon = LoadoutAPI.CreateSkinIcon(new Color(0.31f, 0.49f, 0.69f), new Color(0.86f, 0.83f, 0.63f), new Color(0.1f, 0.07f, 0.06f), new Color(0.21f, 0.29f, 0.38f));
            masterySkinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    renderer = mainRenderer,
                    mesh = Assets.sexMesh
                }
            };
            masterySkinDefInfo.Name = "ENFORCERBODY_MASTERY_SKIN_NAME";
            masterySkinDefInfo.NameToken = "ENFORCERBODY_MASTERY_SKIN_NAME";
            masterySkinDefInfo.RendererInfos = characterModel.baseRendererInfos;
            masterySkinDefInfo.RootObject = model;
            masterySkinDefInfo.UnlockableName = "ENFORCER_MASTERYUNLOCKABLE_REWARD_ID";

            rendererInfos = skinDefInfo.RendererInfos;
            array = new CharacterModel.RendererInfo[rendererInfos.Length];
            rendererInfos.CopyTo(array, 0);

            array[0].defaultMaterial = Assets.CreateMaterial("matSexforcer", 1f, Color.white, 0f);

            masterySkinDefInfo.RendererInfos = array;

            SkinDef masterySkin = LoadoutAPI.CreateNewSkinDef(masterySkinDefInfo);
            #endregion

            #region GrandMastery
            LoadoutAPI.SkinDefInfo grandMasterySkinDefInfo = default(LoadoutAPI.SkinDefInfo);
            grandMasterySkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            grandMasterySkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            grandMasterySkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            grandMasterySkinDefInfo.GameObjectActivations = getActivations(allObjects, shotgunModel, rifleModel, superShotgun, shieldModel);

            grandMasterySkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texTyphoonAchievement");
            //grandMasterySkinDefInfo.Icon = LoadoutAPI.CreateSkinIcon(new Color(0.31f, 0.49f, 0.69f), new Color(0.86f, 0.83f, 0.63f), new Color(0.1f, 0.07f, 0.06f), new Color(0.21f, 0.29f, 0.38f));
            grandMasterySkinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    renderer = mainRenderer,
                    mesh = Assets.gmMesh
                }
            };
            grandMasterySkinDefInfo.Name = "ENFORCERBODY_TYPHOON_SKIN_NAME";
            grandMasterySkinDefInfo.NameToken = "ENFORCERBODY_TYPHOON_SKIN_NAME";
            grandMasterySkinDefInfo.RendererInfos = characterModel.baseRendererInfos;
            grandMasterySkinDefInfo.RootObject = model;
            grandMasterySkinDefInfo.UnlockableName = "ENFORCER_TYPHOONUNLOCKABLE_REWARD_ID";

            rendererInfos = skinDefInfo.RendererInfos;
            array = new CharacterModel.RendererInfo[rendererInfos.Length];
            rendererInfos.CopyTo(array, 0);

            array[0].defaultMaterial = Assets.CreateMaterial("matEnforcerGM", 0f, Color.black, 0f);

            grandMasterySkinDefInfo.RendererInfos = array;

            SkinDef grandMasterySkin = LoadoutAPI.CreateNewSkinDef(grandMasterySkinDefInfo);
            #endregion

            #region Desperado
            LoadoutAPI.SkinDefInfo desperadoSkinDefInfo = default(LoadoutAPI.SkinDefInfo);
            desperadoSkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            desperadoSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            desperadoSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            desperadoSkinDefInfo.GameObjectActivations = getActivations(allObjects, shotgunModel, rifleModel, superShotgun, shieldModel, lightL, lightR);

            desperadoSkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texDesperadoAchievement");
            //desperadoSkinDefInfo.Icon = LoadoutAPI.CreateSkinIcon(new Color(0.43f, 0.1f, 0.1f), Color.red, new Color(0.31f, 0.04f, 0.07f), Color.black);
            desperadoSkinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    renderer = mainRenderer,
                    mesh = mainRenderer.sharedMesh
                }
            };
            desperadoSkinDefInfo.Name = "ENFORCERBODY_DESPERADO_SKIN_NAME";
            desperadoSkinDefInfo.NameToken = "ENFORCERBODY_DESPERADO_SKIN_NAME";
            desperadoSkinDefInfo.RendererInfos = characterModel.baseRendererInfos;
            desperadoSkinDefInfo.RootObject = model;
            desperadoSkinDefInfo.UnlockableName = "ENFORCER_DESPERADOUNLOCKABLE_REWARD_ID";

            rendererInfos = skinDefInfo.RendererInfos;
            array = new CharacterModel.RendererInfo[rendererInfos.Length];
            rendererInfos.CopyTo(array, 0);

            array[0].defaultMaterial = Assets.CreateMaterial("matEnforcerDesperado", 8f, Color.white, 1f);
            array[2].defaultMaterial = Assets.CreateMaterial("matRiotShieldDesperado", 0f, Color.white, 0f);
            array[33].defaultMaterial = Assets.CreateMaterial("matEnforcerDesperado", 1f, Color.white, 1f);
            array[34].defaultMaterial = Assets.CreateMaterial("matEnforcerDesperado", 1f, Color.white, 1f);

            desperadoSkinDefInfo.RendererInfos = array;

            SkinDef desperadoSkin = LoadoutAPI.CreateNewSkinDef(desperadoSkinDefInfo);
            #endregion

            #region Froge
            LoadoutAPI.SkinDefInfo frogSkinDefInfo = default(LoadoutAPI.SkinDefInfo);
            frogSkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            frogSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            frogSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            frogSkinDefInfo.GameObjectActivations = getActivations(allObjects, shotgunModel, rifleModel, superShotgun, shieldModel);

            frogSkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texZeroSuitAchievement");
            //frogSkinDefInfo.Icon = LoadoutAPI.CreateSkinIcon(new Color(0.13f, 0.10588f, 0.1137f), new Color(0.86f, 0.83f, 0.63f), new Color(0.13f, 0.07f, 0.04f), new Color(0.047f, 0.047f, 0.047f));
            frogSkinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    renderer = mainRenderer,
                    mesh = Assets.zeroSuitMesh
                }
            };
            frogSkinDefInfo.Name = "ENFORCERBODY_FROG_SKIN_NAME";
            frogSkinDefInfo.NameToken = "ENFORCERBODY_FROG_SKIN_NAME";
            frogSkinDefInfo.RendererInfos = characterModel.baseRendererInfos;
            frogSkinDefInfo.RootObject = model;
            frogSkinDefInfo.UnlockableName = "ENFORCER_FROGUNLOCKABLE_REWARD_ID";

            rendererInfos = skinDefInfo.RendererInfos;
            array = new CharacterModel.RendererInfo[rendererInfos.Length];
            rendererInfos.CopyTo(array, 0);

            array[0].defaultMaterial = Assets.CreateMaterial("matZeroSuit", 1f, Color.white, 1f);

            frogSkinDefInfo.RendererInfos = array;

            SkinDef frogSkin = LoadoutAPI.CreateNewSkinDef(frogSkinDefInfo);
            #endregion

            #region Femforcer
            LoadoutAPI.SkinDefInfo femSkinDefInfo = default(LoadoutAPI.SkinDefInfo);
            femSkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            femSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            femSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            femSkinDefInfo.GameObjectActivations = getActivations(allObjects, shotgunModel, rifleModel, superShotgun, femShield);

            //femSkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texEnforcerAchievement");
            femSkinDefInfo.Icon = LoadoutAPI.CreateSkinIcon(new Color(0.31f, 0.49f, 0.69f), new Color(0.86f, 0.83f, 0.63f), new Color(0.1f, 0.07f, 0.06f), new Color(0.21f, 0.29f, 0.38f), new Color(0.9f, 0.6f, 0.69f));
            femSkinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    renderer = mainRenderer,
                    mesh = Assets.femMesh
                }
            };
            femSkinDefInfo.Name = "ENFORCERBODY_FEM_SKIN_NAME";
            femSkinDefInfo.NameToken = "ENFORCERBODY_FEM_SKIN_NAME";
            femSkinDefInfo.RendererInfos = characterModel.baseRendererInfos;
            femSkinDefInfo.RootObject = model;
            femSkinDefInfo.UnlockableName = "";

            rendererInfos = skinDefInfo.RendererInfos;
            array = new CharacterModel.RendererInfo[rendererInfos.Length];
            rendererInfos.CopyTo(array, 0);

            material = array[0].defaultMaterial;

            array[0].defaultMaterial = Assets.CreateMaterial("matFemforcer", 2.5f, Color.white, 0.14f);
            array[0].defaultMaterial.SetFloat("_SpecularStrength", 0.5f);

            femSkinDefInfo.RendererInfos = array;

            SkinDef femSkin = LoadoutAPI.CreateNewSkinDef(femSkinDefInfo);
            #endregion

            #region Steve
            LoadoutAPI.SkinDefInfo fuckingSteveSkinDefInfo = default(LoadoutAPI.SkinDefInfo);
            fuckingSteveSkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            fuckingSteveSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            fuckingSteveSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            fuckingSteveSkinDefInfo.GameObjectActivations = getActivations(allObjects, cubeShotgun, cubeRifle, superShotgun, cubeShield);

            fuckingSteveSkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texSbeveAchievement");
            //fuckingSteveSkinDefInfo.Icon = LoadoutAPI.CreateSkinIcon(new Color(0.31f, 0.49f, 0.69f), new Color(0.86f, 0.83f, 0.63f), new Color(0.1f, 0.07f, 0.06f), new Color(0.21f, 0.29f, 0.38f));
            fuckingSteveSkinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    renderer = mainRenderer,
                    mesh = Assets.fuckingSteveMesh
                }
            };
            fuckingSteveSkinDefInfo.Name = "ENFORCERBODY_STEVE_SKIN_NAME";
            fuckingSteveSkinDefInfo.NameToken = "ENFORCERBODY_STEVE_SKIN_NAME";
            fuckingSteveSkinDefInfo.RendererInfos = characterModel.baseRendererInfos;
            fuckingSteveSkinDefInfo.RootObject = model;
            fuckingSteveSkinDefInfo.UnlockableName = "ENFORCER_STEVEUNLOCKABLE_REWARD_ID";

            rendererInfos = skinDefInfo.RendererInfos;
            array = new CharacterModel.RendererInfo[rendererInfos.Length];
            rendererInfos.CopyTo(array, 0);

            array[0].defaultMaterial = Assets.CreateMaterial("matFuckingSteve", 0.3f, Color.white, 0f);

            fuckingSteveSkinDefInfo.RendererInfos = array;

            SkinDef fuckingSteveSkin = LoadoutAPI.CreateNewSkinDef(fuckingSteveSkinDefInfo);
            #endregion

            #region Nemesis
            LoadoutAPI.SkinDefInfo nemesisSkinDefInfo = default(LoadoutAPI.SkinDefInfo);
            nemesisSkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            nemesisSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            nemesisSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            nemesisSkinDefInfo.GameObjectActivations = getActivations(allObjects, shotgunModel, rifleModel, superShotgun, shieldModel, lightL, lightR);

            nemesisSkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texNemforcerAchievement");
            //nemesisSkinDefInfo.Icon = LoadoutAPI.CreateSkinIcon(new Color(0.43f, 0.1f, 0.1f), Color.red, new Color(0.31f, 0.04f, 0.07f), Color.black);
            nemesisSkinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    renderer = mainRenderer,
                    mesh = mainRenderer.sharedMesh
                }
            };
            nemesisSkinDefInfo.Name = "ENFORCERBODY_NEMESIS_SKIN_NAME";
            nemesisSkinDefInfo.NameToken = "ENFORCERBODY_NEMESIS_SKIN_NAME";
            nemesisSkinDefInfo.RendererInfos = characterModel.baseRendererInfos;
            nemesisSkinDefInfo.RootObject = model;
            nemesisSkinDefInfo.UnlockableName = "ENFORCER_NEMESISSKINUNLOCKABLE_REWARD_ID";

            rendererInfos = skinDefInfo.RendererInfos;
            array = new CharacterModel.RendererInfo[rendererInfos.Length];
            rendererInfos.CopyTo(array, 0);

            array[0].defaultMaterial = Assets.CreateMaterial("matNemforcer", 3f, Color.white, 1f);
            array[2].defaultMaterial = Assets.CreateMaterial("matEquippedShieldBlack", 0f, Color.white, 0f);
            array[33].defaultMaterial = Assets.CreateMaterial("matNemforcer", 3f, Color.white, 1f);
            array[34].defaultMaterial = Assets.CreateMaterial("matNemforcer", 3f, Color.white, 1f);

            nemesisSkinDefInfo.RendererInfos = array;

            SkinDef nemesisSkin = LoadoutAPI.CreateNewSkinDef(nemesisSkinDefInfo);
            #endregion

            #region Pig
            LoadoutAPI.SkinDefInfo pigSkinDefInfo = fuckingSteveSkinDefInfo;

            pigSkinDefInfo.Name = "ENFORCERBODY_PIG_SKIN_NAME";
            pigSkinDefInfo.NameToken = "ENFORCERBODY_PIG_SKIN_NAME";
            pigSkinDefInfo.UnlockableName = "ENFORCER_PIGUNLOCKABLE_REWARD_ID";
            pigSkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texPigAchievement");

            rendererInfos = skinDefInfo.RendererInfos;
            array = new CharacterModel.RendererInfo[rendererInfos.Length];
            rendererInfos.CopyTo(array, 0);

            array[0].defaultMaterial = Assets.CreateMaterial("matPig", 0f, Color.white, 0f);

            pigSkinDefInfo.RendererInfos = array;

            SkinDef pigSkin = LoadoutAPI.CreateNewSkinDef(pigSkinDefInfo);
            #endregion
            */

            /*var skinDefs = new List<SkinDef>()
            {
                    defaultSkin,
                    masterySkin,
                    doomSkin,
                    desperadoSkin,
                    nemesisSkin
            };

            if (Config.cursed.Value)
            {
                skinDefs = new List<SkinDef>()
                {
                    defaultSkin,
                    masterySkin,
                    doomSkin,
                    engiSkin,
                    spaceSkin,
                    desperadoSkin,
                    nemesisSkin
                };
            }

            if (EnforcerPlugin.starstormInstalled)
            {
                // jesus fuck this is awful LMAO
                if (!Config.cursed.Value)
                {
                    skinDefs = new List<SkinDef>() {
                    defaultSkin,
                    masterySkin,
                    grandMasterySkin,
                    doomSkin,
                    desperadoSkin,
                    nemesisSkin
                };}
                else
                {
                    skinDefs = new List<SkinDef>() {
                    defaultSkin,
                    masterySkin,
                    grandMasterySkin,
                    doomSkin,
                    engiSkin,
                    spaceSkin,
                    desperadoSkin,
                    nemesisSkin
                };};

                EnforcerPlugin.doomGuyIndex++;
                EnforcerPlugin.engiIndex++;
                EnforcerPlugin.frogIndex++;
                EnforcerPlugin.stormtrooperIndex++;
            }

            if (Config.cursed.Value)
            {
                skinDefs.Add(frogSkin);
                skinDefs.Add(fuckingSteveSkin);
            }

            if (EnforcerPlugin.pig.Value)
            {
                skinDefs.Add(pigSkin);
            }

            if (EnforcerPlugin.femSkin.Value)
            {
                skinDefs.Add(femSkin);
            }*/
            #endregion

            skinController.skins = skinDefs.ToArray();

            for (int i = 0; i < skinDefs.Count; i++) {
                SkinIdices[(uint)i] = skinDefs[i].name;
            }
        }

        #region tools
        internal static List<GameObject> allGameObjectActivations = new List<GameObject>();
        /// <summary>
        /// create an array of all gameobjects that are activated/deactivated by skins, then for each skin pass in the specific objects that will be active
        /// </summary>
        /// <param name="activatedObjects">specific objects that will be active. pass in nothing for all objects to be off</param>
        /// <returns></returns>
        internal static SkinDef.GameObjectActivation[] getGameObjectActivations(params GameObject[] activatedObjects) {

            List<SkinDef.GameObjectActivation> GameObjectActivations = new List<SkinDef.GameObjectActivation>();

            for (int i = 0; i < allGameObjectActivations.Count; i++) {

                bool activate = activatedObjects.Contains(allGameObjectActivations[i]);

                GameObjectActivations.Add(new SkinDef.GameObjectActivation {
                    gameObject = allGameObjectActivations[i],
                    shouldActivate = activate
                });
            }

            return GameObjectActivations.ToArray();
        }

        internal static SkinDef.MeshReplacement[] getMeshReplacements(CharacterModel.RendererInfo[] rendererinfos, params string[] meshes) {

            List<SkinDef.MeshReplacement> meshReplacements = new List<SkinDef.MeshReplacement>();

            for (int i = 0; i < rendererinfos.Length; i++) {
                if (string.IsNullOrEmpty(meshes[i]))
                    continue;

                meshReplacements.Add(
                new SkinDef.MeshReplacement {
                    renderer = rendererinfos[i].renderer,
                    mesh = Assets.MainAssetBundle.LoadAsset<Mesh>(meshes[i])
                });
            }

            return meshReplacements.ToArray();
        }

        internal static SkinDef CreateSkinDef(SkinDefInfo skinDefInfo) {
            On.RoR2.SkinDef.Awake += DoNothing;

            SkinDef skinDef = ScriptableObject.CreateInstance<SkinDef>();
            skinDef.baseSkins = skinDefInfo.BaseSkins;
            skinDef.icon = skinDefInfo.Icon;
            skinDef.unlockableDef = skinDefInfo.UnlockableDef;
            skinDef.rootObject = skinDefInfo.RootObject;
            skinDef.rendererInfos = skinDefInfo.RendererInfos;
            skinDef.gameObjectActivations = skinDefInfo.GameObjectActivations;
            skinDef.meshReplacements = skinDefInfo.MeshReplacements;
            skinDef.projectileGhostReplacements = skinDefInfo.ProjectileGhostReplacements;
            skinDef.minionSkinReplacements = skinDefInfo.MinionSkinReplacements;
            skinDef.nameToken = skinDefInfo.NameToken;
            (skinDef as ScriptableObject).name = skinDefInfo.Name;

            On.RoR2.SkinDef.Awake -= DoNothing;

            return skinDef;
        }

        internal static SkinDef CreateSkinDef(string skinName, Sprite skinIcon, CharacterModel.RendererInfo[] rendererInfos, SkinnedMeshRenderer mainRenderer, GameObject root) {
            return CreateSkinDef(skinName, skinIcon, rendererInfos, mainRenderer, root, null);
        }

        internal static SkinDef CreateSkinDef(string skinName, Sprite skinIcon, CharacterModel.RendererInfo[] rendererInfos, SkinnedMeshRenderer mainRenderer, GameObject root, UnlockableDef unlockableDef) {
            SkinDefInfo skinDefInfo = new SkinDefInfo {
                BaseSkins = Array.Empty<SkinDef>(),
                GameObjectActivations = new SkinDef.GameObjectActivation[0],
                Icon = skinIcon,
                MeshReplacements = new SkinDef.MeshReplacement[0],
                MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0],
                Name = skinName,
                NameToken = skinName,
                ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0],
                RendererInfos = rendererInfos,
                RootObject = root,
                UnlockableDef = unlockableDef
            };

            On.RoR2.SkinDef.Awake += DoNothing;

            SkinDef skinDef = ScriptableObject.CreateInstance<SkinDef>();
            skinDef.baseSkins = skinDefInfo.BaseSkins;
            skinDef.icon = skinDefInfo.Icon;
            skinDef.unlockableDef = skinDefInfo.UnlockableDef;
            skinDef.rootObject = skinDefInfo.RootObject;
            skinDef.rendererInfos = skinDefInfo.RendererInfos;
            skinDef.gameObjectActivations = skinDefInfo.GameObjectActivations;
            skinDef.meshReplacements = skinDefInfo.MeshReplacements;
            skinDef.projectileGhostReplacements = skinDefInfo.ProjectileGhostReplacements;
            skinDef.minionSkinReplacements = skinDefInfo.MinionSkinReplacements;
            skinDef.nameToken = skinDefInfo.NameToken;
            skinDef.name = skinDefInfo.Name;

            On.RoR2.SkinDef.Awake -= DoNothing;

            return skinDef;
        }

        private static void DoNothing(On.RoR2.SkinDef.orig_Awake orig, SkinDef self) {
        }

        internal struct SkinDefInfo {
            internal SkinDef[] BaseSkins;
            internal Sprite Icon;
            internal string NameToken;
            internal UnlockableDef UnlockableDef;
            internal GameObject RootObject;
            internal CharacterModel.RendererInfo[] RendererInfos;
            internal SkinDef.MeshReplacement[] MeshReplacements;
            internal SkinDef.GameObjectActivation[] GameObjectActivations;
            internal SkinDef.ProjectileGhostReplacement[] ProjectileGhostReplacements;
            internal SkinDef.MinionSkinReplacement[] MinionSkinReplacements;
            internal string Name;
        }
        #endregion
    }
}