using System;
using R2API;
using R2API.Utils;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace EnforcerPlugin {
    public class PluginUtils {

        public static SkillFamily.Variant SetupSkillVariant(SkillDef skillDef, string unlockableType, params Type[] skillTypes) {
            RegisterSkillDef(skillDef, skillTypes);

            return new SkillFamily.Variant {
                skillDef = skillDef,
                unlockableName = unlockableType,
                viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
            };
        }

        public static SkillFamily.Variant SetupSkillVariant(SkillDef skillDef, params Type[] skillTypes) {
            return SetupSkillVariant(skillDef, "", skillTypes);
        }

        public static void RegisterSkillDef(SkillDef skillDef, params Type[] skillTypes) {
            for (int i = 0; i < skillTypes.Length; i++) {
                LoadoutAPI.AddSkill(skillTypes[i]);
            }

            LoadoutAPI.AddSkillDef(skillDef);
        }

        public static GenericSkill RegisterSkillsToFamily(GameObject characterBodyObject, params SkillFamily.Variant[] skillVariants) {
            GenericSkill genericSkill = characterBodyObject.AddComponent<GenericSkill>();

            SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
            LoadoutAPI.AddSkillFamily(newFamily);

            genericSkill.SetFieldValue("_skillFamily", newFamily);

            newFamily.variants = skillVariants;

            return genericSkill;
        }

        public static void RegisterAdditionalSkills(GenericSkill genericSkill, params SkillFamily.Variant[] skillVariants) {

            SkillFamily skillfamily = genericSkill.skillFamily;

            skillfamily.variants = skillfamily.variants.Concat(skillVariants).ToArray();
        }

        public static void createHitbox(HitBoxGroup hitboxGroup,
                                        ChildLocator childLocator,
                                        string childName) {

            createHitbox(hitboxGroup, childLocator, childName, Vector3.one, Vector3.one);
        }

        public static void createHitbox(HitBoxGroup hitboxGroup,
                                        ChildLocator childLocator,
                                        string childName,
                                        Vector3 scaleMultiplier,
                                        Vector3 position) {

            GameObject hitboxObject = childLocator.FindChild(childName).gameObject;
            hitboxObject.transform.localScale = Vector3.Scale(hitboxObject.transform.localScale, scaleMultiplier);
            hitboxObject.transform.localPosition = Vector3.one;
            hitboxObject.layer = LayerIndex.projectile.intVal;

            HitBox hitBox = hitboxObject.AddComponent<HitBox>();

            int hitboxes = hitboxGroup.hitBoxes.Length;

            if (hitboxGroup.hitBoxes == null) {
                hitboxGroup.hitBoxes = new HitBox[0];
            } else {
                hitboxGroup.hitBoxes = new HitBox[hitboxes + 1];
            }

            hitboxGroup.hitBoxes[hitboxes] = hitBox;

        }
    }
}