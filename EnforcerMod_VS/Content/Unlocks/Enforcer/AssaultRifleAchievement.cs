using RoR2;
using RoR2.Achievements;

namespace EnforcerPlugin.Achievements {

    [RegisterAchievement(identifier, unlockableIdentifier, "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_ID", 3, null)]
    public class AssaultRifleAchievement : BaseAchievement
    {
        public const string identifier = "ENFORCER_RIFLEUNLOCKABLE_ACHIEVEMENT_ID";
        public const string unlockableIdentifier = "ENFORCER_RIFLEUNLOCKABLE_REWARD_ID";
        public const string AchievementSpriteName = "texAssaultRifleAchievement";

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("EnforcerBody");
        }

        public override void OnInstall() {
            base.OnInstall();

            RoR2Application.onUpdate += this.CheckAttackSpeed;
        }

        public override void OnUninstall() {
            base.OnUninstall();

            RoR2Application.onUpdate -= this.CheckAttackSpeed;
        }

        public void CheckAttackSpeed()
        {
            if (base.localUser != null && base.localUser.cachedBody != null && base.localUser.cachedBody.attackSpeed >= 4f && base.meetsBodyRequirement)
            {
                base.Grant();
            }
        }
    }
}