using EntityStates.Enforcer;
using RoR2;
using System;
using UnityEngine;

public class EnforcerWeaponComponent : MonoBehaviour 
{
    public enum EquippedGun {
        GUN,
        SUPER,
        HMG,
        HAMMER,
        NEEDLER
    }

    public enum EquippedShield {
        SHIELD,
        SHIELD2, //I still believe man
        BOARD
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

    private GameObject shieldObject { get => this.childLocator.FindChild("ShieldModel").gameObject; }
    //private GameObject shielDevicedObject { get => this.childLocator.FindChild("ShieldDeviceModel").gameObject; }
    private GameObject skateBoardObject { get => this.childLocator.FindChild("SkamteBordModel").gameObject; }

    public bool isMultiplayer;

    public static int maxShellCount = 12;

    public bool shieldUp;

    private GameObject skateboard;
    private Transform skateboardBase;
    private Transform skateboardHandBase;

    private FootstepHandler footStep;
    private SfxLocator sfx;

    private string stepSoundString;
    private string landSoundString;

    private CharacterBody charBody;
    private CharacterMotor charMotor;
    private HealthComponent charHealth;
    private CameraTargetParams cameraShit;
    private ChildLocator childLocator;
    private int impCount;
    private int currentShell;
    private GameObject[] shellObjects;


    private void Start()
    {
        this.charBody = this.GetComponentInChildren<CharacterBody>();
        this.charMotor = this.GetComponentInChildren<CharacterMotor>();
        this.charHealth = this.GetComponentInChildren<HealthComponent>();
        this.cameraShit = this.GetComponent<CameraTargetParams>();
        this.childLocator = this.GetComponentInChildren<ChildLocator>();
        this.footStep = this.GetComponentInChildren<FootstepHandler>();
        this.sfx = this.GetComponentInChildren<SfxLocator>();

        if (this.footStep) this.stepSoundString = this.footStep.baseFootstepString;
        if (this.sfx) this.landSoundString = this.sfx.landingSound;

        this.impCount = 0;

        this.InitWeapons();
        this.InitShells();
        this.InitSkateboard();

        this.Invoke("ModelCheck", 0.2f);

        this.UpdateCamera();
    }

    public void InitWeapons()
    {
        this.HideWeapons();

        EquippedGun weapon = GetWeapon();
        this.EquipWeapon(weapon);
        this.SetCrosshair(weapon);
        //SetWeaponDisplayRules(weapon);

        EquippedShield shield = GetShield();
        this.EquipShield(shield);
        this.SetShieldDisplayRules(shield);
    }

    public void HideWeapons()
    {
        if (this.childLocator)
        {
            shotgunObject.SetActive(false);
            ssgobject.gameObject.SetActive(false);
            hmgObject.SetActive(false);
            hammerObject.SetActive(false);
            needlerObject.SetActive(false);

            shieldObject.SetActive(false);
            skateBoardObject.SetActive(false);
        }
    }


    #region weapons
    private EquippedGun GetWeapon() {
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
                    //case "SKILL_LUNAR_PRIMARY_REPLACEMENT_NAME":
                    //    weapon = equippedGun.NEEDLER;
                    //    break;
            }
        }

        return weapon;
    }
    
    private void EquipWeapon(EquippedGun weapon)
    {
        if (this.childLocator)
        {
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

    private void SetCrosshair(EquippedGun weapon)
    {
        if (this.charBody)
        {
            switch (weapon) {
                case EquippedGun.GUN:
                case EquippedGun.SUPER:
                case EquippedGun.HMG: 
                    this.charBody.crosshairPrefab = Resources.Load<GameObject>("Prefabs/Crosshair/SMGCrosshair");
                    break;
                case EquippedGun.HAMMER:
                    this.charBody.crosshairPrefab = Resources.Load<GameObject>("Prefabs/Crosshair/SimpleDotCrosshair");
                    break;
                case EquippedGun.NEEDLER:
                    this.charBody.crosshairPrefab = EnforcerPlugin.EnforcerModPlugin.needlerCrosshair;
                    break;
            }
        }
    }

    //called when checking for needler
    public void DelayedResetWeapon() {
        this.Invoke("ResetWeapon", 0.1f);
    }

    public void ResetWeapon() {
        InitWeapons();
        //EquippedGun weapon = GetWeapon();
        //this.EquipWeapon(weapon);
        //this.SetCrosshair(weapon);
    }

    #endregion

    private EquippedShield GetShield() {

        EquippedShield shield = EquippedShield.SHIELD;

        if (this.charBody && this.charBody.skillLocator) {
            string skillString = this.charBody.skillLocator.special.skillDef.skillNameToken;
            switch (skillString) {
                default:

                case "ENFORCER_SPECIAL_SHIELDUP_NAME":

                    shield = EquippedShield.SHIELD;

                    //if (this.charBody.skinIndex == EnforcerPlugin.EnforcerPlugin.doomGuyIndex) 
                    //    shield = EquippedShield.SHIELD2;
                    //if (this.charBody.skinIndex == EnforcerPlugin.EnforcerPlugin.engiIndex)
                    //    shield = EquippedShield.SHIELD2;

                    break;
                    
                case "ENFORCER_SPECIAL_SHIELDON_NAME":
                    shield = EquippedShield.SHIELD2;
                    break;

                case "ENFORCER_SPECIAL_BOARDUP_NAME":
                    shield = EquippedShield.BOARD;
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
                    shieldObject.SetActive(true);
                    break;
                case EquippedShield.SHIELD2:
                    shieldObject.SetActive(false);
                    break;
                case EquippedShield.BOARD:
                    shieldObject.SetActive(false);
                    skateBoardObject.SetActive(true);
                    break;
            }
        }
    }

    private void SetShieldDisplayRules(EquippedShield shield)
    {
        /*ItemDisplayRuleSet ruleset = this.GetComponentInChildren<CharacterModel>().itemDisplayRuleSet;
        
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
            //case EquippedShield.BOARD?
        }*/
    }

    public void ModelCheck()
    {
        if (this.charBody && this.charBody.master)
        {
            if (this.charBody.master.inventory)
            {
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

    public void UpdateCamera()
    {
        this.isMultiplayer = Run.instance.participatingPlayerCount > 1;

        if (this.isMultiplayer)
        {
            this.cameraShit.cameraParams.standardLocalCameraPos = EnforcerMain.standardCameraPosition;
        }
        else
        {
            if (!this.shieldUp)
            {
                this.cameraShit.cameraParams.standardLocalCameraPos = EnforcerMain.standardCameraPosition;
            }
            else
            {
                this.cameraShit.cameraParams.standardLocalCameraPos = EnforcerMain.shieldCameraPosition;
            }
        }
    }

    private void InitShells()
    {
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

        GameObject desiredShell = EnforcerPlugin.Assets.shotgunShell;
        if (this.GetWeapon() == EquippedGun.SUPER) desiredShell = EnforcerPlugin.Assets.superShotgunShell;

        for (int i = 0; i < EnforcerWeaponComponent.maxShellCount; i++)
        {
            this.shellObjects[i] = GameObject.Instantiate(desiredShell, this.childLocator.FindChild("Gun"), false);
            this.shellObjects[i].transform.localScale = Vector3.one * 1.1f;
            this.shellObjects[i].SetActive(false);
            this.shellObjects[i].GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
        }
    }

    public void DropShell(Vector3 force)
    {
        //if (shellController) shellController.Play();
        if (this.shellObjects == null) return;

        if (this.shellObjects[currentShell] == null) return;

        Transform origin = this.childLocator.FindChild("Muzzle");

        this.shellObjects[currentShell].SetActive(false);

        this.shellObjects[currentShell].transform.position = origin.position;
        this.shellObjects[currentShell].transform.parent = null;

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

        ReparentSkateboard(SkateBoardParent.HAND);
    }

    public void ReparentSkateboard(SkateBoardParent newParent)
    {
        if (!this.skateboard) return;

        switch (newParent)
        {
            case SkateBoardParent.BASE:
                this.skateboard.transform.parent = this.skateboardBase;
                this.skateboard.transform.localPosition = Vector3.zero;
                if (this.footStep) this.footStep.baseFootstepString = "";
                if (this.sfx) this.sfx.landingSound = EnforcerPlugin.Sounds.SkateLand;
                break;
            case SkateBoardParent.HAND:
                this.skateboard.transform.parent = this.skateboardHandBase;
                this.skateboard.transform.localPosition = Vector3.zero;
                if (this.footStep) this.footStep.baseFootstepString = this.stepSoundString;
                if (this.sfx) this.sfx.landingSound = this.landSoundString;
                break;
        }

        this.skateboard.transform.localPosition = Vector3.zero;
        this.skateboard.transform.localRotation = Quaternion.identity;
    }

    private void OnDestroy()
    {
        if (this.shellObjects != null && this.shellObjects.Length > 0)
        {
            for (int i = 0; i < this.shellObjects.Length; i++)
            {
                if (this.shellObjects[i]) Destroy(this.shellObjects[i]);
            }
        }
    }
}