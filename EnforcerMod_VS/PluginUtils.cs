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

        /// <summary>
        /// LoadoutApi needs to add your skilldef, as well as any EntityState types your skill might use
        /// </summary>
        /// <param name="skillDef"></param>
        /// <param name="skillTypes"></param>
        public static void RegisterSkillDef(SkillDef skillDef, params Type[] skillTypes) {
            for (int i = 0; i < skillTypes.Length; i++) {
                LoadoutAPI.AddSkill(skillTypes[i]);
            }

            LoadoutAPI.AddSkillDef(skillDef); 
        }

        /// <summary>
        /// </summary>
        /// <param name="skillDef"></param>
        /// <param name="unlockableName"></param>
        /// <returns>Returns a new skill variant to add to your character's skill families</returns>
        public static SkillFamily.Variant SetupSkillVariant(SkillDef skillDef, string unlockableName = "") {

            return new SkillFamily.Variant {
                skillDef = skillDef,
                unlockableName = unlockableName,
                viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
            };
            
        }

        /// <summary>
        /// Takes care of adding your character's skills. Adds a GenericSkill component to your CharacterBody, sets up a Skillfamily, and adds all your skill variants 
        /// </summary>
        /// <param name="characterBodyObject"></param>
        /// <param name="skillVariants"></param>
        /// <returns>Returns a SkillFamily with your skill variants. Set your character's SkillLocator (for example skillLocator.primary, etc)</returns>
        public static GenericSkill RegisterSkillsToFamily(GameObject characterBodyObject, params SkillFamily.Variant[] skillVariants) {
            return RegisterSkillsToFamily(characterBodyObject, "", skillVariants);
            
        }
        /// <summary>
        /// Takes care of adding your character's skills. Adds a GenericSkill component to your characterBody, sets up a skillfamily, and adds all your skill variants 
        /// </summary>
        /// <param name="characterBodyObject"></param>
        /// <param name="skillname">A label to the GenericSkill component. Useful for when a skill needs to reference your currently equipped skills</param>
        /// <param name="skillVariants"></param>
        /// <returns></returns>
        public static GenericSkill RegisterSkillsToFamily(GameObject characterBodyObject, string skillname, params SkillFamily.Variant[] skillVariants) {
            GenericSkill genericSkill = characterBodyObject.AddComponent<GenericSkill>();

            SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
            LoadoutAPI.AddSkillFamily(newFamily);

            genericSkill.SetFieldValue("_skillFamily", newFamily);
            genericSkill.SetFieldValue("skillName", skillname);

            newFamily.variants = skillVariants;

            return genericSkill;
        }

        /// <summary>
        /// if you're simply setting up all your variants at once, use RegisterSkillsToFamily. Use this if you want to add additional skills to an already set up family (for example in a config)
        /// </summary>
        /// <param name="genericSkill"></param>
        /// <param name="skillVariants"></param>
        public static void RegisterAdditionalSkills(GenericSkill genericSkill, params SkillFamily.Variant[] skillVariants) {

            SkillFamily skillfamily = genericSkill.skillFamily;

            skillfamily.variants = skillfamily.variants.Concat(skillVariants).ToArray();
        }

        public static void createHitbox(HitBoxGroup hitboxGroup,
                                        ChildLocator childLocator,
                                        string objectName) {
            createHitbox(hitboxGroup, childLocator, objectName, Vector3.one, Vector3.zero);
        }

        public static void createHitbox(HitBoxGroup hitboxGroup,
                                        ChildLocator childLocator,
                                        string objectName,
                                        Vector3 scaleMultiplier,
                                        Vector3 position) {

            GameObject hitboxObject = childLocator.FindChild(objectName).gameObject;
            hitboxObject.transform.localScale = Vector3.Scale(hitboxObject.transform.localScale, scaleMultiplier);
            hitboxObject.transform.localPosition = position;
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