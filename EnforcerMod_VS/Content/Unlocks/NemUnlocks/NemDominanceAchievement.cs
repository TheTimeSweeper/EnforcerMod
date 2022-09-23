using RoR2;

namespace EnforcerPlugin.Achievements {
    public class NemDominanceAchievement : GenericModdedUnlockable {

        public override string AchievementTokenPrefix => "NEMFORCER_DOMINANCE" + knee.grow;
        public override string PrerequisiteUnlockableIdentifier => "ENFORCER_NEMESIS2UNLOCKABLE_ACHIEVEMENT_ID";

        public override string AchievementSpriteName => "texNemforcerEnforcer";

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