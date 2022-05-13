using UnityEngine;

public class shieldToggler : MonoBehaviour {

    [SerializeField]
    private GameObject[] thing1;

    [SerializeField]
    private GameObject[] thing2;

    bool tog;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.H)) {
            tog = !tog;

            foreach (var item in thing1) {
                Debug.Log("thing1 " + tog);
                item.SetActive(tog);
            }
            foreach (var item in thing2) {
                Debug.Log("thing2 " + !tog);
                item.SetActive(!tog);
            }
        }
    }
}
