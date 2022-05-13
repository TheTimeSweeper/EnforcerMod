using RoR2;

namespace EnforcerPlugin.Achievements
{
    public class DoomAchievement2 : GenericModdedUnlockable
    {
        public override string AchievementTokenPrefix => "ENFORCER_DOOMINTERNAL";
        public override string PrerequisiteUnlockableIdentifier => "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_ID";

        public override string AchievementSpriteName => "texClassicAchievement";

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
            protected override int impRequirement => 4;
            protected override BodyIndex impBodyIndex => BodyCatalog.FindBodyIndex("ImpBossBody");
        }
    }
}