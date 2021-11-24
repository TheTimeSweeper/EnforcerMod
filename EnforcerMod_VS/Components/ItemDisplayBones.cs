using UnityEngine;

public class ItemDisplayBones : MonoBehaviour {

    [SerializeField]
    private GameObject[] itemBones;

    private void OnEnable() {
        for (int i = 0; i < itemBones.Length; i++) {
            itemBones[i].SetActive(true);
        }
    }

    private void OnDisable() {
        for (int i = 0; i < itemBones.Length; i++) {
            itemBones[i].SetActive(false);
        }
    }
}
