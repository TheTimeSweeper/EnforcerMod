namespace EnforcerPlugin.Achievements
{
    public class MasteryAchievementButEpic : BaseMasteryUnlockable
    {
        public override string AchievementTokenPrefix => "ENFORCER_2";

        public override string PrerequisiteTokenPrefix => "ENFORCER_";

        public override string AchievementSpriteName => "texNemforcerEnforcer";

        public override float RequiredDifficultyCoefficient => 3.5f;

        public override string RequiredCharacterBody => "EnforcerBody";
    }
}