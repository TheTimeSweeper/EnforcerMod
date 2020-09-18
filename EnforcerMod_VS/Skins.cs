using System;
using UnityEngine;
using R2API;
using RoR2;
using R2API.Utils;

namespace EnforcerPlugin
{
    public static class Skins
    {
        public static void RegisterSkins()
        {
            GameObject bodyPrefab = EnforcerPlugin.characterPrefab;

            GameObject model = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = model.GetComponent<CharacterModel>();

            ModelSkinController skinController = model.AddComponent<ModelSkinController>();
            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            SkinnedMeshRenderer mainRenderer = Reflection.GetFieldValue<SkinnedMeshRenderer>(characterModel, "mainSkinnedMeshRenderer");

            LanguageAPI.Add("ENFORCERBODY_DEFAULT_SKIN_NAME", "Default");
            LanguageAPI.Add("ENFORCERBODY_MASTERY_SKIN_NAME", "Peacekeeper");
            LanguageAPI.Add("ENFORCERBODY_SPACE_SKIN_NAME", "Rainstormtrooper");
            LanguageAPI.Add("ENFORCERBODY_ENGI_SKIN_NAME", "Engineer?");
            LanguageAPI.Add("ENFORCERBODY_DOOM_SKIN_NAME", "Doom Slayer");
            LanguageAPI.Add("ENFORCERBODY_DESPERADO_SKIN_NAME", "Desperado");
            LanguageAPI.Add("ENFORCERBODY_FROG_SKIN_NAME", "Zero Suit");

            LoadoutAPI.SkinDefInfo skinDefInfo = default(LoadoutAPI.SkinDefInfo);
            skinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            skinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            skinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            skinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[]
            {
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("EngiShield").gameObject,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("ShotgunModel").gameObject,
                    shouldActivate = true
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("RifleModel").gameObject,
                    shouldActivate = true
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("Blaster").gameObject,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("BlasterRifle").gameObject,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("SuperShotgunModel").gameObject,
                    shouldActivate = true
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("BlasterSuper").gameObject,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("ShieldModel").gameObject,
                    shouldActivate = true
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("SexShieldModel").gameObject,
                    shouldActivate = false
                }
            };

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

            //clone commando material for that spicy hopoo shader
            Material material = array[0].defaultMaterial;

            if (material)
            {
                material = UnityEngine.Object.Instantiate<Material>(Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial);
                material.SetColor("_Color", Color.white);
                material.SetTexture("_MainTex", Assets.MainAssetBundle.LoadAsset<Material>("matEnforcer").GetTexture("_MainTex"));
                material.SetColor("_EmColor", Color.white);
                material.SetFloat("_EmPower", 1.5f);
                material.SetTexture("_EmTex", Assets.MainAssetBundle.LoadAsset<Material>("matEnforcer").GetTexture("_EmissionMap"));
                material.SetFloat("_NormalStrength", 1);
                material.SetTexture("_NormalTex", Assets.MainAssetBundle.LoadAsset<Material>("matEnforcer").GetTexture("_BumpMap"));

                array[0].defaultMaterial = material;
            }

            //change the shield texture too
            material = array[2].defaultMaterial;

            if (material)
            {
                material = UnityEngine.Object.Instantiate<Material>(Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial);
                material.SetColor("_Color", Color.white);
                material.SetTexture("_MainTex", Assets.MainAssetBundle.LoadAsset<Material>("matEquippedShield").GetTexture("_MainTex"));
                material.SetFloat("_EmPower", 0);
                material.SetFloat("_NormalStrength", 1);
                material.SetTexture("_NormalTex", Assets.MainAssetBundle.LoadAsset<Material>("matEquippedShield").GetTexture("_BumpMap"));

                array[2].defaultMaterial = material;
            }

            //and guns
            material = array[1].defaultMaterial;

            if (material)
            {
                material = UnityEngine.Object.Instantiate<Material>(Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial);
                material.SetColor("_Color", Assets.MainAssetBundle.LoadAsset<Material>("matShotgun").GetColor("_Color"));
                material.SetTexture("_MainTex", Assets.MainAssetBundle.LoadAsset<Material>("matShotgun").GetTexture("_MainTex"));
                material.SetFloat("_EmPower", 0);
                material.SetFloat("_NormalStrength", 0);

                array[1].defaultMaterial = material;
            }

            material = array[3].defaultMaterial;

            if (material)
            {
                material = UnityEngine.Object.Instantiate<Material>(Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial);
                material.SetColor("_Color", Assets.MainAssetBundle.LoadAsset<Material>("matShotgun").GetColor("_Color"));
                material.SetTexture("_MainTex", Assets.MainAssetBundle.LoadAsset<Material>("matShotgun").GetTexture("_MainTex"));
                material.SetFloat("_EmPower", 0);
                material.SetFloat("_NormalStrength", 0);

                array[3].defaultMaterial = material;
            }

            material = array[4].defaultMaterial;

            if (material)
            {
                material = UnityEngine.Object.Instantiate<Material>(Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial);
                material.SetColor("_Color", Assets.MainAssetBundle.LoadAsset<Material>("matShotgun").GetColor("_Color"));
                material.SetTexture("_MainTex", Assets.MainAssetBundle.LoadAsset<Material>("matShotgun").GetTexture("_MainTex"));
                material.SetFloat("_EmPower", 0);
                material.SetFloat("_NormalStrength", 0);

                array[4].defaultMaterial = material;
            }

            material = array[5].defaultMaterial;

            if (material)
            {
                material = UnityEngine.Object.Instantiate<Material>(Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial);
                material.SetColor("_Color", Assets.MainAssetBundle.LoadAsset<Material>("matRifle").GetColor("_Color"));
                material.SetTexture("_MainTex", Assets.MainAssetBundle.LoadAsset<Material>("matRifle").GetTexture("_MainTex"));
                material.SetFloat("_EmPower", 0);
                material.SetFloat("_NormalStrength", 0);

                array[5].defaultMaterial = material;
            }

            material = array[7].defaultMaterial;

            if (material)
            {
                material = UnityEngine.Object.Instantiate<Material>(Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial);
                material.SetColor("_Color", Assets.MainAssetBundle.LoadAsset<Material>("matShotgun").GetColor("_Color"));
                material.SetTexture("_MainTex", Assets.MainAssetBundle.LoadAsset<Material>("matShotgun").GetTexture("_MainTex"));
                material.SetFloat("_EmPower", 0);
                material.SetFloat("_NormalStrength", 0);

                array[7].defaultMaterial = material;
            }

            material = array[8].defaultMaterial;

            if (material)
            {
                material = UnityEngine.Object.Instantiate<Material>(Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial);
                material.SetColor("_Color", Assets.MainAssetBundle.LoadAsset<Material>("matTemp").GetColor("_Color"));
                material.SetTexture("_MainTex", Assets.MainAssetBundle.LoadAsset<Material>("matTemp").GetTexture("_MainTex"));
                material.SetFloat("_EmPower", 0);
                material.SetFloat("_NormalStrength", 0);

                array[8].defaultMaterial = material;
            }

            material = array[9].defaultMaterial;

            if (material)
            {
                material = UnityEngine.Object.Instantiate<Material>(Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial);
                material.SetColor("_Color", Assets.MainAssetBundle.LoadAsset<Material>("matShotgun").GetColor("_Color"));
                material.SetTexture("_MainTex", Assets.MainAssetBundle.LoadAsset<Material>("matShotgun").GetTexture("_MainTex"));
                material.SetFloat("_EmPower", 0);
                material.SetFloat("_NormalStrength", 0);

                array[9].defaultMaterial = material;
            }

            material = array[10].defaultMaterial;

            if (material)
            {
                material = UnityEngine.Object.Instantiate<Material>(Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial);
                material.SetColor("_Color", Assets.MainAssetBundle.LoadAsset<Material>("matTemp").GetColor("_Color"));
                material.SetTexture("_MainTex", Assets.MainAssetBundle.LoadAsset<Material>("matTemp").GetTexture("_MainTex"));
                material.SetFloat("_EmPower", 0);
                material.SetFloat("_NormalStrength", 0);

                array[10].defaultMaterial = material;
            }

            material = array[11].defaultMaterial;

            if (material)
            {
                material = UnityEngine.Object.Instantiate<Material>(Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial);
                material.SetColor("_Color", Assets.MainAssetBundle.LoadAsset<Material>("matShotgun").GetColor("_Color"));
                material.SetTexture("_MainTex", Assets.MainAssetBundle.LoadAsset<Material>("matShotgun").GetTexture("_MainTex"));
                material.SetFloat("_EmPower", 0);
                material.SetFloat("_NormalStrength", 0);

                array[11].defaultMaterial = material;
            }

            material = array[12].defaultMaterial;

            if (material)
            {
                material = UnityEngine.Object.Instantiate<Material>(Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial);
                material.SetColor("_Color", Assets.MainAssetBundle.LoadAsset<Material>("matSuperShotgun").GetColor("_Color"));
                material.SetTexture("_MainTex", Assets.MainAssetBundle.LoadAsset<Material>("matSuperShotgun").GetTexture("_MainTex"));
                material.SetFloat("_EmPower", 0);
                material.SetFloat("_NormalStrength", 0);

                array[12].defaultMaterial = material;
            }

            material = array[13].defaultMaterial;

            if (material)
            {
                material = UnityEngine.Object.Instantiate<Material>(Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial);
                material.SetColor("_Color", Assets.MainAssetBundle.LoadAsset<Material>("matTemp").GetColor("_Color"));
                material.SetTexture("_MainTex", Assets.MainAssetBundle.LoadAsset<Material>("matTemp").GetTexture("_MainTex"));
                material.SetFloat("_EmPower", 0);
                material.SetFloat("_NormalStrength", 0);

                array[13].defaultMaterial = material;
            }

            material = array[14].defaultMaterial;

            if (material)
            {
                material = UnityEngine.Object.Instantiate<Material>(Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial);
                material.SetColor("_Color", Assets.MainAssetBundle.LoadAsset<Material>("matShotgun").GetColor("_Color"));
                material.SetTexture("_MainTex", Assets.MainAssetBundle.LoadAsset<Material>("matShotgun").GetTexture("_MainTex"));
                material.SetFloat("_EmPower", 0);
                material.SetFloat("_NormalStrength", 0);

                array[14].defaultMaterial = material;
            }

            material = array[15].defaultMaterial;

            if (material)
            {
                material = UnityEngine.Object.Instantiate<Material>(Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial);
                material.SetColor("_Color", Assets.MainAssetBundle.LoadAsset<Material>("matShotgun").GetColor("_Color"));
                material.SetTexture("_MainTex", Assets.MainAssetBundle.LoadAsset<Material>("matShotgun").GetTexture("_MainTex"));
                material.SetFloat("_EmPower", 0);
                material.SetFloat("_NormalStrength", 0);

                array[15].defaultMaterial = material;
            }

            material = array[16].defaultMaterial;

            if (material)
            {
                material = UnityEngine.Object.Instantiate<Material>(Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial);
                material.SetColor("_Color", Assets.MainAssetBundle.LoadAsset<Material>("matTemp").GetColor("_Color"));
                material.SetTexture("_MainTex", Assets.MainAssetBundle.LoadAsset<Material>("matTemp").GetTexture("_MainTex"));
                material.SetFloat("_EmPower", 0);
                material.SetFloat("_NormalStrength", 0);

                array[16].defaultMaterial = material;
            }

            material = array[17].defaultMaterial;

            if (material)
            {
                material = UnityEngine.Object.Instantiate<Material>(Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial);
                material.SetColor("_Color", Assets.MainAssetBundle.LoadAsset<Material>("matTemp").GetColor("_Color"));
                material.SetTexture("_MainTex", Assets.MainAssetBundle.LoadAsset<Material>("matTemp").GetTexture("_MainTex"));
                material.SetFloat("_EmPower", 0);
                material.SetFloat("_NormalStrength", 0);

                array[17].defaultMaterial = material;
            }

            material = array[18].defaultMaterial;

            if (material)
            {
                material = UnityEngine.Object.Instantiate<Material>(Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial);
                material.SetColor("_Color", Assets.MainAssetBundle.LoadAsset<Material>("matSexShield").GetColor("_Color"));
                material.SetTexture("_MainTex", Assets.MainAssetBundle.LoadAsset<Material>("matSexShield").GetTexture("_MainTex"));
                material.SetFloat("_EmPower", 0);
                material.SetFloat("_NormalStrength", 0);

                array[18].defaultMaterial = material;
            }

            skinDefInfo.RendererInfos = array;

            SkinDef defaultSkin = LoadoutAPI.CreateNewSkinDef(skinDefInfo);

            LoadoutAPI.SkinDefInfo spaceSkinDefInfo = default(LoadoutAPI.SkinDefInfo);
            spaceSkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            spaceSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            spaceSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            spaceSkinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[]
            {
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("EngiShield").gameObject,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("ShotgunModel").gameObject,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("RifleModel").gameObject,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("Blaster").gameObject,
                    shouldActivate = true
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("BlasterRifle").gameObject,
                    shouldActivate = true
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("SuperShotgunModel").gameObject,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("BlasterSuper").gameObject,
                    shouldActivate = true
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("ShieldModel").gameObject,
                    shouldActivate = true
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("SexShieldModel").gameObject,
                    shouldActivate = false
                }
            };

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

            //change the shield texture
            material = array[2].defaultMaterial;

            if (material)
            {
                material = UnityEngine.Object.Instantiate<Material>(material);
                material.SetTexture("_MainTex", Assets.MainAssetBundle.LoadAsset<Material>("matEquippedShieldWhite").GetTexture("_MainTex"));
                material.SetColor("_Color", Assets.MainAssetBundle.LoadAsset<Material>("matEquippedShieldWhite").GetColor("_Color"));
                material.SetFloat("_NormalStrength", 0);

                array[2].defaultMaterial = material;
            }

            spaceSkinDefInfo.RendererInfos = array;

            SkinDef spaceSkin = LoadoutAPI.CreateNewSkinDef(spaceSkinDefInfo);

            LoadoutAPI.SkinDefInfo engiSkinDefInfo = default(LoadoutAPI.SkinDefInfo);
            engiSkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            engiSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            engiSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            engiSkinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[]
            {
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("EngiShield").gameObject,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("ShotgunModel").gameObject,
                    shouldActivate = true
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("RifleModel").gameObject,
                    shouldActivate = true
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("Blaster").gameObject,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("BlasterRifle").gameObject,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("SuperShotgunModel").gameObject,
                    shouldActivate = true
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("BlasterSuper").gameObject,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("ShieldModel").gameObject,
                    shouldActivate = true
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("SexShieldModel").gameObject,
                    shouldActivate = false
                }
            };

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

            material = array[0].defaultMaterial;

            if (material)
            {
                material = UnityEngine.Object.Instantiate<Material>(material);
                material.SetTexture("_MainTex", Assets.MainAssetBundle.LoadAsset<Material>("matEngiforcer").GetTexture("_MainTex"));
                material.SetTexture("_EmTex", Assets.MainAssetBundle.LoadAsset<Material>("matEngiforcer").GetTexture("_EmissionMap"));
                material.SetFloat("_EmPower", 1);

                array[0].defaultMaterial = material;
            }

            material = array[2].defaultMaterial;

            if (material)
            {
                material = UnityEngine.Object.Instantiate<Material>(material);
                material.SetTexture("_MainTex", Assets.MainAssetBundle.LoadAsset<Material>("matEquippedShieldEngi").GetTexture("_MainTex"));
                material.SetColor("_Color", Assets.MainAssetBundle.LoadAsset<Material>("matEquippedShieldEngi").GetColor("_Color"));
                material.SetFloat("_NormalStrength", 0);

                array[2].defaultMaterial = material;
            }

            engiSkinDefInfo.RendererInfos = array;

            SkinDef engiSkin = LoadoutAPI.CreateNewSkinDef(engiSkinDefInfo);

            LoadoutAPI.SkinDefInfo doomSkinDefInfo = default(LoadoutAPI.SkinDefInfo);
            doomSkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            doomSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            doomSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            doomSkinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[]
            {
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("EngiShield").gameObject,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("ShotgunModel").gameObject,
                    shouldActivate = true
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("RifleModel").gameObject,
                    shouldActivate = true
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("Blaster").gameObject,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("BlasterRifle").gameObject,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("SuperShotgunModel").gameObject,
                    shouldActivate = true
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("BlasterSuper").gameObject,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("ShieldModel").gameObject,
                    shouldActivate = true
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("SexShieldModel").gameObject,
                    shouldActivate = false
                }
            };

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

            material = array[0].defaultMaterial;

            if (material)
            {
                material = UnityEngine.Object.Instantiate<Material>(material);
                material.SetTexture("_MainTex", Assets.MainAssetBundle.LoadAsset<Material>("matDoomEnforcer").GetTexture("_MainTex"));
                material.SetTexture("_EmTex", Assets.MainAssetBundle.LoadAsset<Material>("matDoomEnforcer").GetTexture("_EmissionMap"));
                material.SetFloat("_EmPower", 1);

                array[0].defaultMaterial = material;
            }

            material = array[2].defaultMaterial;

            if (material)
            {
                material = UnityEngine.Object.Instantiate<Material>(material);
                material.SetTexture("_MainTex", Assets.MainAssetBundle.LoadAsset<Material>("matEquippedShieldBlack").GetTexture("_MainTex"));
                material.SetColor("_Color", Assets.MainAssetBundle.LoadAsset<Material>("matEquippedShieldBlack").GetColor("_Color"));
                material.SetFloat("_NormalStrength", 0);

                array[2].defaultMaterial = material;
            }

            doomSkinDefInfo.RendererInfos = array;

            SkinDef doomSkin = LoadoutAPI.CreateNewSkinDef(doomSkinDefInfo);

            LoadoutAPI.SkinDefInfo masterySkinDefInfo = default(LoadoutAPI.SkinDefInfo);
            masterySkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            masterySkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            masterySkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            masterySkinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[]
            {
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("EngiShield").gameObject,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("ShotgunModel").gameObject,
                    shouldActivate = true
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("RifleModel").gameObject,
                    shouldActivate = true
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("Blaster").gameObject,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("BlasterRifle").gameObject,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("SuperShotgunModel").gameObject,
                    shouldActivate = true
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("BlasterSuper").gameObject,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("ShieldModel").gameObject,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("SexShieldModel").gameObject,
                    shouldActivate = true
                }
            };

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

            material = array[0].defaultMaterial;

            if (material)
            {
                material = UnityEngine.Object.Instantiate<Material>(material);
                material.SetTexture("_MainTex", Assets.MainAssetBundle.LoadAsset<Material>("matSexforcer").GetTexture("_MainTex"));
                //material.SetTexture("_EmTex", Assets.MainAssetBundle.LoadAsset<Material>("matSexforcer").GetTexture("_EmissionMap"));
                material.SetFloat("_EmPower", 0);

                array[0].defaultMaterial = material;
            }

            masterySkinDefInfo.RendererInfos = array;

            SkinDef masterySkin = LoadoutAPI.CreateNewSkinDef(masterySkinDefInfo);

            LoadoutAPI.SkinDefInfo desperadoSkinDefInfo = default(LoadoutAPI.SkinDefInfo);
            desperadoSkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            desperadoSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            desperadoSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            desperadoSkinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[]
            {
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("EngiShield").gameObject,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("ShotgunModel").gameObject,
                    shouldActivate = true
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("RifleModel").gameObject,
                    shouldActivate = true
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("Blaster").gameObject,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("BlasterRifle").gameObject,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("SuperShotgunModel").gameObject,
                    shouldActivate = true
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("BlasterSuper").gameObject,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("ShieldModel").gameObject,
                    shouldActivate = true
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("SexShieldModel").gameObject,
                    shouldActivate = false
                }
            };

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

            material = array[0].defaultMaterial;

            if (material)
            {
                material = UnityEngine.Object.Instantiate<Material>(material);
                material.SetTexture("_MainTex", Assets.MainAssetBundle.LoadAsset<Material>("matEnforcerDesperado").GetTexture("_MainTex"));
                material.SetTexture("_EmTex", Assets.MainAssetBundle.LoadAsset<Material>("matEnforcerDesperado").GetTexture("_EmissionMap"));
                material.SetFloat("_EmPower", 5f);

                array[0].defaultMaterial = material;
            }

            material = array[2].defaultMaterial;

            if (material)
            {
                material = UnityEngine.Object.Instantiate<Material>(material);
                material.SetTexture("_MainTex", Assets.MainAssetBundle.LoadAsset<Material>("matRiotShieldDesperado").GetTexture("_MainTex"));
                material.SetColor("_Color", Assets.MainAssetBundle.LoadAsset<Material>("matRiotShieldDesperado").GetColor("_Color"));
                material.SetFloat("_NormalStrength", 0);

                array[2].defaultMaterial = material;
            }

            desperadoSkinDefInfo.RendererInfos = array;

            SkinDef desperadoSkin = LoadoutAPI.CreateNewSkinDef(desperadoSkinDefInfo);

            LoadoutAPI.SkinDefInfo frogSkinDefInfo = default(LoadoutAPI.SkinDefInfo);
            frogSkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            frogSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            frogSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            frogSkinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[]
            {
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("EngiShield").gameObject,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("ShotgunModel").gameObject,
                    shouldActivate = true
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("RifleModel").gameObject,
                    shouldActivate = true
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("Blaster").gameObject,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("BlasterRifle").gameObject,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("SuperShotgunModel").gameObject,
                    shouldActivate = true
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("BlasterSuper").gameObject,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("ShieldModel").gameObject,
                    shouldActivate = true
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("SexShieldModel").gameObject,
                    shouldActivate = false
                }
            };

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

            material = array[0].defaultMaterial;

            if (material)
            {
                material = UnityEngine.Object.Instantiate<Material>(material);
                material.SetTexture("_MainTex", Assets.MainAssetBundle.LoadAsset<Material>("matZeroSuit").GetTexture("_MainTex"));
                material.SetTexture("_EmTex", Assets.MainAssetBundle.LoadAsset<Material>("matEnforcer").GetTexture("_EmissionMap"));
                material.SetFloat("_EmPower", 1);

                array[0].defaultMaterial = material;
            }

            frogSkinDefInfo.RendererInfos = array;

            SkinDef frogSkin = LoadoutAPI.CreateNewSkinDef(frogSkinDefInfo);

            LoadoutAPI.SkinDefInfo classicSkinDefInfo = default(LoadoutAPI.SkinDefInfo);
            classicSkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            classicSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            classicSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            classicSkinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[]
            {
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("EngiShield").gameObject,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("ShotgunModel").gameObject,
                    shouldActivate = true
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("RifleModel").gameObject,
                    shouldActivate = true
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("Blaster").gameObject,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("BlasterRifle").gameObject,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("SuperShotgunModel").gameObject,
                    shouldActivate = true
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("BlasterSuper").gameObject,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("ShieldModel").gameObject,
                    shouldActivate = true
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChild("SexShieldModel").gameObject,
                    shouldActivate = false
                }
            };

            classicSkinDefInfo.Icon = LoadoutAPI.CreateSkinIcon(new Color(0.83f, 0.83f, 0.83f), new Color(0.64f, 0.64f, 0.64f), new Color(0.25f, 0.25f, 0.25f), new Color(0f, 0f, 0f));
            classicSkinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    renderer = mainRenderer,
                    mesh = Assets.classicMesh
                }
            };
            classicSkinDefInfo.Name = "ENFORCERBODY_CLASSIC_SKIN_NAME";
            classicSkinDefInfo.NameToken = "ENFORCERBODY_CLASSIC_SKIN_NAME";
            classicSkinDefInfo.RendererInfos = characterModel.baseRendererInfos;
            classicSkinDefInfo.RootObject = model;
            classicSkinDefInfo.UnlockableName = "";

            rendererInfos = skinDefInfo.RendererInfos;
            array = new CharacterModel.RendererInfo[rendererInfos.Length];
            rendererInfos.CopyTo(array, 0);

            classicSkinDefInfo.RendererInfos = array;

            SkinDef classicSkin = LoadoutAPI.CreateNewSkinDef(classicSkinDefInfo);



            bool flag = false;

            if (EnforcerPlugin.antiFun.Value)
            {
                if (flag)
                {
                    skinController.skins = new SkinDef[]
                    {
                        defaultSkin,
                        masterySkin,
                        doomSkin,
                        classicSkin
                    };
                }
                else
                {
                    skinController.skins = new SkinDef[]
                    {
                        defaultSkin,
                        masterySkin,
                        doomSkin
                    };
                }
            }
            else
            {
                if (flag)
                {
                    skinController.skins = new SkinDef[]
                    {
                        defaultSkin,
                        masterySkin,
                        doomSkin,
                        engiSkin,
                        spaceSkin,
                        desperadoSkin,
                        frogSkin,
                        classicSkin
                    };
                }
                else
                {
                    skinController.skins = new SkinDef[]
                    {
                        defaultSkin,
                        masterySkin,
                        doomSkin,
                        engiSkin,
                        spaceSkin,
                        desperadoSkin,
                        frogSkin
                    };
                }
            }
        }
    }
}
