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

            contentPack.bodyPrefabs.Add(EnforcerModPlugin.bodyPrefabs.ToArray());
            contentPack.buffDefs.Add(Buffs.buffDefs.ToArray());
            contentPack.effectDefs.Add(Modules.Effects.effectDefs.ToArray());
            contentPack.entityStateTypes.Add(Modules.States.entityStates.ToArray());
            contentPack.masterPrefabs.Add(EnforcerModPlugin.masterPrefabs.ToArray());
            contentPack.networkSoundEventDefs.Add(Assets.networkSoundEventDefs.ToArray());
            contentPack.projectilePrefabs.Add(EnforcerModPlugin.projectilePrefabs.ToArray());
            contentPack.skillDefs.Add(Modules.States.skillDefs.ToArray());
            contentPack.skillFamilies.Add(Modules.States.skillFamilies.ToArray());
            contentPack.survivorDefs.Add(EnforcerModPlugin.survivorDefs.ToArray());

            args.ReportProgress(1f);

            yield break;
        }
    }
}