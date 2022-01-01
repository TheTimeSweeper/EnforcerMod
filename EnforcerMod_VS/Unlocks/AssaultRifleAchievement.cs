using RoR2;

namespace EnforcerPlugin.Achievements
{

    public class AssaultRifleAchievement : GenericModdedUnlockable
    {
        public override string AchievementTokenPrefix => "ENFORCER_RIFLE";
        public override string PrerequisiteUnlockableIdentifier => "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_ID";

        public override string AchievementSpriteName => "texAssaultRifleAchievement";

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