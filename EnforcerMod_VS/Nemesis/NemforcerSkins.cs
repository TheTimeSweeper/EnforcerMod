using System;
using UnityEngine;
using R2API;
using RoR2;
using R2API.Utils;

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
            LanguageAPI.Add("NEMFORCERBODY_MASTERY_SKIN_NAME", "Enforcer");
            LanguageAPI.Add("NEMFORCERBODY_CLASSIC_SKIN_NAME", "Classic");

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

            LoadoutAPI.SkinDefInfo masterySkinDefInfo = default(LoadoutAPI.SkinDefInfo);
            masterySkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            masterySkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            masterySkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];
            masterySkinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[0];
            masterySkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texEnforcerAchievement");

            masterySkinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
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
            masterySkinDefInfo.Name = "NEMFORCERBODY_MASTERY_SKIN_NAME";
            masterySkinDefInfo.NameToken = "NEMFORCERBODY_MASTERY_SKIN_NAME";
            masterySkinDefInfo.RendererInfos = characterModel.baseRendererInfos;
            masterySkinDefInfo.RootObject = model;
            masterySkinDefInfo.UnlockableName = "NEMFORCER_DOMINANCEUNLOCKABLE_REWARD_ID";

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

            masterySkinDefInfo.RendererInfos = array;

            SkinDef masterySkin = LoadoutAPI.CreateNewSkinDef(masterySkinDefInfo);

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

            skinController.skins = new SkinDef[]
            {
                defaultSkin,
                classicSkin,
                masterySkin
            };
        }
    }
}
