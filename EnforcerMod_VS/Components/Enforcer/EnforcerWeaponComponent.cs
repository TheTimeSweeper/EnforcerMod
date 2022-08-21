using EnforcerPlugin;
using EntityStates.Enforcer;
using Modules;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;
using VRAPI;

public class EnforcerWeaponComponent : MonoBehaviour {

    public enum EquippedGun {
        NONE = -1,
        GUN,
        SUPER,
        HMG,
        HAMMER,
        NEEDLER
    }

    public enum EquippedShield {
        NONE = -1,
        SHIELD,
        SHIELD2, //I still believe man
        BOARD,
        NOTHING
    }

    public enum SkateBoardParent {
        BASE,
        HAND,
    }

    private GameObject shotgunObject { get => this.childLocator.FindChild("GunModel").gameObject; }
    private GameObject ssgobject { get => this.childLocator.FindChild("SuperGunModel").gameObject; }
    private GameObject hmgObject { get => this.childLocator.FindChild("HMGModel").gameObject; }
    private GameObject hammerObject { get => this.childLocator.FindChild("HammerModel").gameObject; }
    private GameObject needlerObject { get => this.childLocator.FindChild("NeedlerModel").gameObject; }

    private GameObject shieldModelObject { get => this.childLocator.FindChild("ShieldModel").gameObject; }
    private Transform shieldBoneTransform{ get => this.childLocator.FindChild("Shield"); }
    //private GameObject shielDevicedObject { get => this.childLocator.FindChild("ShieldDeviceModel").gameObject; }
    private GameObject skateBoardObject { get => this.childLocator.FindChild("SkamteBordModel").gameObject; }

    private List<GameObject> _allWeapons;
    private List<GameObject> allWeapons {
        get {
            if (_allWeapons != null)
                return _allWeapons;

            _allWeapons = new List<GameObject> {
                shotgunObject,
                ssgobject,
                hmgObject,
                hammerObject,
                needlerObject,
            };

            _allWeapons.RemoveAll(gob => gob == null);

            return _allWeapons;
        }

    }

    public EquippedGun currentGun = EquippedGun.NONE;
    public EquippedShield currentShield = EquippedShield.NONE;

    public bool isMultiplayer;

    public static int maxShellCount = 12;
    private int currentShell;
    private GameObject[] shellObjects;

    public bool shieldUp;

    private GameObject skateboard;
    private Transform skateboardBase;
    private Transform skateboardHandBase;

    private string stepSoundString;
    private string landSoundString;

    public CharacterBody charBody;
    public CharacterMotor charMotor;
    public HealthComponent charHealth;
    public CameraTargetParams cameraShit;
    public ChildLocator childLocator;

    private EnforcerVRComponent enforcerVRComponent;

    public FootstepHandler footStep;
    public SfxLocator sfx;

    public void Init() {
        this.charBody = this.GetComponentInChildren<CharacterBody>();
        this.charMotor = this.GetComponentInChildren<CharacterMotor>();
        this.charHealth = this.GetComponentInChildren<HealthComponent>();
        this.cameraShit = this.GetComponent<CameraTargetParams>();
        this.childLocator = this.GetComponentInChildren<ChildLocator>();

        this.footStep = this.GetComponentInChildren<FootstepHandler>();
        this.sfx = this.GetComponentInChildren<SfxLocator>();

        if (this.footStep) this.stepSoundString = this.footStep.baseFootstepString;
        if (this.sfx) this.landSoundString = this.sfx.landingSound;
    }

    void Awake() {

        if (EnforcerModPlugin.VRInstalled) {
            this.enforcerVRComponent = this.GetComponent<EnforcerVRComponent>();
        }
    }

    void Start() {

        this.SetWeaponsAndShields();
        this.InitShells();
        this.InitSkateboard();

        this.Invoke("ModelCheck", 0.2f);
        //todo CUM2 delete this
        //this.UpdateCamera();
    }

    public void SetWeaponsAndShields() {

        EquippedGun weapon = GetWeapon();
        if (weapon != currentGun) {
            currentGun = weapon;

            this.HideWeapons();
            this.EquipWeapon(weapon);
            this.SetCrosshair(weapon);
            //SetWeaponDisplayRules(weapon);
        }

        EquippedShield shield = GetShield();
        if (shield != currentShield) {
            currentShield = shield;

            this.HideSpecials();
            this.EquipShield(shield);
            this.SetShieldDisplayRules(shield);
        }
    }

    public void HideEquips()
    {
        HideWeapons();
        HideSpecials();
    }

    public void UnHideEquips() {

        this.EquipWeapon(currentGun);
        this.EquipShield(currentShield);
    }

    public void HideWeapons() {
        if (this.childLocator) {
            for (int i = 0; i < allWeapons.Count; i++) {
                //bit of a hack to make sure objects hide their associated item bones
                allWeapons[i].SetActive(true);
                allWeapons[i].SetActive(false);
            }
        }
    }

    public void HideSpecials()
    {
        if (this.childLocator)
        {
            showShield(false);

            skateBoardObject.SetActive(false);
        }
    }

    public EquippedGun GetWeapon() {
        EquippedGun weapon = EquippedGun.GUN;

        if (this.charBody && this.charBody.skillLocator) {
            string skillString = this.charBody.skillLocator.primary.skillDef.skillNameToken;
            switch (skillString) {
                default:
                case "ENFORCER_PRIMARY_SHOTGUN_NAME":
                    weapon = EquippedGun.GUN;
                    break;
                case "ENFORCER_PRIMARY_SUPERSHOTGUN_NAME":
                    weapon = EquippedGun.SUPER;
                    break;
                case "ENFORCER_PRIMARY_RIFLE_NAME":
                    weapon = EquippedGun.HMG;
                    break;
                case "ENFORCER_PRIMARY_HAMMER_NAME":
                    weapon = EquippedGun.HAMMER;
                    break;
                case "SKILL_LUNAR_PRIMARY_REPLACEMENT_NAME":
                    weapon = EquippedGun.NEEDLER;
                    break;
            }
        }

        return weapon;
    }

    private void EquipWeapon(EquippedGun weapon) {
        if (this.childLocator) {
            switch (weapon) {
                default:
                case EquippedGun.GUN:
                    shotgunObject.SetActive(true);
                    break;
                case EquippedGun.SUPER:
                    ssgobject.SetActive(true);
                    break;
                case EquippedGun.HMG:
                    hmgObject.SetActive(true);
                    break;
                case EquippedGun.HAMMER:
                    hammerObject.SetActive(true);
                    break;
                case EquippedGun.NEEDLER:
                    needlerObject.SetActive(true);
                    break;
            }
        }
    }

    private void SetCrosshair(EquippedGun weapon) {
        if (this.charBody) {
            switch (weapon) {
                case EquippedGun.GUN:
                case EquippedGun.SUPER:
                case EquippedGun.HMG:
                    this.charBody._defaultCrosshairPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/SMGCrosshair");
                    break;
                case EquippedGun.HAMMER:
                    this.charBody._defaultCrosshairPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/SimpleDotCrosshair");
                    break;
                case EquippedGun.NEEDLER:
                    this.charBody._defaultCrosshairPrefab = EnforcerPlugin.EnforcerModPlugin.needlerCrosshair;
                    break;
            }
        }
    }

    //called when checking for needler
    public void DelayedResetWeaponsAndShields() {

        this.Invoke("ResetWeaponsAndShields", 0.1f);
    }

    public void ResetWeaponsAndShields() {

        SetWeaponsAndShields();
    }

    public EquippedShield GetShield() {

        EquippedShield shield = EquippedShield.SHIELD;

        if (this.charBody && this.charBody.skillLocator) {
            string skillString = this.charBody.skillLocator.special.skillDef.skillNameToken;
            switch (skillString) {
                case "ENFORCER_SPECIAL_SHIELDUP_NAME":
                case "ENFORCER_SPECIAL_SHIELDDOWN_NAME":
                    shield = EquippedShield.SHIELD;

                    //if (this.charBody.skinIndex == EnforcerPlugin.EnforcerPlugin.doomGuyIndex) 
                    //    shield = EquippedShield.SHIELD2;
                    //if (this.charBody.skinIndex == EnforcerPlugin.EnforcerPlugin.engiIndex)
                    //    shield = EquippedShield.SHIELD2;
                    break;

                case "ENFORCER_SPECIAL_SHIELDON_NAME":
                case "ENFORCER_SPECIAL_SHIELDOFF_NAME":
                    shield = EquippedShield.SHIELD2;
                    break;

                case "ENFORCER_SPECIAL_BOARDUP_NAME":
                case "ENFORCER_SPECIAL_BOARDDOWN_NAME":
                    shield = EquippedShield.BOARD;
                    break;

                default:
                    shield = EquippedShield.NOTHING;
                    break;
            }
        }

        return shield;
    }

    private void EquipShield(EquippedShield shield) {

        if (this.childLocator) {
            switch (shield) {
                default:
                case EquippedShield.SHIELD:
                    showShield(true);
                    break;
                case EquippedShield.SHIELD2:
                    showShield(false);
                    break;
                case EquippedShield.BOARD:
                    skateBoardObject.SetActive(true);
                    break;
                case EquippedShield.NOTHING:
                    break;
            }
        }
    }

    private void showShield(bool show) {

        shieldModelObject.SetActive(show);
        shieldBoneTransform.localScale = show ? Vector3.one : Vector3.zero;
    }

    private void SetShieldDisplayRules(EquippedShield shield) {
        /*ItemDisplayRuleSet ruleset = this.GetComponentInChildren<CharacterModel>().itemDisplayRuleSet;
        
        //should do this for shields rather than the bone thing I do for weapons
            //weapon things are relegated to 1 thing contained together
            //alterante shield displays will be a lot more complicated

        switch (shield) {
            case EquippedShield.SHIELD:
                ruleset.FindItemDisplayRuleGroup("ArmorPlate").rules[0].childName = "Shield";
                ruleset.FindItemDisplayRuleGroup("ArmorPlate").rules[0].localPos = new Vector3(2.5f, 0.5f, -2);
                ruleset.FindItemDisplayRuleGroup("ArmorPlate").rules[0].localAngles = new Vector3(0, 0, 180);
                ruleset.FindItemDisplayRuleGroup("ArmorPlate").rules[0].localScale = new Vector3(8, 4, 8);

                ruleset.FindItemDisplayRuleGroup("Bear").rules[0].childName = "Shield";
                ruleset.FindItemDisplayRuleGroup("Bear").rules[0].localPos = new Vector3(0, 1.28f, 0.97f);
                ruleset.FindItemDisplayRuleGroup("Bear").rules[0].localAngles = new Vector3(-77, 180, 0);
                ruleset.FindItemDisplayRuleGroup("Bear").rules[0].localScale = new Vector3(5, 5, 5);

                ruleset.FindItemDisplayRuleGroup("ExtraLife").rules[0].childName = "Shield";
                ruleset.FindItemDisplayRuleGroup("ExtraLife").rules[0].localPos = new Vector3(0, 1.15f, -3.65f);
                ruleset.FindItemDisplayRuleGroup("ExtraLife").rules[0].localAngles = new Vector3(-80, 180, 0);
                ruleset.FindItemDisplayRuleGroup("ExtraLife").rules[0].localScale = new Vector3(5, 5, 5);

                ruleset.FindItemDisplayRuleGroup("RegenOnKill").rules[0].childName = "Shield";
                ruleset.FindItemDisplayRuleGroup("RegenOnKill").rules[0].localPos = new Vector3(2, 0, 7.8f);
                ruleset.FindItemDisplayRuleGroup("RegenOnKill").rules[0].localAngles = new Vector3(-25, 0, 180);
                ruleset.FindItemDisplayRuleGroup("RegenOnKill").rules[0].localScale = new Vector3(2f, 2f, 2f);

                ruleset.FindItemDisplayRuleGroup("BounceNearby").rules[0].childName = "Shield";
                ruleset.FindItemDisplayRuleGroup("BounceNearby").rules[0].localPos = new Vector3(0, 0, 7.5f);
                ruleset.FindItemDisplayRuleGroup("BounceNearby").rules[0].localAngles = new Vector3(0, 0, 25);
                ruleset.FindItemDisplayRuleGroup("BounceNearby").rules[0].localScale = new Vector3(1, 1, 1);
                break;
            case EquippedShield.SHIELD2:
                ruleset.FindItemDisplayRuleGroup("ArmorPlate").rules[0].childName = "Shield";
                ruleset.FindItemDisplayRuleGroup("ArmorPlate").rules[0].localPos = new Vector3(1, 0, 0);
                ruleset.FindItemDisplayRuleGroup("ArmorPlate").rules[0].localAngles = new Vector3(0, 0, 180);
                ruleset.FindItemDisplayRuleGroup("ArmorPlate").rules[0].localScale = new Vector3(3, 3, 3);

                ruleset.FindItemDisplayRuleGroup("Bear").rules[0].childName = "Shield";
                ruleset.FindItemDisplayRuleGroup("Bear").rules[0].localPos = new Vector3(0, -0.4f, 1);
                ruleset.FindItemDisplayRuleGroup("Bear").rules[0].localAngles = new Vector3(0, 0, 0);
                ruleset.FindItemDisplayRuleGroup("Bear").rules[0].localScale = new Vector3(3, 3, 3);

                ruleset.FindItemDisplayRuleGroup("ExtraLife").rules[0].childName = "Shield";
                ruleset.FindItemDisplayRuleGroup("ExtraLife").rules[0].localPos = new Vector3(0, 0, 0);
                ruleset.FindItemDisplayRuleGroup("ExtraLife").rules[0].localAngles = new Vector3(-90, 90, 0);
                ruleset.FindItemDisplayRuleGroup("ExtraLife").rules[0].localScale = new Vector3(4, 4, 4);

                ruleset.FindItemDisplayRuleGroup("RegenOnKill").rules[0].childName = "Shield";
                ruleset.FindItemDisplayRuleGroup("RegenOnKill").rules[0].localPos = new Vector3(1.4f, 0, 1.5f);
                ruleset.FindItemDisplayRuleGroup("RegenOnKill").rules[0].localAngles = new Vector3(-40, 0, 180);
                ruleset.FindItemDisplayRuleGroup("RegenOnKill").rules[0].localScale = new Vector3(1.5f, 1.5f, 1.5f);

                ruleset.FindItemDisplayRuleGroup("BounceNearby").rules[0].childName = "Shield";
                ruleset.FindItemDisplayRuleGroup("BounceNearby").rules[0].localPos = new Vector3(0, 0, 0);
                ruleset.FindItemDisplayRuleGroup("BounceNearby").rules[0].localAngles = new Vector3(0, 0, 0);
                ruleset.FindItemDisplayRuleGroup("BounceNearby").rules[0].localScale = new Vector3(1, 1, 1);
                break;
            case EquippedShield.BOARD:
                break
        }*/
    }

    public void ModelCheck() {
        if (this.charBody && this.charBody.master) {
            if (this.charBody.master.inventory) {
                /*if (EnforcerPlugin.EnforcerPlugin.sillyHammer.Value)
                {
                    var characterModel = charBody.modelLocator.modelTransform.GetComponentInChildren<CharacterModel>();
                    if (characterModel)
                    {
                        //characterModel.baseRendererInfos[0].defaultMaterial = characterModel.gameObject.GetComponent<ModelSkinController>().skins[charBody.skinIndex].rendererInfos[0].defaultMaterial;
                        //if (charBody.master.inventory.GetItemCount(ItemIndex.ArmorReductionOnHit) > 0) characterModel.baseRendererInfos[0].defaultMaterial = null;
                    }
                }*/
                /*else
                {
                    var characterModel = charBody.modelLocator.modelTransform;
                    if (characterModel)
                    {
                        float num = 0f;
                        if (charBody.master.inventory.GetItemCount(ItemIndex.ArmorReductionOnHit) > 0) num = 1f;

                        var animator = characterModel.GetComponent<Animator>();
                        if (animator) animator.SetFloat("shitpost", num);
                    }
                }*/
            }
        }
    }

    private void InitShells() {
        if (this.childLocator is null) return;

        /*GameObject desiredShell = EnforcerPlugin.Assets.shotgunShell;
        if (GetWeapon() == 2) desiredShell = EnforcerPlugin.Assets.superShotgunShell;

        GameObject shellObject = GameObject.Instantiate<GameObject>(desiredShell, childLocator.FindChild("GrenadeMuzzle"));

        shellObject.transform.localPosition = Vector3.zero;
        shellObject.transform.localRotation = Quaternion.identity;

        shellController = shellObject.GetComponentInChildren<ParticleSystem>();

        shellController.transform.localPosition = new Vector3(0, -0.1f, 0);

        var x = shellController.main;
        x.simulationSpace = ParticleSystemSimulationSpace.World;*/

        this.currentShell = 0;

        this.shellObjects = new GameObject[EnforcerWeaponComponent.maxShellCount + 1];

        GameObject desiredShell = Assets.shotgunShell;
        if (this.GetWeapon() == EquippedGun.SUPER) desiredShell = Assets.superShotgunShell;

        for (int i = 0; i < EnforcerWeaponComponent.maxShellCount; i++) {
            this.shellObjects[i] = GameObject.Instantiate(desiredShell, this.childLocator.FindChild("Gun"), false);
            this.shellObjects[i].transform.localScale = Vector3.one * 1.1f;
            this.shellObjects[i].SetActive(false);
            this.shellObjects[i].GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
        }
    }

    public void DropShell(Vector3 force) {
        //if (shellController) shellController.Play();
        if (this.shellObjects == null) return;

        if (this.shellObjects[currentShell] == null) return;

        Transform origin = this.childLocator.FindChild("Muzzle");

        this.shellObjects[currentShell].SetActive(false);

        this.shellObjects[currentShell].transform.position = origin.position;
        this.shellObjects[currentShell].transform.SetParent(null);

        this.shellObjects[currentShell].SetActive(true);

        Rigidbody rb = this.shellObjects[currentShell].gameObject.GetComponent<Rigidbody>();
        if (rb) rb.velocity = force;

        this.currentShell++;
        if (this.currentShell >= EnforcerWeaponComponent.maxShellCount) this.currentShell = 0;
    }

    private void InitSkateboard() {
        this.skateboard = this.childLocator.FindChild("Skateboard").gameObject;
        this.skateboardBase = this.childLocator.FindChild("BoardBase");
        this.skateboardHandBase = this.childLocator.FindChild("BoardHandBase");

        this.enforcerVRComponent?.InitSkateBoardBase(skateboardBase);

        ReparentSkateboard(SkateBoardParent.HAND);
    }

    public void ReparentSkateboard(SkateBoardParent newParent) {
        if (!this.skateboard) return;

        switch (newParent) {
            case SkateBoardParent.BASE:
                this.skateboard.transform.SetParent(this.skateboardBase);
                this.skateboard.transform.localPosition = Vector3.zero;
                if (this.footStep) this.footStep.baseFootstepString = "";
                if (this.sfx) this.sfx.landingSound = Sounds.SkateLand;
                break;
            case SkateBoardParent.HAND:
                this.skateboard.transform.SetParent(this.skateboardHandBase);
                this.skateboard.transform.localPosition = Vector3.zero;
                if (this.footStep) this.footStep.baseFootstepString = this.stepSoundString;
                if (this.sfx) this.sfx.landingSound = this.landSoundString;
                break;
        }

        this.skateboard.transform.localPosition = Vector3.zero;
        this.skateboard.transform.localRotation = Quaternion.identity;

        this.enforcerVRComponent?.ReparentSkateboard(newParent);
    }

    void OnDestroy() {
        if (this.shellObjects != null && this.shellObjects.Length > 0) {
            for (int i = 0; i < this.shellObjects.Length; i++) {
                if (this.shellObjects[i]) Destroy(this.shellObjects[i]);
            }
        }
    }
}