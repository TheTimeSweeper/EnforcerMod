namespace EnforcerPlugin.Achievements
{
    public class NemMasteryAchievementButEpic : BaseMasteryUnlockable
    {
        public override string AchievementTokenPrefix => "NEMFORCER_MASTERY";
        public override string PrerequisiteUnlockableIdentifier => "ENFORCER_NEMESIS2UNLOCKABLE_ACHIEVEMENT_ID";
        public override string AchievementSpriteName => "texNemforcerMastery";

        public override string RequiredCharacterBody => "NemesisEnforcerBody";

        public override float RequiredDifficultyCoefficient => 3f;
    }

    public class NemGrandMasteryAchievement : BaseMasteryUnlockable
    {
        public override string AchievementTokenPrefix => "NEMFORCER_TYPHOON";
        public override string PrerequisiteUnlockableIdentifier => "ENFORCER_NEMESIS2UNLOCKABLE_ACHIEVEMENT_ID";
        public override string AchievementSpriteName => "texNemforcerMastery";

        public override string RequiredCharacterBody => "NemesisEnforcerBody";

        public override float RequiredDifficultyCoefficient => 3.5f;
    }
}