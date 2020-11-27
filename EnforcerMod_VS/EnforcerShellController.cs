using UnityEngine;
using RoR2;

namespace Enforcer
{
    class EnforcerShellController : MonoBehaviour
    {
        private Rigidbody rb;
        private bool triggered;

        private void Awake()
        {
            this.rb = this.GetComponentInChildren<Rigidbody>();
            this.gameObject.layer = LayerIndex.debris.intVal;
            this.GetComponentInChildren<Collider>().gameObject.layer = LayerIndex.debris.intVal;
            this.transform.rotation = Quaternion.Euler(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
        }
        
        private void OnEnable()
        {
            this.triggered = false;
        }

        private void OnCollisionEnter()
        {
            if (!this.triggered)
            {
                this.triggered = true;
                //if (Random.value > 0.5f) Util.PlaySound(EnforcerPlugin.Sounds.ShellSlow, this.gameObject);
                //else Util.PlaySound(EnforcerPlugin.Sounds.ShellFast, this.gameObject);
            }
        }
    }
}
