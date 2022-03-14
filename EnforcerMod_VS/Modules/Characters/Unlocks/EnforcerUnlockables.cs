using RoR2;
using R2API;
using EnforcerPlugin.Achievements;
using EnforcerPlugin;

namespace Modules {
    public static class EnforcerUnlockables {

        public static UnlockableDef enforcerUnlockableDef;
        public static UnlockableDef enforcerMasteryUnlockableDef;
        public static UnlockableDef enforcerGrandMasteryUnlockableDef;

        //skills
        public static UnlockableDef enforcerARUnlockableDef;
        public static UnlockableDef enforcerDoomUnlockableDef;
        public static UnlockableDef enforcerStunGrenadeUnlockableDef;

        //extra skins
        public static UnlockableDef enforcerRobitUnlockableDef;
        public static UnlockableDef enforcerClassicUnlockableDef;
        public static UnlockableDef enforcerNemesisSkinUnlockableDef;

        //Nemforcer
        public static UnlockableDef nemesisUnlockableDef;
        public static UnlockableDef nemMasteryUnlockableDef;
        public static UnlockableDef nemGrandMasteryUnlockableDef;

        public static void RegisterUnlockables() {

            //this is the version that works with the altered AddUnlockable I changed in R2API.
            //look at #r2api in the discord to see what I mean. I went into more detail in #development as well
            //      lol fucking that was a year ago you have to search from:timesweeper i was retarded for saying this
            //if the pull requests gets accepted I'll add the other needed ones to this
            //      fucking it was never merged in i'm reeing so hard right now
            enforcerUnlockableDef = Config.forceUnlock.Value ? null : Unlockables.AddUnlockable<EnforcerUnlockAchievement>(typeof(EnforcerUnlockAchievement.EnforcerUnlockAchievementServer));
            enforcerMasteryUnlockableDef = Unlockables.AddUnlockable<MasteryAchievementButEpic>();
            enforcerGrandMasteryUnlockableDef = Unlockables.AddUnlockable<GrandMasteryAchievement>();

            enforcerDoomUnlockableDef = Unlockables.AddUnlockable<DoomAchievement>(typeof(DoomAchievement.DoomAchievementServer));
            enforcerARUnlockableDef = Unlockables.AddUnlockable<AssaultRifleAchievement>();
            enforcerStunGrenadeUnlockableDef = Unlockables.AddUnlockable<StunGrenadeAchievement>();

            enforcerRobitUnlockableDef = Unlockables.AddUnlockable<RobitAchievement>(typeof(RobitAchievement.RobitAchievementServer));

            enforcerNemesisSkinUnlockableDef = Unlockables.AddUnlockable<RecolorNemesisSkinAchievement>();

            enforcerClassicUnlockableDef = Unlockables.AddUnlockable<ClassicAchievement>();

            //if (!Config.hateFun.Value)
            //{
            //   enforcerDoom2UnlockableDef = Unlockables.AddUnlockable<DoomAchievement2>(typeof(DoomAchievement2.DoomAchievement2Server));
            //    UnlockablesAPI.AddUnlockable<Achievements.DesperadoAchievement>(true);
            //    UnlockablesAPI.AddUnlockable<Achievements.BungusAchievement>(true);
            //    UnlockablesAPI.AddUnlockable<Achievements.StormtrooperAchievement>(true);
            //}

            //if (Config.cursed.Value)
            //{
            //    UnlockablesAPI.AddUnlockable<Achievements.FrogAchievement>(true);
            //    UnlockablesAPI.AddUnlockable<Achievements.SteveAchievement>(true);
            //}

            nemesisUnlockableDef = Unlockables.AddUnlockable<NemesisAchievement>();
            nemMasteryUnlockableDef = Unlockables.AddUnlockable<NemMasteryAchievement>();
            nemGrandMasteryUnlockableDef = Unlockables.AddUnlockable<NemGrandMasteryAchievement>();
            //    UnlockablesAPI.AddUnlockable<Achievements.NemDominanceAchievement>(true);
        }
    }
}

namespace EnforcerPlugin.Achievements {

    #region fuck

    //public class BungusAchievement : ModdedUnlockableAndAchievement<CustomSpriteProvider>
    //{
    //    public override String AchievementIdentifier { get; } = "ENFORCER_BUNGUSUNLOCKABLE_ACHIEVEMENT_ID";
    //    public override String UnlockableIdentifier { get; } = "ENFORCER_BUNGUSUNLOCKABLE_REWARD_ID";
    //    public override String PrerequisiteUnlockableIdentifier { get; } = "ENFORCER_BUNGUSUNLOCKABLE_PREREQ_ID";
    //    public override String AchievementNameToken { get; } = "ENFORCER_BUNGUSUNLOCKABLE_ACHIEVEMENT_NAME";
    //    public override String AchievementDescToken { get; } = "ENFORCER_BUNGUSUNLOCKABLE_ACHIEVEMENT_DESC";
    //    public override String UnlockableNameToken { get; } = "ENFORCER_BUNGUSUNLOCKABLE_UNLOCKABLE_NAME";
    //    protected override CustomSpriteProvider SpriteProvider { get; } = new CustomSpriteProvider("@Enforcer:Assets/Enforcer/EnforcerAssets/Icons/texBungusAchievement.png");

    //    public static float bungusTime = 240f;

    //    public override int LookUpRequiredBodyIndex()
    //    {
    //        return BodyCatalog.FindBodyIndex("EnforcerBody");
    //    }

    //    public void BungusCheck(float bungus)
    //    {
    //        if (base.meetsBodyRequirement && bungus >= BungusAchievement.bungusTime) base.Grant();
    //    }

    //    public override void OnInstall()
    //    {
    //        base.OnInstall();

    //        EntityStates.Enforcer.EnforcerMain.Bungus += BungusCheck;
    //    }

    //    public override void OnUninstall()
    //    {
    //        base.OnUninstall();

    //        EntityStates.Enforcer.EnforcerMain.Bungus -= BungusCheck;
    //    }
    //}

    //public class DesperadoAchievement : ModdedUnlockableAndAchievement<CustomSpriteProvider>
    //{
    //    public override String AchievementIdentifier { get; } = "ENFORCER_DESPERADOUNLOCKABLE_ACHIEVEMENT_ID";
    //    public override String UnlockableIdentifier { get; } = "ENFORCER_DESPERADOUNLOCKABLE_REWARD_ID";
    //    public override String PrerequisiteUnlockableIdentifier { get; } = "ENFORCER_DESPERADOUNLOCKABLE_PREREQ_ID";
    //    public override String AchievementNameToken { get; } = "ENFORCER_DESPERADOUNLOCKABLE_ACHIEVEMENT_NAME";
    //    public override String AchievementDescToken { get; } = "ENFORCER_DESPERADOUNLOCKABLE_ACHIEVEMENT_DESC";
    //    public override String UnlockableNameToken { get; } = "ENFORCER_DESPERADOUNLOCKABLE_UNLOCKABLE_NAME";
    //    protected override CustomSpriteProvider SpriteProvider { get; } = new CustomSpriteProvider("@Enforcer:Assets/Enforcer/EnforcerAssets/Icons/texDesperadoAchievement.png");

    //    public override int LookUpRequiredBodyIndex()
    //    {
    //        return BodyCatalog.FindBodyIndex("EnforcerBody");
    //    }

    //    private void CheckDeath(DamageReport report)
    //    {
    //        if (!report.victimBody) return;
    //        if (report.damageInfo is null) return;
    //        if (report.damageInfo.inflictor is null) return;

    //        GameObject inflictor = report.damageInfo.inflictor;
    //        if (!inflictor || !inflictor.GetComponent<MapZone>())
    //        {
    //            return;
    //        }

    //        if (report.victimBodyIndex == BodyCatalog.FindBodyIndex("TitanGoldBody") && report.victimBody.teamComponent.teamIndex != TeamIndex.Player)
    //        {
    //            if (base.meetsBodyRequirement) base.Grant();
    //        }
    //    }

    //    public override void OnInstall()
    //    {
    //        base.OnInstall();

    //        GlobalEventManager.onCharacterDeathGlobal += CheckDeath;
    //    }

    //    public override void OnUninstall()
    //    {
    //        base.OnUninstall();

    //        GlobalEventManager.onCharacterDeathGlobal -= CheckDeath;
    //    }
    //}

    //public class StormtrooperAchievement : ModdedUnlockableAndAchievement<CustomSpriteProvider>
    //{
    //    public override String AchievementIdentifier { get; } = "ENFORCER_STORMTROOPERUNLOCKABLE_ACHIEVEMENT_ID";
    //    public override String UnlockableIdentifier { get; } = "ENFORCER_STORMTROOPERUNLOCKABLE_REWARD_ID";
    //    public override String PrerequisiteUnlockableIdentifier { get; } = "ENFORCER_STORMTROOPERUNLOCKABLE_PREREQ_ID";
    //    public override String AchievementNameToken { get; } = "ENFORCER_STORMTROOPERUNLOCKABLE_ACHIEVEMENT_NAME";
    //    public override String AchievementDescToken { get; } = "ENFORCER_STORMTROOPERUNLOCKABLE_ACHIEVEMENT_DESC";
    //    public override String UnlockableNameToken { get; } = "ENFORCER_STORMTROOPERUNLOCKABLE_UNLOCKABLE_NAME";
    //    protected override CustomSpriteProvider SpriteProvider { get; } = new CustomSpriteProvider("@Enforcer:Assets/Enforcer/EnforcerAssets/Icons/texStormtrooperAchievement.png");

    //    public override int LookUpRequiredBodyIndex()
    //    {
    //        return BodyCatalog.FindBodyIndex("EnforcerBody");
    //    }

    //    private void CheckDeath(DamageReport report)
    //    {
    //        if (!report.victimBody) return;
    //        if (report.damageInfo is null) return;

    //        //hard coding this i guess since someone reported it only works on malachites
    //        //i think that's bullshit but couldn't hurt to do it this way
    //        if (report.victimBodyIndex == BodyCatalog.FindBodyIndex("RoboBallBossBody"))
    //        {
    //            bool flag = false;

    //            if (report.victimBody.HasBuff(BuffIndex.AffixBlue) || report.victimBody.HasBuff(BuffIndex.AffixHaunted) || report.victimBody.HasBuff(BuffIndex.AffixPoison) || report.victimBody.HasBuff(BuffIndex.AffixRed) || report.victimBody.HasBuff(BuffIndex.AffixWhite)) flag = true;

    //            if (flag && base.meetsBodyRequirement) base.Grant();
    //        }
    //    }

    //    public override void OnInstall()
    //    {
    //        base.OnInstall();

    //        GlobalEventManager.onCharacterDeathGlobal += CheckDeath;
    //    }

    //    public override void OnUninstall()
    //    {
    //        base.OnUninstall();

    //        GlobalEventManager.onCharacterDeathGlobal -= CheckDeath;
    //    }
    //}

    //public class FrogAchievement : ModdedUnlockableAndAchievement<CustomSpriteProvider>
    //{
    //    public override String AchievementIdentifier { get; } = "ENFORCER_FROGUNLOCKABLE_ACHIEVEMENT_ID";
    //    public override String UnlockableIdentifier { get; } = "ENFORCER_FROGUNLOCKABLE_REWARD_ID";
    //    public override String PrerequisiteUnlockableIdentifier { get; } = "ENFORCER_FROGUNLOCKABLE_PREREQ_ID";
    //    public override String AchievementNameToken { get; } = "ENFORCER_FROGUNLOCKABLE_ACHIEVEMENT_NAME";
    //    public override String AchievementDescToken { get; } = "ENFORCER_FROGUNLOCKABLE_ACHIEVEMENT_DESC";
    //    public override String UnlockableNameToken { get; } = "ENFORCER_FROGUNLOCKABLE_UNLOCKABLE_NAME";
    //    protected override CustomSpriteProvider SpriteProvider { get; } = new CustomSpriteProvider("@Enforcer:Assets/Enforcer/EnforcerAssets/Icons/texZeroSuitAchievement.png");

    //    public override int LookUpRequiredBodyIndex()
    //    {
    //        return BodyCatalog.FindBodyIndex("EnforcerBody");
    //    }

    //    private void Froge(bool cum)
    //    {
    //        if (cum && base.meetsBodyRequirement) base.Grant();
    //    }

    //    public override void OnInstall()
    //    {
    //        base.OnInstall();

    //        EnforcerFrogComponent.FrogGet += Froge;
    //    }

    //    public override void OnUninstall()
    //    {
    //        base.OnUninstall();

    //        EnforcerFrogComponent.FrogGet -= Froge;
    //    }
    //}

    //public class SteveAchievement : ModdedUnlockableAndAchievement<CustomSpriteProvider>
    //{
    //    public override String AchievementIdentifier { get; } = "ENFORCER_STEVEUNLOCKABLE_ACHIEVEMENT_ID";
    //    public override String UnlockableIdentifier { get; } = "ENFORCER_STEVEUNLOCKABLE_REWARD_ID";
    //    public override String PrerequisiteUnlockableIdentifier { get; } = "ENFORCER_STEVEUNLOCKABLE_PREREQ_ID";
    //    public override String AchievementNameToken { get; } = "ENFORCER_STEVEUNLOCKABLE_ACHIEVEMENT_NAME";
    //    public override String AchievementDescToken { get; } = "ENFORCER_STEVEUNLOCKABLE_ACHIEVEMENT_DESC";
    //    public override String UnlockableNameToken { get; } = "ENFORCER_STEVEUNLOCKABLE_UNLOCKABLE_NAME";
    //    protected override CustomSpriteProvider SpriteProvider { get; } = new CustomSpriteProvider("@Enforcer:Assets/Enforcer/EnforcerAssets/Icons/texSbeveAchievement.png");

    //    public override int LookUpRequiredBodyIndex()
    //    {
    //        return BodyCatalog.FindBodyIndex("EnforcerBody");
    //    }

    //    private void Blocked(bool cum)
    //    {
    //        if (cum && base.meetsBodyRequirement) base.Grant();
    //    }

    //    public override void OnInstall()
    //    {
    //        base.OnInstall();

    //        ShieldComponent.BlockedGet += Blocked;
    //    }

    //    public override void OnUninstall()
    //    {
    //        base.OnUninstall();

    //        ShieldComponent.BlockedGet -= Blocked;
    //    }
    //}

    //public class PigAchievement : ModdedUnlockableAndAchievement<CustomSpriteProvider>
    //{
    //    public override String AchievementIdentifier { get; } = "ENFORCER_PIGUNLOCKABLE_ACHIEVEMENT_ID";
    //    public override String UnlockableIdentifier { get; } = "ENFORCER_PIGUNLOCKABLE_REWARD_ID";
    //    public override String PrerequisiteUnlockableIdentifier { get; } = "ENFORCER_PIGUNLOCKABLE_PREREQ_ID";
    //    public override String AchievementNameToken { get; } = "ENFORCER_PIGUNLOCKABLE_ACHIEVEMENT_NAME";
    //    public override String AchievementDescToken { get; } = "ENFORCER_PIGUNLOCKABLE_ACHIEVEMENT_DESC";
    //    public override String UnlockableNameToken { get; } = "ENFORCER_PIGUNLOCKABLE_UNLOCKABLE_NAME";
    //    protected override CustomSpriteProvider SpriteProvider { get; } = new CustomSpriteProvider("@Enforcer:Assets/Enforcer/EnforcerAssets/Icons/texPigAchievement.png");

    //    public override int LookUpRequiredBodyIndex()
    //    {
    //        return BodyCatalog.FindBodyIndex("EnforcerBody");
    //    }

    //    private void CheckDeath(DamageReport report)
    //    {
    //        if (report is null) return;
    //        if (report.victimBody is null) return;
    //        if (report.attackerBody is null) return;

    //        if (report.victimTeamIndex != TeamIndex.Player)
    //        {
    //            if (report.victimBodyIndex == BodyCatalog.FindBodyIndex("EnforcerBody")) base.Grant();
    //        }
    //    }

    //    private void Check(bool cum)
    //    {
    //        if (cum && base.meetsBodyRequirement) base.Grant();
    //    }

    //    public override void OnInstall()
    //    {
    //        base.OnInstall();

    //        GlobalEventManager.onCharacterDeathGlobal += CheckDeath;
    //    }

    //    public override void OnUninstall()
    //    {
    //        base.OnUninstall();

    //        GlobalEventManager.onCharacterDeathGlobal -= CheckDeath;
    //    }
    //}

    //public class NemGrandMasteryAchievement : ModdedUnlockableAndAchievement<CustomSpriteProvider>
    //{
    //    public override String AchievementIdentifier { get; } = "NEMFORCER_TYPHOONUNLOCKABLE_ACHIEVEMENT_ID";
    //    public override String UnlockableIdentifier { get; } = "NEMFORCER_TYPHOONUNLOCKABLE_REWARD_ID";
    //    public override String PrerequisiteUnlockableIdentifier { get; } = "NEMFORCER_TYPHOONUNLOCKABLE_PREREQ_ID";
    //    public override String AchievementNameToken { get; } = "NEMFORCER_TYPHOONUNLOCKABLE_ACHIEVEMENT_NAME";
    //    public override String AchievementDescToken { get; } = "NEMFORCER_TYPHOONUNLOCKABLE_ACHIEVEMENT_DESC";
    //    public override String UnlockableNameToken { get; } = "NEMFORCER_TYPHOONUNLOCKABLE_UNLOCKABLE_NAME";
    //    protected override CustomSpriteProvider SpriteProvider { get; } = new CustomSpriteProvider("@Enforcer:Assets/Enforcer/EnforcerAssets/Icons/texNemforcerGrandMastery.png");

    //    public override int LookUpRequiredBodyIndex()
    //    {
    //        return BodyCatalog.FindBodyIndex("NemesisEnforcerBody");
    //    }

    //    public void ClearCheck(Run run, RunReport runReport)
    //    {
    //        if (run is null) return;
    //        if (runReport is null) return;

    //        if (!runReport.gameEnding) return;

    //        if (runReport.gameEnding.isWin)
    //        {
    //            DifficultyDef difficultyDef = DifficultyCatalog.GetDifficultyDef(runReport.ruleBook.FindDifficulty());

    //            if (difficultyDef != null && difficultyDef.nameToken == "DIFFICULTY_TYPHOON_NAME")
    //            {
    //                if (base.meetsBodyRequirement)
    //                {
    //                    base.Grant();
    //                }
    //            }
    //        }
    //    }

    //    public override void OnInstall()
    //    {
    //        base.OnInstall();

    //        Run.onClientGameOverGlobal += this.ClearCheck;
    //    }

    //    public override void OnUninstall()
    //    {
    //        base.OnUninstall();

    //        Run.onClientGameOverGlobal -= this.ClearCheck;
    //    }
    //}

    //public class NemDominanceAchievement : ModdedUnlockableAndAchievement<CustomSpriteProvider>
    //{
    //    public override String AchievementIdentifier { get; } = "NEMFORCER_DOMINANCEUNLOCKABLE_ACHIEVEMENT_ID";
    //    public override String UnlockableIdentifier { get; } = "NEMFORCER_DOMINANCEUNLOCKABLE_REWARD_ID";
    //    public override String PrerequisiteUnlockableIdentifier { get; } = "NEMFORCER_DOMINANCEUNLOCKABLE_PREREQ_ID";
    //    public override String AchievementNameToken { get; } = "NEMFORCER_DOMINANCEUNLOCKABLE_ACHIEVEMENT_NAME";
    //    public override String AchievementDescToken { get; } = "NEMFORCER_DOMINANCEUNLOCKABLE_ACHIEVEMENT_DESC";
    //    public override String UnlockableNameToken { get; } = "NEMFORCER_DOMINANCEUNLOCKABLE_UNLOCKABLE_NAME";
    //    protected override CustomSpriteProvider SpriteProvider { get; } = new CustomSpriteProvider("@Enforcer:Assets/Enforcer/EnforcerAssets/Icons/texNemforcerEnforcer.png");

    //    public override int LookUpRequiredBodyIndex()
    //    {
    //        return BodyCatalog.FindBodyIndex("NemesisEnforcerBody");
    //    }

    //    public void Bonked(Run run)
    //    {
    //        if (run is null) return;

    //        if (base.meetsBodyRequirement)
    //        {
    //            base.Grant();
    //        }
    //    }

    //    public override void OnInstall()
    //    {
    //        base.OnInstall();

    //        EntityStates.Nemforcer.HammerSlam.Bonked += this.Bonked;
    //    }

    //    public override void OnUninstall()
    //    {
    //        base.OnUninstall();

    //        EntityStates.Nemforcer.HammerSlam.Bonked -= this.Bonked;
    //    }
    //}
    #endregion

}