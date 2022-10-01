using RoR2;
using UnityEngine;

namespace EnforcerPlugin.Achievements {
    public class SteveAchievement : GenericModdedUnlockable {
        public override string AchievementTokenPrefix => "ENFORCER_STEVE" + knee.grow;
        public override string PrerequisiteUnlockableIdentifier => "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_ID";

        public override string AchievementSpriteName => "texSbeveAchievement";

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