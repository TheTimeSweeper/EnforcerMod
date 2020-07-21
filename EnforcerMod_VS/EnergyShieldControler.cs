using RoR2;
using UnityEngine;

public class EnergyShieldControler : MonoBehaviour
{
    public Vector3 aimRayDirection = new Vector3(0, 0, 0);

    private MeshCollider collider;
    private MeshRenderer[] renderers;
    private float angle;

    void Start()
    {
        collider = GetComponentInChildren<MeshCollider>();
        renderers = GetComponentsInChildren<MeshRenderer>();

        collider.enabled = false;
        for (int i = 0; i < 2; i++)
        {
            renderers[i].enabled = false;
        }
    }

    void Update()
    {
        Vector3 horizontal = new Vector3(aimRayDirection.x, 0, aimRayDirection.z);
        float sign = -1 * (aimRayDirection.y / Mathf.Abs(aimRayDirection.y));
        float theta = sign * Vector3.Angle(aimRayDirection, horizontal);

        transform.Rotate(new Vector3(theta - angle, 0, 0));

        angle = theta;
    }

    public void Toggle()
    {
        collider.enabled = !collider.enabled;
        for (int i = 0; i < 2; i++)
        {
            renderers[i].enabled = !renderers[i].enabled;
        }
    }
}
