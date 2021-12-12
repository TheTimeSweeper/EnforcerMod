using UnityEngine;

namespace EnforcerPlugin {

    public class MenuSoundComponent : MonoBehaviour
    {
        private uint playID;

        private void OnEnable()
        {
            //this.playID = Util.PlaySound(Sounds.CharSelect, base.gameObject);

            var i = GetComponentInChildren<EnforcerLightController>();
            if (i)
            {
                i.FlashLights(3);
            }

            var j = GetComponentInChildren<EnforcerLightControllerAlt>();
            if (j)
            {
                j.ToggleSiren();
            }
        }

        private void OnDestroy()
        {
            if (this.playID != 0) AkSoundEngine.StopPlayingID(this.playID);
        }
    }
}