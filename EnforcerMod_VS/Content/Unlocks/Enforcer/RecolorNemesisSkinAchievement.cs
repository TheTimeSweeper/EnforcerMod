using Modules;
using RoR2;

namespace EnforcerPlugin.Achievements {
    public class RecolorNemesisSkinAchievement : GenericModdedUnlockable {

        public override string AchievementTokenPrefix => "ENFORCER_NEMESISSKIN" + knee.grow;
        public override string PrerequisiteUnlockableIdentifier => "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_ID";

        public override string AchievementSpriteName => "texNemforcerAchievement";

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

        public class RecolorNemesisSkinAchievementServer : RoR2.Achievements.BaseServerAchievement {


            private void Check() {

                base.Grant();

                base.GetCurrentBody().GetComponent<EnforcerNetworkComponent>().Uhh(Skins.getEnforcerSkinIndex(Skins.EnforcerSkin.RECOLORNEMESIS));
            }


            public override void OnInstall() {
                base.OnInstall();

                ArenaMissionController.onBeatArena += Check;
            }

            public override void OnUninstall() {
                base.OnUninstall();

                ArenaMissionController.onBeatArena -= Check;
            }
        }
    }
}