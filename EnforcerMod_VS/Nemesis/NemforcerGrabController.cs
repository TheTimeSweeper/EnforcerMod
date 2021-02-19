using RoR2;
using UnityEngine;

namespace Enforcer.Nemesis
{
    public class NemforcerGrabController : MonoBehaviour
    {
        public Transform pivotTransform;

        private CharacterBody body;
        private CharacterMotor motor;

        private void Awake()
        {
            this.body = this.GetComponent<CharacterBody>();
            this.motor = this.GetComponent<CharacterMotor>();

            if (this.motor)
            {
                this.gameObject.layer = LayerIndex.fakeActor.intVal;
                this.motor.Motor.RebuildCollidableLayers();
            }
        }

        private void FixedUpdate()
        {
            if (this.motor)
            {
                this.motor.disableAirControlUntilCollision = true;
                this.motor.velocity = Vector3.zero;
                this.motor.rootMotion = Vector3.zero;
            }

            if (this.pivotTransform)
            {
                this.transform.position = this.pivotTransform.position;
            }
        }

        public void Release()
        {
            if (this.motor)
            {
                this.gameObject.layer = LayerIndex.defaultLayer.intVal;
                this.motor.Motor.RebuildCollidableLayers();
            }

            Destroy(this);
        }
    }
}