using RoR2;
using RoR2.Achievements;
using UnityEngine;

namespace EnforcerPlugin.Achievements {
    [RegisterAchievement(identifier, unlockableIdentifier, "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_ID", 1, typeof(SteveAchievementServer))]
    public class SteveAchievement : BaseAchievement
    {
        public const string identifier = "ENFORCER_STEVEUNLOCKABLE_ACHIEVEMENT_ID" + knee.grow;
        public const string unlockableIdentifier = "ENFORCER_STEVEUNLOCKABLE_REWARD_ID" + knee.grow;
        public const string AchievementSpriteName = "texSbeveAchievement";

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

        public class SteveAchievementServer : RoR2.Achievements.BaseServerAchievement {

            public override void OnInstall() {
                base.OnInstall();

                EnforcerComponent.BlockedGet += Blocked;
            }

            public override void OnUninstall() {
                base.OnUninstall();

                EnforcerComponent.BlockedGet -= Blocked;
            }

            private void Blocked(GameObject cum) {
                //don't apply the skin cause that's kinda silly just for blocking
                if (cum == base.GetCurrentBody().gameObject) base.Grant();
            }
        }
    }
}