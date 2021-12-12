using RoR2;
using UnityEngine;

namespace EntityStates.Enforcer
{
    public class SirenToggle : BaseState
    {
        private EnforcerLightController sirenComponent;

        public override void OnEnter()
        {
            base.OnEnter();

            this.sirenComponent = base.GetComponent<EnforcerLightController>();

            if (this.sirenComponent) this.sirenComponent.ToggleSiren();

            base.GetComponent<EnforcerLightControllerAlt>().ToggleSiren();

            base.outer.SetNextStateToMain();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Any;
        }
    }
}