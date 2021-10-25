using System;
using UnityEngine;
using R2API;
using RoR2;
using R2API.Utils;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;

namespace EnforcerPlugin
{
    public static class Skins
    {

        public static void RegisterSkins() {
            GameObject bodyPrefab = EnforcerModPlugin.characterPrefab;
            GameObject modelTransform = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = modelTransform.GetComponent<CharacterModel>();
            ModelSkinController skinController = modelTransform.AddComponent<ModelSkinController>();
            ChildLocator childLocator = modelTransform.GetComponent<ChildLocator>();
            SkinnedMeshRenderer mainRenderer = characterModel.mainSkinnedMeshRenderer;
            List<SkinDef> skinDefs = new List<SkinDef>();

            #region LanguageTokens
            LanguageAPI.Add("ENFORCERBODY_DEFAULT_SKIN_NAME", "Default");
            LanguageAPI.Add("ENFORCERBODY_MASTERY_SKIN_NAME", "Peacekeeper");
            LanguageAPI.Add("ENFORCERBODY_CLASSIC_SKIN_NAME", "Classic");
            LanguageAPI.Add("ENFORCERBODY_BOT_SKIN_NAME", "N-4CR");

            LanguageAPI.Add("ENFORCERBODY_TYPHOON_SKIN_NAME", "Lawbringer");
            LanguageAPI.Add("ENFORCERBODY_SPACE_SKIN_NAME", "Rainstormtrooper");
            LanguageAPI.Add("ENFORCERBODY_ENGI_SKIN_NAME", "Engineer?");
            LanguageAPI.Add("ENFORCERBODY_DOOM_SKIN_NAME", "Doom Slayer");
            LanguageAPI.Add("ENFORCERBODY_DESPERADO_SKIN_NAME", "Desperado");
            LanguageAPI.Add("ENFORCERBODY_FEM_SKIN_NAME", "Femforcer");
            LanguageAPI.Add("ENFORCERBODY_FUCKINGSTEVE_SKIN_NAME", "Block");
            LanguageAPI.Add("ENFORCERBODY_NEMESIS_SKIN_NAME", "Nemesis");
            #endregion

            #region GameObjectActivations

            GameObject sexforcerGlass = childLocator.FindChild("ShieldGlassModel").gameObject;
            //GameObject pauldrons = childLocator.FindChild("PauldronModel").gameObject;

            allGameObjectActivations = new List<GameObject> {
                sexforcerGlass,
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

            defaultSkinDefInfo.GameObjectActivations = getGameObjectActivations();

            defaultSkinDefInfo.MeshReplacements = getMeshReplacements(characterModel.baseRendererInfos,
                "meshEnforcerShield",
                null,//sex shield glass
                "meshEnforcerSkamteBord",
                "meshEnforcerGun",
                "meshClassicGunSuper",
                "meshClassicGunHMG",
                "meshEnforcerHammer",
                "meshEnforcerPauldron",
                "meshEnforcer"
                );

            defaultSkinDefInfo.RendererInfos = characterModel.baseRendererInfos;

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

            masterySkinDefInfo.RendererInfos[0].defaultMaterial = Assets.CreateMaterial("matSexforcerShield", 0f, Color.black, 1f);
            //take default
            //masterySkinDefInfo.RendererInfos[1].defaultMaterial = Assets.CreateMaterial("matSexforcerShieldGlass", 0f, Color.black, 0);
            masterySkinDefInfo.RendererInfos[2].defaultMaterial = Assets.CreateMaterial("matSexforcerBoard", 0f, Color.white, 0f);
            //masterySkinDefInfo.RendererInfos[3].defaultMaterial = Assets.CreateMaterial("matEnforcerGun", 0f, Color.white, 0f);
            //masterySkinDefInfo.RendererInfos[4].defaultMaterial = Assets.CreateMaterial("matClassicGunSuper", 0f, Color.white, 0f);
            //masterySkinDefInfo.RendererInfos[5].defaultMaterial = Assets.CreateMaterial("matClassicGunHMG", 0f, Color.white, 0f);
            //masterySkinDefInfo.RendererInfos[6].defaultMaterial = Assets.CreateMaterial("matEnforcerHammer", 0f, Color.white, 0f);
            masterySkinDefInfo.RendererInfos[7].defaultMaterial = Assets.CreateMaterial("matSexforcer", 1f, Color.white, 0f);
            masterySkinDefInfo.RendererInfos[8].defaultMaterial = Assets.CreateMaterial("matSexforcer", 1f, Color.white, 0f);

            SkinDef masterySkin = CreateSkinDef(masterySkinDefInfo);
            skinDefs.Add(masterySkin);
            #endregion

            #region robit
            SkinDefInfo robitSkinDefInfo = new SkinDefInfo();
            robitSkinDefInfo.Name = "ENFORCERBODY_BOT_SKIN_NAME";
            robitSkinDefInfo.NameToken = "ENFORCERBODY_BOT_SKIN_NAME";
            robitSkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texNemforcerEnforcer");
            //robitSkinDefInfo.UnlockableDef = EnforcerUnlockables.enforcerMasteryUnlockableDef;
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

            robitSkinDefInfo.RendererInfos[0].defaultMaterial = Assets.CreateMaterial("matN4CR", 1f, Color.white, 0f);
            //[1] take default
            //[2] take default
            robitSkinDefInfo.RendererInfos[3].defaultMaterial = Assets.CreateMaterial("matN4CR", 1f, Color.white, 0f);
            robitSkinDefInfo.RendererInfos[4].defaultMaterial = Assets.CreateMaterial("matN4CR", 1f, Color.white, 0f);
            robitSkinDefInfo.RendererInfos[5].defaultMaterial = Assets.CreateMaterial("matN4CR", 1f, Color.white, 0f);
            //[6] take default hammer
            robitSkinDefInfo.RendererInfos[7].defaultMaterial = Assets.CreateMaterial("matN4CR", 1f, Color.white, 0f);
            robitSkinDefInfo.RendererInfos[8].defaultMaterial = Assets.CreateMaterial("matN4CR", 1f, Color.white, 0f);

            SkinDef robitSkinDef = CreateSkinDef(robitSkinDefInfo);
            skinDefs.Add(robitSkinDef);
            #endregion

            #region Mastery
            SkinDefInfo classicSkinDefInfo = new SkinDefInfo();
            classicSkinDefInfo.Name = "ENFORCERBODY_CLASSIC_SKIN_NAME";
            classicSkinDefInfo.NameToken = "ENFORCERBODY_CLASSIC_SKIN_NAME";
            classicSkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texEnforcerAchievement");
            //classicSkinDefInfo.UnlockableDef = EnforcerUnlockables.enforcerMasteryUnlockableDef;
            classicSkinDefInfo.RootObject = modelTransform;

            classicSkinDefInfo.BaseSkins = new SkinDef[] { defaultSkinDef };
            classicSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            classicSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            classicSkinDefInfo.GameObjectActivations = getGameObjectActivations();

            classicSkinDefInfo.MeshReplacements = getMeshReplacements(characterModel.baseRendererInfos,
                null,//"meshEnforcerShield",
                null,//"meshSexforcerShieldGlass",
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
            classicSkinDefInfo.RendererInfos[3].defaultMaterial = Assets.CreateMaterial("matClassicGun", 0f, Color.white, 0f);
            classicSkinDefInfo.RendererInfos[4].defaultMaterial = Assets.CreateMaterial("matClassicGunSuper", 0f, Color.white, 0f);
            classicSkinDefInfo.RendererInfos[5].defaultMaterial = Assets.CreateMaterial("matClassicGunHMG", 0f, Color.white, 0f);
            //classicSkinDefInfo.RendererInfos[6].defaultMaterial = Assets.CreateMaterial("matEnforcerHammer", 0f, Color.white, 0f);
            classicSkinDefInfo.RendererInfos[7].defaultMaterial = Assets.CreateMaterial("matClassic", 1f, Color.white, 0f);
            classicSkinDefInfo.RendererInfos[8].defaultMaterial = Assets.CreateMaterial("matClassic", 1f, Color.white, 0f);

            SkinDef classicSkin = CreateSkinDef(classicSkinDefInfo);
            skinDefs.Add(classicSkin);
            #endregion

            #region If she don't play the craft
            SkinDefInfo dontgettheshaftSkinDefInfo = new SkinDefInfo();
            dontgettheshaftSkinDefInfo.Name = "ENFORCERBODY_FUCKINGSTEVE_SKIN_NAME";
            dontgettheshaftSkinDefInfo.NameToken = "ENFORCERBODY_FUCKINGSTEVE_SKIN_NAME";
            dontgettheshaftSkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texSbeveAchievement");
            dontgettheshaftSkinDefInfo.UnlockableDef = EnforcerUnlockables.enforcerMasteryUnlockableDef;
            dontgettheshaftSkinDefInfo.RootObject = modelTransform;

            dontgettheshaftSkinDefInfo.BaseSkins = new SkinDef[] { defaultSkinDef };
            dontgettheshaftSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            dontgettheshaftSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            dontgettheshaftSkinDefInfo.GameObjectActivations = getGameObjectActivations();
            dontgettheshaftSkinDefInfo.GameObjectActivations.AddItem(new SkinDef.GameObjectActivation {
                gameObject = childLocator.FindChild("PauldronModel").gameObject,
                shouldActivate = false
            });

            dontgettheshaftSkinDefInfo.MeshReplacements = getMeshReplacements(characterModel.baseRendererInfos,
                "meshSbeveShield",
                null,//"meshSexforcerShieldGlass",
                "meshSbeveBoard",
                "meshSbeveGun",
                "meshSbeveGunSuper",
                "meshSbeveGunHMG",
                "meshSbeveHammer",
                "meshSbevePauldron",
                "meshSbeve"
                ); 
            dontgettheshaftSkinDefInfo.RendererInfos = characterModel.baseRendererInfos;

            dontgettheshaftSkinDefInfo.RendererInfos = new CharacterModel.RendererInfo[defaultSkinDef.rendererInfos.Length];
            defaultSkinDef.rendererInfos.CopyTo(dontgettheshaftSkinDefInfo.RendererInfos, 0);

            dontgettheshaftSkinDefInfo.RendererInfos[7].defaultMaterial = Assets.CreateMaterial("matFuckingSteve", 1f, Color.white, 0f);

            SkinDef dontgettheshaftSkin = CreateSkinDef(dontgettheshaftSkinDefInfo);

            //wait more model
            //if(EnforcerModPlugin.cursed.Value)
            //    skinDefs.Add(dontgettheshaftSkin);
            #endregion

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

            engiSkinDefInfo.Icon = Resources.Load<GameObject>("Prefabs/CharacterBodies/EngiBody").GetComponentInChildren<ModelSkinController>().skins[0].icon;
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
            masterySkinDefInfo.UnlockableName = "ENFORCER_MONSOONUNLOCKABLE_REWARD_ID";

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

            if (EnforcerPlugin.cursed.Value)
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
                if (!EnforcerPlugin.cursed.Value)
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

            if (EnforcerPlugin.cursed.Value)
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

                bool activate =  activatedObjects.Contains(allGameObjectActivations[i]);

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

        internal static SkinDef CreateSkinDef(SkinDefInfo skinDefInfo)
        {
            On.RoR2.SkinDef.Awake += DoNothing;

            SkinDef skinDef = ScriptableObject.CreateInstance<RoR2.SkinDef>();
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

        internal static SkinDef CreateSkinDef(string skinName, Sprite skinIcon, CharacterModel.RendererInfo[] rendererInfos, SkinnedMeshRenderer mainRenderer, GameObject root)
        {
            return CreateSkinDef(skinName, skinIcon, rendererInfos, mainRenderer, root, null);
        }

        internal static SkinDef CreateSkinDef(string skinName, Sprite skinIcon, CharacterModel.RendererInfo[] rendererInfos, SkinnedMeshRenderer mainRenderer, GameObject root, UnlockableDef unlockableDef)
        {
            SkinDefInfo skinDefInfo = new SkinDefInfo
            {
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

            SkinDef skinDef = ScriptableObject.CreateInstance<RoR2.SkinDef>();
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

        private static void DoNothing(On.RoR2.SkinDef.orig_Awake orig, RoR2.SkinDef self)
        {
        }

        internal struct SkinDefInfo
        {
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