using RoR2;
using RoR2.Navigation;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace EnforcerPlugin
{
    public class NemesisInvasionManager
    {
        public static void PerformInvasion(Xoroshiro128Plus rng)
        {
            CharacterMaster master = NemforcerPlugin.bossMaster.GetComponent<CharacterMaster>();
            CreateNemesis(master, rng);
        }

        private static void CreateNemesis(CharacterMaster master, Xoroshiro128Plus rng)
        {
            SpawnCard spawnCard = NemesisSpawnCard.FromMaster(master);
            if (!spawnCard) return;

            CharacterMaster targetMaster = null;
            for (int i = CharacterMaster.readOnlyInstancesList.Count - 1; i >= 0; i--)
            {
                CharacterMaster tempMaster = CharacterMaster.readOnlyInstancesList[i];
                if (tempMaster.teamIndex == TeamIndex.Player && tempMaster.playerCharacterMasterController)
                {
                    targetMaster = tempMaster;
                }
            }

            Transform spawnOnTarget = targetMaster.GetBody().coreTransform;
            DirectorCore.MonsterSpawnDistance input = DirectorCore.MonsterSpawnDistance.Far;

            DirectorPlacementRule directorPlacementRule = new DirectorPlacementRule
            {
                spawnOnTarget = spawnOnTarget,
                placementMode = DirectorPlacementRule.PlacementMode.NearestNode
            };

            DirectorCore.GetMonsterSpawnDistance(input, out directorPlacementRule.minDistance, out directorPlacementRule.maxDistance);
            DirectorSpawnRequest directorSpawnRequest = new DirectorSpawnRequest(spawnCard, directorPlacementRule, rng);
            directorSpawnRequest.teamIndexOverride = new TeamIndex?(TeamIndex.Monster);
            directorSpawnRequest.ignoreTeamMemberLimit = true;

            CombatSquad combatSquad = null;

            DirectorSpawnRequest directorSpawnRequest2 = directorSpawnRequest;
            directorSpawnRequest2.onSpawnedServer = (Action<SpawnCard.SpawnResult>)Delegate.Combine(directorSpawnRequest2.onSpawnedServer, new Action<SpawnCard.SpawnResult>(delegate (SpawnCard.SpawnResult result)
            {
                if (!combatSquad)
                {
                    combatSquad = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/NetworkedObjects/Encounters/ShadowCloneEncounter")).GetComponent<CombatSquad>();
                }

                combatSquad.AddMember(result.spawnedInstance.GetComponent<CharacterMaster>());
            }));

            DirectorCore.instance.TrySpawnObject(directorSpawnRequest);

            if (combatSquad)
            {
                NetworkServer.Spawn(combatSquad.gameObject);
            }

            UnityEngine.Object.Destroy(spawnCard);
        }
    }

    public class NemesisSpawnCard : CharacterSpawnCard
    {
        private CharacterMaster characterMaster;
        private Inventory inventory;

        public static NemesisSpawnCard FromMaster(CharacterMaster master)
        {
            if (!master) return null;

            CharacterBody body = master.bodyPrefab.GetComponent<CharacterBody>();
            if (!body) return null;

            NemesisSpawnCard spawnCard = ScriptableObject.CreateInstance<NemesisSpawnCard>();
            spawnCard.hullSize = HullClassification.Human;
            spawnCard.nodeGraphType = (body.isFlying ? MapNodeGroup.GraphType.Air : MapNodeGroup.GraphType.Ground);
            spawnCard.prefab = MasterCatalog.GetMasterPrefab(MasterCatalog.FindAiMasterIndexForBody(body.bodyIndex));
            spawnCard.sendOverNetwork = true;
            spawnCard.runtimeLoadout = new Loadout();
            spawnCard.characterMaster = master;
            spawnCard.characterMaster.loadout.Copy(spawnCard.runtimeLoadout);
            spawnCard.inventory = master.inventory;

            return spawnCard;
        }
    }
}
