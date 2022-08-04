using EnforcerPlugin;
using EntityStates.Enforcer;
using EntityStates.Enforcer.NeutralSpecial;
using R2API;
using System;

namespace Modules {
    internal class Tokens {
        //we doin this?
        public static void RegisterTokens() {

            RegisterEnforcerTokens();

            RegisterNemforcerTokens();

        }

        private static void RegisterNemforcerTokens() {

            #region Achievements
            //character
            LanguageAPI.Add("ENFORCER_NEMESIS2UNLOCKABLE_ACHIEVEMENT_NAME", "???");
            LanguageAPI.Add("ENFORCER_NEMESIS2UNLOCKABLE_ACHIEVEMENT_DESC", "On monsoon, stabilize the anomaly, and defeat Enforcer's Vestige.");
            LanguageAPI.Add("ENFORCER_NEMESIS2UNLOCKABLE_UNLOCKABLE_NAME", "???");

            LanguageAPI.Add("NEMFORCER_MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "Nemesis Enforcer: Mastery");
            LanguageAPI.Add("NEMFORCER_MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "As Nemesis Enforcer, beat the game or obliterate on Monsoon.");
            LanguageAPI.Add("NEMFORCER_MASTERYUNLOCKABLE_UNLOCKABLE_NAME", "Nemesis Enforcer: Mastery");

            string masteryFootnote = EnforcerModPlugin.starstormInstalled ? "" : "\n<color=#8888>(Counts any difficulty Typhoon or higher)</color>";

            LanguageAPI.Add("NEMFORCER_TYPHOONUNLOCKABLE_ACHIEVEMENT_NAME", "Nemesis Enforcer: Grand Mastery");
            LanguageAPI.Add("NEMFORCER_TYPHOONUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, beat the game or obliterate on Typhoon or Eclipse." + masteryFootnote);
            LanguageAPI.Add("NEMFORCER_TYPHOONUNLOCKABLE_UNLOCKABLE_NAME", "Nemesis Enforcer: Grand Mastery");

            //sken
            LanguageAPI.Add("NEMFORCER_DOMINANCEUNLOCKABLE_ACHIEVEMENT_NAME", "Nemesis Enforcer: Demolition");
            LanguageAPI.Add("NEMFORCER_DOMINANCEUNLOCKABLE_ACHIEVEMENT_DESC", "As Nemesis Enforcer, destroy 5 projectiles at once with Dominance.");
            LanguageAPI.Add("NEMFORCER_DOMINANCEUNLOCKABLE_UNLOCKABLE_NAME", "Nemesis Enforcer: Demolition");
            #endregion
        }

        private static void RegisterEnforcerTokens() {

            #region Enforcer

            const string characterName = "Enforcer";
            const string characterSubtitle = "Unwavering Bastion";
            const string characterOutro = "..and so he left, unsure of his title as protector.";
            const string characterOutroFailure = "..and so he vanished, the planet's minorities finally at peace.";
            const string characterLore = "\n<style=cMono>\"You don't have to do this.\"</style>\r\n\r\nThe words echoed in his head, yet he pushed forward. The pod was only a few steps away — he had a chance to leave — but something in his core kept him moving. He didn't know what it was, but he didn't question it. It was a natural force: the same force that always drove him to follow orders.\n\nThis time, however, it didn't seem so natural. There were no orders. The heavy trigger and its rhythmic thunder were his — and his alone.";

            string outro = characterOutro;
            if (Modules.Config.forceUnlock.Value) outro = "..and so he left, having cheated not only the game, but himself. He didn't grow. He didn't improve. He took a shortcut and gained nothing. He experienced a hollow victory. Nothing was risked and nothing was rained.";

            string desc = "The Enforcer is a defensive juggernaut who can take and give a beating.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine
                 + "< ! > Riot Shotgun can pierce through many enemies at once." + Environment.NewLine + Environment.NewLine
                 + "< ! > Batting away enemies with Shield Bash guarantees you will keep enemies at a safe range." + Environment.NewLine + Environment.NewLine
                 + "< ! > Use Tear Gas to weaken large crowds of enemies, then get in close and crush them." + Environment.NewLine + Environment.NewLine
                 + "< ! > When you can, use Protect and Serve against walls to prevent enemies from flanking you." + Environment.NewLine + Environment.NewLine;

            LanguageAPI.Add("ENFORCER_NAME", characterName);
            LanguageAPI.Add("ENFORCER_DESCRIPTION", desc);
            LanguageAPI.Add("ENFORCER_SUBTITLE", characterSubtitle);
            //LanguageAPI.Add("ENFORCER_LORE", "I'M FUCKING INVINCIBLE");
            LanguageAPI.Add("ENFORCER_LORE", characterLore);
            LanguageAPI.Add("ENFORCER_OUTRO_FLAVOR", outro);
            LanguageAPI.Add("ENFORCER_OUTRO_FAILURE", characterOutroFailure);

            #endregion Enforcer

            #region Skills

            #region Primary
            //riot
            string riotDesc = "Fire a short-range blast that <style=cIsUtility>pierces</style> for <style=cIsDamage>" + Config.shotgunBulletCount.Value + "x" + 100f * Modules.Config.shotgunDamage.Value + "% damage.</style>";

            LanguageAPI.Add("ENFORCER_PRIMARY_SHOTGUN_NAME", "Riot Shotgun");
            LanguageAPI.Add("ENFORCER_PRIMARY_SHOTGUN_DESCRIPTION", riotDesc);

            //super
            string superDesc = "Fire up to 2 shotgun blasts for <style=cIsDamage>" + SuperShotgun2.bulletCount / 2 + "x" + 100f * Config.superDamage.Value + "% damage</style>.\nWhile using <style=cIsUtility>Protect and Serve</style>, fire <style=cIsDamage>both barrels at once.</style>";

            LanguageAPI.Add("ENFORCER_PRIMARY_SUPERSHOTGUN_NAME", "Super Shotgun");
            LanguageAPI.Add("ENFORCER_PRIMARY_SUPERSHOTGUN_DESCRIPTION", superDesc);

            //ar
            string arDamage = $"<style=cIsDamage>{100f * FireMachineGun.damageCoefficient}% damage</style>";

            string arDesc = $"Unload a barrage of bullets for {arDamage}.\nWhile using <style=cIsUtility>Protect and Serve</style>, has <style=cIsDamage>increased accuracy</style>, but <style=cIsHealth>slower movement speed</style>.";

            LanguageAPI.Add("ENFORCER_PRIMARY_RIFLE_NAME", "Heavy Machine Gun");
            LanguageAPI.Add("ENFORCER_PRIMARY_RIFLE_DESCRIPTION", arDesc);

            //Hammer
            string damage = $"<style=cIsDamage>{ 100f * HammerSwing.damageCoefficient}% damage</style>";
            string shieldDamage = $"<style=cIsDamage>{ 100f * HammerSwing.shieldDamageCoefficient}% damage</style>";
            string hamDesc = $"Swing your hammer for {damage}.\nWhile using Protect and Serve, swing in a <style=cIsUtility>larger area</style>, for {shieldDamage} instead.";

            LanguageAPI.Add("ENFORCER_PRIMARY_HAMMER_NAME", "Breaching Hammer");
            LanguageAPI.Add("ENFORCER_PRIMARY_HAMMER_DESCRIPTION", hamDesc);
            #endregion Primary

            #region Secondary
            LanguageAPI.Add("KEYWORD_BASH", "<style=cKeywordName>Bash</style><style=cSub>Applies <style=cIsDamage>stun</style> and <style=cIsUtility>heavy knockback</style>.");

            LanguageAPI.Add("KEYWORD_SPRINTBASH", $"<style=cKeywordName>Shoulder Bash</style><style=cSub><style=cIsDamage>Stunning.</style> A short charge that deals <style=cIsDamage>{100f * ShoulderBash.chargeDamageCoefficient}% damage</style>.\nHitting <style=cIsDamage>heavier enemies</style> deals <style=cIsDamage>{ShoulderBash.knockbackDamageCoefficient * 100f}% damage</style>.");

            //string desc = $"<style=cIsDamage>Bash</style> nearby enemies for <style=cIsDamage>{100f * ShieldBash.damageCoefficient}% damage</style>. <style=cIsUtility>Deflects projectiles</style>. Use while <style=cIsUtility>sprinting</style> to perform a <style=cIsDamage>Shoulder Bash</style> for <style=cIsDamage>{100f * ShoulderBash.chargeDamageCoefficient}-{100f * ShoulderBash.knockbackDamageCoefficient}% damage</style> instead.";
            string shieldBashDesc = $"<style=cIsDamage>Stunning</style>. Knock back enemies for <style=cIsDamage>{100f * ShieldBash.damageCoefficient}% damage</style> and <style=cIsUtility>deflect projectiles</style>.";
            desc += $"\nWhile <style=cIsUtility>sprinting</style>, perform a <style=cIsDamage>Shoulder Bash</style> instead.";
            //desc += $" Deals <style=cIsDamage>{100f * ShoulderBash.chargeDamageCoefficient}% damage</style> while sprinting.";

            LanguageAPI.Add("ENFORCER_SECONDARY_BASH_NAME", "Shield Bash");
            LanguageAPI.Add("ENFORCER_SECONDARY_BASH_DESCRIPTION", shieldBashDesc);
            #endregion Secondary

            #region Utility
            //gas
            LanguageAPI.Add("KEYWORD_BLINDED", "<style=cKeywordName>Impaired</style><style=cSub>Reduce movement speed by <style=cIsDamage>75%</style>, attack speed by <style=cIsDamage>25%</style> and armor by <style=cIsDamage>30</style>.</style></style>");

            LanguageAPI.Add("ENFORCER_UTILITY_TEARGAS_NAME", "Tear Gas");
            LanguageAPI.Add("ENFORCER_UTILITY_TEARGAS_DESCRIPTION", "Toss a grenade that covers an area in <style=cIsDamage>Impairing</style> gas.");

            //stun
            LanguageAPI.Add("ENFORCER_UTILITY_STUNGRENADE_NAME", "Stun Grenade");
            LanguageAPI.Add("ENFORCER_UTILITY_STUNGRENADE_DESCRIPTION", "<style=cIsDamage>Stunning</style>. Launch a grenade that concusses enemies for <style=cIsDamage>" + 100f * StunGrenade.damageCoefficient + "% damage</style>. Hold up to 3.");

            //gas scepter
            LanguageAPI.Add("ENFORCER_UTILITY_TEARGASSCEPTER_NAME", "Mustard Gas");
            LanguageAPI.Add("ENFORCER_UTILITY_TEARGASSCEPTER_DESCRIPTION", "Toss a grenade that covers an area in <style=cIsDamage>Impairing</style> gas, choking enemies for <style=cIsDamage>200% damage per second</style>.");

            //stun scepter
            LanguageAPI.Add("ENFORCER_UTILITY_SHOCKGRENADE_NAME", "Shock Grenade");
            LanguageAPI.Add("ENFORCER_UTILITY_SHOCKGRENADE_DESCRIPTION", "<style=cIsDamage>Shocking</style>. Launch a grenade that electrocutes enemies for <style=cIsDamage>" + 100f * ShockGrenade.damageCoefficient + "% damage</style>. Hold up to 3.");
            #endregion Utility

            #region Special
            //shield
            LanguageAPI.Add("ENFORCER_SPECIAL_SHIELDUP_NAME", "Protect and Serve");
            LanguageAPI.Add("ENFORCER_SPECIAL_SHIELDUP_DESCRIPTION", "Take a defensive stance, <style=cIsUtility>blocking all damage from the front</style>. <style=cIsUtility>Enhances primary fire</style>, but <style=cIsHealth>prevents sprinting and jumping</style>.");

            LanguageAPI.Add("ENFORCER_SPECIAL_SHIELDDOWN_NAME", "Protect and Serve");
            LanguageAPI.Add("ENFORCER_SPECIAL_SHIELDDOWN_DESCRIPTION", "Take a defensive stance, <style=cIsUtility>blocking all damage from the front</style>. <style=cIsDamage>Increases attack speed</style>, but <style=cIsHealth>prevents sprinting and jumping</style>.");

            //energy shield (still believe)

            LanguageAPI.Add("ENFORCER_SPECIAL_SHIELDON_NAME", "Project and Swerve");
            LanguageAPI.Add("ENFORCER_SPECIAL_SHIELDON_DESCRIPTION", "Take a defensive stance, <style=cIsUtility>projecting an Energy Shield in front of you</style>. <style=cIsDamage>Increases your rate of fire</style>, but <style=cIsUtility>prevents sprinting and jumping</style>.");

            LanguageAPI.Add("ENFORCER_SPECIAL_SHIELDOFF_NAME", "Project and Swerve");
            LanguageAPI.Add("ENFORCER_SPECIAL_SHIELDOFF_DESCRIPTION", "Take a defensive stance, <style=cIsUtility>projecting an Energy Shield in front of you</style>. <style=cIsDamage>Increases your rate of fire</style>, but <style=cIsUtility>prevents sprinting and jumping</style>.");

            LanguageAPI.Add("ENFORCER_SPECIAL_BOARDUP_NAME", "Skateboard");
            LanguageAPI.Add("ENFORCER_SPECIAL_BOARDUP_DESCRIPTION", "Swag.");

            LanguageAPI.Add("ENFORCER_SPECIAL_BOARDDOWN_NAME", "Skateboard");
            LanguageAPI.Add("ENFORCER_SPECIAL_BOARDDOWN_DESCRIPTION", "Unswag.");
            #endregion Special

            #endregion Skills

            #region Skins
            LanguageAPI.Add("ENFORCERBODY_DEFAULT_SKIN_NAME", "Default");
            LanguageAPI.Add("ENFORCERBODY_MASTERY_SKIN_NAME", "Peacekeeper");
            LanguageAPI.Add("ENFORCERBODY_TYPHOON_SKIN_NAME", "Lawbringer");
            LanguageAPI.Add("ENFORCERBODY_BOT_SKIN_NAME", "N-4CR");
            LanguageAPI.Add("ENFORCERBODY_CLASSIC_SKIN_NAME", "Classic");

            LanguageAPI.Add("ENFORCERBODY_NEMESIS_SKIN_NAME", "Nemesis");

            //not in yet
            LanguageAPI.Add("ENFORCERBODY_STORM_SKIN_NAME", "Monsoontrooper");
            LanguageAPI.Add("ENFORCERBODY_ENGI_SKIN_NAME", "Engineer?");
            LanguageAPI.Add("ENFORCERBODY_DOOM_SKIN_NAME", "Doom Slayer");
            LanguageAPI.Add("ENFORCERBODY_DESPERADO_SKIN_NAME", "Desperado");
            LanguageAPI.Add("ENFORCERBODY_FEM_SKIN_NAME", "Femforcer");
            LanguageAPI.Add("ENFORCERBODY_FUCKINGSTEVE_SKIN_NAME", "Blocked");
            #endregion Skins

            #region Achievements
            //character
            LanguageAPI.Add("ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_NAME", "Riot");
            LanguageAPI.Add("ENFORCER_CHARACTERUNLOCKABLE_ACHIEVEMENT_DESC", "Kill a Magma Worm, a Wandering Vagrant and a Stone Titan in a single run.");
            LanguageAPI.Add("ENFORCER_CHARACTERUNLOCKABLE_UNLOCKABLE_NAME", "Riot");

            LanguageAPI.Add("ENFORCER_MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: Mastery");
            LanguageAPI.Add("ENFORCER_MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, beat the game or obliterate on Monsoon.");
            LanguageAPI.Add("ENFORCER_MASTERYUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: Mastery");

            string masteryFootnote = EnforcerModPlugin.starstormInstalled ? "" : "\n<color=#8888>(Counts any difficulty Typhoon or higher)</color>";

            LanguageAPI.Add("ENFORCER_GRANDMASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: Grand Mastery");
            LanguageAPI.Add("ENFORCER_GRANDMASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, beat the game or obliterate on Typhoon or Eclipse." + masteryFootnote);
            LanguageAPI.Add("ENFORCER_GRANDMASTERYUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: Grand Mastery");

            //skills
            LanguageAPI.Add("ENFORCER_DOOMUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: Rip and Tear...");
            LanguageAPI.Add("ENFORCER_DOOMUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, kill 40 imps in a single stage.");
            LanguageAPI.Add("ENFORCER_DOOMUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: Rip and Tear...");

            LanguageAPI.Add("ENFORCER_RIFLEUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: Rapidfire");
            LanguageAPI.Add("ENFORCER_RIFLEUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, reach +400% attack speed.");
            LanguageAPI.Add("ENFORCER_RIFLEUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: Rapidfire");

            LanguageAPI.Add("ENFORCER_STUNGRENADEUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: Crowd Control");
            LanguageAPI.Add("ENFORCER_STUNGRENADEUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, have 20 enemies under the effects of Tear Gas at once.");
            LanguageAPI.Add("ENFORCER_STUNGRENADEUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: Crowd Control");

            //skans                                                       //technically not robocop quote but a lot more understandable in general
            LanguageAPI.Add("ENFORCER_ROBITUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: We Have the Technology");
            LanguageAPI.Add("ENFORCER_ROBITUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, fall and be brought back to life.");
            LanguageAPI.Add("ENFORCER_ROBITUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: We Have the Technology");


            LanguageAPI.Add("ENFORCER_NEMESISSKINUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: Clearance");
            LanguageAPI.Add("ENFORCER_NEMESISSKINUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, stabilize the Cell in the Void Fields.");
            LanguageAPI.Add("ENFORCER_NEMESISSKINUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: Clearance");

            LanguageAPI.Add("ENFORCER_DOOMINTERNALUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: ...Until it is done");
            LanguageAPI.Add("ENFORCER_DOOMINTERNALUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, kill 10 imp overlords in a single run.");
            LanguageAPI.Add("ENFORCER_DOOMINTERNALUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: ...Until it is done");

            LanguageAPI.Add("ENFORCER_CLASSICUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: Schmoovin'");
            LanguageAPI.Add("ENFORCER_CLASSICUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, show off your dance moves.");
            LanguageAPI.Add("ENFORCER_CLASSICUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: Schmoovin'");

            LanguageAPI.Add("ENFORCER_DESPERADOUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: Rules of Nature");
            LanguageAPI.Add("ENFORCER_DESPERADOUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, Defeat the unique guardian of Gilded Coast by pushing it off the edge of the map. <color=#c11>Host only</color>");
            LanguageAPI.Add("ENFORCER_DESPERADOUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: Rules of Nature");

            LanguageAPI.Add("ENFORCER_BUNGUSUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: Enforcing Perfection");
            LanguageAPI.Add("ENFORCER_BUNGUSUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, become one with the Bungus.");
            LanguageAPI.Add("ENFORCER_BUNGUSUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: Enforcing Perfection");

            LanguageAPI.Add("ENFORCER_STORMTROOPERUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: Long Live the Empire");
            LanguageAPI.Add("ENFORCER_STORMTROOPERUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, defeat an elite Solus Control Unit. <color=#c11>Host only</color>");
            LanguageAPI.Add("ENFORCER_STORMTROOPERUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: Long Live the Empire");

            LanguageAPI.Add("ENFORCER_FROGUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: Through Thick and Thin");
            LanguageAPI.Add("ENFORCER_FROGUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, make a friend on the moon.");
            LanguageAPI.Add("ENFORCER_FROGUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: Through Thick and Thin");

            LanguageAPI.Add("ENFORCER_STEVEUNLOCKABLE_ACHIEVEMENT_NAME", "Enforcer: Blocks");
            LanguageAPI.Add("ENFORCER_STEVEUNLOCKABLE_ACHIEVEMENT_DESC", "As Enforcer, block an attack with your shield.");
            LanguageAPI.Add("ENFORCER_STEVEUNLOCKABLE_UNLOCKABLE_NAME", "Enforcer: Blocks");

            #endregion Achievements
        }
    }
}