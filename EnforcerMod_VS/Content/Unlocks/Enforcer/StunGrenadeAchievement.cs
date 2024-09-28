using RoR2;
using RoR2.Achievements;

namespace EnforcerPlugin.Achievements {

    [RegisterAchievement(identifier, unlockableIdentifier, "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_ID", 3, null)]
    public class StunGrenadeAchievement : BaseAchievement
    {
        public const string identifier = "ENFORCER_STUNGRENADEUNLOCKABLE_ACHIEVEMENT_ID" + knee.grow;
        public const string unlockableIdentifier = "ENFORCER_STUNGRENADEUNLOCKABLE_REWARD_ID" + knee.grow;
        public const string AchievementSpriteName = "texStunGrenadeAchievement";

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