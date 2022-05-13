using UnityEngine;

namespace RoR2
{
	public class RagdollController : MonoBehaviour
	{
		public Transform[] bones;
		public MonoBehaviour[] componentsToDisableOnRagdoll;

        private Animator animator;

        [ContextMenu("go")]
        public void BeginRagdoll()
        {
            Vector3 force = Vector3.up * 1;
            this.animator = base.GetComponent<Animator>();
            if (this.animator)
            {
                this.animator.enabled = false;
            }
            foreach (Transform transform in this.bones)
            {
                //if (transform.gameObject.layer == LayerIndex.ragdoll.intVal)
                //{
                    transform.parent = base.transform;
                    Rigidbody component = transform.GetComponent<Rigidbody>();
                    transform.GetComponent<Collider>().enabled = true;
                    component.isKinematic = false;
                    component.interpolation = RigidbodyInterpolation.Interpolate;
                    component.collisionDetectionMode = CollisionDetectionMode.Continuous;
                    component.AddForce(force * UnityEngine.Random.Range(0.9f, 1.2f), ForceMode.VelocityChange);
                //}
            }
            MonoBehaviour[] array2 = this.componentsToDisableOnRagdoll;
            for (int i = 0; i < array2.Length; i++)
            {
                array2[i].enabled = false;
            }
        }
    }
}
