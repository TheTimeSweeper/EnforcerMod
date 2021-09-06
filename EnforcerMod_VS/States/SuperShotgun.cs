using EnforcerPlugin;
using RoR2;
using UnityEngine;
using UnityEngine.UI;

namespace EntityStates.Enforcer
{
    public class SuperShotgun2 : BaseState
    {
        public static float baseShotDuration = 0.3f;
        public static float baseReloadDuration = 1.7f;
        public static float baseShieldReloadDuration = baseReloadDuration * 0.8f;
        public static float reloadCompleteFraction = 0.5f;  //This should sync up with the sound. Would be beneficial to separate reload sound from firing sound later.
        public static float bulletForce = 100f;
        public static int bulletCount = 16;
        public static float bulletSpread = EnforcerModPlugin.superSpread.Value;
        public static float damageCoefficient = EnforcerModPlugin.superDamage.Value;
        public static float procCoefficient = 0.75f;
        public static float bulletRecoil = 8f;
        public static float shieldedBulletRecoil = 6f;

        public static float beefDurationNoShield = 0.15f;
        public static float beefDurationShield = 0.2f;
        private float attackStopDuration;

        public bool secondShot = false; //Determines whether player is forced to reload
        private bool isShielded;
        private bool finishedReload;
        private float totalDuration;
        private float shotDuration;
        private float reloadDuration;
        private float reloadCompleteTime;
        private bool buttonReleased;

        private Animator animator;
        private string muzzleString;
        private bool shieldLocked;

        public override void OnEnter()
        {

            base.OnEnter();
            shieldLocked = false;
            this.finishedReload = false;
            buttonReleased = false;

            isShielded = base.HasBuff(EnforcerPlugin.Modules.Buffs.protectAndServeBuff) || base.HasBuff(EnforcerPlugin.Modules.Buffs.energyShieldBuff);
            if (isShielded)
            {
                secondShot = true;
            }

            shotDuration = baseShotDuration / this.attackSpeedStat;
            reloadDuration = (isShielded ? baseShieldReloadDuration : baseReloadDuration) / this.attackSpeedStat;
            totalDuration = shotDuration + reloadDuration;
            reloadCompleteTime = totalDuration * reloadCompleteFraction;//If it's all handled in 2anims, change this to shotDuration + reloadDuration * reloadFraction

            characterBody.SetAimTimer(2f);
            animator = GetModelAnimator();
            muzzleString = "Muzzle";

            if (isShielded)
            {
                attackStopDuration = beefDurationShield / attackSpeedStat;
                PlayAnimation("Gesture, Override", "ShieldFireShotgun", "FireShotgun.playbackRate", totalDuration);
            }
            else
            {
                attackStopDuration = beefDurationNoShield / attackSpeedStat;
                PlayAnimation("Gesture, Override", "FireShotgun", "FireShotgun.playbackRate", totalDuration);
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

            if (base.fixedAge >= reloadCompleteTime && !this.finishedReload)
            {
                this.finishedReload = true;

                EnforcerWeaponComponent poopy = base.GetComponent<EnforcerWeaponComponent>();
                poopy.DropShell(-base.GetModelBaseTransform().transform.right * -Random.Range(6, 16));
                poopy.DropShell(-base.GetModelBaseTransform().transform.right * -Random.Range(6, 16));

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
                    if (!buttonReleased && !base.inputBank.skill1.down)
                    {
                        buttonReleased = true;
                    }
                    //If you've only fired 1 shot, you can cancel the reload early
                    if ((!finishedReload || (buttonReleased && finishedReload)) && !secondShot && base.inputBank.skill1.down && fixedAge > shotDuration)
                    {
                        this.outer.SetNextState(new SuperShotgun2 { secondShot = !finishedReload });
                        return;
                    }
                }
                if (fixedAge >= totalDuration)
                {
                    this.outer.SetNextStateToMain();
                    return;
                }
                
                //Lock stance change when firing while shielded
                if (!shieldLocked && isShielded && base.skillLocator.special && base.skillLocator.special.skillNameToken == "ENFORCER_SPECIAL_SHIELDDOWN_NAME")
                {
                    shieldLocked = true;
                    base.skillLocator.special.enabled = false;
                    base.skillLocator.special.stock = 0;
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

            soundString = !isCrit ? (isShielded ? Sounds.FireSuperShotgun : Sounds.FireSuperShotgunSingle) : (isShielded ? Sounds.FireSuperShotgunCrit : Sounds.FireSuperShotgunSingleCrit);

            Util.PlayAttackSpeedSound(soundString, base.gameObject, this.attackSpeedStat);

            float recoilAmplitude = isShielded ? SuperShotgun2.shieldedBulletRecoil : SuperShotgun2.bulletRecoil / this.attackSpeedStat;

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

                int bullets = SuperShotgun.bulletCount;


                bullets -= 1;
                bulletAttack.bulletCount = 1;
                bulletAttack.minSpread = 0f;
                bulletAttack.maxSpread = 0f;
                bulletAttack.Fire();



                bullets -= 3;
                bulletAttack.bulletCount = 3;
                bulletAttack.minSpread = 0f;
                bulletAttack.maxSpread = bulletSpread / 2f; // radius / 2 does not equate to area / 2
                bulletAttack.Fire();                             // ratio for actual equal areas come out to around 1.45, so dividing by higher than this results in proportionally tigher spread.
                                                                 // which is good, of course. just letting ya know so ya know, ya know?

                bullets -= 4;
                bulletAttack.bulletCount = 4;
                bulletAttack.minSpread = 0f;
                bulletAttack.maxSpread = bulletSpread;
                bulletAttack.spreadPitchScale = 1f;
                bulletAttack.spreadYawScale = isShielded ? 1 : 1.5f;
                bulletAttack.Fire();


                //unshielded shots shoot 8 shots as above
                //shielded shots shoot the additional 8 shots below
                if (isShielded)
                {
                    if (bullets > 0)
                    {
                        bulletAttack.bulletCount = (uint)bullets;
                        bulletAttack.minSpread = bulletSpread / bulletAttack.spreadYawScale;
                        bulletAttack.maxSpread = bulletSpread;
                        bulletAttack.spreadPitchScale = 1f;
                        bulletAttack.spreadYawScale = 2.3f;
                        bulletAttack.Fire();
                    }
                }
            }
        }

        //Handle second shot in fixedupdate instead
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        public string getCurrentAnimation()
        {
            if (isShielded)
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
