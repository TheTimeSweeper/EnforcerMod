using System;
using UnityEngine;
using RoR2;
using ModdedUnlockable = Enforcer.Modules.ModdedUnlockable;
using RoR2.Achievements;

namespace EnforcerPlugin.Achievements {
    public class RobitAchievement : ModdedUnlockable {
        public override String AchievementIdentifier { get; } = "ENFORCER_ROBITUNLOCKABLE_ACHIEVEMENT_ID";
        public override String UnlockableIdentifier { get; } = "ENFORCER_ROBITUNLOCKABLE_REWARD_ID";
        public override String PrerequisiteUnlockableIdentifier { get; } = "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_ID";
        public override String AchievementNameToken { get; } = "ENFORCER_ROBITUNLOCKABLE_ACHIEVEMENT_NAME";
        public override String AchievementDescToken { get; } = "ENFORCER_ROBITUNLOCKABLE_ACHIEVEMENT_DESC";
        public override String UnlockableNameToken { get; } = "ENFORCER_ROBITUNLOCKABLE_UNLOCKABLE_NAME";

        public override Sprite Sprite { get; } = Assets.MainAssetBundle.LoadAsset<Sprite>("texNemforcerEnforcer");

        public override Func<string> GetHowToUnlock { get; } = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
                            {
                                Language.GetString("ENFORCER_ROBITUNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString("ENFORCER_ROBITUNLOCKABLE_ACHIEVEMENT_DESC")
                            }));
        public override Func<string> GetUnlocked { get; } = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
                            {
                                Language.GetString("ENFORCER_ROBITUNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString("ENFORCER_ROBITUNLOCKABLE_ACHIEVEMENT_DESC")
                            }));

        public override BodyIndex LookUpRequiredBodyIndex() {
            return BodyCatalog.FindBodyIndex("EnforcerBody");
        }

        // Token: 0x06003926 RID: 14630 RVA: 0x000DD289 File Offset: 0x000DB489
        public override void OnBodyRequirementMet() {
            base.OnBodyRequirementMet();
            base.SetServerTracked(true);
        }

        // Token: 0x06003927 RID: 14631 RVA: 0x000DD298 File Offset: 0x000DB498
        public override void OnBodyRequirementBroken() {
            base.SetServerTracked(false);
            base.OnBodyRequirementBroken();
        }
        public class RobitAchievementServer : BaseServerAchievement {

            public override void OnInstall() {
                base.OnInstall();
                //oh god this isn't gonna be networked is it
                On.RoR2.CharacterMaster.RespawnExtraLife += CheckRespawn;
            }

            public override void OnUninstall() {
                base.OnUninstall();

                On.RoR2.CharacterMaster.RespawnExtraLife -= CheckRespawn;
            }

            private void CheckRespawn(On.RoR2.CharacterMaster.orig_RespawnExtraLife orig, CharacterMaster self) {
                orig(self);

                bool fucker = self.GetBody() == base.GetCurrentBody();
                if (fucker) {

                    base.Grant();
                }
            }
        }
    }
}