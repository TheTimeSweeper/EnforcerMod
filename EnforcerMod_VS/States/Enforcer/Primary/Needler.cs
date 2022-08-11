using RoR2;
using UnityEngine;
using EntityStates.GlobalSkills.LunarNeedle;
using RoR2.Projectile;
using EntityStates.Enforcer.NeutralSpecial;
using Modules;

namespace EntityStates.Enforcer {
    public class FireNeedler : BaseSkillState
    {
        private float duration;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = FireLunarNeedle.baseDuration / this.attackSpeedStat;

            if (HasBuff(Buffs.protectAndServeBuff) || HasBuff(Buffs.energyShieldBuff)) {
                
                PlayAnimation("Gesture, Override", "ShieldFireShotgun", "FireShotgun.playbackRate", Mathf.Max(0.069f, duration));
                duration *= 0.9f;
            } else {

                PlayAnimation("Gesture, Override", "FireShotgun", "FireShotgun.playbackRate", Mathf.Max(0.05f, 1.75f * duration));
            }
            if (base.isAuthority)
            {
                Ray aimRay = base.GetAimRay();
                aimRay.direction = Util.ApplySpread(aimRay.direction, 0f, FireLunarNeedle.maxSpread, 1f, 1f, 0f, 0f);
                FireProjectileInfo fireProjectileInfo = default(FireProjectileInfo);
                fireProjectileInfo.position = base.GetModelChildLocator().FindChild("GrenadeAimOrigin").position;
                fireProjectileInfo.rotation = Quaternion.LookRotation(aimRay.direction);
                fireProjectileInfo.crit = base.characterBody.RollCrit();
                fireProjectileInfo.damage = base.characterBody.damage * FireLunarNeedle.damageCoefficient;
                fireProjectileInfo.damageColorIndex = DamageColorIndex.Default;
                fireProjectileInfo.owner = base.gameObject;
                fireProjectileInfo.procChainMask = default(ProcChainMask);
                fireProjectileInfo.force = 0f;
                fireProjectileInfo.useFuseOverride = false;
                fireProjectileInfo.useSpeedOverride = false;
                fireProjectileInfo.target = null;
                fireProjectileInfo.projectilePrefab = FireLunarNeedle.projectilePrefab;
                ProjectileManager.instance.FireProjectile(fireProjectileInfo);
            }

            base.AddRecoil(-0.4f * FireLunarNeedle.recoilAmplitude, -0.8f * FireLunarNeedle.recoilAmplitude, -0.3f * FireLunarNeedle.recoilAmplitude, 0.3f * FireLunarNeedle.recoilAmplitude);
            base.characterBody.AddSpreadBloom(FireLunarNeedle.spreadBloomValue);
            base.StartAimMode(2f, false);

            EffectManager.SimpleMuzzleFlash(FireLunarNeedle.muzzleFlashEffectPrefab, base.gameObject, "NeedlerMuzzle", false);
            Util.PlaySound(FireLunarNeedle.fireSound, EnforcerPlugin.VRAPICompat.IsLocalVRPlayer(base.characterBody) ? EnforcerPlugin.VRAPICompat.GetPrimaryMuzzleObject() : base.gameObject);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.isAuthority && base.fixedAge >= this.duration)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}