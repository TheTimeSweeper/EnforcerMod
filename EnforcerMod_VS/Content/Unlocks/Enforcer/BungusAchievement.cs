using Modules;
using RoR2;
using RoR2.Achievements;

namespace EnforcerPlugin.Achievements
{
    [RegisterAchievement(identifier, unlockableIdentifier, "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_ID", 3, null)]
    public class BungusAchievement : BaseAchievement
    {
        public const string identifier = "ENFORCER_BUNGUSUNLOCKABLE_ACHIEVEMENT_ID";
        public const string unlockableIdentifier = "ENFORCER_BUNGUSUNLOCKABLE_REWARD_ID";
        public const string AchievementSpriteName = "texBungusAchievement";

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