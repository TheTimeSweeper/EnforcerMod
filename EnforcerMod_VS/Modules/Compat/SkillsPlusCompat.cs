using SkillsPlusPlus;
using SkillsPlusPlus.Modifiers;
using EntityStates.Enforcer;
using EntityStates.Enforcer.NeutralSpecial;
using RoR2;
using RoR2.Skills;
using System;
using R2API;

namespace EnforcerPlugin
{

    public class SkillsPlusCompat {

        public static float riotInitialDamage;
        public static float riotInitialThiccness;
        public static float riotInitialSpread;

        public static void init() {

            riotInitialDamage = RiotShotgun.damageCoefficient;
            riotInitialThiccness = RiotShotgun.bulletThiccness;
            riotInitialSpread = RiotShotgun.bulletSpread;
            
            SkillModifierManager.LoadSkillModifiers();
        }
    }
                         //SkillDef.skillName
    [SkillLevelModifier("ENFORCER_PRIMARY_SHOTGUN_NAME", typeof(RiotShotgun))]
    public class EnfuckerRiotShotgunModifier : SimpleSkillModifier<RiotShotgun> {

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) {
            base.OnSkillLeveledUp(level, characterBody, skillDef);

            RiotShotgun.damageCoefficient = MultScaling(SkillsPlusCompat.riotInitialDamage, .05f, level);
            RiotShotgun.bulletThiccness = MultScaling(SkillsPlusCompat.riotInitialThiccness, .1f, level);
            RiotShotgun.bulletSpread = AdditiveScaling(SkillsPlusCompat.riotInitialSpread, .1f, level);
            RiotShotgun.levelHasChanged = true;
        }
    }
}
