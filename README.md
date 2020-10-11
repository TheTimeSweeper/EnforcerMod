# Enforcer
- Adds the Enforcer from Risk of Rain 1
- Includes a bunch of unlockable alternate skills and skins
- Now includes support for ClassicItems' Ancient Scepter

[![](https://cdn.discordapp.com/attachments/739704939671978024/764506299857502278/Screenshot_429.png)]()

[![](https://i.imgur.com/lVOcJCY.png)]()
[![](https://i.imgur.com/wVL1Ilk.png)]()

Join the discord server to share any feedback/bugs/suggestions- https://discord.gg/HpQB9fC
Also consider getting the ScrollableLobbyUI mod, this guy has a lot of unlockable skins

## Credits
Gnome - Coding
rob - Cooding
TheTimesweeper - Coooding
Enigma - Cooooding
PapaZach - Skill icons
Jot - Animations
DarkLordLui - Animations
Violet Chaolan - Sounds
LucidInceptor - Modeling
Destructor - Mod icon
Moffein - Idea Guy™
Ruxbieno - Idea Guy™

And big thanks to everyone testing and giving their feedback, the mod wouldn't be the same without it

## Skills

| Skill | | Description | Stats |
|:-|-|------|-|
| Riot Shotgun | ![](https://i.imgur.com/QgTZQqj.png) | Fire a short range `piercing` blast for `8x40%` damage. | Proc: `0.5` |
| Shield Bash | ![](https://i.imgur.com/6iWFhOv.png) | `Bash` nearby enemies away for `250%` damage. Use while sprinting to perform a `Shoulder Bash` for `450%` damage instead. `Deflects Projectiles`. | Proc: `1.0`, CD: `6 sec` |
| Shoulder Bash |  | Short charge that stuns. Hitting heavier enemies deals up to `700%` damage. | Proc: `1.0` |
| Tear Gas | ![](https://i.imgur.com/sb1CzFt.png) | Throw a grenade that explodes into tear gas that leaves enemies `Blinded`. Lasts for 16 seconds. | CD: `24 sec` |
| Blind |  | Reduces `movement speed` by `75%`, `attack speed` by `25`, and `armor` by `20`|
| Protect and Serve | ![](https://i.imgur.com/y7JWEzx.png) | Take a defensive stance `blocking` all damage from the front. `Increases your rate of fire` but prevents sprinting and jumping. | CD: `0 sec` |

### Unlockable Alts (spoiler alert)

| Skill | | Description | Stats |
|:-|-|------|-|
| Super Shotgun | ![](https://i.imgur.com/fJk3Iwn.png) | Fire a powerful short range blast for `16x75%` damage. Has harsh damage falloff. | Proc: `0.75` |
| Assault Rifle | ![](https://i.imgur.com/VV3t6HU.png) | Rapidly fire bullets dealing `65%` damage. | Proc: `0.5`, Proc in shield: `0.4` |
| Stun Grenade | ![](https://i.imgur.com/yuL8mB2.png) | `Stunning`. Launch a stun grenade, dealing `400%` damage. Store up to 3 grenades. | Proc: `0.6`, CD: `8 sec` |

[![Gnome's true dedication](https://i.imgur.com/txUzvAY.png)]()

## Known Issues
- Teleporter particles kinda big (We could fix this but we won't)
- Cancelling shield with sprint does not work properly in multiplayer
- Pending an R2API update , some achievements can't be unlocked in multiplayer. (they only work for host or single player).
- Bungus skin has some weird head clipping
- Mustard Gas doesn't apply the debuff

## Future Plans
- Skills++support
- More polish and skills maybe

## Changelog
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

if you read all this you're cool