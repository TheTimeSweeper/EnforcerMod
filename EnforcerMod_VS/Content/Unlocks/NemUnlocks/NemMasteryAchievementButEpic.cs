using RoR2;

namespace EnforcerPlugin.Achievements
{

    [RegisterAchievement(identifier, unlockableIdentifier, "ENFORCER_NEMESIS2UNLOCKABLE_ACHIEVEMENT_ID", 10, null)]
    public class NemMasteryAchievementButEpic : BaseMasteryAchievement
    {
        public const string identifier = "NEMFORCER_MASTERYUNLOCKABLE_ACHIEVEMENT_ID";
        public const string unlockableIdentifier = "NEMFORCER_MASTERYUNLOCKABLE_REWARD_ID";
        public const string AchievementSpriteName = "texNemforcerMastery";

        public override string RequiredCharacterBody => "NemesisEnforcerBody";

        public override float RequiredDifficultyCoefficient => 3f;
    }

    [RegisterAchievement(identifier, unlockableIdentifier, "ENFORCER_NEMESIS2UNLOCKABLE_ACHIEVEMENT_ID", 10, null)]
    public class NemGrandMasteryAchievement : BaseMasteryAchievement
    {
        public const string identifier = "NEMFORCER_TYPHOONUNLOCKABLE_ACHIEVEMENT_ID";
        public const string unlockableIdentifier = "NEMFORCER_TYPHOONUNLOCKABLE_REWARD_ID";
        public const string AchievementSpriteName = "texNemforcerMastery";

        public override string RequiredCharacterBody => "NemesisEnforcerBody";

        public override float RequiredDifficultyCoefficient => 3.5f;
    }
}