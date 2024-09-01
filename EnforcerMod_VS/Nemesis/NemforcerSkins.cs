using System;
using UnityEngine;
using R2API;
using RoR2;
using System.Collections.Generic;
using Modules;

namespace EnforcerPlugin {
    public static class NemforcerSkins
    {

        public static SkinDef dededeBossSkin;
        public static SkinDef ultraSkin;

        public enum NemforcerSkin
        {
            NONE = -1,
            DEFAULT,
            CLASSIC,
            TYPHOONSKIN,
            ENFORCER,
            DRIP,
            DEDEDE,
            DEDEDEBOSS,
            MINECRAFT
        }

        public static Dictionary<int, string> SkinIdices = new Dictionary<int, string>();

        public static List<SkinDef> skinDefs = new List<SkinDef>();

        public static bool isNemforcerCurrentSkin(CharacterBody characterbody, string skin)
        {
            return characterbody.baseNameToken == "NEMFORCER_NAME" && SkinIdices[(int)characterbody.skinIndex] == skin;
        }

        public static bool isNemforcerCurrentSkin(CharacterBody characterbody, NemforcerSkin skin)
        {
            return isNemforcerCurrentSkin(characterbody, GetFuckingSkinID(skin));
        }

        public static string GetFuckingSkinID(NemforcerSkin skin)
        {
            switch (skin)
            {
                default:
                case NemforcerSkin.DEFAULT:
                    return "NEMFORCERBODY_DEFAULT_SKIN_NAME";
                case NemforcerSkin.CLASSIC:
                    return "NEMFORCERBODY_CLASSIC_SKIN_NAME";
                case NemforcerSkin.TYPHOONSKIN:
                    return "NEMFORCERBODY_TYPHOON_SKIN_NAME";
                case NemforcerSkin.ENFORCER:
                    return "NEMFORCERBODY_ENFORCER_SKIN_NAME";
                case NemforcerSkin.DRIP:
                    return "NEMFORCERBODY_DRIP_SKIN_NAME";
                case NemforcerSkin.DEDEDE:
                    return "NEMFORCERBODY_DEDEDE_SKIN_NAME";
                case NemforcerSkin.DEDEDEBOSS:
                    return "NEMFORCERBODY_DEDEDE_SKIN_NAME";
                case NemforcerSkin.MINECRAFT:
                    return "NEMFORCERBODY_MINECRAFT_SKIN_NAME";
            }
        }

        public static void RegisterSkins() {
            GameObject bodyPrefab = NemforcerPlugin.characterBodyPrefab;

            GameObject model = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = model.GetComponent<CharacterModel>();

            ModelSkinController skinController = model.AddComponent<ModelSkinController>();
            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            SkinnedMeshRenderer mainRenderer = characterModel.mainSkinnedMeshRenderer;

            #region DefaultSkin
            Skins.SkinDefInfo defaultSkinDefInfo = default(Skins.SkinDefInfo);
            defaultSkinDefInfo.Name = "NEMFORCERBODY_DEFAULT_SKIN_NAME";
            defaultSkinDefInfo.NameToken = "NEMFORCERBODY_DEFAULT_SKIN_NAME";
            defaultSkinDefInfo.Icon = Asset.MainAssetBundle.LoadAsset<Sprite>("texNemforcerAchievement");
            defaultSkinDefInfo.UnlockableDef = null;

            defaultSkinDefInfo.RootObject = model;
            defaultSkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            defaultSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            defaultSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            defaultSkinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[0];
            defaultSkinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
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
            defaultSkinDefInfo.RendererInfos = new CharacterModel.RendererInfo[characterModel.baseRendererInfos.Length];
            characterModel.baseRendererInfos.CopyTo(defaultSkinDefInfo.RendererInfos, 0);

            SkinDef defaultSkinDef = Skins.CreateSkinDef(defaultSkinDefInfo);
            #endregion

            #region ClassicSkin
            Skins.SkinDefInfo classicSkinDefInfo = default(Skins.SkinDefInfo);
            classicSkinDefInfo.Name = "NEMFORCERBODY_CLASSIC_SKIN_NAME";
            classicSkinDefInfo.NameToken = "NEMFORCERBODY_CLASSIC_SKIN_NAME";
            classicSkinDefInfo.Icon = Asset.MainAssetBundle.LoadAsset<Sprite>("texNemforcerMastery");
            classicSkinDefInfo.UnlockableDef = EnforcerUnlockables.nemMasteryUnlockableDef;
            classicSkinDefInfo.RootObject = model;

            classicSkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            classicSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            classicSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            classicSkinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[0];

            classicSkinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    renderer = mainRenderer,
                    mesh = Asset.nemClassicMesh
                },
                new SkinDef.MeshReplacement
                {
                    renderer = characterModel.baseRendererInfos[0].renderer,
                    mesh = Asset.nemClassicHammerMesh
                }
            };
            classicSkinDefInfo.RendererInfos = new CharacterModel.RendererInfo[defaultSkinDefInfo.RendererInfos.Length];
            defaultSkinDefInfo.RendererInfos.CopyTo(classicSkinDefInfo.RendererInfos, 0);

            classicSkinDefInfo.RendererInfos[0].defaultMaterial = Asset.CreateMaterial("matNemforcerClassic", 5f, Color.white, 0);
            classicSkinDefInfo.RendererInfos[classicSkinDefInfo.RendererInfos.Length - 1].defaultMaterial = Asset.CreateMaterial("matNemforcerClassic", 5f, Color.white, 0);

            SkinDef classicSkin = Skins.CreateSkinDef(classicSkinDefInfo);
            #endregion

            #region TyphoonSkin
            Skins.SkinDefInfo typhoonSkinDefInfo = default(Skins.SkinDefInfo);
            typhoonSkinDefInfo.Name = "NEMFORCERBODY_TYPHOON_SKIN_NAME";
            typhoonSkinDefInfo.NameToken = "NEMFORCERBODY_TYPHOON_SKIN_NAME";
            typhoonSkinDefInfo.Icon = Asset.MainAssetBundle.LoadAsset<Sprite>("texNemforcerGrandMastery");
            typhoonSkinDefInfo.UnlockableDef = EnforcerUnlockables.nemGrandMasteryUnlockableDef;
            typhoonSkinDefInfo.RootObject = model;

            typhoonSkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            typhoonSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            typhoonSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            typhoonSkinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[0];

            typhoonSkinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    renderer = mainRenderer,
                    mesh = Asset.nemMeshGM
                },
                new SkinDef.MeshReplacement
                {
                    renderer = characterModel.baseRendererInfos[0].renderer,
                    mesh = Asset.nemHammerMeshGM
                }
            };
            typhoonSkinDefInfo.RendererInfos = new CharacterModel.RendererInfo[characterModel.baseRendererInfos.Length];
            characterModel.baseRendererInfos.CopyTo(typhoonSkinDefInfo.RendererInfos, 0);

            SkinDef typhoonSkin = Skins.CreateSkinDef(typhoonSkinDefInfo);
            ultraSkin = typhoonSkin;
            #endregion

            #region EnforcerSkin
            Skins.SkinDefInfo altSkinDefInfo = default(Skins.SkinDefInfo);
            altSkinDefInfo.Name = "NEMFORCERBODY_ENFORCER_SKIN_NAME";
            altSkinDefInfo.NameToken = "NEMFORCERBODY_ENFORCER_SKIN_NAME";
            altSkinDefInfo.Icon = Asset.MainAssetBundle.LoadAsset<Sprite>("texNemforcerEnforcer");
            altSkinDefInfo.UnlockableDef = null;
            altSkinDefInfo.RootObject = model;

            altSkinDefInfo.BaseSkins = new SkinDef[] { defaultSkinDef };
            altSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            altSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            altSkinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[0];

            altSkinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    renderer = mainRenderer,
                    mesh = Asset.nemAltMesh
                },
            };

            altSkinDefInfo.RendererInfos = new CharacterModel.RendererInfo[characterModel.baseRendererInfos.Length];
            characterModel.baseRendererInfos.CopyTo(altSkinDefInfo.RendererInfos, 0);

            altSkinDefInfo.RendererInfos[0].defaultMaterial = Asset.CreateMaterial("matNemforcerAlt", 5f, Color.white, 0);
            altSkinDefInfo.RendererInfos[altSkinDefInfo.RendererInfos.Length - 1].defaultMaterial = Asset.CreateMaterial("matNemforcerAlt", 5f, Color.white, 0);

            SkinDef altSkin = Skins.CreateSkinDef(altSkinDefInfo);
            #endregion

            #region DripSkin
            Skins.SkinDefInfo dripSkinDefInfo = default(Skins.SkinDefInfo);
            dripSkinDefInfo.Name = "NEMFORCERBODY_DRIP_SKIN_NAME";
            dripSkinDefInfo.NameToken = "NEMFORCERBODY_DRIP_SKIN_NAME";
            dripSkinDefInfo.Icon = Asset.MainAssetBundle.LoadAsset<Sprite>("texNemforcerDrip");
            dripSkinDefInfo.UnlockableDef = null;
            dripSkinDefInfo.RootObject = model;

            dripSkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            dripSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            dripSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            dripSkinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[0];

            dripSkinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    renderer = mainRenderer,
                    mesh = Asset.nemDripMesh
                },
                new SkinDef.MeshReplacement
                {
                    renderer = characterModel.baseRendererInfos[0].renderer,
                    mesh = Asset.nemDripHammerMesh
                }
            };
            dripSkinDefInfo.RendererInfos = new CharacterModel.RendererInfo[characterModel.baseRendererInfos.Length];
            characterModel.baseRendererInfos.CopyTo(dripSkinDefInfo.RendererInfos, 0);

            dripSkinDefInfo.RendererInfos[0].defaultMaterial = Asset.CreateMaterial("matDripforcer", 5f, Color.white, 0);
            dripSkinDefInfo.RendererInfos[dripSkinDefInfo.RendererInfos.Length - 1].defaultMaterial = Asset.CreateMaterial("matDripforcer", 5f, Color.white, 0);

            SkinDef dripSkin = Skins.CreateSkinDef(dripSkinDefInfo);
            #endregion

            #region DededeSkin
            Skins.SkinDefInfo dededeSkinDefInfo = default(Skins.SkinDefInfo);
            dededeSkinDefInfo.Name = "NEMFORCERBODY_DEDEDE_SKIN_NAME";
            dededeSkinDefInfo.NameToken = "NEMFORCERBODY_DEDEDE_SKIN_NAME";
            dededeSkinDefInfo.Icon = Asset.MainAssetBundle.LoadAsset<Sprite>("texDededeSkin");
            dededeSkinDefInfo.UnlockableDef = null;
            dededeSkinDefInfo.RootObject = model;

            dededeSkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            dededeSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];

            dededeSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[] {
                new SkinDef.ProjectileGhostReplacement{
                    projectilePrefab = NemforcerPlugin.hammerProjectile,
                    projectileGhostReplacementPrefab = NemforcerPlugin.gordoProjectileGhost
                }
            };

            dededeSkinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[0];

            dededeSkinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    renderer = mainRenderer,
                    mesh = Asset.dededeMesh
                },
                new SkinDef.MeshReplacement
                {
                    renderer = characterModel.baseRendererInfos[0].renderer,
                    mesh = Asset.dededeHammerMesh
                }
            };

            dededeSkinDefInfo.RendererInfos = characterModel.baseRendererInfos;

            dededeSkinDefInfo.RendererInfos = new CharacterModel.RendererInfo[characterModel.baseRendererInfos.Length];
            characterModel.baseRendererInfos.CopyTo(dededeSkinDefInfo.RendererInfos, 0);

            dededeSkinDefInfo.RendererInfos[0].defaultMaterial = Asset.CreateMaterial("matDedede");
            dededeSkinDefInfo.RendererInfos[dededeSkinDefInfo.RendererInfos.Length - 1].defaultMaterial = Asset.CreateMaterial("matDedede");

            SkinDef dededeSkin = Skins.CreateSkinDef(dededeSkinDefInfo);
            #endregion

            //hold on
            #region DededeBossSkin
            Skins.SkinDefInfo dededeBossSkinDefInfo = default(Skins.SkinDefInfo);
            dededeBossSkinDefInfo.Name = "NEMFORCERBODY_DEDEDE_SKIN_NAME";
            dededeBossSkinDefInfo.NameToken = "NEMFORCERBODY_DEDEDE_SKIN_NAME";
            dededeBossSkinDefInfo.Icon = Asset.MainAssetBundle.LoadAsset<Sprite>("texDededeSkin");
            dededeBossSkinDefInfo.UnlockableDef = null;
            dededeBossSkinDefInfo.RootObject = model;

            dededeBossSkinDefInfo.BaseSkins = new SkinDef[] {dededeSkin };
            dededeBossSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            dededeBossSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];
            dededeBossSkinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[0];

            dededeBossSkinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    renderer = mainRenderer,
                    mesh = Asset.dededeBossMesh
                },
            };

            dededeBossSkinDefInfo.RendererInfos = dededeSkinDefInfo.RendererInfos;

            dededeBossSkin = Skins.CreateSkinDef(dededeBossSkinDefInfo);
            #endregion

            #region MinecraftSkin
            Skins.SkinDefInfo minecraftSkinDefInfo = default(Skins.SkinDefInfo);
            minecraftSkinDefInfo.Name = "NEMFORCERBODY_MINECRAFT_SKIN_NAME";
            minecraftSkinDefInfo.NameToken = "NEMFORCERBODY_MINECRAFT_SKIN_NAME";
            minecraftSkinDefInfo.Icon = Asset.MainAssetBundle.LoadAsset<Sprite>("texNemforcerMinecraftShit");
            minecraftSkinDefInfo.UnlockableDef = null;
            minecraftSkinDefInfo.RootObject = model;

            minecraftSkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            minecraftSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            minecraftSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            minecraftSkinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[0];

            minecraftSkinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    renderer = mainRenderer,
                    mesh = Asset.minecraftNemMesh
                },
                new SkinDef.MeshReplacement
                {
                    renderer = characterModel.baseRendererInfos[0].renderer,
                    mesh = Asset.minecraftHammerMesh
                }
            };
            minecraftSkinDefInfo.RendererInfos = characterModel.baseRendererInfos;

            minecraftSkinDefInfo.RendererInfos = new CharacterModel.RendererInfo[characterModel.baseRendererInfos.Length];
            characterModel.baseRendererInfos.CopyTo(minecraftSkinDefInfo.RendererInfos, 0);

            minecraftSkinDefInfo.RendererInfos[0].defaultMaterial = Asset.CreateMaterial("matMinecraftNem", 5f, Color.white, 0);
            minecraftSkinDefInfo.RendererInfos[minecraftSkinDefInfo.RendererInfos.Length - 1].defaultMaterial = Asset.CreateMaterial("matMinecraftNem", 5f, Color.white, 0);

            SkinDef minecraftSkin = Skins.CreateSkinDef(minecraftSkinDefInfo);
            #endregion

            #region FemSkin
            Skins.SkinDefInfo femSkinDefInfo = default(Skins.SkinDefInfo);
            femSkinDefInfo.Name = "NEMFORCERBODY_FEM_SKIN_NAME";
            femSkinDefInfo.NameToken = "NEMFORCERBODY_FEM_SKIN_NAME";
            femSkinDefInfo.Icon = Asset.MainAssetBundle.LoadAsset<Sprite>("texNemforcerFem");
            femSkinDefInfo.UnlockableDef = null;
            femSkinDefInfo.RootObject = model;

            femSkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            femSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            femSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            femSkinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[0];

            femSkinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    renderer = mainRenderer,
                    mesh = Asset.femNemMesh
                },
                new SkinDef.MeshReplacement
                {
                    renderer = characterModel.baseRendererInfos[0].renderer,
                    mesh = Asset.femHammerMesh
                }
            };
            femSkinDefInfo.RendererInfos = characterModel.baseRendererInfos;

            femSkinDefInfo.RendererInfos = new CharacterModel.RendererInfo[characterModel.baseRendererInfos.Length];
            characterModel.baseRendererInfos.CopyTo(femSkinDefInfo.RendererInfos, 0);

            femSkinDefInfo.RendererInfos[0].defaultMaterial = Materials.CreateHotpooMaterial("matNemforcerFem");
            femSkinDefInfo.RendererInfos[femSkinDefInfo.RendererInfos.Length - 1].defaultMaterial = Materials.CreateHotpooMaterial("matNemforcerFem");

            SkinDef femSkin = Skins.CreateSkinDef(femSkinDefInfo);
            #endregion

            skinDefs = new List<SkinDef>()
            {
                defaultSkinDef,
                classicSkin,
                typhoonSkin,
                altSkin
            };

            if (Config.cursed.Value)
            {
                skinDefs.Add(dripSkin);
                skinDefs.Add(minecraftSkin);
                skinDefs.Add(dededeSkin);
            }

            if (Config.femSkin.Value) {
                skinDefs.Add(femSkin);
            }

            skinController.skins = skinDefs.ToArray();

            for (int i = 0; i < skinDefs.Count; i++)
            {
                SkinIdices[i] = skinDefs[i].name;
            }
        }
    }
}