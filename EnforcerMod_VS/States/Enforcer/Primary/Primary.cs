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
        protected bool isStormtrooper;
        protected bool isEngi;
        protected bool hasFired;
        private Animator animator;
        protected string muzzleString;
        protected float beefDuration;

        protected EnforcerComponent enforcerComponent;        

        public override void OnEnter() {
            base.OnEnter();
            characterBody.SetAimTimer(5f);
            animator = GetModelAnimator();
            enforcerComponent = GetComponent<EnforcerComponent>();
            muzzleString = "Muzzle";
            hasFired = false;

            if (Skins.isEnforcerCurrentSkin(base.characterBody, Skins.EnforcerSkin.RECOLORSTORM)) 
            {
                this.isStormtrooper = true;
            }
            if (Skins.isEnforcerCurrentSkin(base.characterBody, Skins.EnforcerSkin.RECOLORENGIBUNG))
            {
                this.isEngi = true;
            }

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
            // i liked the weight okay but it can be simulated via animation well enough so
            // Fair

            beefDuration = attackStopDuration + fireDuration; // one less math op running in fixedupdate
            // then again fireduration is zero now so i could have just used attackStopDuration in its place
            // future proofing? in case we make him clunkier at some point?
        }

        private GameObject tracerPrefab
        {
            get
            {
                if (this.isStormtrooper) return EnforcerModPlugin.laserTracer;
                if (this.isEngi) return EnforcerModPlugin.bungusTracer;
                return EnforcerModPlugin.bulletTracer;
            }
        }

        public virtual void FireBullet() {
            if (!hasFired) {
                hasFired = true;

                bool isCrit = RollCrit();

                string soundString = isCrit ? Sounds.FireShotgunCrit : Sounds.FireShotgun;

                if (Config.classicShotgun.Value) soundString = Sounds.FireClassicShotgun;

                if (isStormtrooper) soundString = Sounds.FireBlasterShotgun;
                if (isEngi) soundString = Sounds.FireBungusShotgun;

                Util.PlayAttackSpeedSound(soundString, EnforcerPlugin.VRAPICompat.IsLocalVRPlayer(characterBody) ? EnforcerPlugin.VRAPICompat.GetPrimaryMuzzleObject() : gameObject, attackSpeedStat);

                float recoilAmplitude = bulletRecoil / this.attackSpeedStat;

                if (HasBuff(Buffs.protectAndServeBuff) || HasBuff(Buffs.energyShieldBuff)) recoilAmplitude = shieldedBulletRecoil;

                base.AddRecoil(-0.4f * recoilAmplitude, -0.8f * recoilAmplitude, -0.3f * recoilAmplitude, 0.3f * recoilAmplitude);
                characterBody.AddSpreadBloom(4f);
                EffectManager.SimpleMuzzleFlash(Commando.CommandoWeapon.FireBarrage.effectPrefab, gameObject, muzzleString, false);

                if (!this.isStormtrooper)
                    enforcerComponent.weaponComponent.DropShell(-GetModelBaseTransform().transform.right * -Random.Range(4, 12));
                // toook off one getcomponent let's go dude
                // also stormtrooper doesn't drop shells for a reason fuck you
                // laser gun

                if (isAuthority) {
                    float damage = damageCoefficient * damageStat;

                    //if (levelHasChanged) {
                    //    levelHasChanged = false;

                    //    thiccenTracer(ref tracerEffect);
                    //}

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
                        tracerEffectPrefab = tracerPrefab,
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
            //  dude?
        }

        public override void FixedUpdate() {
            base.FixedUpdate();
            
            enforcerComponent.beefStop = false;
            if (fixedAge > fireDuration && fixedAge < beefDuration) {
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