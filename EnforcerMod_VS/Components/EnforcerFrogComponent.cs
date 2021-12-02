using RoR2;
using System;
using UnityEngine;

namespace EnforcerPlugin {

    public class EnforcerFrogComponent : MonoBehaviour
    {
        public static event Action<bool> FrogGet = delegate { };
        
        private void Awake()
        {
            InvokeRepeating("Sex", 0.5f, 0.5f);
        }

        private void Sex()
        {
            Collider[] array = Physics.OverlapSphere(transform.position, 16, LayerIndex.defaultLayer.mask);
            for (int i = 0; i < array.Length; i++)
            {
                CharacterBody component = array[i].GetComponent<CharacterBody>();
                if (component)
                {
                    if (component.baseNameToken == "ENFORCER_NAME") FrogGet(true);
                }
            }
        }
    }
}