﻿using UnityEngine;
public class ArmStrapRagdollHandler : MonoBehaviour {

    [SerializeField]
    private Transform ShieldBone;

    void OnDisable() {
        transform.parent = ShieldBone;
    }

}