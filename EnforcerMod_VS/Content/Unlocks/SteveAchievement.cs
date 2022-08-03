using RoR2;

namespace EnforcerPlugin.Achievements {
    public class SteveAchievement : GenericModdedUnlockable {
        public override string AchievementTokenPrefix => "ENFORCER_STEVE";
        public override string PrerequisiteUnlockableIdentifier => "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_ID";

        public override string AchievementSpriteName => "texSbeveAchievement";

        public override BodyIndex LookUpRequiredBodyIndex() {
            return BodyCatalog.FindBodyIndex("EnforcerBody");
        }

        public void Check(int count) {
            if (count >= 20 && base.meetsBodyRequirement) {
                base.Grant();
            }
        }

        public override void OnInstall() {
            base.OnInstall();

            EnforcerComponent.BlockedGet += Blocked;
        }

        public override void OnUninstall() {
            base.OnUninstall();

            EnforcerComponent.BlockedGet -= Blocked;
        }

        private void Blocked(bool cum)
        {
            if (cum && base.meetsBodyRequirement) base.Grant();
        }
    }
}