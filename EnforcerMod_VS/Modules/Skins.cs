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
            FUCKINGFROG,
            FEMFORCER
        }

        public static SkinDef engiNormalSkin;
        public static SkinDef engiBungusSkin;
        public static SkinDef podDoorSkin;

        public static Dictionary<uint, string> SkinIndices = new Dictionary<uint, string>();

        public static List<SkinDef> skinDefs = new List<SkinDef>();

        private static bool isEnforcerCurrentSkin(CharacterBody characterbody, string skin) {

            return characterbody.baseNameToken == "ENFORCER_NAME" && SkinIndices.ContainsKey(characterbody.skinIndex) && SkinIndices[characterbody.skinIndex] == skin;
        }

        public static bool isEnforcerCurrentSkin(CharacterBody characterbody, EnforcerSkin skin)
        {
            if (skin == EnforcerSkin.RECOLORENGIBUNG) {
                return characterbody.inventory.GetItemCount(RoR2Content.Items.Mushroom) > 0 && isEnforcerCurrentSkin(characterbody, GetFuckingSkinID(EnforcerSkin.RECOLORENGI));
            }

            return isEnforcerCurrentSkin(characterbody, GetFuckingSkinID(skin));
        }

        public static int getEnforcerSkinIndex(EnforcerSkin skin) {

            foreach (KeyValuePair<uint, string> dicPair in SkinIndices) {
                if(dicPair.Value == GetFuckingSkinID(skin)) {
                    return (int)dicPair.Key;
                }
            }
            return 0;
        }

        private static string GetFuckingSkinID(EnforcerSkin skin)
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
                case EnforcerSkin.RECOLORSTORM:
                    return "ENFORCERBODY_STORM_SKIN_NAME";
                case EnforcerSkin.RECOLORDESPERADO:
                    return "ENFORCERBODY_DESPERADO_SKIN_NAME";
                case EnforcerSkin.RECOLORENGI:
                    return "ENFORCERBODY_ENGI_SKIN_NAME";
                case EnforcerSkin.RECOLORNEMESIS:
                    return "ENFORCERBODY_NEMESIS_SKIN_NAME";
                    //doesn't work
                case EnforcerSkin.RECOLORENGIBUNG:
                    return "ENFORCERBODY_ENGIBUNGUS_SKIN_NAME";
                case EnforcerSkin.RECOLORDOOM:
                    return "ENFORCERBODY_DOOM_SKIN_NAME";
                case EnforcerSkin.FUCKINGSTEVE:
                    return "ENFORCERBODY_FUCKINGSTEVE_SKIN_NAME";
                case EnforcerSkin.FUCKINGFROG:
                    return "ENFORCERBODY_FUCKINGFROG_SKIN_NAME";
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
            GameObject pauldrons = childLocator.FindChild("PauldronModel").gameObject;
            
            allGameObjectActivations = new List<GameObject> {
                sexforcerGlass,
                pauldrons
            };

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

            defaultSkinDefInfo.GameObjectActivations = getGameObjectActivations(sexforcerGlass, pauldrons);

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

            masterySkinDefInfo.GameObjectActivations = getGameObjectActivations(sexforcerGlass, pauldrons);

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

            grandMasterySkinDefInfo.GameObjectActivations = getGameObjectActivations(sexforcerGlass, pauldrons);

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
            robitSkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texN4CRAchievement");
            robitSkinDefInfo.UnlockableDef = EnforcerUnlockables.enforcerRobitUnlockableDef;
            robitSkinDefInfo.RootObject = modelTransform;

            robitSkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            robitSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            robitSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            robitSkinDefInfo.GameObjectActivations = getGameObjectActivations(pauldrons);

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
            classicSkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texClassicAchievement");
            classicSkinDefInfo.UnlockableDef = EnforcerUnlockables.enforcerClassicUnlockableDef;
            classicSkinDefInfo.RootObject = modelTransform;

            classicSkinDefInfo.BaseSkins = new SkinDef[] { defaultSkinDef };
            classicSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            classicSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            classicSkinDefInfo.GameObjectActivations = getGameObjectActivations(pauldrons);

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
            nemesisSkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texNemSkinAchievement");
            nemesisSkinDefInfo.UnlockableDef = EnforcerUnlockables.enforcerNemesisSkinUnlockableDef;
            nemesisSkinDefInfo.RootObject = modelTransform;

            nemesisSkinDefInfo.BaseSkins = new SkinDef[] { defaultSkinDef };
            nemesisSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            nemesisSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            nemesisSkinDefInfo.GameObjectActivations = getGameObjectActivations(sexforcerGlass, pauldrons);

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

            #region Door
            SkinDefInfo podDoorSkinDefInfo = new SkinDefInfo();
            podDoorSkinDefInfo.Name = "ENFORCERBODY_DOOR_SKIN_NAME";
            podDoorSkinDefInfo.NameToken = "ENFORCERBODY_DOOR_SKIN_NAME";
            podDoorSkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texEnforcerAchievement");
            podDoorSkinDefInfo.RootObject = modelTransform;
            
            podDoorSkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            podDoorSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            podDoorSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];
            
            podDoorSkinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[] {
                new SkinDef.GameObjectActivation{
                    gameObject = sexforcerGlass,
                    shouldActivate = true,
                } 
            };

            podDoorSkinDefInfo.MeshReplacements = getMeshReplacements(characterModel.baseRendererInfos,
                "meshEscapePodDoor",
                "meshEscapePodDoorGlass",
                null, //"meshEnforcerSkamteBord",
                null, //"meshEnforcerGun",
                null, //"meshClassicGunSuper",
                null, //"meshClassicGunHMG",
                null, //"meshEnforcerHammer",
                null, //"meshEnforcerPauldron",
                null //"meshEnforcer"
                );

            podDoorSkinDefInfo.RendererInfos = new CharacterModel.RendererInfo[] {
                new CharacterModel.RendererInfo {
                    defaultMaterial = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<Material>("RoR2/Base/SurvivorPod/matEscapePod.mat").WaitForCompletion(),
                    renderer = characterModel.baseRendererInfos[0].renderer
                },
                //new CharacterModel.RendererInfo {
                //    defaultMaterial = Materials.CreateHotpooMaterial("matPodDoorShieldGlass"),
                //    renderer = characterModel.baseRendererInfos[1].renderer
                //}
            };

            podDoorSkin = CreateSkinDef(podDoorSkinDefInfo);
            #endregion


            if (!Config.hateFun.Value) {

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

                desperadoSkinDefInfo.GameObjectActivations = getGameObjectActivations(sexforcerGlass, pauldrons);

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

                doomSkinDefInfo.GameObjectActivations = getGameObjectActivations(sexforcerGlass, pauldrons);

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

                engiSkinDefInfo.GameObjectActivations = getGameObjectActivations(sexforcerGlass, pauldrons);

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
                engiNormalSkin = engiSkin;
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

                engiBungusSkinDefInfo.GameObjectActivations = getGameObjectActivations(sexforcerGlass, pauldrons);

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

                stormSkinDefInfo.GameObjectActivations = getGameObjectActivations(sexforcerGlass, pauldrons);
                
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
                
                minecraftSkinDefInfo.GameObjectActivations = getGameObjectActivations();

                minecraftSkinDefInfo.MeshReplacements = getMeshReplacements(characterModel.baseRendererInfos,
                    "meshFuckingSteve2Shield",
                    null,//"meshSexforcerShieldGlass",
                    null,//"meshSbeveBoard",
                    "meshFuckingSteve2Gun2",
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
                minecraftSkinDefInfo.RendererInfos[3].defaultMaterial = Materials.CreateHotpooMaterial("matFuckingSteve2Gun");
                minecraftSkinDefInfo.RendererInfos[8].defaultMaterial = Materials.CreateHotpooMaterial("matFuckingSteve2");

                SkinDef minecraftSkin = CreateSkinDef(minecraftSkinDefInfo);

                //wait more model
                skinDefs.Add(minecraftSkin);
                #endregion she don't get the shaft
                
                #region RecolorFuckingFrog
                SkinDefInfo fuckingFrogSkinDefInfo = new SkinDefInfo();
                fuckingFrogSkinDefInfo.Name = "ENFORCERBODY_FUCKINGFROG_SKIN_NAME";
                fuckingFrogSkinDefInfo.NameToken = "ENFORCERBODY_FUCKINGFROG_SKIN_NAME";
                fuckingFrogSkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texZeroSuitAchievement");
                fuckingFrogSkinDefInfo.UnlockableDef = EnforcerUnlockables.enforcerFrogUnlockableDef;
                fuckingFrogSkinDefInfo.RootObject = modelTransform;

                fuckingFrogSkinDefInfo.BaseSkins = new SkinDef[] { defaultSkinDef };
                fuckingFrogSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
                fuckingFrogSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

                fuckingFrogSkinDefInfo.GameObjectActivations = getGameObjectActivations(sexforcerGlass);
                fuckingFrogSkinDefInfo.MeshReplacements = getMeshReplacements(characterModel.baseRendererInfos,
                    null,
                    null,
                    null,//board
                    null,//"meshEnforcerGun",
                    null,//"meshClassicGunSuper",
                    null,//"meshClassicGunHMG",
                    null,//"meshEnforcerHammer",
                    null,
                    "meshFuckingFrog"
                    );

                fuckingFrogSkinDefInfo.RendererInfos = new CharacterModel.RendererInfo[defaultSkinDef.rendererInfos.Length];
                defaultSkinDef.rendererInfos.CopyTo(fuckingFrogSkinDefInfo.RendererInfos, 0);

                //fuckingFrogSkinDefInfo.RendererInfos[0].defaultMaterial = Materials.CreateHotpooMaterial("matRecolorFuckingFrogShield");
                //fuckingFrogSkinDefInfo.RendererInfos[1].defaultMaterial = Assets.LoadAsset<Material>("matRecolorFuckingFrogShieldGlass");
                ////fuckingFrogSkinDefInfo.RendererInfos[2].defaultMaterial = Materials.CreateHotpooMaterial("matSexforcerBoard");
                //fuckingFrogSkinDefInfo.RendererInfos[3].defaultMaterial = Materials.CreateHotpooMaterial("matRecolorFuckingFrogGun");
                ////fuckingFrogSkinDefInfo.RendererInfos[4].defaultMaterial = Assets.CreateMaterial("matClassicGunSuper", 0f, Color.white, 0f);
                ////fuckingFrogSkinDefInfo.RendererInfos[5].defaultMaterial = Assets.CreateMaterial("matClassicGunHMG", 0f, Color.white, 0f);
                ////fuckingFrogSkinDefInfo.RendererInfos[6].defaultMaterial = Materials.CreateHotpooMaterial("matNemforcerClassic");
                //fuckingFrogSkinDefInfo.RendererInfos[7].defaultMaterial = Materials.CreateHotpooMaterial("matRecolorFuckingFrog").MakeUnique();
                fuckingFrogSkinDefInfo.RendererInfos[8].defaultMaterial = Materials.CreateHotpooMaterial("matRecolorNem");

                SkinDef fuckingFrogSkin = CreateSkinDef(fuckingFrogSkinDefInfo);
                skinDefs.Add(fuckingFrogSkin);
                #endregion

            }

            if (Config.femSkin.Value) {
                #region fem
                SkinDefInfo femforcerSkinDefInfo = new SkinDefInfo();
                femforcerSkinDefInfo.Name = "ENFORCERBODY_FEMFORCER_SKIN_NAME";
                femforcerSkinDefInfo.NameToken = "ENFORCERBODY_FEMFORCER_SKIN_NAME";
                femforcerSkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texFemSkin");
                //femforcerSkinDefInfo.Icon = LoadoutAPI.CreateSkinIcon(new Color(0.31f, 0.49f, 0.69f), new Color(0.86f, 0.83f, 0.63f), new Color(0.1f, 0.07f, 0.06f), new Color(0.21f, 0.29f, 0.38f), new Color(0.9f, 0.6f, 0.69f));

                femforcerSkinDefInfo.UnlockableDef = EnforcerUnlockables.enforcerMasteryUnlockableDef;
                femforcerSkinDefInfo.RootObject = modelTransform;

                femforcerSkinDefInfo.BaseSkins = new SkinDef[] { defaultSkinDef };
                femforcerSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
                femforcerSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

                femforcerSkinDefInfo.GameObjectActivations = getGameObjectActivations(sexforcerGlass, pauldrons);

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
            #region FUCK
            //I'M GONNA FIX IT ALL MOTHER FUCKER
            //ONLY TOOK A YEAR OR WHATEVER
            #endregion

            skinController.skins = skinDefs.ToArray();

            for (int i = 0; i < skinDefs.Count; i++) {
                SkinIndices[(uint)i] = skinDefs[i].name;
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