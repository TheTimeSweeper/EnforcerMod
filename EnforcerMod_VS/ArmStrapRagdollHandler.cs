using UnityEngine;

public class ArmStrapRagdollHandler : MonoBehaviour {

    [SerializeField]
    private Transform ShieldBone;

    void OnDestroy() {
        transform.parent = ShieldBone;
    }

}
