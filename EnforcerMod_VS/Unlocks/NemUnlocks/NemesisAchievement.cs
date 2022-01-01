using RoR2;

namespace EnforcerPlugin.Achievements
{
    internal class NemesisAchievement : GenericModdedUnlockable
    {
        public override string AchievementTokenPrefix => "ENFORCER_NEMESIS2";
        public override string PrerequisiteUnlockableIdentifier => "";

        public override string AchievementSpriteName => "texNemesisUnlockAchievement";

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