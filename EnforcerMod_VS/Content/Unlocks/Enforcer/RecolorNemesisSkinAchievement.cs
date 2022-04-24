using RoR2;

namespace EnforcerPlugin.Achievements {
    public class RecolorNemesisSkinAchievement : GenericModdedUnlockable {

        public override string AchievementTokenPrefix => "ENFORCER_NEMESISSKIN";
        public override string PrerequisiteUnlockableIdentifier => "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_ID";

        public override string AchievementSpriteName => "texNemforcerAchievement";

        public override BodyIndex LookUpRequiredBodyIndex() {
            return BodyCatalog.FindBodyIndex("EnforcerBody");
        }

        private void Check()
        {
            if (base.meetsBodyRequirement) {
                base.Grant();

                localUser.cachedBody.GetComponent<EnforcerNetworkComponent>().RpcUhh(6);
            }
        }

        public override void OnInstall()
        {
            base.OnInstall();

            ArenaMissionController.onBeatArena += Check;
        }

        public override void OnUninstall()
        {
            base.OnUninstall();

            ArenaMissionController.onBeatArena -= Check;
        }
    }
}