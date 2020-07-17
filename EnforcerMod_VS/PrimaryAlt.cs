using RoR2;
using UnityEngine;

namespace EntityStates.Enforcer
{
    public class AssaultRifle : BaseSkillState
    {
        public static GameObject bulletTracerEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/Tracers/TracerClayBruiserMinigun");

        public static float damageCoefficient = 0.3f;
        public static float procCoefficient = 0.6f;
        public static float bulletForce = 20f;
        public static float baseDuration = 0.3f; // the base skill duration
        public static float baseShieldDuration = 0.28f; // the duration used while shield is active
        public static float earlyExitDuration = 0.1f;
        public static int projectileCount = 1;
        public float bulletRecoil = 0.5f;
        public static float beefDurationNoShield = 0.0f;
        public static float beefDurationShield = 0.05f;

        private float attackStopDuration;
        private float duration;
        private float fireDuration;
        private float exitDuration;
        private bool hasFired;
        private Animator animator;
        private string muzzleString;

        public override void OnEnter()
        {
            base.OnEnter();
            base.characterBody.SetAimTimer(0.5f);
            this.animator = base.GetModelAnimator();
            this.muzzleString = "RifleMuzzle";
            this.hasFired = false;
            this.exitDuration = AssaultRifle.earlyExitDuration / this.attackSpeedStat;

            if (base.characterBody.GetComponent<ShieldComponent>().isShielding)
            {
                this.duration = AssaultRifle.baseShieldDuration / this.attackSpeedStat;
                this.attackStopDuration = AssaultRifle.beefDurationShield / this.attackSpeedStat;
            }
            else
            {
                this.duration = AssaultRifle.baseDuration / this.attackSpeedStat;
                this.attackStopDuration = AssaultRifle.beefDurationNoShield / this.attackSpeedStat;
                //base.PlayAnimation("Gesture, Override", "FireShotgun", "FireShotgun.playbackRate", this.duration);
            }

            this.fireDuration = 0.25f * this.duration;

            this.setGunAnimation(true);
        }

        private void setGunAnimation(bool setting)
        {
            this.animator.SetBool("gunUp", setting);
        }

        public override void OnExit()
        {
            base.OnExit();

            this.setGunAnimation(false);
        }

        private void FireBullet()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                string soundString = "";

                bool isCrit = base.RollCrit();

                soundString = ClayBruiser.Weapon.MinigunFire.fireSound;

                Util.PlaySound(soundString, base.gameObject);

                base.AddRecoil(-2f * this.bulletRecoil, -3f * this.bulletRecoil, -1f * this.bulletRecoil, 1f * this.bulletRecoil);
                base.characterBody.AddSpreadBloom(0.33f * this.bulletRecoil);
                EffectManager.SimpleMuzzleFlash(Commando.CommandoWeapon.FirePistol.effectPrefab, base.gameObject, this.muzzleString, false);

                if (base.isAuthority)
                {
                    float damage = AssaultRifle.damageCoefficient * this.damageStat;

                    Ray aimRay = base.GetAimRay();

                    new BulletAttack
                    {
                        bulletCount = (uint)projectileCount,
                        aimVector = aimRay.direction,
                        origin = aimRay.origin,
                        damage = damage,
                        damageColorIndex = DamageColorIndex.Default,
                        damageType = DamageType.Generic,
                        falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                        maxDistance = 88,
                        force = AssaultRifle.bulletForce,
                        hitMask = LayerIndex.CommonMasks.bullet,
                        minSpread = 0,
                        maxSpread = 6f,
                        isCrit = isCrit,
                        owner = base.gameObject,
                        muzzleName = muzzleString,
                        smartCollision = false,
                        procChainMask = default(ProcChainMask),
                        procCoefficient = AssaultRifle.procCoefficient,
                        radius = 0.2f,
                        sniper = false,
                        stopperMask = LayerIndex.CommonMasks.bullet,
                        weapon = null,
                        tracerEffectPrefab = AssaultRifle.bulletTracerEffectPrefab,
                        spreadPitchScale = 0.5f,
                        spreadYawScale = 0.5f,
                        queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                        hitEffectPrefab = Commando.CommandoWeapon.FirePistol.hitEffectPrefab,
                        HitEffectNormal = ClayBruiser.Weapon.MinigunFire.bulletHitEffectNormal
                    }.Fire();
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge < this.attackStopDuration)
            {
                if (base.characterMotor)
                {
                    base.characterMotor.moveDirection = Vector3.zero;
                }
            }

            if (base.fixedAge >= this.fireDuration)
            {
                this.FireBullet();
            }

            if (base.fixedAge >= this.duration - this.exitDuration)
            {
                if (base.inputBank)
                {
                    if (base.inputBank.skill1.down)
                    {
                        this.outer.SetNextState(new AssaultRifle());
                    }
                }
            }

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                setGunAnimation(false);
                this.outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}