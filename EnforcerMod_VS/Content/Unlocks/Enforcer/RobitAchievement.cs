using Modules;
using RoR2;
using RoR2.Achievements;

namespace EnforcerPlugin.Achievements {

    [RegisterAchievement(identifier, unlockableIdentifier, "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_ID", 3, typeof(RobitAchievementServer))]
    public class RobitAchievement : BaseAchievement
    {
        public const string identifier = "ENFORCER_ROBITUNLOCKABLE_ACHIEVEMENT_ID" + knee.grow;
        public const string unlockableIdentifier = "ENFORCER_ROBITUNLOCKABLE_REWARD_ID" + knee.grow;
        public const string AchievementSpriteName = "texN4CRAchievement";

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

            private static bool _applyingSkin = false;

            public override void OnInstall() {
                base.OnInstall();

                //oh god this isn't gonna be networked is it
                On.RoR2.CharacterMaster.RespawnExtraLife += CheckRespawnExtraLife;
            }

            public override void OnUninstall() {
                base.OnUninstall();
                
                On.RoR2.CharacterMaster.RespawnExtraLife -= CheckRespawnExtraLife;
            }

            private void CheckRespawnExtraLife(On.RoR2.CharacterMaster.orig_RespawnExtraLife orig, CharacterMaster self) {
                orig(self);

                bool fucker = self == base.GetCurrentBody().master;

                if (fucker) {
                    _applyingSkin = true;
                    base.Grant();

                    On.RoR2.ModelSkinController.ApplySkin += ModelSkinController_ApplySkin;
                }
            }

            private void ModelSkinController_ApplySkin(On.RoR2.ModelSkinController.orig_ApplySkin orig, ModelSkinController self, int skinIndex) {

                //bool fucker = base.GetCurrentBody() == self.GetComponent<CharacterModel>().body;
                if (_applyingSkin) {
                    skinIndex = 3;
                    self.characterModel.body.GetComponent<EnforcerNetworkComponent>().Uhh(Skins.getEnforcerSkinIndex(Skins.EnforcerSkin.ROBIT));
                    On.RoR2.ModelSkinController.ApplySkin -= ModelSkinController_ApplySkin;
                }
                orig(self, skinIndex);
            }
        }
    }
}