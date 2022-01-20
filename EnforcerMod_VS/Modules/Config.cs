using BepInEx;
using BepInEx.Configuration;
using System;
using UnityEngine;

namespace EnforcerPlugin.Modules {
    public class Config {

        public static ConfigEntry<bool> forceUnlock;
        public static ConfigEntry<bool> classicShotgun;
        public static ConfigEntry<bool> classicIcons;
        public static ConfigEntry<float> headSize;
        public static ConfigEntry<bool> sprintShieldCancel;
        public static ConfigEntry<bool> sirenOnDeflect;
        public static ConfigEntry<bool> useNeedlerCrosshair;
        public static ConfigEntry<bool> cursed;
        public static ConfigEntry<bool> hateFun;
        //public static ConfigEntry<bool> femSkin;
        public static ConfigEntry<bool> shellSounds;
        public static ConfigEntry<bool> globalInvasion;
        public static ConfigEntry<bool> multipleInvasions;
        public static ConfigEntry<bool> kingDededeBoss;

        public static ConfigEntry<KeyCode> restKey;
        public static ConfigEntry<KeyCode> saluteKey;
        public static ConfigEntry<KeyCode> danceKey;
        public static ConfigEntry<KeyCode> runKey;
        public static ConfigEntry<KeyCode> sirensKey;

        //i don't wanna fucking buff him so i have no choice but to do this
        public static ConfigEntry<float> baseHealth;
        public static ConfigEntry<float> healthGrowth;
        public static ConfigEntry<float> baseDamage;
        public static ConfigEntry<float> damageGrowth;
        public static ConfigEntry<float> baseArmor;
        public static ConfigEntry<float> armorGrowth;
        public static ConfigEntry<float> baseMovementSpeed;
        public static ConfigEntry<float> baseCrit;
        public static ConfigEntry<float> baseRegen;
        public static ConfigEntry<float> regenGrowth;

        public static ConfigEntry<float> shotgunDamage;
        public static ConfigEntry<int> shotgunBulletCount;
        public static ConfigEntry<float> shotgunProcCoefficient;
        public static ConfigEntry<float> shotgunRange;
        public static ConfigEntry<float> shotgunSpread;

        public static ConfigEntry<float> rifleDamage;
        public static ConfigEntry<int> rifleBaseBulletCount;
        public static ConfigEntry<float> rifleProcCoefficient;
        public static ConfigEntry<float> rifleRange;
        public static ConfigEntry<float> rifleSpread;

        public static ConfigEntry<float> superDamage;
        public static ConfigEntry<float> superSpread;
        public static ConfigEntry<float> superDuration;
        public static ConfigEntry<float> superBeef;

        public static ConfigEntry<bool> balancedShieldBash;
        public static ConfigEntry<bool> stupidShieldBash;


        public static void ConfigShit(BaseUnityPlugin plugin) {

            #region general
            forceUnlock
                = plugin.Config.Bind("01 - General Settings",
                                     "Force Unlock",
                                     false,
                                     "Makes Enforcer unlocked by default");
            classicShotgun
                = plugin.Config.Bind("01 - General Settings",
                                     "Classic Shotgun",
                                     false,
                                     "Use RoR1 shotgun sound");
            classicIcons
                = plugin.Config.Bind("01 - General Settings",
                                     "Classic Icons",
                                     false,
                                     "Use RoR1 skill icons");
            headSize
                = plugin.Config.Bind("01 - General Settings",
                                     "Head Size",
                                     1f,
                                     "Changes the size of Enforcer's head");
            sprintShieldCancel
                = plugin.Config.Bind("01 - General Settings",
                                     "Sprint Cancels Shield",
                                     true,
                                     "Allows Protect and Serve to be cancelled by pressing sprint rather than special again");
            sirenOnDeflect
                = plugin.Config.Bind("01 - General Settings",
                                     "Siren on Deflect",
                                     true,
                                     "Play siren sound upon deflecting a projectile");
            useNeedlerCrosshair
                = plugin.Config.Bind("01 - General Settings",
                                     "Visions Crosshair",
                                     true,
                                     "Gives every survivor the custom crosshair for Visions of Heresy");
            cursed
                = plugin.Config.Bind("01 - General Settings",
                                     "Cursed",
                                     false,
                                     "Enables extra/unfinished content. Enable at own risk.");
            //hateFun
            //  = plugin.Config.Bind("01 - General Settings",
            //                       "I hate fun",
            //                       false,
            //                       "Overrides cursed. Further disables extra content, namely skins and their achievements");
            //cursed.Value |= hateFun.Value;

            //femSkin
            //  = plugin.Config.Bind("01 - General Settings",
            //                       "Femforcer",
            //                       false,
            //                       "Enables femforcer skin. Not for good boys and girls.");
            shellSounds
                = plugin.Config.Bind("01 - General Settings",
                                     "Shell Sounds",
                                     true,
                                     "Play a sound when ejected shotgun shells hit the ground");
            globalInvasion
                = plugin.Config.Bind("01 - General Settings",
                                     "Global Invasion",
                                     false,
                                     "Allows invasions when playing any character, not just Enforcer. Purely for fun.");
            multipleInvasions
                = plugin.Config.Bind("01 - General Settings",
                                     "Multiple Invasion Bosses",
                                     false,
                                     "Allows multiple bosses to spawn from an invasion.");
            kingDededeBoss
                = plugin.Config.Bind("01 - General Settings",
                                     "King Dedede Boss",
                                     false,
                                     "Adds a King Dedede boss that spawns on Sky Meadow and post-loop Titanic Plains.");
            #endregion general

            #region keybinds
            restKey
                = plugin.Config.Bind("02 - Keybinds",
                                     "Rest Emote",
                                     KeyCode.Alpha1,
                                     "Key used to Chair");
            saluteKey
                = plugin.Config.Bind("02 - Keybinds",
                                     "Salute Emote",
                                     KeyCode.Alpha2,
                                     "Key used to Salute");

            if (!EnforcerModPlugin.holdonasec)
            {
                danceKey
                    = plugin.Config.Bind("02 - Keybinds",
                                         "Dance Moves Emote",
                                         KeyCode.Alpha3,
                                         "the Default one");
                runKey
                    = plugin.Config.Bind("02 - Keybinds",
                                            "Earl Run Emote",
                                            KeyCode.Alpha4,
                                            "FLINT LOCKWOOD");
            }
            sirensKey
                = plugin.Config.Bind("02 - Keybinds",
                                        "Sirens",
                                        KeyCode.CapsLock,
                                        "Key used to toggle sirens");
            #endregion keybinds

            #region character stats
            baseHealth
                = plugin.Config.Bind("03 - Character Stats",
                                     "Base Health",
                                     160f,
                                     "");
            healthGrowth
                = plugin.Config.Bind("03 - Character Stats",
                                     "Health Growth",
                                     48f,
                                     "");
            baseRegen
                = plugin.Config.Bind("03 - Character Stats",
                                     "Base Health Regen",
                                     1f,
                                     "");
            regenGrowth
                = plugin.Config.Bind("03 - Character Stats",
                                     "Health Regen Growth",
                                     0.2f,
                                     "");
            baseArmor
                = plugin.Config.Bind("03 - Character Stats",
                                     "Base Armor",
                                     15f,
                                     "");
            armorGrowth
                = plugin.Config.Bind("03 - Character Stats",
                                     "Armor Growth",
                                     0f,
                                     "");
            baseDamage
                = plugin.Config.Bind("03 - Character Stats",
                                     "Base Damage",
                                     12f,
                                     "");
            damageGrowth
                = plugin.Config.Bind("03 - Character Stats",
                                     "Damage Growth",
                                     2.4f,
                                     "");
            baseMovementSpeed
                = plugin.Config.Bind("03 - Character Stats",
                                     "Base Movement Speed",
                                     7f,
                                     "");
            baseCrit
                = plugin.Config.Bind("03 - Character Stats",
                                     "Base Crit",
                                     1f,
                                     "");
            #endregion character stats

            #region riot
            shotgunDamage
                = plugin.Config.Bind("04 - Riot Shotgun 3.1.0",
                                     "Damage Coefficient",
                                     0.45f,
                                     "Damage of each pellet");
            shotgunProcCoefficient
                = plugin.Config.Bind("04 - Riot Shotgun 3.1.0",
                                     "Proc Coefficient",
                                     0.5f,
                                     "Proc Coefficient of each pellet");
            shotgunBulletCount
                = plugin.Config.Bind("04 - Riot Shotgun 3.1.0",
                                     "Bullet Count",
                                     8,
                                     "Amount of pellets fired");
            shotgunRange
                = plugin.Config.Bind("04 - Riot Shotgun 3.1.0",
                                     "Range",
                                     64f,
                                     "Maximum range");
            shotgunSpread
                = plugin.Config.Bind("04 - Riot Shotgun 3.1.0",
                                     "Spread",
                                     5.5f,
                                     "Maximum spread");
            #endregion riot

            #region ssg

            superDamage
                = plugin.Config.Bind("06 - Super Shotgun 3.1.0",
                                     "Damage Coefficient",
                                     0.8f,
                                     "Damage of each pellet");
            superSpread
                = plugin.Config.Bind("06 - Super Shotgun 3.1.0",
                                     "Max Spread",
                                     6f,
                                     "your cheeks");
            superDuration
                = plugin.Config.Bind("06 - Super Shotgun 3.1.0",
                                     "Duration Scale",
                                     1f,
                                     $" Scale the duration of the attack (i.e. attack speed) by this value");
            superBeef
                = plugin.Config.Bind("06 - Super Shotgun 3.1.0",
                                     "beef",
                                     0.4f,
                                     "movement stop while shooting in shield. cannot go lower than 0.2 because I say so");
            #endregion ssg

            #region hmg
            /*rifleDamage
                = plugin.Config.Bind("05 - Assault Rifle",
                                     "Damage Coefficient",
                                     0.85f,
                                     "Damage of each bullet");
            rifleProcCoefficient
                = plugin.Config.Bind("05 - Assault Rifle",
                                     "Proc Coefficient",
                                     0.75f,
                                     "Proc Coefficient of each bullet");
            rifleBaseBulletCount
                = plugin.Config.Bind<int>("05 - Assault Rifle",
                                     "Base Bullet Count",
                                     3,
                                     "Bullets fired with each shot");
            rifleRange
                = plugin.Config.Bind("05 - Assault Rifle",
                                     "Range",
                                     256f,
                                     "Maximum range");
            rifleSpread
                = plugin.Config.Bind("05 - Assault Rifle",
                                     "Spread",
                                     5f,
                                     "Maximum spread");*/
            #endregion hmg

            balancedShieldBash
                = plugin.Config.Bind("07 - Shield Bash",
                                     "Balanced Knockback",
                                     false,
                                     "Applies a cap to knockback so bosses can no longer be thrown around.");
            stupidShieldBash
                = plugin.Config.Bind("07 - Shield Bash",
                                     "Ally Knockback",
                                     true,
                                     "Applies knockback to allies.");
        }
    }
}