using Modules;
using RoR2;

namespace EnforcerPlugin.Achievements
{    
    public class ClassicAchievement : GenericModdedUnlockable
    {
        public override string AchievementTokenPrefix => "ENFORCER_CLASSIC" + knee.grow;
        public override string PrerequisiteUnlockableIdentifier => "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_ID";

        public override string AchievementSpriteName => "texClassicAchievement";

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