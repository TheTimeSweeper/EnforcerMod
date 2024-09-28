using RoR2;
using R2API;
using EnforcerPlugin.Achievements;
using EnforcerPlugin;
using UnityEngine;

namespace EnforcerPlugin.Achievements {
    public static class knee {
        public const string grow = "";
    }
}

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
            enforcerUnlockableDef = Config.forceUnlock.Value ? null : Modules.Content.CreateAndAddUnlockbleDef(
                EnforcerUnlockAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(EnforcerUnlockAchievement.identifier),
                Asset.MainAssetBundle.LoadAsset<Sprite>(EnforcerUnlockAchievement.AchievementSpriteName)); 
            
            enforcerMasteryUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                MasteryAchievementButEpic.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(MasteryAchievementButEpic.identifier),
                Asset.MainAssetBundle.LoadAsset<Sprite>(MasteryAchievementButEpic.AchievementSpriteName));
            enforcerGrandMasteryUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                GrandMasteryAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(GrandMasteryAchievement.identifier),
                Asset.MainAssetBundle.LoadAsset<Sprite>(GrandMasteryAchievement.AchievementSpriteName)); 

            enforcerDoomUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                DoomAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(DoomAchievement.identifier),
                Asset.MainAssetBundle.LoadAsset<Sprite>(DoomAchievement.AchievementSpriteName));

            enforcerARUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                AssaultRifleAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(AssaultRifleAchievement.identifier),
                Asset.MainAssetBundle.LoadAsset<Sprite>(AssaultRifleAchievement.AchievementSpriteName));

            enforcerStunGrenadeUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                StunGrenadeAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(StunGrenadeAchievement.identifier),
                Asset.MainAssetBundle.LoadAsset<Sprite>(StunGrenadeAchievement.AchievementSpriteName));
            
            enforcerRobitUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                RobitAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(RobitAchievement.identifier),
                Asset.MainAssetBundle.LoadAsset<Sprite>(RobitAchievement.AchievementSpriteName));
            enforcerNemesisSkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                RecolorNemesisSkinAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(RecolorNemesisSkinAchievement.identifier),
                Asset.MainAssetBundle.LoadAsset<Sprite>(RecolorNemesisSkinAchievement.AchievementSpriteName)); 
            enforcerClassicUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                ClassicAchievement.unlockableIdentifier, 
                Modules.Tokens.GetAchievementNameToken(ClassicAchievement.identifier), 
                Asset.MainAssetBundle.LoadAsset<Sprite>(ClassicAchievement.AchievementSpriteName));
            
            if (!Config.hateFun.Value)
            {
                enforcerDoomSkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                    DoomAchievement2.unlockableIdentifier, 
                    Modules.Tokens.GetAchievementNameToken(DoomAchievement2.identifier), 
                    Asset.MainAssetBundle.LoadAsset<Sprite>(DoomAchievement2.AchievementSpriteName));
                enforcerDesperadoSkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(DesperadoAchievement.unlockableIdentifier, Modules.Tokens.GetAchievementNameToken(DesperadoAchievement.identifier), Asset.MainAssetBundle.LoadAsset<Sprite>(DesperadoAchievement.AchievementSpriteName));
                enforcerEngiSkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(BungusAchievement.unlockableIdentifier, Modules.Tokens.GetAchievementNameToken(BungusAchievement.identifier), Asset.MainAssetBundle.LoadAsset<Sprite>(BungusAchievement.AchievementSpriteName));
                enforcerStormSkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(StormtrooperAchievement.unlockableIdentifier, Modules.Tokens.GetAchievementNameToken(StormtrooperAchievement.identifier), Asset.MainAssetBundle.LoadAsset<Sprite>(StormtrooperAchievement.AchievementSpriteName));
            }
            
            if (Config.cursed.Value)
            {
                enforcerSteveUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(SteveAchievement.unlockableIdentifier, Modules.Tokens.GetAchievementNameToken(SteveAchievement.identifier), Asset.MainAssetBundle.LoadAsset<Sprite>(SteveAchievement.AchievementSpriteName));
                enforcerFrogUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(FrogAchievement.unlockableIdentifier, Modules.Tokens.GetAchievementNameToken(FrogAchievement.identifier), Asset.MainAssetBundle.LoadAsset<Sprite>(FrogAchievement.AchievementSpriteName));
            }

            nemesisUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(NemesisAchievement.unlockableIdentifier, Modules.Tokens.GetAchievementNameToken(NemesisAchievement.identifier), Asset.MainAssetBundle.LoadAsset<Sprite>(NemesisAchievement.AchievementSpriteName));
            nemMasteryUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                    NemMasteryAchievementButEpic.unlockableIdentifier,
                    Modules.Tokens.GetAchievementNameToken(NemMasteryAchievementButEpic.identifier),
                    Asset.MainAssetBundle.LoadAsset<Sprite>(NemMasteryAchievementButEpic.AchievementSpriteName)); 
            nemGrandMasteryUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(NemGrandMasteryAchievement.unlockableIdentifier, Modules.Tokens.GetAchievementNameToken(NemGrandMasteryAchievement.identifier), Asset.MainAssetBundle.LoadAsset<Sprite>(NemGrandMasteryAchievement.AchievementSpriteName));
            nemDominanceUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(NemDominanceAchievement.unlockableIdentifier, Modules.Tokens.GetAchievementNameToken(NemDominanceAchievement.identifier), Asset.MainAssetBundle.LoadAsset<Sprite>(NemDominanceAchievement.AchievementSpriteName));
        }
    }
}

namespace EnforcerPlugin.Achievements {

    #region fuck

    //PSYCH THEY'RE ALL FIXED MOTHERFUCKER

    #endregion

}