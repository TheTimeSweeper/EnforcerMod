using EnforcerPlugin.Modules;
using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace EnforcerPlugin
{
    public static class EnforcerItemDisplays {

        public static List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemRules;

        //public static List<ItemDisplayRuleSet.KeyAssetRuleGroup> ShieldRules;
        //public static List<ItemDisplayRuleSet.KeyAssetRuleGroup> EnergyShieldRules;
        //public static List<ItemDisplayRuleSet.KeyAssetRuleGroup> SkateRules;

        //public static List<ItemDisplayRuleSet.KeyAssetRuleGroup> RobotWeaponRules
        //public static List<ItemDisplayRuleSet.KeyAssetRuleGroup> SomeOtherSkinSpecificWeaponRules;

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

            //R2API.ItemAPI.letmeoutLETMEOUT("EnforcerBody");
        }

        private static void AddVanillaDisplays() {


            /*for custom copy format in keb's helper
            {childName},
                                                                            {localPos}, 
                                                                            {localAngles},
                                                                            {localScale})
                                                                                         // for some reason idph can only paste one ) at the end
            */
            #region Examples
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("CritGlasses", "DisplayGlasses",
                                                                "Head",
                                                                new Vector3(-0.17211F, 0.16706F, -0.00581F),
                                                                new Vector3(331.284F, 267.9785F, 1.98863F),
                                                                new Vector3(0.31811F, 0.25771F, 0.32899F)));

            itemRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules("SecondarySkillMagazine",
                ItemDisplays.CreateDisplayRule("DisplayDoubleMag",
                                               "ShotgunItems", //make sure you use "ShotgunItems" instead of "Gun" bone
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
            //press number keys to show/hide weapons (and their displays)
            //you don't have to do this. just do the default shotgun I'll do the rest
            //if you do want to i'll kiss ya
                //just don't skimp out on putting things on weapons because doing them all would be too much
                
/*for items with multiple displays (with CreateDisplayRuleGroupWithRules):
{childName},
                                               {localPos}, 
                                               {localAngles},
                                               {localScale})
*/
            #endregion

            #region items
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("AlienHead", "DisplayAlienHead",
                                                                "Pelvis",
																new Vector3(-0.19125F, 0.05712F, -0.3345F),
																new Vector3(85.75905F, 17.91318F, 314.4479F),
																new Vector3(0.3F, 0.3F, 0.3F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ArmorPlate", "DisplayRepulsionArmorPlate",
                                                                "LowerArmR",
                                                                new Vector3(-0.04437F, 0.15214F, 0.03719F),
                                                                new Vector3(85.53775F, 207.959F, 117.8662F),
                                                                new Vector3(0.42477F, 0.42477F, 0.34069F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ArmorReductionOnHit", "DisplayWarhammer",
                                                                "Chest",
																new Vector3(0.39152F, -0.05939F, 0.06301F),
																new Vector3(63.61816F, 17.79318F, 110.8655F),
																new Vector3(0.2175F, 0.2175F, 0.2175F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("AttackSpeedOnCrit", "DisplayWolfPelt",
                                                                "Head", 
																new Vector3(-0.08991F, 0.15139F, 0F), 
																new Vector3(0F, 270F, 0F), 
																new Vector3(0.75F, 0.75F, 0.75F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("AutoCastEquipment", "DisplayFossil",
                                                                "Pelvis",
																new Vector3(-0.25218F, 0.18055F, 0.18496F),
																new Vector3(356.0033F, 25.07749F, 12.97394F),
																new Vector3(0.62803F, 0.62803F, 0.62803F)));

            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Bandolier", "DisplayBandolier",
                                                                "Chest",
																new Vector3(0.05698F, 0.20317F, 0.00153F),
																new Vector3(317.5566F, 193.7607F, 254.3369F),
																new Vector3(-0.92139F, -1.2309F, -1.66832F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("BarrierOnKill", "DisplayBrooch",
                                                                "Chest",
                                                                new Vector3(-0.26885F, 0.18017F, -0.14314F), 
                                                                new Vector3(87.77314F, 257.9498F, 347.8895F),
                                                                new Vector3(0.72285F, 0.61958F, 0.61958F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("BarrierOnOverHeal", "DisplayAegis",
                                                                "ThighL",
																new Vector3(-0.15103F, 0.3551F, 0.01698F),
																new Vector3(276.4102F, 84.83101F, 192.4375F),
																new Vector3(0.15F, 0.15F, 0.15F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Bear", "DisplayBear",
                                                                "Shield",
                                                                new Vector3(0.33149F, -0.18085F, 0.07887F),
                                                                new Vector3(7.01418F, 125.5218F, 351.2383F),
                                                                new Vector3(0.58719F, 0.58719F, 0.58719F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("BeetleGland", "DisplayBeetleGland",
                                                                "Pelvis",
																new Vector3(-0.022F, 0.00488F, -0.30258F),
																new Vector3(346.8375F, 239.4366F, 115.7948F),
																new Vector3(0.12407F, 0.12407F, 0.12407F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Behemoth", "DisplayBehemoth",
                                                                "ShotgunItems",
                                                                new Vector3(0.40627F, -0.31923F, -0.00008F),
                                                                new Vector3(90F, 90F, 0F),
                                                                new Vector3(0.11367F, 0.11367F, 0.11367F)));
            //again, don't have to do this 
            itemRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules("BleedOnHit",
                ItemDisplays.CreateDisplayRule("DisplayTriTip",
                                               "ShotgunItems",
                                               new Vector3(1.013F, 0.00319F, -0.04901F),
                                               new Vector3(0F, 90F, 0F),
                                               new Vector3(0.46347F, 0.46347F, 0.472F)),
                ItemDisplays.CreateDisplayRule("DisplayTriTip",
                                               "SSGItems",
                                               new Vector3(1.38595F, -0.03617F, 0.10774F),
                                               new Vector3(0F, 90F, 0F),
                                               new Vector3(0.39475F, 0.39475F, 0.47606F)),
                ItemDisplays.CreateDisplayRule("DisplayTriTip",
                                               "SSGItems",
                                               new Vector3(1.37468F, -0.03616F, -0.07445F),
                                               new Vector3(0F, 90F, 0F),
                                               new Vector3(0.39475F, 0.39475F, 0.47606F)),
                ItemDisplays.CreateDisplayRule("DisplayTriTip",
                                               "HMGItems",
                                               new Vector3(1.29632F, -0.03511F, -0.03289F),
                                               new Vector3(0F, 90F, 34.91743F),
                                               new Vector3(0.33681F, 0.33681F, 0.40163F)),
                ItemDisplays.CreateDisplayRule("DisplayTriTip",
                                               "HammerHeadFront",
                                               new Vector3(0.00006F, 0.21462F, -0.15747F),
                                               new Vector3(0F, 180F, 0F),
                                               new Vector3(0.78265F, 0.78265F, 0.8231F)),
                ItemDisplays.CreateDisplayRule("DisplayTriTip",
                                               "NeedlerItems",
                                               new Vector3(0.37574F, 0.05803F, -0.01585F),
                                               new Vector3(357.3287F, 92.62193F, 1.78209F),
                                               new Vector3(0.51745F, 0.47049F, 0.75514F))
                ));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("BleedOnHitAndExplode", "DisplayBleedOnHitAndExplode",
                                                                "Pelvis",
																new Vector3(-0.14081F, -0.03862F, -0.29823F),
																new Vector3(49.15022F, 348.3428F, 147.9455F),
																new Vector3(0.05F, 0.05F, 0.05F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("BonusGoldPackOnKill", "DisplayTome",
                                                                "ThighR",
																new Vector3(0.04686F, 0.15891F, 0.20308F),
																new Vector3(4.24283F, 9.4013F, 7.73295F),
																new Vector3(0.1F, 0.1F, 0.1F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("BossDamageBonus", "DisplayAPRound",
                                                                "PauldronR",
																new Vector3(0.0797F, 0.33419F, -0.12632F),
																new Vector3(349.9278F, 178.5847F, 179.3943F),
																new Vector3(0.5F, 0.5F, 0.5F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("BounceNearby", "DisplayHook",
                                                                "Chest",
																new Vector3(0.21642F, 0.42281F, 0.02163F),
																new Vector3(317.5101F, 207.0038F, 320.5225F),
																new Vector3(0.5F, 0.5F, 0.5F)));

            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ChainLightning", "DisplayUkulele",
                                                                "Chest",
																new Vector3(0.38798F, 0.05533F, 0.08688F),
																new Vector3(355.6444F, 89.65604F, 16.94394F),
																new Vector3(1F, 1F, 1F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Clover", "DisplayClover",
                                                                "Chest",
																new Vector3(0.10652F, 0.55431F, 0.15312F),
																new Vector3(295.1103F, 111.8469F, 103.4613F),
																new Vector3(0.3F, 0.3F, 0.3F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("CooldownOnCrit", "DisplaySkull",
                                                                "PauldronR",
																new Vector3(0.04113F, 0.12075F, 0.17865F),
																new Vector3(8.33477F, 0.20208F, 183.0216F),
																new Vector3(0.285F, 0.285F, 0.21F)));
                                                              //CritGlasses: see example above
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Crowbar", "DisplayCrowbar",
                                                                "Shield",
																new Vector3(0.12908F, -0.17119F, -0.13849F),
																new Vector3(358.9909F, 305.6292F, 8.64924F),
																new Vector3(0.5F, 0.5F, 0.5F)));

            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Dagger", "DisplayDagger",
                                                                "PauldronR",
																new Vector3(-0.12813F, 0.0532F, -0.0086F),
																new Vector3(353.1795F, 355.2217F, 80.39191F),
																new Vector3(0.75F, 0.75F, 0.75F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("DeathMark", "DisplayDeathMark",
                                                                "HandR",
                                                                new Vector3(0, 0, 0),
                                                                new Vector3(270, 90, 0),
                                                                new Vector3(0.02f, 0.02f, 0.02f)));

            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("EnergizedOnEquipmentUse", "DisplayWarHorn",
                                                                "Pelvis",
																new Vector3(-0.09076F, -0.00443F, 0.36714F),
																new Vector3(40.89693F, 353.5466F, 165.9615F),
																new Vector3(0.4F, 0.4F, 0.4F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("EquipmentMagazine", "DisplayBattery",
                                                                "Shield",
																new Vector3(0.12485F, -0.17919F, 0.14206F),
																new Vector3(13.85731F, 31.02917F, 18.90965F),
																new Vector3(0.21F, 0.21F, 0.21F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ExecuteLowHealthElite", "DisplayGuillotine",
                                                                "Shield",
																new Vector3(0.42079F, -0.67409F, 0.51216F),
																new Vector3(350.7767F, 92.86279F, 145.5876F),
																new Vector3(0.22F, 0.22F, 0.2175F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ExplodeOnDeath", "DisplayWilloWisp",
                                                                "Pelvis",
																new Vector3(-0.21115F, 0.12512F, -0.171F),
																new Vector3(356.8601F, 344.8517F, 175.7498F),
																new Vector3(0.1F, 0.1F, 0.1F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ExtraLife", "DisplayHippo",
                                                                "PauldronL",
																new Vector3(0.05016F, 0.07421F, 0.19457F),
																new Vector3(328.1188F, 30.00192F, 150.7529F),
																new Vector3(0.22F, 0.22F, 0.22F)));
            //hello
            itemRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules("FallBoots",
                ItemDisplays.CreateDisplayRule("DisplayGravBoots",
                                               "CalfR",
                                               new Vector3(-0.00711F, 0.20184F, -0.14286F),
                                               new Vector3(1.0034F, 84.44292F, 81.36586F),
                                               new Vector3(-0.10227F, 0.06145F, 0.06017F)),
                ItemDisplays.CreateDisplayRule("DisplayGravBoots",
                                               "CalfL",
                                               new Vector3(-0.04985F, -0.11777F, -0.05759F),
                                               new Vector3(357.4058F, 62.00602F, 40.11972F),
                                               new Vector3(0.09645F, 0.06145F, 0.05076F)
                )));
            #region lol
            itemRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules("Feather",
                ItemDisplays.CreateDisplayRule("DisplayFeather",
                                               "PauldronR",
                                               new Vector3(-0.00711F, 0.20184F, -0.14286F),
                                               new Vector3(1.0034F, 84.44292F, 81.36586F),
                                               new Vector3(-0.10227F, 0.06145F, 0.06017F))//,
                //ItemDisplays.CreateDisplayRule("DisplayFeather",
                //                               "PauldronR",
                //                               new Vector3(-0.04985F, -0.11777F, -0.05759F),
                //                               new Vector3(357.4058F, 62.00602F, 40.11972F),
                //                               new Vector3(0.09645F, 0.06145F, 0.05076F)),
                //ItemDisplays.CreateDisplayRule("DisplayFeather",
                //                              "UpperArmR",
                //                               new Vector3(-0.13234F, -0.00069F, -0.0343F),
                //                               new Vector3(353.4506F, 15.61529F, 111.0878F),
                //                               new Vector3(0.11228F, 0.04899F, 0.05685F)),
                //ItemDisplays.CreateDisplayRule("DisplayFeather",
                //                              "UpperArmR",
                //                               new Vector3(-0.07421F, 0.11864F, -0.06196F),
                //                               new Vector3(359.0367F, 28.50466F, 86.69939F),
                //                               new Vector3(0.11228F, 0.04899F, 0.03156F)),
                //ItemDisplays.CreateDisplayRule("DisplayFeather",
                //                               "UpperArmR",
                //                               new Vector3(-0.0479F, 0.40772F, -0.00297F),
                //                               new Vector3(4.83634F, 8.2752F, 93.10972F),
                //                               new Vector3(-0.11404F, 0.04899F, 0.03014F)),
                //ItemDisplays.CreateDisplayRule("DisplayFeather",
                //                               "UpperArmR",
                //                               new Vector3(-0.02645F, 0.08531F, -0.10601F),
                //                               new Vector3(0.08396F, 52.42583F, 105.85F),
                //                               new Vector3(0.08507F, 0.03232F, 0.02821F)),
                //ItemDisplays.CreateDisplayRule("DisplayFeather",
                //                               "LowerArmR",
                //                               new Vector3(0.0304F, 0.24275F, -0.01461F),
                //                               new Vector3(1.12253F, 7.62863F, 77.25879F),
                //                               new Vector3(-0.12035F, 0.05215F, 0.02586F)),
                //ItemDisplays.CreateDisplayRule("DisplayFeather",
                //                               "LowerArmR",
                //                               new Vector3(0.06852F, 0.30352F, -0.0059F),
                //                               new Vector3(19.46174F, 38.02186F, 96.31871F),
                //                               new Vector3(-0.09859F, 0.0325F, 0.02783F)),
                //ItemDisplays.CreateDisplayRule("DisplayFeather",
                //                               "LowerArmR",
                //                               new Vector3(0.04444F, 0.08465F, -0.04641F),
                //                               new Vector3(11.52688F, 33.65718F, 92.9191F),
                //                               new Vector3(0.07482F, 0.02964F, 0.02783F)),
                //ItemDisplays.CreateDisplayRule("DisplayFeather",
                //                              "HandR",
                //                               new Vector3(0.07046F, 0.01094F, -0.00949F),
                //                               new Vector3(4.37529F, 354.2159F, 5.53738F),
                //                               new Vector3(-0.05597F, 0.02184F, 0.03635F)),

                //ItemDisplays.CreateDisplayRule("DisplayFeather",
                //                               "PauldronL",
                //                               new Vector3(-0.01903F, 0.17235F, -0.19982F),
                //                               new Vector3(2.7919F, 98.53754F, 73.55167F),
                //                               new Vector3(-0.10227F, 0.06145F, -0.05685F)),
                //ItemDisplays.CreateDisplayRule("DisplayFeather",
                //                               "PauldronL",
                //                               new Vector3(-0.03674F, -0.04291F, -0.02275F),
                //                               new Vector3(29.02424F, 116.5888F, 41.27706F),
                //                               new Vector3(0.09645F, 0.06145F, -0.05428F)),
                //ItemDisplays.CreateDisplayRule("DisplayFeather",
                //                               "UpperArmL",
                //                               new Vector3(0.0893F, 0.0903F, 0.00256F),
                //                               new Vector3(7.95194F, 167.1458F, 108.7899F),
                //                               new Vector3(0.11228F, 0.04899F, -0.03156F)),
                //ItemDisplays.CreateDisplayRule("DisplayFeather",
                //                               "UpperArmL",
                //                               new Vector3(-0.02306F, 0.12439F, -0.06882F),
                //                               new Vector3(354.9163F, 189.6785F, 91.23473F),
                //                               new Vector3(0.11228F, 0.04899F, -0.03014F)),
                //ItemDisplays.CreateDisplayRule("DisplayFeather",
                //                               "UpperArmL",
                //                               new Vector3(-0.01301F, 0.4053F, -0.03388F),
                //                               new Vector3(4.96039F, 182.6734F, 28.84227F),
                //                               new Vector3(-0.11404F, 0.04899F, -0.02821F)),
                //ItemDisplays.CreateDisplayRule("DisplayFeather",
                //                               "UpperArmL",
                //                               new Vector3(0.02878F, 0.2051F, -0.05549F),
                //                               new Vector3(2.03596F, 134.2346F, 71.33317F),
                //                               new Vector3(-0.0802F, 0.02707F, -0.02544F)),
                //ItemDisplays.CreateDisplayRule("DisplayFeather",
                //                               "LowerArmL",
                //                               new Vector3(-0.01322F, 0.22517F, -0.09324F),
                //                               new Vector3(21.78609F, 201.5365F, 92.93464F),
                //                               new Vector3(-0.12035F, 0.05215F, -0.02675F)),
                //ItemDisplays.CreateDisplayRule("DisplayFeather",
                //                               "LowerArmL",
                //                               new Vector3(-0.01713F, 0.24116F, -0.06643F),
                //                               new Vector3(13.29518F, 173.3967F, 92.58321F),
                //                               new Vector3(-0.06379F, 0.02638F, -0.02783F)),
                //ItemDisplays.CreateDisplayRule("DisplayFeather",
                //                               "LowerArmL",
                //                               new Vector3(-0.0634F, 0.01212F, 0.03855F),
                //                               new Vector3(22.08632F, 179.1511F, 94.84153F),
                //                               new Vector3(0.0671F, 0.02888F, -0.02783F)),
                //ItemDisplays.CreateDisplayRule("DisplayFeather",
                //                               "HandL",
                //                               new Vector3(-0.03535F, -0.02455F, -0.05199F),
                //                               new Vector3(4.46654F, 353.2656F, 5.46412F),
                //                               new Vector3(0.05597F, 0.02184F, 0.03635F)),
                //ItemDisplays.CreateLimbMaskDisplayRule(LimbFlags.RightArm),
                //ItemDisplays.CreateLimbMaskDisplayRule(LimbFlags.LeftArm)
                ));
            #endregion lol
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("FireballsOnHit", "DisplayFireballsOnHit",
                                                                "Head",
                                                                new Vector3(-0.15001F, 0.20035F, -0.0028F),
                                                                new Vector3(318.8332F, 268.2494F, 176.8457F),
                                                                new Vector3(0.1F, 0.1F, 0.1F)));
            //gunn
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("FireRing", "DisplayFireRing",
                                                                "Gun",
                                                                new Vector3(-0.05634F, 0.08417F, 0.03567F),
                                                                new Vector3(2.12931F, 256.7651F, 175.3758F),
                                                                new Vector3(1F, 1F, 1F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Firework", "DisplayFirework",
                                                                "Pelvis",
                                                                new Vector3(0.18761F, 0.03323F, 0.29922F),
                                                                new Vector3(25.02743F, 64.14127F, 78.41231F),
                                                                new Vector3(0.3F, 0.3F, 0.3F)));
            //hello
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("FlatHealth", "DisplaySteakCurved",
                                                                "Chest",
                                                                new Vector3(0, 0.215f, 0.215f),
                                                                new Vector3(335, 0, 180),
                                                                new Vector3(0.275f, 0.275f, 0.275f)));

            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("GhostOnKill", "DisplayMask",
                                                                "Head",
                                                                new Vector3(-0.09385F, 0.08206F, -0.00262F),
                                                                new Vector3(0F, 270F, 0F),
                                                                new Vector3(0.6F, 0.6F, 0.5F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("GoldOnHit", "DisplayBoneCrown",
                                                                "Head",
                                                                new Vector3(-0.01105F, 0.16442F, 0F),
                                                                new Vector3(0F, 270F, 0F),
                                                                new Vector3(1F, 1F, 1F)));

            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("HeadHunter", "DisplaySkullCrown",
                                                                "Head",
                                                                new Vector3(0.00841F, 0.15581F, -0.01431F),
                                                                new Vector3(0F, 272.3771F, 0F),
                                                                new Vector3(0.215F, 0.25F, 0.25F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("HealOnCrit", "DisplayScythe",
                                                                "Chest",
                                                                new Vector3(-0.27846F, 0.20935F, -0.07484F),
                                                                new Vector3(315.5308F, 156.2412F, 116.7797F),
                                                                new Vector3(0.2F, 0.2F, 0.2F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("HealWhileSafe", "DisplaySnail",
                                                                "Chest",
                                                                new Vector3(0.06413F, 0.36924F, -0.23938F),
                                                                new Vector3(340.8154F, 311.9604F, 3.12292F),
                                                                new Vector3(0.1F, 0.1F, 0.1F)));

            itemRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules("Hoof",
                ItemDisplays.CreateDisplayRule("DisplayHoof",
                                               "CalfR",
                                               new Vector3(0.05902F, 0.37682F, -0.01856F),
                                               new Vector3(73.35873F, 286.4382F, 7.98166F),
                                               new Vector3(0.12953F, 0.12351F, 0.10424F)),
                ItemDisplays.CreateLimbMaskDisplayRule(LimbFlags.RightCalf)
                ));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("IceRing", "DisplayIceRing",
                                                                "Gun",
                                                                new Vector3(-0.1295F, 0.08429F, 0.01944F),
                                                                new Vector3(0.35901F, 257.3686F, 175.9728F),
                                                                new Vector3(1F, 1F, 1F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("IgniteOnKill", "DisplayGasoline",
                                                               "ThighL",
                                                                new Vector3(0.06183F, 0.41258F, 0.15294F),
                                                                new Vector3(77.23817F, 252.9002F, 163.0499F),
                                                                new Vector3(0.6F, 0.6F, 0.6F)));
            //hello
            itemRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules("IncreaseHealing",
                ItemDisplays.CreateDisplayRule("DisplayAntler",
                                               "Head",
                                               new Vector3(0.05902F, 0.37682F, -0.01856F),
                                               new Vector3(73.35873F, 286.4382F, 7.98166F),
                                               new Vector3(0.12953F, 0.12351F, 0.10424F)),
                ItemDisplays.CreateDisplayRule("DisplayAntler",
                                               "Head",
                                               new Vector3(0.05902F, 0.37682F, -0.01856F),
                                               new Vector3(73.35873F, 286.4382F, 7.98166F),
                                               new Vector3(0.12953F, 0.12351F, 0.10424F))
                ));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Incubator", "DisplayAncestralIncubator",
                                                                "Chest",
                                                                new Vector3(0.0175F, 0.27074F, -0.2402F),
                                                                new Vector3(9.51898F, 20.83393F, 340.9285F),
                                                                new Vector3(0.05F, 0.05F, 0.05F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Infusion", "DisplayInfusion",
                                                                "UpperArmR",
                                                                new Vector3(-0.00565F, 0.25692F, 0.09434F),
                                                                new Vector3(353.1847F, 167.6059F, 191.5657F),
                                                                new Vector3(0.22F, 0.22F, 0.22F)));

            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("JumpBoost", "DisplayWaxBird",
                                                                "Head",
                                                                new Vector3(0.08598F, -0.40624F, 0F),
                                                                new Vector3(0F, 270F, 0F),
                                                                new Vector3(1F, 1F, 1F)));

            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("KillEliteFrenzy", "DisplayBrainstalk",
                                                                "Head",
                                                                new Vector3(0.00137F, 0.00549F, -0.01466F),
                                                                new Vector3(0F, 90F, 0F),
                                                                new Vector3(0.3F, 0.3F, 0.3F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Knurl", "DisplayKnurl",
                                                                "Head",
                                                                new Vector3(-0.02153F, -0.02843F, -0.04966F),
                                                                new Vector3(80.59711F, 111.1607F, 94.96837F),
                                                                new Vector3(0.05F, 0.05F, 0.05F)));

            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("LaserTurbine", "DisplayLaserTurbine",
                                                                "Chest",
                                                                new Vector3(0.20597F, 0.32601F, 0.00001F),
                                                                new Vector3(0F, 90F, 0F),
                                                                new Vector3(0.215F, 0.215F, 0.215F)));
            //hello
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("LightningStrikeOnHit", "DisplayChargedPerforator",
                                                                "Chest",
                                                                new Vector3(0, 0.22f, 0.21f),
                                                                new Vector3(0, 0, 0),
                                                                new Vector3(0.25f, 0.25f, 0.25f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("LunarDagger", "DisplayLunarDagger",
                                                                "Chest",
                                                                new Vector3(0.44847F, 0.46014F, -0.17672F),
                                                                new Vector3(36.96157F, 337.9217F, 59.63353F),
                                                                new Vector3(0.5F, 0.5F, 0.5F)));
            //uh
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("LunarPrimaryReplacement", "DisplayBirdEye",
                                                                "Chest",
                                                                new Vector3(-0.203f, 0.2025f, 0),
                                                                new Vector3(0, 0, 270),
                                                                new Vector3(0.21f, 0.21f, 0.21f)));
            //hello
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("LunarSecondaryReplacement", "DisplayBirdClaw",
                                                                "Chest",
                                                                new Vector3(-0.203f, 0.2025f, 0),
                                                                new Vector3(0, 0, 270),
                                                                new Vector3(0.21f, 0.21f, 0.21f)));
            //hello
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("LunarSpecialReplacement", "DisplayBirdHeart",
                                                                "Base",
                                                                new Vector3(-0.203f, 0.2025f, 0),
                                                                new Vector3(0, 0, 270),
                                                                new Vector3(0.21f, 0.21f, 0.21f)));
            //uh
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("LunarTrinket", "DisplayBeads",
                                                                "Chest",
                                                                new Vector3(0.202f, 0.204f, 0.2015f),
                                                                new Vector3(0, 90, 90),
                                                                new Vector3(0.24f, 0.24f, 0.24f)));
            //uh
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("LunarUtilityReplacement", "DisplayBirdFoot",
                                                                "Chest",
                                                                new Vector3(0.208f, 0.208f, 0),
                                                                new Vector3(0, 180, 0),
                                                                new Vector3(0.22f, 0.22f, 0.22f)));
            
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Medkit", "DisplayMedkit",
                                                                "Pelvis",
																new Vector3(0.20947F, -0.34134F, 0.0407F),
																new Vector3(300F, 89.99994F, 359.0907F),
																new Vector3(0.6F, 0.6F, 0.6F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Missile", "DisplayMissileLauncher",
                                                                "Chest",
																new Vector3(-0.03963F, 0.93601F, 0.30045F),
																new Vector3(0F, 270F, 0F),
																new Vector3(0.15F, 0.15F, 0.15F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("MonstersOnShrineUse", "DisplayMonstersOnShrineUse",
                                                                "CalfR",
																new Vector3(0.13579F, 0.08768F, 0.03325F),
																new Vector3(313.4106F, 1.43869F, 179.9522F),
																new Vector3(0.1F, 0.1F, 0.1F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Mushroom", "DisplayMushroom",
                                                                "Chest",
																new Vector3(-0.01544F, 0.41609F, -0.31166F),
																new Vector3(23.41035F, 261.0226F, 346.6656F),
																new Vector3(0.1F, 0.1F, 0.1F)));

            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("NearbyDamageBonus", "DisplayDiamond",
                                                                "Shield",
                                                                new Vector3(0.18143F, 0.06812F, 0.2154F),
                                                                new Vector3(0F, 302.551F, 0F),
                                                                new Vector3(0.2F, 0.2F, 0.2F)));
            //hello
            itemRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules("NovaOnHeal",
                ItemDisplays.CreateDisplayRule("DisplayDevilHorns",
                                               "Head",
                                               new Vector3(0.05902F, 0.37682F, -0.01856F),
                                               new Vector3(73.35873F, 286.4382F, 7.98166F),
                                               new Vector3(0.12953F, 0.12351F, 0.10424F)),
                ItemDisplays.CreateDisplayRule("DisplayDevilHorns",
                                               "Head",
                                               new Vector3(0.05902F, 0.37682F, -0.01856F),
                                               new Vector3(73.35873F, 286.4382F, 7.98166F),
                                               new Vector3(0.12953F, 0.12351F, -0.10424F))
                ));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("NovaOnLowHealth", "DisplayJellyGuts",
                                                                "Chest",
                                                                new Vector3(0.18488F, 0.56742F, -0.01227F),
                                                                new Vector3(49.21984F, 292.0475F, 175.16F),
                                                                new Vector3(0.25F, 0.25F, 0.25F)));

            //hello
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ParentEgg", "DisplayParentEgg",
                                                                "Chest",
                                                                new Vector3(0, 0, 0),
                                                                new Vector3(0, 0, 0),
                                                                new Vector3(0.205f, 0.25f, 0.25f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Pearl", "DisplayPearl",
                                                                "Head",
                                                                new Vector3(0.05439F, 0.26667F, -0.02176F),
                                                                new Vector3(82.90738F, 293.1861F, 319.5147F),
                                                                new Vector3(0.2F, 0.2F, 0.2F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("PersonalShield", "DisplayShieldGenerator",
                                                                "Chest",
                                                                new Vector3(-0.21979F, 0.20802F, 0.19508F),
                                                                new Vector3(77.34012F, 284.6326F, 184.1077F),
                                                                new Vector3(0.25F, 0.25F, 0.25F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Phasing", "DisplayStealthkit",
                                                                "ThighL",
                                                                new Vector3(-0.01651F, 0.30469F, 0.09591F),
                                                                new Vector3(77.74274F, 288.25F, 110.3669F),
                                                                new Vector3(0.5F, 0.5F, 0.5F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Plant", "DisplayInterstellarDeskPlant",
                                                                "Head",
                                                                new Vector3(0.03807F, 0.07645F, 0.00371F),
                                                                new Vector3(276.8562F, 88.96255F, 0.15102F),
                                                                new Vector3(0.22F, 0.2175F, 0.2175F)));

            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("RandomDamageZone", "DisplayRandomDamageZone",
                                                                "Chest",
                                                                new Vector3(0.4336F, 0.34415F, -0.00645F),
                                                                new Vector3(359.7298F, 271.0585F, 0.50739F),
                                                                new Vector3(0.2F, 0.2F, 0.2F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("RepeatHeal", "DisplayCorpseFlower",
                                                                "Chest",
                                                                new Vector3(-0.12734F, 0.37462F, -0.19835F),
                                                                new Vector3(356.7155F, 152.0292F, 314.6104F),
                                                                new Vector3(0.21F, 0.21F, 0.21F)));

                                                              //SecondarySkillMagazine: see example above
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Seed", "DisplaySeed",
                                                                "Pelvis",
																new Vector3(0.03883F, -0.03422F, -0.33613F),
																new Vector3(25.313F, 59.4893F, 185.8487F),
																new Vector3(0.07F, 0.07F, 0.07F)));
            //hello
            itemRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules("ShieldOnly",
                ItemDisplays.CreateDisplayRule("DisplayShieldBug",
                                               "Head",
                                               new Vector3(0.05902F, 0.37682F, -0.01856F),
                                               new Vector3(73.35873F, 286.4382F, 7.98166F),
                                               new Vector3(0.12953F, 0.12351F, 0.10424F)),
                ItemDisplays.CreateDisplayRule("DisplayShieldBug",
                                               "Head",
                                               new Vector3(0.05902F, 0.37682F, -0.01856F),
                                               new Vector3(73.35873F, 286.4382F, 7.98166F),
                                               new Vector3(0.12953F, 0.12351F, -0.10424F)) 
                ));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ShinyPearl", "DisplayShinyPearl",
                                                                "Head",
                                                                new Vector3(0.05439F, 0.26667F, -0.02176F),
                                                                new Vector3(82.90738F, 293.1861F, 319.5147F),
                                                                new Vector3(0.2F, 0.2F, 0.2F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ShockNearby", "DisplayTeslaCoil",
                                                                "Chest",
                                                                new Vector3(0.22265F, 0.45773F, -0.00707F),
                                                                new Vector3(345.9214F, 350.8688F, 313.1301F),
                                                                new Vector3(0.21F, 0.21F, 0.21F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("SiphonOnLowHealth", "DisplaySiphonOnLowHealth",
                                                                "Pelvis",
                                                                new Vector3(-0.2131F, 0.15274F, 0.2284F),
                                                                new Vector3(341.8052F, 309.3467F, 179.0716F),
                                                                new Vector3(0.1F, 0.1F, 0.1F)));
            //hello
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("SlowOnHit", "DisplayBauble",
                                                                "Pelvis",
                                                                new Vector3(-0.206f, 0.204f, 0.206f),
                                                                new Vector3(0, 315, 180),
                                                                new Vector3(0.235f, 0.235f, 0.235f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("SprintArmor", "DisplayBuckler",
                                                                "HandR",
                                                                new Vector3(0.00645F, 0.03329F, 0.06265F),
                                                                new Vector3(339.7635F, 316.2477F, 178.7798F),
                                                                new Vector3(0.21F, 0.21F, 0.21F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("SprintBonus", "DisplaySoda",
                                                                "Pelvis",
                                                                new Vector3(0.16407F, 0.08059F, -0.26964F),
                                                                new Vector3(284.3611F, 127.6331F, 323.4904F),
                                                                new Vector3(0.4F, 0.4F, 0.4F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("SprintOutOfCombat", "DisplayWhip",
                                                                "Pelvis",
                                                                new Vector3(0F, 0.03903F, 0.41504F),
                                                                new Vector3(0F, 90F, 200F),
                                                                new Vector3(0.2175F, 0.2175F, 0.2175F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("SprintWisp", "DisplayBrokenMask",
                                                                "UpperArmR",
                                                                new Vector3(-0.02311F, 0.20304F, 0.09964F),
                                                                new Vector3(335.4409F, 2.64948F, 180F),
                                                                new Vector3(0.21F, 0.21F, 0.21F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Squid", "DisplaySquidTurret",
                                                                "Pelvis",
                                                                new Vector3(-0.19058F, 0.11929F, -0.12678F),
                                                                new Vector3(6.47033F, 168.8895F, 289.4581F),
                                                                new Vector3(0.05F, 0.05F, 0.05F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("StickyBomb", "DisplayStickyBomb",
                                                                "Pelvis",
                                                                new Vector3(0.2025f, 0.202f, -0.208f),
                                                                new Vector3(345, 15, 0),
                                                                new Vector3(0.21f, 0.21f, 0.21f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("StunChanceOnHit", "DisplayStunGrenade",
                                                                "Pelvis",
                                                                new Vector3(-0.05505F, 0.06835F, 0.41775F),
                                                                new Vector3(69.53522F, 188.4478F, 279.2932F),
                                                                new Vector3(0.7F, 0.7F, 0.7F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Syringe", "DisplaySyringeCluster",
                                                                "Chest",
                                                                new Vector3(0, 0.212f, 0.206f),
                                                                new Vector3(25, 315, 0),
                                                                new Vector3(0.25f, 0.25f, 0.25f)));

            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("TPHealingNova", "DisplayGlowFlower",
                                                                "Chest",
                                                                new Vector3(-0.21578F, 0.33212F, 0.01521F),
                                                                new Vector3(338.9304F, 272.9285F, 358.9245F),
                                                                new Vector3(0.21F, 0.21F, 0.21F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Thorns", "DisplayRazorwireLeft",
                                                                "UpperArmL",
                                                                new Vector3(0F, 0F, 0F),
                                                                new Vector3(270F, 0F, 0F),
                                                                new Vector3(0.75F, 0.75F, 0.75F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("TitanGoldDuringTP", "DisplayGoldHeart",
                                                                "Chest",
                                                                new Vector3(-0.21763F, 0.22857F, -0.18391F),
                                                                new Vector3(343.9642F, 233.3116F, 19.2015F),
                                                                new Vector3(0.285F, 0.285F, 0.285F)));
            //hello
            itemRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules("Tooth",
                ItemDisplays.CreateDisplayRule("DisplayToothNecklaceDecal",
                                               "Chest",
                                               new Vector3(0, 0.212f, 0.205f),
                                               new Vector3(290, 0, 0),
                                               new Vector3(0.23f, 0.23f, 0.23f)),
                ItemDisplays.CreateDisplayRule("DisplayToothMeshLarge",
                                               "Chest",
                                               new Vector3(0, 0.212f, 0.205f),
                                               new Vector3(290, 0, 0),
                                               new Vector3(0.23f, 0.23f, 0.23f)),
                ItemDisplays.CreateDisplayRule("DisplayToothMeshSmall1",
                                               "Chest",
                                               new Vector3(0, 0.212f, 0.205f),
                                               new Vector3(290, 0, 0),
                                               new Vector3(0.23f, 0.23f, 0.23f)),
                ItemDisplays.CreateDisplayRule("DisplayToothMeshSmall1",
                                               "Chest",
                                               new Vector3(0, 0.212f, 0.205f),
                                               new Vector3(290, 0, 0),
                                               new Vector3(0.23f, 0.23f, 0.23f)),
                ItemDisplays.CreateDisplayRule("DisplayToothMeshSmall2",
                                               "Chest",
                                               new Vector3(0, 0.212f, 0.205f),
                                               new Vector3(290, 0, 0),
                                               new Vector3(0.23f, 0.23f, 0.23f)),
                ItemDisplays.CreateDisplayRule("DisplayToothMeshSmall2",
                                               "Chest",
                                               new Vector3(0, 0.212f, 0.205f),
                                               new Vector3(290, 0, 0),
                                               new Vector3(0.23f, 0.23f, 0.23f))
                ));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("TreasureCache", "DisplayKey",
                                                                "Pelvis",
                                                                new Vector3(0.1747F, 0.1516F, -0.21049F),
                                                                new Vector3(0F, 25F, 270F),
                                                                new Vector3(0.23F, 0.23F, 0.23F)));

            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("UtilitySkillMagazine", "DisplayAfterburnerShoulderRing",
                                                                "Chest",
                                                                new Vector3(0.04056F, 0.12802F, -0.00548F),
                                                                new Vector3(79.77419F, 348.2379F, 76.76024F),
                                                                new Vector3(1F, 1F, 1F)));

            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("WarCryOnMultiKill", "DisplayPauldron",
                                                                "PauldronL",
                                                                new Vector3(-0.02849F, 0.13751F, 0.07184F),
                                                                new Vector3(44.35629F, 358.8894F, 358.1356F),
                                                                new Vector3(1F, 1F, 1F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("WardOnLevel", "DisplayWarbanner",
                                                                "Shield",
                                                                new Vector3(0.32394F, 0.06169F, 0.02166F),
                                                                new Vector3(288.6897F, 151.0441F, 245.5378F),
                                                                new Vector3(0.5F, 0.5F, 0.5F)));
            #endregion items

            #region quips
            //hello
            itemRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRulesE("AffixBlue",
                ItemDisplays.CreateDisplayRule("DisplayEliteRhinoHorn",
                                               "Head",
                                               new Vector3(0.05902F, 0.37682F, -0.01856F),
                                               new Vector3(73.35873F, 286.4382F, 7.98166F),
                                               new Vector3(0.12953F, 0.12351F, 0.10424F)),
                ItemDisplays.CreateDisplayRule("DisplayEliteRhinoHorn",
                                               "Head",
                                               new Vector3(0.05902F, 0.37682F, -0.01856F),
                                               new Vector3(73.35873F, 286.4382F, 7.98166F),
                                               new Vector3(0.12953F, 0.12351F, 0.10424F))
                ));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("AffixHaunted", "DisplayEliteStealthCrown",
                                                                "Head",
                                                                new Vector3(0F, 0.208F, 0F),
                                                                new Vector3(270F, 272.7151F, 0F),
                                                                new Vector3(0.06F, 0.06F, 0.06F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("AffixLunar", "DisplayEliteLunar,Eye",
                                                                "Head",
                                                                new Vector3(0.01343F, 0.27695F, 0.00304F),
                                                                new Vector3(270F, 0F, 0F),
                                                                new Vector3(0.28605F, 0.28605F, 0.28605F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("AffixPoison", "DisplayEliteUrchinCrown",
                                                                "Head",
                                                                new Vector3(0F, 0.14782F, 0F),
                                                                new Vector3(270F, 0F, 0F),
                                                                new Vector3(0.06F, 0.06F, 0.06F)));
            //hello
            itemRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRulesE("AffixRed",
                ItemDisplays.CreateDisplayRule("DisplayEliteHorn",
                                               "Head",
                                               new Vector3(0.05902F, 0.37682F, -0.01856F),
                                               new Vector3(73.35873F, 286.4382F, 7.98166F),
                                               new Vector3(0.12953F, 0.12351F, 0.10424F)),
                ItemDisplays.CreateDisplayRule("DisplayEliteHorn",
                                               "Head",
                                               new Vector3(0.05902F, 0.37682F, -0.01856F),
                                               new Vector3(73.35873F, 286.4382F, 7.98166F),
                                               new Vector3(0.12953F, 0.12351F, 0.10424F))
                ));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("AffixWhite", "DisplayEliteIceCrown",
                                                                "Head",
                                                                new Vector3(0F, 0.212F, 0F),
                                                                new Vector3(270F, 270F, 0F),
                                                                new Vector3(0.03F, 0.03F, 0.03F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("Jetpack", "DisplayBugWings",
                                                                "Chest",
                                                                new Vector3(0.208f, 0.208f, 0),
                                                                new Vector3(0, 270, 0),
                                                                new Vector3(0.25f, 0.25f, 0.25f)));
                                                                               //todo, you know what to do
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("GoldGat", "DisplayGoldGat",
                                                                "PauldronL",
                                                                new Vector3(-0.05062F, 0.07337F, 0.17281F),
                                                                new Vector3(19.81884F, 188.5768F, 119.9942F),
                                                                new Vector3(0.1F, 0.1F, 0.1F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("BFG", "DisplayBFG",
                                                                "Chest",
                                                                new Vector3(0.07101F, 0.41512F, -0.19564F),
                                                                new Vector3(348.1825F, 276.8288F, 25.02212F),
                                                                new Vector3(0.4F, 0.4F, 0.4F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("QuestVolatileBattery", "DisplayBatteryArray",
                                                                "Chest",
                                                                new Vector3(0.33257F, 0.3451F, -0.01117F),
                                                                new Vector3(315F, 90F, 0F),
                                                                new Vector3(0.21F, 0.21F, 0.21F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("CommandMissile", "DisplayMissileRack",
                                                                "Chest",
                                                                new Vector3(0.26506F, 0.45562F, 0.00002F),
                                                                new Vector3(90F, 90F, 0F),
                                                                new Vector3(0.5F, 0.5F, 0.5F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("Fruit", "DisplayFruit",
                                                                "Pelvis",
                                                                new Vector3(0.13026F, 0.28527F, -0.16847F),
                                                                new Vector3(356.3801F, 347.5225F, 216.8458F),
                                                                new Vector3(0.21F, 0.21F, 0.21F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("CritOnUse", "DisplayNeuralImplant",
                                                                "Head",
                                                                new Vector3(-0.20606F, 0.06706F, -0.00143F),
                                                                new Vector3(0F, 90F, 0F),
                                                                new Vector3(0.21F, 0.21F, 0.21F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("DroneBackup", "DisplayRadio",
                                                                "Chest",
                                                                new Vector3(-0.24166F, 0.34219F, -0.08679F),
                                                                new Vector3(348.3908F, 272.9134F, 58.67701F),
                                                                new Vector3(0.4F, 0.4F, 0.4F)));
            itemRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRulesE("Lightning",
                ItemDisplays.CreateDisplayRule("DisplayLightningArmRight",
                                               "UpperArmR",
                                               new Vector3(0, 0, 0),
                                               new Vector3(0, 0, 0),
                                               new Vector3(1, 1, 1)),
                ItemDisplays.CreateLimbMaskDisplayRule(LimbFlags.RightArm)
                ));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("BurnNearby", "DisplayPotion", 
                                                                "Pelvis",
																new Vector3(0.07092F, 0.3068F, 0.50169F),
																new Vector3(36.01561F, 13.14477F, 138.4107F),
																new Vector3(0.03F, 0.03F, 0.03F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("CrippleWard", "DisplayEffigy", 
                                                                "Shield",
																new Vector3(0.08686F, 0.30249F, -0.21051F),
																new Vector3(10.46124F, 170.2762F, 1.58898F),
																new Vector3(0.22F, 0.22F, 0.22F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("GainArmor", "DisplayElephantFigure", 
                                                                "CalfL",
																new Vector3(-0.17336F, 0.08326F, -0.00223F),
																new Vector3(90F, 268.5319F, 0F),
																new Vector3(0.3F, 0.3F, 0.3F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("Recycle", "DisplayRecycler", 
                                                                "Chest",
																new Vector3(0.31706F, 0.37802F, 0.00421F),
																new Vector3(0F, 0F, 348.9059F),
																new Vector3(0.06F, 0.06F, 0.06F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("FireBallDash", "DisplayEgg", 
                                                                "Pelvis",
																new Vector3(0.08967F, 0.06729F, 0.29295F),
																new Vector3(90F, 0F, 0F),
																new Vector3(0.21F, 0.21F, 0.21F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("Cleanse", "DisplayWaterPack", 
                                                                "Chest",
																new Vector3(0.32123F, 0.17569F, -0.01137F),
																new Vector3(357.2928F, 90.24006F, 0.69147F),
																new Vector3(0.1F, 0.1F, 0.1F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("Tonic", "DisplayTonic", 
                                                                "ThighR",
																new Vector3(-0.00001F, 0.2792F, 0.18109F),
																new Vector3(359.9935F, 359.8932F, 180.1613F),
																new Vector3(0.21F, 0.21F, 0.21F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("Gateway", "DisplayVase", 
                                                                "Chest",
																new Vector3(0.11973F, 0.54832F, 0.26252F),
																new Vector3(359.2803F, 349.704F, 3.12542F),
																new Vector3(0.21F, 0.21F, 0.21F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("Scanner", "DisplayScanner", 
                                                                "Shield",
																new Vector3(0.54981F, 0.91523F, 0.27712F),
																new Vector3(293.9302F, 144.0041F, 339.115F),
																new Vector3(0.25F, 0.25F, 0.25F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("DeathProjectile", "DisplayDeathProjectile", 
                                                                "Chest",
																new Vector3(-0.23665F, 0.35165F, 0.12038F),
																new Vector3(335.7568F, 279.3308F, 358.6632F),
																new Vector3(0.04F, 0.04F, 0.04F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("LifestealOnHit", "DisplayLifestealOnHit", 
                                                                "Chest", 
                                                                new Vector3(0.202f, 0.215f, 0.2075f), 
                                                                new Vector3(45, 180, 0), 
                                                                new Vector3(0.25f, 0.25f, 0.25f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("TeamWarCry", "DisplayTeamWarCry", 
                                                                "Chest",
																new Vector3(-0.24951F, -0.12858F, -0.00915F),
																new Vector3(9.0684F, 272.2467F, 359.5266F),
																new Vector3(0.1F, 0.1F, 0.1F)));

            itemRules.Add(ItemDisplays.CreateFollowerDisplayRuleE("Icicle", "DisplayFrostRelic", 
                                                                new Vector3(0.235f, 0.23f, 0.24f), 
                                                                new Vector3(0, 0, 90), 
                                                                new Vector3(2, 2, 2)));
            itemRules.Add(ItemDisplays.CreateFollowerDisplayRuleE("Talisman", "DisplayTalisman", 
                                                                new Vector3(-0.215f, 0.23f, 0.25f), 
                                                                new Vector3(0, 270, 0), 
                                                                new Vector3(1, 1, 1)));
            itemRules.Add(ItemDisplays.CreateFollowerDisplayRuleE("FocusConvergence", "DisplayFocusedConvergence", 
                                                                new Vector3(0.235f, 0.21f, 0.23f), 
                                                                new Vector3(0, 0, 0), 
                                                                new Vector3(0.22f, 0.22f, 0.22f)));

            itemRules.Add(ItemDisplays.CreateFollowerDisplayRuleE("Saw", "DisplaySawmerang", 
																new Vector3(0.26F, 0.22F, 2.68246F),
																new Vector3(90F, 0F, 0F),
																new Vector3(0.225F, 0.225F, 0.225F)));
            itemRules.Add(ItemDisplays.CreateFollowerDisplayRuleE("Meteor", "DisplayMeteor",
																new Vector3(0.44932F, -0.01388F, 2.2416F),
																new Vector3(90F, 0F, 0F),
																new Vector3(1F, 1F, 1F)));
            itemRules.Add(ItemDisplays.CreateFollowerDisplayRuleE("Blackhole", "DisplayGravCube", 
																new Vector3(0.5472F, -0.06699F, 2.2766F),
																new Vector3(90F, 0F, 0F),
																new Vector3(1F, 1F, 1F)));
            #endregion quips

            ItemDisplays.printKeys();
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

            itemRules.Add(ItemDisplays.CreateSupplyDropRuleGroup("Bones",
                                                           "CalfR",
                                                           new Vector3(0.17997F, -0.05238F, 0.07133F),
                                                           new Vector3(13.68323F, 76.44486F, 191.9287F),
                                                           new Vector3(1.25683F, 1.25683F, 1.25683F)));
            itemRules.Add(ItemDisplays.CreateSupplyDropRuleGroup("Berries",
                                                           "loinFront2",
                                                           new Vector3(0.11782F, 0.27382F, -0.13743F),
                                                           new Vector3(341.1884F, 284.1298F, 180.0032F),
                                                           new Vector3(0.08647F, 0.08647F, 0.08647F)));
            itemRules.Add(ItemDisplays.CreateSupplyDropRuleGroup("UnassumingTie",
                                                           "Chest",
                                                           new Vector3(-0.08132F, 0.30226F, 0.34786F),
                                                           new Vector3(351.786F, 356.4574F, 0.73319F),
                                                           new Vector3(0.32213F, 0.35018F, 0.42534F)));
            itemRules.Add(ItemDisplays.CreateSupplyDropRuleGroup("SalvagedWires",
                                                           "UpperArmL",
                                                           new Vector3(-0.00419F, 0.10839F, -0.20693F),
                                                           new Vector3(21.68419F, 165.3445F, 132.0565F),
                                                           new Vector3(0.63809F, 0.63809F, 0.63809F)));

            itemRules.Add(ItemDisplays.CreateSupplyDropRuleGroup("ShellPlating",
                                                           "ThighR",
                                                           new Vector3(0.02115F, 0.52149F, -0.31269F),
                                                           new Vector3(319.6181F, 168.4007F, 184.779F),
                                                           new Vector3(0.24302F, 0.26871F, 0.26871F)));
            itemRules.Add(ItemDisplays.CreateSupplyDropRuleGroup("ElectroPlankton",
                                                           "ThighR",
                                                           new Vector3(0.21067F, 0.49094F, -0.08702F),
                                                           new Vector3(8.08377F, 285.087F, 164.4582F),
                                                           new Vector3(0.11243F, 0.11243F, 0.11243F)));

            itemRules.Add(ItemDisplays.CreateSupplyDropRuleGroup("BloodBook",
                                                           "Root",
                                                           new Vector3(2.19845F, -1.51445F, 1.59871F),
                                                           new Vector3(0, 0, 0),
                                                           new Vector3(0.12F, 0.12F, 0.12F)));
            itemRules.Add(ItemDisplays.CreateSupplyDropRuleGroup("QSGen",
                                                           "LowerArmL",
                                                           new Vector3(0.06003F, 0.1038F, -0.02042F),
                                                           new Vector3(0F, 18.75576F, 268.4665F),
                                                           new Vector3(0.12301F, 0.12301F, 0.12301F)));
        }
        #endregion

    }
}