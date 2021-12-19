using System;
using UnityEngine;
using RoR2;
using ModdedUnlockable = Enforcer.Modules.ModdedUnlockable;

namespace EnforcerPlugin.Achievements
{
    public abstract class BaseMasteryUnlockable : ModdedUnlockable
    {
        public abstract string AchievementTokenPrefix { get; }
        public abstract string PrerequisiteIdentifier { get; }
        public abstract string AchievementSpriteName { get; }

        public abstract string RequiredCharacterBody { get; }

        public abstract float RequiredDifficultyCoefficient { get;}
        //public CharacterBody RequiredCharacterBody { get; }

        public override string AchievementIdentifier { get => AchievementTokenPrefix + "UNLOCKABLE_ACHIEVEMENT_ID"; }
        public override string UnlockableIdentifier { get => AchievementTokenPrefix + "UNLOCKABLE_REWARD_ID"; }
        public override string PrerequisiteUnlockableIdentifier { get => PrerequisiteIdentifier; }
        public override string AchievementNameToken { get => AchievementTokenPrefix + "UNLOCKABLE_ACHIEVEMENT_NAME"; }
        public override string AchievementDescToken { get => AchievementTokenPrefix + "UNLOCKABLE_ACHIEVEMENT_DESC"; }
        public override string UnlockableNameToken { get => AchievementTokenPrefix + "UNLOCKABLE_UNLOCKABLE_NAME"; }

        public override Sprite Sprite => Assets.MainAssetBundle.LoadAsset<Sprite>(AchievementSpriteName);

        public override Func<string> GetHowToUnlock 
        {
            get
            {
                return () => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
                            {
                                Language.GetString(AchievementTokenPrefix + "UNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString(AchievementTokenPrefix + "UNLOCKABLE_ACHIEVEMENT_DESC")
                            });
            }
        }
        public override Func<string> GetUnlocked
        {
            get
            {
                return () => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
                            {
                                Language.GetString(AchievementTokenPrefix + "UNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString(AchievementTokenPrefix + "UNLOCKABLE_ACHIEVEMENT_DESC")
                            });
            }
        }

        //not sure if we use constructors
        //public GenericMasteryAchievement(float requiredDifficultyCoef, CharacterBody requiredCharBody)
        //{
        //    RequiredDifficultyCoefficient = requiredDifficultyCoef;
        //    RequiredCharacterBody = requiredCharBody;
        //}

        public override void OnBodyRequirementMet()
        {
            base.OnBodyRequirementMet();
            Run.onClientGameOverGlobal += OnClientGameOverGlobal;
        }
        public override void OnBodyRequirementBroken()
        {
            Run.onClientGameOverGlobal -= OnClientGameOverGlobal;
            base.OnBodyRequirementBroken();
        }
        private void OnClientGameOverGlobal(Run run, RunReport runReport)
        {
            if ((bool)runReport.gameEnding && runReport.gameEnding.isWin)
            {
                DifficultyDef runDifficulty = DifficultyCatalog.GetDifficultyDef(runReport.ruleBook.FindDifficulty());
                if (runDifficulty.countsAsHardMode && runDifficulty.scalingValue >= RequiredDifficultyCoefficient)
                {
                    Grant();
                }
            }
        }

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(RequiredCharacterBody);
        }
    }
}