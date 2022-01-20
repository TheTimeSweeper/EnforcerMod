using UnityEngine;

public class MemeBoneController : MonoBehaviour
{
    [SerializeField]
    private Transform origBone;

    [SerializeField, UnityEngine.Serialization.FormerlySerializedAs("position")]
    private bool positionAndRotation = true;

    [Header("LateUpdate")]
    [SerializeField]
    private bool localPosition = false;
    [SerializeField, UnityEngine.Serialization.FormerlySerializedAs("scale")]
    private bool localScale = false;

    void Update()
    {
        if (!origBone) return;
        if (positionAndRotation)
        {
            origBone.transform.rotation = transform.rotation;
            origBone.transform.position = transform.position;
        }
    }
    private void LateUpdate()
    {
        if (!origBone) return;
        if (localPosition)
            origBone.transform.localPosition = transform.localPosition;
        if (localScale)
            origBone.transform.localScale = transform.localScale;
    }
}
