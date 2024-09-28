using Modules;
using RoR2;
using RoR2.Achievements;

namespace EnforcerPlugin.Achievements {
    [RegisterAchievement(identifier, unlockableIdentifier, "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_ID", 3, typeof(RecolorNemesisSkinAchievementServer))]
    public class RecolorNemesisSkinAchievement : BaseAchievement
    {
        public const string identifier = "ENFORCER_NEMESISSKINUNLOCKABLE_ACHIEVEMENT_ID" + knee.grow;
        public const string unlockableIdentifier = "ENFORCER_NEMESISSKINUNLOCKABLE_REWARD_ID" + knee.grow;
        public const string AchievementSpriteName = "texNemSkinAchievement";

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