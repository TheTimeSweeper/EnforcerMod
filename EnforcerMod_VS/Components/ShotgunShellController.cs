using UnityEngine;
using RoR2;
using Modules;

namespace Enforcer {
    class ShotgunShellController : MonoBehaviour
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
            if (Config.shellSounds.Value)
            {
                if (!this.triggered)
                {
                    this.triggered = true;
                    AkSoundEngine.SetRTPCValue("Shell_Velocity", rb.velocity.magnitude);

                    if (Random.value > 0.5f) Util.PlaySound(Modules.Sounds.ShellHittingFloorSlow, this.gameObject);
                    else Util.PlaySound(Modules.Sounds.ShellHittingFloorFast, this.gameObject);
                }
            }
        }
    }
}