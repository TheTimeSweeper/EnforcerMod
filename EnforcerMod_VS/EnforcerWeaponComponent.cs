using RoR2;
using System;
using UnityEngine;

public class EnforcerWeaponComponent : MonoBehaviour
{
    public static event Action<int> Imp = delegate { };

    public static int maxShellCount = 12;

    private CharacterBody charBody;
    private CharacterMotor charMotor;
    private HealthComponent charHealth;
    private ChildLocator childLocator;
    private InputBankTest inputBank;
    private int impCount;
    private int currentShell;
    private GameObject[] shellObjects;

    private void Start()
    {
        charBody = GetComponentInChildren<CharacterBody>();
        charMotor = GetComponentInChildren<CharacterMotor>();
        charHealth = GetComponentInChildren<HealthComponent>();
        childLocator = GetComponentInChildren<ChildLocator>();
        inputBank = GetComponentInChildren<InputBankTest>();
        impCount = 0;

        InitWeapon();
        InitShells();
    }

    private int GetWeapon()
    {
        if (charBody && charBody.skillLocator)
        {
            if (charBody.skillLocator.primary.skillDef.skillNameToken == "ENFORCER_PRIMARY_SHOTGUN_NAME")
            {
                return 0;
            }
            else if (charBody.skillLocator.primary.skillDef.skillNameToken == "ENFORCER_PRIMARY_RIFLE_NAME")
            {
                return 1;
            }
            else if (charBody.skillLocator.primary.skillDef.skillNameToken == "ENFORCER_PRIMARY_SUPERSHOTGUN_NAME")
            {
                return 2;
            }
        }
        return -1;
    }

    public void InitWeapon()
    {
        int weapon = GetWeapon();
        EquipWeapon(weapon);
        SetCrosshair(weapon);
        //SetWeaponDisplays(weapon);
    }

    private void EquipWeapon(int weapon)
    {
        if (childLocator)
        {
            switch (weapon)
            {
                case 0:
                    childLocator.FindChild("Shotgun").gameObject.SetActive(true);
                    childLocator.FindChild("Rifle").gameObject.SetActive(false);
                    childLocator.FindChild("SuperShotgun").gameObject.SetActive(false);
                    break;
                case 1:
                    childLocator.FindChild("Shotgun").gameObject.SetActive(false);
                    childLocator.FindChild("Rifle").gameObject.SetActive(true);
                    childLocator.FindChild("SuperShotgun").gameObject.SetActive(false);
                    break;
                case 2:
                    childLocator.FindChild("Shotgun").gameObject.SetActive(false);
                    childLocator.FindChild("Rifle").gameObject.SetActive(false);
                    childLocator.FindChild("SuperShotgun").gameObject.SetActive(true);
                    break;
            }
        }
    }

    private void SetWeaponDisplays(int newWeapon)
    {
        ItemDisplayRuleSet ruleset = GetComponentInChildren<CharacterModel>().itemDisplayRuleSet;
        //for use in the future with item displays attached to the gun
        // actually don't even need this rn. maybe for energy shield tho

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
                    charBody.crosshairPrefab = Resources.Load<GameObject>("prefabs/crosshair/SMGCrosshair");
                    break;
                case 1:
                    charBody.crosshairPrefab = Resources.Load<GameObject>("prefabs/crosshair/StandardCrosshair");
                    break;
                case 2:
                    charBody.crosshairPrefab = Resources.Load<GameObject>("prefabs/crosshair/BanditCrosshair");
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
        if (!childLocator) return;

        currentShell = 0;

        shellObjects = new GameObject[EnforcerWeaponComponent.maxShellCount + 1];

        for(int i = 0; i < EnforcerWeaponComponent.maxShellCount; i++)
        {
            shellObjects[i] = GameObject.Instantiate(EnforcerPlugin.Assets.shotgunShell, childLocator.FindChild("GrenadeMuzzle"), false);
            shellObjects[i].transform.localScale *= 0.075f;
            shellObjects[i].layer = LayerIndex.uiWorldSpace.intVal;
            shellObjects[i].SetActive(false);
        }
    }

    public void DropShell(Vector3 force)
    {
        if (shellObjects == null) return;
        if (childLocator == null) return;

        if (shellObjects[currentShell] == null) return;

        Transform origin = childLocator.FindChild("GrenadeMuzzle");

        shellObjects[currentShell].SetActive(false);

        shellObjects[currentShell].transform.position = origin.position - origin.right;
        shellObjects[currentShell].transform.rotation = Quaternion.identity;
        shellObjects[currentShell].transform.parent = null;

        shellObjects[currentShell].SetActive(true);

        Rigidbody rb = shellObjects[currentShell].gameObject.GetComponent<Rigidbody>();
        if (rb) rb.velocity = force;

        currentShell++;
        if (currentShell >= EnforcerWeaponComponent.maxShellCount) currentShell = 0;
    }
}
