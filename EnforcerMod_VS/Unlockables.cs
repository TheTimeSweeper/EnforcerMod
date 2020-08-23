using System;
using UnityEngine;
using R2API;
using RoR2;
using R2API.Utils;
using RoR2.Stats;

namespace EnforcerPlugin
{
    public static class Unlockables
    {
        public static void RegisterUnlockables()
        {
            LanguageAPI.Add("ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_NAME", "Riot");
            LanguageAPI.Add("ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_DESC", "Kill a Magma Worm, a Wandering Vagrant and a Stone Titan in a single run.");
            LanguageAPI.Add("ENFORCER_CHARACTERUNLOCKABLE_UNLOCKABLE_NAME", "Riot");

            LanguageAPI.Add("ENFORCER_SHOTGUNUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: Super Shotgun");
            LanguageAPI.Add("ENFORCER_SHOTGUNUNLOCKABLE_ACHIEVEMENT_DESC", "something");
            LanguageAPI.Add("ENFORCER_SHOTGUNUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: Super Shotgun");

            LanguageAPI.Add("ENFORCER_RIFLEUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: Assault Rifle");
            LanguageAPI.Add("ENFORCER_RIFLEUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, reach +300% attack speed.");
            LanguageAPI.Add("ENFORCER_RIFLEUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: Assault Rifle");

            LanguageAPI.Add("ENFORCER_STUNGRENADEUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: Stun Grenade");
            LanguageAPI.Add("ENFORCER_STUNGRENADEUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, have 20 enemies under the effects of Tear Gas at once.");
            LanguageAPI.Add("ENFORCER_STUNGRENADEUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: Stun Grenade");

            LanguageAPI.Add("ENFORCER_MONSOONUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: Mastery");
            LanguageAPI.Add("ENFORCER_MONSOONUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, beat the game or obliterate on Monsoon.");
            LanguageAPI.Add("ENFORCER_MONSOONUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: Mastery");

            LanguageAPI.Add("ENFORCER_DOOMUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: Rip and Tear");
            LanguageAPI.Add("ENFORCER_DOOMUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, kill 75 imps in a single run.");
            LanguageAPI.Add("ENFORCER_DOOMUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: Rip and Tear");

            LanguageAPI.Add("ENFORCER_BUNGUSUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: Enforcing Perfection");
            LanguageAPI.Add("ENFORCER_BUNGUSUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, stand still with Bustling Fungus for 4 minutes straight.");
            LanguageAPI.Add("ENFORCER_BUNGUSUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: Enforcing Perfection");

            LanguageAPI.Add("ENFORCER_STORMTROOPERUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: Long Live the Empire");
            LanguageAPI.Add("ENFORCER_STORMTROOPERUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, kill 50 enemies by knocking them off the edge of the map.");
            LanguageAPI.Add("ENFORCER_STORMTROOPERUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: Long Live the Empire");

            LanguageAPI.Add("ENFORCER_DESPERADOUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: Fucking Invincible");
            LanguageAPI.Add("ENFORCER_DESPERADOUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, deflect 100 projectiles with Shield Bash.");
            LanguageAPI.Add("ENFORCER_DESPERADOUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: Fucking Invincible");


            UnlockablesAPI.AddUnlockable<Achievements.UnlockAchievement>(true);
            UnlockablesAPI.AddUnlockable<Achievements.SuperShotgunAchievement>(true);
            UnlockablesAPI.AddUnlockable<Achievements.AssaultRifleAchievement>(true);
            UnlockablesAPI.AddUnlockable<Achievements.StunGrenadeAchievement>(true);
            UnlockablesAPI.AddUnlockable<Achievements.MasteryAchievement>(true);
            UnlockablesAPI.AddUnlockable<Achievements.DoomAchievement>(true);
            UnlockablesAPI.AddUnlockable<Achievements.BungusAchievement>(true);
            UnlockablesAPI.AddUnlockable<Achievements.StormtrooperAchievement>(true);
            UnlockablesAPI.AddUnlockable<Achievements.DesperadoAchievement>(true);
        }
    }
}

namespace EnforcerPlugin.Achievements
{
    [R2APISubmoduleDependency(nameof(UnlockablesAPI))]

    public class UnlockAchievement : ModdedUnlockableAndAchievement<VanillaSpriteProvider>
    {
        public override String AchievementIdentifier { get; } = "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_ID";
        public override String UnlockableIdentifier { get; } = "ENFORCER_CHARACTERUNLOCKABLE_REWARD_ID";
        public override String PrerequisiteUnlockableIdentifier { get; } = "ENFORCER_CHARACTERUNLOCKABLE_PREREQ_ID";
        public override String AchievementNameToken { get; } = "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_NAME";
        public override String AchievementDescToken { get; } = "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_DESC";
        public override String UnlockableNameToken { get; } = "ENFORCER_CHARACTERUNLOCKABLE_UNLOCKABLE_NAME";
        protected override VanillaSpriteProvider SpriteProvider { get; } = new VanillaSpriteProvider("");

        public static bool magmaWormKilled;
        public static bool wanderingVagrantKilled;
        public static bool stoneTitanKilled;
        //this implementation really isn't good but hey as long as it works right
        //really doubt it'll work at all in multiplayer

        private void CheckDeath(DamageReport report)
        {
            if (report is null) return;
            if (report.victimBody is null) return;
            if (report.attackerBody is null) return;

            if (report.victimTeamIndex != TeamIndex.Player)
            {
                if (report.victimBodyIndex == BodyCatalog.FindBodyIndex("MagmaWormBody")) UnlockAchievement.magmaWormKilled = true;
                if (report.victimBodyIndex == BodyCatalog.FindBodyIndex("VagrantBody")) UnlockAchievement.wanderingVagrantKilled = true;
                if (report.victimBodyIndex == BodyCatalog.FindBodyIndex("TitanBody")) UnlockAchievement.stoneTitanKilled = true;

                if (UnlockAchievement.magmaWormKilled && UnlockAchievement.wanderingVagrantKilled && UnlockAchievement.stoneTitanKilled)
                {
                    base.Grant();
                }
            }
        }

        private void ResetOnRunStart(Run run)
        {
            this.ResetKills();
        }

        private void ResetKills()
        {
            UnlockAchievement.magmaWormKilled = false;
            UnlockAchievement.wanderingVagrantKilled = false;
            UnlockAchievement.stoneTitanKilled = false;
        }

        public override void OnInstall()
        {
            base.OnInstall();

            this.ResetKills();
            GlobalEventManager.onCharacterDeathGlobal += this.CheckDeath;
            Run.onRunStartGlobal += ResetOnRunStart;
        }

        public override void OnUninstall()
        {
            base.OnUninstall();

            GlobalEventManager.onCharacterDeathGlobal -= this.CheckDeath;
            Run.onRunStartGlobal -= ResetOnRunStart;
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

        public override int LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("EnforcerBody");
        }

        public override void OnInstall()
        {
            base.OnInstall();
        }

        public override void OnUninstall()
        {
            base.OnUninstall();
        }
    }

    public class DesperadoAchievement : ModdedUnlockableAndAchievement<VanillaSpriteProvider>
    {
        public override String AchievementIdentifier { get; } = "ENFORCER_MASTERYUNLOCKABLE_ACHIEVEMENT_ID";
        public override String UnlockableIdentifier { get; } = "ENFORCER_MASTERYUNLOCKABLE_REWARD_ID";
        public override String PrerequisiteUnlockableIdentifier { get; } = "ENFORCER_MASTERYUNLOCKABLE_PREREQ_ID";
        public override String AchievementNameToken { get; } = "ENFORCER_MASTERYUNLOCKABLE_ACHIEVEMENT_NAME";
        public override String AchievementDescToken { get; } = "ENFORCER_MASTERYUNLOCKABLE_ACHIEVEMENT_DESC";
        public override String UnlockableNameToken { get; } = "ENFORCER_MASTERYUNLOCKABLE_UNLOCKABLE_NAME";
        protected override VanillaSpriteProvider SpriteProvider { get; } = new VanillaSpriteProvider("");

        public override int LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("EnforcerBody");
        }

        public override void OnInstall()
        {
            base.OnInstall();
        }

        public override void OnUninstall()
        {
            base.OnUninstall();
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

        public override void OnInstall()
        {
            base.OnInstall();
        }

        public override void OnUninstall()
        {
            base.OnUninstall();
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

        public override int LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("EnforcerBody");
        }

        public override void OnInstall()
        {
            base.OnInstall();
        }

        public override void OnUninstall()
        {
            base.OnUninstall();
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

        public override void OnInstall()
        {
            base.OnInstall();
        }

        public override void OnUninstall()
        {
            base.OnUninstall();
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