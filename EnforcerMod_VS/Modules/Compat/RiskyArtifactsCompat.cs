using RoR2;
using System.Runtime.CompilerServices;

namespace EnforcerPlugin
{
    public class RiskyArtifactsCompat
    {
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void AddDedede(CharacterSpawnCard characterSpawnCard) {
            Risky_Artifacts.Artifacts.Origin.AddSpawnCard(characterSpawnCard, Risky_Artifacts.Artifacts.Origin.BossTier.t2);
        }
    }
}
