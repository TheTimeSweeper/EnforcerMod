using RoR2;
using UnityEngine;

public class EnergyShieldControler : MonoBehaviour
{
    //messily being set from ShieldComponent.Update()
    public Vector3 shieldAimRayDirection = new Vector3(0, 0, 0);

    //private MeshCollider collider;
    //private MeshRenderer[] renderers;
    private float angle;

    public HealthComponent healthComponent;

    private bool isSet;

    void Start() {

        SetCharacterbody();


        //just deactivate the entire gameobject when the skill isn't active rather than doing all this

        //collider = GetComponentInChildren<MeshCollider>();
        //renderers = GetComponentsInChildren<MeshRenderer>();

        //collider.enabled = false;
        //for (int i = 0; i < 2; i++) {
        //    renderers[i].enabled = false;
        //}
    }

    private void SetCharacterbody() {
        if (isSet) return;
        isSet = true;

        CharacterBody characterBody = gameObject.AddComponent<CharacterBody>();
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
        characterBody._defaultCrosshairPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/SMGCrosshair");
        characterBody.aimOriginTransform = transform;
        characterBody.hullClassification = HullClassification.Human;
        characterBody.portraitIcon = null;
        characterBody.isChampion = false;
        characterBody.currentVehicle = null;
        characterBody.skinIndex = 0U;

        healthComponent = gameObject.AddComponent<HealthComponent>();
        healthComponent.health = 15f;
        healthComponent.shield = 0f;
        healthComponent.barrier = 0f;
        healthComponent.magnetiCharge = 0f;
        healthComponent.body = characterBody;
        healthComponent.dontShowHealthbar = false;
        healthComponent.globalDeathEventChanceCoefficient = 1f;

        HurtBoxGroup hurtBoxGroup = gameObject.AddComponent<HurtBoxGroup>();

        HurtBox componentInChildren = gameObject.GetComponentInChildren<MeshCollider>().gameObject.AddComponent<HurtBox>();
        componentInChildren.gameObject.layer = LayerIndex.entityPrecise.intVal;
        componentInChildren.healthComponent = healthComponent;
        componentInChildren.isBullseye = true;
        componentInChildren.damageModifier = HurtBox.DamageModifier.Normal;
        componentInChildren.hurtBoxGroup = hurtBoxGroup;
        componentInChildren.indexInGroup = 0;

        hurtBoxGroup.hurtBoxes = new HurtBox[]
        {
            componentInChildren
        };
    }

    private void Update()
    {
        Vector3 horizontal = new Vector3(shieldAimRayDirection.x, 0, shieldAimRayDirection.z);
        float sign = -1 * (shieldAimRayDirection.y / Mathf.Abs(shieldAimRayDirection.y));
        float theta = sign * Vector3.Angle(shieldAimRayDirection, horizontal);

        transform.Rotate(new Vector3(theta - angle, 0, 0));

        angle = theta;
    }

    public void Toggle() {

        healthComponent.health = healthComponent.fullHealth;

        //collider.enabled = !collider.enabled;
        //for (int i = 0; i < 2; i++)
        //{
        //    renderers[i].enabled = !renderers[i].enabled;
        //}
    }

    private void OnEnable()
    {
        if (!isSet) SetCharacterbody();

        healthComponent.health = healthComponent.fullHealth;
    }
}
