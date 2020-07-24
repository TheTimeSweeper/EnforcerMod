using RoR2;
using System;
using System.Collections.Specialized;
using UnityEngine;

public class ShieldComponent : MonoBehaviour
{
    static float maxSpeed = 0.1f;
    static float coef = 1; // affects how quickly it reaches max speed

    public bool isShielding = false;
    public Ray aimRay;
    public Vector3 shieldDirection = new Vector3(1,0,0);
    float initialTime = 0;

    public static GameObject bulletTracerEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/Tracers/TracerCommandoShotgun");

    private EnergyShieldControler energyShieldControler;

    private Transform _shieldPreview;
    private Transform _shieldParent;
    private float _shieldSize;
    private float _shieldSizeMultiplier = 1.2f;

    GameObject dummy;
    GameObject boyPrefab = Resources.Load<GameObject>("Prefabs/CharacterBodies/LemurianBody");

    void Start()
    {
        //enough of this tomfoolery
        //dummy = UnityEngine.Object.Instantiate<GameObject>(boyPrefab, aimRay.origin, Quaternion.LookRotation(shieldDirection));
        energyShieldControler = GetComponentInChildren<EnergyShieldControler>();
    }

    void Update() {
        energyShieldControler.aimRayDirection = aimRay.direction;

        aimShield();
    }

    private void aimShield() {
        float time = Time.fixedTime - initialTime;

        Vector3 cross = Vector3.Cross(aimRay.direction, shieldDirection);
        Vector3 turnDirection = Vector3.Cross(shieldDirection, cross);

        float turnSpeed = maxSpeed * (1 - Mathf.Exp(-1 * coef * time));

        shieldDirection += turnSpeed * turnDirection.normalized;
        shieldDirection = shieldDirection.normalized;

        Vector3 difference = aimRay.direction - shieldDirection;
        if (difference.magnitude < 0.05) {
            initialTime = Time.fixedTime;
        }

        displayShieldPreviewCube();

        allHeKnowsIsPain();
    }

    private void allHeKnowsIsPain() {

        if (dummy) {
            var hc = dummy.GetComponent<HealthComponent>();
            if (hc && hc.health <= 0) {
                //stop this madness i swear to god
                //respawnDummy();
            }

            dummy.transform.position = aimRay.origin + shieldDirection;
        }

        #region old Piss Stream
        //BulletAttack bullet = new RoR2.BulletAttack
        //{
        //    bulletCount = 1,
        //    aimVector = shieldDirection,
        //    origin = aimRay.origin,
        //    damage = 1,
        //    damageColorIndex = DamageColorIndex.Default,
        //    damageType = DamageType.Generic,
        //    falloffModel = BulletAttack.FalloffModel.None,
        //    maxDistance = 48,
        //    force = 1,
        //    hitMask = LayerIndex.CommonMasks.bullet,
        //    minSpread = 0,
        //    maxSpread = 12f,
        //    isCrit = false,
        //    owner = base.gameObject,
        //    muzzleName = "hey",
        //    smartCollision = false,
        //    procChainMask = default(ProcChainMask),
        //    procCoefficient =2,
        //    radius = 0.5f,
        //    sniper = false,
        //    stopperMask = LayerIndex.background.collisionMask,
        //    weapon = null,
        //    tracerEffectPrefab = bulletTracerEffectPrefab,
        //    spreadPitchScale = 0.5f,
        //    spreadYawScale = 0.5f,
        //    queryTriggerInteraction = QueryTriggerInteraction.UseGlobal
        //};
        //bullet.Fire();

        //RoR2.Chat.AddMessage("---------------");
        //RoR2.Chat.AddMessage("Aim Direction:    " + aimRay.direction.x.ToString() + ", " + aimRay.direction.y.ToString() + ", " + aimRay.direction.z.ToString());
        //RoR2.Chat.AddMessage("Shield Direction: " + shieldDirection.x.ToString() + ", " + shieldDirection.y.ToString() + ", " + shieldDirection.z.ToString());
        #endregion
    }

    private void displayShieldPreviewCube() {

        if (_shieldParent == null)
            findShieldParent();

        if (_shieldPreview == null)
            makeCube();

        if (!isShielding) {
            _shieldPreview.gameObject.SetActive(false);
            return;
        }
        _shieldPreview.gameObject.SetActive(true);

        alignCube();

        float angle = EnforcerPlugin.EnforcerPlugin.ShieldBlockAngle;
        float zDistance = _shieldSizeMultiplier;// _shieldPreview.localPosition.z

        _shieldSize = Mathf.Tan(Mathf.Deg2Rad * angle) * zDistance * 2;

        setCubeSize(_shieldSize);
    }

    private void findShieldParent() {
        _shieldParent = GetComponent<ModelLocator>().modelTransform;
    }

    private void makeCube() {

        _shieldPreview = _shieldParent.Find("ShieldPreviewCube");

        if (_shieldPreview != null) {

            _shieldPreview.parent = null;
            return;
        }

        _shieldPreview = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
        _shieldPreview.position = aimRay.origin + shieldDirection;
        _shieldPreview.localRotation = Quaternion.identity;
        Destroy(_shieldPreview.GetComponent<Collider>());
    }

    private void setCubeSize(float size) {
        //_shieldPreview.parent = null;
        _shieldPreview.localScale = new Vector3(size, size, _shieldPreview.localScale.z);
        //_shieldPreview.parent = _shieldParent;
    }

    private void alignCube() {

        _shieldPreview.LookAt(aimRay.origin + shieldDirection*2, Vector3.up);
        _shieldPreview.position = aimRay.origin + shieldDirection* _shieldSizeMultiplier;
    }

    private void respawnDummy()
    {
        dummy = UnityEngine.Object.Instantiate<GameObject>(boyPrefab, aimRay.origin, Quaternion.LookRotation(shieldDirection));
    }

    public void toggleEngergyShield()
    {
        energyShieldControler.Toggle();
    }
}