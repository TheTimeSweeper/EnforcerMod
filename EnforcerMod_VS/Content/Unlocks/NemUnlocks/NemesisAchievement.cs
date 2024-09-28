using RoR2;
using RoR2.Achievements;

namespace EnforcerPlugin.Achievements
{
    [RegisterAchievement(identifier, unlockableIdentifier, null, 5, null)]
    internal class NemesisAchievement : BaseAchievement
    {
        public const string identifier = "ENFORCER_NEMESIS2UNLOCKABLE_ACHIEVEMENT_ID";
        public const string unlockableIdentifier = "ENFORCER_NEMESIS2UNLOCKABLE_REWARD_ID";
        public const string AchievementSpriteName = "texNemesisUnlockAchievement";

        private void CheckDeath(Run run) {
            Grant();
        }

        public override void OnInstall() {
            base.OnInstall();

            NemesisUnlockComponent.OnDeath += CheckDeath;
        }

        public override void OnUninstall() {
            base.OnUninstall();

            NemesisUnlockComponent.OnDeath -= CheckDeath;
        }
    }
}