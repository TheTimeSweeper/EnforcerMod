using RoR2;
using UnityEngine;
using EntityStates.ClayBruiser.Weapon;
using Modules;

namespace EntityStates.Enforcer.NeutralSpecial {
    public class FireBurstRifle : BaseSkillState {
        public static float damageCoefficient = Config.rifleDamage.Value;
        public static float procCoefficient = Config.rifleProcCoefficient.Value;
        public static float range = Config.rifleRange.Value;
        public static float baseDuration = 1f;
        public float fireInterval = 0.07f;
        public static int projectileCount = Config.rifleBaseBulletCount.Value;
        public static float minSpread = 0f;
        public static float maxSpread = Config.rifleSpread.Value;
        public float bulletRecoil = 0.75f;

        public static GameObject bulletTracer = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerCommandoDefault");

        private int bulletCount;
        private float duration;
        private float fireDuration;
        private int hasFired;
        private float lastFired;
        private Animator animator;
        private string muzzleString = "Muzzle";

        public override void OnEnter() {
            base.OnEnter();

            duration = baseDuration / attackSpeedStat;
            fireDuration = 0.05f * duration;
            characterBody.SetAimTimer(2f);
            animator = GetModelAnimator();
            muzzleString = "Muzzle";

            hasFired = 0;

            if (characterBody.HasBuff(Buffs.protectAndServeBuff)) {
                bulletCount = 2 * projectileCount;
            } else {
                bulletCount = projectileCount;
            }
        }

        public override void OnExit() {
            base.OnExit();
        }

        private void FireBullet() {

            lastFired = Time.time + fireInterval / attackSpeedStat;

            AddRecoil(-2f * bulletRecoil, -3f * bulletRecoil, -1f * bulletRecoil, 1f * bulletRecoil);
            characterBody.AddSpreadBloom(0.33f * bulletRecoil);
            EffectManager.SimpleMuzzleFlash(Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, gameObject, muzzleString, false);

            if (HasBuff(Buffs.protectAndServeBuff) || HasBuff(Buffs.energyShieldBuff)) {
                PlayAnimation("Gesture, Override", "ShieldFireShotgun", "FireShotgun.playbackRate", 0.5f * duration);
            } else {
                PlayAnimation("Gesture, Override", "FireShotgun", "FireShotgun.playbackRate", 1.75f * duration);
            }

            string soundString = Sounds.FireAssaultRifleSlow;
            //if (this.isStormtrooper) soundString = EnforcerPlugin.Sounds.FireBlasterRifle;
            //if (this.isEngi) soundString = EnforcerPlugin.Sounds.FireBungusRifle;

            Util.PlayAttackSpeedSound(soundString, EnforcerPlugin.VRAPICompat.IsLocalVRPlayer(characterBody) ? EnforcerPlugin.VRAPICompat.GetPrimaryMuzzleObject() : gameObject, attackSpeedStat);

            if (isAuthority) {
                float damage = damageCoefficient * damageStat;
                float force = 10;
                float procCoefficient = 0.75f;
                bool isCrit = RollCrit();

                Ray aimRay = GetAimRay();

                GameObject tracerEffect = bulletTracer;
                //if (this.isStormtrooper) tracerEffect = EnforcerPlugin.EnforcerPlugin.laserTracer;
                //if (this.isEngi) tracerEffect = EnforcerPlugin.EnforcerPlugin.bungusTracer;

                new BulletAttack {
                    bulletCount = 1,
                    aimVector = aimRay.direction,
                    origin = aimRay.origin,
                    damage = damage,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.Generic,
                    falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                    maxDistance = range,
                    force = force,
                    hitMask = LayerIndex.CommonMasks.bullet,
                    minSpread = minSpread,
                    maxSpread = maxSpread,
                    isCrit = isCrit,
                    owner = gameObject,
                    muzzleName = muzzleString,
                    smartCollision = false,
                    procChainMask = default,
                    procCoefficient = procCoefficient,
                    radius = 0.75f,
                    sniper = false,
                    stopperMask = LayerIndex.CommonMasks.bullet,
                    weapon = null,
                    tracerEffectPrefab = tracerEffect,
                    spreadPitchScale = 0.25f,
                    spreadYawScale = 0.25f,
                    queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                    hitEffectPrefab = MinigunFire.bulletHitEffectPrefab,
                    HitEffectNormal = MinigunFire.bulletHitEffectNormal
                }.Fire();
            }
        }

        public override void FixedUpdate() {
            base.FixedUpdate();

            if (fixedAge >= fireDuration && Time.time > lastFired) {

                if (hasFired < bulletCount) {
                    hasFired++;
                    FireBullet();
                }
            }

            if (fixedAge >= duration && isAuthority) {
                outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Skill;
        }
    }
}