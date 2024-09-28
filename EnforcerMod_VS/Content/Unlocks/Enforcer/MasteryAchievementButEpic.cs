using RoR2;

namespace EnforcerPlugin.Achievements
{
    [RegisterAchievement(identifier, unlockableIdentifier, "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_ID", 10, null)]
    public class MasteryAchievementButEpic : BaseMasteryAchievement
    {
        public const string identifier = "ENFORCER_MASTERYUNLOCKABLE_ACHIEVEMENT_ID";
        public const string unlockableIdentifier = "ENFORCER_MASTERYUNLOCKABLE_REWARD_ID";
        public const string AchievementSpriteName = "texSexforcerAchievement";
        
        public override string RequiredCharacterBody => "EnforcerBody";

        public override float RequiredDifficultyCoefficient => 3f;
    }

    [RegisterAchievement(identifier, unlockableIdentifier, "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_ID", 10, null)]
    public class GrandMasteryAchievement: BaseMasteryAchievement
    {
        public const string identifier = "ENFORCER_GRANDMASTERYUNLOCKABLE_ACHIEVEMENT_ID";
        public const string unlockableIdentifier = "ENFORCER_GRANDMASTERYUNLOCKABLE_REWARD_ID";
        public const string AchievementSpriteName = "texTyphoonAchievement";

        public override string RequiredCharacterBody => "EnforcerBody";

        public override float RequiredDifficultyCoefficient => 3.5f;
    }
}