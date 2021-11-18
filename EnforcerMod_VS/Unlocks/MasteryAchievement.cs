using System;
using UnityEngine;
using RoR2;
using ModdedUnlockable = Enforcer.Modules.ModdedUnlockable;

namespace EnforcerPlugin.Achievements {

    public class MasteryAchievement : ModdedUnlockable
    {
        public override string AchievementIdentifier { get; } = "ENFORCER_MASTERYUNLOCKABLE_ACHIEVEMENT_ID";
        public override string UnlockableIdentifier { get; } = "ENFORCER_MASTERYUNLOCKABLE_REWARD_ID";
        public override string PrerequisiteUnlockableIdentifier { get; } = "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_ID";
        public override string AchievementNameToken { get; } = "ENFORCER_MASTERYUNLOCKABLE_ACHIEVEMENT_NAME";
        public override string AchievementDescToken { get; } = "ENFORCER_MASTERYUNLOCKABLE_ACHIEVEMENT_DESC";
        public override string UnlockableNameToken { get; }  = "ENFORCER_MASTERYUNLOCKABLE_UNLOCKABLE_NAME";
        public override Sprite Sprite { get; } = Assets.MainAssetBundle.LoadAsset<Sprite>("texNemforcerEnforcer");

        public override Func<string> GetHowToUnlock { get; } = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
                            {
                                Language.GetString("ENFORCER_MASTERYUNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString("ENFORCER_MASTERYUNLOCKABLE_ACHIEVEMENT_DESC")
                            }));
        public override Func<string> GetUnlocked { get; } = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
                            {
                                Language.GetString("ENFORCER_MASTERYUNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString("ENFORCER_MASTERYUNLOCKABLE_ACHIEVEMENT_DESC")
                            }));

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