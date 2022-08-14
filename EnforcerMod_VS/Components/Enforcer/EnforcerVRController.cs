using EnforcerPlugin;
using EntityStates.Enforcer;
using Modules;
using RoR2;
using System.Collections.Generic;
using UnityEngine;
using VRAPI;
using static EnforcerWeaponComponent;

public class EnforcerVRComponent : MonoBehaviour
{
    private Transform skateboardBase;
    private GameObject VRSkateboard;
    private Transform VRSkateboardHandBase;
    private bool isSkating = false;
    private EnforcerComponent enforcerComponent;
    private EnforcerWeaponComponent enforcerWeaponComponent;

    public CharacterBody charBody;

    public void InitSkateBoardBase(Transform skateboardBase)
    {
        this.skateboardBase = skateboardBase;
    }

    void OnEnable()
    {
        if (EnforcerModPlugin.VRInstalled)
        {
            SubscribeToHandPairEvent();
            On.EntityStates.BaseState.GetAimRay += EditAimRay;
        }
    }

    void OnDisable()
    {
        if (EnforcerModPlugin.VRInstalled)
        {
            UnsubscribeToHandPairEvent();
            On.EntityStates.BaseState.GetAimRay -= EditAimRay;
        }
    }

    private Ray EditAimRay(On.EntityStates.BaseState.orig_GetAimRay orig, EntityStates.BaseState self)
    {
        if (VRAPICompat.IsLocalVRPlayer(charBody) && self.characterBody.name.Contains("EnforcerBody"))
        {
            if (self is ShieldBash)
                return new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        }
        return orig(self);
    }

    private void SubscribeToHandPairEvent()
    {
        MotionControls.onHandPairSet += OnHandPairSet;
    }

    private void UnsubscribeToHandPairEvent()
    {
        MotionControls.onHandPairSet -= OnHandPairSet;
    }

    private void OnHandPairSet(CharacterBody body)
    {
        if (!body.name.Contains("EnforcerBody") || GetComponent<CharacterBody>() != body) return;

        this.charBody = body;

        // Skin not loaded yet, wait a bit
        StartCoroutine(SetVRWeaponAndShield(body));
    }
    private IEnumerator<WaitForSeconds> SetVRWeaponAndShield(CharacterBody body)
    {
        yield return new WaitForSeconds(0.5f);
        enforcerComponent = GetComponent<EnforcerComponent>();
        enforcerWeaponComponent = GetComponent<EnforcerWeaponComponent>();

        if (enforcerComponent)
            enforcerComponent.origOrigin = MotionControls.dominantHand.muzzle;

        ChildLocator vrWeaponChildLocator = MotionControls.dominantHand.transform.GetComponentInChildren<ChildLocator>();
        ChildLocator vrShieldChildLocator = MotionControls.nonDominantHand.transform.GetComponentInChildren<ChildLocator>();

        if (vrWeaponChildLocator && vrShieldChildLocator)
        {
            var donminHandObj = vrWeaponChildLocator.FindChild("HandModel").gameObject;
            List<GameObject> allVRWeapons = new List<GameObject>()
            {
                vrWeaponChildLocator.FindChild("GunModel").gameObject,
                vrWeaponChildLocator.FindChild("SuperGunModel").gameObject,
                vrWeaponChildLocator.FindChild("HMGModel").gameObject,
                vrWeaponChildLocator.FindChild("RobotArmModel").gameObject,
                vrWeaponChildLocator.FindChild("HammerModel").gameObject,
                vrWeaponChildLocator.FindChild("NemHammerModel").gameObject
            };

            foreach (GameObject weaponObject in allVRWeapons)
            {
                //bit of a hack to make sure objects hide their associated item bones
                weaponObject.SetActive(true);
                weaponObject.SetActive(false);
            }

            EquippedGun weapon = enforcerWeaponComponent.GetWeapon();
            Transform muzzle = MotionControls.dominantHand.muzzle;
            if (Skins.isEnforcerCurrentSkin(body, Skins.EnforcerSkin.ROBIT))
                switch (weapon)
                {
                    default:
                    case EquippedGun.GUN:
                    case EquippedGun.SUPER:
                    case EquippedGun.HMG:
                        donminHandObj.SetActive(false);
                        allVRWeapons[3].SetActive(true);
                        muzzle.localPosition = new Vector3(0.0128f, -0.0911f, 0.2719f);
                        break;
                    case EquippedGun.HAMMER:
                        if (Skins.isEnforcerCurrentSkin(body, Skins.EnforcerSkin.RECOLORNEMESIS))
                            allVRWeapons[5].SetActive(true);
                        else
                            allVRWeapons[4].SetActive(true);
                        muzzle.localPosition = new Vector3(0.01657f, -0.21657f, 0.81805f);
                        break;
                }
            else
                switch (weapon)
                {
                    default:
                    case EquippedGun.GUN:
                        allVRWeapons[0].SetActive(true);
                        break;
                    case EquippedGun.SUPER:
                        allVRWeapons[1].SetActive(true);
                        muzzle.localPosition = new Vector3(0.01297f, -0.2533f, 0.8785f);
                        break;
                    case EquippedGun.HMG:
                        allVRWeapons[2].SetActive(true);
                        muzzle.localPosition = new Vector3(0.01657f, -0.21657f, 0.81805f);
                        break;
                    case EquippedGun.HAMMER:
                        allVRWeapons[4].SetActive(true);
                        muzzle.localPosition = new Vector3(0.0156f, -0.025f, 0.0853f);
                        break;
                }

            VRSkateboardHandBase = vrShieldChildLocator.FindChild("SkateboardHandBase");
            List<GameObject> allVRShields = new List<GameObject>()
            {
                vrShieldChildLocator.FindChild("ShieldModel").gameObject,
                vrShieldChildLocator.FindChild("GlassShieldModel").gameObject,
                vrShieldChildLocator.FindChild("FemShieldModel").gameObject,
                vrShieldChildLocator.FindChild("SteveShieldModel").gameObject,
                vrShieldChildLocator.FindChild("N4CRShieldModel").gameObject,
                vrShieldChildLocator.FindChild("Skateboard").gameObject,
                vrShieldChildLocator.FindChild("FemSkateboard").gameObject
            };

            foreach (GameObject shieldObject in allVRShields)
            {
                //bit of a hack to make sure objects hide their associated item bones
                shieldObject.SetActive(true);
                shieldObject.SetActive(false);
            }

            EquippedShield shield = enforcerWeaponComponent.GetShield();
            switch (shield)
            {
                default:
                case EquippedShield.SHIELD:
                    if (Skins.isEnforcerCurrentSkin(body, Skins.EnforcerSkin.MASTERY))
                        allVRShields[1].SetActive(true);
                    else if (Skins.isEnforcerCurrentSkin(body, Skins.EnforcerSkin.FEMFORCER))
                        allVRShields[2].SetActive(true);
                    else if (Skins.isEnforcerCurrentSkin(body, Skins.EnforcerSkin.FUCKINGSTEVE))
                        allVRShields[3].SetActive(true);
                    else if (Skins.isEnforcerCurrentSkin(body, Skins.EnforcerSkin.ROBIT))
                        allVRShields[4].SetActive(true);
                    else
                    {
                        allVRShields[0].SetActive(true);
                    }
                    Shader hotpoo = LegacyResourcesAPI.Load<Shader>("Shaders/Deferred/hgstandard");
                    MotionControls.nonDominantHand.rendererInfos[0].defaultMaterial = Skins.skinDefs[(int)body.skinIndex].rendererInfos[0].defaultMaterial;
                    MotionControls.nonDominantHand.rendererInfos[0].defaultMaterial.shader = hotpoo;
                    break;
                case EquippedShield.BOARD:
                    var skateboard = (Skins.isEnforcerCurrentSkin(body, Skins.EnforcerSkin.FEMFORCER)) ? allVRShields[6] : allVRShields[5];
                    skateboard.SetActive(true);
                    this.VRSkateboard = skateboard.gameObject;
                    break;
            }
        }
    }

    public void ReparentSkateboard(SkateBoardParent newParent)
    {
        switch (newParent)
        {
            case SkateBoardParent.BASE:
                isSkating = true;
                break;
            case SkateBoardParent.HAND:
                isSkating = false;
                if (VRAPICompat.IsLocalVRPlayer(charBody) && VRSkateboard && VRSkateboardHandBase)
                {
                    this.VRSkateboard.transform.SetParent(this.VRSkateboardHandBase);
                    this.VRSkateboard.transform.localPosition = Vector3.zero;
                    this.VRSkateboard.transform.localRotation = Quaternion.identity;
                }
                break;
        }
    }

    private void LateUpdate()
    {
        if (VRAPICompat.IsLocalVRPlayer(charBody))
        {
            // For skateborad to stay under the player in VR
            if (isSkating && VRSkateboard && VRSkateboardHandBase)
            {
                this.VRSkateboard.transform.SetParent(this.skateboardBase);
                this.VRSkateboard.transform.localPosition = Vector3.zero;
                this.VRSkateboard.transform.localRotation = Quaternion.identity;
                this.VRSkateboard.transform.SetParent(this.VRSkateboardHandBase);
            }
            // Auto shielding when you hold the shield in front of you
            if ((Config.physicalVRShieldUp.Value || Config.physicalVRShieldDown.Value) && enforcerWeaponComponent && enforcerWeaponComponent.GetShield() == EquippedShield.SHIELD && enforcerComponent)
            {
                var target = MotionControls.nonDominantHand.muzzle.position - Camera.main.transform.position;
                bool isForward = Vector3.Dot(Camera.main.transform.forward, target) > 0;
                bool isRight = Vector3.Cross(Camera.main.transform.forward, target).y > 0;
                bool otherSide = (MotionControls.dominantHand == MotionControls.rightHand) ? isRight : !isRight;
                if (Config.physicalVRShieldUp.Value && !enforcerComponent.isShielding)
                {
                    // Auto shield up based on the angle between non-dominate hand and camera
                    Vector3 v1 = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
                    Vector3 v2 = new Vector3(target.x, 0, target.z);
                    var angle = Vector3.Angle(v1, v2);
                    if (isForward && otherSide && angle > 20)
                        charBody.skillLocator.special.ExecuteIfReady();
                }
                else if (Config.physicalVRShieldDown.Value && enforcerComponent.isShielding)
                {
                    // Auto shield down when non-dominate hand points downward
                    var angle = Vector3.Angle(MotionControls.nonDominantHand.muzzle.forward, Vector3.down);
                    if (!otherSide && angle < 90 - Config.physicalVRShieldDownAngle.Value)
                        charBody.skillLocator.special.ExecuteIfReady();
                }
            }
        }
    }
}