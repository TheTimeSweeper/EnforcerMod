using EnforcerPlugin;
using RoR2;
using RoR2.Skills;
using System.Collections.Generic;
using UnityEngine;
using static RoR2.CameraTargetParams;

public class NemforcerVRController : MonoBehaviour
{
    private CharacterBody charBody;
    private GameObject VROffHand;
    private Transform VRMuzzleOriginParent;
    private Vector3 VRMuzzleOriginPos;
    private Quaternion VRMuzzleOriginRot;

    private NemforcerController nemforcerController;

    void OnEnable()
    {
        if (EnforcerModPlugin.VREnabled)
        {
            SubscribeToHandPairEvent();
        }
    }

    void OnDisable()
    {
        if (EnforcerModPlugin.VREnabled)
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
        charBody = body;
        nemforcerController = GetComponentInChildren<NemforcerController>();
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
                vrHammerChildLocator.FindChild("HammerEnforcerModel").gameObject,
                vrHammerChildLocator.FindChild("ThrowHammerModel").gameObject
            };
            List<GameObject> allVRMiniguns = new List<GameObject>()
            {
                vrMinigunChildLocator.FindChild("MinigunModel").gameObject,
                vrMinigunChildLocator.FindChild("MinigunClassicModel").gameObject,
                vrMinigunChildLocator.FindChild("MinigunGMModel").gameObject,
                vrMinigunChildLocator.FindChild("MinigunMCModel").gameObject,
                vrMinigunChildLocator.FindChild("MinigunDededeModel").gameObject,
                vrMinigunChildLocator.FindChild("MinigunEnforcerModel").gameObject,
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

            nemforcerController.hammerChargeSmall = allVRHammers[0].transform.Find("HammerChargeSmall").gameObject.GetComponentInChildren<ParticleSystem>();
            nemforcerController.hammerChargeLarge = allVRHammers[0].transform.Find("HammerChargeLarge").gameObject.GetComponentInChildren<ParticleSystem>();
            nemforcerController.hammerBurst = allVRHammers[0].transform.Find("HammerBurst").gameObject.GetComponentInChildren<ParticleSystem>();

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
            else if (NemforcerSkins.isNemforcerCurrentSkin(body, NemforcerSkins.NemforcerSkin.ENFORCER))
            {
                bigHammer = allVRHammers[5];
                VRMinigun = allVRMiniguns[5];
            }
            else if (NemforcerSkins.isNemforcerCurrentSkin(body, NemforcerSkins.NemforcerSkin.DRIP))
            {
                bigHammer = allVRHammers[0];
                VRMinigun = allVRMiniguns[6];
            }
            else
            {
                bigHammer = allVRHammers[0];
                VRMinigun = allVRMiniguns[0];
            }

            int weapon = nemforcerController.GetWeapon();
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
            nemforcerController.hammerBurst.transform.parent = VRHammer.transform.Find("HammerBurst");
            nemforcerController.hammerBurst.transform.localPosition = Vector3.zero;
            nemforcerController.hammerBurst.transform.localRotation = Quaternion.identity;
            nemforcerController.hammerChargeSmall.transform.parent = VRHammer.transform.Find("HammerChargeSmall");
            nemforcerController.hammerChargeSmall.transform.localPosition = Vector3.zero;
            nemforcerController.hammerChargeSmall.transform.localRotation = Quaternion.identity;
            nemforcerController.hammerChargeLarge.transform.parent = VRHammer.transform.Find("HammerChargeLarge");
            nemforcerController.hammerChargeLarge.transform.localPosition = Vector3.zero;
            nemforcerController.hammerChargeLarge.transform.localRotation = Quaternion.identity;

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

    public void VRMinigunToggle(bool minigun)
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
            VROffHand.SetActive(nemforcerController.GetWeapon()==1); // Only show off hand when equiping throw hammer
        }
    }
}
