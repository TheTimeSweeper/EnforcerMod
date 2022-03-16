using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shieldToggler : MonoBehaviour
{

    [SerializeField]
    private GameObject[] thing1;

    [SerializeField]
    private GameObject[] thing2;

    bool tog;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.H)) {
            foreach (var item in thing1) {
                item.SetActive(tog);
            }
            foreach (var item in thing2) {
                item.SetActive(!tog);
            }
            tog = !tog;
        }
    }
}
