using RoR2;
using UnityEngine;
using EnforcerPlugin;
using Modules;

namespace EntityStates.Enforcer.NeutralSpecial {

    public class RiotShotgun : BaseSkillState 
    {
        public const float RAD2 = 1.414f;//for area calculation
        //public const float RAD3 = 1.732f;//for area calculation

        public static float damageCoefficient = Config.shotgunDamage.Value;
        public static float procCoefficient = Config.shotgunProcCoefficient.Value;
        public float baseDuration = 0.9f; // the base skill duration. i.e. attack speed
        public float baseShieldDuration = 0.6f; // the duration used while shield is active
        public static int bulletCount = Config.shotgunBulletCount.Value;
        public static float bulletSpread = Config.shotgunSpread.Value;
        public static float bulletRecoil = 8f;
        public static float shieldedBulletRecoil = 6f;
        public static float beefDurationNoShield = 0.0f;
        public static float beefDurationShield = 0.25f;
        public static float bulletRange = Config.shotgunRange.Value;
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

        protected EnforcerComponent enforcerComponent;

        public override void OnEnter() {
            base.OnEnter();
            characterBody.SetAimTimer(5f);
            animator = GetModelAnimator();
            enforcerComponent = GetComponent<EnforcerComponent>();
            muzzleString = "Muzzle";
            hasFired = false;

            /*this.isStormtrooper = false;
            this.isEngi = false;
            if (base.characterBody.skinIndex == Config.stormtrooperIndex && Config.cursed.Value)
            {
                this.isStormtrooper = true;
            }
            if (base.characterBody.skinIndex == EnforcerModPlugin.engiIndex && Config.cursed.Value)
            {
                this.isEngi = true;
            }*/

            if (HasBuff(Buffs.protectAndServeBuff) || HasBuff(Buffs.energyShieldBuff)) {
                duration = baseShieldDuration / attackSpeedStat;
                attackStopDuration = beefDurationShield / attackSpeedStat;

                PlayAnimation("Gesture, Override", "ShieldFireShotgun", "FireShotgun.playbackRate", Mathf.Max(0.069f, duration));
            } else {
                duration = baseDuration / attackSpeedStat;
                attackStopDuration = beefDurationNoShield / attackSpeedStat;

                PlayCrossfade("Gesture, Override", "FireShotgun", "FireShotgun.playbackRate", Mathf.Max(0.05f, 1.75f * duration), 0.06f);
            }

            fireDuration = 0;// 0.1f * duration; fucking windup on a shotgun you dumb cunt
        }

        public virtual void FireBullet() {
            if (!hasFired) {
                hasFired = true;

                string soundString = "";

                bool isCrit = RollCrit();

                soundString = isCrit ? Sounds.FireShotgunCrit : Sounds.FireShotgun;

                if (Config.classicShotgun.Value) soundString = Sounds.FireClassicShotgun;

                //if (this.isStormtrooper) soundString = EnforcerPlugin.Sounds.FireBlasterShotgun;
                //if (this.isEngi) soundString = EnforcerPlugin.Sounds.FireBungusShotgun;

                Util.PlayAttackSpeedSound(soundString, gameObject, attackSpeedStat);

                float recoilAmplitude = bulletRecoil / this.attackSpeedStat;

                if (HasBuff(Buffs.protectAndServeBuff) || HasBuff(Buffs.energyShieldBuff)) recoilAmplitude = shieldedBulletRecoil;

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
                    float force = 50; // EnforcerPlugin.UtilsComponent.forceUnshield;
                    if (HasBuff(Buffs.protectAndServeBuff)) {
                        spread *= 0.8f;
                        thiccness = 0.69f * thiccness;
                        force = 30; //EnforcerPlugin.UtilsComponent.forceShield; 
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

                    float addedBulletThiccness = bulletThiccness - originalBulletThiccness;
                    i.widthMultiplier = (1 + addedBulletThiccness) * 0.5f;
                }
            }
        }

        public override void FixedUpdate() {
            base.FixedUpdate();
            
            enforcerComponent.beefStop = false;
            if (fixedAge > fireDuration && fixedAge < attackStopDuration + fireDuration) {
                if (characterMotor) {
                    characterMotor.moveDirection = Vector3.zero;
                    enforcerComponent.beefStop = true;
                }
            }

            if (fixedAge >= fireDuration) {
                FireBullet();
            }

            if (fixedAge >= duration && isAuthority) {
                outer.SetNextStateToMain();
            }
        }

        public override void OnExit() {
            base.OnExit();

            enforcerComponent.beefStop = false;
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Skill;
        }
    }
}