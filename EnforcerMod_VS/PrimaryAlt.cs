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

    //scrap all of this
    /*
    public class FireAssaultRifle : AssaultRifleState
    {
        public static float damageCoefficient = EnforcerPlugin.EnforcerPlugin.rifleDamage.Value;
        public static float procCoefficient = EnforcerPlugin.EnforcerPlugin.rifleProcCoefficient.Value;
        public static float shieldProcCoefficient = EnforcerPlugin.EnforcerPlugin.rifleProcCoefficientAlt.Value;
        public static float bulletForce = 5f;
        public static float recoilAmplitude = 1.25f;
        public static float shieldRecoilAmplitude = 0.2f;
        public static float spreadBloom = 0.085f;
        public static float shieldSpreadBloom = 0.025f;
        public static float baseFireInterval = EnforcerPlugin.EnforcerPlugin.rifleFireInterval.Value;
        public static int baseBulletCount = EnforcerPlugin.EnforcerPlugin.rifleBaseBulletCount.Value;
        public static float bulletRange = EnforcerPlugin.EnforcerPlugin.rifleRange.Value;
        public static float bulletRadius = 0.1f;
        public static float minSpread = 0;
        public static float maxSpread = EnforcerPlugin.EnforcerPlugin.rifleSpread.Value;

        public static GameObject bulletTracer = Resources.Load<GameObject>("Prefabs/Effects/Tracers/TracerCommandoDefault");

        private float fireTimer;
        private Transform muzzleVfxTransform;
        private float baseFireRate;
        private float baseBulletsPerSecond;
        private bool isCrit;

        public override void OnEnter()
        {
            base.OnEnter();

            if (this.muzzleTransform && MinigunFire.muzzleVfxPrefab)
            {
                this.muzzleVfxTransform = Object.Instantiate<GameObject>(MinigunFire.muzzleVfxPrefab, this.muzzleTransform).transform;

                if (this.muzzleVfxTransform.Find("Ring, Dark")) Destroy(this.muzzleVfxTransform.Find("Ring, Dark").gameObject);
                if (this.muzzleVfxTransform.Find("Ray")) Destroy(this.muzzleVfxTransform.Find("Ray").gameObject);

                this.muzzleTransform.transform.localScale *= 0.35f;
                this.muzzleTransform.GetComponentInChildren<Light>().range *= 0.25f;
            }

            this.UpdateFireRate();
        }

        private void UpdateFireRate()
        {
            float fireInterval = FireAssaultRifle.baseFireInterval;

            this.baseFireRate = 1f / fireInterval;
            this.baseBulletsPerSecond = ((float)FireAssaultRifle.baseBulletCount * 2f) * this.baseFireRate;
        }

        public override void OnExit()
        {
            if (this.muzzleVfxTransform)
            {
                EntityState.Destroy(this.muzzleVfxTransform.gameObject);
                this.muzzleVfxTransform = null;
            }

            base.OnExit();
        }

        private void OnFireShared()
        {
            string soundString = "";

            if (EnforcerMain.shotgunToggle)
            {
                soundString = EnforcerPlugin.Sounds.FireAssaultRifleFast;
            }
            else
            {
                soundString = EnforcerPlugin.Sounds.FireAssaultRifleSlow;
            }

            if (base.characterBody.skinIndex == EnforcerPlugin.EnforcerPlugin.stormtrooperIndex) soundString = EnforcerPlugin.Sounds.FireBlasterRifle;
            if (base.characterBody.skinIndex == EnforcerPlugin.EnforcerPlugin.engiIndex) soundString = MiniMushroom.SporeGrenade.attackSoundString;

            Util.PlayScaledSound(soundString, base.gameObject, this.attackSpeedStat);

            if (base.isAuthority)
            {
                this.OnFireAuthority();
            }
        }

        private void OnFireAuthority()
        {
            if (base.HasBuff(EnforcerPlugin.EnforcerPlugin.jackBoots) || base.HasBuff(EnforcerPlugin.EnforcerPlugin.energyShieldBuff))
            {
                base.PlayAnimation("RightArm, Override", "FireShotgunShielded", "FireShotgun.playbackRate", this.attackSpeedStat);
            }
            else
            {
                base.PlayAnimation("RightArm, Override", "FireRifle", "FireShotgun.playbackRate", this.attackSpeedStat);
            }

            this.critStat = base.characterBody.crit;
            this.isCrit = this.RollCrit();

            float bloom = FireAssaultRifle.spreadBloom;
            float recoil = FireAssaultRifle.recoilAmplitude;
            if (base.characterBody.HasBuff(EnforcerPlugin.EnforcerPlugin.jackBoots))
            {
                bloom = FireAssaultRifle.shieldSpreadBloom;
                recoil = FireAssaultRifle.shieldRecoilAmplitude;
            }

            base.AddRecoil(-0.5f * FireAssaultRifle.recoilAmplitude, -0.5f * FireAssaultRifle.recoilAmplitude, -0.5f * FireAssaultRifle.recoilAmplitude, 0.5f * FireAssaultRifle.recoilAmplitude);
            base.characterBody.AddSpreadBloom(FireAssaultRifle.spreadBloom);

            float damage = FireAssaultRifle.damageCoefficient * this.damageStat;
            float force = FireAssaultRifle.bulletForce / this.baseBulletsPerSecond;

            //unique tracer for stormtrooper skin
            GameObject tracerEffect = FireAssaultRifle.bulletTracer;

            if (base.characterBody.skinIndex == EnforcerPlugin.EnforcerPlugin.stormtrooperIndex) tracerEffect = EnforcerPlugin.EnforcerPlugin.laserTracer;
            if (base.characterBody.skinIndex == EnforcerPlugin.EnforcerPlugin.engiIndex) tracerEffect = EnforcerPlugin.EnforcerPlugin.bungusTracer;

            Ray aimRay = base.GetAimRay();

            string muzzleString = FireAssaultRifle.muzzleName;
            if (base.characterBody.skinIndex == EnforcerPlugin.EnforcerPlugin.stormtrooperIndex) muzzleString = "BlasterRifleMuzzle";

            int bullets = FireAssaultRifle.baseBulletCount;
            float procCoeff = FireAssaultRifle.procCoefficient;

            if (base.HasBuff(EnforcerPlugin.EnforcerPlugin.jackBoots))
            {
                bullets++;
                procCoeff = FireAssaultRifle.shieldProcCoefficient;
            }

            new BulletAttack
            {
                bulletCount = (uint)bullets,
                aimVector = aimRay.direction,
                origin = aimRay.origin,
                damage = damage,
                damageColorIndex = DamageColorIndex.Default,
                damageType = DamageType.Generic,
                falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                maxDistance = FireAssaultRifle.bulletRange,
                force = force,
                hitMask = LayerIndex.CommonMasks.bullet,
                minSpread = FireAssaultRifle.minSpread,
                maxSpread = FireAssaultRifle.maxSpread,
                isCrit = this.isCrit,
                owner = base.gameObject,
                muzzleName = muzzleString,
                smartCollision = false,
                procChainMask = default(ProcChainMask),
                procCoefficient = procCoeff,
                radius = FireAssaultRifle.bulletRadius,
                sniper = false,
                stopperMask = LayerIndex.CommonMasks.bullet,
                weapon = null,
                tracerEffectPrefab = tracerEffect,
                spreadPitchScale = 0.35f,
                spreadYawScale = 0.35f,
                queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                hitEffectPrefab = MinigunFire.bulletHitEffectPrefab,
                HitEffectNormal = MinigunFire.bulletHitEffectNormal
            }.Fire();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            this.UpdateFireRate();

            this.fireTimer -= Time.fixedDeltaTime;

            if (this.fireTimer <= 0f)
            {
                this.attackSpeedStat = this.characterBody.attackSpeed;

                float num = FireAssaultRifle.baseFireInterval / this.attackSpeedStat;

                this.fireTimer += num;
                this.OnFireShared();
            }

            if (base.isAuthority && !base.skillButtonState.down)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }
    }

    public class AssaultRifleState : BaseSkillState
    {
        public static string muzzleName = "RifleMuzzle";

        protected Transform muzzleTransform;
        protected Animator animator;

        public override void OnEnter()
        {
            base.OnEnter();
            string muzzleString = FireAssaultRifle.muzzleName;
            if (base.characterBody.skinIndex == EnforcerPlugin.EnforcerPlugin.stormtrooperIndex) muzzleString = "BlasterRifleMuzzle";
            this.muzzleTransform = base.FindModelChild(muzzleString);
            this.animator = base.GetModelAnimator();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            base.StartAimMode(0.5f, false);
            base.characterBody.isSprinting = false;
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        protected ref InputBankTest.ButtonState skillButtonState
        {
            get
            {
                return ref base.inputBank.skill1;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
    */

    public class HammerSwing : BaseSkillState {
        //this is just shield bash code for now, change it to an overlap attack eventually
        // the hitbox for it is already set up im just lazy rn
        public static string hitboxString = "HammerModel2";
        public static float baseDuration = 0.8f;
        public static float damageCoefficient = 3.5f;
        public static float procCoefficient = 1f;
        public static float blastRadius = 6f;
        public static float deflectRadius = 3f;
        public static float recoilAmplitude = 1f;
        public static float parryInterval = 0.12f;

        private float duration;
        private float fireDuration;
        private Ray aimRay;
        private BlastAttack blastAttack;
        private ChildLocator childLocator;
        private bool hasFired;

        public override void OnEnter() {
            base.OnEnter();

            duration = baseDuration / attackSpeedStat;
            fireDuration = duration * 0.35f;
            aimRay = GetAimRay();
            hasFired = false;
            childLocator = GetModelChildLocator();

            StartAimMode(aimRay, 2f, false);

            bool grounded = characterMotor.isGrounded;

            PlayAnimation("Gesture, Override", "HammerSwing", "HammerSwing.playbackRate", duration);

            if (childLocator.FindChild("Hammer")) {
                var anim = GetModelChildLocator().FindChild("Hammer").GetComponentInChildren<Animator>();
                if (anim) {
                    anim.Play("HammerSwing");
                    anim.SetFloat("HammerSwing.playbackRate", duration);
                }
            }

            Util.PlayScaledSound(EnforcerPlugin.Sounds.ShieldBash, gameObject, 0.5f + attackSpeedStat);
        }

        private void FireBlast() {
            if (!hasFired) {
                hasFired = true;

                //EffectManager.SimpleMuzzleFlash(EnforcerPlugin.Assets.shieldBashFX, base.gameObject, hitboxString, true);

                if (isAuthority) {
                    AddRecoil(-0.5f * recoilAmplitude * 3f, -0.5f * recoilAmplitude * 3f, -0.5f * recoilAmplitude * 8f, 0.5f * recoilAmplitude * 3f);

                    Vector3 center = childLocator.FindChild(hitboxString).position;

                    blastAttack = new BlastAttack();
                    blastAttack.radius = blastRadius;
                    blastAttack.procCoefficient = procCoefficient;
                    blastAttack.position = center;
                    blastAttack.attacker = gameObject;
                    blastAttack.crit = Util.CheckRoll(characterBody.crit, characterBody.master);
                    blastAttack.baseDamage = characterBody.damage * damageCoefficient;
                    blastAttack.falloffModel = BlastAttack.FalloffModel.None;
                    blastAttack.baseForce = 0f;
                    blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
                    blastAttack.damageType = DamageType.Generic;
                    blastAttack.attackerFiltering = AttackerFiltering.NeverHit;
                    blastAttack.impactEffect = BeetleGuardMonster.GroundSlam.hitEffectPrefab.GetComponent<EffectComponent>().effectIndex;

                    blastAttack.Fire();
                }
            }
        }

        public override void OnExit() {
            base.OnExit();
        }

        public override void FixedUpdate() {
            base.FixedUpdate();

            if (fixedAge >= fireDuration) {
                FireBlast();
            }

            if (fixedAge >= duration && isAuthority) {
                outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.PrioritySkill;
        }
    }
}