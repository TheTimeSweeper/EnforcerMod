using RoR2;
using System;
using UnityEngine;

public class EnforcerWeaponComponent : MonoBehaviour
{
    public static event Action<int> Imp = delegate { };

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

        this.InitWeapon();
        this.InitShells();
        this.InitSkateboard();

        this.Invoke("ModelCheck", 0.2f);

        this.UpdateCamera();
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
            this.cameraShit.cameraParams.standardLocalCameraPos = new Vector3(0, 0f, -12);
        }
        else
        {
            if (!this.shieldUp)
            {
                this.cameraShit.cameraParams.standardLocalCameraPos = new Vector3(0, 0f, -12);
            }
            else
            {
                this.cameraShit.cameraParams.standardLocalCameraPos = new Vector3(1.8f, -0.5f, -6f);
            }
        }
    }


    private int GetWeapon()
    {
        int weapon = -1;

        if (this.charBody && this.charBody.skillLocator)
        {
            string skillString = this.charBody.skillLocator.primary.skillDef.skillNameToken;
            switch (skillString)
            {
                case "ENFORCER_PRIMARY_SHOTGUN_NAME":
                    weapon = 0;
                    break;
                case "ENFORCER_PRIMARY_RIFLE_NAME":
                    weapon = 1;
                    break;
                case "ENFORCER_PRIMARY_SUPERSHOTGUN_NAME":
                    weapon = 2;
                    break;
                case "ENFORCER_PRIMARY_HAMMER_NAME":
                    weapon = 3;
                    break;
                case "SKILL_LUNAR_PRIMARY_REPLACEMENT_NAME":
                    weapon = 4;
                    break;
            }
        }

        return weapon;
    }

    private int GetShield()
    {
        int shield = 0;

        if (this.charBody)
        {
            //if (this.charBody.skinIndex == EnforcerPlugin.EnforcerPlugin.doomGuyIndex) shield = 1;
            //if (this.charBody.skinIndex == EnforcerPlugin.EnforcerPlugin.engiIndex) shield = 2;
        }

        return shield;
    }

    public void InitWeapon()
    {
        int weapon = GetWeapon();
        this.EquipWeapon(weapon);
        this.SetCrosshair(weapon);
        //SetWeaponDisplays(weapon);
        this.SetShieldDisplayRules(GetShield());
    }

    public void HideWeapon()
    {
        return;
        if (this.childLocator)
        {
            this.childLocator.FindChild("Shotgun").gameObject.SetActive(false);
            this.childLocator.FindChild("Rifle").gameObject.SetActive(false);
            this.childLocator.FindChild("SuperShotgun").gameObject.SetActive(false);
            this.childLocator.FindChild("Hammer").gameObject.SetActive(false);
            this.childLocator.FindChild("Needler").gameObject.SetActive(false);
        }
    }

    public void DelayedResetWeapon()
    {
        this.Invoke("ResetWeapon", 0.1f);
    }

    public void ResetWeapon()
    {
        int weapon = GetWeapon();
        this.EquipWeapon(weapon);
        this.SetCrosshair(weapon);
    }

    private void EquipWeapon(int weapon)
    {
        return;
        if (this.childLocator)
        {
            this.HideWeapon();

            switch (weapon)
            {
                case 0:
                    this.childLocator.FindChild("Shotgun").gameObject.SetActive(true);
                    break;
                case 1:
                    this.childLocator.FindChild("Rifle").gameObject.SetActive(true);
                    break;
                case 2:
                    this.childLocator.FindChild("SuperShotgun").gameObject.SetActive(true);
                    break;
                case 3:
                    this.childLocator.FindChild("Hammer").gameObject.SetActive(true);
                    break;
                case 4:
                    this.childLocator.FindChild("Needler").gameObject.SetActive(true);
                    break;
            }
        }
    }

    private void SetShieldDisplayRules(int newShield)
    {
        /*ItemDisplayRuleSet ruleset = this.GetComponentInChildren<CharacterModel>().itemDisplayRuleSet;

        if (newShield == 0)
        {
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
        }
        else if (newShield == 1 || newShield == 2)
        {
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
        }*/
    }

    private void SetWeaponDisplays(int newWeapon)
    {
        return;
        ItemDisplayRuleSet ruleset = this.GetComponentInChildren<CharacterModel>().itemDisplayRuleSet;

        if (newWeapon == 0)
        {
            //ruleset.FindItemDisplayRuleGroup("Behemoth").rules[0].childName = "Shotgun";
        }
        else if (newWeapon == 1)
        {

        }
    }

    private void SetCrosshair(int weapon)
    {
        if (this.charBody)
        {
            switch (weapon)
            {
                case 0:
                    this.charBody.crosshairPrefab = Resources.Load<GameObject>("Prefabs/Crosshair/SMGCrosshair");
                    break;
                case 1:
                    this.charBody.crosshairPrefab = Resources.Load<GameObject>("Prefabs/Crosshair/StandardCrosshair");
                    break;
                case 2:
                    this.charBody.crosshairPrefab = Resources.Load<GameObject>("Prefabs/Crosshair/BanditCrosshair");
                    break;
                case 3:
                    this.charBody.crosshairPrefab = Resources.Load<GameObject>("Prefabs/Crosshair/SimpleDotCrosshair");
                    break;
                case 4:
                    this.charBody.crosshairPrefab = EnforcerPlugin.EnforcerModPlugin.needlerCrosshair;
                    break;
            }
        }
    }

    public void AddImp(int asdf)
    {
        this.impCount += asdf;

        Imp(this.impCount);
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
        if (this.GetWeapon() == 2) desiredShell = EnforcerPlugin.Assets.superShotgunShell;

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

    private void InitSkateboard()
    {
        return;
        if (this.childLocator)
        {
            this.skateboard = this.childLocator.FindChild("Skateboard").gameObject;
            this.skateboardBase = this.childLocator.FindChild("BoardBase");
            this.skateboardHandBase = this.childLocator.FindChild("BoardHandBase");
        }
    }

    public void ReparentSkateboard(string newParent)
    {
        return;
        if (!this.skateboard) return;

        switch (newParent)
        {
            case "Base":
                this.skateboard.transform.parent = this.skateboardBase;
                if (this.footStep) this.footStep.baseFootstepString = "";
                if (this.sfx) this.sfx.landingSound = EnforcerPlugin.Sounds.SkateLand;
                break;
            case "Hand":
                this.skateboard.transform.parent = this.skateboardHandBase;
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