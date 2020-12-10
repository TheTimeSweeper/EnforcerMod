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
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("SecondarySkillMagazine", "DisplayDoubleMag", "Hammer", new Vector3(0, 0.016f, -0.001f), new Vector3(70, 0, 180), new Vector3(0.005f, 0.005f, 0.005f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("Bear", "DisplayBear", "Hammer", new Vector3(0, 0.012f, 0.012f), new Vector3(0, 0, 0), new Vector3(0.0175f, 0.0175f, 0.0175f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("SprintOutOfCombat", "DisplayWhip", "Pelvis", new Vector3(0, 0.004f, 0.009f), new Vector3(0, 90, 200), new Vector3(0.0175f, 0.0175f, 0.0175f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("PersonalShield", "DisplayShieldGenerator", "Chest", new Vector3(-0.006f, 0.005f, 0.005f), new Vector3(90, 100, 0), new Vector3(0.005f, 0.005f, 0.005f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("RegenOnKill", "DisplaySteakCurved", "Hammer", new Vector3(0, 0.015f, 0.015f), new Vector3(335, 0, 180), new Vector3(0.0075f, 0.0075f, 0.0075f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("FireballsOnHit", "DisplayFireballsOnHit", "Hammer", new Vector3(0, 0.02f, 0.01f), new Vector3(0, 0, 0), new Vector3(0.005f, 0.005f, 0.005f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("Hoof", "DisplayHoof", "KneeR", new Vector3(-0.003f, 0.009f, 0), new Vector3(75, 90, 0), new Vector3(0.004f, 0.004f, 0.003f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("WardOnLevel", "DisplayWarbanner", "Pelvis", new Vector3(-0.01f, 0, 0), new Vector3(0, 90, 90), new Vector3(0.0175f, 0.0175f, 0.0175f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("BarrierOnOverHeal", "DisplayAegis", "ElbowR", new Vector3(-0.002f, -0.005f, 0), new Vector3(90, 90, 0), new Vector3(0.01f, 0.01f, 0.01f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("WarCryOnMultiKill", "DisplayPauldron", "ClavicleL", new Vector3(0, 0.008f, -0.008f), new Vector3(60, 180, 0), new Vector3(0.03f, 0.03f, 0.03f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("SprintArmor", "DisplayBuckler", "ElbowL", new Vector3(0.002f, 0.005f, 0), new Vector3(0, 90, 0), new Vector3(0.01f, 0.01f, 0.01f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("IceRing", "DisplayIceRing", "HandL", new Vector3(0, 0.002f, 0), new Vector3(270, 90, 0), new Vector3(0.02f, 0.0175f, 0.0175f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("FireRing", "DisplayFireRing", "HandR", new Vector3(0, 0.002f, 0), new Vector3(270, 90, 0), new Vector3(0.02f, 0.0175f, 0.0175f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("Behemoth", "DisplayBehemoth", "Minigun", new Vector3(-0.016f, 0.008f, -0.005f), new Vector3(0, 270, 90), new Vector3(0.005f, 0.005f, 0.005f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("Missile", "DisplayMissileLauncher", "Chest", new Vector3(0, 0.03f, 0), new Vector3(0, 270, 0), new Vector3(0.005f, 0.005f, 0.005f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("Dagger", "DisplayDagger", "Chest", new Vector3(0, 0, 0), new Vector3(0, 270, 0), new Vector3(0.02f, 0.02f, 0.02f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("ChainLightning", "DisplayUkulele", "MinigunBarrel", new Vector3(0, 0.01f, 0.004f), new Vector3(0, 0, 0), new Vector3(0.03f, 0.03f, 0.03f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("GhostOnKill", "DisplayMask", "Head", new Vector3(-0.0025f, 0.003f, 0), new Vector3(0, 270, 0), new Vector3(0.02f, 0.02f, 0.02f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("Mushroom", "DisplayMushroom", "ClavicleR", new Vector3(0, 0.01f, 0), new Vector3(45, 90, 0), new Vector3(0.0035f, 0.0035f, 0.0035f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("AttackSpeedOnCrit", "DisplayWolfPelt", "Head", new Vector3(0, 0.008f, 0), new Vector3(0, 270, 0), new Vector3(0.015f, 0.015f, 0.015f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("BleedOnHit", "DisplayTriTip", "MinigunBarrel", new Vector3(0, 0.03f, 0), new Vector3(270, 90, 0), new Vector3(0.015f, 0.015f, 0.015f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("HealOnCrit", "DisplayScythe", "Chest", new Vector3(0.012f, 0.01f, 0), new Vector3(270, 90, 0), new Vector3(0.01f, 0.01f, 0.01f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("HealWhileSafe", "DisplaySnail", "Chest", new Vector3(-0.005f, 0.002f, 0), new Vector3(270, 90, 0), new Vector3(0.0035f, 0.0035f, 0.0035f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("Clover", "DisplayClover", "Head", new Vector3(0, 0.008f, -0.003f), new Vector3(270, 0, 0), new Vector3(0.02f, 0.02f, 0.02f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("GoldOnHit", "DisplayBoneCrown", "Head", new Vector3(0.001f, 0.005f, 0), new Vector3(0, 270, 0), new Vector3(0.035f, 0.035f, 0.035f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("JumpBoost", "DisplayWaxBird", "Head", new Vector3(0, -0.008f, 0), new Vector3(0, 270, 0), new Vector3(0.025f, 0.025f, 0.025f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("ArmorPlate", "DisplayRepulsionArmorPlate", "LegL", new Vector3(-0.003f, 0.0075f, 0.001f), new Vector3(90, 90, 0), new Vector3(0.01f, 0.01f, 0.01f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("Feather", "DisplayFeather", "ElbowL", new Vector3(0, 0.008f, 0), new Vector3(0, 0, 270), new Vector3(0.0015f, 0.0015f, 0.0015f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("Crowbar", "DisplayCrowbar", "Chest", new Vector3(0, 0.002f, 0), new Vector3(270, 90, 0), new Vector3(0.02f, 0.0175f, 0.0175f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("ExecuteLowHealthElite", "DisplayGuillotine", "Hammer", new Vector3(0.0068f, 0.007f, 0.01f), new Vector3(0, 180, 90), new Vector3(0.02f, 0.02f, 0.0175f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("EquipmentMagazine", "DisplayBattery", "Chest", new Vector3(0.01f, 0.001f, 0), new Vector3(0, 0, 0), new Vector3(0.01f, 0.01f, 0.01f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("Infusion", "DisplayInfusion", "Pelvis", new Vector3(0, 0.002f, 0.008f), new Vector3(0, 0, 0), new Vector3(0.02f, 0.02f, 0.02f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("Medkit", "DisplayMedkit", "Chest", new Vector3(0.01f, 0.0035f, 0), new Vector3(300, 90, 0), new Vector3(0.02f, 0.02f, 0.02f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("Bandolier", "DisplayBandolier", "Chest", new Vector3(0, 0.005f, 0), new Vector3(315, 0, 180), new Vector3(0.02f, 0.03f, 0.03f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("BounceNearby", "DisplayHook", "Chest", new Vector3(0, 0.002f, 0), new Vector3(270, 90, 0), new Vector3(0.02f, 0.02f, 0.02f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("StunChanceOnHit", "DisplayStunGrenade", "LegR", new Vector3(-0.005f, 0.01f, 0), new Vector3(90, 270, 0), new Vector3(0.03f, 0.03f, 0.03f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("IgniteOnKill", "DisplayGasoline", "Pelvis", new Vector3(0, 0.006f, 0.01f), new Vector3(70, 180, 90), new Vector3(0.02f, 0.02f, 0.02f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("Firework", "DisplayFirework", "Pelvis", new Vector3(0, 0.002f, 0.01f), new Vector3(90, 0, 0), new Vector3(0.01f, 0.01f, 0.01f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("LunarDagger", "DisplayLunarDagger", "Chest", new Vector3(0.01f, 0.002f, 0), new Vector3(290, 90, 0), new Vector3(0.03f, 0.03f, 0.03f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("Knurl", "DisplayKnurl", "Chest", new Vector3(-0.005f, 0.008f, -0.004f), new Vector3(0, 315, 0), new Vector3(0.0035f, 0.0035f, 0.0035f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("BeetleGland", "DisplayBeetleGland", "Chest", new Vector3(0, 0.015f, -0.008f), new Vector3(0, 270, 0), new Vector3(0.0035f, 0.0035f, 0.0035f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("SprintBonus", "DisplaySoda", "Pelvis", new Vector3(0.004f, 0.002f, -0.005f), new Vector3(270, 90, 0), new Vector3(0.01f, 0.01f, 0.01f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("StickyBomb", "DisplayStickyBomb", "Pelvis", new Vector3(0.0025f, 0.002f, -0.008f), new Vector3(345, 15, 0), new Vector3(0.01f, 0.01f, 0.01f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("TreasureCache", "DisplayKey", "Pelvis", new Vector3(0.006f, 0.002f, -0.003f), new Vector3(0, 25, 270), new Vector3(0.03f, 0.03f, 0.03f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("BossDamageBonus", "DisplayAPRound", "Pelvis", new Vector3(0, 0, 0.008f), new Vector3(90, 0, 0), new Vector3(0.02f, 0.02F, 0.02F)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("ExtraLife", "DisplayHippo", "Hammer", new Vector3(0, 0.02f, 0.014f), new Vector3(0, 0, 0), new Vector3(0.02f, 0.02f, 0.02f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("KillEliteFrenzy", "DisplayBrainstalk", "Head", new Vector3(0.001f, 0.007f, 0), new Vector3(0, 90, 0), new Vector3(0.006f, 0.006f, 0.006f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("RepeatHeal", "DisplayCorpseFlower", "ClavicleR", new Vector3(0.005f, 0.01f, 0), new Vector3(0, 25, 300), new Vector3(0.01f, 0.01f, 0.01f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("AutoCastEquipment", "DisplayFossil", "Pelvis", new Vector3(-0.009f, 0.002f, 0), new Vector3(0, 0, 0), new Vector3(0.02f, 0.02f, 0.02f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("TitanGoldDuringTP", "DisplayGoldHeart", "Chest", new Vector3(-0.005f, 0.002f, -0.005f), new Vector3(0, 235, 0), new Vector3(0.0085f, 0.0085f, 0.0085f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("SprintWisp", "DisplayBrokenMask", "ShoulderL", new Vector3(0, 0.002f, 0), new Vector3(270, 90, 0), new Vector3(0.02f, 0.0175f, 0.0175f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("BarrierOnKill", "DisplayBrooch", "Chest", new Vector3(-0.007f, 0.007f, 0), new Vector3(90, 270, 0), new Vector3(0.015f, 0.015f, 0.015f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("TPHealingNova", "DisplayGlowFlower", "Chest", new Vector3(-0.0055f, 0.003f, -0.004f), new Vector3(0, 250, 0), new Vector3(0.01f, 0.01f, 0.01f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("LunarUtilityReplacement", "DisplayBirdFoot", "Head", new Vector3(0.008f, 0.008f, 0), new Vector3(0, 180, 0), new Vector3(0.02f, 0.02f, 0.02f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("Thorns", "DisplayRazorwireLeft", "MinigunBarrel", new Vector3(0, 0, 0), new Vector3(270, 0, 0), new Vector3(0.02f, 0.02f, 0.025f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("LunarPrimaryReplacement", "DisplayBirdEye", "Head", new Vector3(-0.003f, 0.0025f, 0), new Vector3(0, 0, 270), new Vector3(0.01f, 0.01f, 0.01f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("NovaOnLowHealth", "DisplayJellyGuts", "Chest", new Vector3(0, 0.011f, 0), new Vector3(310, 270, 0), new Vector3(0.005f, 0.005f, 0.005f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("LunarTrinket", "DisplayBeads", "ElbowL", new Vector3(0.002f, 0.004f, 0.0015f), new Vector3(0, 90, 90), new Vector3(0.04f, 0.04f, 0.04f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("Plant", "DisplayInterstellarDeskPlant", "Hammer", new Vector3(0, 0.002f, 0), new Vector3(270, 90, 0), new Vector3(0.02f, 0.0175f, 0.0175f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("DeathMark", "DisplayDeathMark", "HandL", new Vector3(0, 0.004f, 0), new Vector3(270, 90, 0), new Vector3(0.002f, 0.002f, 0.002f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("CooldownOnCrit", "DisplaySkull", "HandR", new Vector3(0, 0.005f, 0), new Vector3(270, 90, 0), new Vector3(0.0085f, 0.0085f, 0.01f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("UtilitySkillMagazine", "DisplayAfterburnerShoulderRing", "Hammer", new Vector3(0, 0.01f, 0.002f), new Vector3(90, 0, 0), new Vector3(0.035f, 0.035f, 0.035f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("ExplodeOnDeath", "DisplayWilloWisp", "Pelvis", new Vector3(0, 0, -0.01f), new Vector3(0, 0, 180), new Vector3(0.0025f, 0.0025f, 0.0025f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("Seed", "DisplaySeed", "ElbowR", new Vector3(0, 0.005f, 0), new Vector3(270, 0, 0), new Vector3(0.0025f, 0.0025f, 0.0025f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("Phasing", "DisplayStealthkit", "LegL", new Vector3(-0.004f, 0.01f, 0), new Vector3(90, 90, 0), new Vector3(0.01f, 0.01f, 0.01f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("ShockNearby", "DisplayTeslaCoil", "Chest", new Vector3(0.01f, 0.01f, 0), new Vector3(0, 0, 315), new Vector3(0.01f, 0.01f, 0.01f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("AlienHead", "DisplayAlienHead", "Hammer", new Vector3(0, -0.045f, 0), new Vector3(315, 0, 0), new Vector3(0.05f, 0.05f, 0.05f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("HeadHunter", "DisplaySkullCrown", "Head", new Vector3(0, 0.0075f, 0), new Vector3(0, 270, 0), new Vector3(0.015f, 0.005f, 0.005f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("EnergizedOnEquipmentUse", "DisplayWarHorn", "Pelvis", new Vector3(0.006f, 0.002f, 0.006f), new Vector3(0, 190, 90), new Vector3(0.015f, 0.015f, 0.015f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("Tooth", "DisplayToothMeshLarge", "Chest", new Vector3(0, 0.012f, 0.005f), new Vector3(290, 0, 0), new Vector3(0.3f, 0.3f, 0.3f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("Pearl", "DisplayPearl", "HandL", new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0.005f, 0.005f, 0.005f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("ShinyPearl", "DisplayShinyPearl", "HandR", new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0.005f, 0.005f, 0.005f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("BonusGoldPackOnKill", "DisplayTome", "Pelvis", new Vector3(-0.005f, 0, -0.005f), new Vector3(0, 235, 0), new Vector3(0.0025f, 0.0025f, 0.0025f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("Squid", "DisplaySquidTurret", "Chest", new Vector3(0.012f, 0.01f, 0.004f), new Vector3(0, 0, 290), new Vector3(0.0025f, 0.0025f, 0.0025f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("LaserTurbine", "DisplayLaserTurbine", "Minigun", new Vector3(0.006f, 0.006f, 0), new Vector3(0, 90, 0), new Vector3(0.015f, 0.015f, 0.015f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("Incubator", "DisplayAncestralIncubator", "Hammer", new Vector3(0, 0.012f, 0), new Vector3(90, 0, 0), new Vector3(0.0035f, 0.0035f, 0.0035f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("SiphonOnLowHealth", "DisplaySiphonOnLowHealth", "Pelvis", new Vector3(-0.006f, 0.004f, 0.006f), new Vector3(0, 315, 180), new Vector3(0.0035f, 0.0035f, 0.0035f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("BleedOnHitAndExplode", "DisplayBleedOnHitAndExplode", "LegR", new Vector3(0.005f, 0.005f, 0), new Vector3(0, 0, 0), new Vector3(0.002f, 0.002f, 0.002f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("MonstersOnShrineUse", "DisplayMonstersOnShrineUse", "LegL", new Vector3(-0.005f, 0.005f, 0.002f), new Vector3(90, 0, 0), new Vector3(0.002f, 0.002f, 0.002f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("RandomDamageZone", "DisplayRandomDamageZone", "HandL", new Vector3(0.002f, 0.005f, 0.001f), new Vector3(0, 270, 270), new Vector3(0.002f, 0.002f, 0.002f)));

            equipmentRules.Add(NemItemDisplays.CreateGenericDisplayRule("Jetpack", "DisplayBugWings", "Chest", new Vector3(0.008f, 0.008f, 0), new Vector3(0, 270, 0), new Vector3(0.005f, 0.005f, 0.005f)));
            equipmentRules.Add(NemItemDisplays.CreateGenericDisplayRule("GoldGat", "DisplayGoldGat", "Chest", new Vector3(0.003f, 0.007f, 0), new Vector3(0, 0, 0), new Vector3(0.002f, 0.002f, 0.002f)));
            equipmentRules.Add(NemItemDisplays.CreateGenericDisplayRule("BFG", "DisplayBFG", "Chest", new Vector3(0, 0.012f, -0.006f), new Vector3(15, 270, 25), new Vector3(0.01f, 0.01f, 0.01f)));
            equipmentRules.Add(NemItemDisplays.CreateGenericDisplayRule("QuestVolatileBattery", "DisplayBatteryArray", "Chest", new Vector3(0.012f, 0.012f, 0), new Vector3(315, 90, 0), new Vector3(0.01f, 0.01f, 0.01f)));
            equipmentRules.Add(NemItemDisplays.CreateGenericDisplayRule("CommandMissile", "DisplayMissileRack", "Chest", new Vector3(0.01f, 0.01f, 0), new Vector3(90, 90, 0), new Vector3(0.02f, 0.02f, 0.02f)));
            equipmentRules.Add(NemItemDisplays.CreateGenericDisplayRule("Fruit", "DisplayFruit", "Chest", new Vector3(0, 0, 0), new Vector3(0, 150, 0), new Vector3(0.01f, 0.01f, 0.01f)));
            equipmentRules.Add(NemItemDisplays.CreateGenericDisplayRule("AffixWhite", "DisplayEliteIceCrown", "Head", new Vector3(0, 0.012f, 0), new Vector3(270, 270, 0), new Vector3(0.001f, 0.001f, 0.001f)));
            equipmentRules.Add(NemItemDisplays.CreateGenericDisplayRule("AffixPoison", "DisplayEliteUrchinCrown", "Head", new Vector3(0, 0.008f, 0), new Vector3(270, 0, 0), new Vector3(0.002f, 0.002f, 0.002f)));
            equipmentRules.Add(NemItemDisplays.CreateGenericDisplayRule("AffixHaunted", "DisplayEliteStealthCrown", "Head", new Vector3(0, 0.008f, 0), new Vector3(270, 0, 0), new Vector3(0.002f, 0.002f, 0.002f)));
            equipmentRules.Add(NemItemDisplays.CreateGenericDisplayRule("CritOnUse", "DisplayNeuralImplant", "Head", new Vector3(-0.008f, 0.006f, 0), new Vector3(0, 90, 0), new Vector3(0.01f, 0.01f, 0.01f)));
            equipmentRules.Add(NemItemDisplays.CreateGenericDisplayRule("DroneBackup", "DisplayRadio", "Pelvis", new Vector3(0, 0, 0.008f), new Vector3(0, 0, 180), new Vector3(0.01f, 0.01f, 0.01f)));
            equipmentRules.Add(NemItemDisplays.CreateGenericDisplayRule("Lightning", ItemDisplays.capacitorPrefab, "ClavicleL", new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0.04f, 0.04f, 0.04f)));
            equipmentRules.Add(NemItemDisplays.CreateGenericDisplayRule("BurnNearby", "DisplayPotion", "Pelvis", new Vector3(0, 0, 0.008f), new Vector3(0, 0, 180), new Vector3(0.002f, 0.002f, 0.002f)));
            equipmentRules.Add(NemItemDisplays.CreateGenericDisplayRule("CrippleWard", "DisplayEffigy", "Pelvis", new Vector3(0, 0.008f, 0.009f), new Vector3(0, 180, 180), new Vector3(0.02f, 0.02f, 0.02f)));
            equipmentRules.Add(NemItemDisplays.CreateGenericDisplayRule("GainArmor", "DisplayElephantFigure", "KneeR", new Vector3(0.004f, 0.012f, 0), new Vector3(90, 90, 0), new Vector3(0.02f, 0.02f, 0.02f)));
            equipmentRules.Add(NemItemDisplays.CreateGenericDisplayRule("Recycle", "DisplayRecycler", "Chest", new Vector3(0.012f, 0.012f, 0), new Vector3(0, 0, 0), new Vector3(0.005f, 0.005f, 0.005f)));
            equipmentRules.Add(NemItemDisplays.CreateGenericDisplayRule("FireBallDash", "DisplayEgg", "Pelvis", new Vector3(0, 0, 0.008f), new Vector3(90, 0, 0), new Vector3(0.01f, 0.01f, 0.01f)));
            equipmentRules.Add(NemItemDisplays.CreateGenericDisplayRule("Cleanse", "DisplayWaterPack", "Chest", new Vector3(0.012f, 0.01f, 0), new Vector3(315, 90, 0), new Vector3(0.005f, 0.005f, 0.005f)));
            equipmentRules.Add(NemItemDisplays.CreateGenericDisplayRule("Tonic", "DisplayTonic", "Pelvis", new Vector3(0, 0, 0.008f), new Vector3(0, 0, 180), new Vector3(0.01f, 0.01f, 0.01f)));
            equipmentRules.Add(NemItemDisplays.CreateGenericDisplayRule("Gateway", "DisplayVase", "Pelvis", new Vector3(0, 0, 0.009f), new Vector3(0, 0, 180), new Vector3(0.01f, 0.01f, 0.01f)));
            equipmentRules.Add(NemItemDisplays.CreateGenericDisplayRule("Scanner", "DisplayScanner", "Pelvis", new Vector3(0, 0.005f, 0.008f), new Vector3(90, 270, 0), new Vector3(0.005f, 0.005f, 0.005f)));
            equipmentRules.Add(NemItemDisplays.CreateGenericDisplayRule("DeathProjectile", "DisplayDeathProjectile", "Pelvis", new Vector3(-0.0012f, 0.005f, 0), new Vector3(0, 270, 180), new Vector3(0.005f, 0.005f, 0.005f)));
            equipmentRules.Add(NemItemDisplays.CreateGenericDisplayRule("LifestealOnHit", "DisplayLifestealOnHit", "Head", new Vector3(0.002f, 0.02f, -0.005f), new Vector3(90, 180, 0), new Vector3(0.01f, 0.01f, 0.01f)));
            equipmentRules.Add(NemItemDisplays.CreateGenericDisplayRule("TeamWarCry", "DisplayTeamWarCry", "Pelvis", new Vector3(0.01f, 0.003f, 0), new Vector3(10, 90, 0), new Vector3(0.0035f, 0.0035f, 0.0035f)));

            itemRules.Add(NemItemDisplays.CreateFollowerDisplayRule("Icicle", "DisplayFrostRelic", new Vector3(0.035f, 0.03f, 0.04f), new Vector3(0, 0, 90), new Vector3(2, 2, 2)));
            itemRules.Add(NemItemDisplays.CreateFollowerDisplayRule("Talisman", "DisplayTalisman", new Vector3(-0.015f, 0.03f, 0.05f), new Vector3(0, 270, 0), new Vector3(1, 1, 1)));
            itemRules.Add(NemItemDisplays.CreateFollowerDisplayRule("FocusConvergence", "DisplayFocusedConvergence", new Vector3(0.035f, 0.01f, 0.03f), new Vector3(0, 0, 0), new Vector3(0.2f, 0.2f, 0.2f)));

            equipmentRules.Add(NemItemDisplays.CreateFollowerDisplayRule("Saw", "DisplaySawmerang", new Vector3(0.06f, 0.02f, 0), new Vector3(90, 0, 0), new Vector3(0.25f, 0.25f, 0.25f)));
            equipmentRules.Add(NemItemDisplays.CreateFollowerDisplayRule("Meteor", "DisplayMeteor", new Vector3(0.05f, 0.02f, 0), new Vector3(90, 0, 0), new Vector3(1, 1, 1)));
            equipmentRules.Add(NemItemDisplays.CreateFollowerDisplayRule("Blackhole", "DisplayGravCube", new Vector3(0.05f, 0.02f, 0), new Vector3(90, 0, 0), new Vector3(1, 1, 1)));

            //weird rules here
            #region weirdshit
            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
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
                            localPos = new Vector3(0, 0.005f, 0.002f),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0.015f, 0.015f, 0.015f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayAntler"),
                            childName = "Head",
                            localPos = new Vector3(0, 0.005f, -0.002f),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0.015f, 0.015f, -0.015f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            equipmentRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
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
                            localPos = new Vector3(0, 0.0065f, 0.003f),
                            localAngles = new Vector3(0, 270, 0),
                            localScale = new Vector3(0.005f, 0.005f, 0.005f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteHorn"),
                            childName = "Head",
                            localPos = new Vector3(0, 0.0065f, -0.003f),
                            localAngles = new Vector3(0, 90, 0),
                            localScale = new Vector3(0.005f, 0.005f, -0.005f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            equipmentRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
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
                            localPos = new Vector3(-0.003f, 0.006f, 0),
                            localAngles = new Vector3(315, 270, 0),
                            localScale = new Vector3(0.01f, 0.01f, 0.01f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteRhinoHorn"),
                            childName = "Head",
                            localPos = new Vector3(-0.002f, 0.008f, 0),
                            localAngles = new Vector3(290, 270, 0),
                            localScale = new Vector3(0.005f, 0.005f, 0.005f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
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
                            localPos = new Vector3(0, 0.002f, 0),
                            localAngles = new Vector3(0, 90, 0),
                            localScale = new Vector3(0.01f, 0.01f, 0.01f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayShieldBug"),
                            childName = "Head",
                            localPos = new Vector3(0, 0.002f, 0),
                            localAngles = new Vector3(0, 270, 0),
                            localScale = new Vector3(0.01f, 0.01f, -0.01f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });


            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
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
                            childName = "FootR",
                            localPos = new Vector3(0, 0, 0),
                            localAngles = new Vector3(90, 270, 0),
                            localScale = new Vector3(0.0075f, 0.0075f, 0.0075f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGravBoots"),
                            childName = "FootL",
                            localPos = new Vector3(0, 0, 0),
                            localAngles = new Vector3(90, 270, 0),
                            localScale = new Vector3(0.0075f, 0.0075f, 0.0075f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
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
                            localPos = new Vector3(0, 0.003f, 0),
                            localAngles = new Vector3(0, 270, 20),
                            localScale = new Vector3(0.025f, 0.025f, 0.025f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDevilHorns"),
                            childName = "Head",
                            localPos = new Vector3(0, 0.003f, 0),
                            localAngles = new Vector3(0, 90, 20),
                            localScale = new Vector3(0.025f, 0.025f, -0.025f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });
            #endregion

            #region Aetherium
            if (EnforcerPlugin.aetheriumInstalled)
            {
                itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("ATHRMAccursedPotion", ItemDisplays.LoadAetheriumDisplay("AccursedPotion"), "Pelvis", new Vector3(0.002f, 0, 0.008f), new Vector3(0, 0, 180), new Vector3(0.02f, 0.02f, 0.02f)));
                itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("ATHRMVoidheart", ItemDisplays.LoadAetheriumDisplay("VoidHeart"), "Chest", new Vector3(0, 0.005f, 0), new Vector3(0, 0, 0), new Vector3(0.004f, 0.004f, 0.004f)));
                itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("ATHRMSharkTeeth", ItemDisplays.LoadAetheriumDisplay("SharkTeeth"), "KneeL", new Vector3(0, 0.01f, 0), new Vector3(0, 0, 310), new Vector3(0.02f, 0.02f, 0.01f)));
                itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("ATHRMBloodSoakedShield", ItemDisplays.LoadAetheriumDisplay("BloodSoakedShield"), "ElbowL", new Vector3(0.004f, 0.005f, 0.001f), new Vector3(0, 90, 90), new Vector3(0.01f, 0.01f, 0.01f)));
                itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("ATHRMFeatheredPlume", ItemDisplays.LoadAetheriumDisplay("FeatheredPlume"), "Head", new Vector3(0.003f, 0.006f, 0), new Vector3(0, 270, 0), new Vector3(0.02f, 0.02f, 0.02f)));
                itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("ATHRMShieldingCore", ItemDisplays.LoadAetheriumDisplay("ShieldingCore"), "Chest", new Vector3(-0.008f, 0.001f, 0), new Vector3(0, 0, 0), new Vector3(0.005f, 0.005f, 0.005f)));
                itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("ATHRMUnstableDesign", ItemDisplays.LoadAetheriumDisplay("UnstableDesign"), "Chest", new Vector3(0.012f, 0.01f, 0), new Vector3(0, 315, 0), new Vector3(0.02f, 0.02f, 0.02f)));
                itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("ATHRMWeightedAnklet", ItemDisplays.LoadAetheriumDisplay("WeightedAnklet"), "KneeR", new Vector3(0, 0.005f, 0), new Vector3(0, 0, 0), new Vector3(0.01f, 0.01f, 0.01f)));
                itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("ATHRMBlasterSword", ItemDisplays.LoadAetheriumDisplay("BlasterSword"), "Hammer", new Vector3(0.006f, 0.005f, 0.001f), new Vector3(10, 90, 180), new Vector3(0.005f, 0.005f, 0.005f)));
                itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("ATHRMWitchesRing", ItemDisplays.LoadAetheriumDisplay("WitchesRing"), "Hammer", new Vector3(0, 0.011f, 0), new Vector3(270, 0, 0), new Vector3(0.035f, 0.035f, 0.035f)));
                itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("ATHRMAccursedPotion", ItemDisplays.LoadAetheriumDisplay("AccursedPotion"), "Pelvis", new Vector3(0.002f, 0.005f, 0.001f), new Vector3(0, 270, 270), new Vector3(0.002f, 0.002f, 0.002f)));

                itemRules.Add(NemItemDisplays.CreateFollowerDisplayRule("ATHRMAlienMagnet", ItemDisplays.LoadAetheriumDisplay("AlienMagnet"), new Vector3(0.04f, 0.01f, -0.02f), new Vector3(0, 0, 0), new Vector3(0.005f, 0.005f, 0.005f)));
                itemRules.Add(NemItemDisplays.CreateFollowerDisplayRule("ATHRMInspiringDrone", ItemDisplays.LoadAetheriumDisplay("InspiringDrone"), new Vector3(-0.07f, 0.03f, -0.07f), new Vector3(0, 90, 0), new Vector3(0.005f, 0.005f, 0.005f)));
            }
            #endregion
            #region SivsItems
            if (EnforcerPlugin.sivsItemsInstalled)
            {
                itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("BeetlePlush", ItemDisplays.LoadSivDisplay("BeetlePlush"), "Chest", new Vector3(0.01f, 0.016f, 0), new Vector3(0, 270, 0), new Vector3(0.02f, 0.02f, 0.02f)));
                itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("BisonShield", ItemDisplays.LoadSivDisplay("BisonShield"), "ElbowR", new Vector3(-0.002f, 0.005f, 0.001f), new Vector3(0, 270, 0), new Vector3(0.02f, 0.02f, 0.02f)));
                itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("FlameGland", ItemDisplays.LoadSivDisplay("FlameGland"), "Minigun", new Vector3(0, 0.02f, -0.01f), new Vector3(0, 0, 180), new Vector3(0.02f, 0.02f, 0.02f)));
                itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("Geode", ItemDisplays.LoadSivDisplay("Geode"), "Hammer", new Vector3(0, 0.012f, 0.013f), new Vector3(0, 90, 270), new Vector3(0.01f, 0.01f, 0.01f)));
                itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("Tarbine", ItemDisplays.LoadSivDisplay("Tarbine"), "Minigun", new Vector3(0, 0.006f, -0.01f), new Vector3(270, 0, 0), new Vector3(0.03f, 0.03f, 0.03f)));
                itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("Tentacle", ItemDisplays.LoadSivDisplay("Tentacle"), "Head", new Vector3(0.002f, 0.01f, 0.001f), new Vector3(0, 270, 270), new Vector3(0.02f, 0.02f, 0.02f)));
                itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("ImpEye", ItemDisplays.LoadSivDisplay("ImpEye"), "Head", new Vector3(-0.003f, 0.003f, 0), new Vector3(0, 270, 0), new Vector3(0.02f, 0.02f, 0.02f)));

                itemRules.Add(NemItemDisplays.CreateFollowerDisplayRule("NullSeed", ItemDisplays.LoadSivDisplay("NullSeed"), new Vector3(0.02f, 0.05f, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1)));
            }
            #endregion
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
        public static ItemDisplayRuleSet.NamedRuleGroup CreateGenericDisplayRule(string itemName, GameObject itemPrefab, string childName, Vector3 position, Vector3 rotation, Vector3 scale)
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
        public static ItemDisplayRuleSet.NamedRuleGroup CreateFollowerDisplayRule(string itemName, GameObject itemPrefab, Vector3 position, Vector3 rotation, Vector3 scale)
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
