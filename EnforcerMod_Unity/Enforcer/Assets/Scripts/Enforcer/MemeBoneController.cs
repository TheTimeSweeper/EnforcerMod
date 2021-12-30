﻿using UnityEngine;

public class MemeBoneController : MonoBehaviour
{
    [SerializeField]
    private Transform origBone;

    void Update()
    {
        if (!origBone) return;
        origBone.transform.rotation = transform.rotation;
        origBone.transform.position = transform.position;
    }

#if UNITY_EDITOR
    public Transform OrigBone { get => origBone; set => origBone = value; }
#endif
}