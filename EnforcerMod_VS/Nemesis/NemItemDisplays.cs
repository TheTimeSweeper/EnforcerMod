using Modules;
using RoR2;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace EnforcerPlugin {

    public static class NemItemDisplays
    {
        public static List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemRules;

        public static void RegisterDisplays()
        {
            GameObject bodyPrefab = NemforcerPlugin.characterBodyPrefab;

            GameObject model = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = model.GetComponent<CharacterModel>();

            ItemDisplayRuleSet itemDisplayRuleSet = ScriptableObject.CreateInstance<ItemDisplayRuleSet>();

            itemRules = new List<ItemDisplayRuleSet.KeyAssetRuleGroup>();

            #region Display Rules

            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("CritGlasses", "DisplayGlasses", "Head", new Vector3(-0.003f, 0.008f, 0f), new Vector3(335, 270, 0), new Vector3(0.01f, 0.01f, 0.01f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("Syringe", "DisplaySyringeCluster", "Chest", new Vector3(0, 0.012f, 0.006f), new Vector3(25, 315, 0), new Vector3(0.005f, 0.005f, 0.005f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("NearbyDamageBonus", "DisplayDiamond", "Hammer", new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0.005f, 0.005f, 0.005f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("ArmorReductionOnHit", "DisplayWarhammer", "Hammer", new Vector3(0, 0.005f, 0), new Vector3(270, 90, 0), new Vector3(0.0175f, 0.0175f, 0.0175f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("SecondarySkillMagazine", "DisplayDoubleMag", "Hammer", new Vector3(0, 0.016f, -0.001f), new Vector3(70, 0, 180), new Vector3(0.005f, 0.005f, 0.005f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("Bear", "DisplayBear", "Hammer", new Vector3(0, 0.012f, 0.012f), new Vector3(0, 0, 0), new Vector3(0.0175f, 0.0175f, 0.0175f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("BearVoid", "DisplayBearVoid", "Hammer", new Vector3(0, 0.012f, 0.012f), new Vector3(0, 0, 0), new Vector3(0.0175f, 0.0175f, 0.0175f)));
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
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("ExtraLifeVoid", "DisplayHippoVoid", "Hammer", new Vector3(0, 0.02f, 0.014f), new Vector3(0, 0, 0), new Vector3(0.02f, 0.02f, 0.02f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("CritGlassesVoid", "DisplayGlassesVoid", "Head", new Vector3(-0.003f, 0.008f, 0f), new Vector3(335, 270, 0), new Vector3(0.01f, 0.01f, 0.01f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("MissileVoid", "DisplayMissileLauncherVoid", "Chest", new Vector3(0, 0.03f, 0), new Vector3(0, 270, 0), new Vector3(0.005f, 0.005f, 0.005f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("KillEliteFrenzy", "DisplayBrainstalk", "Head", new Vector3(0.001f, 0.007f, 0), new Vector3(0, 90, 0), new Vector3(0.006f, 0.006f, 0.006f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("RepeatHeal", "DisplayCorpseFlower", "ClavicleR", new Vector3(0.005f, 0.01f, 0), new Vector3(0, 25, 300), new Vector3(0.01f, 0.01f, 0.01f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("AutoCastEquipment", "DisplayFossil", "Pelvis", new Vector3(-0.009f, 0.002f, 0), new Vector3(0, 0, 0), new Vector3(0.02f, 0.02f, 0.02f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("TitanGoldDuringTP", "DisplayGoldHeart", "Chest", new Vector3(-0.005f, 0.002f, -0.005f), new Vector3(0, 235, 0), new Vector3(0.0085f, 0.0085f, 0.0085f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("SprintWisp", "DisplayBrokenMask", "ShoulderL", new Vector3(0.005f, 0.003f, 0), new Vector3(0, 90, 180), new Vector3(0.01f, 0.01f, 0.01f)));
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
            
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRuleE("Jetpack", "DisplayBugWings", "Chest", new Vector3(0.008f, 0.008f, 0), new Vector3(0, 270, 0), new Vector3(0.005f, 0.005f, 0.005f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRuleE("GoldGat", "DisplayGoldGat", "Chest", new Vector3(0.003f, 0.007f, 0), new Vector3(0, 0, 0), new Vector3(0.002f, 0.002f, 0.002f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRuleE("BFG", "DisplayBFG", "Chest", new Vector3(0, 0.012f, -0.006f), new Vector3(15, 270, 25), new Vector3(0.01f, 0.01f, 0.01f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRuleE("QuestVolatileBattery", "DisplayBatteryArray", "Chest", new Vector3(0.012f, 0.012f, 0), new Vector3(315, 90, 0), new Vector3(0.01f, 0.01f, 0.01f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRuleE("CommandMissile", "DisplayMissileRack", "Chest", new Vector3(0.01f, 0.01f, 0), new Vector3(90, 90, 0), new Vector3(0.02f, 0.02f, 0.02f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRuleE("Fruit", "DisplayFruit", "Chest", new Vector3(0, 0, 0), new Vector3(0, 150, 0), new Vector3(0.01f, 0.01f, 0.01f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRuleE("AffixWhite", "DisplayEliteIceCrown", "Head", new Vector3(0, 0.012f, 0), new Vector3(270, 270, 0), new Vector3(0.001f, 0.001f, 0.001f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRuleE("AffixPoison", "DisplayEliteUrchinCrown", "Head", new Vector3(0, 0.008f, 0), new Vector3(270, 0, 0), new Vector3(0.002f, 0.002f, 0.002f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRuleE("AffixHaunted", "DisplayEliteStealthCrown", "Head", new Vector3(0, 0.008f, 0), new Vector3(270, 0, 0), new Vector3(0.002f, 0.002f, 0.002f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRuleE("CritOnUse", "DisplayNeuralImplant", "Head", new Vector3(-0.008f, 0.006f, 0), new Vector3(0, 90, 0), new Vector3(0.01f, 0.01f, 0.01f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRuleE("DroneBackup", "DisplayRadio", "Pelvis", new Vector3(0, 0, 0.008f), new Vector3(0, 0, 180), new Vector3(0.01f, 0.01f, 0.01f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRuleE("Lightning", "DisplayLightningArmNem", "ClavicleL", new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0.04f, 0.04f, 0.04f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRuleE("BurnNearby", "DisplayPotion", "Pelvis", new Vector3(0, 0, 0.008f), new Vector3(0, 0, 180), new Vector3(0.002f, 0.002f, 0.002f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRuleE("CrippleWard", "DisplayEffigy", "Pelvis", new Vector3(0, 0.008f, 0.009f), new Vector3(0, 180, 180), new Vector3(0.02f, 0.02f, 0.02f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRuleE("GainArmor", "DisplayElephantFigure", "KneeR", new Vector3(0.004f, 0.012f, 0), new Vector3(90, 90, 0), new Vector3(0.02f, 0.02f, 0.02f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRuleE("Recycle", "DisplayRecycler", "Chest", new Vector3(0.012f, 0.012f, 0), new Vector3(0, 0, 0), new Vector3(0.005f, 0.005f, 0.005f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRuleE("FireBallDash", "DisplayEgg", "Pelvis", new Vector3(0, 0, 0.008f), new Vector3(90, 0, 0), new Vector3(0.01f, 0.01f, 0.01f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRuleE("Cleanse", "DisplayWaterPack", "Chest", new Vector3(0.012f, 0.01f, 0), new Vector3(315, 90, 0), new Vector3(0.005f, 0.005f, 0.005f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRuleE("Tonic", "DisplayTonic", "Pelvis", new Vector3(0, 0, 0.008f), new Vector3(0, 0, 180), new Vector3(0.01f, 0.01f, 0.01f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRuleE("Gateway", "DisplayVase", "Pelvis", new Vector3(0, 0, 0.009f), new Vector3(0, 0, 180), new Vector3(0.01f, 0.01f, 0.01f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRuleE("Scanner", "DisplayScanner", "Pelvis", new Vector3(0, 0.005f, 0.008f), new Vector3(90, 270, 0), new Vector3(0.005f, 0.005f, 0.005f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRuleE("DeathProjectile", "DisplayDeathProjectile", "Pelvis", new Vector3(-0.0012f, 0.005f, 0), new Vector3(0, 270, 180), new Vector3(0.005f, 0.005f, 0.005f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRuleE("LifestealOnHit", "DisplayLifestealOnHit", "Head", new Vector3(0.002f, 0.015f, 0.0075f), new Vector3(45, 180, 0), new Vector3(0.005f, 0.005f, 0.005f)));
            itemRules.Add(NemItemDisplays.CreateGenericDisplayRuleE("TeamWarCry", "DisplayTeamWarCry", "Pelvis", new Vector3(0.01f, 0.003f, 0), new Vector3(10, 90, 0), new Vector3(0.0035f, 0.0035f, 0.0035f)));

            itemRules.Add(NemItemDisplays.CreateFollowerDisplayRuleE("Icicle", "DisplayFrostRelic", new Vector3(0.035f, 0.03f, 0.04f), new Vector3(0, 0, 90), new Vector3(2, 2, 2)));
            itemRules.Add(NemItemDisplays.CreateFollowerDisplayRuleE("Talisman", "DisplayTalisman", new Vector3(-0.015f, 0.03f, 0.05f), new Vector3(0, 270, 0), new Vector3(1, 1, 1)));
            itemRules.Add(NemItemDisplays.CreateFollowerDisplayRuleE("FocusConvergence", "DisplayFocusedConvergence", new Vector3(0.035f, 0.01f, 0.03f), new Vector3(0, 0, 0), new Vector3(0.2f, 0.2f, 0.2f)));

            itemRules.Add(NemItemDisplays.CreateFollowerDisplayRuleE("Saw", "DisplaySawmerangFollower", new Vector3(0.00698F, 0.01826F, 0.03111F), new Vector3(90F, 0F, 0F), new Vector3(0.00387F, 0.00387F, 0.00387F)));
            itemRules.Add(NemItemDisplays.CreateFollowerDisplayRuleE("Meteor", "DisplayMeteor", new Vector3(0.05f, 0.02f, 0), new Vector3(90, 0, 0), new Vector3(1, 1, 1)));
            itemRules.Add(NemItemDisplays.CreateFollowerDisplayRuleE("Blackhole", "DisplayGravCube", new Vector3(0.05f, 0.02f, 0), new Vector3(90, 0, 0), new Vector3(1, 1, 1)));

            //weird rules here
            #region weirdshit
            itemRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/IncreaseHealing"),
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

            itemRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<EquipmentDef>("EquipmentDefs/AffixRed"),
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

            itemRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<EquipmentDef>("EquipmentDefs/AffixBlue"),
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

            itemRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/ShieldOnly"),
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


            itemRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/FallBoots"),
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

            itemRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/NovaOnHeal"),
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
            /*
            #region Aetherium
            if (EnforcerPlugin.aetheriumInstalled) 
            {
                itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("ITEM_ACCURSED_POTION", ItemDisplays.LoadAetheriumDisplay("AccursedPotion"), "Pelvis", new Vector3(0.002f, 0, 0.008f), new Vector3(0, 0, 180), new Vector3(0.02f, 0.02f, 0.02f)));
                itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("ITEM_VOID_HEART", ItemDisplays.LoadAetheriumDisplay("VoidHeart"), "Chest", new Vector3(0, 0.005f, 0), new Vector3(0, 0, 0), new Vector3(0.004f, 0.004f, 0.004f)));
                itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("ITEM_SHARK_TEETH", ItemDisplays.LoadAetheriumDisplay("SharkTeeth"), "KneeL", new Vector3(0, 0.01f, 0), new Vector3(0, 0, 310), new Vector3(0.02f, 0.02f, 0.01f)));
                itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("ITEM_BLOOD_SOAKED_SHIELD", ItemDisplays.LoadAetheriumDisplay("BloodSoakedShield"), "ElbowL", new Vector3(0.004f, 0.005f, 0.001f), new Vector3(0, 90, 90), new Vector3(0.01f, 0.01f, 0.01f)));
                itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("ITEM_FEATHERED_PLUME", ItemDisplays.LoadAetheriumDisplay("FeatheredPlume"), "Head", new Vector3(0.003f, 0.006f, 0), new Vector3(0, 270, 0), new Vector3(0.02f, 0.02f, 0.02f)));
                itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("ITEM_SHIELDING_CORE", ItemDisplays.LoadAetheriumDisplay("ShieldingCore"), "Chest", new Vector3(-0.008f, 0.001f, 0), new Vector3(0, 0, 0), new Vector3(0.005f, 0.005f, 0.005f)));
                itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("ITEM_UNSTABLE_DESIGN", ItemDisplays.LoadAetheriumDisplay("UnstableDesign"), "Chest", new Vector3(0.012f, 0.01f, 0), new Vector3(0, 315, 0), new Vector3(0.02f, 0.02f, 0.02f)));
                itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("ITEM_WEIGHTED_ANKLET", ItemDisplays.LoadAetheriumDisplay("WeightedAnklet"), "KneeR", new Vector3(0, 0.005f, 0), new Vector3(0, 0, 0), new Vector3(0.01f, 0.01f, 0.01f)));
                itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("ITEM_BLASTER_SWORD", ItemDisplays.LoadAetheriumDisplay("BlasterSword"), "Hammer", new Vector3(0.006f, 0.005f, 0.001f), new Vector3(10, 90, 180), new Vector3(0.005f, 0.005f, 0.005f)));
                itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("ITEM_WITCHES_RING", ItemDisplays.LoadAetheriumDisplay("WitchesRing"), "Hammer", new Vector3(0, 0.011f, 0), new Vector3(270, 0, 0), new Vector3(0.035f, 0.035f, 0.035f)));

                itemRules.Add(NemItemDisplays.CreateFollowerDisplayRule("ITEM_ALIEN_MAGNET", ItemDisplays.LoadAetheriumDisplay("AlienMagnet"), new Vector3(0.04f, 0.01f, -0.02f), new Vector3(0, 0, 0), new Vector3(0.005f, 0.005f, 0.005f)));
                itemRules.Add(NemItemDisplays.CreateFollowerDisplayRule("ITEM_INSPIRING_DRONE", ItemDisplays.LoadAetheriumDisplay("InspiringDrone"), new Vector3(-0.07f, 0.03f, -0.07f), new Vector3(0, 90, 0), new Vector3(0.005f, 0.005f, 0.005f)));

                itemRules.Add(NemItemDisplays.CreateFollowerDisplayRule("EQUIPMENT_JAR_OF_RESHAPING", ItemDisplays.LoadAetheriumDisplay("JarOfReshaping"), new Vector3(0.04f, 0.04f, 0), new Vector3(0, 270, 270), new Vector3(0.003f, 0.003f, 0.003f)));
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
            #region SupplyDrop
            if (EnforcerPlugin.supplyDropInstalled)
            {
                itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("SUPPDRPElectroPlankton", ItemDisplays.LoadSupplyDropDisplay("ElectroPlankton"), "Chest", new Vector3(0.014f, 0.0085f, 0), new Vector3(90, 0, 0), new Vector3(0.004f, 0.004f, 0.004f)));
                itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("SUPPDRPHardenedBoneFragments", ItemDisplays.LoadSupplyDropDisplay("HardenedBoneFragments"), "Chest", new Vector3(-0.008f, 0, 0), new Vector3(0, 270, 0), new Vector3(0.075f, 0.075f, 0.075f)));
                itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("SUPPDRPQSGen", ItemDisplays.LoadSupplyDropDisplay("QSGen"), "ElbowL", new Vector3(0, 0.006f, 0), new Vector3(0, 0, 270), new Vector3(0.0035f, 0.0035f, 0.0035f)));
                itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("SUPPDRPSalvagedWires", ItemDisplays.LoadSupplyDropDisplay("SalvagedWires"), "Minigun", new Vector3(0.005f, 0.004f, -0.014f), new Vector3(0, 90, 0), new Vector3(0.02f, 0.02f, 0.02f)));
                itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("SUPPDRPShellPlating", ItemDisplays.LoadSupplyDropDisplay("ShellPlating"), "Pelvis", new Vector3(0.0095f, 0.006f, 0), new Vector3(30, 90, 0), new Vector3(0.0075f, 0.0075f, 0.0075f)));
                itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("SUPPDRPPlagueHat", ItemDisplays.LoadSupplyDropDisplay("PlagueHat"), "Head", new Vector3(1.896406E-07f, 0.008753167f, 1.76311E-07f), new Vector3(0, 0, 0), new Vector3(0.005486776f, 0.005486776f, 0.005486776f)));
                itemRules.Add(NemItemDisplays.CreateGenericDisplayRule("SUPPDRPPlagueMask", ItemDisplays.LoadSupplyDropDisplay("PlagueMask"), "Head", new Vector3(-0.007689702f, 0.004630757f, 2.040761E-07f), new Vector3(0, 90, 0), new Vector3(0.0075f, 0.0075f, 0.0075f)));

                itemRules.Add(NemItemDisplays.CreateFollowerDisplayRule("SUPPDRPBloodBook", ItemDisplays.LoadSupplyDropDisplay("BloodBook"), new Vector3(0, 0.05f, 0.04f), new Vector3(0, 270, 0), new Vector3(0.08f, 0.08f, 0.08f)));
            }
            #endregion*/
            #endregion

            ItemDisplayRuleSet.KeyAssetRuleGroup[] item = itemRules.ToArray();
            itemDisplayRuleSet.keyAssetRuleGroups = item;

            characterModel.itemDisplayRuleSet = itemDisplayRuleSet;
        }

        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateGenericDisplayRule(string itemName, string prefabName, string childName, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            ItemDef itemDef = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/" + itemName);
            if (itemDef == null)
                return new ItemDisplayRuleSet.KeyAssetRuleGroup();
            ItemDisplayRuleSet.KeyAssetRuleGroup displayRule = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = itemDef,
                displayRuleGroup = new DisplayRuleGroup {
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

        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateGenericDisplayRuleE(string itemName, string prefabName, string childName, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            EquipmentDef equipmentDef = RoR2.LegacyResourcesAPI.Load<EquipmentDef>("EquipmentDefs/" + itemName);
            if (equipmentDef == null)
                return new ItemDisplayRuleSet.KeyAssetRuleGroup();

            ItemDisplayRuleSet.KeyAssetRuleGroup displayRule = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = equipmentDef,
                displayRuleGroup = new DisplayRuleGroup {
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

        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateGenericDisplayRule(string itemName, GameObject itemPrefab, string childName, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            ItemDef itemDef = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/" + itemName);
            if (itemDef == null)
                return new ItemDisplayRuleSet.KeyAssetRuleGroup();
            ItemDisplayRuleSet.KeyAssetRuleGroup displayRule = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = itemDef,
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
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/" + itemName),
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

        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateFollowerDisplayRuleE(string itemName, string prefabName, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            ItemDisplayRuleSet.KeyAssetRuleGroup displayRule = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<EquipmentDef>("EquipmentDefs/" + itemName),
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

        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateFollowerDisplayRule(string itemName, GameObject itemPrefab, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            ItemDisplayRuleSet.KeyAssetRuleGroup displayRule = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/" + itemName),
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
                keyAsset = RoR2.LegacyResourcesAPI.Load<EquipmentDef>("EquipmentDefs/" + itemName),
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