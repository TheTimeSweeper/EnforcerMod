using System;
using UnityEngine;
using RoR2;
using Enforcer.Modules;
using ModdedUnlockable = Enforcer.Modules.ModdedUnlockable;

namespace EnforcerPlugin.Achievements {

    //networked when R2API updates
    //fuck you for never updating
    public class EnforcerUnlockAchievement : ModdedUnlockable {

        public override String AchievementIdentifier { get; } = "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_ID";
        public override String UnlockableIdentifier { get; } = "ENFORCER_CHARACTERUNLOCKABLE_REWARD_ID";
        public override String AchievementNameToken { get; } = "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_NAME";
        public override String PrerequisiteUnlockableIdentifier { get; } = "";
        public override String UnlockableNameToken { get; } = "ENFORCER_CHARACTERUNLOCKABLE_UNLOCKABLE_NAME";
        public override String AchievementDescToken { get; } = "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_DESC";
        public override Sprite Sprite { get; } = Assets.MainAssetBundle.LoadAsset<Sprite>("texEnforcerUnlockAchievement");

        public override Func<string> GetHowToUnlock { get; } = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
                            {
                                Language.GetString("ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString("ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_DESC")
                            }));
        public override Func<string> GetUnlocked { get; } = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
                            {
                                Language.GetString("ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString("ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_DESC")
                            }));
        //need to network this, only gives it to the host rn
            //after a fucking year it finally happened

        public override void OnInstall() {
            base.OnInstall();
            base.SetServerTracked(true);
        }

        public override void OnUninstall() {
            base.OnUninstall();
        }

        public class EnforcerUnlockAchievementServer : RoR2.Achievements.BaseServerAchievement {

            public bool magmaWormKilled;
            public bool wanderingVagrantKilled;
            public bool stoneTitanKilled;

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

            private void CheckDeath(DamageReport report) {
                if (report is null) return;
                if (report.victimBody is null) return;
                if (report.attackerBody is null) return;

                if (report.victimTeamIndex != TeamIndex.Player) {
                    if (report.victimBodyIndex == BodyCatalog.FindBodyIndex("MagmaWormBody")) {
                        this.magmaWormKilled = true;

                        //Debug.LogWarning("killed worm");
                        //Debug.LogWarning($"wom: {magmaWormKilled}, vag: {wanderingVagrantKilled}, tit: {stoneTitanKilled}");
                    }
                    if (report.victimBodyIndex == BodyCatalog.FindBodyIndex("VagrantBody")) {
                        this.wanderingVagrantKilled = true;

                        //Debug.LogWarning("killed vag");
                        //Debug.LogWarning($"wom: {magmaWormKilled}, vag: {wanderingVagrantKilled}, tit: {stoneTitanKilled}");
                    }
                    if (report.victimBodyIndex == BodyCatalog.FindBodyIndex("TitanBody")) {
                        this.stoneTitanKilled = true;

                        //Debug.LogWarning("killed tit");
                        //Debug.LogWarning($"wom: {magmaWormKilled}, vag: {wanderingVagrantKilled}, tit: {stoneTitanKilled}");
                    }

                    if (this.magmaWormKilled && this.wanderingVagrantKilled && this.stoneTitanKilled) {
                        //Debug.LogWarning($"ya fuckin");
                        base.Grant();
                        //Debug.LogWarning($"did it");
                    }
                }
            }

            private void ResetOnRunStart(Run run) {
                this.ResetKills();

                //throwing this in here because lazy
                EnforcerModPlugin.cum = false;
                //it's for doom music this fucking retard
            }

            private void ResetKills() {
                this.magmaWormKilled = false;
                this.wanderingVagrantKilled = false;
                this.stoneTitanKilled = false;
            }
        }
    }
}