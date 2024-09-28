using Modules;
using RoR2;
using RoR2.Achievements;

namespace EnforcerPlugin.Achievements
{
    [RegisterAchievement(identifier, unlockableIdentifier, "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_ID", 1, null)]
    public class ClassicAchievement : BaseAchievement
    {
        public const string identifier = "ENFORCER_CLASSICUNLOCKABLE_ACHIEVEMENT_ID" + knee.grow;
        public const string unlockableIdentifier = "ENFORCER_CLASSICUNLOCKABLE_REWARD_ID" + knee.grow;
        public const string AchievementSpriteName = "texClassicAchievement";

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("EnforcerBody");
        }

        private void Schmoovin(bool isDancing)
        {
            if (base.meetsBodyRequirement && isDancing) {
                base.Grant();

                localUser.cachedBody.GetComponent<EnforcerNetworkComponent>().Uhh(Skins.getEnforcerSkinIndex(Skins.EnforcerSkin.CLASSIC));
            }
        }

        public override void OnInstall()
        {
            base.OnInstall();

            EntityStates.Enforcer.EnforcerMain.onDance += Schmoovin;
        }

        public override void OnUninstall()
        {
            base.OnUninstall();

            EntityStates.Enforcer.EnforcerMain.onDance -= Schmoovin;
        }
    }
}