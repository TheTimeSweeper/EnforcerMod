using RoR2;
using UnityEngine;

namespace Enforcer.Nemesis
{
    public class NemforcerGrabController : MonoBehaviour
    {
        public Transform pivotTransform;

        private CharacterBody body;
        private CharacterMotor motor;
        private CharacterDirection direction;
        private ModelLocator modelLocator;
        private Transform modelTransform;
        private Quaternion originalRotation;

        private void Awake()
        {
            this.body = this.GetComponent<CharacterBody>();
            this.motor = this.GetComponent<CharacterMotor>();
            this.direction = this.GetComponent<CharacterDirection>();
            this.modelLocator = this.GetComponent<ModelLocator>();

            if (this.direction) this.direction.enabled = false;

            if (this.modelLocator)
            {
                if (this.modelLocator.modelTransform)
                {
                    this.modelTransform = modelLocator.modelTransform;
                    this.originalRotation = this.modelTransform.rotation;

                    this.modelLocator.enabled = false;
                }
            }
        }

        private void FixedUpdate()
        {
            if (this.motor)
            {
                this.motor.disableAirControlUntilCollision = true;
                this.motor.velocity = Vector3.zero;
                this.motor.rootMotion = Vector3.zero;

                this.motor.Motor.SetPosition(this.pivotTransform.position, true);
            }

            if (this.pivotTransform)
            {
                this.transform.position = this.pivotTransform.position;
            }
            else
            {
                this.Release();
            }

            if (this.modelTransform)
            {
                this.modelTransform.position = this.pivotTransform.position;
                this.modelTransform.rotation = this.pivotTransform.rotation;
            }
        }

        public void Release()
        {
            if (this.modelLocator) this.modelLocator.enabled = true;
            if (this.modelTransform) this.modelTransform.rotation = this.originalRotation;
            if (this.direction) this.direction.enabled = true;

            Destroy(this);
        }
    }
}