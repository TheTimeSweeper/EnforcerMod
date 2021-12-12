using System;
using UnityEngine;
using RoR2;
using ModdedUnlockable = Enforcer.Modules.ModdedUnlockable;

namespace EnforcerPlugin.Achievements
{
    internal class NemesisAchievement : ModdedUnlockable {
        public override string AchievementIdentifier { get; } = "ENFORCER_NEMESIS2UNLOCKABLE_ACHIEVEMENT_ID";
        public override string UnlockableIdentifier { get; } = "ENFORCER_NEMESIS2UNLOCKABLE_REWARD_ID";
        public override string AchievementNameToken { get; } = "ENFORCER_NEMESIS2UNLOCKABLE_ACHIEVEMENT_NAME";
        public override string PrerequisiteUnlockableIdentifier { get; } = "";
        public override string UnlockableNameToken { get; } = "ENFORCER_NEMESIS2UNLOCKABLE_UNLOCKABLE_NAME";
        public override string AchievementDescToken { get; } = "ENFORCER_NEMESIS2UNLOCKABLE_ACHIEVEMENT_DESC";
        public override Sprite Sprite { get; } = Assets.MainAssetBundle.LoadAsset<Sprite>("texNemesisUnlockAchievement");

        public override Func<string> GetHowToUnlock { get; } = () => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
                            {
                                Language.GetString("ENFORCER_NEMESIS2UNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString("ENFORCER_NEMESIS2UNLOCKABLE_ACHIEVEMENT_DESC")
                            });
        public override Func<string> GetUnlocked { get; } = () => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
                            {
                                Language.GetString("ENFORCER_NEMESIS2UNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString("ENFORCER_NEMESIS2UNLOCKABLE_ACHIEVEMENT_DESC")
                            });

        private void CheckDeath(Run run) {
            Grant();
        }

        public override void OnInstall() {
            base.OnInstall();

            NemesisUnlockComponent.OnDeath += CheckDeath;
        }

        public override void OnUninstall() {
            base.OnUninstall();

            NemesisUnlockComponent.OnDeath -= CheckDeath;
        }
    }
}