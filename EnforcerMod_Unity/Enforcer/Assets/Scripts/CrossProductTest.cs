using UnityEngine;

public class CrossProductTest: MonoBehaviour {

    public Transform aim;
    public Transform vUp;
    public Transform cross1transform;
    public Transform cross2transform;

    private Vector3 aimForward;
    private Vector3 cross1;
    private Vector3 cross2;

    void Update() {

        aimForward = aim.forward;

        vUp.LookAt(vUp.position + Vector3.up);

        cross1 = Vector3.Cross(aim.forward, Vector3.up);
        cross1transform.LookAt(cross1transform.position + cross1);


        cross2 = Vector3.Cross(cross1, aim.forward);
        cross2transform.LookAt(cross2transform.position + cross2);
    }
}
