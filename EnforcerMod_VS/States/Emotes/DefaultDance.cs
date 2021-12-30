﻿using UnityEngine;

namespace EntityStates.Enforcer
{
    public class DefaultDance : BaseEmote
    {
        
        
        public override void OnEnter()
        {
            base.OnEnter();

            PlayFromMemeRig("DefaultDance", EnforcerPlugin.Sounds.DefaultDance);
        }
    }

    public class FLINTLOCKWOOD : BaseEmote
    {
        public override void OnEnter()
        {
            base.OnEnter();

            PlayFromMemeRig("FLINT LOCK WOOD");
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            base.StartAimMode(1, false);
            base.characterMotor.rootMotion = /*base.characterDirection.forward*/GetAimRay().direction * this.moveSpeedStat * base.characterBody.sprintingSpeedMultiplier * Time.fixedDeltaTime;
        }
    }

    public class Rest : BaseEmote
    {
        public override void OnEnter()
        {
            base.OnEnter();

            PlayEmote("RestEmote", "", 1.5f);
        }
    }
}