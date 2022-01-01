using RoR2;

namespace EnforcerPlugin.Achievements
{
    public class ClassicAchievement : GenericModdedUnlockable
    {
        public override string AchievementTokenPrefix => "ENFORCER_CLASSIC";
        public override string PrerequisiteUnlockableIdentifier => "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_ID";

        public override string AchievementSpriteName => "texClassicAchievement";

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("EnforcerBody");
        }

        private void Schmoovin(bool isDancing)
        {
            if (base.meetsBodyRequirement && isDancing) base.Grant();
        }

        public override void OnInstall()
        {
            base.OnInstall();

            EntityStates.Enforcer.EnforcerMain.onDance += Schmoovin;
        }

        public override void OnUninstall()
        {
            base.OnUninstall();

            EntityStates.Enforcer.EnforcerMain.onDance -= Schmoovin;
        }
    }
}