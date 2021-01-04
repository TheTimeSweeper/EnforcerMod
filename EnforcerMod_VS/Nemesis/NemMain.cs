using UnityEngine;
using RoR2;

namespace EntityStates.Nemforcer
{
    public class NemforcerMain : GenericCharacterMain
    {
        private bool wasShielding = false;
        private float initialTime;
        private float currentHealth;
        private Animator animator;
        private AimAnimator aimAnimator;
        private NemforcerController nemComponent;

        public override void OnEnter()
        {
            base.OnEnter();
            this.nemComponent = base.characterBody.GetComponent<NemforcerController>();
            this.nemComponent.mainStateMachine = outer;

            this.animator = base.GetModelAnimator();
            this.aimAnimator = base.GetAimAnimator();
            base.smoothingParameters.forwardSpeedSmoothDamp = 0.02f;
            base.smoothingParameters.rightSpeedSmoothDamp = 0.02f;
        }

        public override void Update()
        {
            base.Update();

            //minigun mode camera stuff
            bool minigunUp = base.HasBuff(EnforcerPlugin.EnforcerPlugin.minigunBuff);

            if (minigunUp != this.wasShielding)
            {
                this.wasShielding = minigunUp;
                this.initialTime = Time.fixedTime;
            }

            //emotes
            if (base.isAuthority && base.characterMotor.isGrounded && !minigunUp)
            {
                if (Input.GetKeyDown(EnforcerPlugin.EnforcerPlugin.defaultDanceKey.Value))
                {
                    this.outer.SetInterruptState(EntityState.Instantiate(new SerializableEntityStateType(typeof(Enforcer.NemesisRest))), InterruptPriority.Any);
                    return;
                }
            }

            if (this.nemComponent.isMultiplayer)
            {
                if (minigunUp)
                {
                    CameraTargetParams ctp = base.cameraTargetParams;
                    float denom = (1 + Time.fixedTime - this.initialTime);
                    float smoothFactor = 8 / Mathf.Pow(denom, 2);
                    Vector3 smoothVector = new Vector3(-3 / 20, 1 / 16, -1);
                    ctp.idealLocalCameraPos = new Vector3(-1.2f, -0.5f, -9f) + smoothFactor * smoothVector;
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