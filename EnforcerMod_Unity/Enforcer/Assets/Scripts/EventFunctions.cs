using UnityEngine;
// Token: 0x02000261 RID: 609

namespace RoR2 {
    public class EventFunctions : MonoBehaviour {

		public void DisableAllChildrenExcept(GameObject objectToEnable) {
			for (int i = base.transform.childCount - 1; i >= 0; i--) {
				GameObject gameObject = base.transform.GetChild(i).gameObject;
				if (!(gameObject == objectToEnable)) {
					gameObject.SetActive(false);
				}
			}
			objectToEnable.SetActive(true);
		}

	}
}
