using UnityEngine;
using RoR2;

public class EnforcerItemDisplayEditorComponent : MonoBehaviour {

    private ChildLocator childLocator;

    //i'm sure there's a better way to get the main EnforcerWeaponComponent to cover this with less copy paste but oh well
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

    private GameObject shotgunObject { get => this.childLocator.FindChild("GunModel").gameObject; }
    private GameObject ssgobject { get => this.childLocator.FindChild("SuperGunModel").gameObject; }
    private GameObject hmgObject { get => this.childLocator.FindChild("HMGModel").gameObject; }
    private GameObject hammerObject { get => this.childLocator.FindChild("HammerModel").gameObject; }
    private GameObject needlerObject { get => this.childLocator.FindChild("NeedlerModel").gameObject; }

    private GameObject shieldObject { get => this.childLocator.FindChild("ShieldModel").gameObject; }
    //private GameObject shielDeviceObject { get => this.childLocator.FindChild("ShieldDeviceModel").gameObject; }
    private GameObject skateBoardObject { get => this.childLocator.FindChild("SkamteBordModel").gameObject; }

    void Start() {

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "KingEnderBrine_IDRS_Editor") {
            this.childLocator = this.GetComponentInChildren<ChildLocator>();
            HideWeapons();
            EquipWeapon(EquippedGun.GUN);
        }
    }

    public void HideWeapons() {

        shotgunObject.SetActive(true);
        ssgobject.gameObject.SetActive(true);
        hmgObject.SetActive(true);
        hammerObject.SetActive(true);
        needlerObject.SetActive(true);

        shotgunObject.SetActive(false);
        ssgobject.gameObject.SetActive(false);
        hmgObject.SetActive(false);
        hammerObject.SetActive(false);
        needlerObject.SetActive(false);

        //shieldObject.SetActive(false);
        //skateBoardObject.SetActive(false);
    }

    void Update() {

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "KingEnderBrine_IDRS_Editor") {

            if (Input.anyKeyDown) {

                EquippedGun? gun = null;

                if (Input.GetKeyDown(KeyCode.Alpha1)) {
                    gun = EquippedGun.GUN;
                }
                if (Input.GetKeyDown(KeyCode.Alpha2)) {
                    gun = EquippedGun.SUPER;
                }
                if (Input.GetKeyDown(KeyCode.Alpha3)) {
                    gun = EquippedGun.HMG;
                }
                if (Input.GetKeyDown(KeyCode.Alpha4)) {
                    gun = EquippedGun.HAMMER;
                }
                if (Input.GetKeyDown(KeyCode.Alpha5)) {
                    gun = EquippedGun.NEEDLER;
                }

                if (gun != null) {
                    HideWeapons();
                    EquipWeapon((EquippedGun)gun);
                }
            }
        }
    }

    private void EquipWeapon(EquippedGun weapon) {
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
