using RoR2;
using R2API;
using EnforcerPlugin.Achievements;
using Unlockables = Enforcer.Modules.Unlockables;

namespace EnforcerPlugin.Modules
{
    public static class EnforcerUnlockables {

        public static UnlockableDef enforcerUnlockableDef;
        public static UnlockableDef enforcerMasteryUnlockableDef;
        public static UnlockableDef enforcerGrandMasteryUnlockableDef;

        public static UnlockableDef enforcerARUnlockableDef;
        public static UnlockableDef enforcerDoomUnlockableDef;
        public static UnlockableDef enforcerStunGrenadeUnlockableDef;

        public static UnlockableDef enforcerRobitUnlockableDef;

        public static UnlockableDef nemesisUnlockableDef;
        public static UnlockableDef nemMasteryUnlockableDef;
        public static UnlockableDef nemGrandMasteryUnlockableDef;

        public static void RegisterUnlockables() {
            RegisterTokens();
            //this is the version that works with the altered AddUnlockable I changed in R2API.
            //look at #r2api in the discord to see what I mean. I went into more detail in #development as well
            //      lol fucking that was a year ago you have to search from:timesweeper i was retarded for saying this
            //if the pull requests gets accepted I'll add the other needed ones to this
            //      fucking it was never merged in i'm reeing so hard right now
            enforcerUnlockableDef = Config.forceUnlock.Value ? null : Unlockables.AddUnlockable<EnforcerUnlockAchievement>(typeof(EnforcerUnlockAchievement.EnforcerUnlockAchievementServer));
            enforcerMasteryUnlockableDef = Unlockables.AddUnlockable<MasteryAchievementButEpic>();
            //enforcerGrandMasteryUnlockableDef = Unlockables.AddUnlockable<GrandMasteryAchievement>();
            
            enforcerDoomUnlockableDef = Unlockables.AddUnlockable<DoomAchievement>(typeof(DoomAchievement.DoomAchievementServer));
            enforcerARUnlockableDef = Unlockables.AddUnlockable<AssaultRifleAchievement>();
            enforcerStunGrenadeUnlockableDef = Unlockables.AddUnlockable<StunGrenadeAchievement>();

            enforcerRobitUnlockableDef = Unlockables.AddUnlockable<RobitAchievement>(typeof(RobitAchievement.RobitAchievementServer));

            //if (!Config.hateFun.Value)
            //{
            //    UnlockablesAPI.AddUnlockable<Achievements.BungusAchievement>(true);
            //    UnlockablesAPI.AddUnlockable<Achievements.StormtrooperAchievement>(true);
            //}

            //if (Config.cursed.Value)
            //{
            //    UnlockablesAPI.AddUnlockable<Achievements.FrogAchievement>(true);
            //    UnlockablesAPI.AddUnlockable<Achievements.SteveAchievement>(true);
            //}

            //UnlockablesAPI.AddUnlockable<Achievements.DesperadoAchievement>(true);
            //UnlockablesAPI.AddUnlockable<Achievements.NemesisSkinAchievement>(true);

            nemesisUnlockableDef = Unlockables.AddUnlockable<NemesisAchievement>();
            nemMasteryUnlockableDef = Unlockables.AddUnlockable<NemMasteryAchievement>();
            nemGrandMasteryUnlockableDef = Unlockables.AddUnlockable<NemGrandMasteryAchievement>();
            //    UnlockablesAPI.AddUnlockable<Achievements.NemDominanceAchievement>(true);
        }

        private static void RegisterTokens()
        {
            #region enf
            //character
            LanguageAPI.Add("ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_NAME", "Riot");
            LanguageAPI.Add("ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_DESC", "Kill a Magma Worm, a Wandering Vagrant and a Stone Titan in a single run.");
            LanguageAPI.Add("ENFORCER_CHARACTERUNLOCKABLE_UNLOCKABLE_NAME", "Riot");

            LanguageAPI.Add("ENFORCER_MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: Mastery");
            LanguageAPI.Add("ENFORCER_MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, beat the game or obliterate on Monsoon.");
            LanguageAPI.Add("ENFORCER_MASTERYUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: Mastery");

            string masteryFootnote = EnforcerModPlugin.starstormInstalled ? "" : "\n<color=#8888>(Typhoon difficulty requires Starstorm 2)</color>";

            LanguageAPI.Add("ENFORCER_GRANDMASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: Grand Mastery");
            LanguageAPI.Add("ENFORCER_GRANDMASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, beat the game or obliterate on Typhoon or higher." + masteryFootnote);
            LanguageAPI.Add("ENFORCER_GRANDMASTERYUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: Grand Mastery");

            //skills
            LanguageAPI.Add("ENFORCER_DOOMUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: Rip and Tear");
            LanguageAPI.Add("ENFORCER_DOOMUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, kill 40 imps in a single stage.");
            LanguageAPI.Add("ENFORCER_DOOMUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: Rip and Tear");

            LanguageAPI.Add("ENFORCER_RIFLEUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: Rapidfire");
            LanguageAPI.Add("ENFORCER_RIFLEUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, reach +400% attack speed.");
            LanguageAPI.Add("ENFORCER_RIFLEUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: Rapidfire");

            LanguageAPI.Add("ENFORCER_STUNGRENADEUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: Crowd Control");
            LanguageAPI.Add("ENFORCER_STUNGRENADEUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, have 20 enemies under the effects of Tear Gas at once.");
            LanguageAPI.Add("ENFORCER_STUNGRENADEUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: Crowd Control");

            //skans
            LanguageAPI.Add("ENFORCER_ROBITUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: Your Move, Creep");
            LanguageAPI.Add("ENFORCER_ROBITUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, be brought back to life.");
            LanguageAPI.Add("ENFORCER_ROBITUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: Your Move, Creep");

            //LanguageAPI.Add("ENFORCER_CLASSICUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: Schmoovin'");
            //LanguageAPI.Add("ENFORCER_CLASSICUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, show off your dance moves.");
            //LanguageAPI.Add("ENFORCER_CLASSICUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: Schmoovin'");

            //LanguageAPI.Add("ENFORCER_DESPERADOUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: Rules of Nature");
            //LanguageAPI.Add("ENFORCER_DESPERADOUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, Defeat the unique guardian of Gilded Coast by pushing it off the edge of the map. <color=#c11>Host only</color>");
            //LanguageAPI.Add("ENFORCER_DESPERADOUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: Rules of Nature");

            //    LanguageAPI.Add("ENFORCER_BUNGUSUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: Enforcing Perfection");
            //    LanguageAPI.Add("ENFORCER_BUNGUSUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, become one with the Bungus.");
            //    LanguageAPI.Add("ENFORCER_BUNGUSUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: Enforcing Perfection");

            //    LanguageAPI.Add("ENFORCER_STORMTROOPERUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: Long Live the Empire");
            //    LanguageAPI.Add("ENFORCER_STORMTROOPERUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, defeat an elite Solus Control Unit. <color=#c11>Host only</color>");
            //    LanguageAPI.Add("ENFORCER_STORMTROOPERUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: Long Live the Empire");

            //    LanguageAPI.Add("ENFORCER_FROGUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: Through Thick and Thin");
            //    LanguageAPI.Add("ENFORCER_FROGUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, make a friend on the moon.");
            //    LanguageAPI.Add("ENFORCER_FROGUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: Through Thick and Thin");

            //    LanguageAPI.Add("ENFORCER_STEVEUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: Blocks");
            //    LanguageAPI.Add("ENFORCER_STEVEUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, block an attack with your shield.");
            //    LanguageAPI.Add("ENFORCER_STEVEUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: Blocks");

            //LanguageAPI.Add("ENFORCER_NEMESISSKINUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: Clearance");
            //LanguageAPI.Add("ENFORCER_NEMESISSKINUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, stabilize the Cell in the Void Fields.");
            //LanguageAPI.Add("ENFORCER_NEMESISSKINUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: Clearance");
            #endregion

            #region nem
            //character
            LanguageAPI.Add("ENFORCER_NEMESIS2UNLOCKABLE_ACHIEVEMENT_NAME", "???");
            LanguageAPI.Add("ENFORCER_NEMESIS2UNLOCKABLE_ACHIEVEMENT_DESC", "On monsoon, stabilize the anomaly, and defeat Enforcer's Vestige.");
            LanguageAPI.Add("ENFORCER_NEMESIS2UNLOCKABLE_UNLOCKABLE_NAME", "???");

            LanguageAPI.Add("NEMFORCER_MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "Nemesis Enforcer: Mastery");
            LanguageAPI.Add("NEMFORCER_MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "As Nemesis Enforcer, beat the game or obliterate on Monsoon.");
            LanguageAPI.Add("NEMFORCER_MASTERYUNLOCKABLE_UNLOCKABLE_NAME", "Nemesis Enforcer: Mastery");

            LanguageAPI.Add("NEMFORCER_TYPHOONUNLOCKABLE_ACHIEVEMENT_NAME", "Nemesis Enforcer: Grand Mastery");
            LanguageAPI.Add("NEMFORCER_TYPHOONUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, beat the game or obliterate on Typhoon or higher." + masteryFootnote);
            LanguageAPI.Add("NEMFORCER_TYPHOONUNLOCKABLE_UNLOCKABLE_NAME", "Nemesis Enforcer: Grand Mastery");

            //sken
            LanguageAPI.Add("NEMFORCER_DOMINANCEUNLOCKABLE_ACHIEVEMENT_NAME", "Nemesis Enforcer: Demolition");
            LanguageAPI.Add("NEMFORCER_DOMINANCEUNLOCKABLE_ACHIEVEMENT_DESC", "As Nemesis Enforcer, destroy 5 projectiles at once with Dominance.");
            LanguageAPI.Add("NEMFORCER_DOMINANCEUNLOCKABLE_UNLOCKABLE_NAME", "Nemesis Enforcer: Demolition");
            #endregion
        }
    }
}

namespace EnforcerPlugin.Achievements
{

    #region fuck
    //public class GrandMasteryAchievement : ModdedUnlockableAndAchievement<CustomSpriteProvider>
    //{
    //    public override String AchievementIdentifier { get; } = "ENFORCER_TYPHOONUNLOCKABLE_ACHIEVEMENT_ID";
    //    public override String UnlockableIdentifier { get; } = "ENFORCER_TYPHOONUNLOCKABLE_REWARD_ID";
    //    public override String PrerequisiteUnlockableIdentifier { get; } = "ENFORCER_TYPHOONUNLOCKABLE_PREREQ_ID";
    //    public override String AchievementNameToken { get; } = "ENFORCER_TYPHOONUNLOCKABLE_ACHIEVEMENT_NAME";
    //    public override String AchievementDescToken { get; } = "ENFORCER_TYPHOONUNLOCKABLE_ACHIEVEMENT_DESC";
    //    public override String UnlockableNameToken { get; } = "ENFORCER_TYPHOONUNLOCKABLE_UNLOCKABLE_NAME";
    //    protected override CustomSpriteProvider SpriteProvider { get; } = new CustomSpriteProvider("@Enforcer:Assets/Enforcer/EnforcerAssets/Icons/texTyphoonAchievement.png");

    //    public override int LookUpRequiredBodyIndex()
    //    {
    //        return BodyCatalog.FindBodyIndex("EnforcerBody");
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

    //public class SuperShotgunAchievement : ModdedUnlockableAndAchievement<CustomSpriteProvider>
    //{
    //    public override String AchievementIdentifier { get; } = "ENFORCER_SHOTGUNUNLOCKABLE_ACHIEVEMENT_ID";
    //    public override String UnlockableIdentifier { get; } = "ENFORCER_SHOTGUNUNLOCKABLE_REWARD_ID";
    //    public override String PrerequisiteUnlockableIdentifier { get; } = "ENFORCER_SHOTGUNUNLOCKABLE_PREREQ_ID";
    //    public override String AchievementNameToken { get; } = "ENFORCER_SHOTGUNUNLOCKABLE_ACHIEVEMENT_NAME";
    //    public override String AchievementDescToken { get; } = "ENFORCER_SHOTGUNUNLOCKABLE_ACHIEVEMENT_DESC";
    //    public override String UnlockableNameToken { get; } = "ENFORCER_SHOTGUNUNLOCKABLE_UNLOCKABLE_NAME";
    //    protected override CustomSpriteProvider SpriteProvider { get; } = new CustomSpriteProvider("@Enforcer:Assets/Enforcer/EnforcerAssets/Icons/texSuperShotgunAchievement.png");

    //    public override int LookUpRequiredBodyIndex()
    //    {
    //        return BodyCatalog.FindBodyIndex("EnforcerBody");
    //    }

    //    private void Schmoovin(bool isDancing)
    //    {
    //        if (base.meetsBodyRequirement && isDancing) base.Grant();
    //    }

    //    public override void OnInstall()
    //    {
    //        base.OnInstall();

    //        EntityStates.Enforcer.EnforcerMain.onDance += Schmoovin;
    //    }

    //    public override void OnUninstall()
    //    {
    //        base.OnUninstall();

    //        EntityStates.Enforcer.EnforcerMain.onDance -= Schmoovin;
    //    }
    //}

    //public class NemesisSkinAchievement : ModdedUnlockableAndAchievement<CustomSpriteProvider>
    //{
    //    public override String AchievementIdentifier { get; } = "ENFORCER_NEMESISSKINUNLOCKABLE_ACHIEVEMENT_ID";
    //    public override String UnlockableIdentifier { get; } = "ENFORCER_NEMESISSKINUNLOCKABLE_REWARD_ID";
    //    public override String PrerequisiteUnlockableIdentifier { get; } = "ENFORCER_NEMESISSKINUNLOCKABLE_PREREQ_ID";
    //    public override String AchievementNameToken { get; } = "ENFORCER_NEMESISSKINUNLOCKABLE_ACHIEVEMENT_NAME";
    //    public override String AchievementDescToken { get; } = "ENFORCER_NEMESISSKINUNLOCKABLE_ACHIEVEMENT_DESC";
    //    public override String UnlockableNameToken { get; } = "ENFORCER_NEMESISSKINUNLOCKABLE_UNLOCKABLE_NAME";
    //    protected override CustomSpriteProvider SpriteProvider { get; } = new CustomSpriteProvider("@Enforcer:Assets/Enforcer/EnforcerAssets/Icons/texNemforcerAchievement.png");

    //    public override int LookUpRequiredBodyIndex()
    //    {
    //        return BodyCatalog.FindBodyIndex("EnforcerBody");
    //    }

    //    private void Check()
    //    {
    //        if (base.meetsBodyRequirement) base.Grant();
    //    }

    //    public override void OnInstall()
    //    {
    //        base.OnInstall();

    //        ArenaMissionController.onBeatArena += Check;
    //    }

    //    public override void OnUninstall()
    //    {
    //        base.OnUninstall();

    //        ArenaMissionController.onBeatArena -= Check;
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