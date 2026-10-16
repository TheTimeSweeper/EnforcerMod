using HG;
using Modules;
using RoR2;
using System;
using UnityEngine;

namespace EnforcerPlugin {
    public class TearGasComponent : MonoBehaviour
    {
        private uint playID;

        public static event Action<int> CheckImpairedCount = delegate { };

        private void Awake()
        {
            playID = Util.PlaySound(Sounds.GasContinuous, base.gameObject);
        }

        private void OnDestroy()
        {
            AkSoundEngine.StopPlayingID(playID);
        }
    }
}