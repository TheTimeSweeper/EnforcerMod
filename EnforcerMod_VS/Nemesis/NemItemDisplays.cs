using RoR2;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace EnforcerPlugin
{
    public static class NemItemDisplays
    {
        public static List<ItemDisplayRuleSet.NamedRuleGroup> itemRules;
        public static List<ItemDisplayRuleSet.NamedRuleGroup> equipmentRules;

        public static void RegisterDisplays()
        {
            GameObject bodyPrefab = NemforcerPlugin.characterPrefab;

            GameObject model = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = model.GetComponent<CharacterModel>();

            ItemDisplayRuleSet itemDisplayRuleSet = ScriptableObject.CreateInstance<ItemDisplayRuleSet>();

            itemRules = new List<ItemDisplayRuleSet.NamedRuleGroup>();
            equipmentRules = new List<ItemDisplayRuleSet.NamedRuleGroup>();

            #region Display Rules

            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("CritGlasses", "DisplayGlasses", "Head", new Vector3(-0.003f, 0.008f, 0f), new Vector3(335, 270, 0), new Vector3(0.01f, 0.01f, 0.01f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("Syringe", "DisplaySyringeCluster", "Chest", new Vector3(0, 0.012f, 0.006f), new Vector3(25, 315, 0), new Vector3(0.005f, 0.005f, 0.005f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("NearbyDamageBonus", "DisplayDiamond", "Hammer", new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0.005f, 0.005f, 0.005f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("ArmorReductionOnHit", "DisplayWarhammer", "Hammer", new Vector3(0, 0.005f, 0), new Vector3(270, 90, 0), new Vector3(0.0175f, 0.0175f, 0.0175f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("SecondarySkillMagazine", "DisplayDoubleMag", "Hammer", new Vector3(0, 0.016f, 0), new Vector3(70, 0, 180), new Vector3(0.005f, 0.005f, 0.005f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("Bear", "DisplayBear", "Hammer", new Vector3(0, 0.012f, 0.012f), new Vector3(0, 0, 0), new Vector3(0.0175f, 0.0175f, 0.0175f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("SprintOutOfCombat", "DisplayWhip", "Pelvis", new Vector3(0, 0.004f, 0.0085f), new Vector3(90, 180, 0), new Vector3(0.0175f, 0.0175f, 0.0175f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("PersonalShield", "DisplayShieldGenerator", "Chest", new Vector3(-0.006f, 0.005f, 0.005f), new Vector3(90, 100, 0), new Vector3(0.005f, 0.005f, 0.005f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("RegenOnKill", "DisplaySteakCurved", "Hammer", new Vector3(0, 0.015f, 0.015f), new Vector3(335, 0, 180), new Vector3(0.0075f, 0.0075f, 0.0075f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("FireballsOnHit", "DisplayFireballsOnHit", "Hammer", new Vector3(0, 0.02f, 0.01f), new Vector3(0, 0, 0), new Vector3(0.005f, 0.005f, 0.005f)));
            //itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("Hoof", "DisplayHoof", "KneeR", new Vector3(0, 0.005f, 0), new Vector3(270, 90, 0), new Vector3(0.0175f, 0.0175f, 0.0175f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("WardOnLevel", "DisplayWarbanner", "Pelvis", new Vector3(-0.01f, 0, 0), new Vector3(0, 90, 90), new Vector3(0.0175f, 0.0175f, 0.0175f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("BarrierOnOverHeal", "DisplayAegis", "ElbowR", new Vector3(-0.002f, -0.005f, 0), new Vector3(90, 90, 0), new Vector3(0.01f, 0.01f, 0.01f)));
            //itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("WarCryOnMultiKill", "DisplayPauldron", "ClavicleR", new Vector3(0, 0.005f, 0), new Vector3(270, 90, 0), new Vector3(0.0175f, 0.0175f, 0.0175f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("SprintArmor", "DisplayBuckler", "ElbowL", new Vector3(0.002f, 0.005f, 0), new Vector3(0, 90, 0), new Vector3(0.01f, 0.01f, 0.01f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("IceRing", "DisplayIceRing", "HandL", new Vector3(0, 0.002f, 0), new Vector3(270, 90, 0), new Vector3(0.02f, 0.0175f, 0.0175f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("FireRing", "DisplayFireRing", "HandR", new Vector3(0, 0.002f, 0), new Vector3(270, 90, 0), new Vector3(0.02f, 0.0175f, 0.0175f)));

            equipmentRules.Add(NemItemDisplays.CreateGenericDisplayRule("Jetpack", "DisplayBugWings", "Chest", new Vector3(0.008f, 0.008f, 0), new Vector3(0, 270, 0), new Vector3(0.005f, 0.005f, 0.005f)));
            equipmentRules.Add(NemItemDisplays.CreateGenericDisplayRule("GoldGat", "DisplayGoldGat", "Chest", new Vector3(0.003f, 0.007f, 0), new Vector3(0, 0, 0), new Vector3(0.002f, 0.002f, 0.002f)));
            equipmentRules.Add(NemItemDisplays.CreateGenericDisplayRule("BFG", "DisplayBFG", "Chest", new Vector3(0, 0.012f, -0.006f), new Vector3(15, 270, 25), new Vector3(0.01f, 0.01f, 0.01f)));
            equipmentRules.Add(NemItemDisplays.CreateGenericDisplayRule("QuestVolatileBattery", "DisplayBatteryArray", "Chest", new Vector3(0.012f, 0.012f, 0), new Vector3(315, 90, 0), new Vector3(0.01f, 0.01f, 0.01f)));

            itemRules.Add(NemItemDisplays.CreateFollowerDisplayRule("Icicle", "DisplayFrostRelic", new Vector3(0.05f, 0.03f, 0.04f), new Vector3(0, 0, 90), new Vector3(2, 2, 2)));
            itemRules.Add(NemItemDisplays.CreateFollowerDisplayRule("Talisman", "DisplayTalisman", new Vector3(-0.05f, 0.03f, 0.05f), new Vector3(0, 270, 0), new Vector3(1, 1, 1)));
            itemRules.Add(NemItemDisplays.CreateFollowerDisplayRule("FocusConvergence", "DisplayFocusedConvergence", new Vector3(0.05f, 0.01f, 0.03f), new Vector3(0, 0, 0), new Vector3(0.2f, 0.2f, 0.2f)));

            equipmentRules.Add(NemItemDisplays.CreateFollowerDisplayRule("Saw", "DisplaySawmerang", new Vector3(0.05f, 0.02f, 0), new Vector3(0, 0, 0), new Vector3(0.5f, 0.5f, 0.5f)));

            #endregion

            BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            ItemDisplayRuleSet.NamedRuleGroup[] item = itemRules.ToArray();
            ItemDisplayRuleSet.NamedRuleGroup[] equip = equipmentRules.ToArray();
            typeof(ItemDisplayRuleSet).GetField("namedItemRuleGroups", bindingAttr).SetValue(itemDisplayRuleSet, item);
            typeof(ItemDisplayRuleSet).GetField("namedEquipmentRuleGroups", bindingAttr).SetValue(itemDisplayRuleSet, equip);

            characterModel.itemDisplayRuleSet = itemDisplayRuleSet;
        }

        public static ItemDisplayRuleSet.NamedRuleGroup CreateGenericDisplayRule(string itemName, string prefabName, string childName, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            ItemDisplayRuleSet.NamedRuleGroup displayRule = new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = itemName,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            childName = childName,
                            followerPrefab = ItemDisplays.LoadDisplay(prefabName),
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

        public static ItemDisplayRuleSet.NamedRuleGroup CreateFollowerDisplayRule(string itemName, string prefabName, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            ItemDisplayRuleSet.NamedRuleGroup displayRule = new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = itemName,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            childName = "Base",
                            followerPrefab = ItemDisplays.LoadDisplay(prefabName),
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
