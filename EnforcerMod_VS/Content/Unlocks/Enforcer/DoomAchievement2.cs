using Modules;
using RoR2;
using RoR2.Achievements;

namespace EnforcerPlugin.Achievements
{
    [RegisterAchievement(identifier, unlockableIdentifier, "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_ID", 5, typeof(DoomAchievement2Server))]
    public class DoomAchievement2 : BaseAchievement
    {
        public const string identifier = "ENFORCER_DOOMINTERNALUNLOCKABLE_ACHIEVEMENT_ID" + knee.grow;
        public const string unlockableIdentifier = "ENFORCER_DOOMINTERNALUNLOCKABLE_REWARD_ID" + knee.grow;
        public const string AchievementSpriteName = "texDoomAchievement";

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("EnforcerBody");
        }

        // Token: 0x06003926 RID: 14630 RVA: 0x000DD289 File Offset: 0x000DB489
        public override void OnBodyRequirementMet()
        {
            base.OnBodyRequirementMet();

            base.SetServerTracked(true);
        }

        // Token: 0x06003927 RID: 14631 RVA: 0x000DD298 File Offset: 0x000DB498
        public override void OnBodyRequirementBroken()
        {
            base.SetServerTracked(false);

            base.OnBodyRequirementBroken();
        }

        public class DoomAchievement2Server : DoomAchievement.DoomAchievementServer
        {
            protected override int impRequirement => 10;
            protected override BodyIndex impBodyIndex => BodyCatalog.FindBodyIndex("ImpBossBody");

            protected override void onGrant() {

                base.GetCurrentBody().GetComponent<EnforcerNetworkComponent>().Uhh(Skins.getEnforcerSkinIndex(Skins.EnforcerSkin.RECOLORDOOM));
                Util.PlaySound(Modules.Sounds.DOOM, base.GetCurrentBody().gameObject);
            }
        }
    }
}