using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationdamptest : MonoBehaviour
{
    public float set;
    public bool flip;
    public float damp;
    public string setFloat;
    public Animator animator;

    // Update is called once per frame
    void Update()
    {
        animator?.SetFloat(setFloat, flip?set:-set, damp, Time.deltaTime);   
    }
}
