using EntityStates;
using RoR2;
using UnityEngine;

namespace Gnome.EntityStatez
{
    public class SpecialState : BaseSkillState
    {
        public float baseDuration = 0.5f;
        private float duration;
        ShieldComponent sComp;
        public override void OnEnter()
        {
            base.OnEnter();
            sComp = base.characterBody.GetComponent<ShieldComponent>();
            this.duration = this.baseDuration / base.attackSpeedStat;
            if (sComp.isShielding) { this.duration += 1; }
            Ray aimRay = base.GetAimRay();
            base.StartAimMode(aimRay, 2f, false);
            if (base.isAuthority)
            {
                sComp.isShielding = !sComp.isShielding;
                if (sComp.isShielding)
                {
                    base.characterBody.AddBuff(andrewPlugin.jackBootsIndex);
                }
            }
        }
        public override void OnExit()
        {
            if (!sComp.isShielding)
            {
                base.characterBody.RemoveBuff(andrewPlugin.jackBootsIndex);
            }
            base.OnExit();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
    }
}