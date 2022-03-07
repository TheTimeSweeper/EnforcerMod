using RoR2;

namespace EnforcerPlugin.Achievements
{
    public class MasteryAchievement : GenericModdedUnlockable
    {

        public override string AchievementTokenPrefix => "ENFORCER_MASTERY";
        public override string PrerequisiteUnlockableIdentifier => "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_ID";

        public override string AchievementSpriteName => "texNemforcerEnforcer";

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("EnforcerBody");
        }

        public override void OnInstall() {
            base.OnInstall();
            Run.onClientGameOverGlobal += this.ClearCheck;
        }

        public override void OnUninstall() {
            base.OnUninstall();
            Run.onClientGameOverGlobal -= this.ClearCheck;
        }

        public void ClearCheck(Run run, RunReport runReport)
        {
            if (run is null) return;
            if (runReport is null) return;

            if (!runReport.gameEnding) return;

            if (runReport.gameEnding.isWin)
            {
                DifficultyDef difficultyDef = DifficultyCatalog.GetDifficultyDef(runReport.ruleBook.FindDifficulty());

                if (difficultyDef != null && difficultyDef.countsAsHardMode)
                {
                    if (base.meetsBodyRequirement)
                    {
                        base.Grant();
                    }
                }
            }
        }
    }

}