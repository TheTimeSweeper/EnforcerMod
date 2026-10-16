using RoR2;
using RoR2.Achievements;

namespace EnforcerPlugin.Achievements {

    [RegisterAchievement(identifier, unlockableIdentifier, "ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_ID", 3, null)]
    public class StunGrenadeAchievement : BaseAchievement
    {
        public const string identifier = "ENFORCER_STUNGRENADEUNLOCKABLE_ACHIEVEMENT_ID" + Iter.ator;
        public const string unlockableIdentifier = "ENFORCER_STUNGRENADEUNLOCKABLE_REWARD_ID" + Iter.ator;
        public const string AchievementSpriteName = "texStunGrenadeAchievement";

        public bool checkThisFrame;

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("EnforcerBody");
        }

        public override void OnInstall()
        {
            base.OnInstall();
            On.RoR2.CharacterBody.SetBuffCount += CharacterBody_SetBuffCount;
            RoR2Application.onFixedUpdate += RoR2Application_onFixedUpdate;
        }

        public override void OnUninstall()
        {
            base.OnUninstall();
            On.RoR2.CharacterBody.SetBuffCount -= CharacterBody_SetBuffCount;
            RoR2Application.onFixedUpdate -= RoR2Application_onFixedUpdate;
        }

        private void CharacterBody_SetBuffCount(On.RoR2.CharacterBody.orig_SetBuffCount orig, CharacterBody self, BuffIndex buffType, int newCount)
        {
            orig(self, buffType, newCount);

            checkThisFrame = true;
        }

        private void RoR2Application_onFixedUpdate()
        {
            if (!checkThisFrame)
            {
                return;
            }

            checkThisFrame = false;

            int count = 0;

            var bodies = CharacterBody.readOnlyInstancesList;
            for (int i = 0; i < bodies.Count; i++)
            {
                if (bodies[i] && bodies[i].HasBuff(Modules.Buffs.impairedBuff))
                {
                    count++;
                }
            }

            if (count >= 20 && base.meetsBodyRequirement)
            {
                base.Grant();
            }
        }
    }
}