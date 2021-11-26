using System;
using UnityEngine;
using RoR2;
using ModdedUnlockable = Enforcer.Modules.ModdedUnlockable;

namespace EnforcerPlugin.Achievements {

    public class DoomAchievement : ModdedUnlockable {
        public override String AchievementIdentifier { get; } = "ENFORCER_DOOMUNLOCKABLE_ACHIEVEMENT_ID";
        public override String UnlockableIdentifier { get; } = "ENFORCER_DOOMUNLOCKABLE_REWARD_ID";
        public override String PrerequisiteUnlockableIdentifier { get; } = "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_ID";
        public override String AchievementNameToken { get; } = "ENFORCER_DOOMUNLOCKABLE_ACHIEVEMENT_NAME";
        public override String AchievementDescToken { get; } = "ENFORCER_DOOMUNLOCKABLE_ACHIEVEMENT_DESC";
        public override String UnlockableNameToken { get; } = "ENFORCER_DOOMUNLOCKABLE_UNLOCKABLE_NAME";

        public override Sprite Sprite { get; } = Assets.MainAssetBundle.LoadAsset<Sprite>("texSuperShotgunAchievement");

        public override Func<string> GetHowToUnlock { get; } = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
                            {
                                Language.GetString("ENFORCER_DOOMUNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString("ENFORCER_DOOMUNLOCKABLE_ACHIEVEMENT_DESC")
                            }));
        public override Func<string> GetUnlocked { get; } = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
                            {
                                Language.GetString("ENFORCER_DOOMUNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString("ENFORCER_DOOMUNLOCKABLE_ACHIEVEMENT_DESC")
                            }));

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

            private static readonly int impRequirement = 40;
            private int impProgress = 0;

            private BodyIndex impBodyIndex;

            public override void OnInstall() {
                base.OnInstall();

                impBodyIndex = BodyCatalog.FindBodyIndex("ImpBody");
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
                    this.impProgress++;
                    //Debug.Log("doom imps" + this.impProgress);
                    if (this.impProgress > DoomAchievementServer.impRequirement) {
                        base.Grant();
                    }
                }
            }
        }
    }
}