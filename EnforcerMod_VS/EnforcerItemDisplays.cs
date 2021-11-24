using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace EnforcerPlugin {
    public static class EnforcerItemDisplays {

        public static List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemRules;

        public static void RegisterDisplays() {
            GameObject bodyPrefab = EnforcerModPlugin.characterPrefab;

            GameObject model = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = model.GetComponent<CharacterModel>();

            ItemDisplayRuleSet itemDisplayRuleSet = ScriptableObject.CreateInstance<ItemDisplayRuleSet>();

            itemRules = new List<ItemDisplayRuleSet.KeyAssetRuleGroup>();

            #region Display Rules

            AddVanillaDisplays();

            #region nerd rules

            if (EnforcerPlugin.EnforcerModPlugin.supplyDropInstalled)
                AddSupplyDropDisplays();

            #endregion nerd rules

            #endregion Display Rules

            ItemDisplayRuleSet.KeyAssetRuleGroup[] item = itemRules.ToArray();
            itemDisplayRuleSet.keyAssetRuleGroups = item;

            characterModel.itemDisplayRuleSet = itemDisplayRuleSet;
        }

/*for custom copy format in keb's helper
{childName},
                                                                {localPos}, 
                                                                {localAngles},
                                                                {localScale})

{childName},
                                               {localPos}, 
                                               {localAngles},
                                               {localScale})
*/

        private static void AddVanillaDisplays() {
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("CritGlasses", "DisplayGlasses",
                                                                "Head",
                                                                new Vector3(-0.19034F, 0.07312F, 0F),
                                                                new Vector3(0F, 270F, 0F),
                                                                new Vector3(0.31811F, 0.25771F, 0.32899F)));


            itemRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules("SecondarySkillMagazine",
                ItemDisplays.CreateDisplayRule("DisplayDoubleMag",
                                               "ShotgunItems",
                                               new Vector3(0.23681F, 0.18249F, 0.02189F),
                                               new Vector3(3.15304F, 90F, 180F),
                                               new Vector3(0.09266F, 0.07507F, 0.09583F)),
                ItemDisplays.CreateDisplayRule("DisplayDoubleMag",
                                               "SSGItems",
                                               new Vector3(0.41312F, 0.16682F, 0.06131F),
                                               new Vector3(3.15304F, 90F, 180F),
                                               new Vector3(0.09266F, 0.07507F, 0.09583F)),
                ItemDisplays.CreateDisplayRule("DisplayDoubleMag",
                                               "SSGItems",
                                               new Vector3(0.41788F, 0.16017F, -0.045F),
                                               new Vector3(3.15304F, 90F, 180F),
                                               new Vector3(-0.09266F, 0.07507F, 0.09583F)),
                ItemDisplays.CreateDisplayRule("DisplayDoubleMag",
                                               "HMGItems",
                                               new Vector3(0.26451F, 0.24186F, 0.03677F),
                                               new Vector3(2.74917F, 90.11697F, 179.8363F),
                                               new Vector3(0.09875F, 0.10525F, 0.12089F)),
                ItemDisplays.CreateDisplayRule("DisplayDoubleMag",
                                               "HammerShaftFront",
                                               new Vector3(-0.05377F, 0.14861F, -0.21077F),
                                               new Vector3(270.4614F, 331.0887F, 214.0607F),
                                               new Vector3(0.11733F, 0.09338F, 0.11921F)),
                ItemDisplays.CreateDisplayRule("DisplayDoubleMag",
                                               "NeedlerItems",
                                               new Vector3(0.25984F, 0.33422F, 0.03651F),
                                               new Vector3(42.9152F, 89.97276F, 179.7767F),
                                               new Vector3(0.09875F, 0.10525F, 0.12089F))
                ));
        }

        #region HEE HEE HEE
        //[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static void AddSupplyDropDisplays() {
            itemRules.Add(ItemDisplays.CreateSupplyDropRuleGroup("PlagueMask",
                                                                "Head",
                                                                new Vector3(-0.28915F, 0.03836F, -0.00003F),
                                                                new Vector3(353.5286F, 90F, 0F),
                                                                new Vector3(0.26163F, 0.23819F, 0.26633F)));
            itemRules.Add(ItemDisplays.CreateSupplyDropRuleGroup("PlagueHat",
                                                                "Head",
                                                                new Vector3(-0.00003F, 0.42981F, -0.00005F),
                                                                new Vector3(0F, 0F, 0F),
                                                                new Vector3(0.164F, 1.06472F, 0.189F)));

            //itemDisplayRules.Add(CreateSupplyDropRuleGroup("Bones",
            //                                               "CalfR",
            //                                               new Vector3(0.17997F, -0.05238F, 0.07133F),
            //                                               new Vector3(13.68323F, 76.44486F, 191.9287F),
            //                                               new Vector3(1.25683F, 1.25683F, 1.25683F)));
            //itemDisplayRules.Add(CreateSupplyDropRuleGroup("Berries",
            //                                               "loinFront2",
            //                                               new Vector3(0.11782F, 0.27382F, -0.13743F),
            //                                               new Vector3(341.1884F, 284.1298F, 180.0032F),
            //                                               new Vector3(0.08647F, 0.08647F, 0.08647F)));
            //itemDisplayRules.Add(CreateSupplyDropRuleGroup("UnassumingTie",
            //                                               "Chest",
            //                                               new Vector3(-0.08132F, 0.30226F, 0.34786F),
            //                                               new Vector3(351.786F, 356.4574F, 0.73319F),
            //                                               new Vector3(0.32213F, 0.35018F, 0.42534F)));
            //itemDisplayRules.Add(CreateSupplyDropRuleGroup("SalvagedWires",
            //                                               "UpperArmL",
            //                                               new Vector3(-0.00419F, 0.10839F, -0.20693F),
            //                                               new Vector3(21.68419F, 165.3445F, 132.0565F),
            //                                               new Vector3(0.63809F, 0.63809F, 0.63809F)));

            //itemDisplayRules.Add(CreateSupplyDropRuleGroup("ShellPlating",
            //                                               "ThighR",
            //                                               new Vector3(0.02115F, 0.52149F, -0.31269F),
            //                                               new Vector3(319.6181F, 168.4007F, 184.779F),
            //                                               new Vector3(0.24302F, 0.26871F, 0.26871F)));
            //itemDisplayRules.Add(CreateSupplyDropRuleGroup("ElectroPlankton",
            //                                               "ThighR",
            //                                               new Vector3(0.21067F, 0.49094F, -0.08702F),
            //                                               new Vector3(8.08377F, 285.087F, 164.4582F),
            //                                               new Vector3(0.11243F, 0.11243F, 0.11243F)));

            //itemDisplayRules.Add(CreateSupplyDropRuleGroup("BloodBook",
            //                                               "Root",
            //                                               new Vector3(2.19845F, -1.51445F, 1.59871F),
            //                                               new Vector3(303.5005F, 271.0879F, 269.2205F),
            //                                               new Vector3(0.12F, 0.12F, 0.12F)));
            //itemDisplayRules.Add(CreateSupplyDropRuleGroup("QSGen",
            //                                               "LowerArmL",
            //                                               new Vector3(0.06003F, 0.1038F, -0.02042F),
            //                                               new Vector3(0F, 18.75576F, 268.4665F),
            //                                               new Vector3(0.12301F, 0.12301F, 0.12301F)));
        }
        #endregion

    }
}