using RoR2;
using UnityEngine;

public class ShieldComponent : MonoBehaviour
{
    static float maxSpeed = 0.1f;
    static float coef = 1; // affects how quickly it reaches max speed

    public bool isAlternate = false;
    public bool isShielding = false;
    public Ray aimRay;
    public Vector3 shieldDirection = new Vector3(1,0,0);
    float initialTime = 0;

    private EnergyShieldControler energyShieldControler;
    private HealthComponent healthComponent;
    public float shieldHealth
    {
        get { return healthComponent.health; }
        set { ; }
    }

    private Light[] lights;
    private int lightCounter = 201;

    void Start()
    {
        energyShieldControler = GetComponentInChildren<EnergyShieldControler>();

        CharacterBody characterBody = energyShieldControler.gameObject.AddComponent<CharacterBody>(); 
        characterBody.bodyIndex = -1;
        characterBody.name = "EnergyShield";
        characterBody.baseNameToken = "";
        characterBody.subtitleNameToken = "";
        characterBody.bodyFlags = CharacterBody.BodyFlags.ImmuneToExecutes;
        characterBody.rootMotionInMainState = false;
        characterBody.mainRootSpeed = 0;
        characterBody.baseMaxHealth = 15;
        characterBody.levelMaxHealth = 0;
        characterBody.baseRegen = 0;
        characterBody.levelRegen = 0f;
        characterBody.baseMaxShield = 0;
        characterBody.levelMaxShield = 0;
        characterBody.baseMoveSpeed = 0;
        characterBody.levelMoveSpeed = 0;
        characterBody.baseAcceleration = 0;
        characterBody.baseJumpPower = 0;
        characterBody.levelJumpPower = 0;
        characterBody.baseDamage = 0;
        characterBody.levelDamage = 0f;
        characterBody.baseAttackSpeed = 0;
        characterBody.levelAttackSpeed = 0;
        characterBody.baseCrit = 0;
        characterBody.levelCrit = 0;
        characterBody.baseArmor = 0;
        characterBody.levelArmor = 0;
        characterBody.baseJumpCount = 0;
        characterBody.sprintingSpeedMultiplier = 0;
        characterBody.wasLucky = false;
        characterBody.hideCrosshair = true;
        characterBody.crosshairPrefab = Resources.Load<GameObject>("Prefabs/Crosshair/SMGCrosshair");
        characterBody.aimOriginTransform = transform;
        characterBody.hullClassification = HullClassification.Human;
        characterBody.portraitIcon = null;
        characterBody.isChampion = false;
        characterBody.currentVehicle = null;
        characterBody.skinIndex = 0U;

        healthComponent = energyShieldControler.gameObject.AddComponent<HealthComponent>();
        healthComponent.health = 15f;
        healthComponent.shield = 0f;
        healthComponent.barrier = 0f;
        healthComponent.magnetiCharge = 0f;
        healthComponent.body = characterBody;
        healthComponent.dontShowHealthbar = false;
        healthComponent.globalDeathEventChanceCoefficient = 1f;

        HurtBoxGroup hurtBoxGroup = energyShieldControler.gameObject.AddComponent<HurtBoxGroup>();

        HurtBox componentInChildren = energyShieldControler.GetComponentInChildren<MeshCollider>().gameObject.AddComponent<HurtBox>();
        componentInChildren.gameObject.layer = LayerIndex.entityPrecise.intVal;
        componentInChildren.healthComponent = healthComponent;
        componentInChildren.isBullseye = true;
        componentInChildren.damageModifier = HurtBox.DamageModifier.Normal;
        componentInChildren.hurtBoxGroup = hurtBoxGroup;
        componentInChildren.indexInGroup = 0;

        lights = GetComponentsInChildren<Light>();
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

        if (lightCounter < 100)
        {
            if (lightCounter % 10 == 0)
            {
                lights[0].enabled = !lights[0].enabled;
                lights[1].enabled = !lights[1].enabled;
            }

            lightCounter++;
        }
        else
        {
            lights[0].enabled = false;
            lights[1].enabled = false;
        }
    }

    public void toggleEngergyShield()
    {
        healthComponent.health = healthComponent.fullHealth;
        energyShieldControler.Toggle();
    }

    public void flashLights()
    {
        lightCounter = 0;
    }
}