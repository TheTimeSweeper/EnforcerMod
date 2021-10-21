using RoR2;
using UnityEngine;
using EnforcerPlugin;

namespace EntityStates.Enforcer.NeutralSpecial {

    public class RiotShotgun : BaseSkillState 
    {
        public const float RAD2 = 1.414f;//for area calculation
        //public const float RAD3 = 1.732f;//for area calculation

        public static float damageCoefficient = EnforcerModPlugin.shotgunDamage.Value;
        public static float procCoefficient = EnforcerModPlugin.shotgunProcCoefficient.Value;
        public float baseDuration = 0.9f; // the base skill duration. i.e. attack speed
        public float baseShieldDuration = 0.6f; // the duration used while shield is active
        public static int bulletCount = EnforcerModPlugin.shotgunBulletCount.Value;
        public static float bulletSpread = EnforcerModPlugin.shotgunSpread.Value;
        public static float bulletRecoil = 8f;
        public static float shieldedBulletRecoil = 6f;
        public static float beefDurationNoShield = 0.0f;
        public static float beefDurationShield = 0.25f;
        public static float bulletRange = EnforcerModPlugin.shotgunRange.Value;
        public static float bulletThiccness = 0.7f;

        //too much effort being put here
        public static bool levelHasChanged;
        private float originalBulletThiccness = 0.7f;

        protected float duration;
        protected float fireDuration;
        protected float attackStopDuration;
        //protected bool isStormtrooper;
        //protected bool isEngi;
        protected bool hasFired;
        private Animator animator;
        protected string muzzleString;

        protected ShieldComponent shieldComponent;

        public override void OnEnter() {
            base.OnEnter();
            characterBody.SetAimTimer(5f);
            animator = GetModelAnimator();
            shieldComponent = GetComponent<ShieldComponent>();
            muzzleString = "Muzzle";
            hasFired = false;

            /*this.isStormtrooper = false;
            this.isEngi = false;
            if (base.characterBody.skinIndex == EnforcerModPlugin.stormtrooperIndex && EnforcerModPlugin.cursed.Value)
            {
                this.isStormtrooper = true;
            }
            if (base.characterBody.skinIndex == EnforcerModPlugin.engiIndex && EnforcerModPlugin.cursed.Value)
            {
                this.isEngi = true;
            }*/


            if (HasBuff(EnforcerPlugin.Modules.Buffs.protectAndServeBuff) || HasBuff(EnforcerPlugin.Modules.Buffs.energyShieldBuff)) {
                duration = baseShieldDuration / attackSpeedStat;
                attackStopDuration = beefDurationShield / attackSpeedStat;

                PlayAnimation("Gesture, Additive", "ShieldFireShotgun", "FireShotgun.playbackRate", 0.5f * duration);
            } else {
                duration = baseDuration / attackSpeedStat;
                attackStopDuration = beefDurationNoShield / attackSpeedStat;

                PlayCrossfade("Gesture, Override", "FireShotgun", "FireShotgun.playbackRate", 1.75f * duration, 0.06f);
            }

            fireDuration = 0.0f * duration; //fucking windup on a shotgun you dumb cunt
        }

        public override void OnExit() {
            base.OnExit();
        }

        public virtual void FireBullet() {
            if (!hasFired) {
                hasFired = true;

                string soundString = "";

                bool isCrit = RollCrit();

                soundString = isCrit ? Sounds.FireShotgunCrit : Sounds.FireShotgun;

                if (EnforcerModPlugin.classicShotgun.Value) soundString = Sounds.FireClassicShotgun;

                //if (this.isStormtrooper) soundString = EnforcerPlugin.Sounds.FireBlasterShotgun;
                //if (this.isEngi) soundString = EnforcerPlugin.Sounds.FireBungusShotgun;

                Util.PlayAttackSpeedSound(soundString, gameObject, attackSpeedStat);

                float recoilAmplitude = bulletRecoil / this.attackSpeedStat;

                if (HasBuff(EnforcerPlugin.Modules.Buffs.protectAndServeBuff) || HasBuff(EnforcerPlugin.Modules.Buffs.energyShieldBuff)) recoilAmplitude = shieldedBulletRecoil;

                base.AddRecoil(-0.4f * recoilAmplitude, -0.8f * recoilAmplitude, -0.3f * recoilAmplitude, 0.3f * recoilAmplitude);
                characterBody.AddSpreadBloom(4f);
                EffectManager.SimpleMuzzleFlash(Commando.CommandoWeapon.FireBarrage.effectPrefab, gameObject, muzzleString, false);

                //if (!this.isStormtrooper && !this.isEngi)
                GetComponent<EnforcerWeaponComponent>().DropShell(-GetModelBaseTransform().transform.right * -Random.Range(4, 12));

                if (isAuthority) {
                    float damage = damageCoefficient * damageStat;

                    GameObject tracerEffect = EnforcerModPlugin.bulletTracer;

                    if (levelHasChanged) {
                        levelHasChanged = false;

                        thiccenTracer(ref tracerEffect);
                    }
                    //if (this.isStormtrooper) tracerEffect = EnforcerModPlugin.laserTracer;
                    //if (this.isEngi) tracerEffect = EnforcerModPlugin.bungusTracer;

                    Ray aimRay = GetAimRay();

                    float spread = bulletSpread;
                    float thiccness = bulletThiccness;
                    float force = 100; // EnforcerPlugin.UtilsComponent.forceUnshield;
                    if (HasBuff(EnforcerPlugin.Modules.Buffs.protectAndServeBuff)) {
                        spread *= 0.8f;
                        thiccness = 0.69f * thiccness;
                        force = 60; //EnforcerPlugin.UtilsComponent.forceShield; 
                    }

                    BulletAttack bulletAttack = new BulletAttack {
                        aimVector = aimRay.direction,
                        origin = aimRay.origin,
                        damage = damage,
                        damageColorIndex = DamageColorIndex.Default,
                        damageType = DamageType.Generic,
                        falloffModel = BulletAttack.FalloffModel.None,
                        maxDistance = bulletRange,
                        force = force,// RiotShotgun.bulletForce,
                        hitMask = LayerIndex.CommonMasks.bullet,
                        isCrit = isCrit,
                        owner = gameObject,
                        muzzleName = muzzleString,
                        smartCollision = false,
                        procChainMask = default,
                        procCoefficient = procCoefficient,
                        radius = thiccness,
                        sniper = false,
                        stopperMask = LayerIndex.world.collisionMask,
                        weapon = null,
                        tracerEffectPrefab = tracerEffect,
                        spreadPitchScale = 1f,
                        spreadYawScale = 1f,
                        queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                        hitEffectPrefab = Commando.CommandoWeapon.FireBarrage.hitEffectPrefab,
                        HitEffectNormal = false
                    };

                    bulletAttack.minSpread = 0;
                    bulletAttack.maxSpread = 0;
                    bulletAttack.bulletCount = 1;
                    bulletAttack.Fire();

                    uint secondShot = (uint)Mathf.CeilToInt(bulletCount / 2f) - 1;
                    bulletAttack.minSpread = 0;
                    bulletAttack.maxSpread = spread / 1.45f;
                    bulletAttack.bulletCount = secondShot;
                    bulletAttack.Fire();

                    bulletAttack.minSpread = spread / 1.45f;
                    bulletAttack.maxSpread = spread;
                    bulletAttack.bulletCount = (uint)Mathf.FloorToInt(bulletCount / 2f);
                    bulletAttack.Fire();
                }
            }
        }

        private void thiccenTracer(ref GameObject tracerEffect) {

            // getcomponents in foreach forgive my insolence
            foreach (LineRenderer i in tracerEffect.GetComponentsInChildren<LineRenderer>()) {
                if (i) {

                    i.startColor = new Color(0.68f, 0.58f, 0.05f);
                    i.endColor = new Color(0.68f, 0.58f, 0.05f);
                    i.widthMultiplier = (1 + bulletThiccness - originalBulletThiccness) * 0.5f;
                }
            }
        }

        public override void FixedUpdate() {
            base.FixedUpdate();

            
            
            if (isAuthority) {

                shieldComponent.beefStop = false;
                if (fixedAge > fireDuration && fixedAge < attackStopDuration + fireDuration) {
                    if (characterMotor) {
                        //animator.speed = 0;
                        characterMotor.moveDirection = Vector3.zero;
                        //characterBody.moveSpeed = 0;
                        //animator.SetFloat(AnimationParameters.walkSpeed, 0);
                        shieldComponent.beefStop = true;
                    }
                }
            }

            if (fixedAge >= fireDuration) {
                FireBullet();
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