using RoR2;
using RoR2.Achievements;

namespace EnforcerPlugin.Achievements
{

    public class RobitAchievement : GenericModdedUnlockable {
        static string nig = "";

        public override string AchievementTokenPrefix => "ENFORCER_ROBIT" + nig;
        public override string PrerequisiteUnlockableIdentifier => "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_ID";

        public override string AchievementSpriteName => "texNemforcerEnforcer";

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
                    self.characterModel.body.GetComponent<EnforcerNetworkComponent>().RpcUhh(3);
                    On.RoR2.ModelSkinController.ApplySkin -= ModelSkinController_ApplySkin;
                }
                orig(self, skinIndex);
            }
        }
    }
}