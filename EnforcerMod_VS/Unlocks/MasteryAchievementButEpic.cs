namespace EnforcerPlugin.Achievements
{
    public class MasteryAchievementButEpic : BaseMasteryUnlockable
    {
        public override string AchievementTokenPrefix => "ENFORCER_MASTERY";
        public override string PrerequisiteIdentifier => "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_ID";
        public override string AchievementSpriteName => "texNemforcerEnforcer";
        
        public override string RequiredCharacterBody => "EnforcerBody";

        public override float RequiredDifficultyCoefficient => 3f;
    }

    public class GrandMasteryAchievement: BaseMasteryUnlockable
    {
        public override string AchievementTokenPrefix => "ENFORCER_GRANDMASTERY";
        public override string PrerequisiteIdentifier => "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_ID";
        public override string AchievementSpriteName => "texTyphoonAchievement";

        public override string RequiredCharacterBody => "EnforcerBody";

        public override float RequiredDifficultyCoefficient => 3.5f;
    }
}