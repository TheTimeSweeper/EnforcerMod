using RoR2;
using System;
using UnityEngine;

namespace EnforcerPlugin
{
    public class NemesisUnlockComponent : MonoBehaviour
    {
        private HealthComponent healthComponent;

        public static event Action<Run> OnDeath = delegate { };

        private void Awake()
        {
            this.healthComponent = this.GetComponent<HealthComponent>();

            if (EnforcerPlugin.starstormInstalled)
            {
                ModelSkinController skinController = this.GetComponentInChildren<ModelSkinController>();
                skinController.skins = new SkinDef[]
                {
                NemforcerSkins.ultraSkin
                };

                Invoke("FuckYou", 2f);
            }
        }

        private void FuckYou()
        {
            // i hate this shit
            this.healthComponent.body.skinIndex = 2;
        }

        private void FixedUpdate()
        {
            if (this.healthComponent && !this.healthComponent.alive)
            {
                if (this.healthComponent.body.teamComponent.teamIndex != TeamIndex.Player)
                {
                    OnDeath?.Invoke(Run.instance);
                }
            }
        }
    }
}