using EnforcerPlugin;
using Modules;
using RoR2;
using UnityEngine;
using UnityEngine.UI;

namespace EntityStates.Enforcer.NeutralSpecial {
    public class SuperShotgun2 : BaseState {

        public static float baseShotDuration = 0.3125f; //This number is FaN //0.85f first shot 62.5% dps? //TODO: base shot needs to not finish reloading until little after the shot
        public static float baseSecondShotDuration = 1.4333f + 0.3125f; //1.8f second shot, total shots 2.65s. 
        public static float baseShieldShotDuration = 1.4333f + 0.3125f; //1.7f shield shot. 

        public static float baseReloadDuration { get => baseSecondShotDuration - baseShotDuration; }
        public static float baseShieldReloadDuration { get => baseShieldShotDuration - baseShotDuration; }

        public static float reloadCompleteFraction = 0.8f;  //This should sync up with the sound. Would be beneficial to separate reload sound from firing sound later.

        public static float bulletForce = 100f;
        public static int bulletCount = 16;
        public static float bulletSpread = Config.superSpread.Value;
        public static float damageCoefficient = Config.superDamage.Value;
        public static float procCoefficient = 0.75f;
        public static float bulletRecoil = 8f;
        public static float shieldedBulletRecoil = 6f;

        public static float beefDurationNoShield = 0.15f;
        public static float beefDurationShield = 0.3f;
        private float attackStopDuration;
        private EnforcerComponent enforcerComponent;

        private bool _secondShot = false; //Determines whether player is forced to reload
        
        private bool _isShielded;
        private float _shieldLockTime = 0.3125f;    //0.6f
        private float _shieldInputBufferableTime = 0.4f;
        private bool _shieldBufferable = false;
        private bool _shieldInputBuffer;

        private bool _finishedReload;
        private float _totalDuration;
        private float _shotDuration;
        private float _reloadDuration;
        private float _reloadCompleteTime;
        private bool _buttonReleased;

        private Animator animator;
        private string muzzleString;
        private bool shieldLocked;

        private bool isStormtrooper;

        public override void OnEnter() {

            base.OnEnter();
            enforcerComponent = GetComponent<EnforcerComponent>();

            if (Skins.isEnforcerCurrentSkin(base.characterBody, "ENFORCERBODY_STORM_SKIN_NAME")) {
                isStormtrooper = true;
            }

            shieldLocked = false;
            _finishedReload = false;
            _buttonReleased = false;

            _isShielded = HasBuff(Buffs.protectAndServeBuff) || HasBuff(Buffs.energyShieldBuff);
            if (_isShielded) {
                _secondShot = true;
            }

            _shotDuration = baseShotDuration / attackSpeedStat; //* Config.superDuration.Value; (1f)
            _reloadDuration = (_isShielded ? baseShieldReloadDuration : baseReloadDuration) / attackSpeedStat; //* Config.superDuration.Value; (1f)
            _totalDuration = _shotDuration + _reloadDuration;
            //If it's all handled in 2anims, change this to shotDuration + reloadDuration * reloadFraction
            _reloadCompleteTime = _totalDuration * reloadCompleteFraction;

            characterBody.SetAimTimer(2f);
            animator = GetModelAnimator();
            muzzleString = "Muzzle";

            if (_isShielded) {
                attackStopDuration = beefDurationShield / attackSpeedStat;
                PlayAnimation("Gesture, Override", "ShieldFireShotgun", "FireShotgun.playbackRate", _totalDuration);
            } else {
                attackStopDuration = beefDurationNoShield / attackSpeedStat;
                PlayAnimation("Gesture, Override", "FireShotgun", "FireShotgun.playbackRate", _totalDuration);
            }

            FireBullet();

        }

        public override void FixedUpdate() {
            base.FixedUpdate();

            enforcerComponent.beefStop = false;
            if (fixedAge < attackStopDuration) {
                if (characterMotor) {
                    characterMotor.moveDirection = Vector3.zero;
                    enforcerComponent.beefStop = true;
                }
            }

            if (fixedAge >= _reloadCompleteTime && !_finishedReload) {
                _finishedReload = true;

                EnforcerWeaponComponent poopy = GetComponent<EnforcerWeaponComponent>();
                poopy.DropShell(-GetModelBaseTransform().transform.right * -Random.Range(6, 16));

                //if it's the first shot, you've only reloaded one shell
                if (_isShielded || _secondShot) {
                    poopy.DropShell(-GetModelBaseTransform().transform.right * -Random.Range(6, 16));
                }

                if (shieldLocked && skillLocator.special && skillLocator.special.skillNameToken == "ENFORCER_SPECIAL_SHIELDDOWN_NAME") {
                    shieldLocked = false;
                    skillLocator.special.enabled = true;
                    skillLocator.special.stock = 1;
                }
            }

            if (isAuthority) {
                if (inputBank) {
                    //I feel like we're reinventing the wheel about "requirekeypress" here?
                    if (!_buttonReleased && !inputBank.skill1.down) {
                        _buttonReleased = true;
                    }

                    //If you've only fired 1 shot, you can cancel the reload early               
                    if (!_secondShot && inputBank.skill1.down && fixedAge > _shotDuration && (!_finishedReload || _buttonReleased && _finishedReload)) {
                        outer.SetNextState(new SuperShotgun2 { _secondShot = !_finishedReload });
                        return;
                    }
                }
                if (fixedAge >= _totalDuration) {
                    outer.SetNextStateToMain();
                    return;
                }

                //wouldn't isShielded cover this?
                if (skillLocator.special && skillLocator.special.skillNameToken == "ENFORCER_SPECIAL_SHIELDDOWN_NAME") {

                    //allow shield input buffer after we've released the key once
                    if (_shieldBufferable && inputBank.skill4.down) {
                        _shieldInputBuffer = true;
                    }
                    if (fixedAge > _shieldInputBufferableTime * _totalDuration && !inputBank.skill4.down) {
                        _shieldBufferable = true;
                    }

                    if (fixedAge > _shieldLockTime * _totalDuration) {
                        shieldLocked = false;
                        skillLocator.special.enabled = true;
                        skillLocator.special.stock = 1;

                        if (_shieldInputBuffer) {

                            outer.SetNextState(new ProtectAndServe());
                            return;
                        }

                    } else if (!shieldLocked && _isShielded) {
                        shieldLocked = true;
                        skillLocator.special.enabled = false;
                        skillLocator.special.stock = 0;

                    }
                }
            }
        }

        public override void OnExit() {
            if (shieldLocked && skillLocator.special && skillLocator.special.skillNameToken == "ENFORCER_SPECIAL_SHIELDDOWN_NAME") {
                shieldLocked = false;
                skillLocator.special.enabled = true;
                skillLocator.special.stock = 1;
            }
            enforcerComponent.beefStop = false;
            base.OnExit();
        }

        public void FireBullet() {
            muzzleString = "Muzzle";

            bool isCrit = RollCrit();

            string soundString = !isCrit ? _isShielded ? Sounds.FireSuperShotgun : Sounds.FireSuperShotgunSingle : _isShielded ? Sounds.FireSuperShotgunCrit : Sounds.FireSuperShotgunSingleCrit;

            if (isStormtrooper) soundString = _isShielded ? Sounds.FireBlasterSSG : Sounds.FireBlasterShotgun;

            Util.PlayAttackSpeedSound(soundString, gameObject, attackSpeedStat);

            float recoilAmplitude = _isShielded ? shieldedBulletRecoil : bulletRecoil / attackSpeedStat;

            AddRecoil(-0.4f * recoilAmplitude, -0.8f * recoilAmplitude, -0.3f * recoilAmplitude, 0.3f * recoilAmplitude);
            characterBody.AddSpreadBloom(4f);
            EffectManager.SimpleMuzzleFlash(Commando.CommandoWeapon.FireBarrage.effectPrefab, gameObject, muzzleString, false);

            if (isAuthority) {
                float damage = damageCoefficient * damageStat;

                GameObject tracerEffect = EnforcerModPlugin.bulletTracerSSG;

                if (this.isStormtrooper) tracerEffect = EnforcerPlugin.EnforcerModPlugin.laserTracer;
                //if (this.isEngi) tracerEffect = EnforcerPlugin.EnforcerModPlugin.bungusTracer;

                Ray aimRay = GetAimRay();

                BulletAttack bulletAttack = new BulletAttack {
                    aimVector = aimRay.direction,
                    origin = aimRay.origin,
                    damage = damage,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.Generic,
                    falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                    maxDistance = 156,
                    force = SuperShotgun2.bulletForce,
                    hitMask = LayerIndex.CommonMasks.bullet,
                    //minSpread = 0,
                    //maxSpread = this.bulletSpread,
                    //bulletCount = (uint)this.projectileCount,
                    isCrit = isCrit,
                    owner = gameObject,
                    muzzleName = muzzleString,
                    smartCollision = false,
                    procChainMask = default,
                    procCoefficient = procCoefficient,
                    radius = 0.4f,
                    sniper = false,
                    stopperMask = LayerIndex.CommonMasks.bullet,
                    weapon = null,
                    tracerEffectPrefab = tracerEffect,
                    spreadPitchScale = 1f,
                    spreadYawScale = 1f,
                    queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                    hitEffectPrefab = Commando.CommandoWeapon.FireBarrage.hitEffectPrefab,
                    HitEffectNormal = ClayBruiser.Weapon.MinigunFire.bulletHitEffectNormal
                };

                if (_isShielded) {
                    shootShielded(bulletAttack);
                } else {
                    shootUnShielded(bulletAttack);
                }
            }
        }

        private void shootUnShielded(BulletAttack bulletAttack) {

            int remainingBullets = SuperShotgun2.bulletCount/2;

            remainingBullets -= 1;
            bulletAttack.bulletCount = 1;
            bulletAttack.minSpread = 0f;
            bulletAttack.maxSpread = 0f;
            bulletAttack.Fire();

            int bullets = Mathf.FloorToInt(remainingBullets/2);
            remainingBullets -= bullets;
            bulletAttack.bulletCount = (uint)bullets;
            bulletAttack.minSpread = 0f;
            bulletAttack.maxSpread = bulletSpread * 0.69f;
            bulletAttack.spreadPitchScale = 0.8f;
            bulletAttack.spreadYawScale = 1.7f;
            bulletAttack.Fire();

            bulletAttack.bulletCount = (uint)remainingBullets;
            bulletAttack.minSpread = bulletSpread * 0.69f;
            bulletAttack.maxSpread = bulletSpread;
            bulletAttack.spreadPitchScale = 0.8f;
            bulletAttack.spreadYawScale =  1.7f;
            bulletAttack.Fire();
        }

        private void shootShielded(BulletAttack bulletAttack) {

            int remainingBullets = SuperShotgun2.bulletCount;

            remainingBullets -= 1;
            bulletAttack.bulletCount = 1;
            bulletAttack.minSpread = 0f;
            bulletAttack.maxSpread = 0f;
            bulletAttack.Fire();

            remainingBullets -= 3;
            bulletAttack.bulletCount = 3;
            bulletAttack.minSpread = 0f;
            bulletAttack.maxSpread = bulletSpread * 0.5f;
            bulletAttack.Fire();

            remainingBullets -= 4;
            bulletAttack.bulletCount = 4;
            bulletAttack.minSpread = 0;
            bulletAttack.maxSpread = bulletSpread;
            bulletAttack.Fire();

            if (remainingBullets > 0) {
                bulletAttack.bulletCount = (uint)remainingBullets;
                bulletAttack.spreadPitchScale = 1f;
                bulletAttack.spreadYawScale = 2.3f;
                bulletAttack.minSpread = bulletSpread / bulletAttack.spreadYawScale;
                bulletAttack.maxSpread = bulletSpread;
                bulletAttack.Fire();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Skill;
        }

        

        public string getCurrentAnimation() {
            if (_isShielded) {
                return "ShieldFireShotgun";
            } else {
                return "FireShotgun";
            }
        }
    }
}
