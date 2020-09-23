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
            LanguageAPI.Add("NEMFORCERBODY_MASTERY_SKIN_NAME", "Something");

            LoadoutAPI.SkinDefInfo skinDefInfo = default(LoadoutAPI.SkinDefInfo);
            skinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            skinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            skinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];
            skinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[0];
            skinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texEnforcerAchievement");
            skinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    renderer = mainRenderer,
                    mesh = mainRenderer.sharedMesh
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

            //clone commando material for that spicy hopoo shader
            Material material = array[0].defaultMaterial;

            if (material)
            {
                material = UnityEngine.Object.Instantiate<Material>(Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial);
                material.SetColor("_Color", Color.white);
                material.SetTexture("_MainTex", Assets.MainAssetBundle.LoadAsset<Material>("matNemforcer").GetTexture("_MainTex"));
                material.SetColor("_EmColor", Color.white);
                material.SetFloat("_EmPower", 1.5f);
                material.SetTexture("_EmTex", Assets.MainAssetBundle.LoadAsset<Material>("matNemforcer").GetTexture("_EmissionMap"));
                material.SetFloat("_NormalStrength", 1);
                material.SetTexture("_NormalTex", Assets.MainAssetBundle.LoadAsset<Material>("matNemforcer").GetTexture("_BumpMap"));

                array[0].defaultMaterial = material;
            }

            material = array[1].defaultMaterial;

            if (material)
            {
                material = UnityEngine.Object.Instantiate<Material>(Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial);
                material.SetColor("_Color", Assets.MainAssetBundle.LoadAsset<Material>("matMinigun").GetColor("_Color"));
                material.SetTexture("_MainTex", Assets.MainAssetBundle.LoadAsset<Material>("matMinigun").GetTexture("_MainTex"));
                material.SetFloat("_EmPower", 0);
                material.SetFloat("_NormalStrength", 0);

                array[1].defaultMaterial = material;
            }

            material = array[2].defaultMaterial;

            if (material)
            {
                material = UnityEngine.Object.Instantiate<Material>(Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial);
                material.SetColor("_Color", Assets.MainAssetBundle.LoadAsset<Material>("matTemp").GetColor("_Color"));
                material.SetTexture("_MainTex", Assets.MainAssetBundle.LoadAsset<Material>("matTemp").GetTexture("_MainTex"));
                material.SetFloat("_EmPower", 0);
                material.SetFloat("_NormalStrength", 0);

                array[2].defaultMaterial = material;
            }

            material = array[3].defaultMaterial;

            if (material)
            {
                material = UnityEngine.Object.Instantiate<Material>(Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial);
                material.SetColor("_Color", Assets.MainAssetBundle.LoadAsset<Material>("matTemp").GetColor("_Color"));
                material.SetTexture("_MainTex", Assets.MainAssetBundle.LoadAsset<Material>("matTemp").GetTexture("_MainTex"));
                material.SetFloat("_EmPower", 0);
                material.SetFloat("_NormalStrength", 0);

                array[3].defaultMaterial = material;
            }

            skinDefInfo.RendererInfos = array;

            SkinDef defaultSkin = LoadoutAPI.CreateNewSkinDef(skinDefInfo);

            LoadoutAPI.SkinDefInfo masterySkinDefInfo = default(LoadoutAPI.SkinDefInfo);
            masterySkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            masterySkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            masterySkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];
            masterySkinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[0];
            masterySkinDefInfo.Icon = Assets.MainAssetBundle.LoadAsset<Sprite>("texStormtrooperAchievement");

            masterySkinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    renderer = mainRenderer,
                    mesh = Assets.stormtrooperMesh
                }
            };
            masterySkinDefInfo.Name = "NEMFORCERBODY_MASTERY_SKIN_NAME";
            masterySkinDefInfo.NameToken = "NEMFORCERBODY_MASTERY_SKIN_NAME";
            masterySkinDefInfo.RendererInfos = characterModel.baseRendererInfos;
            masterySkinDefInfo.RootObject = model;
            masterySkinDefInfo.UnlockableName = "";

            rendererInfos = skinDefInfo.RendererInfos;
            array = new CharacterModel.RendererInfo[rendererInfos.Length];
            rendererInfos.CopyTo(array, 0);

            //change the body texture
            material = array[0].defaultMaterial;

            if (material)
            {
                material = UnityEngine.Object.Instantiate<Material>(material);
                material.SetTexture("_MainTex", Assets.MainAssetBundle.LoadAsset<Material>("matSpaceEnforcer").GetTexture("_MainTex"));
                material.SetColor("_EmColor", Color.black);
                material.SetFloat("_NormalStrength", 0);

                array[0].defaultMaterial = material;
            }

            masterySkinDefInfo.RendererInfos = array;

            SkinDef masterySkin = LoadoutAPI.CreateNewSkinDef(masterySkinDefInfo);

            skinController.skins = new SkinDef[]
            {
                defaultSkin
            };
        }
    }
}
