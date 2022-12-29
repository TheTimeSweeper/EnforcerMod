# Enforcer
- Adds the Enforcer from Risk of Rain 1
- Includes a bunch of unlockable alternate skills and skins
- Includes support for Ancient Scepter, VR, and a few other mods

[![](https://raw.githubusercontent.com/TheTimeSweeper/EnforcerMod/master/Release/readme/screen0.png)]()

[![](https://raw.githubusercontent.com/TheTimeSweeper/EnforcerMod/master/Release/readme/screen1.png)]()
[![](https://raw.githubusercontent.com/TheTimeSweeper/EnforcerMod/master/Release/readme/screen2.png)]()

[![](https://cdn.discordapp.com/attachments/739696016755785859/828394816986808360/EnfuckerIcon.png)]()

Join the discord server to share any feedback/bugs/suggestions- https://discord.gg/HpQB9fC
Or ping/message me directly (TheTimesweeper#5727) with all the nastiest feedback you can think of

___

## Credits
Gnome - Coding  
rob - Cooding  
TheTimesweeper - Coooding  
Enigma - Cooooding  
Moffein - Idea Guy™, Coooooding  
PapaZach - Skill icons  
bruh - Modeling, swag, animation  
Jot - Animations  
DarkLordLui (JestAnotherAnimator) - Animations  
Paysus - Animations  
Violet Chaolan - Sounds  
LucidInceptor (2cute2game) - Modeling  
Dr.Bibop - VR implementation  
PureDark - VR implementation  
Dotflare - texture assistance  
SalvadorBunny - Heavy TF2 Minecraft skin  
Destructor - Original mod icon  
Reithierion - New mod icon  
Nebby - Help  
Draymarc - Concepts  
Lethan - Screenshots  
wetpudding - item displays  
Ruxbieno - Idea Guy™  
Swuff - Idea Guy™  

And big thanks to everyone testing and giving their feedback, the mod wouldn't be the same without it

___

## Skills

| Skill | | Description | Stats |
|:-|-|-------|-|
| Riot Shotgun | ![](https://raw.githubusercontent.com/TheTimeSweeper/EnforcerMod/master/EnforcerMod_Unity/Enforcer/Assets/Enforcer/Enforcer/Icons/Skills/RiotShotgunIcon.png) | Fire a short range `piercing` blast for `6x60%` damage. | Proc: `0.5` |
| Shield Bash | ![](https://raw.githubusercontent.com/TheTimeSweeper/EnforcerMod/master/EnforcerMod_Unity/Enforcer/Assets/Enforcer/Enforcer/Icons/Skills/ShieldBashIcon.png) | `Bash` nearby enemies away for `250%` damage. Use while sprinting to perform a `Shoulder Bash` for `450%` damage instead. `Deflects Projectiles`. | Proc: `1.0`, CD: `6 sec` |
| Shoulder Bash |  | Short charge that stuns. Hitting heavier enemies deals up to `700%` damage. | Proc: `1.0` |
| Tear Gas | ![](https://raw.githubusercontent.com/TheTimeSweeper/EnforcerMod/master/EnforcerMod_Unity/Enforcer/Assets/Enforcer/Enforcer/Icons/Skills/TearGasIcon.png) | Throw a grenade that explodes into tear gas that leaves enemies `Impaired`. Lasts for 12 seconds. | CD: `16 sec` |
| Impaired |  | Reduces `movement speed` by `75%`, `attack speed` by `25`, and `armor` by `20`|
| Protect and Serve | ![](https://raw.githubusercontent.com/TheTimeSweeper/EnforcerMod/master/EnforcerMod_Unity/Enforcer/Assets/Enforcer/Enforcer/Icons/Skills/ShieldUpIcon.png) | Take a defensive stance `blocking` all damage from the front. `Increases your rate of fire` but prevents sprinting and jumping. | CD: `0 sec` |

### Unlockable Alts (spoiler alert)

| Skill | | Description | Stats |
|:-|-|-------|-|
| Super Shotgun | ![](https://raw.githubusercontent.com/TheTimeSweeper/EnforcerMod/master/EnforcerMod_Unity/Enforcer/Assets/Enforcer/Enforcer/Icons/Skills/SuperShotgunIcon.png) | Fire up to 2 shotgun blasts for `8x80%` damage. While using `Protect and Serve`, fire both barrels at once. | Proc: `0.75` |
| Heavy Machine Gun | ![](https://raw.githubusercontent.com/TheTimeSweeper/EnforcerMod/master/EnforcerMod_Unity/Enforcer/Assets/Enforcer/Enforcer/Icons/Skills/AssaultRifleIcon.png) | Unload a barrage of bullets for `130%` damage. While using `Protect and Serve` has increased accuracy, but slower movement speed. | Proc: `1` |
| Stun Grenade | ![](https://raw.githubusercontent.com/TheTimeSweeper/EnforcerMod/master/EnforcerMod_Unity/Enforcer/Assets/Enforcer/Enforcer/Icons/Skills/StunGrenadeIcon.png) | `Stunning`. Launch a stun grenade, dealing `540%` damage. Store up to 3 grenades. | Proc: `1`, CD: `7 sec` |

[![Gnome's true dedication](https://i.imgur.com/txUzvAY.png)]()

___

## VR 
Enforcer is now compatible with [Dr.Bibop's VRMod](https://thunderstore.io/package/DrBibop/VRMod/). make sure you have [VRAPI installed](https://thunderstore.io/package/DrBibop/VRAPI/).  
Huge thanks to Dr.Bibop and PureDark for the *full* VR implementation  
[![](https://raw.githubusercontent.com/TheTimeSweeper/EnforcerMod/master/Release/readme/enforvr.png)]()

#### Guns
 - aimed with dominant hand
#### Shield Bash
 - can be activated by swinging shield-hand. 
 - knocks enemies back based on your look direction
 - sprint shield bash also faces your look direction
#### Protect and Serve
 - can be automatically activated by raising and lowering your shield
   - raising your shield is triggered by having your shield-hand in front of you and raised
   - lowering your shield is triggered when you point your shield-hand downwards
   - can be disabled in config

___

#### Hammer
 - held with both hands
 - activated by swinging with motion controls
#### Hammer uppercut
 - aimed with dominant hand
#### Minigun
 - held and aimed with both hands

___

## Known Issues 
- Cancelling shield with sprint does not work properly in multiplayer

___

## Future Plans
- More alternate skill(s)
- Skills++ support
- animation pass
- mention heavy tf2
- Other things 100% decided on pure whimsy
- ~~Readding everything~~
- Fixing this fucker
- Help

___

## Changelog

`3.7.4`

- Nemforcer can now be headshotted by Railgunner.

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

`2.X.X`
- for previous changelogs see legacy readme on github