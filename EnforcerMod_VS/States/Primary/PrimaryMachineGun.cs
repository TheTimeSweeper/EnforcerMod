using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;
using UnityEngine.Networking;

namespace EntityStates.Enforcer.NeutralSpecial {
    class FireMachineGun : BaseState {
        public static float damageCoefficient = 1.3f;
        public static float baseDuration = 0.21f;

        public static float baseMaxSpread = 7f;
        public static float shieldSpreadMult = 0.3f;
        public static float bloom = 0.4f;
        public static float range = 300f;
        public static float force = 300f;
        public static float recoilAmplitude = 6f;
        public static string muzzleName = "Muzzle";

        private float duration;
        private bool isShielded;
        private float firingStopwatch;

        public override void OnEnter() {
            base.OnEnter();

            isShielded = HasBuff(EnforcerPlugin.Modules.Buffs.protectAndServeBuff) || HasBuff(EnforcerPlugin.Modules.Buffs.energyShieldBuff);
            if (NetworkServer.active) {
                if (isShielded && characterBody) {
                    characterBody.AddBuff(RoR2Content.Buffs.Slow50);
                }
            }
            FireBullet();
        }

        private void FireBullet() {
            if (characterBody.isSprinting) {
                characterBody.isSprinting = false;
            }
            characterBody.SetAimTimer(2f);
            duration = baseDuration / characterBody.attackSpeed * (isShielded ? 0.8f : 1f);
            firingStopwatch = 0f;

            //Someone tweak these animation numbers later
            if (isShielded) {
                PlayAnimation("Gesture, Override", "ShieldFireShotgun", "FireShotgun.playbackRate", 2f * duration);
            } else {
                PlayAnimation("Gesture, Override", "FireShotgun", "FireShotgun.playbackRate", 2f * duration);
            }

            bool crit = characterBody.RollCrit();
            if (crit) {
                Util.PlaySound(EnforcerPlugin.Sounds.HMGCrit, gameObject);
            } else {
                Util.PlaySound(EnforcerPlugin.Sounds.HMGShoot, gameObject);
            }

            EffectManager.SimpleMuzzleFlash(Commando.CommandoWeapon.FireBarrage.effectPrefab, gameObject, muzzleName, false);

            if (isAuthority && characterBody) {
                float shieldSpreadMult = isShielded ? FireMachineGun.shieldSpreadMult : 1f;
                Ray aimRay = GetAimRay();

                float maxSpread = baseMaxSpread * shieldSpreadMult;
                if (isShielded && characterBody.spreadBloomAngle > maxSpread) {
                    characterBody.SetSpreadBloom(maxSpread, false);
                    characterBody.spreadBloomInternal = maxSpread;
                }

                new BulletAttack {
                    bulletCount = 1,
                    aimVector = aimRay.direction,
                    origin = aimRay.origin,
                    damage = characterBody.damage * damageCoefficient,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.Generic,
                    falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                    maxDistance = range,
                    force = force,
                    hitMask = LayerIndex.CommonMasks.bullet,
                    minSpread = 0f,
                    maxSpread = Mathf.Min(characterBody.spreadBloomAngle, maxSpread),
                    isCrit = crit,
                    owner = gameObject,
                    muzzleName = muzzleName,
                    smartCollision = false,
                    procChainMask = default,
                    procCoefficient = 1f,
                    radius = 0.3f,
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

                float scaledRecoil = recoilAmplitude / characterBody.attackSpeed;
                characterBody.AddSpreadBloom(FireMachineGun.bloom * shieldSpreadMult);
                AddRecoil(-0.2f * scaledRecoil, -0.69f * scaledRecoil, -0.3f * scaledRecoil, 0.3f * scaledRecoil);
            }
        }

        public override void FixedUpdate() {
            base.FixedUpdate();
            firingStopwatch += Time.fixedDeltaTime;
            if (firingStopwatch > duration) {
                if (!inputBank || !inputBank.skill1.down) {
                    if (isAuthority) {
                        outer.SetNextStateToMain();
                    }
                } else {
                    FireBullet();
                }
            }
        }

        public override void OnExit() {
            if (NetworkServer.active && isShielded && characterBody) {
                characterBody.RemoveBuff(RoR2Content.Buffs.Slow50);
            }
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Skill;
        }
    }
}
