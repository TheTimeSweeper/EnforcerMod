using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;
using UnityEngine.Networking;
using Modules;

namespace EntityStates.Enforcer.NeutralSpecial {
    class FireMachineGun : BaseSkillState {
        public static float damageCoefficient = 1.3f;
        public static float procCoefficient = 1f;
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
        private float firingStopwatch = 0;
        private bool hasFired = false;

        private bool isStormtrooper;
        private bool isEngi;

        public override void OnEnter() {
            base.OnEnter();

            isShielded = HasBuff(Buffs.protectAndServeBuff) || HasBuff(Buffs.energyShieldBuff);

            duration = baseDuration / characterBody.attackSpeed * (isShielded ? 0.8f : 1f);

            if (Skins.isEnforcerCurrentSkin(base.characterBody, Skins.EnforcerSkin.RECOLORSTORM)) {
                this.isStormtrooper = true;
            }
            if (Skins.isEnforcerCurrentSkin(base.characterBody, Skins.EnforcerSkin.RECOLORENGIBUNG)) {
                this.isEngi = true;
            }

            if (NetworkServer.active) {
                if (isShielded && characterBody) {                                  //this ugly?
                    characterBody.AddTimedBuff(RoR2Content.Buffs.Slow50, duration + 0.05f);
                    //characterBody.AddBuff(RoR2Content.Buffs.Slow50);
                }
            }
        }

        private void FireBullet(uint bullets) {

            characterBody.SetAimTimer(2f);

            bool crit = characterBody.RollCrit();

            if (characterBody.isSprinting) {
                characterBody.isSprinting = false;
            }

            if (isShielded) {
                PlayAnimation("Gesture, Override", "ShieldFireShotgun", "FireShotgun.playbackRate", Mathf.Max(0.069f, duration));
            } else {
                PlayAnimation("Gesture, Override", "FireShotgun", "FireShotgun.playbackRate", Mathf.Max(0.05f, 2f * duration));
            }

            string soundString = crit ? Sounds.HMGCrit : Sounds.HMGShoot;

            if (this.isStormtrooper) soundString = Sounds.FireBlasterRifle;
            if (this.isEngi) soundString = Sounds.FireBungusRifle;

            Util.PlaySound(soundString, EnforcerPlugin.VRAPICompat.IsLocalVRPlayer(characterBody) ? EnforcerPlugin.VRAPICompat.GetPrimaryMuzzleObject() : gameObject);

            GameObject flashEffect = Commando.CommandoWeapon.FireBarrage.effectPrefab;

            EffectManager.SimpleMuzzleFlash(flashEffect, gameObject, muzzleName, false);

            if (isAuthority && characterBody) {
                float shieldSpreadMult = isShielded ? FireMachineGun.shieldSpreadMult : 1f;
                Ray aimRay = GetAimRay();

                float maxSpread = baseMaxSpread * shieldSpreadMult;
                if (isShielded && characterBody.spreadBloomAngle > maxSpread) {
                    characterBody.SetSpreadBloom(maxSpread, false);
                    characterBody.spreadBloomInternal = maxSpread;
                }

                GameObject tracer = Commando.CommandoWeapon.FireBarrage.tracerEffectPrefab;
                if (this.isStormtrooper) tracer = EnforcerPlugin.EnforcerModPlugin.laserTracer;
                if (this.isEngi) tracer = EnforcerPlugin.EnforcerModPlugin.bungusTracer;

                new BulletAttack {
                    bulletCount = bullets,
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
                    procCoefficient = procCoefficient,
                    radius = 0.3f,
                    sniper = false,
                    stopperMask = LayerIndex.CommonMasks.bullet,
                    weapon = null,
                    tracerEffectPrefab = tracer,
                    spreadPitchScale = 1f,
                    spreadYawScale = 1f,
                    queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                    hitEffectPrefab = Commando.CommandoWeapon.FireBarrage.hitEffectPrefab,
                    HitEffectNormal = false
                }.Fire();

                float scaledRecoil = recoilAmplitude / characterBody.attackSpeed * bullets;
                characterBody.AddSpreadBloom(FireMachineGun.bloom * shieldSpreadMult);
                scaledRecoil *= Modules.Config.rifleScreenShake.Value;
                AddRecoil(-0.2f * scaledRecoil, -0.69f * scaledRecoil, -0.3f * scaledRecoil, 0.3f * scaledRecoil);
            }
        }

        public override void FixedUpdate() {
            base.FixedUpdate();

            firingStopwatch += Time.deltaTime;

            if (!hasFired) {
                hasFired = true;

                // this scales attack speed past framerate >:)
                float currentStopwatch = firingStopwatch;
                uint bullets = 1;
                while (currentStopwatch > duration) {
                    currentStopwatch -= duration;
                    bullets++;
                }

                FireBullet(bullets);
            }
                
            if (firingStopwatch > duration) {

                outer.SetNextStateToMain();
            }
        }

        public override void OnExit() {
            //if (NetworkServer.active && isShielded && characterBody) {
            //    characterBody.RemoveBuff(RoR2Content.Buffs.Slow50);
            //}
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Skill;
        }
    }
}
