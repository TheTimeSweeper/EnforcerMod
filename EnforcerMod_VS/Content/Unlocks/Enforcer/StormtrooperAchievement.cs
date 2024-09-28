using Modules;
using RoR2;
using RoR2.Achievements;

namespace EnforcerPlugin.Achievements {

    [RegisterAchievement(identifier, unlockableIdentifier, "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_ID", 5, typeof(StormtrooperAchievementServer))]
    public class StormtrooperAchievement : BaseAchievement
    {
        public const string identifier = "ENFORCER_STORMTROOPERUNLOCKABLE_ACHIEVEMENT_ID" + knee.grow;
        public const string unlockableIdentifier = "ENFORCER_STORMTROOPERUNLOCKABLE_REWARD_ID" + knee.grow;
        public const string AchievementSpriteName = "texStormtrooperAchievement";

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

        public class StormtrooperAchievementServer : RoR2.Achievements.BaseServerAchievement {

            public override void OnInstall() {
                base.OnInstall();

                GlobalEventManager.onCharacterDeathGlobal += onCharacterDeathGlobal;
            }

            public override void OnUninstall() {
                base.OnUninstall();

                GlobalEventManager.onCharacterDeathGlobal -= onCharacterDeathGlobal;
            }

            private void onCharacterDeathGlobal(DamageReport report) {

                if (!report.victimBody) return;
                if (report.damageInfo is null) return;

                if (report.victimBodyIndex == BodyCatalog.FindBodyIndex("RoboBallBossBody") || report.victimBodyIndex == BodyCatalog.FindBodyIndex("SuperRoboBallBossBody")) {

                    if (report.victimBody.isElite) {
                        base.Grant();

                        base.GetCurrentBody().GetComponent<EnforcerNetworkComponent>().Uhh(Skins.getEnforcerSkinIndex(Skins.EnforcerSkin.RECOLORSTORM));
                    }
                }
            }
        }
    }
}