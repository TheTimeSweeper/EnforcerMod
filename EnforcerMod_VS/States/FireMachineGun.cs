using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;
using EntityStates;
using UnityEngine.Networking;

namespace EntityStates.Enforcer
{
    class FireMachineGun : BaseState
    {
        public static float damageCoefficient = 1.3f;
        public static float baseDuration = 0.21f;

        public static float baseMaxSpread = 7f;
        public static float shieldSpreadMult = 0.3f;
        public static float bloom = 0.4f;
        public static float range = 300f;
        public static float force = 500f;
        public static float recoilAmplitude = 6f;
        public static string muzzleName = "Muzzle";

        private float duration;
        private bool isShielded;
        private float firingStopwatch;

        public override void OnEnter()
        {
            base.OnEnter();

            if (base.characterBody)
            {
                if (NetworkServer.active)
                {
                    base.characterBody.AddBuff(RoR2Content.Buffs.Slow50);
                }
            }
            FireBullet();
        }

        private void FireBullet()
        {
            characterBody.SetAimTimer(2f);
            isShielded = HasBuff(EnforcerPlugin.Modules.Buffs.protectAndServeBuff) || HasBuff(EnforcerPlugin.Modules.Buffs.energyShieldBuff);
            duration = FireMachineGun.baseDuration / base.characterBody.attackSpeed * (isShielded ? 0.8f : 1f);
            firingStopwatch = 0f;

            //Someone tweak these animation numbers later
            if (isShielded)
            {
                PlayAnimation("Gesture, Override", "ShieldFireShotgun", "FireShotgun.playbackRate", 2f * duration);
            }
            else
            {
                PlayAnimation("Gesture, Override", "FireShotgun", "FireShotgun.playbackRate", 2f * duration);
            }

            bool crit = base.characterBody.RollCrit();
            if (crit)
            {
                Util.PlaySound(EnforcerPlugin.Sounds.HMGCrit, base.gameObject);
            }
            else
            {
                Util.PlaySound(EnforcerPlugin.Sounds.HMGShoot, base.gameObject);
            }

            EffectManager.SimpleMuzzleFlash(Commando.CommandoWeapon.FireBarrage.effectPrefab, base.gameObject, muzzleName, false);

            if (base.isAuthority && base.characterBody)
            {
                float shieldSpreadMult = (isShielded ? FireMachineGun.shieldSpreadMult : 1f);
                Ray aimRay = base.GetAimRay();
                //EffectManager.SpawnEffect(muzzleFlashPrefab, new EffectData { origin = aimRay.origin }, false);   //Use this to check aimorigin

                float maxSpread = FireMachineGun.baseMaxSpread * shieldSpreadMult;
                if (isShielded && base.characterBody.spreadBloomAngle > maxSpread)
                {
                    base.characterBody.SetSpreadBloom(maxSpread, false);
                    base.characterBody.spreadBloomInternal = maxSpread;
                }

                new BulletAttack
                {
                    bulletCount = 1,
                    aimVector = aimRay.direction,
                    origin = aimRay.origin,
                    damage = base.characterBody.damage * FireMachineGun.damageCoefficient,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.Generic,
                    falloffModel = BulletAttack.FalloffModel.None,
                    maxDistance = FireMachineGun.range,
                    force = FireMachineGun.force,
                    hitMask = LayerIndex.CommonMasks.bullet,
                    minSpread = 0f,
                    maxSpread = Mathf.Min(base.characterBody.spreadBloomAngle, maxSpread),
                    isCrit = crit,
                    owner = gameObject,
                    muzzleName = muzzleName,
                    smartCollision = false,
                    procChainMask = default,
                    procCoefficient = 1f,
                    radius = 0.5f,
                    sniper = false,
                    stopperMask = LayerIndex.CommonMasks.bullet,
                    weapon = null,
                    tracerEffectPrefab = Commando.CommandoWeapon.FireBarrage.tracerEffectPrefab,
                    spreadPitchScale = 1f,
                    spreadYawScale = 1f,
                    queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                    hitEffectPrefab = Commando.CommandoWeapon.FireBarrage.hitEffectPrefab,
                    HitEffectNormal = false
                }.Fire();

                base.characterBody.AddSpreadBloom(FireMachineGun.bloom * shieldSpreadMult);
                base.AddRecoil(-0.4f * recoilAmplitude, -0.8f * recoilAmplitude, -0.3f * recoilAmplitude, 0.3f * recoilAmplitude);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            firingStopwatch += Time.fixedDeltaTime;
            if (base.isAuthority && firingStopwatch > duration)
            {
                if (!base.inputBank || !base.inputBank.skill1.down)
                {
                    base.outer.SetNextStateToMain();
                }
                else
                {
                    FireBullet();
                }
            }
        }

        public override void OnExit()
        {
            if (base.characterBody && NetworkServer.active)
            {
                base.characterBody.RemoveBuff(RoR2Content.Buffs.Slow50);
            }
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}
