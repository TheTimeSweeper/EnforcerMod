using System.Collections;
using System.Collections.Generic;

using RoR2.ContentManagement;
using UnityEngine;

namespace EnforcerPlugin.Modules
{
    //quick and dirty adadptaion of this module to the new interface
    internal class ContentPacks : IContentPackProvider
    {
        private static ContentPack contentPack = new ContentPack();

        public string identifier => throw new System.NotImplementedException();

        public IEnumerator FinalizeAsync(FinalizeAsyncArgs args) {
            args.ReportProgress(1f);
            yield break;
        }

        public IEnumerator GenerateContentPackAsync(GetContentPackAsyncArgs args) {
            ContentPack.Copy(contentPack, args.output); 
            yield break;
        }

        public IEnumerator LoadStaticContentAsync(LoadStaticContentAsyncArgs args) {

            contentPack.bodyPrefabs.Add(EnforcerPlugin.bodyPrefabs.ToArray());
            contentPack.buffDefs.Add(Modules.Buffs.buffDefs.ToArray());
            contentPack.effectDefs.Add(Modules.Effects.effectDefs.ToArray());
            contentPack.entityStateTypes.Add(Modules.States.entityStates.ToArray());
            contentPack.masterPrefabs.Add(EnforcerPlugin.masterPrefabs.ToArray());
            contentPack.networkSoundEventDefs.Add(Assets.networkSoundEventDefs.ToArray());
            contentPack.projectilePrefabs.Add(EnforcerPlugin.projectilePrefabs.ToArray());
            contentPack.skillDefs.Add(Modules.States.skillDefs.ToArray());
            contentPack.skillFamilies.Add(Modules.States.skillFamilies.ToArray());
            contentPack.survivorDefs.Add(EnforcerPlugin.survivorDefs.ToArray());

            args.ReportProgress(1f);

            yield break;
        }

        //rip
        internal void CreateContentPack()
        {
            //contentPack = new ContentPack()
            //{
            //    //artifactDefs = new ArtifactDef[0],
            //    bodyPrefabs = EnforcerPlugin.bodyPrefabs.ToArray(),
            //    buffDefs = Modules.Buffs.buffDefs.ToArray(),
            //    effectDefs = Modules.Effects.effectDefs.ToArray(),
            //    //eliteDefs = new EliteDef[0],
            //    //entityStateConfigurations = new EntityStateConfiguration[0],
            //    entityStateTypes = Modules.States.entityStates.ToArray(),
            //    //equipmentDefs = new EquipmentDef[0],
            //    //gameEndingDefs = new GameEndingDef[0],
            //    //gameModePrefabs = new Run[0],
            //    //itemDefs = new ItemDef[0],
            //    masterPrefabs = EnforcerPlugin.masterPrefabs.ToArray(),
            //    //musicTrackDefs = new MusicTrackDef[0],
            //    //networkedObjectPrefabs = new GameObject[0],
            //    networkSoundEventDefs = Assets.networkSoundEventDefs.ToArray(),
            //    projectilePrefabs = EnforcerPlugin.projectilePrefabs.ToArray(),
            //    //sceneDefs = new SceneDef[0],
            //    skillDefs = Modules.States.skillDefs.ToArray(),
            //    skillFamilies = Modules.States.skillFamilies.ToArray(),
            //    //surfaceDefs = new SurfaceDef[0],
            //    survivorDefs = EnforcerPlugin.survivorDefs.ToArray(),
            //    //unlockableDefs = new UnlockableDef[0]
            //};

            contentPack.bodyPrefabs.Add(EnforcerPlugin.bodyPrefabs.ToArray());
            contentPack.buffDefs.Add(Modules.Buffs.buffDefs.ToArray());
            contentPack.effectDefs.Add(Modules.Effects.effectDefs.ToArray());
            contentPack.entityStateTypes.Add(Modules.States.entityStates.ToArray());
            contentPack.masterPrefabs.Add(EnforcerPlugin.masterPrefabs.ToArray());
            contentPack.networkSoundEventDefs.Add(Assets.networkSoundEventDefs.ToArray());
            contentPack.projectilePrefabs.Add(EnforcerPlugin.projectilePrefabs.ToArray());
            contentPack.skillDefs.Add(Modules.States.skillDefs.ToArray());
            contentPack.skillFamilies.Add(Modules.States.skillFamilies.ToArray());
            contentPack.survivorDefs.Add(EnforcerPlugin.survivorDefs.ToArray());

            //On.RoR2.ContentManager.SetContentPacks += AddContent;
        }

        //rip
        //private void AddContent(On.RoR2.ContentManager.orig_SetContentPacks orig, List<ContentPack> newContentPacks)
        //{
        //    newContentPacks.Add(contentPack);
        //    orig(newContentPacks);
        //}
    }
}