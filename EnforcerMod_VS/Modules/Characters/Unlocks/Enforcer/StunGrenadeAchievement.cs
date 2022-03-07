using RoR2;

namespace EnforcerPlugin.Achievements
{
    public class StunGrenadeAchievement : GenericModdedUnlockable
    {
        public override string AchievementTokenPrefix => "ENFORCER_STUNGRENADE";
        public override string PrerequisiteUnlockableIdentifier => "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_ID";

        public override string AchievementSpriteName => "texStunGrenadeAchievement";

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("EnforcerBody");
        }

        public void Check(int count)
        {
            if (count >= 20 && base.meetsBodyRequirement) 
            {
                base.Grant();
            }
        }

        public override void OnInstall()
        {
            base.OnInstall();

            TearGasComponent.CheckImpairedCount += Check;
        }

        public override void OnUninstall()
        {
            base.OnUninstall();

            TearGasComponent.CheckImpairedCount -= Check;
        }
    }
}