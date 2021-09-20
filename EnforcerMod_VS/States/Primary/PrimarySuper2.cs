using EnforcerPlugin;
using RoR2;
using UnityEngine;
using UnityEngine.UI;

namespace EntityStates.Enforcer.NeutralSpecial {
    public class SuperShotgun2 : BaseState {

        public static float baseShotDuration = 0.8f; //first shot 62.5% dps? //TODO: base shot needs to not finish reloading until little after the shot
        public static float baseSecondShotDuration = 1.7f; //second shot, total shots 2.5s. 40%dps
        public static float baseShieldShotDuration = 1.6f; //shield shot. 62.5%dps, 56% dps increase overall 

        public static float baseReloadDuration { get => baseSecondShotDuration - baseShotDuration; }
        public static float baseShieldReloadDuration { get => baseShieldShotDuration - baseShotDuration; }

        public static float reloadCompleteFraction = 0.8f;  //This should sync up with the sound. Would be beneficial to separate reload sound from firing sound later.

        public static float bulletForce = 100f;
        public static int bulletCount = 16;
        public static float bulletSpread = EnforcerModPlugin.superSpread.Value;
        public static float damageCoefficient = EnforcerModPlugin.superDamage.Value;
        public static float procCoefficient = 0.75f;
        public static float bulletRecoil = 8f;
        public static float shieldedBulletRecoil = 6f;

        public static float beefDurationNoShield = 0.15f;
        public static float beefDurationShield = 0.3f;
        private float attackStopDuration;

        private bool _secondShot = false; //Determines whether player is forced to reload

        private bool _isShielded;
        private float _shieldLockTime = 0.6f;
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

        public override void OnEnter() {

            base.OnEnter();
            shieldLocked = false;
            _finishedReload = false;
            _buttonReleased = false;

            _isShielded = HasBuff(EnforcerPlugin.Modules.Buffs.protectAndServeBuff) || HasBuff(EnforcerPlugin.Modules.Buffs.energyShieldBuff);
            if (_isShielded) {
                _secondShot = true;
            }

            _shotDuration = baseShotDuration / attackSpeedStat * EnforcerModPlugin.superDuration.Value;
            _reloadDuration = (_isShielded ? baseShieldReloadDuration : baseReloadDuration) / attackSpeedStat * EnforcerModPlugin.superDuration.Value;
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

            animator.speed = 1;
            if (fixedAge < attackStopDuration) {
                if (characterMotor) {
                    animator.speed = 0;
                    characterMotor.moveDirection = Vector3.zero;
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
                    //are we fuckin reinventing the wheel about "requirekeypress" here?
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
            base.OnExit();
        }

        public void FireBullet() {
            muzzleString = "Muzzle";

            string soundString = "";

            bool isCrit = RollCrit();

            soundString = !isCrit ? _isShielded ? Sounds.FireSuperShotgun : Sounds.FireSuperShotgunSingle : _isShielded ? Sounds.FireSuperShotgunCrit : Sounds.FireSuperShotgunSingleCrit;

            Util.PlayAttackSpeedSound(soundString, gameObject, attackSpeedStat);

            float recoilAmplitude = _isShielded ? shieldedBulletRecoil : bulletRecoil / attackSpeedStat;

            AddRecoil(-0.4f * recoilAmplitude, -0.8f * recoilAmplitude, -0.3f * recoilAmplitude, 0.3f * recoilAmplitude);
            characterBody.AddSpreadBloom(4f);
            EffectManager.SimpleMuzzleFlash(Commando.CommandoWeapon.FireBarrage.effectPrefab, gameObject, muzzleString, false);

            if (isAuthority) {
                float damage = damageCoefficient * damageStat;

                GameObject tracerEffect = EnforcerModPlugin.bulletTracerSSG;

                //if (this.isStormtrooper) tracerEffect = EnforcerPlugin.EnforcerPlugin.laserTracer;
                //if (this.isEngi) tracerEffect = EnforcerPlugin.EnforcerPlugin.bungusTracer;

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

                //shit's gotten messy
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
            bulletAttack.spreadPitchScale = 1f;
            bulletAttack.spreadYawScale = TestValueManager.yaw;// 2f;
            bulletAttack.Fire();

            bulletAttack.bulletCount = (uint)remainingBullets;
            bulletAttack.minSpread = bulletSpread * 0.69f;
            bulletAttack.maxSpread = bulletSpread;
            bulletAttack.spreadPitchScale = 1f;
            bulletAttack.spreadYawScale = TestValueManager.yaw;// 2f;
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
