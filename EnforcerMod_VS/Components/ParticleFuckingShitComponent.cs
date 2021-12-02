using RoR2;
using UnityEngine;

namespace EnforcerPlugin {
    public class ParticleFuckingShitComponent : MonoBehaviour
    {
        private void Start()
        {
            this.transform.parent = null;
            this.gameObject.AddComponent<DestroyOnTimer>().duration = 8;
        }
    }
}