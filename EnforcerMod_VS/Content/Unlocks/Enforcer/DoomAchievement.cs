using RoR2;
using RoR2.Achievements;

namespace EnforcerPlugin.Achievements
{
    [RegisterAchievement(identifier, unlockableIdentifier, "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_ID", 3, typeof(DoomAchievementServer))]
    public class DoomAchievement : BaseAchievement
    {
        public const string identifier = "ENFORCER_DOOMUNLOCKABLE_ACHIEVEMENT_ID" + knee.grow;
        public const string unlockableIdentifier = "ENFORCER_DOOMUNLOCKABLE_REWARD_ID" + knee.grow;
        public const string AchievementSpriteName = "texSuperShotgunAchievement";

        public override BodyIndex LookUpRequiredBodyIndex() {
            return BodyCatalog.FindBodyIndex("EnforcerBody");
        }

        // Token: 0x06003926 RID: 14630 RVA: 0x000DD289 File Offset: 0x000DB489
        public override void OnBodyRequirementMet() {
            base.OnBodyRequirementMet();

            base.SetServerTracked(true);
        }

        // Token: 0x06003927 RID: 14631 RVA: 0x000DD298 File Offset: 0x000DB498
        public override void OnBodyRequirementBroken() {
            base.SetServerTracked(false);

            base.OnBodyRequirementBroken();
        }

        public class DoomAchievementServer : RoR2.Achievements.BaseServerAchievement {

            protected virtual int impRequirement => 20;
            protected virtual BodyIndex impBodyIndex => BodyCatalog.FindBodyIndex("ImpBody");

            private int _impProgress = 0;


            public override void OnInstall() {
                base.OnInstall();

                GlobalEventManager.onCharacterDeathGlobal += onCharacterDeathGlobal;
                //idk if this resets every stage.
                    //ok it does, but Idk how
            }

            public override void OnUninstall() {
                base.OnUninstall();

                GlobalEventManager.onCharacterDeathGlobal -= onCharacterDeathGlobal;
            }

            private void onCharacterDeathGlobal(DamageReport damageReport) {

                bool imp = damageReport.victimBody && damageReport.victimBodyIndex == impBodyIndex;
                //uncomment this if we want kills to be tracked respective to individual players
                    //which is rad that it fuckin works networked like that, but not fun to actually do
                //imp |= damageReport.attackerBody && damageReport.attackerBody == base.GetCurrentBody();
                if (imp) {
                    this._impProgress++;
                    //Debug.Log("doom imps" + this.impProgress);
                    if (this._impProgress >= impRequirement) {
                        base.Grant();
                        onGrant();
                    }
                }
            }

            protected virtual void onGrant() { }
        }
    }
}