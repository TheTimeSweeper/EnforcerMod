using Modules;
using RoR2;

namespace EnforcerPlugin.Achievements {

    public class StormtrooperAchievement : GenericModdedUnlockable {
        public override string AchievementTokenPrefix => "ENFORCER_STORMTROOPER" + knee.grow;
        public override string PrerequisiteUnlockableIdentifier => "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_ID";

        public override string AchievementSpriteName => "texStormtrooperAchievement";

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