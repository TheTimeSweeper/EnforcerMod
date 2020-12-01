using RoR2;
using UnityEngine.Networking;

namespace EntityStates.Nemforcer
{
    public class SpawnState : BaseState
    {
        public static float duration = 2f;

        public override void OnEnter()
        {
            base.OnEnter();
            //need a spawn anim
            //base.PlayAnimation("Body", "Spawn", "Spawn.playbackRate", SpawnState.duration);
            Util.PlaySound(NullifierMonster.SpawnState.spawnSoundString, base.gameObject);

            base.GetModelAnimator().SetLayerWeight(base.GetModelAnimator().GetLayerIndex("Minigun"), 0);

            if (NetworkServer.active) base.characterBody.AddBuff(BuffIndex.HiddenInvincibility);

            if (NullifierMonster.SpawnState.spawnEffectPrefab)
            {
                EffectManager.SimpleMuzzleFlash(NullifierMonster.SpawnState.spawnEffectPrefab, base.gameObject, "Chest", false);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

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
