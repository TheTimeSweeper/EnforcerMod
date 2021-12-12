//using System;
using UnityEngine;
using R2API;
using RoR2;
using R2API.Utils;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace EnforcerPlugin.Modules
{
    internal static class ItemDisplays
    {
        private static Dictionary<string, GameObject> itemDisplayPrefabs = new Dictionary<string, GameObject>();

        internal static void PopulateDisplays()
        {
            ItemDisplayRuleSet itemDisplayRuleSet = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponent<ModelLocator>().modelTransform.GetComponent<CharacterModel>().itemDisplayRuleSet;

            ItemDisplayRuleSet.KeyAssetRuleGroup[] item = itemDisplayRuleSet.keyAssetRuleGroups;

            for (int i = 0; i < item.Length; i++)
            {
                ItemDisplayRule[] rules = item[i].displayRuleGroup.rules;

                for (int j = 0; j < rules.Length; j++)
                {
                    GameObject followerPrefab = rules[j].followerPrefab;
                    if (followerPrefab)
                    {
                        string name = followerPrefab.name;
                        string key = name != null ? name.ToLower() : null;
                        if (!itemDisplayPrefabs.ContainsKey(key))
                        {
                            itemDisplayPrefabs[key] = followerPrefab;
                        }
                    }
                }
            }
        }

        internal static GameObject LoadDisplay(string name)
        {
            if (itemDisplayPrefabs.ContainsKey(name.ToLower()))
            {
                if (itemDisplayPrefabs[name.ToLower()]) return itemDisplayPrefabs[name.ToLower()];
            }

            return null;
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
                //case "BloodSoakedShield":
                //    return Aetherium.Items.BloodSoakedShield.ItemBodyModelPrefab;
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


        public static GameObject LoadSupplyDropDisplay(string name)
        {
            switch (name)
            {
                //would be cool if these are enums maybe
                case "Bones":
                    return SupplyDrop.Items.HardenedBoneFragments.ItemBodyModelPrefab;
                case "Berries":
                    return SupplyDrop.Items.NumbingBerries.ItemBodyModelPrefab;
                case "UnassumingTie":
                    return SupplyDrop.Items.UnassumingTie.ItemBodyModelPrefab;
                case "SalvagedWires":
                    return SupplyDrop.Items.SalvagedWires.ItemBodyModelPrefab;

                case "ShellPlating":
                    return SupplyDrop.Items.ShellPlating.ItemBodyModelPrefab;
                case "ElectroPlankton":
                    return SupplyDrop.Items.ElectroPlankton.ItemBodyModelPrefab;
                case "PlagueHat":
                    return SupplyDrop.Items.PlagueHat.ItemBodyModelPrefab;
                case "PlagueMask":
                    GameObject masku = SupplyDrop.Items.PlagueMask.ItemBodyModelPrefab.InstantiateClone("PlagueMask");
                    Material heeheehee = new Material(masku.GetComponent<ItemDisplay>().rendererInfos[0].defaultMaterial);
                    heeheehee.color = Color.green; ;
                    masku.GetComponent<ItemDisplay>().rendererInfos[0].defaultMaterial = heeheehee;
                    return masku;

                case "BloodBook":
                    return SupplyDrop.Items.BloodBook.ItemBodyModelPrefab;
                case "QSGen":
                    return SupplyDrop.Items.QSGen.ItemBodyModelPrefab;
            }
            return null;
        }
        public static Object LoadSupplyDropKeyAsset(string name)
        {
            switch (name)
            {
                //would be cool if these are enums maybe
                case "Bones":
                    return SupplyDrop.Items.HardenedBoneFragments.instance.itemDef;
                case "Berries":
                    return SupplyDrop.Items.NumbingBerries.instance.itemDef;
                case "UnassumingTie":
                    return SupplyDrop.Items.UnassumingTie.instance.itemDef;
                case "SalvagedWires":
                    return SupplyDrop.Items.SalvagedWires.instance.itemDef;

                case "ShellPlating":
                    return SupplyDrop.Items.ShellPlating.instance.itemDef;
                case "ElectroPlankton":
                    return SupplyDrop.Items.ElectroPlankton.instance.itemDef;
                case "PlagueHat":
                    return SupplyDrop.Items.PlagueHat.instance.itemDef;
                case "PlagueMask":
                    return SupplyDrop.Items.PlagueMask.instance.itemDef;

                case "BloodBook":
                    return SupplyDrop.Items.BloodBook.instance.itemDef;
                case "QSGen":
                    return SupplyDrop.Items.QSGen.instance.itemDef;

            }
            return null;
        }

        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateSupplyDropRuleGroup(string itemName, string childName, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            try
            {
                return CreateGenericDisplayRuleGroup(LoadSupplyDropKeyAsset(itemName), LoadSupplyDropDisplay(itemName), childName, position, rotation, scale);
            }
            catch (System.Exception e)
            {

                Debug.LogWarning($"could not create item display for supply drop's {itemName}. skipping.\n(Error: {e.Message})");
                return new ItemDisplayRuleSet.KeyAssetRuleGroup();
            }
        }

        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateGenericDisplayRuleGroup(Object keyAsset_, GameObject itemPrefab, string childName, Vector3 position, Vector3 rotation, Vector3 scale)
        {

            ItemDisplayRule singleRule = CreateDisplayRule(itemPrefab, childName, position, rotation, scale);
            return CreateDisplayRuleGroupWithRules(keyAsset_, singleRule);
        }

        public static ItemDisplayRule CreateDisplayRule(GameObject itemPrefab, string childName, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            return new ItemDisplayRule
            {
                ruleType = ItemDisplayRuleType.ParentedPrefab,
                childName = childName,
                followerPrefab = itemPrefab,
                limbMask = LimbFlags.None,
                localPos = position,
                localAngles = rotation,
                localScale = scale
            };
        }

        public static ItemDisplayRule CreateDisplayRule(string prefabName, string childName, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            return new ItemDisplayRule
            {
                ruleType = ItemDisplayRuleType.ParentedPrefab,
                childName = childName,
                followerPrefab = LoadDisplay(prefabName),
                limbMask = LimbFlags.None,
                localPos = position,
                localAngles = rotation,
                localScale = scale
            };
        }

        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateDisplayRuleGroupWithRules(string itemName, params ItemDisplayRule[] rules)
        {
            return CreateDisplayRuleGroupWithRules(Resources.Load<ItemDef>("ItemDefs/" + itemName), rules);
        }

        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateDisplayRuleGroupWithRules(Object keyAsset_, params ItemDisplayRule[] rules)
        {
            return new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = keyAsset_,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = rules
                }
            };
        }

        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateGenericDisplayRule(string itemName, string prefabName, string childName, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            ItemDisplayRuleSet.KeyAssetRuleGroup displayRule = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = Resources.Load<ItemDef>("ItemDefs/" + itemName),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            childName = childName,
                            followerPrefab = LoadDisplay(prefabName),
                            limbMask = LimbFlags.None,
                            localPos = position,
                            localAngles = rotation,
                            localScale = scale
                        }
                    }
                }
            };

            return displayRule;
        }

        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateGenericDisplayRuleE(string itemName, string prefabName, string childName, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            ItemDisplayRuleSet.KeyAssetRuleGroup displayRule = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = Resources.Load<EquipmentDef>("EquipmentDefs/" + itemName),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            childName = childName,
                            followerPrefab = LoadDisplay(prefabName),
                            limbMask = LimbFlags.None,
                            localPos = position,
                            localAngles = rotation,
                            localScale = scale
                        }
                    }
                }
            };

            return displayRule;
        }

        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateGenericDisplayRule(string itemName, GameObject itemPrefab, string childName, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            ItemDisplayRuleSet.KeyAssetRuleGroup displayRule = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = Resources.Load<ItemDef>("ItemDefs/" + itemName),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            childName = childName,
                            followerPrefab = itemPrefab,
                            limbMask = LimbFlags.None,
                            localPos = position,
                            localAngles = rotation,
                            localScale = scale
                        }
                    }
                }
            };

            return displayRule;
        }

        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateFollowerDisplayRule(string itemName, string prefabName, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            ItemDisplayRuleSet.KeyAssetRuleGroup displayRule = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = Resources.Load<ItemDef>("ItemDefs/" + itemName),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            childName = "Base",
                            followerPrefab = LoadDisplay(prefabName),
                            limbMask = LimbFlags.None,
                            localPos = position,
                            localAngles = rotation,
                            localScale = scale
                        }
                    }
                }
            };

            return displayRule;
        }

        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateFollowerDisplayRuleE(string itemName, string prefabName, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            ItemDisplayRuleSet.KeyAssetRuleGroup displayRule = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = Resources.Load<EquipmentDef>("EquipmentDefs/" + itemName),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            childName = "Base",
                            followerPrefab = LoadDisplay(prefabName),
                            limbMask = LimbFlags.None,
                            localPos = position,
                            localAngles = rotation,
                            localScale = scale
                        }
                    }
                }
            };

            return displayRule;
        }

        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateFollowerDisplayRule(string itemName, GameObject itemPrefab, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            ItemDisplayRuleSet.KeyAssetRuleGroup displayRule = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = Resources.Load<ItemDef>("ItemDefs/" + itemName),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            childName = "Base",
                            followerPrefab = itemPrefab,
                            limbMask = LimbFlags.None,
                            localPos = position,
                            localAngles = rotation,
                            localScale = scale
                        }
                    }
                }
            };

            return displayRule;
        }

        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateFollowerDisplayRuleE(string itemName, GameObject itemPrefab, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            ItemDisplayRuleSet.KeyAssetRuleGroup displayRule = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = Resources.Load<EquipmentDef>("EquipmentDefs/" + itemName),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            childName = "Base",
                            followerPrefab = itemPrefab,
                            limbMask = LimbFlags.None,
                            localPos = position,
                            localAngles = rotation,
                            localScale = scale
                        }
                    }
                }
            };

            return displayRule;
        }

    }
}