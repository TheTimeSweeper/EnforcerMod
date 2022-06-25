using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace EnforcerPlugin
{
    class VRAPICompat
    {
        public static GameObject GetPrimaryMuzzleObject()
        {
            return VRAPI.MotionControls.dominantHand.muzzle.gameObject;
        }

        public static GameObject GetSecondaryMuzzleObject()
        {
            return VRAPI.MotionControls.dominantHand.muzzle.gameObject;
        }

        public static GameObject GetShieldMuzzleObject()
        {
            return VRAPI.MotionControls.nonDominantHand.GetMuzzleByIndex(1).gameObject;
        }

        public static bool IsLocalVRPlayer(CharacterBody body)
        {
            return EnforcerPlugin.EnforcerModPlugin.VRInstalled && body == LocalUserManager.GetFirstLocalUser().cachedBody;
        }
    }
}
