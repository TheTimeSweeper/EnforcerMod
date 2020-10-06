﻿using RoR2;
using UnityEngine;
using EntityStates.GlobalSkills.LunarNeedle;
using RoR2.Projectile;

namespace EntityStates.Enforcer
{
    public class FireNeedler : BaseSkillState
    {
        private float duration;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = FireLunarNeedle.baseDuration / this.attackSpeedStat;

            base.PlayAnimation("RightArm, Override", "FireShotgun", "FireShotgun.playbackRate", this.duration);

            if (base.isAuthority)
            {
                Ray aimRay = base.GetAimRay();
                aimRay.direction = Util.ApplySpread(aimRay.direction, 0f, FireLunarNeedle.maxSpread, 1f, 1f, 0f, 0f);
                FireProjectileInfo fireProjectileInfo = default(FireProjectileInfo);
                fireProjectileInfo.position = base.GetModelChildLocator().FindChild(TearGas.muzzleString).position;
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

            EffectManager.SimpleMuzzleFlash(FireLunarNeedle.muzzleFlashEffectPrefab, base.gameObject, TearGas.muzzleString, false);
            Util.PlaySound(FireLunarNeedle.fireSound, base.gameObject);
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