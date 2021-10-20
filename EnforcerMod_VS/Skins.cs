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

        private static List<GameObject> allGameObjectActivations = new List<GameObject>();

        /// <summary>
        /// create an array of all gameobjects that are activated/deactivated by skins, then for each skin pass in the specific objects that will be active
        /// </summary>
        /// <param name="allObjects">array of all gameobjects that are activated/deactivated by skins</param>
        /// <param name="activatedObjects">specific objects that will be active</param>
        /// <returns></returns>
        private static SkinDef.GameObjectActivation[] getActivations(params GameObject[] activatedObjects) {

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

        public static void RegisterSkins()
        {
            GameObject bodyPrefab = EnforcerModPlugin.characterPrefab;
            GameObject model = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = model.GetComponent<CharacterModel>();
            ModelSkinController skinController = model.AddComponent<ModelSkinController>();
            ChildLocator childLocator = model.GetComponent<ChildLocator>();
            SkinnedMeshRenderer mainRenderer = characterModel.mainSkinnedMeshRenderer;
            List<SkinDef> skinDefs = new List<SkinDef>();

            #region LanguageTokens
            LanguageAPI.Add("ENFORCERBODY_DEFAULT_SKIN_NAME", "Default");
            LanguageAPI.Add("ENFORCERBODY_MASTERY_SKIN_NAME", "Peacekeeper");
            LanguageAPI.Add("ENFORCERBODY_TYPHOON_SKIN_NAME", "Lawbringer");
            LanguageAPI.Add("ENFORCERBODY_CLASSIC_SKIN_NAME", "Classic");
            LanguageAPI.Add("ENFORCERBODY_BOT_SKIN_NAME", "N-4CR");
            LanguageAPI.Add("ENFORCERBODY_SPACE_SKIN_NAME", "Rainstormtrooper");
            LanguageAPI.Add("ENFORCERBODY_ENGI_SKIN_NAME", "Engineer?");
            LanguageAPI.Add("ENFORCERBODY_DOOM_SKIN_NAME", "Doom Slayer");
            LanguageAPI.Add("ENFORCERBODY_DESPERADO_SKIN_NAME", "Desperado");
            LanguageAPI.Add("ENFORCERBODY_FROG_SKIN_NAME", "Zero Suit");
            LanguageAPI.Add("ENFORCERBODY_FEM_SKIN_NAME", "Femforcer");
            LanguageAPI.Add("ENFORCERBODY_STEVE_SKIN_NAME", "Minecraft");
            LanguageAPI.Add("ENFORCERBODY_PIG_SKIN_NAME", "Pig");
            LanguageAPI.Add("ENFORCERBODY_NEMESIS_SKIN_NAME", "Nemesis");
            #endregion

            #region GameObjectActivations

            GameObject sexforcerGlass = childLocator.FindChild("ShieldGlassModel").gameObject;

            allGameObjectActivations = new List<GameObject> {
                sexforcerGlass,
            };

            #endregion

            #region default
            SkinDefInfo defaultSkinDefInfo = new SkinDefInfo();
            defaultSkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            defaultSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            defaultSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            defaultSkinDefInfo.GameObjectActivations = getActivations();

            defaultSkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texEnforcerAchievement");
            //skinDefInfo.Icon = LoadoutAPI.CreateSkinIcon(new Color(0.31f, 0.49f, 0.69f), new Color(0.86f, 0.83f, 0.63f), new Color(0.1f, 0.07f, 0.06f), new Color(0.21f, 0.29f, 0.38f));
            defaultSkinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    renderer = characterModel.baseRendererInfos[0].renderer,
                    mesh = Assets.MainAssetBundle.LoadAsset<Mesh>("meshEnforcerShield")
                },
                //[1] shield glass
                new SkinDef.MeshReplacement
                {
                    renderer = characterModel.baseRendererInfos[2].renderer,
                    mesh = Assets.MainAssetBundle.LoadAsset<Mesh>("meshEnforcerGun")
                },
                new SkinDef.MeshReplacement
                {
                    renderer = characterModel.baseRendererInfos[3].renderer,
                    mesh = Assets.MainAssetBundle.LoadAsset<Mesh>("meshClassicGunSuper")
                },
                new SkinDef.MeshReplacement
                {
                    renderer = characterModel.baseRendererInfos[4].renderer,
                    mesh = Assets.MainAssetBundle.LoadAsset<Mesh>("meshClassicGunHMG")
                },
                new SkinDef.MeshReplacement
                {
                    renderer = characterModel.baseRendererInfos[5].renderer,
                    mesh = Assets.MainAssetBundle.LoadAsset<Mesh>("meshEnforcerHammer")
                },
                new SkinDef.MeshReplacement
                {
                    renderer = characterModel.baseRendererInfos[6].renderer,
                    mesh = Assets.MainAssetBundle.LoadAsset<Mesh>("meshEnforcerPauldron")
                },
                new SkinDef.MeshReplacement
                {
                    renderer = characterModel.baseRendererInfos[7].renderer,
                    mesh = Assets.MainAssetBundle.LoadAsset<Mesh>("meshEnforcer")
                }
            };
            defaultSkinDefInfo.Name = "ENFORCERBODY_DEFAULT_SKIN_NAME";
            defaultSkinDefInfo.NameToken = "ENFORCERBODY_DEFAULT_SKIN_NAME";
            defaultSkinDefInfo.RootObject = model;
            defaultSkinDefInfo.RendererInfos = characterModel.baseRendererInfos;

            SkinDef defaultSkin = CreateSkinDef(defaultSkinDefInfo);
            skinDefs.Add(defaultSkin);
            #endregion

            #region Mastery
            SkinDefInfo masterySkinDefInfo = new SkinDefInfo();
            masterySkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            masterySkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            masterySkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];
            masterySkinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[0];

            masterySkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texSexforcerAchievement");
            masterySkinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    renderer = characterModel.baseRendererInfos[0].renderer,
                    mesh = Assets.MainAssetBundle.LoadAsset<Mesh>("meshSexforcerShield")
                },
                new SkinDef.MeshReplacement
                {
                    renderer = characterModel.baseRendererInfos[1].renderer,
                    mesh = Assets.MainAssetBundle.LoadAsset<Mesh>("meshSexforcerShieldGlass")
                },
                new SkinDef.MeshReplacement
                {
                    renderer = characterModel.baseRendererInfos[2].renderer,
                    mesh = Assets.MainAssetBundle.LoadAsset<Mesh>("meshEnforcerGun")
                },
                new SkinDef.MeshReplacement
                {
                    renderer = characterModel.baseRendererInfos[3].renderer,
                    mesh = Assets.MainAssetBundle.LoadAsset<Mesh>("meshClassicGunSuper")
                },
                new SkinDef.MeshReplacement
                {
                    renderer = characterModel.baseRendererInfos[4].renderer,
                    mesh = Assets.MainAssetBundle.LoadAsset<Mesh>("meshClassicGunHMG")
                },
                new SkinDef.MeshReplacement
                {
                    renderer = characterModel.baseRendererInfos[5].renderer,
                    mesh = Assets.MainAssetBundle.LoadAsset<Mesh>("meshEnforcerHammer")
                },
                new SkinDef.MeshReplacement
                {
                    renderer = characterModel.baseRendererInfos[6].renderer,
                    mesh = Assets.MainAssetBundle.LoadAsset<Mesh>("meshSexforcerPauldron")
                },
                new SkinDef.MeshReplacement
                {
                    renderer = characterModel.baseRendererInfos[7].renderer,
                    mesh = Assets.MainAssetBundle.LoadAsset<Mesh>("meshSexforcer")
                }
            };
            masterySkinDefInfo.Name = "ENFORCERBODY_MASTERY_SKIN_NAME";
            masterySkinDefInfo.NameToken = "ENFORCERBODY_MASTERY_SKIN_NAME";
            masterySkinDefInfo.RootObject = model;
            masterySkinDefInfo.UnlockableDef = EnforcerUnlockables.enforcerMasteryUnlockableDef;
            masterySkinDefInfo.RendererInfos = characterModel.baseRendererInfos;

            CharacterModel.RendererInfo[] defaultRendererInfos = new CharacterModel.RendererInfo[defaultSkin.rendererInfos.Length];
            defaultSkin.rendererInfos.CopyTo(defaultRendererInfos, 0);

            masterySkinDefInfo.RendererInfos = defaultRendererInfos;
            masterySkinDefInfo.RendererInfos[0].defaultMaterial = Assets.CreateMaterial("matSexforcerShield", 0f, Color.black, 1f);
            //take default
            //masterySkinDefInfo.RendererInfos[1].defaultMaterial = Assets.CreateMaterial("matSexforcerShieldGlass", 0f, Color.black, 0);
            masterySkinDefInfo.RendererInfos[2].defaultMaterial = Assets.CreateMaterial("matEnforcerGun", 0f, Color.white, 0f);
            //take default
            //masterySkinDefInfo.RendererInfos[3].defaultMaterial = Assets.CreateMaterial("matClassicGunSuper", 0f, Color.white, 0f);
            //masterySkinDefInfo.RendererInfos[4].defaultMaterial = Assets.CreateMaterial("matClassicGunHMG", 0f, Color.white, 0f);
            //masterySkinDefInfo.RendererInfos[5].defaultMaterial = Assets.CreateMaterial("matEnforcerHammer", 0f, Color.white, 0f);
            masterySkinDefInfo.RendererInfos[6].defaultMaterial = Assets.CreateMaterial("matSexforcerPauldron", 1f, Color.white, 0f);
            masterySkinDefInfo.RendererInfos[7].defaultMaterial = Assets.CreateMaterial("matSexforcer", 1f, Color.white, 0f);

            SkinDef masterySkin = CreateSkinDef(masterySkinDefInfo);
            skinDefs.Add(masterySkin);
            #endregion

            #region robit
            SkinDefInfo robitSkinDefInfo = new SkinDefInfo();
            robitSkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            robitSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            robitSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];
            robitSkinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[0];

            robitSkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texNemforcerEnforcer");
            //skinDefInfo.Icon = LoadoutAPI.CreateSkinIcon(new Color(0.31f, 0.49f, 0.69f), new Color(0.86f, 0.83f, 0.63f), new Color(0.1f, 0.07f, 0.06f), new Color(0.21f, 0.29f, 0.38f));
            robitSkinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    renderer = characterModel.baseRendererInfos[0].renderer,
                    mesh = Assets.MainAssetBundle.LoadAsset<Mesh>("meshN4CRShield")
                },
                //[1] sex glass
                new SkinDef.MeshReplacement
                {
                    renderer = characterModel.baseRendererInfos[2].renderer,
                    mesh = Assets.MainAssetBundle.LoadAsset<Mesh>("meshN4CRGun")
                },
                new SkinDef.MeshReplacement
                {
                    renderer = characterModel.baseRendererInfos[3].renderer,
                    mesh = Assets.MainAssetBundle.LoadAsset<Mesh>("meshN4CRGun")
                },
                new SkinDef.MeshReplacement
                {
                    renderer = characterModel.baseRendererInfos[4].renderer,
                    mesh = Assets.MainAssetBundle.LoadAsset<Mesh>("meshN4CRGun")
                },
                new SkinDef.MeshReplacement
                {
                    renderer = characterModel.baseRendererInfos[5].renderer,
                    mesh = Assets.MainAssetBundle.LoadAsset<Mesh>("meshEnforcerHammer")
                },
                new SkinDef.MeshReplacement
                {
                    renderer = characterModel.baseRendererInfos[6].renderer,
                    mesh = Assets.MainAssetBundle.LoadAsset<Mesh>("meshN4CRPauldron")
                },
                new SkinDef.MeshReplacement
                {
                    renderer = characterModel.baseRendererInfos[7].renderer,
                    mesh = Assets.MainAssetBundle.LoadAsset<Mesh>("meshN4CR")
                }
            };
            robitSkinDefInfo.Name = "ENFORCERBODY_MASTERY_SKIN_NAME";
            robitSkinDefInfo.NameToken = "ENFORCERBODY_MASTERY_SKIN_NAME";
            robitSkinDefInfo.RootObject = model;
            //robitSkinDefInfo.UnlockableDef = EnforcerUnlockables.enforcerMasteryUnlockableDef;
            robitSkinDefInfo.RendererInfos = characterModel.baseRendererInfos;

            robitSkinDefInfo.RendererInfos = defaultRendererInfos;
            robitSkinDefInfo.RendererInfos[0].defaultMaterial = Assets.CreateMaterial("matEnforcerBot", 0f, Color.black, 1f);
            robitSkinDefInfo.RendererInfos[1].defaultMaterial = Assets.CreateMaterial("matEnforcerBot", 0f, Color.black, 0);
            robitSkinDefInfo.RendererInfos[2].defaultMaterial = Assets.CreateMaterial("matEnforcerBot", 1f, Color.white, 0f);
            robitSkinDefInfo.RendererInfos[3].defaultMaterial = Assets.CreateMaterial("matEnforcerBot", 1f, Color.white, 0f);

            SkinDef robitSkin = CreateSkinDef(robitSkinDefInfo);
            skinDefs.Add(robitSkin);
            #endregion

            #region If she don't play the craft
            SkinDefInfo dontgettheshaftSkinDefInfo = new SkinDefInfo();
            dontgettheshaftSkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            dontgettheshaftSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            dontgettheshaftSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];
            dontgettheshaftSkinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[0];

            dontgettheshaftSkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texSbeveAchievement");
            //skinDefInfo.Icon = LoadoutAPI.CreateSkinIcon(new Color(0.31f, 0.49f, 0.69f), new Color(0.86f, 0.83f, 0.63f), new Color(0.1f, 0.07f, 0.06f), new Color(0.21f, 0.29f, 0.38f));
            dontgettheshaftSkinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                //new SkinDef.MeshReplacement
                //{
                //    renderer = characterModel.baseRendererInfos[0].renderer,
                //    mesh = Assets.MainAssetBundle.LoadAsset<Mesh>("meshShieldBot")
                //},
                //new SkinDef.MeshReplacement
                //{
                //    renderer = characterModel.baseRendererInfos[1].renderer,
                //    mesh = Assets.MainAssetBundle.LoadAsset<Mesh>("meshGunBot")
                //},
                //new SkinDef.MeshReplacement
                //{
                //    renderer = characterModel.baseRendererInfos[2].renderer,
                //    mesh = Assets.MainAssetBundle.LoadAsset<Mesh>("meshPauldronBot")
                //},
                new SkinDef.MeshReplacement
                {
                    renderer = characterModel.baseRendererInfos[3].renderer,
                    mesh = Assets.MainAssetBundle.LoadAsset<Mesh>("meshEnfuckerSbeve")
                }
            };
            dontgettheshaftSkinDefInfo.Name = "ENFORCERBODY_FUCKINGSTEVE_SKIN_NAME";
            dontgettheshaftSkinDefInfo.NameToken = "ENFORCERBODY_FUCKINGSTEVE_SKIN_NAME";
            dontgettheshaftSkinDefInfo.RendererInfos = characterModel.baseRendererInfos;
            dontgettheshaftSkinDefInfo.RootObject = model;
            dontgettheshaftSkinDefInfo.UnlockableDef = EnforcerUnlockables.enforcerMasteryUnlockableDef;

            defaultRendererInfos = new CharacterModel.RendererInfo[defaultSkin.rendererInfos.Length];
            defaultSkin.rendererInfos.CopyTo(defaultRendererInfos, 0);

            dontgettheshaftSkinDefInfo.RendererInfos = defaultRendererInfos;
            dontgettheshaftSkinDefInfo.RendererInfos[0].defaultMaterial = Assets.CreateMaterial("matEnforcerBot", 0f, Color.black, 1f);
            dontgettheshaftSkinDefInfo.RendererInfos[1].defaultMaterial = Assets.CreateMaterial("matEnforcerBot", 0f, Color.black, 0);
            dontgettheshaftSkinDefInfo.RendererInfos[2].defaultMaterial = Assets.CreateMaterial("matEnforcerBot", 1f, Color.white, 0f);
            dontgettheshaftSkinDefInfo.RendererInfos[3].defaultMaterial = Assets.CreateMaterial("matFuckingSteve", 1f, Color.white, 0f);

            SkinDef dontgettheshaftSkin = CreateSkinDef(dontgettheshaftSkinDefInfo);

            //if(EnforcerModPlugin.cursed.Value)
            //    skinDefs.Add(dontgettheshaftSkin);
            #endregion

            // what are we gonna do about all this...........
            // fuckin nothing that's what you did faggot
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
            #endregion

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

            skinController.skins = skinDefs.ToArray();
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
    }
}