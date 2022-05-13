using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace Modules {
    internal static class Effects {

        internal static void AddEffect(GameObject effectPrefab) {
            AddEffect(effectPrefab, "");
        }

        internal static void AddEffect(GameObject effectPrefab, string soundName) {
            EffectDef newEffectDef = new EffectDef();
            newEffectDef.prefab = effectPrefab;
            newEffectDef.prefabEffectComponent = effectPrefab.GetComponent<EffectComponent>();
            newEffectDef.prefabName = effectPrefab.name;
            newEffectDef.prefabVfxAttributes = effectPrefab.GetComponent<VFXAttributes>();
            newEffectDef.spawnSoundEventName = soundName;

            Modules.Content.AddEffectDef(newEffectDef);
        }
    }
}
