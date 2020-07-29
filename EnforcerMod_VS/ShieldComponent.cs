using RoR2;
using System;
using System.Collections.Specialized;
using UnityEngine;

public class ShieldComponent : MonoBehaviour
{
    static float maxSpeed = 0.1f;
    static float coef = 1; // affects how quickly it reaches max speed

    public bool isAlternate = false;
    public bool isShielding = false;
    public Ray aimRay;
    public Vector3 shieldDirection = new Vector3(1,0,0);
    float initialTime = 0;

    //private EnergyShieldControler energyShieldControler;

    private Transform _shieldPreview;
    private Transform _shieldParent;
    private float _shieldSize;
    private float _shieldSizeMultiplier = 1.2f;

    GameObject dummy;
    GameObject boyPrefab = Resources.Load<GameObject>("Prefabs/CharacterBodies/LemurianBody");

    private Light[] lights;
    private int lightCounter = 201;
    /*public float shieldHealth {
        get => energyShieldControler.healthComponent.health;
    }*/

    void Start()
    {
        //energyShieldControler = GetComponentInChildren<EnergyShieldControler>();

        lights = GetComponentsInChildren<Light>();
    }

    void Update() {

        aimShield();

        //energyShieldControler.shieldAimRayDirection = aimRay.direction;

        handleShoulderLights();
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

        //displayShieldPreviewCube();
    }

    // this code is inexplicably in this class because I am lazy
    private void handleShoulderLights()
    {
        if (lightCounter < 100)
        {
            if (lightCounter % 10 == 0)
            {
                lights[0].enabled = !lights[0].enabled;
                lights[1].enabled = !lights[1].enabled;
            }

            lightCounter++;
        }
        else
        {
            lights[0].enabled = false;
            lights[1].enabled = false;
        }
    }

    #region previews

    //here we pay respect to Prometheus
    //a lifeless commando clone
    //cast to eternal torture for giving modders fire

    /*private void displayShieldPreviewCube() {

        if (_shieldParent == null)
            findShieldParent();

        if (_shieldPreview == null)
            findPreviewCube();

        if (!isShielding) {
            _shieldPreview.gameObject.SetActive(false);
            return;
        }
        _shieldPreview.gameObject.SetActive(true);

        alignCube();

        float angle = EnforcerPlugin.EnforcerPlugin.ShieldBlockAngle;
        float zDistance = _shieldSizeMultiplier;// _shieldPreview.localPosition.z

        _shieldSize = Mathf.Tan(Mathf.Deg2Rad * angle) * zDistance * 2;

        setPreviewCubeSize(_shieldSize);
    }

    private void findShieldParent() {
        //character body
        _shieldParent = GetComponent<ModelLocator>().modelTransform;
    }

    private void findPreviewCube() {

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

    private void setPreviewCubeSize(float size) {
        //_shieldPreview.parent = null;
        _shieldPreview.localScale = new Vector3(size, size, _shieldPreview.localScale.z);
        //_shieldPreview.parent = _shieldParent;
    }

    private void alignCube() {

        _shieldPreview.LookAt(aimRay.origin + shieldDirection*2, Vector3.up);
        _shieldPreview.position = aimRay.origin + shieldDirection* _shieldSizeMultiplier;
    }*/
    #endregion

    public void toggleEngergyShield()
    {
        //energyShieldControler.Toggle();
    }

    public void flashLights()
    {
        lightCounter = 0;
    }
}