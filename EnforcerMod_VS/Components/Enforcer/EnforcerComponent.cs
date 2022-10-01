using EntityStates;
using Modules;
using Modules.Characters;
using RoR2;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using static RoR2.CameraTargetParams;

public class EnforcerComponent : MonoBehaviour
{
    protected CharacterCameraParamsData shieldCameraParams = new CharacterCameraParamsData() {
        maxPitch = 70,
        minPitch = -70,
        pivotVerticalOffset = EnforcerSurvivor.instance.bodyInfo.cameraParamsVerticalOffset,
        idealLocalCameraPos = shieldCameraPosition,
        wallCushion = 0.1f,
    };

    public static CameraParamsOverrideHandle camOverrideHandle;

    public static Vector3 shieldCameraPosition = new Vector3(2.3f, -1.0f, -6.5f);

    static float maxSpeed = 0.1f;
    static float coef = 1; // affects how quickly it reaches max speed

    public EntityStateMachine drOctagonapus { get; set; }
    
    public bool isDeflecting { get; set; }

    public event Action onLaserHit = delegate { };

    public Transform origOrigin { get; set; }

    private bool _isShielding;
    public bool isShielding {
        get => _isShielding;
        set {
            _isShielding = value;
            toggleShieldCamera(value);
        }
    }


    public Ray aimRay;
    public Vector3 shieldDirection = new Vector3(1,0,0);

    public bool beefStop;
    float initialTime = 0;

    public ChildLocator childLocator;
    public CameraTargetParams cameraShit2;

    private GameObject energyShield;
    public EnergyShieldControler energyShieldControler;

    private Transform _shieldPreview;
    private Transform _shieldParent;
    private float _shieldSize;
    private float _shieldSizeMultiplier = 1.2f;
    private MeshRenderer shieldMeshVR;

    GameObject dummy;
    GameObject boyPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/LemurianBody");
    public static bool skateJump;

    private Transform head;

    private CharacterBody enforcerBody;

    public float shieldHealth {
        get => energyShieldControler.healthComponent.health;
    }

    public void Init()
    {
        // dead.
        childLocator = GetComponentInChildren<ChildLocator>();
        head = childLocator.FindChild("Head");

        cameraShit2 = GetComponentInChildren<CameraTargetParams>();

        /*energyShield = childLocator.FindChild("EnergyShield").gameObject;

        energyShield.SetActive(true);// i don't know if the object has to be active to get the component but i'm playing it safe
        energyShieldControler = energyShield.GetComponentInChildren<EnergyShieldControler>();
        energyShield = energyShieldControler?.gameObject;
        energyShield.SetActive(false);*/
    }
    void Start () {

        if (drOctagonapus == null) {
            drOctagonapus = EntityStateMachine.FindByCustomName(gameObject, "EnforcerParry");
        }
        if (enforcerBody == null) {
            enforcerBody = GetComponent<CharacterBody>();
        }
    }

    void FixedUpdate() {

        Vector3 shieldAimDirection;

        if (EnforcerPlugin.VRAPICompat.IsLocalVRPlayer(enforcerBody))
        {
            shieldAimDirection = GetVRShieldDirection();
        }
        else
        {
            shieldAimDirection = aimRay.direction;
        }

        aimShield(shieldAimDirection);

        if (energyShieldControler) energyShieldControler.shieldAimRayDirection = shieldAimDirection;
    }

    void LateUpdate() {

        if (Config.headSize.Value > 2)
        {
            head.transform.localScale = Vector3.one * Config.headSize.Value;
            //magic numbers based on head bone's default position
            head.transform.localPosition = new Vector3(0, 0.0535f + 0.0450f * Config.headSize.Value, 0);
        }

        if (EnforcerPlugin.VRAPICompat.IsLocalVRPlayer(enforcerBody) && Config.translucentVRShield.Value)
        {
            UpdateVRShieldTransparency();
        }
    }

    private void UpdateVRShieldTransparency()
    {
        VRAPI.MotionControls.HandController shieldHand = VRAPI.MotionControls.nonDominantHand;
        if (shieldHand == null) return;

        shieldHand.rendererInfos[0].defaultMaterial.SetFloat(CommonShaderProperties._Fade, enforcerBody.HasBuff(Buffs.protectAndServeBuff) ? 0.6f : 0.3f);
    }

    public void ResetAimOrigin(CharacterBody characterBody) {

        characterBody.aimOriginTransform = origOrigin;
    }

    private void toggleShieldCamera(bool shieldIsUp) {

        //shield mode camera stuff
        if (shieldIsUp) {

            CameraParamsOverrideRequest request = new CameraParamsOverrideRequest {
                cameraParamsData = shieldCameraParams,
                priority = 0,
            };

            camOverrideHandle = cameraShit2.AddParamsOverride(request, 0.6f);
        } else {

            for (int i = cameraShit2.cameraParamsOverrides.Count - 1; i >= 0; i--) {

                camOverrideHandle.target = cameraShit2.cameraParamsOverrides[i];

                cameraShit2.RemoveParamsOverride(camOverrideHandle, 0.3f);
            }

        }
    }

    private void aimShield(Vector3 aimDirection)
    {
        bool isInVR = EnforcerPlugin.VRAPICompat.IsLocalVRPlayer(enforcerBody);

        float time = Time.fixedTime - initialTime;

        Vector3 cross = Vector3.Cross(aimDirection, shieldDirection);
        Vector3 turnDirection = Vector3.Cross(shieldDirection, cross);

        //Instant turnning in VR in order to actually block with shield
        float turnSpeed = (isInVR)? 1 : maxSpeed * (1 - Mathf.Exp(-1 * coef * time));

        shieldDirection += turnSpeed * turnDirection.normalized;
        shieldDirection = shieldDirection.normalized;

        if (isInVR && enforcerBody.HasBuff(Buffs.protectAndServeBuff))
        {
            FaceTowardsVRShield();
        }

        Vector3 difference = aimDirection - shieldDirection;
        if (difference.magnitude < 0.05) {
            initialTime = Time.fixedTime;
        }

        //displayShieldPreviewCube();
    }

    private void FaceTowardsVRShield()
    {
        if (enforcerBody == null) return;

        Vector3 forwardDirection = shieldDirection;
        forwardDirection.y = 0;
        forwardDirection.Normalize();

        enforcerBody.characterDirection.forward = forwardDirection;
    }

    private Vector3 GetVRShieldDirection()
    {
        return VRAPI.MotionControls.nonDominantHand.GetMuzzleByIndex(1).forward;
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

    public static event Action<GameObject> BlockedGet = delegate { };

    public void AttackBlocked(GameObject gameObject)
    {
        BlockedGet(gameObject);
    }
}