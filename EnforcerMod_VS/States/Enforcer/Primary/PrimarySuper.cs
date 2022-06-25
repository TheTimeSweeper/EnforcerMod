using RoR2;
using UnityEngine;
using EntityStates.Enforcer.NeutralSpecial;
using Modules;

namespace EntityStates.Enforcer {
    public class SuperShotgun : RiotShotgun
    {
        public enum shotType {
            NONE,
            BARREL_1,
            BARREL_2,
            SHIELD_SUPER,
        }

        public new float damageCoefficient = Config.superDamage.Value;
        public new float procCoefficient = 0.75f;
        public new float beefDurationNoShield = 0.15f;
        public new float beefDurationShield { // 0.4f
            get {
                return Mathf.Max(Config.superBeef.Value, 0.2f); //fuck you lol
            } 
        }

        public static float bulletForce = 100f;
        public static int bulletCount = 16;
        public new float bulletSpread = Config.superSpread.Value;// 6f

        public static float durationBaseMeasure = Config.superDuration.Value;
        public new float baseShieldDuration = durationBaseMeasure * 2;// = 2
        public new float baseDuration = durationBaseMeasure * 1.5f;
        public float shot1InterruptibleDuration = durationBaseMeasure * 1; //fuck you config and fuck me for doing it


        public shotType currentShot {
            get; set;
            //TODO: move currentshot from shieldcomponent to here via next states like a normal person
            //get => outer.GetComponent<ShieldComponent>().currentShot;
            //set => outer.GetComponent<ShieldComponent>().currentShot = value;
        }

        private bool droppedShell;

        //my shot differentiating code has been a mix of real proper lookups like this, but also random ifs and ternaries sprinkled th
        public float getBaseDuration() {
            switch (currentShot) {
                default:
                    return baseDuration;
                case shotType.SHIELD_SUPER:
                    return baseShieldDuration;
            }
        }

        public string getCurrentAnimation() {
            switch (currentShot) {
                default:
                    return "FireShotgun";
                case shotType.SHIELD_SUPER:
                    return "ShieldFireShotgun";
            }
        }

        public float getShieldStop() {
            switch (currentShot) {
                default:
                    return beefDurationNoShield;
                case shotType.SHIELD_SUPER:
                    return beefDurationShield;
            }
        }

        public override void OnEnter() {

            base.OnEnter();

            this.droppedShell = false;

            bool isShielded = base.HasBuff(Buffs.protectAndServeBuff) || base.HasBuff(Buffs.energyShieldBuff);

            switch(currentShot) {
                case shotType.NONE:
                case shotType.SHIELD_SUPER:
                    currentShot = shotType.BARREL_1;
                    break;
                case shotType.BARREL_1:
                    currentShot = shotType.BARREL_2;
                    break;
                case shotType.BARREL_2:
                    Debug.LogWarning("fucking how");
                    break;
            }

            if (isShielded)
                currentShot = shotType.SHIELD_SUPER;

            this.duration = getBaseDuration() / this.attackSpeedStat;
            this.attackStopDuration = getShieldStop() / this.attackSpeedStat;

            base.PlayAnimation("Gesture, Override", getCurrentAnimation(), "FireShotgun.playbackRate", this.duration);


            this.fireDuration = 0.05f * this.duration;
        }

        public override void FixedUpdate()
        {
            if (base.fixedAge >= 0.55f * this.duration && !this.droppedShell)
            {
                this.droppedShell = true;

                EnforcerWeaponComponent poopy = base.GetComponent<EnforcerWeaponComponent>();
                poopy.DropShell(-base.GetModelBaseTransform().transform.right * -Random.Range(6, 16));

                if(currentShot == shotType.SHIELD_SUPER || currentShot == shotType.BARREL_2)
                poopy.DropShell(-base.GetModelBaseTransform().transform.right * -Random.Range(6, 16));
            }

            base.FixedUpdate();

            if (fixedAge >= duration && isAuthority) {
                //assumes shot was not interrupted.
                currentShot = shotType.NONE;
            }
        }

        public override void FireBullet()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                this.muzzleString = "Muzzle";

                string soundString = "";

                bool isCrit = base.RollCrit();

                soundString = !isCrit ? Sounds.FireSuperShotgun : Sounds.FireSuperShotgunCrit;

                if (Config.classicShotgun.Value) soundString = Sounds.FireClassicShotgun;

                //if (base.characterBody.skinIndex == EnforcerPlugin.EnforcerPlugin.doomGuyIndex) soundString = EnforcerPlugin.Sounds.FireSuperShotgunDOOM;
                //if (this.isStormtrooper) soundString = EnforcerPlugin.Sounds.FireBlasterShotgun;
                //if (this.isEngi) soundString = EnforcerPlugin.Sounds.FireBungusSSG;

                Util.PlayAttackSpeedSound(soundString, EnforcerPlugin.VRAPICompat.IsLocalVRPlayer(characterBody) ? EnforcerPlugin.VRAPICompat.GetPrimaryMuzzleObject() : gameObject, this.attackSpeedStat);

                float recoilAmplitude = RiotShotgun.bulletRecoil / this.attackSpeedStat;

                if (base.HasBuff(Buffs.protectAndServeBuff) || base.HasBuff(Buffs.energyShieldBuff)) recoilAmplitude = RiotShotgun.shieldedBulletRecoil;

                base.AddRecoil(-0.4f * recoilAmplitude, -0.8f * recoilAmplitude, -0.3f * recoilAmplitude, 0.3f * recoilAmplitude);
                base.characterBody.AddSpreadBloom(4f);
                EffectManager.SimpleMuzzleFlash(Commando.CommandoWeapon.FireBarrage.effectPrefab, base.gameObject, this.muzzleString, false);

                if (base.isAuthority) {
                    float damage = this.damageCoefficient * this.damageStat;

                    GameObject tracerEffect = EnforcerPlugin.EnforcerModPlugin.bulletTracerSSG;

                    //if (this.isStormtrooper) tracerEffect = EnforcerPlugin.EnforcerPlugin.laserTracer;
                    //if (this.isEngi) tracerEffect = EnforcerPlugin.EnforcerPlugin.bungusTracer;

                    Ray aimRay = base.GetAimRay();

                    BulletAttack bulletAttack = new BulletAttack {
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
                        procCoefficient = this.procCoefficient,
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
                    bulletAttack.spreadPitchScale = 1f;
                    bulletAttack.spreadYawScale = currentShot == shotType.SHIELD_SUPER ? 1 : 1.4f;
                    bulletAttack.minSpread = 0f;
                    bulletAttack.maxSpread = this.bulletSpread / 2f; // radius / 2 does not equate to area / 2
                    bulletAttack.Fire();                             // ratio for actual equal areas come out to around 1.45, so dividing by higher than this results in proportionally tigher spread.
                                                                     // which is good, of course. just letting ya know so ya know, ya know?

                    bullets -= 4;
                    bulletAttack.bulletCount = 4;
                    bulletAttack.minSpread = 0f;
                    bulletAttack.maxSpread = this.bulletSpread;
                    bulletAttack.spreadPitchScale = 1f;
                    bulletAttack.spreadYawScale = currentShot == shotType.SHIELD_SUPER ? 1 : 1.7f;
                    bulletAttack.Fire();


                    //unshielded shots shoot 8 shots as above
                    //shielded shots shoot the additional 8 shots below
                    if (currentShot != shotType.SHIELD_SUPER)
                        return;

                    if (bullets > 0) {
                        bulletAttack.bulletCount = (uint)bullets;
                        bulletAttack.minSpread = this.bulletSpread / bulletAttack.spreadYawScale;
                        bulletAttack.maxSpread = this.bulletSpread;
                        bulletAttack.spreadPitchScale = 1f;
                        bulletAttack.spreadYawScale = 2.3f;
                        bulletAttack.Fire();
                    }

                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() {

            //first shot can be intterupted after 1 second by itself
            if(currentShot == shotType.BARREL_1 && fixedAge >= shot1InterruptibleDuration) {

                return InterruptPriority.Any;
            } else {

                //second shot and shield shot cannot be interrupted by self
                return InterruptPriority.Skill;
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