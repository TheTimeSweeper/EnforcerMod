using System.Collections.Generic;
using RoR2;
using UnityEngine;

namespace EnforcerPlugin.Modules
{
    internal class ContentPacks
    {
        internal static ContentPack contentPack;

        internal void CreateContentPack()
        {
            contentPack = new ContentPack()
            {
                artifactDefs = new ArtifactDef[0],
                bodyPrefabs = EnforcerPlugin.bodyPrefabs.ToArray(),
                buffDefs = Modules.Buffs.buffDefs.ToArray(),
                effectDefs = Modules.Effects.effectDefs.ToArray(),
                eliteDefs = new EliteDef[0],
                entityStateConfigurations = new EntityStateConfiguration[0],
                entityStateTypes = Modules.States.entityStates.ToArray(),
                equipmentDefs = new EquipmentDef[0],
                gameEndingDefs = new GameEndingDef[0],
                gameModePrefabs = new Run[0],
                itemDefs = new ItemDef[0],
                masterPrefabs = EnforcerPlugin.masterPrefabs.ToArray(),
                musicTrackDefs = new MusicTrackDef[0],
                networkedObjectPrefabs = new GameObject[0],
                networkSoundEventDefs = Assets.networkSoundEventDefs.ToArray(),
                projectilePrefabs = EnforcerPlugin.projectilePrefabs.ToArray(),
                sceneDefs = new SceneDef[0],
                skillDefs = Modules.States.skillDefs.ToArray(),
                skillFamilies = Modules.States.skillFamilies.ToArray(),
                surfaceDefs = new SurfaceDef[0],
                survivorDefs = EnforcerPlugin.survivorDefs.ToArray(),
                unlockableDefs = new UnlockableDef[0]
            };

            On.RoR2.ContentManager.SetContentPacks += AddContent;
        }

        private void AddContent(On.RoR2.ContentManager.orig_SetContentPacks orig, List<ContentPack> newContentPacks)
        {
            newContentPacks.Add(contentPack);
            orig(newContentPacks);
        }
    }
}