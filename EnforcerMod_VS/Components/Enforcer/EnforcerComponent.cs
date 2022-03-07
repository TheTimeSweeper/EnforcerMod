using EntityStates;
using Modules;
using RoR2;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class EnforcerComponent : MonoBehaviour
{
    static float maxSpeed = 0.1f;
    static float coef = 1; // affects how quickly it reaches max speed

    public EntityStateMachine drOctagonapus { get; set; }
    
    public bool isDeflecting { get; set; }

    public event Action onLaserHit = delegate { };

    public Transform origOrigin { get; set; }

    public bool isShielding = false;
    public Ray aimRay;
    public Vector3 shieldDirection = new Vector3(1,0,0);

    public bool beefStop;
    float initialTime = 0;

    private ChildLocator childLocator;

    private GameObject energyShield;
    private EnergyShieldControler energyShieldControler;

    private Transform _shieldPreview;
    private Transform _shieldParent;
    private float _shieldSize;
    private float _shieldSizeMultiplier = 1.2f;

    GameObject dummy;
    GameObject boyPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/LemurianBody");
    public static bool skateJump;

    private Transform head;

    public float shieldHealth {
        get => energyShieldControler.healthComponent.health;
    }

    void Start()
    {
        // dead.
        childLocator = GetComponentInChildren<ChildLocator>();
        head = childLocator.FindChild("Head");
        /*energyShield = childLocator.FindChild("EnergyShield").gameObject;

        energyShield.SetActive(true);// i don't know if the object has to be active to get the component but i'm playing it safe
        energyShieldControler = energyShield.GetComponentInChildren<EnergyShieldControler>();
        energyShield = energyShieldControler?.gameObject;
        energyShield.SetActive(false);*/
        
        if(drOctagonapus == null) {
            drOctagonapus = EntityStateMachine.FindByCustomName(gameObject, "EnforcerParry");
        }
    }

    void FixedUpdate() {

        aimShield();

        if (energyShieldControler) energyShieldControler.shieldAimRayDirection = aimRay.direction;
    }

    void LateUpdate() {

        if (Config.headSize.Value > 2)
        {
            head.transform.localScale = Vector3.one * Config.headSize.Value;
            //magic numbers based on head bone's default position
            head.transform.localPosition = new Vector3(0, 0.0535f + 0.0450f * Config.headSize.Value, 0);
        }
    }

    private void aimShield() {

        float time = Time.fixedTime - initialTime;
        
        Vector3 cross = Vector3.Cross(aimRay.direction, shieldDirection);
        Vector3 turnDirection = Vector3.Cross(shieldDirection, cross);

        float turnSpeed = maxSpeed * (1 - Mathf.Exp(-1 * coef * time));

        shieldDirection += turnSpeed * turnDirection.normalized;
        shieldDirection = shieldDirection.normalized;

        Vector3 difference = aimRay.direction - shieldDirection;
        if (difference.magnitude < 0.05) {
            initialTime = Time.fixedTime;
        }

        //displayShieldPreviewCube();
    }

    public void ToggleEnergyShield(bool shieldToggle)
    {
        if (energyShield) energyShield.SetActive(shieldToggle);
    }

    public void invokeOnLaserHitEvent() {

        onLaserHit?.Invoke();
    }

    public void OnTriggerStay (Collider other) {
        if (other.GetComponent<JumpVolume>()) {
            skateJump = false;
        }
    }

    public static event Action<bool> BlockedGet = delegate { };

    public void AttackBlocked(bool flag)
    {
        BlockedGet(flag);
    }
}