using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using System.Collections.Generic;

namespace EntityStates.Enforcer
{
    public class RiotShotgun : BaseSkillState
    {
        public float damageCoefficient = 0.6f;
        public float baseDuration = 0.9f; // the base skill duration
        public float baseShieldDuration = 0.6f; // the duration used while shield is active
        public int projectileCount = 4;
        public float bulletRecoil = 3f;
        public static GameObject bulletTracerEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/Tracers/TracerCommandoShotgun");

        private float duration;
        private float fireDuration;
        private bool hasFired;
        private Animator animator;
        private string muzzleString;

        public override void OnEnter()
        {
            base.OnEnter();
            base.characterBody.SetAimTimer(2f);
            this.animator = base.GetModelAnimator();
            this.muzzleString = "Root"; //use root as the muzzle for now, until the childlocator is set up at least
            this.hasFired = false;


            if (base.characterBody.HasBuff(EnforcerPlugin.EnforcerPlugin.jackBootsIndex))
            {
                this.duration = this.baseShieldDuration / this.attackSpeedStat;
                this.fireDuration = 0.1f * this.duration;
            }
            else
            {
                this.duration = this.baseDuration / this.attackSpeedStat;
                this.fireDuration = 0.1f * this.duration;

                base.PlayAnimation("Gesture, Override", "FireShotgun", "FireShotgun.playbackRate", this.duration);
            }

            //Util.PlaySound("", base.gameObject);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        private void FireBullet()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                //Util.PlaySound("", base.gameObject);

                base.AddRecoil(-2f * this.bulletRecoil, -3f * this.bulletRecoil, -1f * this.bulletRecoil, 1f * this.bulletRecoil);
                base.characterBody.AddSpreadBloom(0.33f * this.bulletRecoil);
                EffectManager.SimpleMuzzleFlash(Commando.CommandoWeapon.FireShotgun.effectPrefab, base.gameObject, this.muzzleString, false);

                if (base.isAuthority)
                {
                    float damage = this.damageCoefficient * this.damageStat;
                    float force = 10;
                    float procCoefficient = 0.5f;
                    bool isCrit = base.RollCrit();

                    Ray aimRay = base.GetAimRay();

                    new BulletAttack
                    {
                        bulletCount = (uint)projectileCount,
                        aimVector = aimRay.direction,
                        origin = aimRay.origin,
                        damage = damage,
                        damageColorIndex = DamageColorIndex.Default,
                        damageType = DamageType.Generic,
                        falloffModel = BulletAttack.FalloffModel.None,
                        maxDistance = 48,
                        force = force,
                        hitMask = LayerIndex.CommonMasks.bullet,
                        minSpread = 0,
                        maxSpread = 15f,
                        isCrit = isCrit,
                        owner = base.gameObject,
                        muzzleName = muzzleString,
                        smartCollision = false,
                        procChainMask = default(ProcChainMask),
                        procCoefficient = procCoefficient,
                        radius = 1.75f,
                        sniper = false,
                        stopperMask = LayerIndex.background.collisionMask,
                        weapon = null,
                        tracerEffectPrefab = bulletTracerEffectPrefab,
                        spreadPitchScale = 0.5f,
                        spreadYawScale = 0.5f,
                        queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                        hitEffectPrefab = ClayBruiser.Weapon.MinigunFire.bulletHitEffectPrefab,
                        HitEffectNormal = ClayBruiser.Weapon.MinigunFire.bulletHitEffectNormal
                    }.Fire();
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.fireDuration)
            {
                FireBullet();
            }

            if (base.fixedAge >= this.duration && base.isAuthority)
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