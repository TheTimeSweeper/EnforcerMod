using System;
using UnityEngine;
using RoR2;
using ModdedUnlockable = Enforcer.Modules.ModdedUnlockable;

namespace EnforcerPlugin.Achievements
{
    public class StunGrenadeAchievement : ModdedUnlockable
    {
        public override String AchievementIdentifier { get; } = "ENFORCER_STUNGRENADEUNLOCKABLE_ACHIEVEMENT_ID";
        public override String UnlockableIdentifier { get; } = "ENFORCER_STUNGRENADEUNLOCKABLE_REWARD_ID";
        public override String PrerequisiteUnlockableIdentifier { get; } = "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_ID";
        public override String AchievementNameToken { get; } = "ENFORCER_STUNGRENADEUNLOCKABLE_ACHIEVEMENT_NAME";
        public override String AchievementDescToken { get; } = "ENFORCER_STUNGRENADEUNLOCKABLE_ACHIEVEMENT_DESC";
        public override String UnlockableNameToken { get; } = "ENFORCER_STUNGRENADEUNLOCKABLE_UNLOCKABLE_NAME";

        public override Sprite Sprite { get; } = Assets.MainAssetBundle.LoadAsset<Sprite>("texStunGrenadeAchievement.png");

        public override Func<string> GetHowToUnlock { get; } = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
                            {
                                Language.GetString("ENFORCER_STUNGRENADEUNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString("ENFORCER_STUNGRENADEUNLOCKABLE_ACHIEVEMENT_DESC")
                            }));
        public override Func<string> GetUnlocked { get; } = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
                            {
                                Language.GetString("ENFORCER_STUNGRENADEUNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString("ENFORCER_STUNGRENADEUNLOCKABLE_ACHIEVEMENT_DESC")
                            }));

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("EnforcerBody");
        }

        public void Check(int count)
        {
            if (count >= 20 && base.meetsBodyRequirement) 
            {
                base.Grant();
            }
        }

        public override void OnInstall()
        {
            base.OnInstall();

            TearGasComponent.CheckImpairedCount += Check;
        }

        public override void OnUninstall()
        {
            base.OnUninstall();

            TearGasComponent.CheckImpairedCount -= Check;
        }
    }
}