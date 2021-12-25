using RoR2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnforcerPlugin {

    public class MenuSoundComponent : MonoBehaviour
    {
        private uint playIDSiren = 0;
        private float takeOutGunDelay;
        private uint playIDGun = 0;

        private void OnEnable()
        {                              //Sounds.CharSelect
            this.playIDSiren = Util.PlaySound(Sounds.SirenDeflect, base.gameObject);
                    //presto! frame / framerate * animation speed in animator state 
            takeOutGunDelay = 105f / 24f / 1.1f;

            StartCoroutine(delayedGunSound());

            EnforcerLightController lightController = GetComponentInChildren<EnforcerLightController>();
            if (lightController)
            {
                lightController.FlashLights(3);
            }

            EnforcerLightControllerAlt lightControllerAlt = GetComponentInChildren<EnforcerLightControllerAlt>();
            if (lightControllerAlt)
            {
                lightControllerAlt.ToggleSiren();
            }
        }

        private IEnumerator delayedGunSound()
        {
            yield return new WaitForSeconds(takeOutGunDelay);

            this.playIDGun = Util.PlaySound(Sounds.ShieldDown, base.gameObject);
        }

        private void OnDestroy()
        {
            if (this.playIDSiren != 0) AkSoundEngine.StopPlayingID(this.playIDSiren);
            if (this.playIDGun != 0) AkSoundEngine.StopPlayingID(this.playIDGun);
        }
    }
}