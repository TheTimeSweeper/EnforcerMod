using System;
using UnityEngine;
using R2API;
using RoR2;
using R2API.Utils;
using System.Collections.Generic;

namespace EnforcerPlugin
{
    public static class NemforcerSkins
    {
        public static SkinDef dededeBossSkin;

        public static void RegisterSkins()
        {
            GameObject bodyPrefab = NemforcerPlugin.characterPrefab;

            GameObject model = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = model.GetComponent<CharacterModel>();

            ModelSkinController skinController = model.AddComponent<ModelSkinController>();
            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            SkinnedMeshRenderer mainRenderer = characterModel.mainSkinnedMeshRenderer;

            LanguageAPI.Add("NEMFORCERBODY_DEFAULT_SKIN_NAME", "Nemesis");
            LanguageAPI.Add("NEMFORCERBODY_ENFORCER_SKIN_NAME", "Enforcer");
            LanguageAPI.Add("NEMFORCERBODY_CLASSIC_SKIN_NAME", "Classic");
            LanguageAPI.Add("NEMFORCERBODY_TYPHOON_SKIN_NAME", "Champion");
            LanguageAPI.Add("NEMFORCERBODY_DRIP_SKIN_NAME", "Dripforcer");
            LanguageAPI.Add("NEMFORCERBODY_DEDEDE_SKIN_NAME", "King Dedede");
            LanguageAPI.Add("NEMFORCERBODY_SNEED_SKIN_NAME", "Sneed");
            LanguageAPI.Add("NEMFORCERBODY_MINECRAFT_SKIN_NAME", "Minecraft");

            #region DefaultSkin
            LoadoutAPI.SkinDefInfo skinDefInfo = default(LoadoutAPI.SkinDefInfo);
            skinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            skinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            skinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];
            skinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[0];
            skinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texNemforcerAchievement");
            skinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    renderer = mainRenderer,
                    mesh = mainRenderer.sharedMesh
                },
                new SkinDef.MeshReplacement
                {
                    renderer = characterModel.baseRendererInfos[0].renderer,
                    mesh = characterModel.baseRendererInfos[0].renderer.GetComponent<SkinnedMeshRenderer>().sharedMesh
                }
            };
            skinDefInfo.Name = "NEMFORCERBODY_DEFAULT_SKIN_NAME";
            skinDefInfo.NameToken = "NEMFORCERBODY_DEFAULT_SKIN_NAME";
            skinDefInfo.RendererInfos = characterModel.baseRendererInfos;
            skinDefInfo.RootObject = model;
            skinDefInfo.UnlockableName = "";

            CharacterModel.RendererInfo[] rendererInfos = skinDefInfo.RendererInfos;
            CharacterModel.RendererInfo[] array = new CharacterModel.RendererInfo[rendererInfos.Length];
            rendererInfos.CopyTo(array, 0);

            Material material = array[0].defaultMaterial;
            array[0].defaultMaterial = Assets.CreateNemMaterial("matNemforcer", 5f, Color.white, 0);
            array[array.Length - 1].defaultMaterial = Assets.CreateNemMaterial("matNemforcer", 5f, Color.white, 0);

            skinDefInfo.RendererInfos = array;

            SkinDef defaultSkin = LoadoutAPI.CreateNewSkinDef(skinDefInfo);
            #endregion

            #region EnforcerSkin
            LoadoutAPI.SkinDefInfo altSkinDefInfo = default(LoadoutAPI.SkinDefInfo);
            altSkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            altSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            altSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];
            altSkinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[0];
            altSkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texNemforcerEnforcer");

            altSkinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    renderer = mainRenderer,
                    mesh = Assets.nemAltMesh
                },
                new SkinDef.MeshReplacement
                {
                    renderer = characterModel.baseRendererInfos[0].renderer,
                    mesh = characterModel.baseRendererInfos[0].renderer.GetComponent<SkinnedMeshRenderer>().sharedMesh
                }
            };
            altSkinDefInfo.Name = "NEMFORCERBODY_ENFORCER_SKIN_NAME";
            altSkinDefInfo.NameToken = "NEMFORCERBODY_ENFORCER_SKIN_NAME";
            altSkinDefInfo.RendererInfos = characterModel.baseRendererInfos;
            altSkinDefInfo.RootObject = model;
            altSkinDefInfo.UnlockableName = "NEMFORCER_DOMINANCEUNLOCKABLE_REWARD_ID";

            rendererInfos = skinDefInfo.RendererInfos;
            array = new CharacterModel.RendererInfo[rendererInfos.Length];
            rendererInfos.CopyTo(array, 0);

            array[0].defaultMaterial = Assets.CreateNemMaterial("matNemforcerAlt", 5f, Color.white, 0);
            array[array.Length - 1].defaultMaterial = Assets.CreateNemMaterial("matNemforcerAlt", 5f, Color.white, 0);

            altSkinDefInfo.RendererInfos = array;

            SkinDef altSkin = LoadoutAPI.CreateNewSkinDef(altSkinDefInfo);
            #endregion

            #region ClassicSkin
            LoadoutAPI.SkinDefInfo classicSkinDefInfo = default(LoadoutAPI.SkinDefInfo);
            classicSkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            classicSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            classicSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];
            classicSkinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[0];
            classicSkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texNemforcerMastery");

            classicSkinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    renderer = mainRenderer,
                    mesh = Assets.nemClassicMesh
                },
                new SkinDef.MeshReplacement
                {
                    renderer = characterModel.baseRendererInfos[0].renderer,
                    mesh = Assets.nemClassicHammerMesh
                }
            };
            classicSkinDefInfo.Name = "NEMFORCERBODY_CLASSIC_SKIN_NAME";
            classicSkinDefInfo.NameToken = "NEMFORCERBODY_CLASSIC_SKIN_NAME";
            classicSkinDefInfo.RendererInfos = characterModel.baseRendererInfos;
            classicSkinDefInfo.RootObject = model;
            classicSkinDefInfo.UnlockableName = "NEMFORCER_MASTERYUNLOCKABLE_REWARD_ID";

            rendererInfos = skinDefInfo.RendererInfos;
            array = new CharacterModel.RendererInfo[rendererInfos.Length];
            rendererInfos.CopyTo(array, 0);

            array[0].defaultMaterial = Assets.CreateNemMaterial("matNemforcerClassic", 5f, Color.white, 0);
            array[array.Length - 1].defaultMaterial = Assets.CreateNemMaterial("matNemforcerClassic", 5f, Color.white, 0);

            classicSkinDefInfo.RendererInfos = array;

            SkinDef classicSkin = LoadoutAPI.CreateNewSkinDef(classicSkinDefInfo);
            #endregion

            #region TyphoonSkin
            LoadoutAPI.SkinDefInfo typhoonSkinDefInfo = default(LoadoutAPI.SkinDefInfo);
            typhoonSkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            typhoonSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            typhoonSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];
            typhoonSkinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[0];
            typhoonSkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texNemforcerGrandMastery");

            typhoonSkinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    renderer = mainRenderer,
                    mesh = Assets.nemMeshGM
                },
                new SkinDef.MeshReplacement
                {
                    renderer = characterModel.baseRendererInfos[0].renderer,
                    mesh = Assets.nemHammerMeshGM
                }
            };
            typhoonSkinDefInfo.Name = "NEMFORCERBODY_TYPHOON_SKIN_NAME";
            typhoonSkinDefInfo.NameToken = "NEMFORCERBODY_TYPHOON_SKIN_NAME";
            typhoonSkinDefInfo.RendererInfos = characterModel.baseRendererInfos;
            typhoonSkinDefInfo.RootObject = model;
            typhoonSkinDefInfo.UnlockableName = "NEMFORCER_TYPHOONUNLOCKABLE_REWARD_ID";

            rendererInfos = skinDefInfo.RendererInfos;
            array = new CharacterModel.RendererInfo[rendererInfos.Length];
            rendererInfos.CopyTo(array, 0);

            array[0].defaultMaterial = Assets.CreateNemMaterial("matNemforcer", 5f, Color.white, 0);
            array[array.Length - 1].defaultMaterial = Assets.CreateNemMaterial("matNemforcer", 5f, Color.white, 0);

            typhoonSkinDefInfo.RendererInfos = array;

            SkinDef typhoonSkin = LoadoutAPI.CreateNewSkinDef(typhoonSkinDefInfo);
            #endregion

            #region DripSkin
            LoadoutAPI.SkinDefInfo dripSkinDefInfo = default(LoadoutAPI.SkinDefInfo);
            dripSkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            dripSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            dripSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];
            dripSkinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[0];
            dripSkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texNemforcerDrip");

            dripSkinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    renderer = mainRenderer,
                    mesh = Assets.nemDripMesh
                },
                new SkinDef.MeshReplacement
                {
                    renderer = characterModel.baseRendererInfos[0].renderer,
                    mesh = Assets.nemDripHammerMesh
                }
            };
            dripSkinDefInfo.Name = "NEMFORCERBODY_DRIP_SKIN_NAME";
            dripSkinDefInfo.NameToken = "NEMFORCERBODY_DRIP_SKIN_NAME";
            dripSkinDefInfo.RendererInfos = characterModel.baseRendererInfos;
            dripSkinDefInfo.RootObject = model;
            dripSkinDefInfo.UnlockableName = "";

            rendererInfos = skinDefInfo.RendererInfos;
            array = new CharacterModel.RendererInfo[rendererInfos.Length];
            rendererInfos.CopyTo(array, 0);

            array[0].defaultMaterial = Assets.CreateNemMaterial("matDripforcer", 5f, Color.white, 0);
            array[array.Length - 1].defaultMaterial = Assets.CreateNemMaterial("matDripforcer", 5f, Color.white, 0);

            dripSkinDefInfo.RendererInfos = array;

            SkinDef dripSkin = LoadoutAPI.CreateNewSkinDef(dripSkinDefInfo);
            #endregion

            #region DededeSkin
            LoadoutAPI.SkinDefInfo dededeSkinDefInfo = default(LoadoutAPI.SkinDefInfo);
            dededeSkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            dededeSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            dededeSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];
            dededeSkinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[0];
            dededeSkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texDededeSkin");

            dededeSkinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    renderer = mainRenderer,
                    mesh = Assets.dededeMesh
                },
                new SkinDef.MeshReplacement
                {
                    renderer = characterModel.baseRendererInfos[0].renderer,
                    mesh = Assets.dededeHammerMesh
                }
            };
            dededeSkinDefInfo.Name = "NEMFORCERBODY_DEDEDE_SKIN_NAME";
            dededeSkinDefInfo.NameToken = "NEMFORCERBODY_DEDEDE_SKIN_NAME";
            dededeSkinDefInfo.RendererInfos = characterModel.baseRendererInfos;
            dededeSkinDefInfo.RootObject = model;
            dededeSkinDefInfo.UnlockableName = "";

            rendererInfos = skinDefInfo.RendererInfos;
            array = new CharacterModel.RendererInfo[rendererInfos.Length];
            rendererInfos.CopyTo(array, 0);

            array[0].defaultMaterial = Assets.CreateNemMaterial("matDedede");
            array[array.Length - 1].defaultMaterial = Assets.CreateNemMaterial("matDedede");

            dededeSkinDefInfo.RendererInfos = array;

            SkinDef dededeSkin = LoadoutAPI.CreateNewSkinDef(dededeSkinDefInfo);
            #endregion

            #region DededeBossSkin
            LoadoutAPI.SkinDefInfo dededeBossSkinDefInfo = default(LoadoutAPI.SkinDefInfo);
            dededeBossSkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            dededeBossSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            dededeBossSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];
            dededeBossSkinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[0];
            dededeBossSkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texDededeSkin");

            dededeBossSkinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    renderer = mainRenderer,
                    mesh = Assets.dededeBossMesh
                },
                new SkinDef.MeshReplacement
                {
                    renderer = characterModel.baseRendererInfos[0].renderer,
                    mesh = Assets.dededeHammerMesh
                }
            };
            dededeBossSkinDefInfo.Name = "NEMFORCERBODY_DEDEDE_SKIN_NAME";
            dededeBossSkinDefInfo.NameToken = "NEMFORCERBODY_DEDEDE_SKIN_NAME";
            dededeBossSkinDefInfo.RendererInfos = characterModel.baseRendererInfos;
            dededeBossSkinDefInfo.RootObject = model;
            dededeBossSkinDefInfo.UnlockableName = "";

            rendererInfos = skinDefInfo.RendererInfos;
            array = new CharacterModel.RendererInfo[rendererInfos.Length];
            rendererInfos.CopyTo(array, 0);

            array[0].defaultMaterial = Assets.CreateNemMaterial("matDedede");
            array[array.Length - 1].defaultMaterial = Assets.CreateNemMaterial("matDedede");

            dededeBossSkinDefInfo.RendererInfos = array;

            dededeBossSkin = LoadoutAPI.CreateNewSkinDef(dededeBossSkinDefInfo);
            #endregion

            #region sneedSkin
            LoadoutAPI.SkinDefInfo sneedSkinDefInfo = default(LoadoutAPI.SkinDefInfo);
            sneedSkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            sneedSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            sneedSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];
            sneedSkinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[0];
            sneedSkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texSneedSkin");

            sneedSkinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    renderer = mainRenderer,
                    mesh = Assets.sneedMesh
                },
                new SkinDef.MeshReplacement
                {
                    renderer = characterModel.baseRendererInfos[0].renderer,
                    mesh = Assets.sneedHammerMesh
                }
            };
            sneedSkinDefInfo.Name = "NEMFORCERBODY_SNEED_SKIN_NAME";
            sneedSkinDefInfo.NameToken = "NEMFORCERBODY_SNEED_SKIN_NAME";
            sneedSkinDefInfo.RendererInfos = characterModel.baseRendererInfos;
            sneedSkinDefInfo.RootObject = model;
            sneedSkinDefInfo.UnlockableName = "";

            rendererInfos = skinDefInfo.RendererInfos;
            array = new CharacterModel.RendererInfo[rendererInfos.Length];
            rendererInfos.CopyTo(array, 0);

            array[0].defaultMaterial = Assets.CreateNemMaterial("matSneed");
            array[array.Length - 1].defaultMaterial = Assets.CreateNemMaterial("matSneed");

            sneedSkinDefInfo.RendererInfos = array;

            SkinDef sneedSkin = LoadoutAPI.CreateNewSkinDef(sneedSkinDefInfo);
            #endregion

            #region MinecraftSkin
            LoadoutAPI.SkinDefInfo minecraftSkinDefInfo = default(LoadoutAPI.SkinDefInfo);
            minecraftSkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            minecraftSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            minecraftSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];
            minecraftSkinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[0];
            minecraftSkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texMinecraftShit");

            minecraftSkinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    renderer = mainRenderer,
                    mesh = Assets.minecraftNemMesh
                },
                new SkinDef.MeshReplacement
                {
                    renderer = characterModel.baseRendererInfos[0].renderer,
                    mesh = Assets.minecraftHammerMesh
                }
            };
            minecraftSkinDefInfo.Name = "NEMFORCERBODY_MINECRAFT_SKIN_NAME";
            minecraftSkinDefInfo.NameToken = "NEMFORCERBODY_MINECRAFT_SKIN_NAME";
            minecraftSkinDefInfo.RendererInfos = characterModel.baseRendererInfos;
            minecraftSkinDefInfo.RootObject = model;
            minecraftSkinDefInfo.UnlockableName = "";

            rendererInfos = skinDefInfo.RendererInfos;
            array = new CharacterModel.RendererInfo[rendererInfos.Length];
            rendererInfos.CopyTo(array, 0);

            array[0].defaultMaterial = Assets.CreateNemMaterial("matMinecraftNem", 5f, Color.white, 0);
            array[array.Length - 1].defaultMaterial = Assets.CreateNemMaterial("matMinecraftNem", 5f, Color.white, 0);

            minecraftSkinDefInfo.RendererInfos = array;

            SkinDef minecraftSkin = LoadoutAPI.CreateNewSkinDef(minecraftSkinDefInfo);
            #endregion

            var skinDefs = new List<SkinDef>();

            skinDefs = new List<SkinDef>()
            {
                defaultSkin,
                classicSkin
            };

            if (EnforcerPlugin.starstormInstalled)
            {
                skinDefs.Add(typhoonSkin);
            }

            skinDefs.Add(altSkin);

            if (EnforcerPlugin.cursed.Value)
            {
                skinDefs.Add(dripSkin);
                skinDefs.Add(minecraftSkin);
                skinDefs.Add(dededeSkin);
                skinDefs.Add(sneedSkin);
            }

            skinController.skins = skinDefs.ToArray();
        }
    }
}
