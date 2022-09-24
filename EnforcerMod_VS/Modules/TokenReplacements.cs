using System;
using System.Collections.Generic;
using System.Linq;

namespace EnforcerPlugin
{
	public class TokenReplacers
	{
		public struct ConditionalTokenParams
		{
			public string token;
			public string[] trueParams;
			public string[] falseParams;
			public bool condition;
		}
		
		public struct ConditionalFullTokenReplacement
		{
			public string token;
			public string replacementToken;
			public bool condition;
		}
		
		
		//Enforcer
		//Is the ToString redundant? check in an IDE for fucks sakes.
		public static Dict<string, string[]> tokenPlusParams = new Dict<string, string[]>()
		{
			//Enfucker
			NTPP("ENFORCER_PRIMARY_SHOTGUN_DESCRIPTION", new object[]{Config.shotgunBulletCount.Value, (100f * Modules.Config.shotgunDamage.Value)}},
			NTPP("ENFORCER_PRIMARY_SUPERSHOTGUN_DESCRIPTION", new object[]{(SuperShotgun2.bulletCount / 2), (100f * Config.superDamage.Value)}),
			NTPP("ENFORCER_PRIMARY_RIFLE_DESCRIPTION", new object[]{(100f * FireMachineGun.damageCoefficient)}),
			NTPP("ENFORCER_PRIMARY_HAMMER_DESCRIPTION", new object[]{(100f * HammerSwing.damageCoefficient),(100f * HammerSwing.shieldDamageCoefficient)}),
			NTPP("ENFORCER_SECONDARY_BASH_DESCRIPTION", new object[]{(100f * ShieldBash.damageCoefficient)}),
			NTPP("ENFORCER_UTILITY_STUNGRENADE_DESCRIPTION", new object[]{(100f * StunGrenade.damageCoefficient)}),
			NTPP("ENFORCER_UTILITY_SHOCKGRENADE_DESCRIPTION", new object[]{(100f * ShockGrenade.damageCoefficient)}),
			
			//Unfucker
			NTPP("NEMFORCER_PASSIVE_DESCRIPTION", new object[]{(100 * NemforcerPlugin.passiveRegenBonus)}),
			NTPP("NEMFORCER_PRIMARY_HAMMER_DESCRIPTION", new object[]{(100f * EntityStates.Nemforcer.HammerSwing.damageCoefficient)}),
			NTPP("NEMFORCER_PRIMARY_THROWHAMMER_DESCRIPTION", new object[]{(100f * EntityStates.Nemforcer.ThrowHammer.damageCoefficient)}),
			NTPP("NEMFORCER_PRIMARY_MINIGUN_DESCRIPTION", new object[]{(NemMinigunFire.baseDamageCoefficient * 100f)}),
			NTPP("NEMFORCER_SECONDARY_BASH_DESCRIPTION", new object[]{(100f * HammerUppercut.minDamageCoefficien), (100f * HammerUppercut.maxDamageCoefficient)}),
			NTPP("NEMFORCER_SECONDARY_SLAM_DESCRIPTION", new object[]{(100f * HammerSlam.damageCoefficient)}),
			NTPP("NEMFORCER_UTILITY_JUMP_DESCRIPTION", new object[]{(100f * SuperDededeJump.slamDamageCoefficient)}),
			NTPP("NEMFORCER_UTILITY_CRASH_DESCRIPTION", new object[]{(100f * HeatCrash.slamDamageCoefficient)}),
			
			//Keywords
			NTPP("KEYWORD_SPRINTBASH", new object[]{(100f * ShoulderBash.chargeDamageCoefficient), (ShoulderBash.knockbackDamageCoefficient * 100f)}),
			NTPP("KEYWORD_SLAM", new object[]{(100f * HammerAirSlam.minDamageCoefficient), (100f * HammerAirSlam.maxDamageCoefficient)}),
		};
		
		//new tokenPlusParam
		//string.Format accepts an object[], do i really fucking need this?
		//If I don't, replace tokenPlusParams string[] to object[]
		public static KeyValuePair<string, string[]>NTPP(string token, object[] params)
		{
			List<string> replacementParams = new List<string>();
			foreach (var param in params)
			{
				replacementParams.Add(param.ToString());
			}
			return new KeyValuePair(){token, replacementParams.ToArray()};
		}
		
		public static List<ConditionalTokenParams> conditionalReplacements = new List<ConditionalTokenParams>()
		{
			new ConditionalTokenParams(){ token = "ENFORCER_GRANDMASTERYUNLOCKABLE_ACHIEVEMENT_DESC", trueParams = new string[]{Language.GetString("UNLOCKABLE_MASTERYFOOTNOTE")}, falseParams = new string[]{""}, condition = EnforcerModPlugin.starstormInstalled},
			new ConditionalTokenParams(){ token = "NEMFORCER_TYPHOONUNLOCKABLE_ACHIEVEMENT_DESC", trueParams = new string[]{Language.GetString("UNLOCKABLE_MASTERYFOOTNOTE")}, falseParams = new string[]{""}, condition = EnforcerModPlugin.starstormInstalled},
		};
		
		public static List<ConditionalFullTokenReplacement> fullConditionalReplacements = new List<ConditionalFullTokenReplacement>()
		{
			new ConditionalFullTokenReplacement(){ token = "ENFORCER_OUTRO_FLAVOR", replacementToken = "ENFORCER_OUTRO_FLAVOR_CHEATED", condition = Modules.Config.forceUnlock.Value}
		};
		
		//call me after language is setup by r2api thanks
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
		
		private static void SetupLanguage()
		{
			ApplySpecialReplacements();
			ApplyReplacements();
		}
		
		public static ApplyReplacements()
		{
			foreach (var info in tokenPlusParams)
			{
				LanguageAPI.Add(info.key, info.value)
			}
		}
		
		public static ApplySpecialReplacements()
		{
			foreach (var info in fullConditionalReplacements)
			{
				if (info.condition)
					LanguageAPI.Add(info.token, Language.GetString(info.replacementToken);
			}
			foreach (var info in conditionalReplacements)
			{
				string resolvedParams = info.condition ? trueParams : falseParams;
				string tokenValue = Language.GetString(info.token);
				string resolvedValue = string.Format(tokenValue, resolvedParams);
				LanguageAPI.Add(info.token, resolvedValue);
			}
		}
		
	}
}
