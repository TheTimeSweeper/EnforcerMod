using RoR2;
using System;
using UnityEngine;

public class EnforcerWeaponComponent : MonoBehaviour
{
    public static event Action<int> Imp = delegate { };

    private CharacterBody charBody;
    private CharacterMotor charMotor;
    private HealthComponent charHealth;
    private ChildLocator childLocator;
    private InputBankTest inputBank;
    private ParticleSystem shellController;
    private int impCount;

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

        Invoke("ModelCheck", 0.2f);
    }

    public void ModelCheck()
    {
        if (charBody && charBody.master)
        {
            if (charBody.master.inventory)
            {
                var characterModel = charBody.modelLocator.modelTransform.GetComponentInChildren<CharacterModel>();
                if (characterModel)
                {
                    characterModel.baseRendererInfos[0].defaultMaterial = characterModel.gameObject.GetComponent<ModelSkinController>().skins[charBody.skinIndex].rendererInfos[0].defaultMaterial;
                    if (charBody.master.inventory.GetItemCount(ItemIndex.ArmorReductionOnHit) > 0) characterModel.baseRendererInfos[0].defaultMaterial = null;
                }
            }
        }
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
            else if (charBody.skillLocator.primary.skillDef.skillNameToken == "ENFORCER_PRIMARY_HAMMER_NAME")
            {
                return 3;
            }
            else if (charBody.skillLocator.primary.skillDef.skillNameToken == "SKILL_LUNAR_PRIMARY_REPLACEMENT_NAME")
            {
                return 4;
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

    public void ResetWeapon()
    {
        EquipWeapon(GetWeapon());
    }

    private void EquipWeapon(int weapon)
    {
        if (childLocator)
        {
            childLocator.FindChild("Shotgun").gameObject.SetActive(false);
            childLocator.FindChild("Rifle").gameObject.SetActive(false);
            childLocator.FindChild("SuperShotgun").gameObject.SetActive(false);
            childLocator.FindChild("Hammer").gameObject.SetActive(false);
            childLocator.FindChild("Needler").gameObject.SetActive(false);

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

    private void SetWeaponDisplays(int newWeapon)
    {
        ItemDisplayRuleSet ruleset = GetComponentInChildren<CharacterModel>().itemDisplayRuleSet;
        //for use in the future with item displays attached to the gun
        // actually don't even need this rn. maybe for energy shield tho
        // definitely for doom shield. coming soon:tm:

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
                case 3:
                    charBody.crosshairPrefab = Resources.Load<GameObject>("prefabs/crosshair/SimpleDotCrosshair");
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

        GameObject desiredShell = EnforcerPlugin.Assets.shotgunShell;
        if (GetWeapon() == 2) desiredShell = EnforcerPlugin.Assets.superShotgunShell;

        GameObject shellObject = GameObject.Instantiate<GameObject>(desiredShell, childLocator.FindChild("GrenadeMuzzle"));

        shellObject.transform.localPosition = Vector3.zero;
        shellObject.transform.localRotation = Quaternion.identity;

        shellController = shellObject.GetComponentInChildren<ParticleSystem>();

        shellController.transform.localPosition = new Vector3(0, -0.1f, 0);

        var x = shellController.main;
        x.simulationSpace = ParticleSystemSimulationSpace.World;
    }

    public void DropShell()
    {
        if (childLocator is null) return;

        if (shellController) shellController.Play();
    }
}
