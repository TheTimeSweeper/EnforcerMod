using System;
using System.Collections.Generic;
using System.Linq;
using RoR2;
using R2API;

namespace EnforcerPlugin
{
	public class TokenReplacers
	{
		// Usage: Tokens that completely change their value based on a condition.
		// ex: Enforcer's "Nothing Risked and Nothing Rained Achievement"
		private struct ConditionalFullTokenReplacement
		{
			public string token;
			public string replacementToken;
			public bool condition;
		}
		
		
		//  TOKEN_CUM: "Fire cum for {0}x{1}% damage."; int cumCount = 9; float cumDamage = 0.5f;
		//  FormatToken("TOKEN_CUM", "en", new string[]{cumCount, cumDamage * 100}
		// => returns "Fire cum for 9x50% damage."
        private static string FormatToken(string token, string lang, params string[] objects)
        {
            var oldString = RoR2.Language.GetString(token, lang);
            var newString = string.Format(oldString, objects);
            return newString;
        }
		
		
		//Enforcer
		//Is the ToString redundant? check in an IDE for fucks sakes.
		//If you add any new tokens that have parameters, put them here
		//
		public static Dict<string, string[]> tokenPlusParams = new Dict<string, string[]>()
		{
			//Enfucker
			NTPP("ENFORCER_PRIMARY_SHOTGUN_DESCRIPTION", Config.shotgunBulletCount.Value, (100f * Modules.Config.shotgunDamage.Value),
			NTPP("ENFORCER_PRIMARY_SUPERSHOTGUN_DESCRIPTION", (SuperShotgun2.bulletCount / 2), (100f * Config.superDamage.Value)),
			NTPP("ENFORCER_PRIMARY_RIFLE_DESCRIPTION", (100f * FireMachineGun.damageCoefficient)),
			NTPP("ENFORCER_PRIMARY_HAMMER_DESCRIPTION", (100f * HammerSwing.damageCoefficient),(100f * HammerSwing.shieldDamageCoefficient)),
			NTPP("ENFORCER_SECONDARY_BASH_DESCRIPTION", (100f * ShieldBash.damageCoefficient)),
			NTPP("ENFORCER_UTILITY_STUNGRENADE_DESCRIPTION", (100f * StunGrenade.damageCoefficient)),
			NTPP("ENFORCER_UTILITY_SHOCKGRENADE_DESCRIPTION", (100f * ShockGrenade.damageCoefficient)),
			
			//Unfucker
			NTPP("NEMFORCER_PASSIVE_DESCRIPTION", (100 * NemforcerPlugin.passiveRegenBonus)),
			NTPP("NEMFORCER_PRIMARY_HAMMER_DESCRIPTION", (100f * EntityStates.Nemforcer.HammerSwing.damageCoefficient)),
			NTPP("NEMFORCER_PRIMARY_THROWHAMMER_DESCRIPTION", (100f * EntityStates.Nemforcer.ThrowHammer.damageCoefficient)),
			NTPP("NEMFORCER_PRIMARY_MINIGUN_DESCRIPTION", (NemMinigunFire.baseDamageCoefficient * 100f)),
			NTPP("NEMFORCER_SECONDARY_BASH_DESCRIPTION", (100f * HammerUppercut.minDamageCoefficient), (100f * HammerUppercut.maxDamageCoefficient)),
			NTPP("NEMFORCER_SECONDARY_SLAM_DESCRIPTION", (100f * HammerSlam.damageCoefficient)),
			NTPP("NEMFORCER_UTILITY_JUMP_DESCRIPTION", (100f * SuperDededeJump.slamDamageCoefficient)),
			NTPP("NEMFORCER_UTILITY_CRASH_DESCRIPTION", 100f * HeatCrash.slamDamageCoefficient)),
			
			//Keywords
			NTPP("KEYWORD_SPRINTBASH", (100f * ShoulderBash.chargeDamageCoefficient), (ShoulderBash.knockbackDamageCoefficient * 100f)),
			NTPP("KEYWORD_SLAM", (100f * HammerAirSlam.minDamageCoefficient), (100f * HammerAirSlam.maxDamageCoefficient))
			
			//Achievements
			NTPP("ENFORCER_GRANDMASTERYUNLOCKABLE_ACHIEVEMENT_DESC", (EnforcerModPlugin.starstormInstalled ? Language.GetString("UNLOCKABLE_MASTERYFOOTNOTE") : "")),
			NTPP("NEMFORCER_TYPHOONUNLOCKABLE_ACHIEVEMENT_DESC", (EnforcerModPlugin.starstormInstalled ? Language.GetString("UNLOCKABLE_MASTERYFOOTNOTE") : "")),
			
		};
		
		//technically this can be achieved with tokenPlusParams list, but this feels cleaner
		public static List<ConditionalFullTokenReplacement> fullConditionalReplacements = new List<ConditionalFullTokenReplacement>()
		{
			new ConditionalFullTokenReplacement(){ token = "ENFORCER_OUTRO_FLAVOR", replacementToken = "ENFORCER_OUTRO_FLAVOR_CHEATED", condition = Modules.Config.forceUnlock.Value}
		};
		
		
		//new tokenPlusParam
		//string.Format accepts an object[], do i really fucking need this?
		//If I don't, replace tokenPlusParams string[] to object[]
		public static KeyValuePair<string, string[]> NTPP(string token, object[] parameters)
		{
			List<string> replacementParams = new List<string>();
			foreach (var param in parameters)
			{
				replacementParams.Add(param.ToString());
			}
			return new KeyValuePair(){token, replacementParams.ToArray()};
		}

		//A more userfriendly look for an overload, add more params as needed but 6 seems like overkill already.
		// NTPP("ENFORCER_PRIMARY_HAMMER_DESCRIPTION", new object[]{(100f * HammerSwing.damageCoefficient),(100f * HammerSwing.shieldDamageCoefficient)}),
		// vs
		// NTPP("ENFORCER_PRIMARY_HAMMER_DESCRIPTION", (100f * HammerSwing.damageCoefficient), (100f * HammerSwing.shieldDamageCoefficient))
		// its like 14 character reduction per usage "new object[]{}" (14)
		//maybe i should replace the old method entirely
		public static KeyValuePair<string, string[]> NTPP(string token, object param1 = null, object param2 = null, object param3 = null, object param4 = null, object param5 = null, object param6 = null)
		{
			return NTPP(token, new object[]{param1, param2, param3, param4, param5, param6};
		}			
		
		//called after language tokens already gathered by say R2API
		//imo why use zio if r2api already does the work?
		private static void SetupLanguage()
		{
			//if you're not doing this you're patching one language
			//quicker but i dont have a check to 
			foreach (var lang in RoR2.Language.steamLanguageTable)
			{
                var langName = lang.Value.webApiName;
				
				//havent tested but it should cut down on replacements
				//this check should probably be removed when all languages are added
				//ex: english, result is "Unwavering Bastion" so it would do the rest
				//on the other hand if its chinese then the result would be the token itself, and skip it.
				//which also means, don't add the language until its complete. idk
				if (Language.GetString("ENFORCER_BODY_SUBTITLE", langName) == "ENFORCER_BODY_SUBTITLE")
				{
					continue;
				}
				
				
				// Special or conditional replacements
				foreach (var info in fullConditionalReplacements)
				{
					if (info.condition)
					{
						LanguageAPI.Add(info.token, Language.GetString(info.replacementToken), langName);
					}
				}
				
				foreach (var tokenParams in tokenPlusParams)
				{
					var newString = FormatToken(tokenParams.Key, langName, tokenParams.Value);
					LanguageAPI.Add(tokenParams.Key, newString, langName);
				}
			}
		}
		
		//what a crusty late setup
		//btw i use this alot in my own mods
		public static Init()
		{
            On.RoR2.UI.MainMenu.MainMenuController.Start += FinalizeLanguage;
		}
		
		private static void FinalizeLanguage(On.RoR2.UI.MainMenu.MainMenuController.orig_Start orig, RoR2.UI.MainMenu.MainMenuController self)
        {
            orig(self);
            SetupLanguage();
            //SetupConfigLanguage(config);
            On.RoR2.UI.MainMenu.MainMenuController.Start -= FinalizeLanguage;
        }
		
	}
}
