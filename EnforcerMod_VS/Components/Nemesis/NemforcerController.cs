using EnforcerPlugin;
using RoR2;
using RoR2.Skills;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static RoR2.CameraTargetParams;

public class NemforcerController : MonoBehaviour
{

    private CharacterCameraParamsData minigunCameraParams = new CharacterCameraParamsData() {

        maxPitch = 70,
        minPitch = -70,
        pivotVerticalOffset = 1.37f,
        idealLocalCameraPos = minigunCameraPosition,
        wallCushion = 0.1f,
    };

    private static Vector3 minigunCameraPosition = new Vector3(-2.2f, 0.0f, -9f);

    public CameraParamsOverrideHandle camOverrideHandle;

    private bool _minigunUp;
    public bool minigunUp {
        get => _minigunUp;
        set {
            _minigunUp = value;
            toggleMinigunCamera(value);
            if (VRAPICompat.IsLocalVRPlayer(charBody))
                VRMinigunToggle(value);
        }
    }

    public bool isMultiplayer;

    public SkillDef primarySkillDef;

    public EntityStateMachine mainStateMachine;


    private float maxLightningIntensity = 64f;
    private float maxMoonIntensity = 16f;

    private ParticleSystem passiveLightning;
    private ParticleSystem passiveMoons;
    private ParticleSystem.EmissionModule passiveLightningEm;
    private ParticleSystem.EmissionModule passiveMoonsEm;

    public ParticleSystem hammerChargeSmall;
    public ParticleSystem hammerChargeLarge;
    public ParticleSystem hammerBurst;

    private CharacterBody charBody;
    private CharacterMotor charMotor;
    private HealthComponent charHealth;
    private CameraTargetParams cameraShit;
    private ChildLocator childLocator;

    private Vector3 previousDirection;
    private float previousAngle;
    private bool passiveIsPlaying;

    private GameObject VROffHand;
    private Transform VRMuzzleOriginParent;
    private Vector3 VRMuzzleOriginPos;
    private Quaternion VRMuzzleOriginRot;

    private void Start()
    {
        charBody = GetComponent<CharacterBody>();
        charMotor = GetComponent<CharacterMotor>();
        charHealth = GetComponent<HealthComponent>();
        cameraShit = GetComponent<CameraTargetParams>();
        childLocator = GetComponentInChildren<ChildLocator>();

        primarySkillDef = charBody.skillLocator.primary.skillDef;

        if (childLocator)
        {
            hammerChargeSmall = childLocator.FindChild("HammerChargeSmall").gameObject.GetComponentInChildren<ParticleSystem>();
            hammerChargeLarge = childLocator.FindChild("HammerChargeLarge").gameObject.GetComponentInChildren<ParticleSystem>();
            hammerBurst = childLocator.FindChild("HammerBurst").gameObject.GetComponentInChildren<ParticleSystem>();

            passiveLightning = childLocator.FindChild("PassiveHealEffect").GetChild(0).gameObject.GetComponent<ParticleSystem>();
            passiveMoons = childLocator.FindChild("PassiveHealEffect").GetChild(1).gameObject.GetComponent<ParticleSystem>();
            passiveLightningEm = passiveLightning.emission;
            passiveMoonsEm = passiveMoons.emission;

            UpdatePassiveEffect();
        }

        InitWeapon();

        Invoke("ModelCheck", 0.2f);
    }

    void OnEnable()
    {
        if (EnforcerPlugin.EnforcerModPlugin.VRInstalled)
        {
            SubscribeToHandPairEvent();
        }
    }

    void OnDisable()
    {
        if (EnforcerPlugin.EnforcerModPlugin.VRInstalled)
        {
            UnsubscribeToHandPairEvent();
        }
    }

    private void SubscribeToHandPairEvent()
    {
        VRAPI.MotionControls.onHandPairSet += OnHandPairSet;
        On.EntityStates.BaseState.GetAimRay += EditAimRay;
    }

    private void UnsubscribeToHandPairEvent()
    {
        VRAPI.MotionControls.onHandPairSet -= OnHandPairSet;
        On.EntityStates.BaseState.GetAimRay -= EditAimRay;
    }

    private void OnHandPairSet(CharacterBody body)
    {
        if (!body.name.Contains("NemesisEnforcerBody") || GetComponent<CharacterBody>() != body) return;

        StartCoroutine(SetVRWeaponAndMinigun(body));
    }
    private IEnumerator<WaitForSeconds> SetVRWeaponAndMinigun(CharacterBody body)
    {
        // Skin not loaded yet, wait a bit
        yield return new WaitForSeconds(0.5f);

        body.aimOriginTransform = VRAPI.MotionControls.dominantHand.muzzle;

        ChildLocator vrHammerChildLocator = VRAPI.MotionControls.dominantHand.transform.GetComponentInChildren<ChildLocator>();
        ChildLocator vrMinigunChildLocator = VRAPI.MotionControls.nonDominantHand.transform.GetComponentInChildren<ChildLocator>();

        if (vrHammerChildLocator && vrMinigunChildLocator)
        {
            List<GameObject> allVRHammers = new List<GameObject>()
            {
                vrHammerChildLocator.FindChild("HammerModel").gameObject,
                vrHammerChildLocator.FindChild("HammerClassicModel").gameObject,
                vrHammerChildLocator.FindChild("HammerGMModel").gameObject,
                vrHammerChildLocator.FindChild("HammerMCModel").gameObject,
                vrHammerChildLocator.FindChild("HammerDededeModel").gameObject,
                vrHammerChildLocator.FindChild("ThrowHammerModel").gameObject
            };
            List<GameObject> allVRMiniguns = new List<GameObject>()
            {
                vrMinigunChildLocator.FindChild("MinigunModel").gameObject,
                vrMinigunChildLocator.FindChild("MinigunClassicModel").gameObject,
                vrMinigunChildLocator.FindChild("MinigunGMModel").gameObject,
                vrMinigunChildLocator.FindChild("MinigunMCModel").gameObject,
                vrMinigunChildLocator.FindChild("MinigunDededeModel").gameObject,
                vrMinigunChildLocator.FindChild("MinigunDripModel").gameObject
            };

            foreach (GameObject hammer in allVRHammers)
            {
                hammer.SetActive(true);
                hammer.SetActive(false);
            }
            foreach (GameObject minigun in allVRMiniguns)
            {
                minigun.SetActive(true);
                minigun.SetActive(false);
            }

            hammerChargeSmall = allVRHammers[0].transform.Find("HammerChargeSmall").gameObject.GetComponentInChildren<ParticleSystem>();
            hammerChargeLarge = allVRHammers[0].transform.Find("HammerChargeLarge").gameObject.GetComponentInChildren<ParticleSystem>();
            hammerBurst = allVRHammers[0].transform.Find("HammerBurst").gameObject.GetComponentInChildren<ParticleSystem>();

            GameObject bigHammer;
            GameObject throwHammer = allVRHammers[5];
            GameObject VRHammer, VRMinigun;
            if (NemforcerSkins.isNemforcerCurrentSkin(body, NemforcerSkins.NemforcerSkin.CLASSIC))
            {
                bigHammer = allVRHammers[1];
                VRMinigun = allVRMiniguns[1];
            }
            else if (NemforcerSkins.isNemforcerCurrentSkin(body, NemforcerSkins.NemforcerSkin.TYPHOONSKIN))
            {
                bigHammer = allVRHammers[2];
                VRMinigun = allVRMiniguns[2];
            }
            else if (NemforcerSkins.isNemforcerCurrentSkin(body, NemforcerSkins.NemforcerSkin.MINECRAFT))
            {
                bigHammer = allVRHammers[3];
                VRMinigun = allVRMiniguns[3];
            }
            else if (NemforcerSkins.isNemforcerCurrentSkin(body, NemforcerSkins.NemforcerSkin.DEDEDE))
            {
                bigHammer = allVRHammers[4];
                VRMinigun = allVRMiniguns[4];
            }
            else if (NemforcerSkins.isNemforcerCurrentSkin(body, NemforcerSkins.NemforcerSkin.DRIP))
            {
                bigHammer = allVRHammers[0];
                VRMinigun = allVRMiniguns[5];
            }
            else if (NemforcerSkins.isNemforcerCurrentSkin(body, NemforcerSkins.NemforcerSkin.ENFORCER))
            {
                bigHammer = allVRHammers[0];
                VRMinigun = allVRMiniguns[0];
                var mat = NemforcerSkins.skinDefs[(int)body.skinIndex].rendererInfos[0].defaultMaterial;
                bigHammer.GetComponentInChildren<MeshRenderer>().material = mat;
                allVRMiniguns[0].GetComponentsInChildren<MeshRenderer>()[0].material = mat;
                allVRMiniguns[0].GetComponentsInChildren<MeshRenderer>()[1].material = mat;
                VRAPI.MotionControls.dominantHand.rendererInfos[0].defaultMaterial = mat;
            }
            else
            {
                bigHammer = allVRHammers[0];
                VRMinigun = allVRMiniguns[0];
            }

            int weapon = GetWeapon();
            // 0 for Hammer and 1 for Throw Hammer
            if (weapon == 0)
                VRHammer = bigHammer;
            else
            {
                VRHammer = throwHammer;
                // disable two-handed for throw hammer
                VRAPI.MotionControls.dominantHand.animator.SetBool("OneHandedHammer", true);
            }

            VRHammer.SetActive(true);
            VRMinigun.SetActive(true);

            // setting up hammer charging VFX
            hammerBurst.transform.parent = VRHammer.transform.Find("HammerBurst");
            hammerBurst.transform.localPosition = Vector3.zero;
            hammerBurst.transform.localRotation = Quaternion.identity;
            hammerChargeSmall.transform.parent = VRHammer.transform.Find("HammerChargeSmall");
            hammerChargeSmall.transform.localPosition = Vector3.zero;
            hammerChargeSmall.transform.localRotation = Quaternion.identity;
            hammerChargeLarge.transform.parent = VRHammer.transform.Find("HammerChargeLarge");
            hammerChargeLarge.transform.localPosition = Vector3.zero;
            hammerChargeLarge.transform.localRotation = Quaternion.identity;

            // gotta place the muzzle from dominant hand to non-dominant hand, reserve the origin data for later restore
            VRMuzzleOriginParent = VRAPI.MotionControls.dominantHand.muzzle.parent;
            VRMuzzleOriginPos = VRAPI.MotionControls.dominantHand.muzzle.localPosition;
            VRMuzzleOriginRot = VRAPI.MotionControls.dominantHand.muzzle.localRotation;
            VROffHand = vrMinigunChildLocator.FindChild("HandModel").gameObject;

            VRMinigunToggle(false);
        }
    }

    // using left hand to aim gas
    private Ray EditAimRay(On.EntityStates.BaseState.orig_GetAimRay orig, EntityStates.BaseState self)
    {
        if (VRAPICompat.IsLocalVRPlayer(charBody) && self.characterBody.name.Contains("NemesisEnforcerBody"))
        {
            if (self is EntityStates.Nemforcer.AimNemGas || self is EntityStates.Enforcer.StunGrenade)
                return VRAPI.MotionControls.nonDominantHand.aimRay;
        }
        return orig(self);
    }

    private void VRMinigunToggle(bool minigun)
    {
        if (minigun)
        {
            VRAPI.MotionControls.dominantHand.muzzle.SetParent(VRAPICompat.GetMinigunMuzzleObject().transform.parent);
            VRAPI.MotionControls.dominantHand.muzzle.localPosition = VRAPICompat.GetMinigunMuzzleObject().transform.localPosition;
            VRAPI.MotionControls.dominantHand.muzzle.localRotation = VRAPICompat.GetMinigunMuzzleObject().transform.localRotation;
            VROffHand.SetActive(false);
        } 
        else
        {
            VRAPI.MotionControls.dominantHand.muzzle.SetParent(VRMuzzleOriginParent);
            VRAPI.MotionControls.dominantHand.muzzle.localPosition = VRMuzzleOriginPos;
            VRAPI.MotionControls.dominantHand.muzzle.localRotation = VRMuzzleOriginRot;
            VROffHand.SetActive(GetWeapon()==1); // Only show off hand when equiping throw hammer
        }
    }

    private void FixedUpdate()
    {
        if (mainStateMachine)
        {
            previousDirection = mainStateMachine.commonComponents.characterDirection.forward;
        }

        UpdatePassiveEffect();
    }

    private void UpdatePassiveEffect()
    {
        //dont give dedede the cool effect for obvious reasons
        if (charBody.baseNameToken == "DEDEDE_NAME") return;
        if (passiveLightning == null || passiveMoons == null) return;

        float healthRemaining = charHealth.combinedHealth / charHealth.fullCombinedHealth;

        if (healthRemaining <= 0.5f)
        {
            if (!passiveIsPlaying)
            {
                passiveIsPlaying = true;
                passiveLightning.Play();
                passiveMoons.Play();
            }

            float lerp = Mathf.InverseLerp(0.2f, 0.6f, healthRemaining);

            passiveLightningEm.rateOverTime = Mathf.SmoothStep(maxLightningIntensity, 0, lerp);
            passiveMoonsEm.rateOverTime = Mathf.SmoothStep(maxMoonIntensity, 0, lerp);
        }
        else
        {
            if (passiveIsPlaying)
            {
                passiveIsPlaying = false;
                passiveLightning.Stop();
                passiveMoons.Stop();
            }
        }
    }

    private void toggleMinigunCamera(bool minigunUp) {

        if (minigunUp) {

            CameraParamsOverrideRequest request = new CameraParamsOverrideRequest {
                cameraParamsData = minigunCameraParams,
                priority = 0,
            };

            camOverrideHandle = cameraShit.AddParamsOverride(request, 0.5f);
        } else {

            for (int i = cameraShit.cameraParamsOverrides.Count - 1; i >= 0; i--) {

                camOverrideHandle.target = cameraShit.cameraParamsOverrides[i];

                cameraShit.RemoveParamsOverride(camOverrideHandle, 0.3f);
            }
        }

    }

    public void ModelCheck()
    {
        if (charBody && charBody.master)
        {
            if (charBody.baseNameToken == "DEDEDE_NAME") return;

            if (charBody.master.inventory)
            {
                //this hides the hammer
                var characterModel = charBody.modelLocator?.modelTransform.GetComponentInChildren<CharacterModel>();
                if (characterModel)
                {
                    characterModel.baseRendererInfos[0].defaultMaterial = characterModel.gameObject.GetComponent<ModelSkinController>().skins[charBody.skinIndex].rendererInfos[0].defaultMaterial;
                    if (charBody.master.inventory.GetItemCount(RoR2Content.Items.ArmorReductionOnHit) > 0) 
                        characterModel.baseRendererInfos[0].defaultMaterial = null;
                }
            }
        }
    }

    private int GetWeapon()
    {
        int weapon = 0;

        if (charBody && charBody.skillLocator)
        {
            string skillString = primarySkillDef.skillNameToken;
            switch (skillString)
            {
                case "NEMFORCER_PRIMARY_HAMMER_NAME":
                    weapon = 0;
                    break;
                case "NEMFORCER_PRIMARY_THROWHAMMER_NAME":
                    weapon = 1;
                    break;
            }
        }

        return weapon;
    }

    public void InitWeapon()
    {
        if (charBody.baseNameToken == "DEDEDE_NAME") return;

        int weapon = GetWeapon();
        EquipWeapon(weapon);
        SetCrosshair(weapon);
    }

    public void HideWeapon()
    {
        if (childLocator)
        {
            childLocator.FindChild("HammerModel").gameObject.SetActive(false);
            childLocator.FindChild("AltHammer").gameObject.SetActive(false);
        }
    }

    public void DelayedResetWeapon()
    {
        Invoke("ResetWeapon", 0.1f);
    }

    public void ResetCrosshair()
    {
        int weapon = GetWeapon();
        SetCrosshair(weapon);
    }

    private void EquipWeapon(int weapon)
    {
        if (childLocator)
        {
            HideWeapon();

            switch (weapon)
            {
                case 0:
                    childLocator.FindChild("HammerModel").gameObject.SetActive(true);
                    break;
                case 1:
                    childLocator.FindChild("AltHammer").gameObject.SetActive(true);
                    break;
            }
        }
    }

    private void SetCrosshair(int weapon)
    {
        if (charBody)
        {
            if (minigunUp)
            {
                charBody._defaultCrosshairPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/BanditCrosshair");
            }
            else
            {
                switch (weapon)
                {
                    case 0:
                        charBody._defaultCrosshairPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/SimpleDotCrosshair");
                        break;
                    case 1:
                        charBody._defaultCrosshairPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/StandardCrosshair");
                        break;
                }
            }
        }
    }

    public void resetAimAngle() {
        previousAngle = 0;
    }

    public void pseudoAimMode(float angle) {

        Vector3 aimDirection = mainStateMachine.commonComponents.inputBank.aimDirection;
        pseudoAimMode(angle, aimDirection);
    }

    public void pseudoAimMode(float angle, Vector3 aimDirection) {
        //rotationResetTimer = 1;

        if (!mainStateMachine)
            return;

        //angle = Mathf.Lerp(previousAngle, angle, 0.1f);

        aimDirection = aimDirection.normalized;
        aimDirection.y = 0;

        //direction perpendicular to the aiming
        Vector3 turningDirection = Vector3.Cross(aimDirection, Vector3.up);

        float rot = angle;

        Vector3 turnedDirection = Vector3.RotateTowards(aimDirection, turningDirection, -rot * Mathf.Deg2Rad, 1);

        turnedDirection = Vector3.Lerp(previousDirection, turnedDirection, 0.5f);

        pseudoAimMode(turnedDirection);
        previousAngle = angle;
    }

    //copied and pasted only what we need from SetAimMode cause using the whole thing is a little fucky
    public void pseudoAimMode(Vector3 direction) {
        mainStateMachine.commonComponents.characterDirection.forward = direction;
        mainStateMachine.commonComponents.characterDirection.moveVector = direction;

        if (mainStateMachine.commonComponents.modelLocator) {
            Transform modelTransform = mainStateMachine.commonComponents.modelLocator.modelTransform;
            if (modelTransform) {
                AimAnimator component = modelTransform.GetComponent<AimAnimator>();
                component.AimImmediate();
            }
        }
    }
}
