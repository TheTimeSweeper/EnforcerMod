using RoR2;
using System;
using UnityEngine;

public class EnforcerWeaponComponent : MonoBehaviour
{
    public static event Action<int> Imp = delegate { };

    public static int maxShellCount = 12;

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
    private ChildLocator childLocator;
    private ParticleSystem shellController;
    private int impCount;
    private int currentShell;
    private GameObject[] shellObjects;

    private void Start()
    {
        charBody = GetComponentInChildren<CharacterBody>();
        charMotor = GetComponentInChildren<CharacterMotor>();
        charHealth = GetComponentInChildren<HealthComponent>();
        childLocator = GetComponentInChildren<ChildLocator>();
        footStep = GetComponentInChildren<FootstepHandler>();
        sfx = GetComponentInChildren<SfxLocator>();

        if (footStep) stepSoundString = footStep.baseFootstepString;
        if (sfx) landSoundString = sfx.landingSound;

        impCount = 0;

        InitWeapon();
        InitShells();
        InitSkateboard();

        Invoke("ModelCheck", 0.2f);
    }

    public void ModelCheck()
    {
        if (charBody && charBody.master)
        {
            if (charBody.master.inventory)
            {
                if (EnforcerPlugin.EnforcerPlugin.sillyHammer.Value)
                {
                    var characterModel = charBody.modelLocator.modelTransform.GetComponentInChildren<CharacterModel>();
                    if (characterModel)
                    {
                        characterModel.baseRendererInfos[0].defaultMaterial = characterModel.gameObject.GetComponent<ModelSkinController>().skins[charBody.skinIndex].rendererInfos[0].defaultMaterial;
                        if (charBody.master.inventory.GetItemCount(ItemIndex.ArmorReductionOnHit) > 0) characterModel.baseRendererInfos[0].defaultMaterial = null;
                    }
                }
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

    private int GetWeapon()
    {
        int weapon = -1;

        if (charBody && charBody.skillLocator)
        {
            string skillString = charBody.skillLocator.primary.skillDef.skillNameToken;
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

        if (charBody)
        {
            if (charBody.skinIndex == EnforcerPlugin.EnforcerPlugin.doomGuyIndex) shield = 1;
            if (charBody.skinIndex == EnforcerPlugin.EnforcerPlugin.engiIndex) shield = 2;
        }

        return shield;
    }

    public void InitWeapon()
    {
        int weapon = GetWeapon();
        EquipWeapon(weapon);
        SetCrosshair(weapon);
        //SetWeaponDisplays(weapon);
        SetShieldDisplayRules(GetShield());
    }

    public void HideWeapon()
    {
        if (childLocator)
        {
            childLocator.FindChild("Shotgun").gameObject.SetActive(false);
            childLocator.FindChild("Rifle").gameObject.SetActive(false);
            childLocator.FindChild("SuperShotgun").gameObject.SetActive(false);
            childLocator.FindChild("Hammer").gameObject.SetActive(false);
            childLocator.FindChild("Needler").gameObject.SetActive(false);
        }
    }

    public void DelayedResetWeapon()
    {
        Invoke("ResetWeapon", 0.1f);
    }

    public void ResetWeapon()
    {
        int weapon = GetWeapon();
        EquipWeapon(weapon);
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
                    childLocator.FindChild("Shotgun").gameObject.SetActive(true);
                    break;
                case 1:
                    childLocator.FindChild("Rifle").gameObject.SetActive(true);
                    break;
                case 2:
                    childLocator.FindChild("SuperShotgun").gameObject.SetActive(true);
                    break;
                case 3:
                    childLocator.FindChild("Hammer").gameObject.SetActive(true);
                    break;
                case 4:
                    childLocator.FindChild("Needler").gameObject.SetActive(true);
                    break;
            }
        }
    }

    private void SetShieldDisplayRules(int newShield)
    {
        ItemDisplayRuleSet ruleset = GetComponentInChildren<CharacterModel>().itemDisplayRuleSet;

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
        }
    }

    private void SetWeaponDisplays(int newWeapon)
    {
        ItemDisplayRuleSet ruleset = GetComponentInChildren<CharacterModel>().itemDisplayRuleSet;

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
        if (charBody)
        {
            switch (weapon)
            {
                case 0:
                    charBody.crosshairPrefab = Resources.Load<GameObject>("Prefabs/Crosshair/SMGCrosshair");
                    break;
                case 1:
                    charBody.crosshairPrefab = Resources.Load<GameObject>("Prefabs/Crosshair/StandardCrosshair");
                    break;
                case 2:
                    charBody.crosshairPrefab = Resources.Load<GameObject>("Prefabs/Crosshair/BanditCrosshair");
                    break;
                case 3:
                    charBody.crosshairPrefab = Resources.Load<GameObject>("Prefabs/Crosshair/SimpleDotCrosshair");
                    break;
                case 4:
                    charBody.crosshairPrefab = EnforcerPlugin.EnforcerPlugin.needlerCrosshair;
                    break;
            }
        }
    }

    public void AddImp(int asdf)
    {
        impCount += asdf;

        Imp(impCount);
    }

    private void InitShells()
    {
        if (childLocator is null) return;

        /*GameObject desiredShell = EnforcerPlugin.Assets.shotgunShell;
        if (GetWeapon() == 2) desiredShell = EnforcerPlugin.Assets.superShotgunShell;

        GameObject shellObject = GameObject.Instantiate<GameObject>(desiredShell, childLocator.FindChild("GrenadeMuzzle"));

        shellObject.transform.localPosition = Vector3.zero;
        shellObject.transform.localRotation = Quaternion.identity;

        shellController = shellObject.GetComponentInChildren<ParticleSystem>();

        shellController.transform.localPosition = new Vector3(0, -0.1f, 0);

        var x = shellController.main;
        x.simulationSpace = ParticleSystemSimulationSpace.World;*/

        currentShell = 0;

        shellObjects = new GameObject[EnforcerWeaponComponent.maxShellCount + 1];

        GameObject desiredShell = EnforcerPlugin.Assets.shotgunShell;
        if (GetWeapon() == 2) desiredShell = EnforcerPlugin.Assets.superShotgunShell;

        for (int i = 0; i < EnforcerWeaponComponent.maxShellCount; i++)
        {
            shellObjects[i] = GameObject.Instantiate(desiredShell, childLocator.FindChild("Shotgun"), false);
            shellObjects[i].transform.localScale *= 0.075f;
            shellObjects[i].SetActive(false);
            shellObjects[i].GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
        }
    }

    public void DropShell(Vector3 force)
    {
        //if (shellController) shellController.Play();
        if (shellObjects == null) return;

        if (shellObjects[currentShell] == null) return;

        Transform origin = childLocator.FindChild("GrenadeMuzzle");

        shellObjects[currentShell].SetActive(false);

        shellObjects[currentShell].transform.position = origin.position;
        shellObjects[currentShell].transform.parent = null;

        shellObjects[currentShell].SetActive(true);

        Rigidbody rb = shellObjects[currentShell].gameObject.GetComponent<Rigidbody>();
        if (rb) rb.velocity = force;

        currentShell++;
        if (currentShell >= EnforcerWeaponComponent.maxShellCount) currentShell = 0;
    }

    private void InitSkateboard()
    {
        if (childLocator)
        {
            skateboard = childLocator.FindChild("Skateboard").gameObject;
            skateboardBase = childLocator.FindChild("BoardBase");
            skateboardHandBase = childLocator.FindChild("BoardHandBase");
        }
    }

    public void ReparentSkateboard(string newParent)
    {
        if (!skateboard) return;

        switch (newParent)
        {
            case "Base":
                skateboard.transform.parent = skateboardBase;
                if (footStep) footStep.baseFootstepString = "";
                if (sfx) sfx.landingSound = EnforcerPlugin.Sounds.SkateLand;
                break;
            case "Hand":
                skateboard.transform.parent = skateboardHandBase;
                if (footStep) footStep.baseFootstepString = stepSoundString;
                if (sfx) sfx.landingSound = landSoundString;
                break;
        }

        skateboard.transform.localPosition = Vector3.zero;
        skateboard.transform.localRotation = Quaternion.identity;
    }
}
