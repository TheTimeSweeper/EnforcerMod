using EntityStates;
using EntityStates.Enforcer;
using EntityStates.Enforcer.NeutralSpecial;
using Modules;
using R2API;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Modules.Characters {
    class EnforcerSkillDefs {

        #region skilldefs
        public static SkillDef PrimarySkillDef_RiotShotgun() {

            SkillDef skillDefRiotShotgun = ScriptableObject.CreateInstance<SkillDef>();
            skillDefRiotShotgun.activationState = new SerializableEntityStateType(typeof(RiotShotgun));
            skillDefRiotShotgun.activationStateMachineName = "Weapon";
            skillDefRiotShotgun.baseMaxStock = 1;
            skillDefRiotShotgun.baseRechargeInterval = 0f;
            skillDefRiotShotgun.beginSkillCooldownOnSkillEnd = false;
            skillDefRiotShotgun.canceledFromSprinting = false;
            skillDefRiotShotgun.fullRestockOnAssign = true;
            skillDefRiotShotgun.interruptPriority = InterruptPriority.Any;
            skillDefRiotShotgun.resetCooldownTimerOnUse = false;
            skillDefRiotShotgun.isCombatSkill = true;
            skillDefRiotShotgun.mustKeyPress = false;
            skillDefRiotShotgun.cancelSprintingOnActivation = true;
            skillDefRiotShotgun.rechargeStock = 1;
            skillDefRiotShotgun.requiredStock = 1;
            skillDefRiotShotgun.stockToConsume = 1;
            skillDefRiotShotgun.icon = Assets.icon10Shotgun;
            skillDefRiotShotgun.skillDescriptionToken = "ENFORCER_PRIMARY_SHOTGUN_DESCRIPTION";
            skillDefRiotShotgun.skillName = "ENFORCER_PRIMARY_SHOTGUN_NAME";
            skillDefRiotShotgun.skillNameToken = "ENFORCER_PRIMARY_SHOTGUN_NAME";

            return skillDefRiotShotgun;
        }

        public static SkillDef PrimarySkillDef_SuperShotgun() {

            SkillDef skillDefSuperShotgun = ScriptableObject.CreateInstance<SkillDef>();
            skillDefSuperShotgun.activationState = new SerializableEntityStateType(typeof(SuperShotgun2));
            skillDefSuperShotgun.activationStateMachineName = "Weapon";
            skillDefSuperShotgun.baseMaxStock = 1;
            skillDefSuperShotgun.baseRechargeInterval = 0f;
            skillDefSuperShotgun.beginSkillCooldownOnSkillEnd = false;
            skillDefSuperShotgun.canceledFromSprinting = false;
            skillDefSuperShotgun.fullRestockOnAssign = true;
            skillDefSuperShotgun.interruptPriority = InterruptPriority.Any;
            skillDefSuperShotgun.resetCooldownTimerOnUse = false;
            skillDefSuperShotgun.isCombatSkill = true;
            skillDefSuperShotgun.mustKeyPress = false;
            skillDefSuperShotgun.cancelSprintingOnActivation = true;
            skillDefSuperShotgun.rechargeStock = 1;
            skillDefSuperShotgun.requiredStock = 1;
            skillDefSuperShotgun.stockToConsume = 1;
            skillDefSuperShotgun.icon = Assets.icon11SuperShotgun;
            skillDefSuperShotgun.skillDescriptionToken = "ENFORCER_PRIMARY_SUPERSHOTGUN_DESCRIPTION";
            skillDefSuperShotgun.skillName = "ENFORCER_PRIMARY_SUPERSHOTGUN_NAME";
            skillDefSuperShotgun.skillNameToken = "ENFORCER_PRIMARY_SUPERSHOTGUN_NAME";

            return skillDefSuperShotgun;
        }

        public static SkillDef PrimarySkillDef_AssaultRifle() {

            SkillDef skillDefAssaultRifle = ScriptableObject.CreateInstance<SkillDef>();
            //skillDefAssaultRifle.activationState = new SerializableEntityStateType(typeof(FireBurstRifle));
            skillDefAssaultRifle.activationState = new SerializableEntityStateType(typeof(FireMachineGun));
            skillDefAssaultRifle.activationStateMachineName = "Weapon";
            skillDefAssaultRifle.baseMaxStock = 1;
            skillDefAssaultRifle.baseRechargeInterval = 0f;
            skillDefAssaultRifle.beginSkillCooldownOnSkillEnd = false;
            skillDefAssaultRifle.canceledFromSprinting = false;
            skillDefAssaultRifle.fullRestockOnAssign = true;
            skillDefAssaultRifle.interruptPriority = InterruptPriority.Any;
            skillDefAssaultRifle.resetCooldownTimerOnUse = false;
            skillDefAssaultRifle.isCombatSkill = true;
            skillDefAssaultRifle.mustKeyPress = false;
            skillDefAssaultRifle.cancelSprintingOnActivation = true;
            skillDefAssaultRifle.rechargeStock = 1;
            skillDefAssaultRifle.requiredStock = 1;
            skillDefAssaultRifle.stockToConsume = 1;
            skillDefAssaultRifle.icon = Assets.icon12AssaultRifle;
            skillDefAssaultRifle.skillDescriptionToken = "ENFORCER_PRIMARY_RIFLE_DESCRIPTION";
            skillDefAssaultRifle.skillName = "ENFORCER_PRIMARY_RIFLE_NAME";
            skillDefAssaultRifle.skillNameToken = "ENFORCER_PRIMARY_RIFLE_NAME";

            return skillDefAssaultRifle;
        }

        public static SkillDef PrimarySkillDef_Hammer() {

            SkillDef skillDefHammer = ScriptableObject.CreateInstance<SkillDef>();
            skillDefHammer.activationState = new SerializableEntityStateType(typeof(HammerSwing));
            skillDefHammer.activationStateMachineName = "Weapon";
            skillDefHammer.baseMaxStock = 1;
            skillDefHammer.baseRechargeInterval = 0f;
            skillDefHammer.beginSkillCooldownOnSkillEnd = false;
            skillDefHammer.canceledFromSprinting = false;
            skillDefHammer.fullRestockOnAssign = true;
            skillDefHammer.interruptPriority = InterruptPriority.Any;
            skillDefHammer.resetCooldownTimerOnUse = false;
            skillDefHammer.isCombatSkill = true;
            skillDefHammer.mustKeyPress = false;
            skillDefHammer.cancelSprintingOnActivation = true;
            skillDefHammer.rechargeStock = 1;
            skillDefHammer.requiredStock = 1;
            skillDefHammer.stockToConsume = 1;
            skillDefHammer.icon = Assets.icon13Hammer;
            skillDefHammer.skillDescriptionToken = "ENFORCER_PRIMARY_HAMMER_DESCRIPTION";
            skillDefHammer.skillName = "ENFORCER_PRIMARY_HAMMER_NAME";
            skillDefHammer.skillNameToken = "ENFORCER_PRIMARY_HAMMER_NAME";

            return skillDefHammer;
        }

        public static SkillDef SecondarySkillDef_Bash() {

            SkillDef mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(ShieldBash));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 6f;
            mySkillDef.beginSkillCooldownOnSkillEnd = false;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Skill;
            mySkillDef.resetCooldownTimerOnUse = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.cancelSprintingOnActivation = false;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Assets.icon20ShieldBash;
            mySkillDef.skillName = "ENFORCER_SECONDARY_BASH_NAME";
            mySkillDef.skillNameToken = "ENFORCER_SECONDARY_BASH_NAME";
            mySkillDef.skillDescriptionToken = "ENFORCER_SECONDARY_BASH_DESCRIPTION";
            mySkillDef.keywordTokens = new string[] {
                "KEYWORD_STUNNING",
                "KEYWORD_SPRINTBASH"
            };

            return mySkillDef;
        }
        
        public static SkillDef UtilitySkillDef_TearGas() {
            SkillDef tearGasDef = ScriptableObject.CreateInstance<SkillDef>();
            tearGasDef.activationState = new SerializableEntityStateType(typeof(AimTearGas));
            tearGasDef.activationStateMachineName = "Weapon";
            tearGasDef.baseMaxStock = 1;
            tearGasDef.baseRechargeInterval = 16f;
            tearGasDef.beginSkillCooldownOnSkillEnd = true;
            tearGasDef.canceledFromSprinting = false;
            tearGasDef.fullRestockOnAssign = true;
            tearGasDef.interruptPriority = InterruptPriority.Skill;
            tearGasDef.resetCooldownTimerOnUse = false;
            tearGasDef.isCombatSkill = true;
            tearGasDef.mustKeyPress = true;
            tearGasDef.cancelSprintingOnActivation = true;
            tearGasDef.rechargeStock = 1;
            tearGasDef.requiredStock = 1;
            tearGasDef.stockToConsume = 1;
            tearGasDef.icon = Assets.icon30TearGas;
            tearGasDef.skillDescriptionToken = "ENFORCER_UTILITY_TEARGAS_DESCRIPTION";
            tearGasDef.skillName = "ENFORCER_UTILITY_TEARGAS_NAME";
            tearGasDef.skillNameToken = "ENFORCER_UTILITY_TEARGAS_NAME";
            tearGasDef.keywordTokens = new string[] {
                "KEYWORD_BLINDED"
            };

            return tearGasDef;
        }

        public static SkillDef UtilitySkillDef_StunGrenade() {

            SkillDef stunGrenadeDef = ScriptableObject.CreateInstance<SkillDef>();
            stunGrenadeDef.activationState = new SerializableEntityStateType(typeof(StunGrenade));
            stunGrenadeDef.activationStateMachineName = "Weapon";
            stunGrenadeDef.baseMaxStock = 3;
            stunGrenadeDef.baseRechargeInterval = 7f;
            stunGrenadeDef.beginSkillCooldownOnSkillEnd = false;
            stunGrenadeDef.canceledFromSprinting = false;
            stunGrenadeDef.fullRestockOnAssign = true;
            stunGrenadeDef.interruptPriority = InterruptPriority.Skill;
            stunGrenadeDef.resetCooldownTimerOnUse = false;
            stunGrenadeDef.isCombatSkill = true;
            stunGrenadeDef.mustKeyPress = false;
            stunGrenadeDef.cancelSprintingOnActivation = true;
            stunGrenadeDef.rechargeStock = 1;
            stunGrenadeDef.requiredStock = 1;
            stunGrenadeDef.stockToConsume = 1;
            stunGrenadeDef.icon = Assets.icon31StunGrenade;
            stunGrenadeDef.skillDescriptionToken = "ENFORCER_UTILITY_STUNGRENADE_DESCRIPTION";
            stunGrenadeDef.skillName = "ENFORCER_UTILITY_STUNGRENADE_NAME";
            stunGrenadeDef.skillNameToken = "ENFORCER_UTILITY_STUNGRENADE_NAME";
            stunGrenadeDef.keywordTokens = new string[] {
                "KEYWORD_STUNNING"
            };

            return stunGrenadeDef;
        }

        public static SkillDef SpecialSkillDef_ProtectAndServe() {

            SkillDef mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(ProtectAndServe));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 0f;
            mySkillDef.beginSkillCooldownOnSkillEnd = false;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.PrioritySkill;
            mySkillDef.resetCooldownTimerOnUse = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = true;
            mySkillDef.cancelSprintingOnActivation = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Assets.icon40Shield;
            mySkillDef.skillDescriptionToken = "ENFORCER_SPECIAL_SHIELDUP_DESCRIPTION";
            mySkillDef.skillName = "ENFORCER_SPECIAL_SHIELDUP_NAME";
            mySkillDef.skillNameToken = "ENFORCER_SPECIAL_SHIELDUP_NAME";

            return mySkillDef;
        }

        public static SkillDef SpecialSkillDef_ShieldDown() {

            SkillDef mySkillDef2 = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef2.activationState = new SerializableEntityStateType(typeof(ProtectAndServe));
            mySkillDef2.activationStateMachineName = "Weapon";
            mySkillDef2.baseMaxStock = 1;
            mySkillDef2.baseRechargeInterval = 0f;
            mySkillDef2.beginSkillCooldownOnSkillEnd = false;
            mySkillDef2.canceledFromSprinting = false;
            mySkillDef2.fullRestockOnAssign = true;
            mySkillDef2.interruptPriority = InterruptPriority.PrioritySkill;
            mySkillDef2.resetCooldownTimerOnUse = false;
            mySkillDef2.isCombatSkill = true;
            mySkillDef2.mustKeyPress = true;
            mySkillDef2.cancelSprintingOnActivation = false;
            mySkillDef2.rechargeStock = 1;
            mySkillDef2.requiredStock = 1;
            mySkillDef2.stockToConsume = 1;
            mySkillDef2.icon = Assets.icon40ShieldOff;
            mySkillDef2.skillDescriptionToken = "ENFORCER_SPECIAL_SHIELDDOWN_DESCRIPTION";
            mySkillDef2.skillName = "ENFORCER_SPECIAL_SHIELDDOWN_NAME";
            mySkillDef2.skillNameToken = "ENFORCER_SPECIAL_SHIELDDOWN_NAME";

            return mySkillDef2;
        }

        public static SkillDef SpecialSkillDef_EnergyShield() {

            SkillDef mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(EnergyShield));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 0f;
            mySkillDef.beginSkillCooldownOnSkillEnd = false;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.PrioritySkill;
            mySkillDef.resetCooldownTimerOnUse = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = true;
            mySkillDef.cancelSprintingOnActivation = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Assets.testIcon;
            mySkillDef.skillDescriptionToken = "ENFORCER_SPECIAL_SHIELDON_DESCRIPTION";
            mySkillDef.skillName = "ENFORCER_SPECIAL_SHIELDON_NAME";
            mySkillDef.skillNameToken = "ENFORCER_SPECIAL_SHIELDON_NAME";

            return mySkillDef;
        }
        public static SkillDef SpecialSkillDef_EnergyShieldDown() {

            SkillDef mySkillDef2 = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef2.activationState = new SerializableEntityStateType(typeof(EnergyShield));
            mySkillDef2.activationStateMachineName = "Weapon";
            mySkillDef2.baseMaxStock = 1;
            mySkillDef2.baseRechargeInterval = 0f;
            mySkillDef2.beginSkillCooldownOnSkillEnd = false;
            mySkillDef2.canceledFromSprinting = false;
            mySkillDef2.fullRestockOnAssign = true;
            mySkillDef2.interruptPriority = InterruptPriority.PrioritySkill;
            mySkillDef2.resetCooldownTimerOnUse = false;
            mySkillDef2.isCombatSkill = true;
            mySkillDef2.mustKeyPress = true;
            mySkillDef2.cancelSprintingOnActivation = false;
            mySkillDef2.rechargeStock = 1;
            mySkillDef2.requiredStock = 1;
            mySkillDef2.stockToConsume = 1;
            mySkillDef2.icon = Assets.testIcon;
            mySkillDef2.skillDescriptionToken = "ENFORCER_SPECIAL_SHIELDOFF_DESCRIPTION";
            mySkillDef2.skillName = "ENFORCER_SPECIAL_SHIELDOFF_NAME";
            mySkillDef2.skillNameToken = "ENFORCER_SPECIAL_SHIELDOFF_NAME";

            return mySkillDef2;
        }

        public static SkillDef SpecialSkillDef_SkamteBord() {

            SkillDef mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(Skateboard));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 0f;
            mySkillDef.beginSkillCooldownOnSkillEnd = false;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.PrioritySkill;
            mySkillDef.resetCooldownTimerOnUse = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = true;
            mySkillDef.cancelSprintingOnActivation = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Assets.icon42SkateBoard;
            mySkillDef.skillDescriptionToken = "ENFORCER_SPECIAL_BOARDUP_DESCRIPTION";
            mySkillDef.skillName = "ENFORCER_SPECIAL_BOARDUP_NAME";
            mySkillDef.skillNameToken = "ENFORCER_SPECIAL_BOARDUP_NAME";

            return mySkillDef;
        }
        public static SkillDef SpecialSkillDef_SkamteBordDown() {

            SkillDef mySkillDef2 = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef2.activationState = new SerializableEntityStateType(typeof(Skateboard));
            mySkillDef2.activationStateMachineName = "Weapon";
            mySkillDef2.baseMaxStock = 1;
            mySkillDef2.baseRechargeInterval = 0f;
            mySkillDef2.beginSkillCooldownOnSkillEnd = false;
            mySkillDef2.canceledFromSprinting = false;
            mySkillDef2.fullRestockOnAssign = true;
            mySkillDef2.interruptPriority = InterruptPriority.PrioritySkill;
            mySkillDef2.resetCooldownTimerOnUse = false;
            mySkillDef2.isCombatSkill = true;
            mySkillDef2.mustKeyPress = true;
            mySkillDef2.cancelSprintingOnActivation = false;
            mySkillDef2.rechargeStock = 1;
            mySkillDef2.requiredStock = 1;
            mySkillDef2.stockToConsume = 1;
            mySkillDef2.icon = Assets.icon42SkateBoardOff;
            mySkillDef2.skillDescriptionToken = "ENFORCER_SPECIAL_BOARDDOWN_DESCRIPTION";
            mySkillDef2.skillName = "ENFORCER_SPECIAL_BOARDDOWN_NAME";
            mySkillDef2.skillNameToken = "ENFORCER_SPECIAL_BOARDDOWN_NAME";

            return mySkillDef2;
        }
        #endregion

    }
}
