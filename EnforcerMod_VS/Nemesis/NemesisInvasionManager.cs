using R2API.Utils;
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
            if (master) CreateNemesis(master, rng);
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

            if (!targetMaster.GetBody()) return;

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

                CharacterMaster characterMaster = result.spawnedInstance.GetComponent<CharacterMaster>();

                //fuck this
                // - I will
                if (ArenaMissionController.instance) {
                    Inventory arenaInventory = ArenaMissionController.instance.inventory;
                    characterMaster.inventory.AddItemsFrom(arenaInventory);
                }

                //thanks man
                //have some bullshit boss scaling~
                float num = 1f;
                float num2 = 1f;
                num += Run.instance.difficultyCoefficient / 2.5f;
                num2 += Run.instance.difficultyCoefficient / 30f;
                int num3 = Mathf.Max(1, Run.instance.livingPlayerCount);
                num *= Mathf.Pow((float)num3, 0.5f);
                /*Debug.LogFormat("Nemesis Encounter: currentBoostHpCoefficient={0}, currentBoostDamageCoefficient={1}", new object[]
                {
                        num,
                        num2
                });*/
                characterMaster.inventory.GiveItem(ItemIndex.BoostHp, Mathf.RoundToInt((num - 1f) * 10f));
                characterMaster.inventory.GiveItem(ItemIndex.BoostDamage, Mathf.RoundToInt((num2 - 1f) * 10f));

                //haha fuck you
                //!
                characterMaster.inventory.GiveItem(ItemIndex.AdaptiveArmor, 1);

                combatSquad.AddMember(characterMaster);
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

            return spawnCard;
        }
    }
}