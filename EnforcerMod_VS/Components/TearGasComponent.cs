using RoR2;
using System;
using UnityEngine;

namespace EnforcerPlugin {
    public class TearGasComponent : MonoBehaviour
    {
        private int count;
        private int lastCount;
        private uint playID;

        public static event Action<int> CheckImpairedCount = delegate { };

        private void Awake()
        {
            playID = Util.PlaySound(Sounds.GasContinuous, base.gameObject);

            InvokeRepeating("Fuck", 0.25f, 0.25f);
        }

        private void Fuck()
        {
            //this is gross and hacky pls someone do this a different way eventually
                //good luck with that faget

            count = 0;

            foreach(CharacterBody i in GameObject.FindObjectsOfType<CharacterBody>())
            {
                if (i && i.HasBuff(Modules.Buffs.impairedBuff)) count++;
            }

            if (lastCount != count) 
                CheckImpairedCount(count);

            lastCount = count;
        }

        private void OnDestroy()
        {
            AkSoundEngine.StopPlayingID(playID);
        }
    }
}