using RoR2;
using UnityEngine;
using EntityStates.ClayBruiser.Weapon;

namespace EntityStates.Enforcer
{
    public class FireBurstRifle : BaseSkillState
    {
        public static float damageCoefficient = EnforcerPlugin.EnforcerPlugin.rifleDamage.Value;
        public static float procCoefficient = EnforcerPlugin.EnforcerPlugin.rifleProcCoefficient.Value;
        public static float range = EnforcerPlugin.EnforcerPlugin.rifleRange.Value;
        public static float baseDuration = 1f;
        public float fireInterval = 0.07f;
        public static int projectileCount = EnforcerPlugin.EnforcerPlugin.rifleBaseBulletCount.Value;
        public static float minSpread = 0f;
        public static float maxSpread = EnforcerPlugin.EnforcerPlugin.rifleSpread.Value;
        public float bulletRecoil = 0.75f;

        public static GameObject bulletTracer = Resources.Load<GameObject>("Prefabs/Effects/Tracers/TracerCommandoDefault");

        private int bulletCount;
        private float duration;
        private float fireDuration;
        private int hasFired;
        private float lastFired;
        private Animator animator;
        private string muzzleString;
        private bool isStormtrooper;
        private bool isEngi;

        public override void OnEnter()
        {
            base.OnEnter();

            this.duration = FireBurstRifle.baseDuration / this.attackSpeedStat;
            this.fireDuration = 0.05f * this.duration;
            base.characterBody.SetAimTimer(2f);
            this.animator = base.GetModelAnimator();
            this.muzzleString = "RifleMuzzle";
            this.isStormtrooper = false;
            this.isEngi = false;
            if (base.characterBody.skinIndex == EnforcerPlugin.EnforcerPlugin.stormtrooperIndex && EnforcerPlugin.EnforcerPlugin.cursed.Value)
            {
                this.muzzleString = "BlasterRifleMuzzle";
                this.isStormtrooper = true;
            }
            if (base.characterBody.skinIndex == EnforcerPlugin.EnforcerPlugin.engiIndex && EnforcerPlugin.EnforcerPlugin.cursed.Value)
            {
                this.muzzleString = "GrenadeMuzzle";
                this.isEngi = true;
            }
            this.hasFired = 0;

            if (base.characterBody.HasBuff(EnforcerPlugin.EnforcerPlugin.jackBoots))
            {
                this.bulletCount = 2 * FireBurstRifle.projectileCount;
            }
            else
            {
                this.bulletCount = FireBurstRifle.projectileCount;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        private void FireBullet()
        {
            if (this.hasFired < this.bulletCount)
            {
                this.hasFired++;
                this.lastFired = Time.time + (this.fireInterval / this.attackSpeedStat);

                base.AddRecoil(-2f * this.bulletRecoil, -3f * this.bulletRecoil, -1f * this.bulletRecoil, 1f * this.bulletRecoil);
                base.characterBody.AddSpreadBloom(0.33f * this.bulletRecoil);
                EffectManager.SimpleMuzzleFlash(Commando.CommandoWeapon.FirePistol.effectPrefab, base.gameObject, this.muzzleString, false);

                if (base.characterBody.HasBuff(EnforcerPlugin.EnforcerPlugin.jackBoots))
                {
                    base.PlayAnimation("RightArm, Override", "FireShotgunShielded", "FireShotgun.playbackRate", this.duration);
                }
                else
                {
                    base.PlayAnimation("RightArm, Override", "FireShotgun", "FireShotgun.playbackRate", this.duration);
                }

                string soundString = EnforcerPlugin.Sounds.FireAssaultRifleSlow;
                if (this.isStormtrooper) soundString = EnforcerPlugin.Sounds.FireBlasterRifle;
                if (this.isEngi) soundString = EnforcerPlugin.Sounds.FireBungusRifle;

                Util.PlayScaledSound(soundString, base.gameObject, this.attackSpeedStat);

                if (base.isAuthority)
                {
                    float damage = FireBurstRifle.damageCoefficient * this.damageStat;
                    float force = 10;
                    float procCoefficient = 0.75f;
                    bool isCrit = base.RollCrit();

                    Ray aimRay = base.GetAimRay();

                    GameObject tracerEffect = FireBurstRifle.bulletTracer;
                    if (this.isStormtrooper) tracerEffect = EnforcerPlugin.EnforcerPlugin.laserTracer;
                    if (this.isEngi) tracerEffect = EnforcerPlugin.EnforcerPlugin.bungusTracer;

                    new BulletAttack
                    {
                        bulletCount = 1,
                        aimVector = aimRay.direction,
                        origin = aimRay.origin,
                        damage = damage,
                        damageColorIndex = DamageColorIndex.Default,
                        damageType = DamageType.Generic,
                        falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                        maxDistance = FireBurstRifle.range,
                        force = force,
                        hitMask = LayerIndex.CommonMasks.bullet,
                        minSpread = FireBurstRifle.minSpread,
                        maxSpread = FireBurstRifle.maxSpread,
                        isCrit = isCrit,
                        owner = base.gameObject,
                        muzzleName = muzzleString,
                        smartCollision = false,
                        procChainMask = default(ProcChainMask),
                        procCoefficient = procCoefficient,
                        radius = 0.75f,
                        sniper = false,
                        stopperMask = LayerIndex.CommonMasks.bullet,
                        weapon = null,
                        tracerEffectPrefab = tracerEffect,
                        spreadPitchScale = 0.25f,
                        spreadYawScale = 0.25f,
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

            if (base.fixedAge >= this.fireDuration && Time.time > this.lastFired)
            {
                FireBullet();
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
}