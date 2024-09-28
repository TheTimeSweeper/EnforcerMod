# Enforcer
- Adds the Enforcer from Risk of Rain 1
- Includes a bunch of unlockable alternate skills and skins
- Includes support for Ancient Scepter, VR, and a few other mods

![](https://raw.githubusercontent.com/TheTimeSweeper/EnforcerMod/master/Release/readme/screen0.png)

![](https://raw.githubusercontent.com/TheTimeSweeper/EnforcerMod/master/Release/readme/screen1.png)
![](https://raw.githubusercontent.com/TheTimeSweeper/EnforcerMod/master/Release/readme/screen2.png)

![](https://raw.githubusercontent.com/TheTimeSweeper/EnforcerMod/8817c519fd461e0afbb8920bc6a5d6c40a0dbc40/EnforcerMod_Unity/Enforcer/Assets/Enforcer/Enforcer/Icons/texEnforcerIcon.png)

Join the discord server to share any feedback/bugs/suggestions- https://discord.gg/HpQB9fC  
Or ping/message me directly (`thetimesweeper`) with all the nastiest feedback you can think of

___

## Credits
Gnome - Coding  
rob - Cooding  
TheTimesweeper - Coooding  
Enigma - Cooooding  
Moffein - Idea Guy™, Coooooding  
PapaZach - Skill icons  
Jot - Animations  
DarkLordLui (JestAnAnimator) - Animations  
Violet Chaolan - Sounds  
LucidInceptor (2cute2game) - Modeling  
bruh - Modeling, swag, animation  
Paysus - Animations  
Dr.Bibop - VR implementation  
PureDark - VR implementation  
Dotflare - texture assistance  
SalvadorBunny - Heavy TF2 Minecraft skin  
Destructor - Original mod icon  
Reithierion - New mod icon  
Draymarc - Concepts  
Lethan - Screenshots  
wetpudding - item displays  
Ruxbieno - Idea Guy™  
Swuff - Idea Guy™  

And big thanks to everyone testing and giving their feedback, the mod wouldn't be the same without it

## Languages
If you'd like to translate to your langauge, check out the [language folder on github](https://github.com/TheTimeSweeper/EnforcerMod/tree/master/Release/plugins/Language).  
You can submit a pull request or issue with your language files, or can send it directly to `thetimesweeper` on discord  
Thanks to those that have and in advance to those that may.

French - StyleMyk  
Chinese - Meteorite1014  
Brazilian Portuguese - Kauzok  
Russian - Lecarde  
Japanese - punch  
Spanish (Spain) - Bagre
Spanish (Mexico) - same as Spain
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

![Gnome's true dedication](https://i.imgur.com/txUzvAY.png)

___

## VR 
Enforcer is now compatible with [Dr.Bibop's VRMod](https://thunderstore.io/package/DrBibop/VRMod/). make sure you have [VRAPI installed](https://thunderstore.io/package/DrBibop/VRAPI/).  
Huge thanks to Dr.Bibop and PureDark for the *full* VR implementation  
![](https://raw.githubusercontent.com/TheTimeSweeper/EnforcerMod/master/Release/readme/enforvr.png)

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
  - perhaps new rorr skills
    - if anyone would like to pr any of those (or anything you want) to the repo you're free to do so
- Skills++ support
- animation pass
- mention heavy tf2
- Other things 100% decided on pure whimsy
- ~~Readding everything~~
- Fixing this fucker
- Help

___