`3.11.3`
- fixed for latest game update

`3.11.2`
- fixed achievements, adding lunar coin rewards
- removed debug code including cycling skins on G
- separated english from the language files so it is updated with configs (untested)

`3.11.1`
- fixed skin-specific goodies not working
- removed fixinvinciblemithrix dependency

`3.11.0`
- fixed nemforcer secondary slam taking fall damage
- added "FixInvincibleMithrix" as a hard dependency until either gearbox fixes chronic expansion or we add an item display
- fixed missing VFX
- added Spanish (Spain) translation (thanks Bagre!)

`3.10.0`
- sots fix
- added slight hop after heat crash that should maybe combat falling through the floor when grabbing bosses

`3.9.0`
- Survariants Compat
- heavy tf2
	- Now uses standard health regen. 0.5 (+0.25) -> 1.0 (+0.2)
	- Fixed Shuriken not working with primaries.
	- Added Config:
		- Passive Rework (Beta) (Default: False)
			- Melee hits regenerate 2% HP over 3s. Can stack.
			
			*Idea is to make this more of an active part of his kit, but I'm not sure how I feel about it. Feedback would be appreciated.*
			
		- Nerf Stats (Default: False)
			- Reduces HP from 224 (+56) -> 160 (+48)
			
			*Stats are the same as Enforcer, aside from Nemforcer having 20 armor instead of 12.*
			
		- Heat Crash - Allow Bosses (Default: True)

`3.8.4`
- fixed CN and BR translations breaking the universe

`3.8.3`

- Fixed Nemesis Invasion void fields rounds cleared being hardcoded.
- Added CN TL (Thanks Meteorite1014!)
- Added PT-BR TL (Thanks Kauzok!)

`3.8.2`  
- updated fr translation proc coefficients (google translated)

`3.8.1`  
- added Russian translation (thanks Lecarde!)
- added Japanesse translation (thanks Punch!)
- added betterUI proc coefficients to translations
- adjusted some textures for lower texture scaling (if you want simpson enforcer back I'll put it back)

`3.8.0`  
been a year since we updated this fucker. let me know if anything broke.  
- Added Language Support. see Readme for translations
- Added new skin in femforcer config
- Added a lunge to shield bash if inputting forward
- Fixed nemforcer royal capacitor missing display

`3.7.4`

- Nemforcer can now be "head"shotted by Railgunner.

`3.7.3`
- updated for r2api split ass(emblies)
- nothing else changed. you can revert if you're still on old r2api

`3.7.2`
 - added config for HMG screenshake
 - fixed some networking stuff I think I broke last update

`3.7.1`
 - blacklisted blind vermin from melee blocking code
 - separated assetbundles and soundbank from dll
   - *let me know if this breaks anything*
 - fix grandmastery missing hand

`3.7.0`
 - added logic for blocking melee attacks based on character's core position being in front of you
 - this makes blocking beetles, gups, and other melee enemies very consistent
 - some enemies (beetle guards, mithrix, worms) are blacklisted from this logic
   - *A. Some large attacks still feel like they should get past the shield (beetle guard, mithrix)*
   - *B. The code checks character body's position, and worm's center position is very desynced from where it looks like it is*
   - *if you'd like to add your melee enemy to this blacklist, add their body name to EnforcerModPlugin.GuaranteedBlockBlacklistBodyNames list*

`3.6.1`
 - fixed eclipse not saving progress

`3.6.0` Anniversary Update  
Holy shit it's been two years.
 - readded ALL the old skins
   - Desperado
   - Doom
   - Engi
   - Stormtrooper
   - yes, the fucking frog too
   - disable them in the "I hate fun" config
   - celebrate by starting a new profile and unlocking them all!
 - readded their achievements and networked them
   - skins are applied when the achievement is achieved for the first time
 - added sotv item displays
 - updated minecraft skin gun
 - fixed shell ejection port on default gun pointing towards enforcer's face
 - fixed breaching hammer animations
 - readded heavy tf2 dominance achievement

`3.5.0` holy shit VR  
Huge thanks to Dr.Bibop and PureDark for *full* VR implementation
 - custom VR hands with custom models based on each skin
 - motion controls for melee skills
 - two-handed aiming for minigun
 - read more above

Additional:
 - updated heavy tf2's grenades with enforcer's changes from previous patches
 - updated nemforcer unlock condition in the logbook to be more clear
 - fixed enforcer and nemforcer with sawmerang
 - automatically disabled sprint shield cancel if autosprint is installed (been about time, truthfully)
 
`3.4.1`
- fixed guns breaking with custom skins
- fixed missing enforcer in logbook

`3.4.0`
- Riot Shotgun
    - bullet count reduced 8->6
    - bullet damage increased 45%-60%

  *math on total damage remains the same, but procs were way too crazy and have been brought in line a bit*
    
- Tear Gas
    - Increased stun radius from 6m -> 8m
    - Reduced uptime from 18s -> 12s
    - Reduced cooldown from 24s -> 16s
    - Now changes damage number color to orange to better communicate the armor reduction.
    - Reduced bounce time after impact

  *having a 20+ second cooldown on the move before you could reposition it was making it way too inflexible. basically only seeing use in the teleporter. new values make it more active and flexible to use, as well as some polish*

- Shield Bash
    - Reduced knockback 20%.
    - logic improved, beetle queens and gups can be pushed now
    - Now has a minimum vertical knockback amount, so aiming downwards will still knock enemies back.
    - Balanced Knockback
        - Enabled balanced knockback by default.
        - Changed how balanced knockback works.
            - Old Behavior: Caps mass scaling at 500.
            - New Behavior: Simply applies a 0.7x force multiplier against Champion enemies. 
    
  *should feel a lot more consistent and fair*

- Super Shotgun
    - Increased fire rate from 0.85s/shot -> 0.3125s/shot
    - Reduced shield lock time from 0.6s -> 0.3125s
    - Removed fire rate config option due to code jank.
    
 - Removed HP regen on Nemforcer Boss
 - added inferno compat for grandmastery skins
 - readded minecraft skin under cursed config
 - added redone femforcer skin under femforcer config (*thanks anon*)

`3.3.9`
 - moved unlockable code to r2api, fixing achievement issue with recent update
 - hammer animation tweaks

`3.3.8`
 - fixed dedede bug with r2api update
 - fixed sawmerang bug
 - some texture optimization

`3.3.7`
 - added a few void item displays
 - added proc coefficients to betterUI's skill tooltips
 - removed physical collider from the shield. it was fun and maybe we'll revisit it again.
 - tweaked/fixed animation for transition to aiming minigun

`3.3.6`
 - fixed a weird slowing issue introduced in the last patch

`3.3.5`
 - tweaked physical shield from stopping deflecting golem lasers
   - as far as jank goes this is the least that I expected
 - tweaked shield in/out animation to not snap jarringly

`3.3.4`
 - added an actual physical collider in front of the shield to stop enemies from walking through
   - *definitely let us know if this causes any jank*
 - tweaked shield hurtbox a bit
 - fixed stun grenade and other projectiles being eaten by shield
 - these changes make shield a little more reliable, but you still must reposition when you're overrun
 - fixed gas grenade being thrown from an odd position
 - more squashing the camera bug. 
   - let me know if it's survived like a fuckin cockroach still

`3.3.3`
 - removed big thing thing was there by accident
 - I mean nothing

`3.3.2`
 - Made grandmastery skins achievable in Eclipse (thanks moff)
 - fixed buffs having missing icons (last update, forgot to mention)

`3.3.1`
 - Re-Added two skins with achievements: 
   - Grandmastery Achievement: Unlocked by beating the game on typhoon or higher (use UntitledDifficultyMod or MoreDifficulties for that)
     - huge thanks to Dotflare for the texture work, and of course Bruh for the model
   - Clearance Achievement: Clear Void Fields at any stage
 - Added unlock achievement for classic skin. press 3
 - Fixed buffs having missing icons

`3.2.13`
 - Fixed shuriken and other things hitting shield
 - readded item displays, might still need fixing, doesn't include sotv content
 - fixed camera getting stuck in shield/minigun POV
 - potential fix for void dios bug
 - fixed achievements not properly added for skills

`3.2.12`
 - Fixed some sounds being too ear-bleeding loud. 
 - definitely let me know if i've overcorrected and they're too quiet now
 - man fuck sounds

`3.2.11`
 - Fixed global hmg sound

`3.2.10`
 - GUPdate fix
 - camera updated. no longer shaking on emotes holy shit
 - item displays removed for now
 - tweaks to shield model
 
`3.2.9`
 - re-done emotes added, incomplete. enjoy default dance and earl run (3 and 4) for one week before cum2
 - VR implementation, also incomplete. In progress, but it's not breaking anything so it can go out
   - proper implementation not done at all. Everything's aimed with right hand, and going in shield stance messes with a few things, 
   - huge thanks to drbibop for getting this going
 - added cross compat with Risky Artifacts for King Dedede if he's enabled

`3.2.8`
 - woops missed a few, how embarrassing
 - fixed crazy ragdoll

`3.2.7`
 - touched up intrusive item displays
   - except crowdfunder. got something special for that
 - fixed crazy ragdoll

`3.2.6`
 - tweaked shield hurtbox and added additional shield hurtbox closer to enforcer 
   - make blocking more consistent, but still less bullshit than before
 - item displays holy shit! big thanks wetpudding

`3.2.5`
 - fixed some item displays, what little I had time to do
 - featuring actually working goat hoof!
 - last small update I promise. I had my fun

`3.2.4`
   - Ok actually made hopoo feather display smaller this time.
   - fix bug with heat crash spending all your stacks
   - Adjusted enforcer's shield hurtbox.
     - it was way too big. Would basically block everything in a 180 degree radius and then some, making it very easy to back against a wall and block everything
     - this gives room for you to get hit on the sides. you must now be more mindful of positioning and repositioning
     - tl;dr don't be a bussy
   - fixed all weapons showing in logbook
   - fixed css audio
   - removed an achievement that wasn't supposed to be there yet don't tell nobody

`3.2.3`
   - Adjusted Hopoo Feather display. Let me know if you like it and especially if you don't.

`3.2.2`
   - woops king dedede gone

`3.2.1`
   - added to previous changelog I forgot to say
   - fixed networking on n4cr achievement
   - readded stun grenade achievement. forgot that one in 3.1
   - fixed masteries grandmasteries being implemented in the most retarded way
     - thanks neb for the prototype
     - grandmastery skin(s) no longer require starstorm after unlocked
   - spent way too much time figuring out the [visions of heresy thing]
   - made shield go away with essence of heresy
   - added some footsteps
   - fixed visuals on heavy tf2's minigun m2 exploding projectiles
   - fixed king dedede boss ragdoll
   - added placeholder item displays. They are very jank
     - if anyone wants to do the item displays that'd be lovely
   
`3.2.0`
 - <ins>Slowly but surely he's coming back</ins>
   - Rig has been fixed and finalized, still a long road but we're on our way
     - Readded true Mastery skin: ~~Sexforcer~~ Peacekeeper on new rig
       - Mastery Achievement reset to celebrate!
     - N4CR skin moved to its own achievement: We Have the Technology
     - Added classic skin on new rig
   - Animations on new rig are a mix of 
     - new and redone, old ones updated, old ones salvaged to work but janky
   - Readded Gun models for each m1 weapon (using classic guns for now)
     - Guns also appear in css when switching
   - Readded cursed config (wip) skills: Breaching Hammer and Skateboard 
     - (yes, rob added a fucking skateboard before and it's back)
   - Readded a few item displays
     - Remade framework for gun-unique item displays
     - someone do the rest pls
     - New Aetherium item displays have been added by Komrade on his end. Say thank you!
 - <ins>Other stuff</ins>
   - after a year, deflecting golem lasers is now networked
     - redid the logic a bit, can't tell if it's more or less janky than before
     - added absolutely disgusting hacky code to deflect lasers from stone wisps in nebby's monster variants
   - made machine gun scale past framerate (did you know fire rate maxed at 50 syringes?)
     - also made minigun scale past framerate (did you know fire rate maxed at 20 syringes?)
   - after like two fuckin years, fixed the animator pausing terribly when stopping while shooting 
 - <ins>On this episode of I had much more important shit to do but I did this instead</ins>
   - skillsplusplus (default m1 only for now)
   - added gordo projectile replacement to dedede skin
   - fixed head size config

`3.1.2`
 - fixed force unlock not working for bebbys
 - fixed multiplayer sound bug with HMG

`3.1.1`
 - fixed double lobby bug with ScrollableLobbyUI and AbyssalLobby

`3.1.0`
- Unlockables fixed!
  - Character unlock, mastery, alternate skills.
  - Actually networked, fucking after over a year.
- Assault Rifle reworked. Now Heavy Machine Gun, Moffein's parting gift.
  - Heavy continuous fire. Rather than burst fire.
  - In shield, Attack speed increased slightly, spread reduced, movement speed slowed.
  - configs removed for now.
  - friendship ended with ar.
  - rip ar 2020-1974.
- Super Shotgun rework.
  - Unshielded now shoots two individual barrel shots, at relatively higher attack speed and lower spread.
  - Shielded shoots both beefy barrels at once.
  - Horrible falloff reduced, spread widened (snuck in last update hee hee hee).
  - Spread patterns adjusted so more bullets are consistently landed in the middle of the crosshair.
  - Added configs, fuck configs and fuck me for doing them.
- Riot Shotgun's bullets as well have been tweaked for feel.
- New sounds for all guns.
- Stun Grenade buffed. 
  - Apparently Tear Gas was outclassing it even in damage cause of its armor reduction.
  - Damage increased to 540%.
  - Cooldown reduced to 6s.
  - Proc coefficient increased to 1. Why wasn't it 1 wtf?
- Aim origin for guns moved closer to crosshair for more consistent landing.
- Shock grenade effect changed.
- Fixed first-time css bug.
- sneed skin removed. fuck you sneed you should have never stayed

`3.0.5`
- Hotfix update so it doesn't destroy the world with the new aetherium update
- Enjoy incredibly WIP probably broken animations (run, shoot, shoulder bash, css), by Paysus (THE Paysus)
- AR bug fixed
- Shock grenade fixed

`3.0.4`
- Uploaded the proper dll this time

`3.0.3`
- Properly timed explosion on Dominance(Minigun) to when the hammer slams down
- Lowered hitpause with more enemies hit (so you don't get stuck in Grovetenders anymore)
- Added standing still animation for legs
- Made Heavy TF2's hands properly hold the hammer on charging animations (finally)
- Tweaks to uppercut animations
- Made Heat Crash the default utility
- Lowered Heavy TF2 boss total regen by 20%
- Fixed Heavy TF2's item displays
- Fixed Heavy TF2's weird unintended cooldown quirks
- Fixed damaging gas grenades not dealing damage
- Fixed Ultra Heavy TF2 boss not working, moved spawn position to somewhere reasonable
- Fixed King Dedede boss not spawning
- Fixed Starstorm 2 compatibility- now drops a Titanic Knurl rather than a Genesis Loop(it just makes sense)

`3.0.2`
- Hotfix update for the hotfix update

`3.0.1`
- Updated mod icon and fixed manifest version

`3.0.0`
- Fixed to work on the anniversary update
- Rebuilt entire model + animations from the ground up, courtesy of bruh- not 100% finished yet
- All skins removed for now
- All skill unlocks removed for now
- Alternate gun models + animations removed for now
- Item displays removed for now
- Skateboard removed for now
- Various config options removed
- Fortnite dances removed

`2.2.6`
- King Dedede's bonus health regen is now disabled upon taking damage, and bleed now cuts ALL regen by 100% (only applies to Starstorm 2 invasion variant)

`2.2.5`
- Added compatibility with Standalone Ancient Scepter and removed ClassicItems compatibility

`2.2.4`
- Fixed a minor issue with skins

`2.2.3`
- Added compatibility for Starstorm 2's void event- the default invasion event is still here, only now with a twist
- Added Enforcer's Grand Mastery skin- huge thanks to bruh for the model
- Added work in progress alt utility for Heavy TF2, locked behind config

`2.2.2`
- Updated skill icons
- Networked shield block effect
- Added item displays for Plague Mask and Plague Hat from Supply Drop
- Heavy TF2 changes below
- Added Grand Mastery skin, available via Starstorm 2 crosscompat
- Added new minigun sounds
- Updated a lot of VFX
- Updated uppercut animation
- Updated skin icons
- Tweaked camera angle on spawn
- Tweaked hammer hitstop to stop it from interfering with movement
- Networked salute emote, oops
- Networked melee hit sounds

`2.2.1`
- Unsneeded the feed

`2.2.0`
- Added a new skin for Heavy TF2
- Updated hammer sounds

`2.1.9`
- Fixed salute emote doing nothing

`2.1.8`
- Fixed Dominance breaking when used on dedicated servers

`2.1.7`
- Fixed Fortnite dances (sorry)

`2.1.6`
- Added salute emote
- Tweaked sprint animation timing
- Tweaked uppercut animation
- Added work in progress alt primary (enable cursed in config)
- Removed all reflection from code, improving performance
- Fixed SSG reload animation
- Fixed some stupid vanilla EffectCatalog errors
- Fixed Red Whip activating while using certain primaries

`2.1.5`
- Quick hotfix, King Dedede's skills were replacing Commando's by mistake

`2.1.4`
- Added a custom King Dedede boss, must be enabled via config- spawns on Sky Meadow and post-loop Titanic Plains
- Added a new skill variant for Heavy TF2, must be enabled via cursed config
- Fixed Heavy TF2's spawn animation
- Fixed Heavy TF2's umbra not having proper AI
- Increased size of secret boss
- Added passive regen VFX
- Tweaked Minecraft skin and improved rig

`2.1.3`
- Updated King Dedede's Minecraft skin- special thanks to SalvadorBunny for making it!
- Made Breaching Hammer an actual skill (not sure if it's still locked behind config to be honest)
- Improved Stun Grenade aim
- Tweaked King Dedede hammer hitboxes so they can be more reliably blocked by the shield
- King Dedede is now immune to the void
- Improved Golden Hammer swing animations
- Golden Minigun now aims up and down
- Added Enforcer idlein animation
- Toggling sirens now toggles an alternate run animation because letting this feature sit around unused was depressing
- Fixed Shattering Justice not properly replacing the hammer

`2.1.2`
- Optimized all gases, should no longer kill fps after using too many on a stage
- Fixed rest emote not being looped

`2.1.1`
- Fixed an issue causing the game to softlock after Mithrix's defeat
- Fixed a minor camera bug
- Fixed Dedede's rig
- Fixed big teleporter particles
- Updated shield model
- Added an experimental Heavy TF2 skin, still a heavy work in progress

`2.1.0`
- Partially reverted some camera stuff to fix bugs resulting from it- smoother camera now only works in singleplayer

`2.0.9`
- Fixed stuff

`2.0.7`
- Adjusted some camera things, improving overall smoothness
- Minor optimization to shotgun shells
- King Dedede model tweaks- Improved rig, 200% less jank squish, added minigun
- Tweaked Heavy TF2 spawn animation to somewhat more consistently face the camera

`2.0.6`
- Fixed Aetherium item displays
- Lots of King Dedede changes
- No longer requires Enforcer to unlock- meaning someone else can play him and get the unlock for everyone
- Added fall damage immunity and adaptive armor to boss variant
- Capped maximum spawns per invasion to 1(can revert in config if you like the challenge)
- Added a new skin(Must be enabled via cursed config(I sincerely apologize))
- Added Supply Drop item displays
- Tweaked secondary visuals
- Fixed uppercut not playing sounds in multiplayer
- Increased Dominance (Minigun) base duration from 0.9s to 1.2s
- Dominance (Minigun) now explodes projectiles, rather than simply destroying them

`2.0.5`
- Alright the uppercut animation is ACTUALLY 100% fixed now, if it's not we will deprecate the mod
- Some more animation and VFX tweaks
- Minigun variant of Dominance is now shown in loadout
- Modded difficulties above Monsoon are now counted for Monsoon exclusive content
- Fixed Dripforcer clipping issues
- Gave Enforcer's umbra proper AI
- Changed Herobrine's unlock text to something more accurate

`2.0.4`
- Fixed multiplayer animations for real this time
- Fixed minigun crosshair not working properly for spectators
- Fixed certain effects being duplicated in multiplayer, causing excessive screenshake
- Fixed Little Disciple and Leech item displays
- Lowered minigun secondary damage to from 1000% to 600%- Backup Mags were a little excessive
- Lowered aerial slam minimum radius to 6m from 12m, max unchanged

`2.0.3`
- Added Supply Drop item displays(missed change from 2.0)
- Added config for shotgun shell sounds
- Adjusted Leech placement
- Fixed Enforcer always being elevated slightly above the ground
- Heavy TF2 changes- goal is to make him more fluid and address his low durability
- Boss variant now uses unique boss scaling, should no longer be a pushover
- Added config to enable invasions as any character, for fun
- Various animation tweaks
- Base max health increased from 160 to 224
- Max health growth increased from 48 to 56
- Passive regen increased from 2% to 2.5%
- Lowered primary base duration from 1.2s to 1.05s, tweaked animation
- Fixed duplicate effects on primary
- Fixed bugged secondary animation in multiplayer
- Secondary slam now only activates when falling and looking down
- Secondary slam now carries over speed from the charge state when calculating impact radius
- Uppercut cancel window now begins at the start of the uppercut rather than the end
- Minigun spread now ramps up more slowly
- Minigun self slow reduced
- Minigun swap no longer prevents jumping, shorter slow duration when sheathing minigun
- Minigun stance armor buff increased from 50 to 60
- Fixed Happiest Mask placement
- Added drip
- Life Savings still does nothing
- I don't even know if the problem is on our end nothing makes sense
- Just don't grab the item

`2.0.2`
- Made Enforcer: Enforcing Perfection require much less Bustling Fungus to unlock
- Fixed some effects being duplicated in multiplayer
- Tweaked Herobrine spawn mechanics

`2.0.1`
- Spotted another readme typo, hopefully last one for today

`2.0.0`
- Fixed a typo in readme

`1.1.5`
- Updated character portrait
- Updated Shield Bash Icon
- Shield Bash now pushes allies(toggleable via config)
- Fixed a minor bug

`1.1.4`
- Fixed absurd Stun Grenade knockback

`1.1.3`
- Updated skill icons
- Added a new skin, moved some old skins to a config option to prevent bloat
- Updated shotgun shells, added sounds
- Added fancy shoulder light effect- made it flash on level up too because I'm a shameless showoff
- Buff Riot Shotgun damage to 8x45% and SSG to 16x80%- resetting config recommended
- Riot Shotgun's spread is now tightened during Protect and Serve
- Increased Protect and Serve self slow from 50% to 65%
- Shield Bash now has minimum knockback, so light monsters like wisps and jellyfish can now get launched
- Added a config option to add a cap to the knockback to prevent bosses from being thrown around
- Tear Gas no longer snaps to the ground, meaning it's effective on flying monsters
- Actually fixed Mustard Gas not applying debuff for real this time
- Mustard Gas damage doubled, proc coefficient raised from 0 to 0.05
- Stun Grenade visuals updated to better match his RoR1 grenade
- Added knockback to Stun Grenade
- Added item displays for the new Aetherium items and SivsItems
- Added pig
- Fixed Enforcer's logbook display using the wrong shader
- Tweaked shotgun volume

`1.1.2`
- Reworked Assault Rifle- resetting config is recommended
- Even more model tweaks, fixed Bungus head clipping
- Added back missing shoulder lights
- Added new skin
- Loadout choices are now visible in character select
- Added SSG bullet tracer
- Added sounds for the Bungus guns
- Fixed Mustard Gas not applying debuff
- Fixed Fresh Meat being unreasonably huge
- Shield Bash into P&S transition made smoother
- Tweaked Femforcer skin
- Added skateboard sounds and fixed some bugs
- Unfinished Breaching Hammer is now an actual melee attack

`1.1.1`
- Tweaked model some more
- Fixed item displays not showing up on the Assault Rifle
- Added skateboard model and animations- must be enabled in config to use it

`1.1.0`
- Updated model

`1.0.9`
- Added Femforcer- disabled by default, must be enabled via config (thanks modanon!)
- Added more bungus weapons
- Buffed Assault Rifle damage to 80%- config must be reset or edited manually to get the new value
- Buffed Assault Rifle proc coefficient during P&S to 0.4
- Buffed Assault Rifle fire rate
- Lowered Assault Rifle spread
- Added configuration for Assault Rifle
- Added config option to revert the Engi shield
- Added a new emote
- Fixed some animation weirdness
- Fixed Fresh Meat being huge

`1.0.8`
- Added gun shooting animations
- Tweaked sprint animation
- Tweaked aim animation
- Added item displays for items from the Aetherium mod
- Added support for Ancient Scepter!
- Tear Gas > Mustard Gas: Gas now deals 100% damage per second
- Stun Grenade > Shock Grenade: Grenade does more damage, has a larger radius and applies shock rather than stun
- Half reverted Engi skin's shield
- Fixed Sawmerang item display
- Fixed weird shield overlay on certain skins

`1.0.7`
- Fixed another minor bug

`1.0.6`
- Fixed a null projectile being registered to the catalog possibly causing bugs

`1.0.5`
- Updated Engineer skin's shield
- Updated Engineer skin's shotgun- only Riot Shotgun for now, other weapons coming soon
- Updated Doomguy skin's shield
- Updated Needler texture
- Lowered the amount of Bustling Fungus needed for Enforcer: Enforcing Perfection even more
- Fixed Shattering Justice placement- left the old hammer placement as a config option
- Fixed deflected projectiles sometimes not hitting the original owner of the projectile
- Fixed Needler not dropping when ragdolling
- Fixed Crowdfunder hitting the shield during Protect & Serve- the drone wasn't actually needed but it's staying
- Updated Super Massive Leech item display
- Added a config option to enable unfinished skills
- Changed config to use proper keycodes- this will reset your keybinds back to default but makes changing them easier, resetting config is recommended
- Fixed a typo in Shield Bash's description

`1.0.4`
- Increased Assault Rifle damage to 65%; 40% was a bug and unintended, sorry!
- Added animation when using the Needler
- Added a custom crosshair for Visions of Heresy- it's applied to every survivor but this can be toggled off via config
- Due to complaints, Shattering Justice has been placed somewhere more serious
- Lowered amount of Bustling Fungus required for Enforcer: Enforcing Perfection from 250 to 200
- Fixed Riot Shotgun firing no bullets when configured to fire only one
- Fixed an issue with Needler model still being visible even after getting rid of the item
- Fixed Backup Mags on the Needler being huge
- Fixed Visions of Heresy being hidden inside the head
- Fixed sirens not going off when deflecting golem lasers

`1.0.3`
- Nerfed Super Shotgun fire rate; it was actually bugged and was never supposed to have the same fire rate as Riot Shotgun
- Fixed bug with Super Shotgun ejecting a stupid amount of shells, added unique shells
- Added a Needler gun for Visions of Heresy
- Calm idle stance now only triggers when out of combat
- Fixed bug causing shield to stop blocking damage if sirens were toggled during Protect and Serve
- Fixed the annoying (but harmless) warning messages on startup
- Renamed 'Blinded' debuff to 'Impaired'
- Added the host only warning to the Rules of Nature achievement since that one seems to be bugged too
- Some minor texture tweaks
- Fixed a rare bug involving a pink cube and Enforcer flying into space? Never actually found the cause but that cube is deleted so it should be gone

`1.0.2`
- Added keybinds to config
- Added a custom Crowdfunder Drone display, to try and fix the Crowdfunder bug(it didn't)
- Reworked Super Shotgun
- Added config for base stats and Riot Shotgun, more might come eventually but this is really tedious to code
- Enforcing Perfection achievement now scales better with the amount of Bustling Fungus you have

`1.0.1`
- Added dependencies oops

`1.0.0`
- Initial release