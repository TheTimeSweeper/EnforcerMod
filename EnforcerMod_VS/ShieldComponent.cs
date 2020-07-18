using RoR2;
using UnityEngine;

public class ShieldComponent : MonoBehaviour
{
    static float maxSpeed = 0.1f;
    static float coef = 1; // affects how quickly it reaches max speed

    public bool isShielding = false;
    public Ray aimRay;
    public Vector3 shieldDirection = new Vector3(1,0,0);
    float initialTime = 0;

    public static GameObject bulletTracerEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/Tracers/TracerCommandoShotgun");

    private EnergyShieldControler energyShieldControler;

    GameObject dummy;
    GameObject boyPrefab = Resources.Load<GameObject>("Prefabs/CharacterBodies/LemurianBody");

    void Start()
    {
        //enough of this tomfoolery
        //dummy = UnityEngine.Object.Instantiate<GameObject>(boyPrefab, aimRay.origin, Quaternion.LookRotation(shieldDirection));
        energyShieldControler = GetComponentInChildren<EnergyShieldControler>();
    }

    void Update()
    {
        energyShieldControler.aimRayDirection = aimRay.direction;

        float time = Time.fixedTime - initialTime;

        Vector3 cross = Vector3.Cross(aimRay.direction, shieldDirection);
        Vector3 turnDirection = Vector3.Cross(shieldDirection, cross);

        float turnSpeed = maxSpeed * (1 - Mathf.Exp(-1 * coef * time));

        shieldDirection += turnSpeed * turnDirection.normalized;
        shieldDirection = shieldDirection.normalized;

        Vector3 difference = aimRay.direction - shieldDirection;
        if (difference.magnitude < 0.05)
        {
            initialTime = Time.fixedTime;
        }

        if (dummy)
        {
            var hc = dummy.GetComponent<HealthComponent>();
            if (hc && hc.health <= 0)
            {
                //stop this madness i swear to god
                //respawnDummy();
            }

            dummy.transform.position = aimRay.origin + shieldDirection;
        }

        /*BulletAttack bullet = new RoR2.BulletAttack
        {
            bulletCount = 1,
            aimVector = shieldDirection,
            origin = aimRay.origin,
            damage = 1,
            damageColorIndex = DamageColorIndex.Default,
            damageType = DamageType.Generic,
            falloffModel = BulletAttack.FalloffModel.None,
            maxDistance = 48,
            force = 1,
            hitMask = LayerIndex.CommonMasks.bullet,
            minSpread = 0,
            maxSpread = 12f,
            isCrit = false,
            owner = base.gameObject,
            muzzleName = "hey",
            smartCollision = false,
            procChainMask = default(ProcChainMask),
            procCoefficient =2,
            radius = 0.5f,
            sniper = false,
            stopperMask = LayerIndex.background.collisionMask,
            weapon = null,
            tracerEffectPrefab = bulletTracerEffectPrefab,
            spreadPitchScale = 0.5f,
            spreadYawScale = 0.5f,
            queryTriggerInteraction = QueryTriggerInteraction.UseGlobal
        };*/
        //bullet.Fire();

        //RoR2.Chat.AddMessage("---------------");
        //RoR2.Chat.AddMessage("Aim Direction:    " + aimRay.direction.x.ToString() + ", " + aimRay.direction.y.ToString() + ", " + aimRay.direction.z.ToString());
        //RoR2.Chat.AddMessage("Shield Direction: " + shieldDirection.x.ToString() + ", " + shieldDirection.y.ToString() + ", " + shieldDirection.z.ToString());
    }

    private void respawnDummy()
    {
        dummy = UnityEngine.Object.Instantiate<GameObject>(boyPrefab, aimRay.origin, Quaternion.LookRotation(shieldDirection));
    }

    public void toggleEngergyShield()
    {
        energyShieldControler.Toggle();
    }
}