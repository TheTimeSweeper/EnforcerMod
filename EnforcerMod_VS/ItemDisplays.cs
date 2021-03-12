using System;
using UnityEngine;
using R2API;
using RoR2;
using R2API.Utils;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace EnforcerPlugin
{
    public static class ItemDisplays
    {
        public static List<ItemDisplayRuleSet.NamedRuleGroup> list;
        public static List<ItemDisplayRuleSet.NamedRuleGroup> list2;

        public static GameObject capacitorPrefab;
        public static GameObject gatDronePrefab;

        private static Dictionary<string, GameObject> itemDisplayPrefabs = new Dictionary<string, GameObject>();

        public static void RegisterDisplays()
        {
            GameObject bodyPrefab = EnforcerPlugin.characterPrefab;

            GameObject model = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = model.GetComponent<CharacterModel>();

            PopulateDisplays();

            ItemDisplayRuleSet itemDisplayRuleSet = ScriptableObject.CreateInstance<ItemDisplayRuleSet>();

            list = new List<ItemDisplayRuleSet.NamedRuleGroup>();
            list2 = new List<ItemDisplayRuleSet.NamedRuleGroup>();

            //add item displays here
            #region DisplayRules
            list2.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Jetpack",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBugWings"),
                            childName = "Chest",
                            localPos = new Vector3(0, 0.0214f, -0.0208f),
                            localAngles = new Vector3(45, 0, 0),
                            localScale = new Vector3(0.015f, 0.015f, 0.015f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list2.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "GoldGat",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = gatDronePrefab,
                            childName = "Pelvis",
                            localPos = new Vector3(0.08f, 0.15f, 0.05f),
                            localAngles = new Vector3(0, 90, 0),
                            localScale = new Vector3(0.015f, 0.015f, 0.015f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list2.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "BFG",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBFG"),
                            childName = "Chest",
                            localPos = new Vector3(0.0118f, 0.0301f, -0.0082f),
                            localAngles = new Vector3(0, 0, -30f),
                            localScale = new Vector3(0.03f, 0.03f, 0.03f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "CritGlasses",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGlasses"),
childName = "Head",
localPos = new Vector3(-0.1475F, 0.2316F, 0F),
localAngles = new Vector3(337.4243F, 270F, 0F),
localScale = new Vector3(0.328F, 0.328F, 0.328F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Syringe",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySyringeCluster"),
                            childName = "Chest",
                            localPos = new Vector3(0.01002f, 0.02729f, 0.01233f),
                            localAngles = new Vector3(0, 112, 52),
                            localScale = new Vector3(0.012f, 0.012f, 0.012f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Behemoth",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBehemoth"),
childName = "Gun",
localPos = new Vector3(0.4603F, -0.4775F, 0.2552F),
localAngles = new Vector3(55.9276F, 312.569F, 236.5995F),
localScale = new Vector3(0.0989F, 0.0989F, 0.0989F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Missile",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMissileLauncher"),
childName = "Chest",
localPos = new Vector3(-0.019F, 0.675F, 0.4532F),
localAngles = new Vector3(0F, 267.5956F, 333.4432F),
localScale = new Vector3(0.123F, 0.123F, 0.123F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Dagger",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDagger"),
                            childName = "Stomach",
                            localPos = new Vector3(0.005f, 0.0141f, 0),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0.08f, 0.08f, 0.08f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Hoof",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayHoof"),
                            childName = "CalfL",
                            localPos = new Vector3(-0.0025f, 0.012f, -0.0125f),
                            localAngles = new Vector3(60, 0, 0),
                            localScale = new Vector3(0.0125f, 0.0125f, 0.008f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "ChainLightning",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayUkulele"),
                            childName = "Chest",
                            localPos = new Vector3(-0.0057f, -0.0011f, -0.023f),
                            localAngles = new Vector3(0, 180, 90),
                            localScale = new Vector3(0.05f, 0.05f, 0.05f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "GhostOnKill",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMask"),
                            childName = "Head",
                            localPos = new Vector3(0, 0.0098f, 0.01f),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0.05f, 0.05f, 0.05f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Mushroom",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMushroom"),
                            childName = "PauldronR",
                            localPos = new Vector3(0, 0.01f, 0),
                            localAngles = new Vector3(64, 0, 0),
                            localScale = new Vector3(0.0075f, 0.0075f, 0.0075f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "AttackSpeedOnCrit",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWolfPelt"),
                            childName = "Head",
                            localPos = new Vector3(0, 0.016f, -0.005f),
                            localAngles = new Vector3(-25, 0, 0),
                            localScale = new Vector3(0.035f, 0.035f, 0.035f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "BleedOnHit",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayTriTip"),
                            childName = "Chest",
                            localPos = new Vector3(0.01261f, 0.04054f, 0.02003f),
                            localAngles = new Vector3(124, 0, 0),
                            localScale = new Vector3(0.025f, 0.025f, 0.025f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "WardOnLevel",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWarbanner"),
                            childName = "Chest",
                            localPos = new Vector3(0, -0.01383f, -0.02629f),
                            localAngles = new Vector3(0, 0, 90),
                            localScale = new Vector3(0.035f, 0.035f, 0.035f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "HealOnCrit",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayScythe"),
                            childName = "Chest",
                            localPos = new Vector3(-0.0064f, 0.0177f, -0.023f),
                            localAngles = new Vector3(-145, 92, -94),
                            localScale = new Vector3(0.025f, 0.025f, 0.025f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "HealWhileSafe",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySnail"),
                            childName = "PauldronL",
                            localPos = new Vector3(0, 0.01f, 0),
                            localAngles = new Vector3(0, 45, 90),
                            localScale = new Vector3(0.008f, 0.008f, 0.008f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Clover",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayClover"),
                            childName = "PauldronL",
                            localPos = new Vector3(0, 0.01f, 0.01f),
                            localAngles = new Vector3(45, 0, 0),
                            localScale = new Vector3(0.06f, 0.06f, 0.06f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "BarrierOnOverHeal",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayAegis"),
childName = "Shield",
localPos = new Vector3(0.2927F, 0.2613F, 0.0855F),
localAngles = new Vector3(273.2491F, 150.735F, 332.7914F),
localScale = new Vector3(1.1643F, 0.8932F, 0.8932F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "GoldOnHit",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBoneCrown"),
                            childName = "Head",
                            localPos = new Vector3(0, 0.01f, 0),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0.1f, 0.1f, 0.1f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "WarCryOnMultiKill",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayPauldron"),
                            childName = "UpperArmL",
                            localPos = new Vector3(-0.015f, 0.01f, 0),
                            localAngles = new Vector3(60, 270, 0),
                            localScale = new Vector3(0.075f, 0.075f, 0.075f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "SprintArmor",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBuckler"),
                            childName = "UpperArmR",
                            localPos = new Vector3(0, 0.028f, -0.008f),
                            localAngles = new Vector3(0, 180, 90),
                            localScale = new Vector3(0.015f, 0.015f, 0.015f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "IceRing",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayIceRing"),
                            childName = "LowerArmL",
                            localPos = new Vector3(0.0004f, 0.0267f, 0),
                            localAngles = new Vector3(90, 0, 0),
                            localScale = new Vector3(0.07575839f, 0.101f, 0.101f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "FireRing",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFireRing"),
                            childName = "LowerArmL",
                            localPos = new Vector3(0.00042f, 0.0313f, 0),
                            localAngles = new Vector3(90, 0, 0),
                            localScale = new Vector3(0.072f, 0.101f, 0.101f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "UtilitySkillMagazine",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayAfterburnerUpperArmRing"),
                            childName = "PauldronL",
                            localPos = new Vector3(0, 0.015f, 0),
                            localAngles = new Vector3(0, 22, -90),
                            localScale = new Vector3(0.075f, 0.075f, 0.075f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayAfterburnerUpperArmRing"),
                            childName = "PauldronR",
                            localPos = new Vector3(0, 0.015f, 0),
                            localAngles = new Vector3(0, -22, 90),
                            localScale = new Vector3(0.075f, 0.075f, 0.075f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "JumpBoost",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWaxBird"),
                            childName = "Head",
                            localPos = new Vector3(0, -0.028f, -0.0037f),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0.08f, 0.08f, 0.08f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            if (EnforcerPlugin.sillyHammer.Value)
            {
                list.Add(new ItemDisplayRuleSet.NamedRuleGroup
                {
                    name = "ArmorReductionOnHit",
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWarhammer"),
                            childName = "Stomach",
                            localPos = new Vector3(0, 0.02f, 0),
                            localAngles = new Vector3(70, 0, 0),
                            localScale = new Vector3(0.02f, 0.02f, 0.02f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWarhammer"),
                            childName = "Chest",
                            localPos = new Vector3(0, 0.05f, 0),
                            localAngles = new Vector3(270, 0, 0),
                            localScale = new Vector3(0.025f, 0.025f, 0.025f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWarhammer"),
                            childName = "ThighL",
                            localPos = new Vector3(0, 0, 0),
                            localAngles = new Vector3(90, 0, 0),
                            localScale = new Vector3(0.02f, 0.02f, 0.02f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWarhammer"),
                            childName = "CalfL",
                            localPos = new Vector3(0, 0.035f, -0.005f),
                            localAngles = new Vector3(270, 0, 0),
                            localScale = new Vector3(0.01f, 0.025f, 0.01f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWarhammer"),
                            childName = "ThighR",
                            localPos = new Vector3(0, 0, 0),
                            localAngles = new Vector3(90, 0, 0),
                            localScale = new Vector3(0.02f, 0.02f, 0.02f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWarhammer"),
                            childName = "CalfR",
                            localPos = new Vector3(0, 0.035f, -0.005f),
                            localAngles = new Vector3(270, 0, 0),
                            localScale = new Vector3(0.01f, 0.025f, 0.01f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWarhammer"),
                            childName = "UpperArmL",
                            localPos = new Vector3(0, 0, 0),
                            localAngles = new Vector3(90, 0, 0),
                            localScale = new Vector3(0.02f, 0.02f, 0.014f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWarhammer"),
                            childName = "UpperArmR",
                            localPos = new Vector3(0, 0, 0),
                            localAngles = new Vector3(90, 0, 0),
                            localScale = new Vector3(0.02f, 0.02f, 0.014f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWarhammer"),
                            childName = "LowerArmL",
                            localPos = new Vector3(0, 0.06f, 0),
                            localAngles = new Vector3(270, 0, 0),
                            localScale = new Vector3(0.025f, 0.025f, 0.02f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWarhammer"),
                            childName = "LowerArmR",
                            localPos = new Vector3(0, 0.06f, 0),
                            localAngles = new Vector3(270, 0, 0),
                            localScale = new Vector3(0.025f, 0.025f, 0.02f),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

            }
            else
            {
                list.Add(new ItemDisplayRuleSet.NamedRuleGroup
                {
                    name = "ArmorReductionOnHit",
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                            new ItemDisplayRule
                            {
                                ruleType = ItemDisplayRuleType.ParentedPrefab,
                                followerPrefab = ItemDisplays.LoadDisplay("DisplayWarhammer"),
                                childName = "Chest",
                                localPos = new Vector3(0, 0.03f, -0.023f),
                                localAngles = new Vector3(-90, 0, 0),
                                localScale = new Vector3(0.025f, 0.025f, 0.025f),
                                limbMask = LimbFlags.None
                            }
                        }
                    }
                });
            }

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "NearbyDamageBonus",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDiamond"),
                            childName = "LowerArmR",
                            localPos = new Vector3(-0.0031f, 0.0297f, 0),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0.015f, 0.015f, 0.015f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "ArmorPlate",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayRepulsionArmorPlate"),
                            childName = "Shield",
                            localPos = new Vector3(2.5f, 0.5f, -2),
                            localAngles = new Vector3(0, 0, 180),
                            localScale = new Vector3(8, 4, 8),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list2.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "CommandMissile",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMissileRack"),
                            childName = "Chest",
                            localPos = new Vector3(0, 0.0173f, -0.0261f),
                            localAngles = new Vector3(90, 180, 0),
                            localScale = new Vector3(0.058f, 0.058f, 0.058f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Feather",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFeather"),
                            childName = "LowerArmL",
                            localPos = new Vector3(-0.0047f, 0.0185f, -0.0034f),
                            localAngles = new Vector3(-90, 0, 0),
                            localScale = new Vector3(0.004f, 0.004f, 0.004f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Crowbar",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayCrowbar"),
                            childName = "Chest",
                            localPos = new Vector3(-0.0022f, 0.0168f, -0.023f),
                            localAngles = new Vector3(45, 90, 0),
                            localScale = new Vector3(0.04f, 0.04f, 0.04f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "FallBoots",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGravBoots"),
                            childName = "CalfR",
                            localPos = new Vector3(0, 0.02246f, -0.00456f),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0.024f, 0.024f, 0.024f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGravBoots"),
                            childName = "CalfL",
                            localPos = new Vector3(0, 0.02246f, -0.00456f),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0.024f, 0.024f, 0.024f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "ExecuteLowHealthElite",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGuillotine"),
                            childName = "ThighR",
                            localPos = new Vector3(-0.0152f, 0.0308f, 0),
                            localAngles = new Vector3(90, -90, 0),
                            localScale = new Vector3(0.022f, 0.022f, 0.022f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "EquipmentMagazine",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBattery"),
                            childName = "Chest",
                            localPos = new Vector3(0.0118f, 0.0129f, -0.0246f),
                            localAngles = new Vector3(-90, 0, 0),
                            localScale = new Vector3(0.02f, 0.02f, 0.02f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "NovaOnHeal",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDevilHorns"),
                            childName = "Head",
                            localPos = new Vector3(0.0094f, 0.01f, 0),
                            localAngles = new Vector3(0, 0, 20),
                            localScale = new Vector3(0.05f, 0.05f, 0.05f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDevilHorns"),
                            childName = "Head",
                            localPos = new Vector3(-0.0094f, 0.01f, 0),
                            localAngles = new Vector3(0, 0, -20),
                            localScale = new Vector3(-0.05f, 0.05f, 0.05f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Infusion",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayInfusion"),
                            childName = "Pelvis",
                            localPos = new Vector3(-0.01452f, 0.02237f, 0.01542f),
                            localAngles = new Vector3(0, -20, 0),
                            localScale = new Vector3(0.05f, 0.05f, 0.05f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Medkit",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMedkit"),
                            childName = "ThighR",
                            localPos = new Vector3(-0.0148f, 0.0061f, 0),
                            localAngles = new Vector3(80, 90, 0),
                            localScale = new Vector3(0.075f, 0.075f, 0.075f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Bandolier",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBandolier"),
                            childName = "Stomach",
                            localPos = new Vector3(0, 0.0452f, 0.005f),
                            localAngles = new Vector3(-134.304f, -90, 100.864f),
                            localScale = new Vector3(0.084f, 0.03f, 0.08f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "BounceNearby",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayHook"),
childName = "Shield",
localPos = new Vector3(-0.0668F, 1.0006F, -0.4916F),
localAngles = new Vector3(314.6442F, 208.173F, 194.5599F),
localScale = new Vector3(0.435F, 0.435F, 0.435F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "IgniteOnKill",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGasoline"),
                            childName = "ThighL",
                            localPos = new Vector3(0.0142f, 0.0123f, 0),
                            localAngles = new Vector3(96, 90, 90),
                            localScale = new Vector3(0.05f, 0.05f, 0.05f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "StunChanceOnHit",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayStunGrenade"),
                            childName = "ThighR",
                            localPos = new Vector3(0, 0.04f, -0.02f),
                            localAngles = new Vector3(90, 0, 0),
                            localScale = new Vector3(0.1f, 0.1f, 0.1f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Firework",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFirework"),
                            childName = "Stomach",
                            localPos = new Vector3(-0.02187928f, 0.02602776f, 0.01359699f),
                            localAngles = new Vector3(-108, 102, -99),
                            localScale = new Vector3(0.02f, 0.02f, 0.02f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "LunarDagger",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayLunarDagger"),
                            childName = "Chest",
                            localPos = new Vector3(0, 0, -0.0219f),
                            localAngles = new Vector3(-54, 90, -90),
                            localScale = new Vector3(0.05f, 0.05f, 0.05f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Knurl",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayKnurl"),
                            childName = "Chest",
                            localPos = new Vector3(0.01961f, 0.0076f, 0.02533f),
                            localAngles = new Vector3(0, 116, 0),
                            localScale = new Vector3(0.0075f, 0.0075f, 0.0075f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "BeetThighLand",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBeetThighLand"),
                            childName = "PauldronL",
                            localPos = new Vector3(-0.02f, 0.02f, 0),
                            localAngles = new Vector3(0, 206, 64),
                            localScale = new Vector3(0.01f, 0.01f, 0.01f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "SprintBonus",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySoda"),
                            childName = "Stomach",
                            localPos = new Vector3(-0.015f, 0.01f, 0.015f),
                            localAngles = new Vector3(-90, 0, 0),
                            localScale = new Vector3(0.03f, 0.03f, 0.03f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "SecondarySkillMagazine",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDoubleMag"),
                            childName = "Gun",
                            localPos = new Vector3(-0.0244f, 0, -0.01927f),
                            localAngles = new Vector3(0, 2, 90),
                            localScale = new Vector3(0.0065f, 0.0065f, 0.0065f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "StickyBomb",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayStickyBomb"),
                            childName = "Stomach",
                            localPos = new Vector3(-0.018f, 0.02f, -0.01f),
                            localAngles = new Vector3(0, 45, 45),
                            localScale = new Vector3(0.02f, 0.02f, 0.02f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "TreasureCache",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayKey"),
                            childName = "Chest",
                            localPos = new Vector3(-0.00693f, -0.0298f, -0.02f),
                            localAngles = new Vector3(90, 0, -14),
                            localScale = new Vector3(0.09f, 0.09f, 0.09f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "BossDamageBonus",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayAPRound"),
                            childName = "Stomach",
                            localPos = new Vector3(0.01988f, 0.02043f, -0.00202f),
                            localAngles = new Vector3(-90, 0, -84),
                            localScale = new Vector3(0.04f, 0.04f, 0.04f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "SlowOnHit",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBauble"),
                            childName = "Chest",
                            localPos = new Vector3(0.0272f, -0.0164f, -0.028f),
                            localAngles = new Vector3(0, 0, 64),
                            localScale = new Vector3(0.0344364f, 0.0344364f, 0.0344364f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "ExtraLife",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayHippo"),
                            childName = "Shield",
                            localPos = new Vector3(0, 1.15f, -3.65f),
                            localAngles = new Vector3(-80, 180, 0),
                            localScale = new Vector3(5, 5, 5),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "KillEliteFrenzy",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBrainstalk"),
                            childName = "Head",
                            localPos = new Vector3(0, 0.015f, 0.0033f),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0.02187931f, 0.02187931f, 0.02187931f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "RepeatHeal",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayCorpseFlower"),
                            childName = "PauldronR",
                            localPos = new Vector3(0.00647f, 0.01332f, 0),
                            localAngles = new Vector3(0, -38, -90),
                            localScale = new Vector3(0.02119026f, 0.02119026f, 0.02119026f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "AutoCastEquipment",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFossil"),
                            childName = "Chest",
                            localPos = new Vector3(0.0063f, -0.0223f, -0.022f),
                            localAngles = new Vector3(0, 90, 0),
                            localScale = new Vector3(0.05f, 0.05f, 0.05f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "IncreaseHealing",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayAntler"),
                            childName = "Head",
                            localPos = new Vector3(0.0083f, 0.015f, -0.0054f),
                            localAngles = new Vector3(0, 90, 0),
                            localScale = new Vector3(0.035f, 0.035f, 0.035f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayAntler"),
                            childName = "Head",
                            localPos = new Vector3(-0.0083f, 0.015f, -0.0054f),
                            localAngles = new Vector3(0, -90, 0),
                            localScale = new Vector3(0.035f, 0.035f, 0.035f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "TitanGoldDuringTP",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGoldHeart"),
                            childName = "Chest",
                            localPos = new Vector3(0.00011f, 0.00371f, 0.02213f),
                            localAngles = new Vector3(0, 0, -75),
                            localScale = new Vector3(0.03f, 0.03f, 0.03f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "SprintWisp",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBrokenMask"),
                            childName = "PauldronL",
                            localPos = new Vector3(0, 0.02f, 0.01f),
                            localAngles = new Vector3(328, 0, 0),
                            localScale = new Vector3(0.02f, 0.02f, 0.02f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "BarrierOnKill",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBrooch"),
                            childName = "Chest",
                            localPos = new Vector3(-0.01043f, 0.01134f, 0.025f),
                            localAngles = new Vector3(45, 90, 90),
                            localScale = new Vector3(0.04f, 0.04f, 0.04f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "TPHealingNova",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGlowFlower"),
                            childName = "PauldronL",
                            localPos = new Vector3(0, -0.03373f, 0.00664f),
                            localAngles = new Vector3(64, 0, 0),
                            localScale = new Vector3(0.05f, 0.05f, 0.05f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "LunarUtilityReplacement",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBirdFoot"),
childName = "Head",
localPos = new Vector3(0.1761F, 0.2009F, 0F),
localAngles = new Vector3(0F, 180F, 0F),
localScale = new Vector3(0.5464F, 0.5464F, 0.5464F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Thorns",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayRazorwireLeft"),
childName = "UpperArmL",
localPos = new Vector3(0F, 0F, 0F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.6608F, 0.6608F, 0.6608F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "LunarPrimaryReplacement",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBirdEye"),
                            childName = "Head",
                            localPos = new Vector3(0, 0.0075f, 0.01f),
                            localAngles = new Vector3(-90, 0, 0),
                            localScale = new Vector3(0.025f, 0.025f, 0.025f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "NovaOnLowHealth",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayJellyGuts"),
                            childName = "ThighR",
                            localPos = new Vector3(0, 0.0135f, -0.0077f),
                            localAngles = new Vector3(-41, 0, 0),
                            localScale = new Vector3(0.01f, 0.01f, 0.01f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "LunarTrinket",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBeads"),
                            childName = "LowerArmL",
                            localPos = new Vector3(-0.0031f, 0.0147f, 0.0033f),
                            localAngles = new Vector3(0, 0, 90),
                            localScale = new Vector3(0.152965f, 0.152965f, 0.152965f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Plant",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayInterstellarDeskPlant"),
                            childName = "PauldronR",
                            localPos = new Vector3(0, 0.01f, 0),
                            localAngles = new Vector3(342, 0, 0),
                            localScale = new Vector3(0.01f, 0.01f, 0.01f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Bear",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBear"),
childName = "Shield",
localPos = new Vector3(0.364F, 0.0051F, -0.0051F),
localAngles = new Vector3(11.9039F, 129.3834F, 0F),
localScale = new Vector3(0.7238F, 0.7238F, 0.7238F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "DeathMark",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDeathMark"),
                            childName = "HandL",
                            localPos = new Vector3(0.0053f, 0.0128f, 0.0011f),
                            localAngles = new Vector3(0, 0, 180),
                            localScale = new Vector3(0.002616626f, 0.002616626f, 0.002616626f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "ExplodeOnDeath",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWilloWisp"),
                            childName = "Stomach",
                            localPos = new Vector3(0.02f, 0.02f, 0),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0.005f, 0.005f, 0.005f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Seed",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySeed"),
                            childName = "Chest",
                            localPos = new Vector3(0, 0.035f, -0.01f),
                            localAngles = new Vector3(-90, 0, 0),
                            localScale = new Vector3(0.006f, 0.006f, 0.006f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "SprintOutOfCombat",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWhip"),
                            childName = "Stomach",
                            localPos = new Vector3(0.02f, 0, -0.015f),
                            localAngles = new Vector3(0, 45, 15),
                            localScale = new Vector3(0.05f, 0.05f, 0.03f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "CooldownOnCrit",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySkull"),
                            childName = "HandR",
                            localPos = new Vector3(0, 0.01f, 0),
                            localAngles = new Vector3(0, 90, 180),
                            localScale = new Vector3(0.02f, 0.02f, 0.02f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Phasing",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayStealthkit"),
                            childName = "CalfL",
                            localPos = new Vector3(-0.0025f, 0, -0.01f),
                            localAngles = new Vector3(90, 0, 0),
                            localScale = new Vector3(0.02f, 0.02f, 0.02f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "PersonalShield",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayShieldGenerator"),
                            childName = "Chest",
                            localPos = new Vector3(-0.0075f, 0.01f, 0.025f),
                            localAngles = new Vector3(45, 90, -90),
                            localScale = new Vector3(0.02f, 0.02f, 0.02f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "ShockNearby",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayTeslaCoil"),
                            childName = "LowerArmL",
                            localPos = new Vector3(0, 0.01f, 0),
                            localAngles = new Vector3(0, 0, 90),
                            localScale = new Vector3(0.05f, 0.05f, 0.05f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "ShieldOnly",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayShieldBug"),
                            childName = "Head",
                            localPos = new Vector3(0.0083f, 0.0207f, -0.0054f),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0.035f, 0.035f, 0.035f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayShieldBug"),
                            childName = "Head",
                            localPos = new Vector3(-0.0083f, 0.0207f, -0.0054f),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(-0.035f, 0.035f, 0.035f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "AlienHead",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayAlienHead"),
                            childName = "Chest",
                            localPos = new Vector3(-0.02f, 0.02f, -0.015f),
                            localAngles = new Vector3(180, 45, 180),
                            localScale = new Vector3(0.1f, 0.1f, 0.1f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "HeadHunter",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySkullCrown"),
                            childName = "Head",
                            localPos = new Vector3(0, 0.02f, 0),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0.05f, 0.02f, 0.02f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "EnergizedOnEquipmentUse",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWarHorn"),
                            childName = "Chest",
                            localPos = new Vector3(0, 0.04f, -0.01f),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0.04f, 0.04f, 0.04f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "RegenOnKill",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySteakCurved"),
childName = "Shield",
localPos = new Vector3(0.6821F, 1.0895F, 0.3417F),
localAngles = new Vector3(315.5934F, 119.5471F, 5.0775F),
localScale = new Vector3(0.3189F, 0.3189F, 0.3189F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                //this one is supposed to be 6 display rules because hopoo is fuCKING RETARDED but i'm only doing one because it's a shit item anyway
                name = "Tooth",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayToothMeshLarge"),
                            childName = "Chest",
                            localPos = new Vector3(0, 0.03f, 0.015f),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0.5f, 0.5f, 0.5f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Pearl",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayPearl"),
                            childName = "HandR",
                            localPos = new Vector3(0, 0, 0),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0.02f, 0.02f, 0.02f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "ShinyPearl",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayShinyPearl"),
                            childName = "HandL",
                            localPos = new Vector3(0, 0, 0),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0.02f, 0.02f, 0.02f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "BonusGoldPackOnKill",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayTome"),
                            childName = "ThighR",
                            localPos = new Vector3(0, 0.035f, -0.016f),
                            localAngles = new Vector3(10, 0, 0),
                            localScale = new Vector3(0.005f, 0.005f, 0.005f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Squid",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySquidTurret"),
                            childName = "Chest",
                            localPos = new Vector3(0.015f, 0, -0.025f),
                            localAngles = new Vector3(0, -90, 90),
                            localScale = new Vector3(0.01f, 0.01f, 0.01f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Icicle",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFrostRelic"),
                            childName = "Chest",
                            localPos = new Vector3(0.05f, 0, -0.1f),
                            localAngles = new Vector3(90, 0, 0),
                            localScale = new Vector3(1, 1, 1),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Talisman",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayTalisman"),
                            childName = "Chest",
                            localPos = new Vector3(-0.05f, 0, -0.1f),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(1, 1, 1),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "LaserTurbine",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayLaserTurbine"),
                            childName = "Chest",
                            localPos = new Vector3(0.015f, -0.01f, -0.023f),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0.02f, 0.02f, 0.02f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "FocusConvergence",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFocusedConvergence"),
                            childName = "Chest",
                            localPos = new Vector3(0, 0, -0.15f),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0.1f, 0.1f, 0.1f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Incubator",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayAncestralIncubator"),
                            childName = "Chest",
                            localPos = new Vector3(-0.01f, 0.01f, 0),
                            localAngles = new Vector3(-25, 25, 0),
                            localScale = new Vector3(0.005f, 0.005f, 0.005f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "FireballsOnHit",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFireballsOnHit"),
                            childName = "LowerArmL",
                            localPos = new Vector3(0, 0.02f, -0.015f),
                            localAngles = new Vector3(-90, 0, 0),
                            localScale = new Vector3(0.0075f, 0.0075f, 0.0075f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "SiphonOnLowHealth",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySiphonOnLowHealth"),
                            childName = "Stomach",
                            localPos = new Vector3(0.01f, 0.0075f, 0.02f),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0.0075f, 0.0075f, 0.0075f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "BleedOnHitAndExplode",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBleedOnHitAndExplode"),
                            childName = "ThighR",
                            localPos = new Vector3(-0.002f, 0.01f, 0.02f),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0.005f, 0.005f, 0.005f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "MonstersOnShrineUse",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMonstersOnShrineUse"),
                            childName = "ThighR",
                            localPos = new Vector3(0, 0.01f, -0.0125f),
                            localAngles = new Vector3(0, -90, 0),
                            localScale = new Vector3(0.0075f, 0.0075f, 0.0075f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "RandomDamageZone",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayRandomDamageZone"),
                            childName = "HandL",
                            localPos = new Vector3(-0.01f, 0.005f, 0.005f),
                            localAngles = new Vector3(0, 90, 90),
                            localScale = new Vector3(0.01f, 0.005f, 0.007f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list2.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Fruit",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFruit"),
                            childName = "Chest",
                            localPos = new Vector3(0.0193f, -0.0069f, 0.0025f),
                            localAngles = new Vector3(0, -70, 30),
                            localScale = new Vector3(0.02759527f, 0.02759527f, 0.02759527f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list2.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "AffixRed",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteHorn"),
                            childName = "Head",
                            localPos = new Vector3(0.0072f, 0.015f, 0),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0.009661668f, 0.009661668f, 0.009661668f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteHorn"),
                            childName = "Head",
                            localPos = new Vector3(-0.0072f, 0.015f, 0),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(-0.009661668f, 0.009661668f, 0.009661668f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list2.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "AffixBlue",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteRhinoHorn"),
                            childName = "Head",
                            localPos = new Vector3(0, 0.015f, 0.0168f),
                            localAngles = new Vector3(-45, 0, 0),
                            localScale = new Vector3(0.032f, 0.032f, 0.032f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteRhinoHorn"),
                            childName = "Head",
                            localPos = new Vector3(0, 0.02f, 0.01012f),
                            localAngles = new Vector3(-69, 0, 0),
                            localScale = new Vector3(0.016f, 0.016f, 0.016f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list2.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "AffixWhite",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteIceCrown"),
                            childName = "Head",
                            localPos = new Vector3(0, 0.0254f, -0.0012f),
                            localAngles = new Vector3(-90, 0, 0),
                            localScale = new Vector3(0.002664f, 0.002664f, 0.002664f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list2.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "AffixPoison",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteUrchinCrown"),
                            childName = "Head",
                            localPos = new Vector3(0, 0.015f, 0),
                            localAngles = new Vector3(-90, 0, 0),
                            localScale = new Vector3(0.005f, 0.005f, 0.005f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list2.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "AffixHaunted",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteStealthCrown"),
                            childName = "Head",
                            localPos = new Vector3(0, 0.025f, 0),
                            localAngles = new Vector3(-90, 0, 0),
                            localScale = new Vector3(0.005f, 0.005f, 0.005f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list2.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "CritOnUse",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayNeuralImplant"),
                            childName = "Head",
                            localPos = new Vector3(0, 0.005f, 0.025f),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0.025f, 0.02f, 0.02f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list2.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "DroneBackup",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayRadio"),
                            childName = "Stomach",
                            localPos = new Vector3(0.02f, 0.02f, 0),
                            localAngles = new Vector3(-20, 90, 0),
                            localScale = new Vector3(0.05f, 0.05f, 0.05f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list2.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Lightning",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.capacitorPrefab,
                            childName = "PauldronL",
                            localPos = new Vector3(-0.0128f, -0.0046f, 0.0311f),
                            localAngles = new Vector3(64, 12, 120),
                            localScale = new Vector3(0.06833912f, 0.06833912f, 0.06833912f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list2.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "BurnNearby",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayPotion"),
                            childName = "Chest",
                            localPos = new Vector3(0.02f, 0.04f, 0),
                            localAngles = new Vector3(0, 0, -45),
                            localScale = new Vector3(0.005f, 0.005f, 0.005f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list2.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "CrippleWard",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEffigy"),
                            childName = "Chest",
                            localPos = new Vector3(0.0134f, 0.02949f, -0.00808f),
                            localAngles = new Vector3(0, 180, 0),
                            localScale = new Vector3(0.04f, 0.04f, 0.04f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list2.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "QuestVolatileBattery",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBatteryArray"),
                            childName = "Chest",
                            localPos = new Vector3(0, 0.0189f, -0.0274f),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0.02f, 0.02f, 0.02f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list2.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "GainArmor",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayElephantFigure"),
                            childName = "CalfR",
                            localPos = new Vector3(0, 0.02f, 0.008f),
                            localAngles = new Vector3(115, 0, 0),
                            localScale = new Vector3(0.05f, 0.05f, 0.05f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list2.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Recycle",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayRecycler"),
                            childName = "Chest",
                            localPos = new Vector3(0, 0.015f, -0.03f),
                            localAngles = new Vector3(0, 90, 0),
                            localScale = new Vector3(0.01f, 0.01f, 0.01f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list2.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "FireBallDash",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEgg"),
                            childName = "Chest",
                            localPos = new Vector3(0.015f, 0.035f, -0.01f),
                            localAngles = new Vector3(0, 90, 0),
                            localScale = new Vector3(0.02f, 0.02f, 0.02f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list2.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Cleanse",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWaterPack"),
                            childName = "Chest",
                            localPos = new Vector3(0, 0, -0.02f),
                            localAngles = new Vector3(0, 180, 0),
                            localScale = new Vector3(0.01f, 0.01f, 0.01f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list2.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Tonic",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayTonic"),
                            childName = "Chest",
                            localPos = new Vector3(0.015f, 0.045f, 0),
                            localAngles = new Vector3(0, 90, 0),
                            localScale = new Vector3(0.02f, 0.02f, 0.02f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list2.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Gateway",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayVase"),
                            childName = "Chest",
                            localPos = new Vector3(0.01f, 0.04f, -0.015f),
                            localAngles = new Vector3(-45, 0, 0),
                            localScale = new Vector3(0.02f, 0.02f, 0.02f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list2.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Meteor",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMeteor"),
                            childName = "Chest",
                            localPos = new Vector3(0, 0.05f, -0.15f),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(1, 1, 1),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list2.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Saw",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySawmerang"),
                            childName = "LowerArmL",
                            localPos = new Vector3(-0.05f, 0.05f, 0),
                            localAngles = new Vector3(0, 90, 0),
                            localScale = new Vector3(0.2f, 0.2f, 0.2f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list2.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Blackhole",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGravCube"),
                            childName = "Chest",
                            localPos = new Vector3(0, 0.05f, -0.15f),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(1, 1, 1),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list2.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Scanner",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayScanner"),
                            childName = "Stomach",
                            localPos = new Vector3(0.025f, 0.01f, 0),
                            localAngles = new Vector3(-90, 90, 0),
                            localScale = new Vector3(0.02f, 0.02f, 0.02f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list2.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "DeathProjectile",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDeathProjectile"),
                            childName = "Stomach",
                            localPos = new Vector3(-0.01f, 0.005f, -0.025f),
                            localAngles = new Vector3(0, 180, 0),
                            localScale = new Vector3(0.01f, 0.01f, 0.01f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list2.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "LifestealOnHit",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayLifestealOnHit"),
                            childName = "Head",
                            localPos = new Vector3(0.02f, 0.04f, -0.01f),
                            localAngles = new Vector3(45, 270, 0),
                            localScale = new Vector3(0.015f, 0.015f, 0.015f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            list2.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "TeamWarCry",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayTeamWarCry"),
                            childName = "Stomach",
                            localPos = new Vector3(0, 0.015f, 0.03f),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0.01f, 0.01f, 0.01f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });
            #endregion

            //aetherium displays
            #region Aetherium
            if (EnforcerPlugin.aetheriumInstalled)
            {
                list.Add(new ItemDisplayRuleSet.NamedRuleGroup
                {
                    name = "ITEM_ACCURSED_POTION",
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                            new ItemDisplayRule
                            {
                                ruleType = ItemDisplayRuleType.ParentedPrefab,
                                followerPrefab = ItemDisplays.LoadAetheriumDisplay("AccursedPotion"),
                                childName = "Stomach",
                                localPos = new Vector3(0.1f, 0.15f, -0.04f),
                                localAngles = new Vector3(0, 0, 0),
                                localScale = new Vector3(0.015f, 0.015f, 0.015f),
                                limbMask = LimbFlags.None
                            }
                        }
                    }
                });

                list.Add(new ItemDisplayRuleSet.NamedRuleGroup
                {
                    name = "ITEM_ALIEN_MAGNET",
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                            new ItemDisplayRule
                            {
                                ruleType = ItemDisplayRuleType.ParentedPrefab,
                                followerPrefab = ItemDisplays.LoadAetheriumDisplay("AlienMagnet"),
                                childName = "Base",
                                localPos = new Vector3(0.1f, 0.15f, -0.04f),
                                localAngles = new Vector3(0, 0, 0),
                                localScale = new Vector3(0.015f, 0.015f, 0.015f),
                                limbMask = LimbFlags.None
                            }
                        }
                    }
                });

                list.Add(new ItemDisplayRuleSet.NamedRuleGroup
                {
                    name = "ITEM_VOID_HEART",
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                            new ItemDisplayRule
                            {
                                ruleType = ItemDisplayRuleType.ParentedPrefab,
                                followerPrefab = ItemDisplays.LoadAetheriumDisplay("VoidHeart"),
                                childName = "Chest",
                                localPos = new Vector3(0, 0, 0),
                                localAngles = new Vector3(0, 0, 0),
                                localScale = new Vector3(0.0125f, 0.0125f, 0.0125f),
                                limbMask = LimbFlags.None
                            }
                        }
                    }
                });

                list.Add(new ItemDisplayRuleSet.NamedRuleGroup
                {
                    name = "ITEM_SHARK_TEETH",
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                            new ItemDisplayRule
                            {
                                ruleType = ItemDisplayRuleType.ParentedPrefab,
                                followerPrefab = ItemDisplays.LoadAetheriumDisplay("SharkTeeth"),
                                childName = "ThighL",
                                localPos = new Vector3(0, 0.06f, 0),
                                localAngles = new Vector3(0, 0, 300),
                                localScale = new Vector3(0.05f, 0.05f, 0.03f),
                                limbMask = LimbFlags.None
                            }
                        }
                    }
                });

                list.Add(new ItemDisplayRuleSet.NamedRuleGroup
                {
                    name = "ITEM_BLOOD_SOAKED_SHIELD",
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                            new ItemDisplayRule
                            {
                                ruleType = ItemDisplayRuleType.ParentedPrefab,
                                followerPrefab = ItemDisplays.LoadAetheriumDisplay("BloodSoakedShield"),
                                childName = "LowerArmL",
                                localPos = new Vector3(-0.01f, 0.01f, 0),
                                localAngles = new Vector3(0, 270, 0),
                                localScale = new Vector3(0.03f, 0.03f, 0.03f),
                                limbMask = LimbFlags.None
                            }
                        }
                    }
                });

                list.Add(new ItemDisplayRuleSet.NamedRuleGroup
                {
                    name = "ITEM_INSPIRING_DRONE",
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                            new ItemDisplayRule
                            {
                                ruleType = ItemDisplayRuleType.ParentedPrefab,
                                followerPrefab = ItemDisplays.LoadAetheriumDisplay("InspiringDrone"),
                                childName = "Base",
                                localPos = new Vector3(-0.1f, 0.15f, -0.04f),
                                localAngles = new Vector3(0, 0, 0),
                                localScale = new Vector3(0.015f, 0.015f, 0.015f),
                                limbMask = LimbFlags.None
                            }
                        }
                    }
                });

                list.Add(new ItemDisplayRuleSet.NamedRuleGroup
                {
                    name = "ITEM_FEATHERED_PLUME",
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                            new ItemDisplayRule
                            {
                                ruleType = ItemDisplayRuleType.ParentedPrefab,
                                followerPrefab = ItemDisplays.LoadAetheriumDisplay("FeatheredPlume"),
                                childName = "Head",
                                localPos = new Vector3(0, 0.02f, -0.01f),
                                localAngles = new Vector3(335, 0, 0),
                                localScale = new Vector3(0.05f, 0.05f, 0.05f),
                                limbMask = LimbFlags.None
                            }
                        }
                    }
                });

                list.Add(new ItemDisplayRuleSet.NamedRuleGroup
                {
                    name = "ITEM_SHIELDING_CORE",
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                            new ItemDisplayRule
                            {
                                ruleType = ItemDisplayRuleType.ParentedPrefab,
                                followerPrefab = ItemDisplays.LoadAetheriumDisplay("ShieldingCore"),
                                childName = "Chest",
                                localPos = new Vector3(0, 0.02f, -0.025f),
                                localAngles = new Vector3(0, 90, 0),
                                localScale = new Vector3(0.01f, 0.01f, 0.01f),
                                limbMask = LimbFlags.None
                            }
                        }
                    }
                });

                list.Add(new ItemDisplayRuleSet.NamedRuleGroup
                {
                    name = "ITEM_UNSTABLE_DESIGN",
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                            new ItemDisplayRule
                            {
                                ruleType = ItemDisplayRuleType.ParentedPrefab,
                                followerPrefab = ItemDisplays.LoadAetheriumDisplay("UnstableDesign"),
                                childName = "Chest",
                                localPos = new Vector3(0, -0.01f, -0.02f),
                                localAngles = new Vector3(0, 45, 0),
                                localScale = new Vector3(0.1f, 0.1f, 0.1f),
                                limbMask = LimbFlags.None
                            }
                        }
                    }
                });

                list.Add(new ItemDisplayRuleSet.NamedRuleGroup
                {
                    name = "ITEM_WEIGHTED_ANKLET",
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                            new ItemDisplayRule
                            {
                                ruleType = ItemDisplayRuleType.ParentedPrefab,
                                followerPrefab = ItemDisplays.LoadAetheriumDisplay("WeightedAnklet"),
                                childName = "CalfL",
                                localPos = new Vector3(-0.002f, 0.01f, -0.003f),
                                localAngles = new Vector3(0, 0, 0),
                                localScale = new Vector3(0.02f, 0.025f, 0.025f),
                                limbMask = LimbFlags.None
                            }
                        }
                    }
                });

                list.Add(new ItemDisplayRuleSet.NamedRuleGroup
                {
                    name = "ITEM_BLASTER_SWORD",
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadAetheriumDisplay("BlasterSword"),
                            childName = "Gun",
                            localPos = new Vector3(0.1f, 0, 0),
                            localAngles = new Vector3(0, 0, 90),
                            localScale = new Vector3(0.01f, 0.01f, 0.01f),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });
                
                list.Add(new ItemDisplayRuleSet.NamedRuleGroup
                {
                    name = "ITEM_WITCHES_RING",
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                            new ItemDisplayRule
                            {
                                ruleType = ItemDisplayRuleType.ParentedPrefab,
                                followerPrefab = ItemDisplays.LoadAetheriumDisplay("WitchesRing"),
                                childName = "LowerArmL",
                                localPos = new Vector3(0, 0, 0.02f),
                                localAngles = new Vector3(0, 270, 0),
                                localScale = new Vector3(0.04f, 0.04f, 0.04f),
                                limbMask = LimbFlags.None
                            }
                        }
                    }
                });

                list2.Add(new ItemDisplayRuleSet.NamedRuleGroup
                {
                    name = "EQUIPMENT_JAR_OF_RESHAPING",
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                            new ItemDisplayRule
                            {
                                ruleType = ItemDisplayRuleType.ParentedPrefab,
                                followerPrefab = ItemDisplays.LoadAetheriumDisplay("JarOfReshaping"),
                                childName = "Base",
                                localPos = new Vector3(0, 0.2f, -0.05f),
                                localAngles = new Vector3(0, 0, 0),
                                localScale = new Vector3(0.01f, 0.01f, 0.01f),
                                limbMask = LimbFlags.None
                            }
                        }
                    }
                });
            }
            #endregion

            //sivsitems displays
            #region SivsItems
            if (EnforcerPlugin.sivsItemsInstalled)
            {
                list.Add(new ItemDisplayRuleSet.NamedRuleGroup
                {
                    name = "BeetlePlush",
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                            new ItemDisplayRule
                            {
                                ruleType = ItemDisplayRuleType.ParentedPrefab,
                                followerPrefab = ItemDisplays.LoadSivDisplay("BeetlePlush"),
                                childName = "Chest",
                                localPos = new Vector3(0f, 0.03f, -0.025f),
                                localAngles = new Vector3(0, 0, 0),
                                localScale = new Vector3(0.1f, 0.1f, 0.1f),
                                limbMask = LimbFlags.None
                            }
                        }
                    }
                });

                list.Add(new ItemDisplayRuleSet.NamedRuleGroup
                {
                    name = "BisonShield",
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                            new ItemDisplayRule
                            {
                                ruleType = ItemDisplayRuleType.ParentedPrefab,
                                followerPrefab = ItemDisplays.LoadSivDisplay("BisonShield"),
                                childName = "LowerArmL",
                                localPos = new Vector3(-0.005f, 0.03f, 0),
                                localAngles = new Vector3(0, 270, 0),
                                localScale = new Vector3(0.075f, 0.075f, 0.075f),
                                limbMask = LimbFlags.None
                            }
                        }
                    }
                });

                list.Add(new ItemDisplayRuleSet.NamedRuleGroup
                {
                    name = "FlameGland",
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                            new ItemDisplayRule
                            {
                                ruleType = ItemDisplayRuleType.ParentedPrefab,
                                followerPrefab = ItemDisplays.LoadSivDisplay("FlameGland"),
                                childName = "Chest",
                                localPos = new Vector3(0.02f, -0.01f, 0),
                                localAngles = new Vector3(0, 270, 0),
                                localScale = new Vector3(0.05f, 0.05f, 0.05f),
                                limbMask = LimbFlags.None
                            }
                        }
                    }
                });

                list.Add(new ItemDisplayRuleSet.NamedRuleGroup
                {
                    name = "Geode",
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                            new ItemDisplayRule
                            {
                                ruleType = ItemDisplayRuleType.ParentedPrefab,
                                followerPrefab = ItemDisplays.LoadSivDisplay("Geode"),
                                childName = "ThighL",
                                localPos = new Vector3(0, 0.035f, -0.01f),
                                localAngles = new Vector3(90, 0, 0),
                                localScale = new Vector3(0.02f, 0.03f, 0.03f),
                                limbMask = LimbFlags.None
                            }
                        }
                    }
                });

                list.Add(new ItemDisplayRuleSet.NamedRuleGroup
                {
                    name = "ImpEye",
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                            new ItemDisplayRule
                            {
                                ruleType = ItemDisplayRuleType.ParentedPrefab,
                                followerPrefab = ItemDisplays.LoadSivDisplay("ImpEye"),
                                childName = "Head",
                                localPos = new Vector3(0, 0.008f, 0.005f),
                                localAngles = new Vector3(0, 0, 0),
                                localScale = new Vector3(0.1f, 0.1f, 0.1f),
                                limbMask = LimbFlags.None
                            }
                        }
                    }
                });

                list.Add(new ItemDisplayRuleSet.NamedRuleGroup
                {
                    name = "NullSeed",
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                            new ItemDisplayRule
                            {
                                ruleType = ItemDisplayRuleType.ParentedPrefab,
                                followerPrefab = ItemDisplays.LoadSivDisplay("NullSeed"),
                                childName = "Base",
                                localPos = new Vector3(0.1f, 0.15f, 0),
                                localAngles = new Vector3(0, 0, 0),
                                localScale = new Vector3(2, 2, 2),
                                limbMask = LimbFlags.None
                            }
                        }
                    }
                });

                list.Add(new ItemDisplayRuleSet.NamedRuleGroup
                {
                    name = "Tarbine",
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                            new ItemDisplayRule
                            {
                                ruleType = ItemDisplayRuleType.ParentedPrefab,
                                followerPrefab = ItemDisplays.LoadSivDisplay("Tarbine"),
                                childName = "HandL",
                                localPos = new Vector3(0.005f, 0.02f, 0.02f),
                                localAngles = new Vector3(0, 180, 0),
                                localScale = new Vector3(0.065f, 0.065f, 0.065f),
                                limbMask = LimbFlags.None
                            }
                        }
                    }
                });

                list.Add(new ItemDisplayRuleSet.NamedRuleGroup
                {
                    name = "Tentacle",
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                            new ItemDisplayRule
                            {
                                ruleType = ItemDisplayRuleType.ParentedPrefab,
                                followerPrefab = ItemDisplays.LoadSivDisplay("Tentacle"),
                                childName = "Head",
                                localPos = new Vector3(0f, 0.015f, 0f),
                                localAngles = new Vector3(0, 0, 0),
                                localScale = new Vector3(0.1f, 0.1f, 0.1f),
                                limbMask = LimbFlags.None
                            }
                        }
                    }
                });
            }
            #endregion


            //supply drop displays
            #region SupplyDrop
            if (EnforcerPlugin.supplyDropInstalled)
            {
                list.Add(new ItemDisplayRuleSet.NamedRuleGroup
                {
                    name = "SUPPDRPBloodBook",
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                            new ItemDisplayRule
                            {
                                ruleType = ItemDisplayRuleType.ParentedPrefab,
                                followerPrefab = ItemDisplays.LoadSupplyDropDisplay("BloodBook"),
                                childName = "Base",
                                localPos = new Vector3(0f, 0.2f, -0.2f),
                                localAngles = new Vector3(0, 0, 0),
                                localScale = new Vector3(0.1f, 0.1f, 0.1f),
                                limbMask = LimbFlags.None
                            }
                        }
                    }
                });

                list.Add(new ItemDisplayRuleSet.NamedRuleGroup
                {
                    name = "SUPPDRPElectroPlankton",
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                            new ItemDisplayRule
                            {
                                ruleType = ItemDisplayRuleType.ParentedPrefab,
                                followerPrefab = ItemDisplays.LoadSupplyDropDisplay("ElectroPlankton"),
                                childName = "Chest",
                                localPos = new Vector3(0, -0.025f, -0.025f),
                                localAngles = new Vector3(0, 0, 90),
                                localScale = new Vector3(0.01f, 0.01f, 0.01f),
                                limbMask = LimbFlags.None
                            }
                        }
                    }
                });

                list.Add(new ItemDisplayRuleSet.NamedRuleGroup
                {
                    name = "SUPPDRPHardenedBoneFragments",
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                            new ItemDisplayRule
                            {
                                ruleType = ItemDisplayRuleType.ParentedPrefab,
                                followerPrefab = ItemDisplays.LoadSupplyDropDisplay("HardenedBoneFragments"),
                                childName = "Chest",
                                localPos = new Vector3(0f, 0.03f, 0.03f),
                                localAngles = new Vector3(0, 0, 0),
                                localScale = new Vector3(0.2f, 0.2f, 0.2f),
                                limbMask = LimbFlags.None
                            }
                        }
                    }
                });

                list.Add(new ItemDisplayRuleSet.NamedRuleGroup
                {
                    name = "SUPPDRPQSGen",
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                            new ItemDisplayRule
                            {
                                ruleType = ItemDisplayRuleType.ParentedPrefab,
                                followerPrefab = ItemDisplays.LoadSupplyDropDisplay("QSGen"),
                                childName = "LowerArmL",
                                localPos = new Vector3(0, 0, 0),
                                localAngles = new Vector3(0, 0, 90),
                                localScale = new Vector3(0.01f, 0.01f, 0.01f),
                                limbMask = LimbFlags.None
                            }
                        }
                    }
                });

                list.Add(new ItemDisplayRuleSet.NamedRuleGroup
                {
                    name = "SUPPDRPSalvagedWires",
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                            new ItemDisplayRule
                            {
                                ruleType = ItemDisplayRuleType.ParentedPrefab,
                                followerPrefab = ItemDisplays.LoadSupplyDropDisplay("SalvagedWires"),
                                childName = "ThighL",
                                localPos = new Vector3(0, 0.05f, -0.02f),
                                localAngles = new Vector3(0, 90, 180),
                                localScale = new Vector3(0.04f, 0.04f, 0.04f),
                                limbMask = LimbFlags.None
                            }
                        }
                    }
                });

                list.Add(new ItemDisplayRuleSet.NamedRuleGroup
                {
                    name = "SUPPDRPShellPlating",
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                            new ItemDisplayRule
                            {
                                ruleType = ItemDisplayRuleType.ParentedPrefab,
                                followerPrefab = ItemDisplays.LoadSupplyDropDisplay("ShellPlating"),
                                childName = "Chest",
                                localPos = new Vector3(0, 0, 0.0035f),
                                localAngles = new Vector3(30, 0, 0),
                                localScale = new Vector3(0.02f, 0.02f, 0.02f),
                                limbMask = LimbFlags.None
                            }
                        }
                    }
                });

                list.Add(new ItemDisplayRuleSet.NamedRuleGroup
                {
                    name = "SUPPDRPPlagueHat",
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                            new ItemDisplayRule
                            {
                                ruleType = ItemDisplayRuleType.ParentedPrefab,
                                followerPrefab = ItemDisplays.LoadSupplyDropDisplay("PlagueHat"),
childName = "Head",
localPos = new Vector3(0F, 0.0197F, 0F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.0157F, 0.0157F, 0.0157F),
                                limbMask = LimbFlags.None
                            }
                        }
                    }
                });

                list.Add(new ItemDisplayRuleSet.NamedRuleGroup
                {
                    name = "SUPPDRPPlagueMask",
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                            new ItemDisplayRule
                            {
                                ruleType = ItemDisplayRuleType.ParentedPrefab,
                                followerPrefab = ItemDisplays.LoadSupplyDropDisplay("PlagueMask"),
childName = "Head",
localPos = new Vector3(0F, 0.0111F, 0.0226F),
localAngles = new Vector3(0F, 180F, 0F),
localScale = new Vector3(0.016F, 0.016F, 0.016F),
                                limbMask = LimbFlags.None
                            }
                        }
                    }
                });
            }
            #endregion
            //apply displays here

            ItemDisplayRuleSet.NamedRuleGroup[] item = list.ToArray();
            ItemDisplayRuleSet.NamedRuleGroup[] equip = list2.ToArray();
            itemDisplayRuleSet.namedItemRuleGroups = item;
            itemDisplayRuleSet.namedEquipmentRuleGroups = equip;

            characterModel.itemDisplayRuleSet = itemDisplayRuleSet;
        }

        public static GameObject LoadDisplay(string name)
        {
            if (itemDisplayPrefabs.ContainsKey(name.ToLower()))
            {
                if (itemDisplayPrefabs[name.ToLower()]) return itemDisplayPrefabs[name.ToLower()];
            }
            return null;
        }

        private static void PopulateDisplays()
        {
            ItemDisplayRuleSet itemDisplayRuleSet = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponent<ModelLocator>().modelTransform.GetComponent<CharacterModel>().itemDisplayRuleSet;

            EnforcerPlugin.bungusMat = itemDisplayRuleSet.FindItemDisplayRuleGroup("Mushroom").rules[0].followerPrefab.GetComponentInChildren<SkinnedMeshRenderer>().material;

            capacitorPrefab = PrefabAPI.InstantiateClone(itemDisplayRuleSet.FindEquipmentDisplayRuleGroup("Lightning").rules[0].followerPrefab, "DisplayEnforcerLightning", true);
            capacitorPrefab.AddComponent<UnityEngine.Networking.NetworkIdentity>();

            var limbMatcher = capacitorPrefab.GetComponent<LimbMatcher>();

            limbMatcher.limbPairs[0].targetChildLimb = "UpperArmL";
            limbMatcher.limbPairs[1].targetChildLimb = "LowerArmL";
            limbMatcher.limbPairs[2].targetChildLimb = "HandL";

            gatDronePrefab = PrefabAPI.InstantiateClone(itemDisplayRuleSet.FindEquipmentDisplayRuleGroup("GoldGat").rules[0].followerPrefab, "DisplayEnforcerGatDrone", false);

            GameObject gatDrone = PrefabAPI.InstantiateClone(Assets.gatDrone, "GatDrone", false);

            Material gatMaterial = gatDrone.GetComponentInChildren<MeshRenderer>().material;
            Material newMaterial = UnityEngine.Object.Instantiate<Material>(Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial);

            newMaterial.SetColor("_Color", gatMaterial.GetColor("_Color"));
            newMaterial.SetTexture("_MainTex", gatMaterial.GetTexture("_MainTex"));
            newMaterial.SetFloat("_EmPower", 0f);
            newMaterial.SetColor("_EmColor", Color.black);
            newMaterial.SetFloat("_NormalStrength", 0);

            gatDrone.transform.parent = gatDronePrefab.transform;
            gatDrone.transform.localPosition = new Vector3(-0.025f, -3.1f, 0);
            gatDrone.transform.localRotation = Quaternion.Euler(new Vector3(-90, 90, 0));
            gatDrone.transform.localScale = new Vector3(175, 175, 175);

            CharacterModel.RendererInfo[] infos = gatDronePrefab.GetComponent<ItemDisplay>().rendererInfos;
            CharacterModel.RendererInfo[] newInfos = new CharacterModel.RendererInfo[]
            {
                infos[0],
                new CharacterModel.RendererInfo
                {
                    renderer = gatDrone.GetComponentInChildren<MeshRenderer>(),
                    defaultMaterial = newMaterial,
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false
                }
            };

            gatDronePrefab.GetComponent<ItemDisplay>().rendererInfos = newInfos;

            ItemDisplayRuleSet.NamedRuleGroup[] item = itemDisplayRuleSet.namedItemRuleGroups;
            ItemDisplayRuleSet.NamedRuleGroup[] equip = itemDisplayRuleSet.namedEquipmentRuleGroups;

            for (int i = 0; i < item.Length; i++)
            {
                ItemDisplayRule[] rules = item[i].displayRuleGroup.rules;

                for (int j = 0; j < rules.Length; j++)
                {
                    GameObject followerPrefab = rules[j].followerPrefab;
                    if (followerPrefab)
                    {
                        string name = followerPrefab.name;
                        string key = (name != null) ? name.ToLower() : null;
                        if (!itemDisplayPrefabs.ContainsKey(key))
                        {
                            itemDisplayPrefabs[key] = followerPrefab;
                        }
                    }
                }
            }

            for (int i = 0; i < equip.Length; i++)
            {
                ItemDisplayRule[] rules = equip[i].displayRuleGroup.rules;
                for (int j = 0; j < rules.Length; j++)
                {
                    GameObject followerPrefab = rules[j].followerPrefab;

                    if (followerPrefab)
                    {
                        string name2 = followerPrefab.name;
                        string key2 = (name2 != null) ? name2.ToLower() : null;
                        if (!itemDisplayPrefabs.ContainsKey(key2))
                        {
                            itemDisplayPrefabs[key2] = followerPrefab;
                        }
                    }
                }
            }
        }




        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static GameObject LoadAetheriumDisplay(string name)
        {
            switch (name)
            {
                case "AccursedPotion":
                    return Aetherium.Items.AccursedPotion.ItemBodyModelPrefab;
                case "AlienMagnet":
                    return Aetherium.Items.AlienMagnet.ItemFollowerPrefab;
                case "BlasterSword":
                    return Aetherium.Items.BlasterSword.ItemBodyModelPrefab;
                case "BloodSoakedShield":
                    return Aetherium.Items.BloodSoakedShield.ItemBodyModelPrefab;
                case "FeatheredPlume":
                    return Aetherium.Items.FeatheredPlume.ItemBodyModelPrefab;
                case "InspiringDrone":
                    return Aetherium.Items.InspiringDrone.ItemFollowerPrefab;
                case "SharkTeeth":
                    return Aetherium.Items.SharkTeeth.ItemBodyModelPrefab;
                case "ShieldingCore":
                    return Aetherium.Items.ShieldingCore.ItemBodyModelPrefab;
                case "UnstableDesign":
                    return Aetherium.Items.UnstableDesign.ItemBodyModelPrefab;
                case "VoidHeart":
                    return Aetherium.Items.Voidheart.ItemBodyModelPrefab;
                case "WeightedAnklet":
                    return Aetherium.Items.WeightedAnklet.ItemBodyModelPrefab;
                case "WitchesRing":
                    return Aetherium.Items.WitchesRing.ItemBodyModelPrefab;
                case "JarOfReshaping":
                    return Aetherium.Equipment.JarOfReshaping.ItemBodyModelPrefab;
            }
            return null;
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static GameObject LoadSivDisplay(string name)
        {
            switch (name)
            {
                case "BeetlePlush":
                    return SivsItemsRoR2.BeetlePlush.displayPrefab;
                case "BisonShield":
                    return SivsItemsRoR2.BisonShield.displayPrefab;
                case "FlameGland":
                    return SivsItemsRoR2.FlameGland.displayPrefab;
                case "Geode":
                    return SivsItemsRoR2.Geode.displayPrefab;
                case "ImpEye":
                    return SivsItemsRoR2.ImpEye.displayPrefab;
                case "NullSeed":
                    return SivsItemsRoR2.NullSeed.displayPrefab;
                case "Tarbine":
                    return SivsItemsRoR2.Tarbine.displayPrefab;
                case "Tentacle":
                    return SivsItemsRoR2.Tentacle.displayPrefab;
            }
            return null;
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static GameObject LoadSupplyDropDisplay(string name)
        {
            switch (name)
            {
                case "BloodBook":
                    return SupplyDrop.Items.BloodBook.ItemBodyModelPrefab;
                case "ElectroPlankton":
                    return SupplyDrop.Items.ElectroPlankton.ItemBodyModelPrefab;
                case "HardenedBoneFragments":
                    return SupplyDrop.Items.HardenedBoneFragments.ItemBodyModelPrefab;
                case "PlagueHat":
                    return SupplyDrop.Items.PlagueHat.ItemBodyModelPrefab;
                case "PlagueMask":
                    return SupplyDrop.Items.PlagueMask.ItemBodyModelPrefab;
                case "QSGen":
                    return SupplyDrop.Items.QSGen.ItemBodyModelPrefab;
                case "SalvagedWires":
                    return SupplyDrop.Items.SalvagedWires.ItemBodyModelPrefab;
                case "ShellPlating":
                    return SupplyDrop.Items.ShellPlating.ItemBodyModelPrefab;
                case "UnassumingTie":
                    return SupplyDrop.Items.UnassumingTie.ItemBodyModelPrefab;
            }
            return null;
        }
    }
}
