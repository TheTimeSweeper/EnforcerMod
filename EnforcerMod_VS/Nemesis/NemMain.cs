using UnityEngine;
using RoR2;
using EntityStates.Nemforcer.Emotes;
using EntityStates.Enforcer;
using Modules;
using System;
using static RoR2.CameraTargetParams;
using HG.BlendableTypes;

namespace EntityStates.Nemforcer {
    public class NemforcerMain : GenericCharacterMain
    {
        private CharacterCameraParamsData minigunCameraParams = new CharacterCameraParamsData() {

            maxPitch = 70,
            minPitch = -70,
            pivotVerticalOffset = 1.37f,
            idealLocalCameraPos = minigunCameraPosition,
            wallCushion = 0.1f,
        };

        private static Vector3 minigunCameraPosition = new Vector3(-2.2f, 0.0f, -9f);

        public static CameraParamsOverrideHandle camOverrideHandle;

        private bool wasMinigunning = false;
        private float initialTime;
        private float currentHealth;
        private Animator animator;
        private NemforcerController nemComponent;

        public override void OnEnter()
        {
            base.OnEnter();
            this.nemComponent = base.characterBody.GetComponent<NemforcerController>();
            this.nemComponent.mainStateMachine = outer;

            this.animator = base.GetModelAnimator();
            base.smoothingParameters.forwardSpeedSmoothDamp = 0.02f;
            base.smoothingParameters.rightSpeedSmoothDamp = 0.02f;
        }

        public override void Update()
        {
            base.Update();

            //invasion test
            //if (base.isAuthority && Input.GetKeyDown("z")) EnforcerPlugin.NemesisInvasionManager.PerformInvasion(new Xoroshiro128Plus(Run.instance.seed));

            //minigun mode camera stuff
            bool minigunUp = base.HasBuff(Buffs.minigunBuff);

            if (wasMinigunning != minigunUp) {
                toggleMinigunCamera(minigunUp);
                wasMinigunning = minigunUp;
            }

            //emotes
            if (base.isAuthority && base.characterMotor.isGrounded && !minigunUp)
            {
                if (Input.GetKeyDown(Config.restKey.Value))
                {
                    this.outer.SetInterruptState(new Rest(), InterruptPriority.Any);
                    return;
                }
                else if (Input.GetKeyDown(Config.saluteKey.Value))
                {
                    this.outer.SetInterruptState(new Salute(), InterruptPriority.Any);
                    return;
                }
            }
        }

        private void toggleMinigunCamera(bool minigunUp) {

            if (minigunUp) {

                CameraParamsOverrideRequest request = new CameraParamsOverrideRequest {
                    cameraParamsData = minigunCameraParams,
                    priority = 0,
                };

                camOverrideHandle = base.cameraTargetParams.AddParamsOverride(request, 0.5f);
            } else {

                base.cameraTargetParams.RemoveParamsOverride(camOverrideHandle);
            }

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            
            if (base.HasBuff(Buffs.minigunBuff))
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