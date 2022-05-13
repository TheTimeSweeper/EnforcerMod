using RoR2;
using R2API;

namespace EnforcerPlugin.Achievements
{

    //networked when R2API updates
        //fuck you for never updating
    public class EnforcerUnlockAchievement : GenericModdedUnlockable {

        public override string AchievementTokenPrefix => "ENFORCER_CHARACTER";
        public override string PrerequisiteUnlockableIdentifier => "";

        public override string AchievementSpriteName => "texEnforcerUnlockAchievement";
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
                        //I'm not renaming it
                            //wait if it's here doesn't it mean it won't work?
            }

            private void ResetKills() {
                this.magmaWormKilled = false;
                this.wanderingVagrantKilled = false;
                this.stoneTitanKilled = false;
            }
        }
    }
}