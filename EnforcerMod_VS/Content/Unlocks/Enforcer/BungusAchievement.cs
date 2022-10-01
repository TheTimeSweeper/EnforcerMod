using Modules;
using RoR2;

namespace EnforcerPlugin.Achievements {
    public class BungusAchievement : GenericModdedUnlockable {

        public override string AchievementTokenPrefix => "ENFORCER_BUNGUS" + knee.grow;
        public override string PrerequisiteUnlockableIdentifier => "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_ID";

        public override string AchievementSpriteName => "texBungusAchievement";

        public override BodyIndex LookUpRequiredBodyIndex() {
            return BodyCatalog.FindBodyIndex("EnforcerBody");
        }

        public static float bungusTime = 180f;

        public void BungusCheck(float bungus) {
            if (base.meetsBodyRequirement && bungus >= BungusAchievement.bungusTime) {
                base.Grant();
                //localUser.cachedBody.GetComponent<EnforcerNetworkComponent>().Uhh(Skins.getEnforcerSkinIndex(Skins.EnforcerSkin.RECOLORENGI));
                localUser.cachedBody.GetComponent<EnforcerNetworkComponent>().UhhBungus(true);
            }
        }

        public override void OnInstall() {
            base.OnInstall();

            EntityStates.Enforcer.EnforcerMain.Bungus += BungusCheck;
        }

        public override void OnUninstall() {
            base.OnUninstall();

            EntityStates.Enforcer.EnforcerMain.Bungus -= BungusCheck;
        }
    }
}