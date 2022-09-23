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

        public static UnlockableDef enforcerDesperadoSkinUnlockableDef;
        public static UnlockableDef enforcerEngiSkinUnlockableDef;
        public static UnlockableDef enforcerStormSkinUnlockableDef;
        public static UnlockableDef enforcerDoomSkinUnlockableDef;

        public static UnlockableDef enforcerSteveUnlockableDef;
        public static UnlockableDef enforcerFrogUnlockableDef;

        //Nemforcer
        public static UnlockableDef nemesisUnlockableDef;
        public static UnlockableDef nemMasteryUnlockableDef;
        public static UnlockableDef nemGrandMasteryUnlockableDef;
        public static UnlockableDef nemDominanceUnlockableDef;

        public static void RegisterUnlockables() {

            //this is the version that works with the altered AddUnlockable I changed in R2API.
            //look at #r2api in the discord to see what I mean. I went into more detail in #development as well
            //      lol fucking that was a year ago you have to search from:timesweeper i was retarded for saying this
            //if the pull requests gets accepted I'll add the other needed ones to this
            //      fucking it was never merged in i'm reeing so hard right now
            enforcerUnlockableDef = Config.forceUnlock.Value ? null : UnlockableAPI.AddUnlockable<EnforcerUnlockAchievement>(typeof(EnforcerUnlockAchievement.EnforcerUnlockAchievementServer));

            enforcerMasteryUnlockableDef = UnlockableAPI.AddUnlockable<MasteryAchievementButEpic>();
            enforcerGrandMasteryUnlockableDef = UnlockableAPI.AddUnlockable<GrandMasteryAchievement>();

            enforcerDoomUnlockableDef = UnlockableAPI.AddUnlockable<DoomAchievement>(typeof(DoomAchievement.DoomAchievementServer));
            enforcerARUnlockableDef = UnlockableAPI.AddUnlockable<AssaultRifleAchievement>();
            enforcerStunGrenadeUnlockableDef = UnlockableAPI.AddUnlockable<StunGrenadeAchievement>();

            enforcerRobitUnlockableDef = UnlockableAPI.AddUnlockable<RobitAchievement>(typeof(RobitAchievement.RobitAchievementServer));
            enforcerNemesisSkinUnlockableDef = UnlockableAPI.AddUnlockable<RecolorNemesisSkinAchievement>(typeof(RecolorNemesisSkinAchievement.RecolorNemesisSkinAchievementServer));
            enforcerClassicUnlockableDef = UnlockableAPI.AddUnlockable<ClassicAchievement>();
            
            if (!Config.hateFun.Value)
            {
                enforcerDoomSkinUnlockableDef = UnlockableAPI.AddUnlockable<DoomAchievement2>(typeof(DoomAchievement2.DoomAchievement2Server));
                enforcerDesperadoSkinUnlockableDef = UnlockableAPI.AddUnlockable<DesperadoAchievement>(typeof(DesperadoAchievement.DesperadoAchievementServer));
                enforcerEngiSkinUnlockableDef = UnlockableAPI.AddUnlockable<BungusAchievement>();
                enforcerStormSkinUnlockableDef = UnlockableAPI.AddUnlockable<StormtrooperAchievement>(typeof(StormtrooperAchievement.StormtrooperAchievementServer));
            }
            
            if (Config.cursed.Value)
            {
                enforcerSteveUnlockableDef = UnlockableAPI.AddUnlockable<SteveAchievement>(typeof(SteveAchievement.SteveAchievementServer));
                enforcerFrogUnlockableDef = UnlockableAPI.AddUnlockable<FrogAchievement>(typeof(FrogAchievement.FrogAchievementServer));
            }

            nemesisUnlockableDef = UnlockableAPI.AddUnlockable<NemesisAchievement>();
            nemMasteryUnlockableDef = UnlockableAPI.AddUnlockable<NemMasteryAchievement>();
            nemGrandMasteryUnlockableDef = UnlockableAPI.AddUnlockable<NemGrandMasteryAchievement>();
            nemDominanceUnlockableDef = UnlockableAPI.AddUnlockable<NemDominanceAchievement>();
        }
    }
}

namespace EnforcerPlugin.Achievements {

    #region fuck

    //PSYCH THEY'RE ALL FIXED MOTHERFUCKER

    #endregion

}