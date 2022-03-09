using System;
using System.Collections;
using System.Collections.Generic;
using EnforcerPlugin;
using RoR2;
using RoR2.ContentManagement;
using RoR2.Skills;
using UnityEngine;

namespace Modules {
    //quick and dirty adadptaion of this module to the new interface
    public class ContentPacks : IContentPackProvider {
        private static ContentPack contentPack = new ContentPack();

        public string identifier => "Enforcer.EnforcerContent";

        internal static List<GameObject> bodyPrefabs = new List<GameObject>();
        internal static List<GameObject> masterPrefabs = new List<GameObject>();
        internal static List<GameObject> projectilePrefabs = new List<GameObject>();

        internal static List<SurvivorDef> survivorDefs = new List<SurvivorDef>();
        internal static List<UnlockableDef> unlockableDefs = new List<UnlockableDef>();

        internal static List<SkillFamily> skillFamilies = new List<SkillFamily>();
        internal static List<SkillDef> skillDefs = new List<SkillDef>();
        internal static List<Type> entityStates = new List<Type>();

        internal static List<BuffDef> buffDefs = new List<BuffDef>();
        internal static List<EffectDef> effectDefs = new List<EffectDef>();

        internal static List<NetworkSoundEventDef> networkSoundEventDefs = new List<NetworkSoundEventDef>();


        public IEnumerator FinalizeAsync(FinalizeAsyncArgs args) {
            args.ReportProgress(1f);
            yield break;
        }

        public IEnumerator GenerateContentPackAsync(GetContentPackAsyncArgs args) {
            ContentPack.Copy(contentPack, args.output);
            yield break;
        }

        public IEnumerator LoadStaticContentAsync(LoadStaticContentAsyncArgs args) {

            contentPack.bodyPrefabs.Add(bodyPrefabs.ToArray());
            contentPack.buffDefs.Add(buffDefs.ToArray());
            contentPack.effectDefs.Add(effectDefs.ToArray());
            contentPack.entityStateTypes.Add(entityStates.ToArray());
            contentPack.masterPrefabs.Add(masterPrefabs.ToArray());
            contentPack.networkSoundEventDefs.Add(networkSoundEventDefs.ToArray());
            contentPack.projectilePrefabs.Add(projectilePrefabs.ToArray());
            contentPack.skillDefs.Add(skillDefs.ToArray());
            contentPack.skillFamilies.Add(skillFamilies.ToArray());
            contentPack.survivorDefs.Add(survivorDefs.ToArray());

            args.ReportProgress(1f);

            yield break;
        }
    }
}