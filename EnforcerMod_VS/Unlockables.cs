using System;
using UnityEngine;
using R2API;
using RoR2;
using R2API.Utils;
using RoR2.Stats;
using IL.RoR2.Achievements;
using EnforcerPlugin.Achievements;

namespace EnforcerPlugin
{
    public static class Unlockables
    {
        public static void RegisterUnlockables()
        {
            LanguageAPI.Add("ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_NAME", "Riot");
            LanguageAPI.Add("ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_DESC", "Kill a Magma Worm, a Wandering Vagrant and a Stone Titan in a single run.");
            LanguageAPI.Add("ENFORCER_CHARACTERUNLOCKABLE_UNLOCKABLE_NAME", "Riot");

            LanguageAPI.Add("ENFORCER_SHOTGUNUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: Schmoovin'");
            LanguageAPI.Add("ENFORCER_SHOTGUNUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, show off your dance moves.");
            LanguageAPI.Add("ENFORCER_SHOTGUNUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: Schmoovin'");

            LanguageAPI.Add("ENFORCER_RIFLEUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: Rapidfire");
            LanguageAPI.Add("ENFORCER_RIFLEUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, reach +300% attack speed.");
            LanguageAPI.Add("ENFORCER_RIFLEUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: Rapidfire");

            LanguageAPI.Add("ENFORCER_STUNGRENADEUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: Crowd Control");
            LanguageAPI.Add("ENFORCER_STUNGRENADEUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, have 20 enemies under the effects of Tear Gas at once.");
            LanguageAPI.Add("ENFORCER_STUNGRENADEUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: Crowd Control");

            LanguageAPI.Add("ENFORCER_MONSOONUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: Mastery");
            LanguageAPI.Add("ENFORCER_MONSOONUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, beat the game or obliterate on Monsoon.");
            LanguageAPI.Add("ENFORCER_MONSOONUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: Mastery");

            LanguageAPI.Add("ENFORCER_DOOMUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: Rip and Tear");
            LanguageAPI.Add("ENFORCER_DOOMUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, kill 50 imps in a single stage.");
            LanguageAPI.Add("ENFORCER_DOOMUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: Rip and Tear");

            LanguageAPI.Add("ENFORCER_BUNGUSUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: Enforcing Perfection");
            LanguageAPI.Add("ENFORCER_BUNGUSUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, become one with the Bungus.");
            LanguageAPI.Add("ENFORCER_BUNGUSUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: Enforcing Perfection");

            LanguageAPI.Add("ENFORCER_STORMTROOPERUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: Long Live the Empire");
            LanguageAPI.Add("ENFORCER_STORMTROOPERUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, defeat an elite Solus Control Unit.");
            LanguageAPI.Add("ENFORCER_STORMTROOPERUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: Long Live the Empire");

            LanguageAPI.Add("ENFORCER_DESPERADOUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: Rules of Nature");
            LanguageAPI.Add("ENFORCER_DESPERADOUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, Defeat the unique guardian of Gilded Coast by pushing it off the edge of the map.");
            LanguageAPI.Add("ENFORCER_DESPERADOUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: Rules of Nature");

            LanguageAPI.Add("ENFORCER_FROGUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: Through Thick and Thin");
            LanguageAPI.Add("ENFORCER_FROGUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, make a friend on the moon.");
            LanguageAPI.Add("ENFORCER_FROGUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: Through Thick and Thin");

            ///this is the version that works with the altered AddUnlockable I changed in R2API.
            ///look at #r2api in the discord to see what I mean. I went into more detail in #development as well
            ///if the pull requests gets accepted I'll add the other needed ones to this
            //UnlockablesAPI.AddUnlockable<Achievements.EnforcerUnlockAchievement>(true, typeof(EnforcerUnlockAchievement.EnforcerUnlockAchievementServer));

            UnlockablesAPI.AddUnlockable<Achievements.UnlockAchievement>(true);
            UnlockablesAPI.AddUnlockable<Achievements.SuperShotgunAchievement>(true);
            UnlockablesAPI.AddUnlockable<Achievements.AssaultRifleAchievement>(true);
            UnlockablesAPI.AddUnlockable<Achievements.StunGrenadeAchievement>(true);
            UnlockablesAPI.AddUnlockable<Achievements.MasteryAchievement>(true);
            UnlockablesAPI.AddUnlockable<Achievements.DoomAchievement>(true);
            UnlockablesAPI.AddUnlockable<Achievements.BungusAchievement>(true);
            UnlockablesAPI.AddUnlockable<Achievements.StormtrooperAchievement>(true);
            UnlockablesAPI.AddUnlockable<Achievements.DesperadoAchievement>(true);
            UnlockablesAPI.AddUnlockable<Achievements.FrogAchievement>(true);
        }
    }
}

namespace EnforcerPlugin.Achievements
{
    [R2APISubmoduleDependency(nameof(UnlockablesAPI))]

    public class UnlockAchievement : ModdedUnlockableAndAchievement<VanillaSpriteProvider> {
        public override String AchievementIdentifier { get; } = "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_ID";
        public override String UnlockableIdentifier { get; } = "ENFORCER_CHARACTERUNLOCKABLE_REWARD_ID";
        public override String PrerequisiteUnlockableIdentifier { get; } = "ENFORCER_CHARACTERUNLOCKABLE_PREREQ_ID";
        public override String AchievementNameToken { get; } = "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_NAME";
        public override String AchievementDescToken { get; } = "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_DESC";
        public override String UnlockableNameToken { get; } = "ENFORCER_CHARACTERUNLOCKABLE_UNLOCKABLE_NAME";
        protected override VanillaSpriteProvider SpriteProvider { get; } = new VanillaSpriteProvider("");

        public bool magmaWormKilled;
        public bool wanderingVagrantKilled;
        public bool stoneTitanKilled;
        //need to network this, only gives it to the host rn

        private void CheckDeath(DamageReport report) {
            if (report is null) return;
            if (report.victimBody is null) return;
            if (report.attackerBody is null) return;

            if (report.victimTeamIndex != TeamIndex.Player) {
                if (report.victimBodyIndex == BodyCatalog.FindBodyIndex("MagmaWormBody")) this.magmaWormKilled = true;
                if (report.victimBodyIndex == BodyCatalog.FindBodyIndex("VagrantBody")) this.wanderingVagrantKilled = true;
                if (report.victimBodyIndex == BodyCatalog.FindBodyIndex("TitanBody")) this.stoneTitanKilled = true;

                if (this.magmaWormKilled && this.wanderingVagrantKilled && this.stoneTitanKilled) {
                    base.Grant();
                }
            }
        }

        private void ResetOnRunStart(Run run) {
            this.ResetKills();

            //throwing this in here because lazy
            EnforcerPlugin.cum = false;
        }

        private void ResetKills() {
            this.magmaWormKilled = false;
            this.wanderingVagrantKilled = false;
            this.stoneTitanKilled = false;
        }

        public override void OnInstall() {
            base.OnInstall();

            this.ResetKills();
            GlobalEventManager.onCharacterDeathGlobal += this.CheckDeath;
            Run.onRunStartGlobal += ResetOnRunStart;
        }

        public override void OnUninstall() {
            base.OnUninstall();

            GlobalEventManager.onCharacterDeathGlobal -= this.CheckDeath;
            Run.onRunStartGlobal -= ResetOnRunStart;
        }
    }
    //networked
    public class EnforcerUnlockAchievement : ModdedUnlockableAndAchievement<VanillaSpriteProvider>
    {
        public override String AchievementIdentifier { get; } = "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_ID";
        public override String UnlockableIdentifier { get; } = "ENFORCER_CHARACTERUNLOCKABLE_REWARD_ID";
        public override String PrerequisiteUnlockableIdentifier { get; } = "ENFORCER_CHARACTERUNLOCKABLE_PREREQ_ID";
        public override String AchievementNameToken { get; } = "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_NAME";
        public override String AchievementDescToken { get; } = "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_DESC";
        public override String UnlockableNameToken { get; } = "ENFORCER_CHARACTERUNLOCKABLE_UNLOCKABLE_NAME";
        protected override VanillaSpriteProvider SpriteProvider { get; } = new VanillaSpriteProvider("");
        //need to network this, only gives it to the host rn
        public override void OnInstall() {
            base.OnInstall();
            base.SetServerTracked(true);
        }

        // Token: 0x0600310D RID: 12557 RVA: 0x000CD6EC File Offset: 0x000CB8EC
        public override void OnUninstall() {
            base.OnUninstall();
        }



        public class EnforcerUnlockAchievementServer : RoR2.Achievements.BaseServerAchievement {

            public bool magmaWormKilled;
            public bool wanderingVagrantKilled;
            public bool stoneTitanKilled;

            private void CheckDeath(DamageReport report) {
                if (report is null) return;
                if (report.victimBody is null) return;
                if (report.attackerBody is null) return;

                if (report.victimTeamIndex != TeamIndex.Player) {
                    if (report.victimBodyIndex == BodyCatalog.FindBodyIndex("MagmaWormBody")) {
                        this.magmaWormKilled = true;

                        Debug.LogWarning("killed worm");
                        Debug.LogWarning($"worm: {magmaWormKilled}, vag: {wanderingVagrantKilled}, tit: {stoneTitanKilled}");
                    }
                    if (report.victimBodyIndex == BodyCatalog.FindBodyIndex("VagrantBody")) {
                        this.wanderingVagrantKilled = true;

                        Debug.LogWarning("killed vag");
                        Debug.LogWarning($"worm: {magmaWormKilled}, vag: {wanderingVagrantKilled}, tit: {stoneTitanKilled}");
                    }
                    if (report.victimBodyIndex == BodyCatalog.FindBodyIndex("TitanBody")) {
                        this.stoneTitanKilled = true;

                        Debug.LogWarning("killed tit");
                        Debug.LogWarning($"worm: {magmaWormKilled}, vag: {wanderingVagrantKilled}, tit: {stoneTitanKilled}");
                    }

                    if (this.magmaWormKilled && this.wanderingVagrantKilled && this.stoneTitanKilled) {
                        Debug.LogWarning($"ya fuckin");
                        base.Grant();
                        Debug.LogWarning($"did it");
                    }
                }
            }

            private void ResetOnRunStart(Run run) {
                this.ResetKills();

                //throwing this in here because lazy
                EnforcerPlugin.cum = false;
            }

            private void ResetKills() {
                this.magmaWormKilled = false;
                this.wanderingVagrantKilled = false;
                this.stoneTitanKilled = false;
            }

            public override void OnInstall() {
                base.OnInstall();

                this.ResetKills();
                GlobalEventManager.onCharacterDeathGlobal += this.CheckDeath;
                Run.onRunStartGlobal += ResetOnRunStart;
            }

            public override void OnUninstall() {
                base.OnUninstall();

                GlobalEventManager.onCharacterDeathGlobal -= this.CheckDeath;
                Run.onRunStartGlobal -= ResetOnRunStart;
            }

        }
    }

    public class MasteryAchievement : ModdedUnlockableAndAchievement<VanillaSpriteProvider>
    {
        public override String AchievementIdentifier { get; } = "ENFORCER_MONSOONUNLOCKABLE_ACHIEVEMENT_ID";
        public override String UnlockableIdentifier { get; } = "ENFORCER_MONSOONUNLOCKABLE_REWARD_ID";
        public override String PrerequisiteUnlockableIdentifier { get; } = "ENFORCER_MONSOONUNLOCKABLE_PREREQ_ID";
        public override String AchievementNameToken { get; } = "ENFORCER_MONSOONUNLOCKABLE_ACHIEVEMENT_NAME";
        public override String AchievementDescToken { get; } = "ENFORCER_MONSOONUNLOCKABLE_ACHIEVEMENT_DESC";
        public override String UnlockableNameToken { get; } = "ENFORCER_MONSOONUNLOCKABLE_UNLOCKABLE_NAME";
        protected override VanillaSpriteProvider SpriteProvider { get; } = new VanillaSpriteProvider("");

        public override int LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("EnforcerBody");
        }

        public void ClearCheck(Run run, RunReport runReport)
        {
            if (run is null) return;
            if (runReport is null) return;

            if (!runReport.gameEnding) return;

            if (runReport.gameEnding.isWin)
            {
                DifficultyDef difficultyDef = DifficultyCatalog.GetDifficultyDef(runReport.ruleBook.FindDifficulty());

                if (difficultyDef != null && difficultyDef.countsAsHardMode)
                {
                    if (base.meetsBodyRequirement)
                    {
                        base.Grant();
                    }
                }
            }
        }

        public override void OnInstall()
        {
            base.OnInstall();

            Run.onClientGameOverGlobal += this.ClearCheck;
        }

        public override void OnUninstall()
        {
            base.OnUninstall();

            Run.onClientGameOverGlobal -= this.ClearCheck;
        }
    }

    public class BungusAchievement : ModdedUnlockableAndAchievement<VanillaSpriteProvider>
    {
        public override String AchievementIdentifier { get; } = "ENFORCER_BUNGUSUNLOCKABLE_ACHIEVEMENT_ID";
        public override String UnlockableIdentifier { get; } = "ENFORCER_BUNGUSUNLOCKABLE_REWARD_ID";
        public override String PrerequisiteUnlockableIdentifier { get; } = "ENFORCER_BUNGUSUNLOCKABLE_PREREQ_ID";
        public override String AchievementNameToken { get; } = "ENFORCER_BUNGUSUNLOCKABLE_ACHIEVEMENT_NAME";
        public override String AchievementDescToken { get; } = "ENFORCER_BUNGUSUNLOCKABLE_ACHIEVEMENT_DESC";
        public override String UnlockableNameToken { get; } = "ENFORCER_BUNGUSUNLOCKABLE_UNLOCKABLE_NAME";
        protected override VanillaSpriteProvider SpriteProvider { get; } = new VanillaSpriteProvider("");

        public static float bungusTime = 240f;

        public override int LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("EnforcerBody");
        }

        public void BungusCheck(float bungus)
        {
            if (base.meetsBodyRequirement && bungus >= BungusAchievement.bungusTime) base.Grant();
        }

        public override void OnInstall()
        {
            base.OnInstall();

            EntityStates.Enforcer.EnforcerMain.Bungus += BungusCheck;
        }

        public override void OnUninstall()
        {
            base.OnUninstall();

            EntityStates.Enforcer.EnforcerMain.Bungus -= BungusCheck;
        }
    }

    public class DesperadoAchievement : ModdedUnlockableAndAchievement<VanillaSpriteProvider>
    {
        public override String AchievementIdentifier { get; } = "ENFORCER_DESPERADOUNLOCKABLE_ACHIEVEMENT_ID";
        public override String UnlockableIdentifier { get; } = "ENFORCER_DESPERADOUNLOCKABLE_REWARD_ID";
        public override String PrerequisiteUnlockableIdentifier { get; } = "ENFORCER_DESPERADOUNLOCKABLE_PREREQ_ID";
        public override String AchievementNameToken { get; } = "ENFORCER_DESPERADOUNLOCKABLE_ACHIEVEMENT_NAME";
        public override String AchievementDescToken { get; } = "ENFORCER_DESPERADOUNLOCKABLE_ACHIEVEMENT_DESC";
        public override String UnlockableNameToken { get; } = "ENFORCER_DESPERADOUNLOCKABLE_UNLOCKABLE_NAME";
        protected override VanillaSpriteProvider SpriteProvider { get; } = new VanillaSpriteProvider("");

        public override int LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("EnforcerBody");
        }

        private void CheckDeath(DamageReport report)
        {
            if (!report.victimBody) return;
            if (report.damageInfo is null) return;
            if (report.damageInfo.inflictor is null) return;

            GameObject inflictor = report.damageInfo.inflictor;
            if (!inflictor || !inflictor.GetComponent<MapZone>())
            {
                return;
            }

            if (report.victimBodyIndex == BodyCatalog.FindBodyIndex("TitanGoldBody") && report.victimBody.teamComponent.teamIndex != TeamIndex.Player)
            {
                if (base.meetsBodyRequirement) base.Grant();
            }
        }

        public override void OnInstall()
        {
            base.OnInstall();

            GlobalEventManager.onCharacterDeathGlobal += CheckDeath;
        }

        public override void OnUninstall()
        {
            base.OnUninstall();

            GlobalEventManager.onCharacterDeathGlobal -= CheckDeath;
        }
    }

    public class StormtrooperAchievement : ModdedUnlockableAndAchievement<VanillaSpriteProvider>
    {
        public override String AchievementIdentifier { get; } = "ENFORCER_STORMTROOPERUNLOCKABLE_ACHIEVEMENT_ID";
        public override String UnlockableIdentifier { get; } = "ENFORCER_STORMTROOPERUNLOCKABLE_REWARD_ID";
        public override String PrerequisiteUnlockableIdentifier { get; } = "ENFORCER_STORMTROOPERUNLOCKABLE_PREREQ_ID";
        public override String AchievementNameToken { get; } = "ENFORCER_STORMTROOPERUNLOCKABLE_ACHIEVEMENT_NAME";
        public override String AchievementDescToken { get; } = "ENFORCER_STORMTROOPERUNLOCKABLE_ACHIEVEMENT_DESC";
        public override String UnlockableNameToken { get; } = "ENFORCER_STORMTROOPERUNLOCKABLE_UNLOCKABLE_NAME";
        protected override VanillaSpriteProvider SpriteProvider { get; } = new VanillaSpriteProvider("");

        public override int LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("EnforcerBody");
        }

        private void CheckDeath(DamageReport report)
        {
            if (!report.victimBody) return;
            if (report.damageInfo is null) return;
            if (report.damageInfo.inflictor is null) return;

            if (report.victimBodyIndex == BodyCatalog.FindBodyIndex("RoboBallBossBody") && report.victimBody.isElite)
            {
                if (base.meetsBodyRequirement) base.Grant();
            }
        }

        public override void OnInstall()
        {
            base.OnInstall();

            GlobalEventManager.onCharacterDeathGlobal += CheckDeath;
        }

        public override void OnUninstall()
        {
            base.OnUninstall();

            GlobalEventManager.onCharacterDeathGlobal -= CheckDeath;
        }
    }

    public class DoomAchievement : ModdedUnlockableAndAchievement<VanillaSpriteProvider>
    {
        public override String AchievementIdentifier { get; } = "ENFORCER_DOOMUNLOCKABLE_ACHIEVEMENT_ID";
        public override String UnlockableIdentifier { get; } = "ENFORCER_DOOMUNLOCKABLE_REWARD_ID";
        public override String PrerequisiteUnlockableIdentifier { get; } = "ENFORCER_DOOMUNLOCKABLE_PREREQ_ID";
        public override String AchievementNameToken { get; } = "ENFORCER_DOOMUNLOCKABLE_ACHIEVEMENT_NAME";
        public override String AchievementDescToken { get; } = "ENFORCER_DOOMUNLOCKABLE_ACHIEVEMENT_DESC";
        public override String UnlockableNameToken { get; } = "ENFORCER_DOOMUNLOCKABLE_UNLOCKABLE_NAME";
        protected override VanillaSpriteProvider SpriteProvider { get; } = new VanillaSpriteProvider("");

        public static int impCount = 50;

        public override int LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("EnforcerBody");
        }

        public void ImpCheck(int imps)
        {
            if (base.meetsBodyRequirement && imps >= DoomAchievement.impCount) base.Grant();
        }

        public override void OnInstall()
        {
            base.OnInstall();

            EnforcerWeaponComponent.Imp += ImpCheck;
        }

        public override void OnUninstall()
        {
            base.OnUninstall();

            EnforcerWeaponComponent.Imp -= ImpCheck;
        }
    }

    public class FrogAchievement : ModdedUnlockableAndAchievement<VanillaSpriteProvider>
    {
        public override String AchievementIdentifier { get; } = "ENFORCER_FROGUNLOCKABLE_ACHIEVEMENT_ID";
        public override String UnlockableIdentifier { get; } = "ENFORCER_FROGUNLOCKABLE_REWARD_ID";
        public override String PrerequisiteUnlockableIdentifier { get; } = "ENFORCER_FROGUNLOCKABLE_PREREQ_ID";
        public override String AchievementNameToken { get; } = "ENFORCER_FROGUNLOCKABLE_ACHIEVEMENT_NAME";
        public override String AchievementDescToken { get; } = "ENFORCER_FROGUNLOCKABLE_ACHIEVEMENT_DESC";
        public override String UnlockableNameToken { get; } = "ENFORCER_FROGUNLOCKABLE_UNLOCKABLE_NAME";
        protected override VanillaSpriteProvider SpriteProvider { get; } = new VanillaSpriteProvider("");

        public override int LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("EnforcerBody");
        }

        private void Froge(bool cum)
        {
            if (cum && base.meetsBodyRequirement) base.Grant();
        }

        public override void OnInstall()
        {
            base.OnInstall();

            EnforcerFrogComponent.FrogGet += Froge;
        }

        public override void OnUninstall()
        {
            base.OnUninstall();

            EnforcerFrogComponent.FrogGet -= Froge;
        }
    }

    public class SuperShotgunAchievement : ModdedUnlockableAndAchievement<VanillaSpriteProvider>
    {
        public override String AchievementIdentifier { get; } = "ENFORCER_SHOTGUNUNLOCKABLE_ACHIEVEMENT_ID";
        public override String UnlockableIdentifier { get; } = "ENFORCER_SHOTGUNUNLOCKABLE_REWARD_ID";
        public override String PrerequisiteUnlockableIdentifier { get; } = "ENFORCER_SHOTGUNUNLOCKABLE_PREREQ_ID";
        public override String AchievementNameToken { get; } = "ENFORCER_SHOTGUNUNLOCKABLE_ACHIEVEMENT_NAME";
        public override String AchievementDescToken { get; } = "ENFORCER_SHOTGUNUNLOCKABLE_ACHIEVEMENT_DESC";
        public override String UnlockableNameToken { get; } = "ENFORCER_SHOTGUNUNLOCKABLE_UNLOCKABLE_NAME";
        protected override VanillaSpriteProvider SpriteProvider { get; } = new VanillaSpriteProvider("");

        public override int LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("EnforcerBody");
        }

        private void Schmoovin(bool isDancing)
        {
            if (base.meetsBodyRequirement && isDancing) base.Grant();
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

    public class AssaultRifleAchievement : ModdedUnlockableAndAchievement<VanillaSpriteProvider>
    {
        public override String AchievementIdentifier { get; } = "ENFORCER_RIFLEUNLOCKABLE_ACHIEVEMENT_ID";
        public override String UnlockableIdentifier { get; } = "ENFORCER_RIFLEUNLOCKABLE_REWARD_ID";
        public override String PrerequisiteUnlockableIdentifier { get; } = "ENFORCER_RIFLEUNLOCKABLE_PREREQ_ID";
        public override String AchievementNameToken { get; } = "ENFORCER_RIFLEUNLOCKABLE_ACHIEVEMENT_NAME";
        public override String AchievementDescToken { get; } = "ENFORCER_RIFLEUNLOCKABLE_ACHIEVEMENT_DESC";
        public override String UnlockableNameToken { get; } = "ENFORCER_RIFLEUNLOCKABLE_UNLOCKABLE_NAME";
        protected override VanillaSpriteProvider SpriteProvider { get; } = new VanillaSpriteProvider("");

        public override int LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("EnforcerBody");
        }

        public void CheckAttackSpeed()
        {
            if (base.localUser != null && base.localUser.cachedBody && base.localUser.cachedBody.attackSpeed >= 4f && base.meetsBodyRequirement)
            {
                base.Grant();
            }
        }

        public override void OnInstall()
        {
            base.OnInstall();

            RoR2Application.onUpdate += this.CheckAttackSpeed;
        }

        public override void OnUninstall()
        {
            base.OnUninstall();

            RoR2Application.onUpdate -= this.CheckAttackSpeed;
        }
    }

    public class StunGrenadeAchievement : ModdedUnlockableAndAchievement<VanillaSpriteProvider>
    {
        public override String AchievementIdentifier { get; } = "ENFORCER_STUNGRENADEUNLOCKABLE_ACHIEVEMENT_ID";
        public override String UnlockableIdentifier { get; } = "ENFORCER_STUNGRENADEUNLOCKABLE_REWARD_ID";
        public override String PrerequisiteUnlockableIdentifier { get; } = "ENFORCER_STUNGRENADEUNLOCKABLE_PREREQ_ID";
        public override String AchievementNameToken { get; } = "ENFORCER_STUNGRENADEUNLOCKABLE_ACHIEVEMENT_NAME";
        public override String AchievementDescToken { get; } = "ENFORCER_STUNGRENADEUNLOCKABLE_ACHIEVEMENT_DESC";
        public override String UnlockableNameToken { get; } = "ENFORCER_STUNGRENADEUNLOCKABLE_UNLOCKABLE_NAME";
        protected override VanillaSpriteProvider SpriteProvider { get; } = new VanillaSpriteProvider("");

        public override int LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("EnforcerBody");
        }

        public void Check(int count)
        {
            if (count >= 20 && base.meetsBodyRequirement) base.Grant();
        }

        public override void OnInstall()
        {
            base.OnInstall();

            TearGasComponent.GasCheck += Check;
        }

        public override void OnUninstall()
        {
            base.OnUninstall();

            TearGasComponent.GasCheck -= Check;
        }
    }
}