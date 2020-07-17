using RoR2;
using UnityEngine;

public class EnforcerWeaponComponent : MonoBehaviour
{
    private CharacterBody charBody;
    private CharacterMotor charMotor;
    private HealthComponent charHealth;
    private ChildLocator childLocator;
    private InputBankTest inputBank;

    void Start()
    {
        charBody = GetComponentInChildren<CharacterBody>();
        charMotor = GetComponentInChildren<CharacterMotor>();
        charHealth = GetComponentInChildren<HealthComponent>();
        childLocator = GetComponentInChildren<ChildLocator>();
        inputBank = GetComponentInChildren<InputBankTest>();

        InitWeapon();
    }

    int GetWeapon()
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
        }
        return -1;
    }

    public void InitWeapon()
    {
        int weapon = GetWeapon();
        EquipWeapon(weapon);
        SetCrosshair(weapon);
        SetWeaponDisplays(weapon);
    }

    void EquipWeapon(int weapon)
    {
        if (childLocator)
        {
            if (weapon == 0)
            {
                childLocator.FindChild("Shotgun").gameObject.SetActive(true);
                childLocator.FindChild("Rifle").gameObject.SetActive(false);
            }
            else if (weapon == 1)
            {
                childLocator.FindChild("Shotgun").gameObject.SetActive(false);
                childLocator.FindChild("Rifle").gameObject.SetActive(true);
            }
        }
    }

    void SetWeaponDisplays(int newWeapon)
    {
        ItemDisplayRuleSet ruleset = GetComponentInChildren<CharacterModel>().itemDisplayRuleSet;
        //for use in the future with item displays attached to the gun

        if (newWeapon == 0)
        {
            //ruleset.FindItemDisplayRuleGroup("Behemoth").rules[0].childName = "Shotgun";
        }
        else if (newWeapon == 1)
        {

        }
    }

    void SetCrosshair(int weapon)
    {
        if (charBody)
        {
            if (weapon == 0)
            {
                charBody.crosshairPrefab = Resources.Load<GameObject>("prefabs/crosshair/SMGCrosshair");
            }
            else if (weapon == 1)
            {
                charBody.crosshairPrefab = Resources.Load<GameObject>("prefabs/crosshair/StandardCrosshair");
            }
        }
    }
}
