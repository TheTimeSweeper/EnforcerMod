using RoR2;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayTransferTool : MonoBehaviour
{

    [SerializeField]
    private GameObject[] objeys;

    [ContextMenu("send to rendererinfos")]
    public void sendRendererInfos() {

        CharacterModel charaModel = GetComponent<CharacterModel>();

        if (charaModel == null) {

            Debug.LogError("no charactermodel attached");
            return;
        }

#if UNITY_EDITOR
        UnityEditor.Undo.RecordObject(charaModel, "grab rends");
#endif

        charaModel.baseRendererInfos = new CharacterModel.RendererInfo[objeys.Length];
        for (int i = 0; i < objeys.Length; i++) {

            Renderer rend = objeys[i].GetComponent<Renderer>();

            charaModel.baseRendererInfos[i] = new CharacterModel.RendererInfo {
                renderer = rend,
                defaultMaterial = rend.sharedMaterial,
                defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
            };
        }

        objeys = new GameObject[0];
    }

    [ContextMenu("send to childLocator")]
    public void sendChildLocator() {

        ChildLocator childLocator = GetComponent<ChildLocator>();

        if (childLocator == null) {

            Debug.LogError("no ChildLocator attached");
            return;
        }

#if UNITY_EDITOR
        UnityEditor.Undo.RecordObject(childLocator, "grab transes");
#endif

        int originalLength = childLocator.transformPairs.Length;
        Array.Resize(ref childLocator.transformPairs, childLocator.transformPairs.Length + objeys.Length);
        for (int i = 0; i < objeys.Length; i++) {

            childLocator.transformPairs[i + originalLength] = new ChildLocator.NameTransformPair {
                name = objeys[i].name,
                transform = objeys[i].transform,
            };
        }

        objeys = new GameObject[0];
    }

}
