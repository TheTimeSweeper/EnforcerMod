using EntityStates.Events;

namespace TKNemforcer
{
    public class NemforcerInvasion : GenericNemesisEvent
    {
        public override void OnEnter()
        {
            spawnCard = NemforcerStarstorm.NemforcerSpawnCard;
            spawnDistanceString = "Close";
            eventCard = NemforcerStarstorm.NemforcerInvasionCard;
            drizzleDuration = 0;
            typhoonDuration = 0;
            warningDuration = 15;
            base.OnEnter();
        }
    }
}


