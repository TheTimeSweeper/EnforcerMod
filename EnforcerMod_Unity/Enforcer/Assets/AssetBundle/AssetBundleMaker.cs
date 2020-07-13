#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AssetBundleMaker : MonoBehaviour
{
    [MenuItem ("Assets/AB Make")]
    static void BuildBundles()
    {
        BuildPipeline.BuildAssetBundles("Assets/AssetBundle",BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
    }
}
#endif