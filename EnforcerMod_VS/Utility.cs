using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace EntityStates.Enforcer
{
    public class TearGas : BaseSkillState
    {
        public static float baseDuration = 0.5f;
        public static float damageCoefficient = 1.5f;
        public static float procCoefficient = 0.6f;
        public static float blastRadius = 5f;
        public static float bulletRecoil = 1f;

        public static string muzzleString = "Muzzle";

        private float duration;
        public GameObject grenadePrefab = EnforcerPlugin.Assets.grenade;

        private ChildLocator childLocator;
        private Animator animator;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = TearGas.baseDuration / this.attackSpeedStat;
            this.childLocator = base.GetModelTransform().GetComponent<ChildLocator>();
            this.animator = base.GetModelAnimator();

            this.animator.SetBool("gunUp", true);

            //base.PlayAnimation("Gesture, Override", "FireShotgun", "FireShotgun.playbackRate", this.duration);

            Util.PlaySound(EnforcerPlugin.Sounds.LaunchTearGas, base.gameObject);

            base.AddRecoil(-2f * TearGas.bulletRecoil, -3f * TearGas.bulletRecoil, -1f * TearGas.bulletRecoil, 1f * TearGas.bulletRecoil);
            base.characterBody.AddSpreadBloom(0.33f * TearGas.bulletRecoil);
            EffectManager.SimpleMuzzleFlash(Commando.CommandoWeapon.FirePistol.effectPrefab, base.gameObject, TearGas.muzzleString, false);

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
                    damageTypeOverride = DamageType.WeakOnHit,
                    force = 0,
                    owner = base.gameObject,
                    position = childLocator.FindChild(TearGas.muzzleString).position,
                    procChainMask = default(ProcChainMask),
                    projectilePrefab = EnforcerPlugin.EnforcerPlugin.projectilePrefab,
                    rotation = Quaternion.LookRotation(base.GetAimRay().direction),
                    useFuseOverride = false,
                    useSpeedOverride = false,
                    
                };
                ProjectileManager.instance.FireProjectile(info);
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            this.animator.SetBool("gunUp", false);
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