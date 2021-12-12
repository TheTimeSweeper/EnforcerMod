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
                                                                new Vector3(-0.19034F, 0.07312F, 0F),
                                                                new Vector3(0F, 270F, 0F),
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
            //you don't have to do this. just do the default shotgun I'll do the rest
            //if you do want to i'll kiss ya
                //just don't skimp out on putting things on weapons because you don't wanna do them all
                
/*for items with multiple displays (with CreateDisplayRuleGroupWithRules):
{childName},
                                               {localPos}, 
                                               {localAngles},
                                               {localScale})
*/
            #endregion

            #region items
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("AlienHead", "DisplayAlienHead",
                                                                "Head",
                                                                new Vector3(0, -0.245f, 0),
                                                                new Vector3(315, 0, 0),
                                                                new Vector3(0.25f, 0.25f, 0.25f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ArmorPlate", "DisplayRepulsionArmorPlate",
                                                                "LowerArmR",
                                                                new Vector3(-0.04437F, 0.15214F, 0.03719F),
                                                                new Vector3(85.53775F, 207.959F, 117.8662F),
                                                                new Vector3(0.42477F, 0.42477F, 0.34069F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ArmorReductionOnHit", "DisplayWarhammer",
                                                                "Head",
                                                                new Vector3(0, 0.205f, 0),
                                                                new Vector3(270, 90, 0),
                                                                new Vector3(0.2175f, 0.2175f, 0.2175f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("AttackSpeedOnCrit", "DisplayWolfPelt",
                                                                "Head",
                                                                new Vector3(0, 0.208f, 0),
                                                                new Vector3(0, 270, 0),
                                                                new Vector3(0.215f, 0.215f, 0.215f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("AutoCastEquipment", "DisplayFossil",
                                                                "Pelvis",
                                                                new Vector3(-0.209f, 0.202f, 0),
                                                                new Vector3(0, 0, 0),
                                                                new Vector3(0.22f, 0.22f, 0.22f)));

            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Bandolier", "DisplayBandolier",
                                                                "Chest",
                                                                new Vector3(0, 0.205f, 0),
                                                                new Vector3(315, 0, 180),
                                                                new Vector3(0.22f, 0.23f, 0.23f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("BarrierOnKill", "DisplayBrooch",
                                                                "Chest",
                                                                new Vector3(-0.26885F, 0.18017F, -0.14314F), 
                                                                new Vector3(87.77314F, 257.9498F, 347.8895F),
                                                                new Vector3(0.72285F, 0.61958F, 0.61958F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("BarrierOnOverHeal", "DisplayAegis",
                                                                "Head",
                                                                new Vector3(-0.202f, -0.205f, 0),
                                                                new Vector3(90, 90, 0),
                                                                new Vector3(0.21f, 0.21f, 0.21f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Bear", "DisplayBear",
                                                                "Shield",
                                                                new Vector3(0.33149F, -0.18085F, 0.07887F),
                                                                new Vector3(7.01418F, 125.5218F, 351.2383F),
                                                                new Vector3(0.58719F, 0.58719F, 0.58719F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("BeetleGland", "DisplayBeetleGland",
                                                                "Chest",
                                                                new Vector3(0, 0.215f, -0.208f),
                                                                new Vector3(0, 270, 0),
                                                                new Vector3(0.235f, 0.235f, 0.235f)));
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
                                                                "Head",
                                                                new Vector3(0.205f, 0.205f, 0),
                                                                new Vector3(0, 0, 0),
                                                                new Vector3(0.22f, 0.22f, 0.22f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("BonusGoldPackOnKill", "DisplayTome",
                                                                "Pelvis",
                                                                new Vector3(-0.205f, 0, -0.205f),
                                                                new Vector3(0, 235, 0),
                                                                new Vector3(0.225f, 0.225f, 0.225f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("BossDamageBonus", "DisplayAPRound",
                                                                "Pelvis",
                                                                new Vector3(0, 0, 0.208f),
                                                                new Vector3(90, 0, 0),
                                                                new Vector3(0.22f, 0.22F, 0.22F)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("BounceNearby", "DisplayHook",
                                                                "Chest",
                                                                new Vector3(0, 0.202f, 0),
                                                                new Vector3(270, 90, 0),
                                                                new Vector3(0.22f, 0.22f, 0.22f)));

            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ChainLightning", "DisplayUkulele",
                                                                "Head",
                                                                new Vector3(0, 0.21f, 0.204f),
                                                                new Vector3(0, 0, 0),
                                                                new Vector3(0.23f, 0.23f, 0.23f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Clover", "DisplayClover",
                                                                "Head",
                                                                new Vector3(0, 0.208f, -0.203f),
                                                                new Vector3(270, 0, 0),
                                                                new Vector3(0.22f, 0.22f, 0.22f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("CooldownOnCrit", "DisplaySkull",
                                                                "Head",
                                                                new Vector3(0, 0.205f, 0),
                                                                new Vector3(270, 90, 0),
                                                                new Vector3(0.285f, 0.285f, 0.21f)));
                                                              //CritGlasses: see example above
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Crowbar", "DisplayCrowbar",
                                                                "Chest",
                                                                new Vector3(0, 0.202f, 0),
                                                                new Vector3(270, 90, 0),
                                                                new Vector3(0.22f, 0.2175f, 0.2175f)));

            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Dagger", "DisplayDagger",
                                                                "Head",
                                                                new Vector3(0, 0.23f, 0),
                                                                new Vector3(270, 90, 0),
                                                                new Vector3(0.215f, 0.215f, 0.215f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("DeathMark", "DisplayDeathMark",
                                                                "Head",
                                                                new Vector3(0, 0.204f, 0),
                                                                new Vector3(270, 90, 0),
                                                                new Vector3(0.22f, 0.22f, 0.22f)));

            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("EnergizedOnEquipmentUse", "DisplayWarHorn",
                                                                "Pelvis",
                                                                new Vector3(0.206f, 0.202f, 0.206f),
                                                                new Vector3(0, 190, 90),
                                                                new Vector3(0.215f, 0.215f, 0.215f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("EquipmentMagazine", "DisplayBattery",
                                                                "Chest",
                                                                new Vector3(0.21f, 0.201f, 0),
                                                                new Vector3(0, 0, 0),
                                                                new Vector3(0.21f, 0.21f, 0.21f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ExecuteLowHealthElite", "DisplayGuillotine",
                                                                "Head",
                                                                new Vector3(0.2068f, 0.207f, 0.21f),
                                                                new Vector3(0, 180, 90),
                                                                new Vector3(0.22f, 0.22f, 0.2175f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ExplodeOnDeath", "DisplayWilloWisp",
                                                                "Pelvis",
                                                                new Vector3(0, 0, -0.21f),
                                                                new Vector3(0, 0, 180),
                                                                new Vector3(0.225f, 0.225f, 0.225f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ExtraLife", "DisplayHippo",
                                                                "Head",
                                                                new Vector3(0, 0.22f, 0.214f),
                                                                new Vector3(0, 0, 0),
                                                                new Vector3(0.22f, 0.22f, 0.22f)));

            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Feather", "DisplayFeather",
                                                                "Head",
                                                                new Vector3(0, 0.208f, 0),
                                                                new Vector3(0, 0, 270),
                                                                new Vector3(0.215f, 0.215f, 0.215f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("FireballsOnHit", "DisplayFireballsOnHit",
                                                                "Head",
                                                                new Vector3(0, 0.22f, 0.21f),
                                                                new Vector3(0, 0, 0),
                                                                new Vector3(0.25f, 0.25f, 0.25f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("FireRing", "DisplayFireRing",
                                                                "Head",
                                                                new Vector3(0, 0.202f, 0),
                                                                new Vector3(270, 90, 0),
                                                                new Vector3(0.22f, 0.2175f, 0.2175f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Firework", "DisplayFirework",
                                                                "Pelvis",
                                                                new Vector3(0, 0.202f, 0.21f),
                                                                new Vector3(90, 0, 0),
                                                                new Vector3(0.21f, 0.21f, 0.21f)));

            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("GhostOnKill", "DisplayMask",
                                                                "Head",
                                                                new Vector3(-0.2025f, 0.203f, 0),
                                                                new Vector3(0, 270, 0),
                                                                new Vector3(0.22f, 0.22f, 0.22f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("GoldOnHit", "DisplayBoneCrown",
                                                                "Head",
                                                                new Vector3(0.201f, 0.205f, 0),
                                                                new Vector3(0, 270, 0),
                                                                new Vector3(0.235f, 0.235f, 0.235f)));

            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("HeadHunter", "DisplaySkullCrown",
                                                                "Head",
                                                                new Vector3(0, 0.2075f, 0),
                                                                new Vector3(0, 270, 0),
                                                                new Vector3(0.215f, 0.25f, 0.25f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("HealOnCrit", "DisplayScythe",
                                                                "Chest",
                                                                new Vector3(0.212f, 0.21f, 0),
                                                                new Vector3(270, 90, 0),
                                                                new Vector3(0.21f, 0.21f, 0.21f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("HealWhileSafe", "DisplaySnail",
                                                                "Chest",
                                                                new Vector3(-0.205f, 0.202f, 0),
                                                                new Vector3(270, 90, 0),
                                                                new Vector3(0.235f, 0.235f, 0.235f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Hoof", "DisplayHoof",
                                                                "KneeR",
                                                                new Vector3(-0.203f, 0.209f, 0),
                                                                new Vector3(75, 90, 0),
                                                                new Vector3(0.24f, 0.24f, 0.23f)));

            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("IceRing", "DisplayIceRing",
                                                                "Head",
                                                                new Vector3(0, 0.202f, 0),
                                                                new Vector3(270, 90, 0),
                                                                new Vector3(0.22f, 0.2175f, 0.2175f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("IgniteOnKill", "DisplayGasoline",
                                                                "Pelvis",
                                                                new Vector3(0, 0.206f, 0.21f),
                                                                new Vector3(70, 180, 90),
                                                                new Vector3(0.22f, 0.22f, 0.22f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Incubator", "DisplayAncestralIncubator",
                                                                "Head",
                                                                new Vector3(0, 0.212f, 0),
                                                                new Vector3(90, 0, 0),
                                                                new Vector3(0.235f, 0.235f, 0.235f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Infusion", "DisplayInfusion",
                                                                "Pelvis",
                                                                new Vector3(0, 0.202f, 0.208f),
                                                                new Vector3(0, 0, 0),
                                                                new Vector3(0.22f, 0.22f, 0.22f)));


            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("JumpBoost", "DisplayWaxBird",
                                                                "Head",
                                                                new Vector3(0, -0.208f, 0),
                                                                new Vector3(0, 270, 0),
                                                                new Vector3(0.225f, 0.225f, 0.225f)));

            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("KillEliteFrenzy", "DisplayBrainstalk",
                                                                "Head",
                                                                new Vector3(0.201f, 0.207f, 0),
                                                                new Vector3(0, 90, 0),
                                                                new Vector3(0.26f, 0.26f, 0.26f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Knurl", "DisplayKnurl",
                                                                "Chest",
                                                                new Vector3(-0.205f, 0.208f, -0.204f),
                                                                new Vector3(0, 315, 0),
                                                                new Vector3(0.235f, 0.235f, 0.235f)));

            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("LaserTurbine", "DisplayLaserTurbine",
                                                                "Head",
                                                                new Vector3(0.206f, 0.206f, 0),
                                                                new Vector3(0, 90, 0),
                                                                new Vector3(0.215f, 0.215f, 0.215f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("LunarDagger", "DisplayLunarDagger",
                                                                "Chest",
                                                                new Vector3(0.21f, 0.202f, 0),
                                                                new Vector3(290, 90, 0),
                                                                new Vector3(0.23f, 0.23f, 0.23f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("LunarPrimaryReplacement", "DisplayBirdEye",
                                                                "Head",
                                                                new Vector3(-0.203f, 0.2025f, 0),
                                                                new Vector3(0, 0, 270),
                                                                new Vector3(0.21f, 0.21f, 0.21f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("LunarTrinket", "DisplayBeads",
                                                                "Head",
                                                                new Vector3(0.202f, 0.204f, 0.2015f),
                                                                new Vector3(0, 90, 90),
                                                                new Vector3(0.24f, 0.24f, 0.24f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("LunarUtilityReplacement", "DisplayBirdFoot",
                                                                "Head",
                                                                new Vector3(0.208f, 0.208f, 0),
                                                                new Vector3(0, 180, 0),
                                                                new Vector3(0.22f, 0.22f, 0.22f)));

            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Medkit", "DisplayMedkit",
                                                                "Chest",
                                                                new Vector3(0.21f, 0.2035f, 0),
                                                                new Vector3(300, 90, 0),
                                                                new Vector3(0.22f, 0.22f, 0.22f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Missile", "DisplayMissileLauncher",
                                                                "Chest",
                                                                new Vector3(0, 0.23f, 0),
                                                                new Vector3(0, 270, 0),
                                                                new Vector3(0.25f, 0.25f, 0.25f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("MonstersOnShrineUse", "DisplayMonstersOnShrineUse",
                                                                "Head",
                                                                new Vector3(-0.205f, 0.205f, 0.22f),
                                                                new Vector3(90, 0, 0),
                                                                new Vector3(0.22f, 0.22f, 0.22f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Mushroom", "DisplayMushroom",
                                                                "Head",
                                                                new Vector3(0, 0.21f, 0),
                                                                new Vector3(45, 90, 0),
                                                                new Vector3(0.235f, 0.235f, 0.235f)));

            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("NearbyDamageBonus", "DisplayDiamond",
                                                                "Head",
                                                                new Vector3(0, 0, 0),
                                                                new Vector3(0, 0, 0),
                                                                new Vector3(0.25f, 0.25f, 0.25f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("NovaOnLowHealth", "DisplayJellyGuts",
                                                                "Chest",
                                                                new Vector3(0, 0.211f, 0),
                                                                new Vector3(310, 270, 0),
                                                                new Vector3(0.25f, 0.25f, 0.25f)));

            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Pearl", "DisplayPearl",
                                                                "Head",
                                                                new Vector3(0, 0, 0),
                                                                new Vector3(0, 0, 0),
                                                                new Vector3(0.205f, 0.25f, 0.25f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("PersonalShield", "DisplayShieldGenerator",
                                                                "Chest",
                                                                new Vector3(-0.206f, 0.205f, 0.205f),
                                                                new Vector3(90, 100, 0),
                                                                new Vector3(0.25f, 0.25f, 0.25f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Phasing", "DisplayStealthkit",
                                                                "Head",
                                                                new Vector3(-0.204f, 0.21f, 0),
                                                                new Vector3(90, 90, 0),
                                                                new Vector3(0.21f, 0.21f, 0.21f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Plant", "DisplayInterstellarDeskPlant",
                                                                "Head",
                                                                new Vector3(0, 0.202f, 0),
                                                                new Vector3(270, 90, 0),
                                                                new Vector3(0.22f, 0.2175f, 0.2175f)));

            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("RandomDamageZone", "DisplayRandomDamageZone",
                                                                "Head",
                                                                new Vector3(0.202f, 0.205f, 0.201f),
                                                                new Vector3(0, 270, 270),
                                                                new Vector3(0.22f, 0.22f, 0.22f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("RepeatHeal", "DisplayCorpseFlower",
                                                                "Head",
                                                                new Vector3(0.205f, 0.21f, 0),
                                                                new Vector3(0, 25, 300),
                                                                new Vector3(0.21f, 0.21f, 0.21f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("RegenOnKill", "DisplaySteakCurved",
                                                                "Head",
                                                                new Vector3(0, 0.215f, 0.215f),
                                                                new Vector3(335, 0, 180),
                                                                new Vector3(0.275f, 0.275f, 0.275f)));
            
                                                              //SecondarySkillMagazine: see example above
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Seed", "DisplaySeed",
                                                                "Head",
                                                                new Vector3(0, 0.205f, 0),
                                                                new Vector3(270, 0, 0),
                                                                new Vector3(0.225f, 0.225f, 0.225f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ShinyPearl", "DisplayShinyPearl",
                                                                "Head",
                                                                new Vector3(0, 0, 0),
                                                                new Vector3(0, 0, 0),
                                                                new Vector3(0.25f, 0.25f, 0.25f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ShockNearby", "DisplayTeslaCoil",
                                                                "Chest",
                                                                new Vector3(0.21f, 0.21f, 0),
                                                                new Vector3(0, 0, 315),
                                                                new Vector3(0.21f, 0.21f, 0.21f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("SiphonOnLowHealth", "DisplaySiphonOnLowHealth",
                                                                "Pelvis",
                                                                new Vector3(-0.206f, 0.204f, 0.206f),
                                                                new Vector3(0, 315, 180),
                                                                new Vector3(0.235f, 0.235f, 0.235f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("SprintArmor", "DisplayBuckler",
                                                                "Head",
                                                                new Vector3(0.202f, 0.205f, 0),
                                                                new Vector3(0, 90, 0),
                                                                new Vector3(0.21f, 0.21f, 0.21f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("SprintBonus", "DisplaySoda",
                                                                "Pelvis",
                                                                new Vector3(0.204f, 0.202f, -0.205f),
                                                                new Vector3(270, 90, 0),
                                                                new Vector3(0.21f, 0.21f, 0.21f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("SprintOutOfCombat", "DisplayWhip",
                                                                "Pelvis",
                                                                new Vector3(0, 0.204f, 0.209f),
                                                                new Vector3(0, 90, 200),
                                                                new Vector3(0.2175f, 0.2175f, 0.2175f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("SprintWisp", "DisplayBrokenMask",
                                                                "Head",
                                                                new Vector3(0.205f, 0.203f, 0),
                                                                new Vector3(0, 90, 180),
                                                                new Vector3(0.21f, 0.21f, 0.21f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Squid", "DisplaySquidTurret",
                                                                "Chest",
                                                                new Vector3(0.212f, 0.21f, 0.204f),
                                                                new Vector3(0, 0, 290),
                                                                new Vector3(0.225f, 0.225f, 0.225f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("StickyBomb", "DisplayStickyBomb",
                                                                "Pelvis",
                                                                new Vector3(0.2025f, 0.202f, -0.208f),
                                                                new Vector3(345, 15, 0),
                                                                new Vector3(0.21f, 0.21f, 0.21f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("StunChanceOnHit", "DisplayStunGrenade",
                                                                "Head",
                                                                new Vector3(-0.205f, 0.21f, 0),
                                                                new Vector3(90, 270, 0),
                                                                new Vector3(0.23f, 0.23f, 0.23f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Syringe", "DisplaySyringeCluster",
                                                                "Chest",
                                                                new Vector3(0, 0.212f, 0.206f),
                                                                new Vector3(25, 315, 0),
                                                                new Vector3(0.25f, 0.25f, 0.25f)));

            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("TPHealingNova", "DisplayGlowFlower",
                                                                "Chest",
                                                                new Vector3(-0.2055f, 0.203f, -0.204f),
                                                                new Vector3(0, 250, 0),
                                                                new Vector3(0.21f, 0.21f, 0.21f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Thorns", "DisplayRazorwireLeft",
                                                                "Head",
                                                                new Vector3(0, 0, 0),
                                                                new Vector3(270, 0, 0),
                                                                new Vector3(0.22f, 0.22f, 0.225f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("TitanGoldDuringTP", "DisplayGoldHeart",
                                                                "Chest",
                                                                new Vector3(-0.205f, 0.202f, -0.205f),
                                                                new Vector3(0, 235, 0),
                                                                new Vector3(0.285f, 0.285f, 0.285f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Tooth", "DisplayToothMeshLarge",
                                                                "Chest",
                                                                new Vector3(0, 0.212f, 0.205f),
                                                                new Vector3(290, 0, 0),
                                                                new Vector3(0.23f, 0.23f, 0.23f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("TreasureCache", "DisplayKey",
                                                                "Pelvis",
                                                                new Vector3(0.206f, 0.202f, -0.203f),
                                                                new Vector3(0, 25, 270),
                                                                new Vector3(0.23f, 0.23f, 0.23f)));

            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("UtilitySkillMagazine", "DisplayAfterburnerShoulderRing",
                                                                "Head",
                                                                new Vector3(0, 0.21f, 0.202f),
                                                                new Vector3(90, 0, 0),
                                                                new Vector3(0.235f, 0.235f, 0.235f)));

            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("WarCryOnMultiKill", "DisplayPauldron",
                                                                "Head",
                                                                new Vector3(0, 0.208f, -0.208f),
                                                                new Vector3(60, 180, 0),
                                                                new Vector3(0.23f, 0.23f, 0.23f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRule("WardOnLevel", "DisplayWarbanner",
                                                                "Pelvis",
                                                                new Vector3(-0.21f, 0, 0),
                                                                new Vector3(0, 90, 90),
                                                                new Vector3(0.2175f, 0.2175f, 0.2175f)));
#endregion items

            #region quips
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("Jetpack", "DisplayBugWings", 
                                                                "Chest", 
                                                                new Vector3(0.208f, 0.208f, 0), 
                                                                new Vector3(0, 270, 0), 
                                                                new Vector3(0.25f, 0.25f, 0.25f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("GoldGat", "DisplayGoldGat", 
                                                                "Chest", 
                                                                new Vector3(0.203f, 0.207f, 0), 
                                                                new Vector3(0, 0, 0), 
                                                                new Vector3(0.22f, 0.22f, 0.22f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("BFG", "DisplayBFG", 
                                                                "Chest", 
                                                                new Vector3(0, 0.212f, -0.206f), 
                                                                new Vector3(15, 270, 25), 
                                                                new Vector3(0.21f, 0.21f, 0.21f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("QuestVolatileBattery", "DisplayBatteryArray", 
                                                                "Chest", 
                                                                new Vector3(0.212f, 0.212f, 0), 
                                                                new Vector3(315, 90, 0), 
                                                                new Vector3(0.21f, 0.21f, 0.21f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("CommandMissile", "DisplayMissileRack", 
                                                                "Chest", 
                                                                new Vector3(0.21f, 0.21f, 0), 
                                                                new Vector3(90, 90, 0), 
                                                                new Vector3(0.22f, 0.22f, 0.22f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("Fruit", "DisplayFruit", 
                                                                "Chest", 
                                                                new Vector3(0, 0, 0), 
                                                                new Vector3(0, 150, 0), 
                                                                new Vector3(0.21f, 0.21f, 0.21f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("AffixWhite", "DisplayEliteIceCrown", 
                                                                "Head", 
                                                                new Vector3(0, 0.212f, 0), 
                                                                new Vector3(270, 270, 0), 
                                                                new Vector3(0.21f, 0.21f, 0.21f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("AffixPoison", "DisplayEliteUrchinCrown", 
                                                                "Head", 
                                                                new Vector3(0, 0.208f, 0), 
                                                                new Vector3(270, 0, 0), 
                                                                new Vector3(0.22f, 0.22f, 0.22f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("AffixHaunted", "DisplayEliteStealthCrown", 
                                                                "Head", 
                                                                new Vector3(0, 0.208f, 0), 
                                                                new Vector3(270, 0, 0), 
                                                                new Vector3(0.22f, 0.22f, 0.22f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("CritOnUse", "DisplayNeuralImplant", 
                                                                "Head", 
                                                                new Vector3(-0.208f, 0.206f, 0), 
                                                                new Vector3(0, 90, 0), 
                                                                new Vector3(0.21f, 0.21f, 0.21f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("DroneBackup", "DisplayRadio", 
                                                                "Pelvis", 
                                                                new Vector3(0, 0, 0.208f), 
                                                                new Vector3(0, 0, 180), 
                                                                new Vector3(0.21f, 0.21f, 0.21f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("Lightning", "DisplayLightning", 
                                                                "Head", 
                                                                new Vector3(0, 0, 0), 
                                                                new Vector3(0, 0, 0), 
                                                                new Vector3(0.24f, 0.24f, 0.24f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("BurnNearby", "DisplayPotion", 
                                                                "Pelvis", 
                                                                new Vector3(0, 0, 0.208f), 
                                                                new Vector3(0, 0, 180), 
                                                                new Vector3(0.22f, 0.22f, 0.22f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("CrippleWard", "DisplayEffigy", 
                                                                "Pelvis", 
                                                                new Vector3(0, 0.208f, 0.209f), 
                                                                new Vector3(0, 180, 180), 
                                                                new Vector3(0.22f, 0.22f, 0.22f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("GainArmor", "DisplayElephantFigure", 
                                                                "KneeR", 
                                                                new Vector3(0.204f, 0.212f, 0), 
                                                                new Vector3(90, 90, 0), 
                                                                new Vector3(0.22f, 0.22f, 0.22f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("Recycle", "DisplayRecycler", 
                                                                "Chest", 
                                                                new Vector3(0.212f, 0.212f, 0), 
                                                                new Vector3(0, 0, 0), 
                                                                new Vector3(0.25f, 0.25f, 0.25f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("FireBallDash", "DisplayEgg", 
                                                                "Pelvis", 
                                                                new Vector3(0, 0, 0.208f), 
                                                                new Vector3(90, 0, 0), 
                                                                new Vector3(0.21f, 0.21f, 0.21f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("Cleanse", "DisplayWaterPack", 
                                                                "Chest", 
                                                                new Vector3(0.212f, 0.21f, 0), 
                                                                new Vector3(315, 90, 0), 
                                                                new Vector3(0.25f, 0.25f, 0.25f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("Tonic", "DisplayTonic", 
                                                                "Pelvis", 
                                                                new Vector3(0, 0, 0.208f), 
                                                                new Vector3(0, 0, 180), 
                                                                new Vector3(0.21f, 0.21f, 0.21f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("Gateway", "DisplayVase", 
                                                                "Pelvis", 
                                                                new Vector3(0, 0, 0.209f), 
                                                                new Vector3(0, 0, 180), 
                                                                new Vector3(0.21f, 0.21f, 0.21f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("Scanner", "DisplayScanner", 
                                                                "Pelvis", 
                                                                new Vector3(0, 0.205f, 0.208f), 
                                                                new Vector3(90, 270, 0), 
                                                                new Vector3(0.25f, 0.25f, 0.25f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("DeathProjectile", "DisplayDeathProjectile", 
                                                                "Pelvis", 
                                                                new Vector3(-0.2012f, 0.205f, 0), 
                                                                new Vector3(0, 270, 180), 
                                                                new Vector3(0.25f, 0.25f, 0.25f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("LifestealOnHit", "DisplayLifestealOnHit", 
                                                                "Head", 
                                                                new Vector3(0.202f, 0.215f, 0.2075f), 
                                                                new Vector3(45, 180, 0), 
                                                                new Vector3(0.25f, 0.25f, 0.25f)));
            itemRules.Add(ItemDisplays.CreateGenericDisplayRuleE("TeamWarCry", "DisplayTeamWarCry", 
                                                                "Pelvis", 
                                                                new Vector3(0.21f, 0.203f, 0), 
                                                                new Vector3(10, 90, 0), 
                                                                new Vector3(0.235f, 0.235f, 0.235f)));

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
                                                                new Vector3(0.26f, 0.22f, 0), 
                                                                new Vector3(90, 0, 0), 
                                                                new Vector3(0.225f, 0.225f, 0.225f)));
            itemRules.Add(ItemDisplays.CreateFollowerDisplayRuleE("Meteor", "DisplayMeteor", 
                                                                new Vector3(0.25f, 0.22f, 0), 
                                                                new Vector3(90, 0, 0), 
                                                                new Vector3(1, 1, 1)));
            itemRules.Add(ItemDisplays.CreateFollowerDisplayRuleE("Blackhole", "DisplayGravCube", 
                                                                new Vector3(0.25f, 0.22f, 0), 
                                                                new Vector3(90, 0, 0), 
                                                                new Vector3(1, 1, 1)));
#endregion quips
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