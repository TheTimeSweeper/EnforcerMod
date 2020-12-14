using UnityEngine;
using RoR2;

namespace EntityStates.Nemforcer
{
    public class NemforcerMain : GenericCharacterMain
    {
        private float currentHealth;
        private Animator animator;

        public override void OnEnter()
        {
            base.OnEnter();
            base.characterBody.GetComponent<NemforcerController>().mainStateMachine = outer;

            this.animator = base.GetModelAnimator();
            base.smoothingParameters.forwardSpeedSmoothDamp = 0.02f;
            base.smoothingParameters.rightSpeedSmoothDamp = 0.02f;
        }

        public override void Update()
        {
            base.Update();

            //emotes
            if (base.isAuthority && base.characterMotor.isGrounded && !base.characterBody.HasBuff(EnforcerPlugin.EnforcerPlugin.minigunBuff))
            {
                if (Input.GetKeyDown(EnforcerPlugin.EnforcerPlugin.defaultDanceKey.Value))
                {
                    this.outer.SetInterruptState(EntityState.Instantiate(new SerializableEntityStateType(typeof(Enforcer.NemesisRest))), InterruptPriority.Any);
                    return;
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            
            if (base.HasBuff(EnforcerPlugin.EnforcerPlugin.minigunBuff))
            {
                base.characterBody.SetAimTimer(0.2f);
                base.characterBody.isSprinting = false;
            }

            if (this.currentHealth != base.healthComponent.combinedHealth)
            {
                this.currentHealth = base.healthComponent.combinedHealth;
                base.characterBody.RecalculateStats();
            }

            if (this.animator) this.animator.SetBool("inCombat", (!base.characterBody.outOfCombat || !base.characterBody.outOfDanger));
        }
    }
}