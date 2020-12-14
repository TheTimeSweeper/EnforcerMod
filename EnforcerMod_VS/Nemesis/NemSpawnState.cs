using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace EntityStates.Nemforcer
{
    public class SpawnState : BaseState
    {
        public static float duration = 2.25f;

        public override void OnEnter()
        {
            base.OnEnter();
            base.PlayAnimation("Body", "Spawn");
            Util.PlaySound(NullifierMonster.SpawnState.spawnSoundString, base.gameObject);

            base.GetModelAnimator().SetLayerWeight(base.GetModelAnimator().GetLayerIndex("Minigun"), 0);

            if (NetworkServer.active) base.characterBody.AddBuff(BuffIndex.HiddenInvincibility);

            if (EnforcerPlugin.EnforcerPlugin.nemesisSpawnEffect)
            {
                EffectManager.SimpleMuzzleFlash(EnforcerPlugin.EnforcerPlugin.nemesisSpawnEffect, base.gameObject, "SpawnOrigin", false);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            base.inputBank.aimDirection = -base.modelLocator.modelBaseTransform.forward;
            base.cameraTargetParams.idealLocalCameraPos = new Vector3(0f, -0.8f, -14f);

            if (base.fixedAge >= SpawnState.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            if (NetworkServer.active) base.characterBody.RemoveBuff(BuffIndex.HiddenInvincibility);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
    }
}
