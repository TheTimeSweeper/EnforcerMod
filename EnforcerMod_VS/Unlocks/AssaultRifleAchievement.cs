using System;
using UnityEngine;
using RoR2;
using ModdedUnlockable = Enforcer.Modules.ModdedUnlockable;

namespace EnforcerPlugin.Achievements {

    public class AssaultRifleAchievement : ModdedUnlockable
    {
        public override String AchievementIdentifier { get; } = "ENFORCER_RIFLEUNLOCKABLE_ACHIEVEMENT_ID";
        public override String UnlockableIdentifier { get; } = "ENFORCER_RIFLEUNLOCKABLE_REWARD_ID";
        public override String PrerequisiteUnlockableIdentifier { get; } = "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_ID";
        public override String AchievementNameToken { get; } = "ENFORCER_RIFLEUNLOCKABLE_ACHIEVEMENT_NAME";
        public override String AchievementDescToken { get; } = "ENFORCER_RIFLEUNLOCKABLE_ACHIEVEMENT_DESC";
        public override String UnlockableNameToken { get; }  = "ENFORCER_RIFLEUNLOCKABLE_UNLOCKABLE_NAME";

        public override Sprite Sprite { get; } = Assets.MainAssetBundle.LoadAsset<Sprite>("texAssaultRifleAchievement");

        public override Func<string> GetHowToUnlock { get; } = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
                            {
                                Language.GetString("ENFORCER_RIFLEUNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString("ENFORCER_RIFLEUNLOCKABLE_ACHIEVEMENT_DESC")
                            }));
        public override Func<string> GetUnlocked { get; } = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
                            {
                                Language.GetString("ENFORCER_RIFLEUNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString("ENFORCER_RIFLEUNLOCKABLE_ACHIEVEMENT_DESC")
                            }));

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("EnforcerBody");
        }

        public void CheckAttackSpeed()
        {
            if (base.localUser != null && base.localUser.cachedBody != null && base.localUser.cachedBody.attackSpeed >= 4f && base.meetsBodyRequirement)
            {
                base.Grant();
            }
        }

        public override void OnInstall()
        {
            base.OnInstall();

            RoR2Application.onUpdate += this.CheckAttackSpeed;
        }

        public override void OnUninstall()
        {
            base.OnUninstall();

            RoR2Application.onUpdate -= this.CheckAttackSpeed;
        }
    }
}