using RoR2;
using RoR2.Achievements;

namespace EnforcerPlugin.Achievements {
    [RegisterAchievement(identifier, unlockableIdentifier, "ENFORCER_NEMESIS2UNLOCKABLE_ACHIEVEMENT_ID", 3, null)]
    public class NemDominanceAchievement : BaseAchievement
    {
        public const string identifier = "NEMFORCER_DOMINANCEUNLOCKABLE_ACHIEVEMENT_ID" + knee.grow;
        public const string unlockableIdentifier = "NEMFORCER_DOMINANCEUNLOCKABLE_REWARD_ID" + knee.grow;
        public const string AchievementSpriteName = "texNemforcerEnforcer";

        public override BodyIndex LookUpRequiredBodyIndex() {
            return BodyCatalog.FindBodyIndex("NemesisEnforcerBody");
        }

        public override void OnInstall()
        {
            base.OnInstall();

            EntityStates.Nemforcer.HammerSlam.Bonked += this.Bonked;
        }

        public override void OnUninstall()
        {
            base.OnUninstall();

            EntityStates.Nemforcer.HammerSlam.Bonked -= this.Bonked;
        }

        public void Bonked(Run run) {
            if (run is null) return;

            if (base.meetsBodyRequirement) {
                base.Grant();
            }
        }
    }
}