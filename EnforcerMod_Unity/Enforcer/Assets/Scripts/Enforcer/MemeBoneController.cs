using UnityEngine;

public class MemeBoneController : MonoBehaviour
{

    [SerializeField]
    private Transform origBone;
    public Transform OrigBone { get => origBone; set => origBone = value; }

    void Update()
    {
        if (!origBone) return;
        origBone.transform.rotation = transform.rotation;
        origBone.transform.position = transform.position;
    }
}
