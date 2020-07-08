using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace EntityStates.Enforcer
{
    public class EnforcerMain : GenericCharacterMain
    {
        ShieldComponent sComp;
        bool toggle = false;

        public override void OnEnter()
        {
            base.OnEnter();

            sComp = base.characterBody.GetComponent<ShieldComponent>();
        }

        public override void Update()
        {
            base.Update();
            sComp.aimRay = base.GetAimRay();

            if (base.characterBody.HasBuff(EnforcerPlugin.EnforcerPlugin.jackBootsIndex))
            {
                characterBody.isSprinting = false;
                base.characterBody.SetAimTimer(0.2f);
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                toggle = !toggle;
            }

            if (toggle)
            {
                getList();
            }
        }

        public override void OnExit()
        {


            base.OnExit();
        }

        private void getList()
        {
            Collider[] array = Physics.OverlapSphere(base.characterBody.corePosition, 4f, LayerIndex.projectile.mask);
            int num = 0;
            while (num < array.Length)
            {
                ProjectileController pc = array[num].GetComponentInParent<ProjectileController>();
                if (pc)
                {
                    if (pc.teamFilter.teamIndex != TeamIndex.Player)
                    {
                        Ray aimRay = base.GetAimRay();
                        Vector3 aimSpot = (aimRay.origin + 100 * aimRay.direction) - pc.gameObject.transform.position;
                        FireProjectileInfo info = new FireProjectileInfo()
                        {
                            projectilePrefab = pc.gameObject,
                            position = pc.gameObject.transform.position,
                            rotation = base.characterBody.transform.rotation * Quaternion.FromToRotation(new Vector3(0, 0, 1), aimSpot),
                            owner = base.characterBody.gameObject,
                            damage = base.characterBody.damage * 10f,
                            force = 200f,
                            crit = base.RollCrit(),
                            damageColorIndex = DamageColorIndex.Default,
                            target = null,
                            speedOverride = 120f,
                            fuseOverride = -1f
                        };
                        ProjectileManager.instance.FireProjectile(info);

                        Destroy(pc.gameObject);
                    }
                }
                num++;
            }
        }
    }
}