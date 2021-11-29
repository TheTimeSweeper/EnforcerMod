using UnityEngine;

public class EngiShieldTex : MonoBehaviour {

    [SerializeField]
    private float offsetRate = 0.1f;

    private float offsetRateVSOverride = -1;

    private Renderer _rend;

    private float _offset;

    private MaterialPropertyBlock matProperties;
    private Vector2 tiling;

    void Start() {

        //offsetRate is set in Unity Inspector, 
        //this is if we want to change this in code instead of making a new assetbundle
        offsetRate = offsetRateVSOverride == -1 ? offsetRate : offsetRateVSOverride;

        _rend = GetComponent<Renderer>();

        matProperties = new MaterialPropertyBlock();

        tiling = _rend.material.GetTextureScale("_MainTex");
    }
    bool col = false;
    void Update() {

        _offset += offsetRate * Time.deltaTime;
        if (_offset > 1) {
            _offset -= 1;
        }

        _rend.GetPropertyBlock(matProperties);
        matProperties.SetVector("_MainTex_ST", new Vector4(tiling.x, tiling.y, _offset, _offset));
        _rend.SetPropertyBlock(matProperties);
    }
}
