using RoR2;
using UnityEngine;

namespace EntityStates.Enforcer
{
    public class RiotShotgun : BaseSkillState 
    {
        const float RAD2 = 1.414f;//for area calculation

        public static float damageCoefficient = EnforcerPlugin.EnforcerPlugin.shotgunDamage.Value;
        public static float procCoefficient = EnforcerPlugin.EnforcerPlugin.shotgunProcCoefficient.Value;
        public static float bulletForce = 35f;
        public float baseDuration = 0.9f; // the base skill duration
        public float baseShieldDuration = 0.6f; // the duration used while shield is active
        public static int projectileCount = EnforcerPlugin.EnforcerPlugin.shotgunBulletCount.Value;
        public static float bulletSpread = EnforcerPlugin.EnforcerPlugin.shotgunSpread.Value;
        public static float bulletRecoil = 3f;
        public static float shieldedBulletRecoil = 0.5f;
        public static float beefDurationNoShield = 0.0f;
        public static float beefDurationShield = 0.25f;
        public static float bulletRange = EnforcerPlugin.EnforcerPlugin.shotgunRange.Value;

        public float attackStopDuration;   
        public float duration;
        public float fireDuration;
        public bool isStormtrooper;
        public bool isEngi;
        public bool hasFired;
        private Animator animator;
        public string muzzleString;

        public override void OnEnter()
        {
            base.OnEnter();
            base.characterBody.SetAimTimer(2f);
            this.animator = base.GetModelAnimator();
            this.muzzleString = "Muzzle";
            this.isStormtrooper = false;
            this.isEngi = false;
            if (base.characterBody.skinIndex == EnforcerPlugin.EnforcerPlugin.stormtrooperIndex && EnforcerPlugin.EnforcerPlugin.cursed.Value)
            {
                this.isStormtrooper = true;
            }
            if (base.characterBody.skinIndex == EnforcerPlugin.EnforcerPlugin.engiIndex && EnforcerPlugin.EnforcerPlugin.cursed.Value)
            {
                this.isEngi = true;
            }
            this.hasFired = false;

            if (base.HasBuff(EnforcerPlugin.Modules.Buffs.protectAndServeBuff) || base.HasBuff(EnforcerPlugin.Modules.Buffs.energyShieldBuff))
            {
                this.duration = this.baseShieldDuration / this.attackSpeedStat;
                this.attackStopDuration = RiotShotgun.beefDurationShield / this.attackSpeedStat;

                if (base.characterMotor.isGrounded)
                {
                    base.PlayAnimation("Gesture, Override", "ShieldFireShotgun", "FireShotgun.playbackRate", 0.5f * this.duration);
                }
                else
                {
                    base.PlayAnimation("Gesture, Override", "ShieldFireShotgun", "FireShotgun.playbackRate", this.duration);
                }
            }
            else
            {
                this.duration = this.baseDuration / this.attackSpeedStat;
                this.attackStopDuration = RiotShotgun.beefDurationNoShield / this.attackSpeedStat;

                base.PlayAnimation("Gesture, Override", "FireShotgun", "FireShotgun.playbackRate", 1.75f * this.duration);
            }

            this.fireDuration = 0.1f * this.duration;
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public virtual void FireBullet()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                string soundString = "";

                bool isCrit = base.RollCrit();

                soundString = isCrit ? EnforcerPlugin.Sounds.FireShotgun : EnforcerPlugin.Sounds.FireShotgunCrit;

                if (EnforcerPlugin.EnforcerPlugin.classicShotgun.Value) soundString = EnforcerPlugin.Sounds.FireClassicShotgun;

                if (this.isStormtrooper) soundString = EnforcerPlugin.Sounds.FireBlasterShotgun;
                if (this.isEngi) soundString = EnforcerPlugin.Sounds.FireBungusShotgun;

                Util.PlayAttackSpeedSound(soundString, base.gameObject, this.attackSpeedStat);

                float recoil = RiotShotgun.bulletRecoil;

                if (base.HasBuff(EnforcerPlugin.Modules.Buffs.protectAndServeBuff) || base.HasBuff(EnforcerPlugin.Modules.Buffs.energyShieldBuff)) recoil = RiotShotgun.shieldedBulletRecoil;

                base.AddRecoil(-2f * recoil, -3f * recoil, -1f * recoil, 1f * recoil);
                base.characterBody.AddSpreadBloom(0.33f * recoil);
                EffectManager.SimpleMuzzleFlash(Commando.CommandoWeapon.FireBarrage.effectPrefab, base.gameObject, this.muzzleString, false);

                if (!this.isStormtrooper && !this.isEngi) base.GetComponent<EnforcerWeaponComponent>().DropShell(-base.GetModelBaseTransform().transform.right * -Random.Range(4, 12));

                if (base.isAuthority)
                {
                    float damage = RiotShotgun.damageCoefficient * this.damageStat;

                    //unique tracer for stormtrooper skin because this is oddly high effort
                    GameObject tracerEffect = EnforcerPlugin.EnforcerPlugin.bulletTracer;

                    if (this.isStormtrooper) tracerEffect = EnforcerPlugin.EnforcerPlugin.laserTracer;
                    if (this.isEngi) tracerEffect = EnforcerPlugin.EnforcerPlugin.bungusTracer;

                    Ray aimRay = base.GetAimRay();

                    float spread = RiotShotgun.bulletSpread;
                    if (base.HasBuff(EnforcerPlugin.Modules.Buffs.protectAndServeBuff)) spread *= 0.5f;

                    BulletAttack bulletAttack = new BulletAttack {
                        aimVector = aimRay.direction,
                        origin = aimRay.origin,
                        damage = damage,
                        damageColorIndex = DamageColorIndex.Default,
                        damageType = DamageType.Generic,
                        falloffModel = BulletAttack.FalloffModel.None,
                        maxDistance = RiotShotgun.bulletRange,
                        force = RiotShotgun.bulletForce,
                        hitMask = LayerIndex.CommonMasks.bullet,
                        isCrit = isCrit,
                        owner = base.gameObject,
                        muzzleName = muzzleString,
                        smartCollision = false,
                        procChainMask = default(ProcChainMask),
                        procCoefficient = RiotShotgun.procCoefficient,
                        radius = 0.5f,
                        sniper = false,
                        stopperMask = LayerIndex.world.collisionMask,
                        weapon = null,
                        tracerEffectPrefab = tracerEffect,
                        spreadPitchScale = 0.5f,
                        spreadYawScale = 0.5f,
                        queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                        hitEffectPrefab = ClayBruiser.Weapon.MinigunFire.bulletHitEffectPrefab,
                        HitEffectNormal = ClayBruiser.Weapon.MinigunFire.bulletHitEffectNormal
                    };

                    bulletAttack.minSpread = 0;
                    bulletAttack.maxSpread = spread / RAD2;
                    bulletAttack.bulletCount = (uint)Mathf.FloorToInt((float)projectileCount / 2);

                    if (projectileCount == 1)
                    {
                        bulletAttack.bulletCount = 1;
                        bulletAttack.Fire();
                        return;
                    }

                    bulletAttack.Fire();

                    bulletAttack.minSpread = spread / RAD2;
                    bulletAttack.maxSpread = spread;
                    bulletAttack.bulletCount = (uint)Mathf.FloorToInt((float)projectileCount / 2);

                    bulletAttack.Fire();
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            animator.speed = 1;
            if (base.fixedAge < this.attackStopDuration)
            {
                if (base.characterMotor)
                {
                    animator.speed = 0;
                    base.characterMotor.moveDirection = Vector3.zero;
                }
            }

            if (base.fixedAge >= this.fireDuration)
            {
                this.FireBullet();
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

    public class SuperShotgun : RiotShotgun
    {
        public new float damageCoefficient = EnforcerPlugin.EnforcerPlugin.superDamage.Value;
        public new float procCoefficient = 0.75f;
        public new float bulletForce = 25f;
        public new float projectileCount = 16;
        public new float bulletSpread = 18f;
        public new static float baseDuration = 1.5f;
        public new static float baseShieldDuration = 1.3f;
        private bool droppedShell;

        public override void OnEnter()
        {
            base.OnEnter();
            this.droppedShell = false;

            if (base.HasBuff(EnforcerPlugin.Modules.Buffs.protectAndServeBuff) || base.HasBuff(EnforcerPlugin.Modules.Buffs.energyShieldBuff))
            {
                this.duration = SuperShotgun.baseShieldDuration / this.attackSpeedStat;
                this.attackStopDuration = RiotShotgun.beefDurationShield / this.attackSpeedStat;

                base.PlayAnimation("RightArm, Override", "FireSSGShielded", "FireShotgun.playbackRate", this.duration);
            }
            else
            {
                this.duration = SuperShotgun.baseDuration / this.attackSpeedStat;
                this.attackStopDuration = RiotShotgun.beefDurationNoShield / this.attackSpeedStat;

                base.PlayAnimation("RightArm, Override", "FireSSG", "FireShotgun.playbackRate", this.duration);
            }

            this.fireDuration = 0.1f * this.duration;
        }

        public override void FixedUpdate()
        {
            if (base.fixedAge >= 0.55f * this.duration && !this.droppedShell)
            {
                this.droppedShell = true;

                if (!this.isStormtrooper && !this.isEngi)
                {
                    var poopy = base.GetComponent<EnforcerWeaponComponent>();
                    poopy.DropShell(-base.GetModelBaseTransform().transform.right * -Random.Range(6, 16));
                    poopy.DropShell(-base.GetModelBaseTransform().transform.right * -Random.Range(6, 16));
                }
            }

            base.FixedUpdate();
        }

        public override void FireBullet()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                this.muzzleString = "Muzzle";

                string soundString = "";

                bool isCrit = base.RollCrit();

                soundString = isCrit ? EnforcerPlugin.Sounds.FireSuperShotgun : EnforcerPlugin.Sounds.FireSuperShotgunCrit;

                if (EnforcerPlugin.EnforcerPlugin.classicShotgun.Value) soundString = EnforcerPlugin.Sounds.FireClassicShotgun;

                if (base.characterBody.skinIndex == EnforcerPlugin.EnforcerPlugin.doomGuyIndex) soundString = EnforcerPlugin.Sounds.FireSuperShotgunDOOM;
                if (this.isStormtrooper) soundString = EnforcerPlugin.Sounds.FireBlasterShotgun;
                if (this.isEngi) soundString = EnforcerPlugin.Sounds.FireBungusSSG;

                Util.PlayAttackSpeedSound(soundString, base.gameObject, this.attackSpeedStat);

                float recoil = RiotShotgun.bulletRecoil;

                if (base.HasBuff(EnforcerPlugin.Modules.Buffs.protectAndServeBuff) || base.HasBuff(EnforcerPlugin.Modules.Buffs.energyShieldBuff)) recoil = RiotShotgun.shieldedBulletRecoil;

                base.AddRecoil(-2f * recoil, -3f * recoil, -1f * recoil, 1f * recoil);
                base.characterBody.AddSpreadBloom(0.33f * recoil);
                EffectManager.SimpleMuzzleFlash(Commando.CommandoWeapon.FireBarrage.effectPrefab, base.gameObject, this.muzzleString, false);

                if (base.isAuthority)
                {
                    float damage = this.damageCoefficient * this.damageStat;

                    GameObject tracerEffect = EnforcerPlugin.EnforcerPlugin.bulletTracerSSG;

                    if (this.isStormtrooper) tracerEffect = EnforcerPlugin.EnforcerPlugin.laserTracer;
                    if (this.isEngi) tracerEffect = EnforcerPlugin.EnforcerPlugin.bungusTracer;

                    Ray aimRay = base.GetAimRay();

                    new BulletAttack
                    {
                        bulletCount = (uint)this.projectileCount,
                        aimVector = aimRay.direction,
                        origin = aimRay.origin,
                        damage = damage,
                        damageColorIndex = DamageColorIndex.Default,
                        damageType = DamageType.Generic,
                        falloffModel = BulletAttack.FalloffModel.Buckshot,
                        maxDistance = 156,
                        force = this.bulletForce,
                        hitMask = LayerIndex.CommonMasks.bullet,
                        minSpread = 0,
                        maxSpread = this.bulletSpread,
                        isCrit = isCrit,
                        owner = base.gameObject,
                        muzzleName = muzzleString,
                        smartCollision = false,
                        procChainMask = default(ProcChainMask),
                        procCoefficient = this.procCoefficient,
                        radius = 0.3f,
                        sniper = false,
                        stopperMask = LayerIndex.CommonMasks.bullet,
                        weapon = null,
                        tracerEffectPrefab = tracerEffect,
                        spreadPitchScale = 0.3f,
                        spreadYawScale = 0.7f,
                        queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                        hitEffectPrefab = ClayBruiser.Weapon.MinigunFire.bulletHitEffectPrefab,
                        HitEffectNormal = ClayBruiser.Weapon.MinigunFire.bulletHitEffectNormal
                    }.Fire();
                }

                this.PlayGunAnim("Reload");
            }
        }

        public virtual void PlayGunAnim(string animString)
        {
            if (this.GetModelChildLocator().FindChild("SuperShotgunModel"))
            {
                Animator anim = this.GetModelChildLocator().FindChild("SuperShotgunModel").GetComponent<Animator>();
                anim.SetFloat("SuperShottyFire.playbackRate", this.attackSpeedStat);
                anim.SetTrigger(animString);
            }
        }
    }
}