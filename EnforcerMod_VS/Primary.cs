using RoR2;
using UnityEngine;

namespace EntityStates.Enforcer
{
    public class RiotShotgun : BaseSkillState 
    {
        public static GameObject bulletTracerEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/Tracers/TracerCommandoShotgun");

        public static float damageCoefficient = 0.3f;
        public static float procCoefficient = 0.4f;
        public static float bulletForce = 100f;
        public float baseDuration = 0.9f; // the base skill duration
        public float baseShieldDuration = 0.6f; // the duration used while shield is active
        public static int projectileCount = 8;
        public float bulletRecoil = 3f;
        public static float beefDurationNoShield = 0.0f;
        public static float beefDurationShield = 0.2f;

        private float attackStopDuration;
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
            this.muzzleString = "Muzzle";
            this.hasFired = false;

            if (base.characterBody.GetComponent<ShieldComponent>().isShielding)
            {
                this.duration = this.baseShieldDuration / this.attackSpeedStat;
                attackStopDuration = beefDurationShield / attackSpeedStat;
            }
            else
            {
                this.duration = this.baseDuration / this.attackSpeedStat;
                attackStopDuration = beefDurationNoShield / attackSpeedStat;

                base.PlayAnimation("Gesture, Override", "FireShotgun", "FireShotgun.playbackRate", this.duration);
            }

            this.fireDuration = 0.1f * this.duration;

            setGunAnimation(true);


            Util.PlayScaledSound(EntityStates.Commando.CommandoWeapon.FireLightsOut.attackSoundString, base.gameObject, 0.75f);
        }

        private void setGunAnimation(bool setting) {
            animator.SetBool("gunUp", setting);
        }

        public override void OnExit()
        {
            base.OnExit();

            setGunAnimation(false);
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
                    float damage = RiotShotgun.damageCoefficient * this.damageStat;
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
                        force = RiotShotgun.bulletForce,
                        hitMask = LayerIndex.CommonMasks.bullet,
                        minSpread = 0,
                        maxSpread = 12f,
                        isCrit = isCrit,
                        owner = base.gameObject,
                        muzzleName = muzzleString,
                        smartCollision = false,
                        procChainMask = default(ProcChainMask),
                        procCoefficient = RiotShotgun.procCoefficient,
                        radius = 0.5f,
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

            if(fixedAge < attackStopDuration) {

                if (characterMotor) {
                    characterMotor.moveDirection = Vector3.zero;
                }
            }

            if (base.fixedAge >= this.fireDuration)
            {
                FireBullet();
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