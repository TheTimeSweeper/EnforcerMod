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
        public static void RegisterSkins()
        {
            GameObject bodyPrefab = NemforcerPlugin.characterPrefab;

            GameObject model = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = model.GetComponent<CharacterModel>();

            ModelSkinController skinController = model.AddComponent<ModelSkinController>();
            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            SkinnedMeshRenderer mainRenderer = Reflection.GetFieldValue<SkinnedMeshRenderer>(characterModel, "mainSkinnedMeshRenderer");

            LanguageAPI.Add("NEMFORCERBODY_DEFAULT_SKIN_NAME", "Nemesis");
            LanguageAPI.Add("NEMFORCERBODY_ENFORCER_SKIN_NAME", "Enforcer");
            LanguageAPI.Add("NEMFORCERBODY_CLASSIC_SKIN_NAME", "Classic");
            LanguageAPI.Add("NEMFORCERBODY_DRIP_SKIN_NAME", "Dripforcer");

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
                    renderer = characterModel.baseRendererInfos[1].renderer,
                    mesh = characterModel.baseRendererInfos[1].renderer.GetComponent<SkinnedMeshRenderer>().sharedMesh
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
            Material commandoMat = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial;

            material = UnityEngine.Object.Instantiate<Material>(commandoMat);
            material.SetColor("_Color", Color.white);
            material.SetTexture("_MainTex", Assets.NemAssetBundle.LoadAsset<Material>("matNemforcer").GetTexture("_MainTex"));
            material.SetColor("_EmColor", Color.white);
            material.SetFloat("_EmPower", 5f);
            material.SetTexture("_EmTex", Assets.NemAssetBundle.LoadAsset<Material>("matNemforcer").GetTexture("_EmissionMap"));
            material.SetFloat("_NormalStrength", 0);

            array[0].defaultMaterial = material;

            material = UnityEngine.Object.Instantiate<Material>(commandoMat);
            material.SetColor("_Color", Color.white);
            material.SetTexture("_MainTex", Assets.NemAssetBundle.LoadAsset<Material>("matNemforcer").GetTexture("_MainTex"));
            material.SetColor("_EmColor", Color.white);
            material.SetFloat("_EmPower", 5f);
            material.SetTexture("_EmTex", Assets.NemAssetBundle.LoadAsset<Material>("matNemforcer").GetTexture("_EmissionMap"));
            material.SetFloat("_NormalStrength", 0);

            array[1].defaultMaterial = material;

            skinDefInfo.RendererInfos = array;

            SkinDef defaultSkin = LoadoutAPI.CreateNewSkinDef(skinDefInfo);

            LoadoutAPI.SkinDefInfo altSkinDefInfo = default(LoadoutAPI.SkinDefInfo);
            altSkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            altSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            altSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];
            altSkinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[0];
            altSkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texEnforcerAchievement");

            altSkinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    renderer = mainRenderer,
                    mesh = Assets.nemAltMesh
                },
                new SkinDef.MeshReplacement
                {
                    renderer = characterModel.baseRendererInfos[1].renderer,
                    mesh = characterModel.baseRendererInfos[1].renderer.GetComponent<SkinnedMeshRenderer>().sharedMesh
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

            material = array[0].defaultMaterial;

            if (material)
            {
                material = UnityEngine.Object.Instantiate<Material>(material);
                material.SetTexture("_MainTex", Assets.NemAssetBundle.LoadAsset<Material>("matNemforcerAlt").GetTexture("_MainTex"));
                material.SetTexture("_EmTex", Assets.NemAssetBundle.LoadAsset<Material>("matNemforcerAlt").GetTexture("_EmissionMap"));

                array[0].defaultMaterial = material;
            }

            material = array[1].defaultMaterial;

            if (material)
            {
                material = UnityEngine.Object.Instantiate<Material>(material);
                material.SetTexture("_MainTex", Assets.NemAssetBundle.LoadAsset<Material>("matNemforcerAlt").GetTexture("_MainTex"));
                material.SetTexture("_EmTex", Assets.NemAssetBundle.LoadAsset<Material>("matNemforcerAlt").GetTexture("_EmissionMap"));

                array[1].defaultMaterial = material;
            }

            altSkinDefInfo.RendererInfos = array;

            SkinDef altSkin = LoadoutAPI.CreateNewSkinDef(altSkinDefInfo);

            LoadoutAPI.SkinDefInfo classicSkinDefInfo = default(LoadoutAPI.SkinDefInfo);
            classicSkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            classicSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            classicSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];
            classicSkinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[0];
            classicSkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texNemforcerAchievement");

            classicSkinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    renderer = mainRenderer,
                    mesh = Assets.nemClassicMesh
                },
                new SkinDef.MeshReplacement
                {
                    renderer = characterModel.baseRendererInfos[1].renderer,
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

            material = array[0].defaultMaterial;

            if (material)
            {
                material = UnityEngine.Object.Instantiate<Material>(material);
                material.SetTexture("_MainTex", Assets.NemAssetBundle.LoadAsset<Material>("matNemforcerClassic").GetTexture("_MainTex"));
                material.SetTexture("_EmTex", Assets.NemAssetBundle.LoadAsset<Material>("matNemforcerClassic").GetTexture("_EmissionMap"));

                array[0].defaultMaterial = material;
            }

            material = array[1].defaultMaterial;

            if (material)
            {
                material = UnityEngine.Object.Instantiate<Material>(material);
                material.SetTexture("_MainTex", Assets.NemAssetBundle.LoadAsset<Material>("matNemforcerClassic").GetTexture("_MainTex"));
                material.SetTexture("_EmTex", Assets.NemAssetBundle.LoadAsset<Material>("matNemforcerClassic").GetTexture("_EmissionMap"));

                array[1].defaultMaterial = material;
            }

            classicSkinDefInfo.RendererInfos = array;

            SkinDef classicSkin = LoadoutAPI.CreateNewSkinDef(classicSkinDefInfo);

            LoadoutAPI.SkinDefInfo dripSkinDefInfo = default(LoadoutAPI.SkinDefInfo);
            dripSkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            dripSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            dripSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];
            dripSkinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[0];
            dripSkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texNemforcerAchievement");

            dripSkinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    renderer = mainRenderer,
                    mesh = Assets.nemDripMesh
                },
                new SkinDef.MeshReplacement
                {
                    renderer = characterModel.baseRendererInfos[1].renderer,
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

            material = array[0].defaultMaterial;

            if (material)
            {
                material = UnityEngine.Object.Instantiate<Material>(material);
                material.SetTexture("_MainTex", Assets.NemAssetBundle.LoadAsset<Material>("matDripforcer").GetTexture("_MainTex"));
                material.SetTexture("_EmTex", Assets.NemAssetBundle.LoadAsset<Material>("matDripforcer").GetTexture("_EmissionMap"));

                array[0].defaultMaterial = material;
            }

            material = array[1].defaultMaterial;

            if (material)
            {
                material = UnityEngine.Object.Instantiate<Material>(material);
                material.SetTexture("_MainTex", Assets.NemAssetBundle.LoadAsset<Material>("matDripforcer").GetTexture("_MainTex"));
                material.SetTexture("_EmTex", Assets.NemAssetBundle.LoadAsset<Material>("matDripforcer").GetTexture("_EmissionMap"));

                array[1].defaultMaterial = material;
            }

            dripSkinDefInfo.RendererInfos = array;

            SkinDef dripSkin = LoadoutAPI.CreateNewSkinDef(dripSkinDefInfo);

            var skinDefs = new List<SkinDef>();

            skinDefs = new List<SkinDef>()
            {
                defaultSkin,
                classicSkin,
                altSkin
            };

            if (EnforcerPlugin.cursed.Value) skinDefs.Add(dripSkin);

            skinController.skins = skinDefs.ToArray();
        }
    }
}
