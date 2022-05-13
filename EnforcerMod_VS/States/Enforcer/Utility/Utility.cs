using RoR2;
using RoR2.Projectile;
using UnityEngine;
using EntityStates.Toolbot;
using Modules;

namespace EntityStates.Enforcer {

    public class AimTearGas : AimThrowableBase
    {
        private AimStunDrone goodState;

        public override void OnEnter()
        {
            if (goodState == null) goodState = new AimStunDrone();

            this.maxDistance = 48;
            this.rayRadius = 2f;
            this.arcVisualizerPrefab = goodState.arcVisualizerPrefab;
            this.projectilePrefab = EnforcerPlugin.EnforcerModPlugin.tearGasProjectilePrefab;
            this.endpointVisualizerPrefab = goodState.endpointVisualizerPrefab;
            this.endpointVisualizerRadiusScale = 4f;
            this.setFuse = false;
            this.damageCoefficient = 0f;
            this.baseMinimumDuration = 0.2f;
            this.projectileBaseSpeed = 80;

            base.OnEnter();

            base.characterBody.aimOriginTransform = base.GetModelChildLocator().FindChild("GrenadeAimOrigin");

            base.PlayAnimation("Gesture, Override", "BufferEmpty");
            base.PlayAnimation("Grenade, Override", "AimGrenade");
        }

        public override void FixedUpdate()
        {
            base.characterBody.SetAimTimer(0.25f);
            this.fixedAge += Time.fixedDeltaTime;

            bool flag = false;

            if (base.isAuthority && !this.KeyIsDown() && base.fixedAge >= this.minimumDuration) flag = true;
            if (base.characterBody && base.characterBody.isSprinting) flag = true;

            if (flag)
            {
                this.UpdateTrajectoryInfo(out this.currentTrajectoryInfo);

                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            base.PlayAnimation("Grenade, Override", "ThrowGrenade");

            Util.PlaySound(Sounds.NemesisGrenadeThrow, base.gameObject);

            base.AddRecoil(-2f * TearGas.bulletRecoil, -3f * TearGas.bulletRecoil, -1f * TearGas.bulletRecoil, 1f * TearGas.bulletRecoil);
            base.characterBody.AddSpreadBloom(0.33f * TearGas.bulletRecoil);

            GetComponent<EnforcerComponent>().ResetAimOrigin(base.characterBody);
            //EffectManager.SimpleMuzzleFlash(Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, base.gameObject, TearGas.muzzleString, false);
        }
    }

    public class TearGas : BaseSkillState
    {
        public static float baseDuration = 0.5f;
        public static float blastRadius = 4f;
        public static float bulletRecoil = 1f;

        public static string muzzleString = "GrenadeMuzzle";

        private float duration;
        private ChildLocator childLocator;
        private Animator animator;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = TearGas.baseDuration / this.attackSpeedStat;
            this.childLocator = base.GetModelTransform().GetComponent<ChildLocator>();
            this.animator = base.GetModelAnimator();

            base.PlayAnimation("Grenade, Override", "ThrowGrenade");

            Util.PlaySound(Sounds.NemesisGrenadeThrow, base.gameObject);

            base.AddRecoil(-2f * TearGas.bulletRecoil, -3f * TearGas.bulletRecoil, -1f * TearGas.bulletRecoil, 1f * TearGas.bulletRecoil);
            base.characterBody.AddSpreadBloom(0.33f * TearGas.bulletRecoil);
            //EffectManager.SimpleMuzzleFlash(Commando.CommandoWeapon.FirePistol.effectPrefab, base.gameObject, TearGas.muzzleString, false);

            if (base.isAuthority)
            {
                /*ok guys what the fuck?
                GameObject grenade = UnityEngine.Object.Instantiate<GameObject>(grenadePrefab, childLocator.FindChild(muzzleString).position, Quaternion.LookRotation(aimRay.direction));

                Rigidbody rig = grenade.GetComponent<Rigidbody>();
                rig.velocity = 50 * aimRay.direction;

                GrenadeController grc = grenade.GetComponentInChildren<GrenadeController>();
                BlastAttack blastAttack = new BlastAttack();
                blastAttack.radius = StunGrenade.blastRadius;
                blastAttack.procCoefficient = StunGrenade.procCoefficient;
                blastAttack.position = transform.position;
                blastAttack.attacker = base.gameObject;
                blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
                blastAttack.baseDamage = base.characterBody.damage * StunGrenade.damageCoefficient;
                blastAttack.falloffModel = BlastAttack.FalloffModel.SweetSpot;
                blastAttack.baseForce = 3f;
                blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
                blastAttack.damageType = DamageType.Stun1s;
                blastAttack.attackerFiltering = AttackerFiltering.NeverHit;
                grc.blastAttack = blastAttack;*/
                //FireProjectileInfo 

                //OH MY FUCKING GOD YOU RETARD THAT'S NOT THIS WORKS\\
                //At least mine didn't flood the console with null references >:^(\\
                //literally 

                // holy shit you're both fucking retarded what are you doing my god
                Ray aimRay = base.GetAimRay();
                FireProjectileInfo info = new FireProjectileInfo()
                {
                    crit = false,
                    damage = 0,
                    damageColorIndex = DamageColorIndex.Default,
                    damageTypeOverride = DamageType.Stun1s,
                    force = 0,
                    owner = base.gameObject,
                    position = childLocator.FindChild(TearGas.muzzleString).position,
                    procChainMask = default(ProcChainMask),
                    projectilePrefab = EnforcerPlugin.EnforcerModPlugin.tearGasProjectilePrefab,
                    rotation = Quaternion.LookRotation(base.GetAimRay().direction),
                    useFuseOverride = false,
                    useSpeedOverride = false,
                    target = null
                };
                ProjectileManager.instance.FireProjectile(info);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}