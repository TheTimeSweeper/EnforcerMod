using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class copycat : MonoBehaviour {

    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Animator origAnimator;

    [SerializeField]
    private List<string> floats;
    [SerializeField]
    private List<string> bools;

    private void Update() {

        for (int i = 0; i < floats.Count; i++) {
            animator.SetFloat(floats[i], origAnimator.GetFloat(floats[i]));
        }

        for (int i = 0; i < bools.Count; i++) {
            animator.SetBool(bools[i], origAnimator.GetBool(bools[i]));
        }
    }
}
