using Modules;
using RoR2;
using UnityEngine;

namespace EnforcerPlugin.Achievements {

    public class DesperadoAchievement : GenericModdedUnlockable
    {
        public override string AchievementTokenPrefix => "ENFORCER_DESPERADO" + knee.grow;
        public override string PrerequisiteUnlockableIdentifier => "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_ID";

        public override string AchievementSpriteName => "texDesperadoAchievement";

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

        public class DesperadoAchievementServer : RoR2.Achievements.BaseServerAchievement {

            public override void OnInstall() {
                base.OnInstall();

                GlobalEventManager.onCharacterDeathGlobal += onCharacterDeathGlobal;
            }

            public override void OnUninstall() {
                base.OnUninstall();

                GlobalEventManager.onCharacterDeathGlobal -= onCharacterDeathGlobal;
            }

            private void onCharacterDeathGlobal(DamageReport report) {

                if (!report.victimBody) return;
                if (report.damageInfo == null) return;
                if (!report.damageInfo.inflictor) return;

                GameObject inflictor = report.damageInfo.inflictor;
                if (!inflictor || !inflictor.GetComponent<MapZone>()) {
                    return;
                }

                if (report.victimBodyIndex == BodyCatalog.FindBodyIndex("TitanGoldBody") && report.victimBody.teamComponent.teamIndex != TeamIndex.Player) {

                   base.Grant();
                   base.GetCurrentBody().GetComponent<EnforcerNetworkComponent>().Uhh(Skins.getEnforcerSkinIndex(Skins.EnforcerSkin.RECOLORDESPERADO));
                }
            }
        }
    }
}