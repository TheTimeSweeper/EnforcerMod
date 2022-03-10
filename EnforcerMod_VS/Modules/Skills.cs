using EntityStates;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Modules {

    public static class Skills
    {
        #region genericskills
        public static void CreateSkillFamilies(GameObject targetPrefab, int families = 15, bool destroyExisting = true) {

            if (destroyExisting) {
                foreach (GenericSkill obj in targetPrefab.GetComponentsInChildren<GenericSkill>()) {
                    UnityEngine.Object.DestroyImmediate(obj);
                }
            }

            SkillLocator skillLocator = targetPrefab.GetComponent<SkillLocator>();

            if ((families & (1 << 0)) != 0) {
                skillLocator.primary = CreateGenericSkillWithSkillFamily(targetPrefab, "Primary");
            }
            if ((families & (1 << 1)) != 0) {
                skillLocator.secondary = CreateGenericSkillWithSkillFamily(targetPrefab, "Secondary");
            }
            if ((families & (1 << 2)) != 0) {
                skillLocator.utility = CreateGenericSkillWithSkillFamily(targetPrefab, "Utility");
            }
            if ((families & (1 << 3)) != 0) {
                skillLocator.special = CreateGenericSkillWithSkillFamily(targetPrefab, "Special");
            }
        }

        public static GenericSkill CreateGenericSkillWithSkillFamily(GameObject targetPrefab, string familyName, bool hidden = false) {

            GenericSkill skill = targetPrefab.AddComponent<GenericSkill>();
            skill.skillName = familyName;
            skill.hideInCharacterSelect = hidden;

            SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
            (newFamily as ScriptableObject).name = targetPrefab.name + familyName + "SkillFamily";
            newFamily.variants = new SkillFamily.Variant[0];

            skill._skillFamily = newFamily;

            Modules.Content.AddSkillFamily(newFamily);
            return skill;
        }
        #endregion

        #region skillfamilies

        //everything calls this
        public static void AddSkillToFamily(SkillFamily skillFamily, SkillDef skillDef, UnlockableDef unlockableDef = null) {

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);

            if(string.IsNullOrEmpty((skillDef as ScriptableObject).name)) {
                (skillDef as ScriptableObject).name = skillDef.skillName;
            }


            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant {
                skillDef = skillDef,
                unlockableDef = unlockableDef,
                viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
            };

            Modules.Content.AddSkillDef(skillDef);
        }

        public static void AddSkillsToFamily (SkillFamily skillFamily, params SkillDef[] skillDefs) {

            foreach (SkillDef skillDef in skillDefs) {
                AddSkillToFamily(skillFamily, skillDef);
            }
        }
        public static void AddPrimarySkills(GameObject targetPrefab, params SkillDef[] skillDefs) {
            AddSkillsToFamily(targetPrefab.GetComponent<SkillLocator>().primary.skillFamily, skillDefs);
        }
        public static void AddSecondarySkills(GameObject targetPrefab, params SkillDef[] skillDefs) {
            AddSkillsToFamily(targetPrefab.GetComponent<SkillLocator>().secondary.skillFamily, skillDefs);
        }
        public static void AddUtilitySkills(GameObject targetPrefab, params SkillDef[] skillDefs) {
            AddSkillsToFamily(targetPrefab.GetComponent<SkillLocator>().utility.skillFamily, skillDefs);
        }
        public static void AddSpecialSkills(GameObject targetPrefab, params SkillDef[] skillDefs) {
            AddSkillsToFamily(targetPrefab.GetComponent<SkillLocator>().special.skillFamily, skillDefs);
        }

        /// <summary>
        /// pass in an amount of unlockables equal to or less than skill variants
        /// <code>
        /// AddUnlockablesToFamily(skillLocator.primary, null, skill2UnlockableDef, null, skill4UnlockableDef);
        /// </code>
        /// </summary>
        public static void AddUnlockablesToFamily(SkillFamily skillFamily, params UnlockableDef[] unlockableDefs) {

            for (int i = 0; i < unlockableDefs.Length; i++) {
                SkillFamily.Variant variant = skillFamily.variants[i];
                variant.unlockableDef = unlockableDefs[i];
            }
        }
        #endregion

        #region skilldefs
        public static SkillDef CreateSkillDef (SkillDefInfo skillDefInfo){

            SkillDef skillDef = ScriptableObject.CreateInstance<SkillDef>();

            popuplateSKillDef(skillDefInfo, skillDef);

            Modules.Content.AddSkillDef(skillDef);

            return skillDef;
        }

        public static T CreateSkillDef<T>(SkillDefInfo skillDefInfo) where T: SkillDef {

            T skillDef = ScriptableObject.CreateInstance<T>() ;

            popuplateSKillDef(skillDefInfo, skillDef);

            Modules.Content.AddSkillDef(skillDef);

            return skillDef;
        }

        private static void popuplateSKillDef(SkillDefInfo skillDefInfo, SkillDef skillDef) {
            skillDef.skillName = skillDefInfo.skillName;
            (skillDef as ScriptableObject).name = skillDefInfo.skillName;
            skillDef.skillNameToken = skillDefInfo.skillNameToken;
            skillDef.skillDescriptionToken = skillDefInfo.skillDescriptionToken;
            skillDef.icon = skillDefInfo.skillIcon;

            skillDef.activationState = skillDefInfo.activationState;
            skillDef.activationStateMachineName = skillDefInfo.activationStateMachineName;
            skillDef.baseMaxStock = skillDefInfo.baseMaxStock;
            skillDef.baseRechargeInterval = skillDefInfo.baseRechargeInterval;
            skillDef.beginSkillCooldownOnSkillEnd = skillDefInfo.beginSkillCooldownOnSkillEnd;
            skillDef.canceledFromSprinting = skillDefInfo.canceledFromSprinting;
            skillDef.forceSprintDuringState = skillDefInfo.forceSprintDuringState;
            skillDef.fullRestockOnAssign = skillDefInfo.fullRestockOnAssign;
            skillDef.interruptPriority = skillDefInfo.interruptPriority;
            skillDef.resetCooldownTimerOnUse = skillDefInfo.resetCooldownTimerOnUse;
            skillDef.isCombatSkill = skillDefInfo.isCombatSkill;
            skillDef.mustKeyPress = skillDefInfo.mustKeyPress;
            skillDef.cancelSprintingOnActivation = skillDefInfo.cancelSprintingOnActivation;
            skillDef.rechargeStock = skillDefInfo.rechargeStock;
            skillDef.requiredStock = skillDefInfo.requiredStock;
            skillDef.stockToConsume = skillDefInfo.stockToConsume;

            skillDef.keywordTokens = skillDefInfo.keywordTokens;
        }
        #endregion skilldefs
    }
}

/// <summary>
/// class for easily creating skilldefs with default values, and with a field for UnlockableDef
/// </summary>
public class SkillDefInfo { 

    public string skillName;
    public string skillNameToken;
    public string skillDescriptionToken;
    public string[] keywordTokens = new string[0];
    public Sprite skillIcon;

    public SerializableEntityStateType activationState;
    public InterruptPriority interruptPriority;
    public string activationStateMachineName;

    public float baseRechargeInterval;

    public int baseMaxStock = 1;
    public int rechargeStock = 1;
    public int requiredStock = 1;
    public int stockToConsume = 1;

    public bool isCombatSkill = true;
    public bool canceledFromSprinting;
    public bool forceSprintDuringState;
    public bool cancelSprintingOnActivation = true;

    public bool beginSkillCooldownOnSkillEnd;
    public bool fullRestockOnAssign = true;
    public bool resetCooldownTimerOnUse;
    public bool mustKeyPress;

    #region building
    public SkillDefInfo() { }

    public SkillDefInfo(string skillName, 
                          string skillNameToken, 
                          string skillDescriptionToken, 
                          Sprite skillIcon, 

                          SerializableEntityStateType activationState, 
                          string activationStateMachineName, 
                          InterruptPriority interruptPriority, 
                          bool isCombatSkill, 

                          float baseRechargeInterval) {
        this.skillName = skillName;
        this.skillNameToken = skillNameToken;
        this.skillDescriptionToken = skillDescriptionToken;
        this.skillIcon = skillIcon;
        this.activationState = activationState;
        this.activationStateMachineName = activationStateMachineName;
        this.interruptPriority = interruptPriority;
        this.isCombatSkill = isCombatSkill;
        this.baseRechargeInterval = baseRechargeInterval;
    }
    /// <summary>
    /// Creates a skilldef for a typical primary.
    /// <para>combat skill, cooldown: 0, required stock: 0, InterruptPriority: Any</para>
    /// </summary>
    public SkillDefInfo(string skillName,
                          string skillNameToken,
                          string skillDescriptionToken,
                          Sprite skillIcon,

                          SerializableEntityStateType activationState,
                          string activationStateMachineName = "Weapon",
                          bool agile = false) {

        this.skillName = skillName;
        this.skillNameToken = skillNameToken;
        this.skillDescriptionToken = skillDescriptionToken;
        this.skillIcon = skillIcon;

        this.activationState = activationState;
        this.activationStateMachineName = activationStateMachineName;

        this.interruptPriority = InterruptPriority.Any;
        this.isCombatSkill = true;
        this.baseRechargeInterval = 0;

        this.requiredStock = 0;
        this.stockToConsume = 0;

        this.cancelSprintingOnActivation = !agile;
    }
    #endregion construction complete
}