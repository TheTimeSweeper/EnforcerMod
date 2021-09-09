using EnforcerPlugin;
using RoR2;
using UnityEngine;
using UnityEngine.UI;

namespace EntityStates.Enforcer
{
    public class SuperShotgun2 : BaseState
    {
        //man I thought my logic was confusing
        //have to pull out a calculator to see how long the shield shot is wtf. 
        public static float baseShotDuration = 0.8f; //first shot 62.5% dps? //TODO: base shot needs to not finish reloading until little after the shot
        public static float baseSecondShotDuration = 1.7f; //second shot, total shots 2.5s. 40%dps
        public static float baseShieldShotDuration = 1.6f; //shield shot. 62.5%dps, 56% dps increase overall 

        public static float baseReloadDuration { get => baseSecondShotDuration - baseShotDuration; }
        public static float baseShieldReloadDuration { get => baseShieldShotDuration - baseShotDuration; }

        public static float reloadCompleteFraction = 0.5f;  //This should sync up with the sound. Would be beneficial to separate reload sound from firing sound later.
        
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

        public override void OnEnter()
        {

            base.OnEnter();
            shieldLocked = false;
            this._finishedReload = false;
            _buttonReleased = false;

            _isShielded = base.HasBuff(EnforcerPlugin.Modules.Buffs.protectAndServeBuff) || base.HasBuff(EnforcerPlugin.Modules.Buffs.energyShieldBuff);
            if (_isShielded)
            {
                _secondShot = true;
            }

            _shotDuration = baseShotDuration / this.attackSpeedStat * EnforcerModPlugin.superDuration.Value;
            _reloadDuration = (_isShielded ? baseShieldReloadDuration : baseReloadDuration) / this.attackSpeedStat * EnforcerModPlugin.superDuration.Value;
            _totalDuration = _shotDuration + _reloadDuration;
            //If it's all handled in 2anims, change this to shotDuration + reloadDuration * reloadFraction
            _reloadCompleteTime = _totalDuration * reloadCompleteFraction;

            characterBody.SetAimTimer(2f);
            animator = GetModelAnimator();
            muzzleString = "Muzzle";

            if (_isShielded)
            {
                attackStopDuration = beefDurationShield / attackSpeedStat;
                PlayAnimation("Gesture, Override", "ShieldFireShotgun", "FireShotgun.playbackRate", _totalDuration);
            }
            else
            {
                attackStopDuration = beefDurationNoShield / attackSpeedStat;
                PlayAnimation("Gesture, Override", "FireShotgun", "FireShotgun.playbackRate", _totalDuration);
            }

            FireBullet();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            animator.speed = 1;
            if (fixedAge < attackStopDuration)
            {
                if (characterMotor)
                {
                    animator.speed = 0;
                    characterMotor.moveDirection = Vector3.zero;
                }
            }

            if (base.fixedAge >= _reloadCompleteTime && !this._finishedReload)
            {
                this._finishedReload = true;

                EnforcerWeaponComponent poopy = base.GetComponent<EnforcerWeaponComponent>();
                poopy.DropShell(-base.GetModelBaseTransform().transform.right * -Random.Range(6, 16));

                //if it's the first shot, you've only reloaded one shell
                if(_isShielded || _secondShot) {
                    poopy.DropShell(-base.GetModelBaseTransform().transform.right * -Random.Range(6, 16));
                }

                if (shieldLocked && base.skillLocator.special && base.skillLocator.special.skillNameToken == "ENFORCER_SPECIAL_SHIELDDOWN_NAME")
                {
                    shieldLocked = false;
                    base.skillLocator.special.enabled = true;
                    base.skillLocator.special.stock = 1;
                }
            }

            if (isAuthority)
            {
                if (base.inputBank)
                {
                    //are we fuckin reinventing the wheel about "requirekeypress" here?
                    if (!_buttonReleased && !base.inputBank.skill1.down)
                    {
                        _buttonReleased = true;
                    }

                    //If you've only fired 1 shot, you can cancel the reload early               
                    if (!_secondShot && base.inputBank.skill1.down && fixedAge > _shotDuration && (!_finishedReload || (_buttonReleased && _finishedReload)))
                    {
                        this.outer.SetNextState(new SuperShotgun2 { _secondShot = !_finishedReload });
                        return;
                    }
                }
                if (fixedAge >= _totalDuration)
                {
                    this.outer.SetNextStateToMain();
                    return;
                }

                                                                                             //wouldn't isShielded cover this?
                if (base.skillLocator.special && base.skillLocator.special.skillNameToken == "ENFORCER_SPECIAL_SHIELDDOWN_NAME") {

                    //allow shield input buffer after we've released the key once
                    if (_shieldBufferable && base.inputBank.skill4.down) {
                        _shieldInputBuffer = true;
                    }
                    if (fixedAge > _shieldInputBufferableTime * _totalDuration && !base.inputBank.skill4.down) {
                        _shieldBufferable = true;
                    }

                    if (fixedAge > _shieldLockTime * _totalDuration) {
                        shieldLocked = false;
                        base.skillLocator.special.enabled = true;
                        base.skillLocator.special.stock = 1;

                        if(_shieldInputBuffer) {

                            this.outer.SetNextState(new ProtectAndServe());
                            return;
                        }

                    } else if (!shieldLocked && _isShielded) {
                        shieldLocked = true;
                        base.skillLocator.special.enabled = false;
                        base.skillLocator.special.stock = 0;

                    }
                }
            }
        }

        public override void OnExit()
        {
            if (shieldLocked && base.skillLocator.special && base.skillLocator.special.skillNameToken == "ENFORCER_SPECIAL_SHIELDDOWN_NAME")
            {
                shieldLocked = false;
                base.skillLocator.special.enabled = true;
                base.skillLocator.special.stock = 1;
            }
            base.OnExit();
        }

        public void FireBullet()
        {
            this.muzzleString = "Muzzle";

            string soundString = "";

            bool isCrit = base.RollCrit();

            soundString = !isCrit ? (_isShielded ? Sounds.FireSuperShotgun : Sounds.FireSuperShotgunSingle) : (_isShielded ? Sounds.FireSuperShotgunCrit : Sounds.FireSuperShotgunSingleCrit);

            Util.PlayAttackSpeedSound(soundString, base.gameObject, this.attackSpeedStat);

            float recoilAmplitude = _isShielded ? SuperShotgun2.shieldedBulletRecoil : SuperShotgun2.bulletRecoil / this.attackSpeedStat;

            base.AddRecoil(-0.4f * recoilAmplitude, -0.8f * recoilAmplitude, -0.3f * recoilAmplitude, 0.3f * recoilAmplitude);
            base.characterBody.AddSpreadBloom(4f);
            EffectManager.SimpleMuzzleFlash(Commando.CommandoWeapon.FireBarrage.effectPrefab, base.gameObject, this.muzzleString, false);

            if (base.isAuthority)
            {
                float damage = damageCoefficient * this.damageStat;

                GameObject tracerEffect = EnforcerPlugin.EnforcerModPlugin.bulletTracerSSG;

                //if (this.isStormtrooper) tracerEffect = EnforcerPlugin.EnforcerPlugin.laserTracer;
                //if (this.isEngi) tracerEffect = EnforcerPlugin.EnforcerPlugin.bungusTracer;

                Ray aimRay = base.GetAimRay();

                BulletAttack bulletAttack = new BulletAttack
                {
                    aimVector = aimRay.direction,
                    origin = aimRay.origin,
                    damage = damage,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.Generic,
                    falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                    maxDistance = 156,
                    force = SuperShotgun.bulletForce,
                    hitMask = LayerIndex.CommonMasks.bullet,
                    //minSpread = 0,
                    //maxSpread = this.bulletSpread,
                    //bulletCount = (uint)this.projectileCount,
                    isCrit = isCrit,
                    owner = base.gameObject,
                    muzzleName = muzzleString,
                    smartCollision = false,
                    procChainMask = default(ProcChainMask),
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
                //shootShielded(bulletAttack);
                //shootUnShielded(bulletAttack);

                int remainingBullets = SuperShotgun.bulletCount;

                remainingBullets -= 1;
                bulletAttack.bulletCount = 1;
                bulletAttack.minSpread = 0f;
                bulletAttack.maxSpread = 0f;
                bulletAttack.Fire();

                remainingBullets -= 3;
                bulletAttack.bulletCount = 3;
                bulletAttack.spreadPitchScale = 1f;
                bulletAttack.spreadYawScale = _isShielded ? 1 : 1.4f;
                bulletAttack.minSpread = 0f;
                bulletAttack.maxSpread = bulletSpread * 0.5f;
                bulletAttack.Fire();


                remainingBullets -= 4;
                bulletAttack.bulletCount = 4;
                bulletAttack.minSpread = _isShielded ? 0 : bulletSpread * 0.25f;
                bulletAttack.maxSpread = bulletSpread;
                bulletAttack.spreadPitchScale = 1f;
                bulletAttack.spreadYawScale = _isShielded ? 1 : 1.7f;
                bulletAttack.Fire();

                //unshielded shots shoot 8 shots as above
                //shielded shots shoot the additional 8 shots below
                if (_isShielded) {
                    if (remainingBullets > 0) {
                        bulletAttack.bulletCount = (uint)remainingBullets;
                        bulletAttack.minSpread = bulletSpread / bulletAttack.spreadYawScale;
                        bulletAttack.maxSpread = bulletSpread;
                        bulletAttack.spreadPitchScale = 1f;
                        bulletAttack.spreadYawScale = 2.3f;
                        bulletAttack.Fire();
                    }
                }
            }
        }

        private void shootUnShielded(BulletAttack bulletAttack) {

            int remainingBullets = SuperShotgun.bulletCount;

            remainingBullets -= 1;
            bulletAttack.bulletCount = 1;
            bulletAttack.minSpread = 0f;
            bulletAttack.maxSpread = 0f;
            bulletAttack.Fire();

            remainingBullets -= 3;
            bulletAttack.bulletCount = 3;
            bulletAttack.spreadPitchScale = 1f;
            bulletAttack.spreadYawScale = 1.4f;
            bulletAttack.minSpread = 0f;
            bulletAttack.maxSpread = bulletSpread * 0.5f;
            bulletAttack.Fire();


            remainingBullets -= 4;
            bulletAttack.bulletCount = 4;
            bulletAttack.minSpread = _isShielded ? 0 : bulletSpread * 0.25f;
            bulletAttack.maxSpread = bulletSpread;
            bulletAttack.spreadPitchScale = 1f;
            bulletAttack.spreadYawScale = 1.7f;
            bulletAttack.Fire();
        }

        private void shootShielded(BulletAttack bulletAttack) {

            int remainingBullets = SuperShotgun.bulletCount;

            remainingBullets -= 1;
            bulletAttack.bulletCount = 1;
            bulletAttack.minSpread = 0f;
            bulletAttack.maxSpread = 0f;
            bulletAttack.Fire();

            remainingBullets -= 3;
            bulletAttack.bulletCount = 3;
            bulletAttack.spreadPitchScale = 1f;
            bulletAttack.spreadYawScale = 1;
            bulletAttack.minSpread = 0f;
            bulletAttack.maxSpread = bulletSpread * 0.5f;
            bulletAttack.Fire();


            remainingBullets -= 4;
            bulletAttack.bulletCount = 4;
            bulletAttack.minSpread = 0;
            bulletAttack.maxSpread = bulletSpread;
            bulletAttack.spreadPitchScale = 1f;
            bulletAttack.spreadYawScale = 1;
            bulletAttack.Fire();

            if (_isShielded) {
                if (remainingBullets > 0) {
                    bulletAttack.bulletCount = (uint)remainingBullets;
                    bulletAttack.minSpread = bulletSpread / bulletAttack.spreadYawScale;
                    bulletAttack.maxSpread = bulletSpread;
                    bulletAttack.spreadPitchScale = 1f;
                    bulletAttack.spreadYawScale = 2.3f;
                    bulletAttack.Fire();
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        public string getCurrentAnimation()
        {
            if (_isShielded)
            {
                return "ShieldFireShotgun";
            }
            else
            {
                return "FireShotgun";
            }
        }
    }
}
