using System;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Modules {
    internal class Content {

        //must be some DRY way to do this

        //prefabs
        public static void AddCharacterBodyPrefab(params GameObject[] prefabs) {

            foreach (GameObject prefab in prefabs) {
                ContentPacks.bodyPrefabs.Add(prefab);
            }
        }

        public static void AddMasterPrefab(params GameObject[] prefabs) {

            foreach (GameObject prefab in prefabs) {
                ContentPacks.masterPrefabs.Add(prefab);
            }
        }

        public static void AddProjectilePrefab(params GameObject[] prefabs) {

            foreach (GameObject prefab in prefabs) {
                ContentPacks.projectilePrefabs.Add(prefab);
            }
        }

        //survivors and skills
        public static void AddSurvivorDef(SurvivorDef survivorDef) {

            ContentPacks.survivorDefs.Add(survivorDef);
        }

        public static void AddUnlockableDef(UnlockableDef unlockableDef) {

            ContentPacks.unlockableDefs.Add(unlockableDef);
        }

        public static void AddSkillDef(SkillDef skillDef) {

            if (ContentPacks.skillDefs.Contains(skillDef))
                return;

            ContentPacks.skillDefs.Add(skillDef);
        }

        public static void AddSkillFamily(SkillFamily skillFamily) {

            ContentPacks.skillFamilies.Add(skillFamily);
        }

        public static void AddEntityState(params Type[] entityStates) {
            foreach (var entityState in entityStates) {
                ContentPacks.entityStates.Add(entityState);
            }
        }

        //other defs
        public static void AddBuffDef(BuffDef buffDef) {

            ContentPacks.buffDefs.Add(buffDef);
        }
        public static void AddEffectDef(params EffectDef[] effectDefs) {
            foreach (EffectDef effectDef in effectDefs) {
                ContentPacks.effectDefs.Add(effectDef);
            }
        }

        public static void AddNetworkSoundEventDef(NetworkSoundEventDef networkSoundEventDef) {

            ContentPacks.networkSoundEventDefs.Add(networkSoundEventDef);
        }
    }
}