//using System;
using UnityEngine;
using R2API;
using RoR2;
using R2API.Utils;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Modules {
    internal static class ItemDisplays {
        public static Dictionary<string, GameObject> itemDisplayPrefabs = new Dictionary<string, GameObject>();


        public static GameObject gatDronePrefab;

        #region printing unused
        public static bool printingUnused = false;
        public static Dictionary<string, int> itemDisplayCheckCount = new Dictionary<string, int>();
        public static Dictionary<string, string> itemDisplayCheckName = new Dictionary<string, string>();
        #endregion

        internal static void PopulateDisplays() {
            //PopulateDisplaysFromBody("CommandoBody");
            //PopulateDisplaysFromBody("CrocoBody");
            PopulateDisplaysFromBody("MageBody");
            PopulateDisplaysFromBody("LunarExploderBody");

            CreateFuckingGatDrone();
            AddCustomLightningArm();

            //foreach (KeyValuePair<string, GameObject> pair in itemDisplayPrefabs)
            //{
            //    itemDisplayCheck[pair.Key] = 0;
            //}

        }

        private static void CreateFuckingGatDrone() {
            ItemDisplayRuleSet itemDisplayRuleSet = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponent<ModelLocator>().modelTransform.GetComponent<CharacterModel>().itemDisplayRuleSet;

            gatDronePrefab = LoadDisplay("DisplayGoldGat").InstantiateClone("DisplayEnforcerGatDrone", false);

            GameObject gatDrone = Asset.gatDrone.InstantiateClone("GatDrone", false);

            Material gatMaterial = gatDrone.GetComponentInChildren<MeshRenderer>().material;
            Material newMaterial = Object.Instantiate(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial);

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

            itemDisplayPrefabs["DisplayGoldGatDrone"] = gatDronePrefab;
        }


        private static void AddCustomLightningArm() {
            #region IgnoreThisAndRunAway
            //seriously you don't need this
            //I see you're still here, well if you do need this here's what you do
            //but again you don't need this
            //capacitor is hardcoded to track your "UpperArmR", "LowerArmR", and "HandR" bones.
            //this is for having the lightning on custom bones in your childlocator

            GameObject display = R2API.PrefabAPI.InstantiateClone(itemDisplayPrefabs["DisplayLightningArmRight".ToLowerInvariant()], "DisplayLightningArmCustom", false);

            LimbMatcher limbMatcher = display.GetComponent<LimbMatcher>();

            limbMatcher.limbPairs[0].targetChildLimb = "ShoulderR";
            limbMatcher.limbPairs[1].targetChildLimb = "ElbowR";
            limbMatcher.limbPairs[2].targetChildLimb = "HandR";

            string key = "DisplayLightningArmNem".ToLowerInvariant();
            itemDisplayPrefabs[key] = display;
            #endregion
        }
        private static void PopulateDisplaysFromBody(string body) {
            ItemDisplayRuleSet itemDisplayRuleSet = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/" + body).GetComponent<ModelLocator>().modelTransform.GetComponent<CharacterModel>().itemDisplayRuleSet;

            ItemDisplayRuleSet.KeyAssetRuleGroup[] itemGroups = itemDisplayRuleSet.keyAssetRuleGroups;

            for (int i = 0; i < itemGroups.Length; i++) {
                ItemDisplayRule[] rules = itemGroups[i].displayRuleGroup.rules;

                for (int j = 0; j < rules.Length; j++) {
                    GameObject followerPrefab = rules[j].followerPrefab;
                    if (followerPrefab) {
                        string name = followerPrefab.name;
                        string key = name != null ? name.ToLower() : null;

                        if (!itemDisplayPrefabs.ContainsKey(key)) {
                            itemDisplayPrefabs[key] = followerPrefab;

                            if (printingUnused) {
                                itemDisplayCheckCount[key] = 0;
                                itemDisplayCheckName[key] = itemGroups[i].keyAsset.name;
                            }
                        }
                    }
                }
            }
        }

        public static void printUnused() {

            string yes = "used:";
            string no = "not used:";

            foreach (KeyValuePair<string, int> pair in itemDisplayCheckCount) {
                string thing = $"\n{itemDisplayPrefabs[pair.Key].name} | {itemDisplayCheckName[pair.Key]} | {pair.Value}";

                if (pair.Value > 0) {
                    yes += thing;
                } else {
                    no += thing;
                }
            }
            //Debug.Log(yes);
            Debug.LogWarning(no);
        }

        internal static GameObject LoadDisplay(string name) {

            if (itemDisplayPrefabs.ContainsKey(name.ToLower())) {

                if (itemDisplayPrefabs[name.ToLower()]) {

                    if(printingUnused)
                        itemDisplayCheckCount[name.ToLower()]++;

                    return itemDisplayPrefabs[name.ToLower()];
                }
            }

            Debug.LogError("could not load display for " + name);
            return null;
        }

        public static GameObject LoadSupplyDropDisplay(string name) {
            switch (name) {
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
                    GameObject masku = SupplyDrop.Items.PlagueMask.ItemBodyModelPrefab.InstantiateClone("PlagueMask", false);
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
        public static Object LoadSupplyDropKeyAsset(string name) {
            switch (name) {
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

        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateSupplyDropRuleGroup(string itemName, string childName, Vector3 position, Vector3 rotation, Vector3 scale) {
            try {
                return CreateGenericDisplayRuleGroup(LoadSupplyDropKeyAsset(itemName), LoadSupplyDropDisplay(itemName), childName, position, rotation, scale);
            } catch (System.Exception e) {

                Debug.LogWarning($"could not create item display for supply drop's {itemName}. skipping.\n(Error: {e.Message})");
                return new ItemDisplayRuleSet.KeyAssetRuleGroup();
            }
        }

        private static Object GetKeyAssetFromString(string itemName) {
            Object itemDef = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/" + itemName);

            if (itemDef == null) {
                itemDef = RoR2.LegacyResourcesAPI.Load<EquipmentDef>("EquipmentDefs/" + itemName);
            }

            if (itemDef == null) {
                Debug.LogError("Could not load keyasset for " + itemName);
            }

            return itemDef;
        }

        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateGenericDisplayRuleGroup(Object keyAsset_, GameObject itemPrefab, string childName, Vector3 position, Vector3 rotation, Vector3 scale) {

            ItemDisplayRule singleRule = CreateDisplayRule(itemPrefab, childName, position, rotation, scale);
            return CreateDisplayRuleGroupWithRules(keyAsset_, singleRule);
        }

        public static ItemDisplayRule CreateDisplayRule(string prefabName, string childName, Vector3 position, Vector3 rotation, Vector3 scale) {
            return CreateDisplayRule(LoadDisplay(prefabName), childName, position, rotation, scale);
        }
        public static ItemDisplayRule CreateDisplayRule(GameObject itemPrefab, string childName, Vector3 position, Vector3 rotation, Vector3 scale) {
            return new ItemDisplayRule {
                ruleType = ItemDisplayRuleType.ParentedPrefab,
                childName = childName,
                followerPrefab = itemPrefab,
                limbMask = LimbFlags.None,
                localPos = position,
                localAngles = rotation,
                localScale = scale
            };
        }

        public static ItemDisplayRule CreateLimbMaskDisplayRule(LimbFlags limb) {
            return new ItemDisplayRule {
                ruleType = ItemDisplayRuleType.LimbMask,
                limbMask = limb,
                childName = "",
                followerPrefab = null
                //localPos = Vector3.zero,
                //localAngles = Vector3.zero,
                //localScale = Vector3.zero
            };
        }

        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateDisplayRuleGroupWithRules(string itemName, params ItemDisplayRule[] rules) {
            return CreateDisplayRuleGroupWithRules(GetKeyAssetFromString(itemName), rules);
        }
        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateDisplayRuleGroupWithRules(Object keyAsset_, params ItemDisplayRule[] rules) {
            return new ItemDisplayRuleSet.KeyAssetRuleGroup {
                keyAsset = keyAsset_,
                displayRuleGroup = new DisplayRuleGroup {
                    rules = rules
                }
            };
        }

        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateGenericDisplayRule(string itemName, string prefabName, string childName, Vector3 position, Vector3 rotation, Vector3 scale) {
            return CreateGenericDisplayRule(GetKeyAssetFromString(itemName), prefabName, childName, position, rotation, scale);
        }
        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateGenericDisplayRule(Object itemDef, string prefabName, string childName, Vector3 position, Vector3 rotation, Vector3 scale) {
            return CreateGenereicDisplayRule(itemDef, LoadDisplay(prefabName), childName, position, rotation, scale);
        }
        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateGenericDisplayRule(string itemName, GameObject displayPrefab, string childName, Vector3 position, Vector3 rotation, Vector3 scale) {
            return CreateGenereicDisplayRule(GetKeyAssetFromString(itemName), displayPrefab, childName, position, rotation, scale);
        }
        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateGenereicDisplayRule(Object itemDef, GameObject displayPrefab, string childName, Vector3 position, Vector3 rotation, Vector3 scale) {
           
            
            return new ItemDisplayRuleSet.KeyAssetRuleGroup {

                keyAsset = itemDef,
                displayRuleGroup = new DisplayRuleGroup {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            childName = childName,
                            followerPrefab = displayPrefab,
                            limbMask = LimbFlags.None,
                            localPos = position,
                            localAngles = rotation,
                            localScale = scale
                        }
                    }
                }
            };
        }
    }
}