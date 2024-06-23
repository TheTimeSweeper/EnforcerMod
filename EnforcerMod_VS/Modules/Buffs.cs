using EnforcerPlugin;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Modules {
    internal static class Buffs {
        internal static BuffDef protectAndServeBuff;
        internal static BuffDef energyShieldBuff;
        internal static BuffDef skateboardBuff;
        internal static BuffDef minigunBuff;
        internal static BuffDef nemforcerRegenBuff;

        internal static BuffDef smallSlowBuff;
        internal static BuffDef bigSlowBuff;
        internal static BuffDef impairedBuff;
        internal static BuffDef nemImpairedBuff;

        internal static void RegisterBuffs() {

            protectAndServeBuff = AddNewBuff("Heavyweight", Assets.MainAssetBundle.LoadAsset<Sprite>("texBuffProtectAndServe"), EnforcerModPlugin.characterColor, false, false);
            energyShieldBuff = AddNewBuff("EnergyShield", Assets.MainAssetBundle.LoadAsset<Sprite>("texBuffProtectAndServe"), EnforcerModPlugin.characterColor, false, false);
            skateboardBuff = AddNewBuff("Swag", Assets.LoadBuffSprite("RoR2/Base/SprintOutOfCombat/bdWhipBoost.asset"), EnforcerModPlugin.characterColor, false, false);
            minigunBuff = AddNewBuff("MinigunStance", Assets.MainAssetBundle.LoadAsset<Sprite>("texBuffMinigun"), new Color(1, 0.7176f, 0.1725f), false, false);
            nemforcerRegenBuff = AddNewBuff("NemforcerRegen", Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Croco/bdCrocoRegen.asset").WaitForCompletion().iconSprite, new Color32(210, 50, 22, 255), true, false);

            Sprite slowIcon =  Assets.LoadBuffSprite("RoR2/Base/Common/bdSlow50.asset");
            Sprite cloakIcon = Assets.LoadBuffSprite("RoR2/Base/Common/bdCloak.asset");

            smallSlowBuff = AddNewBuff("NemSmallSelfSlow", slowIcon, new Color(0.647f, 0.168f, 0.184f), false, true);
            bigSlowBuff = AddNewBuff("NemBigSelfSlow", slowIcon, new Color(0.65f, 0.078f, 0.078f), false, true);
            impairedBuff = AddNewBuff("Impaired", cloakIcon, new Color(0.85f * 228f / 255f, 0.85f * 255f / 255f, 0.85f * 79f / 255f), false, true);
            nemImpairedBuff = AddNewBuff("NemImpaired", slowIcon, Color.red, false, true);
        }

        // simple helper method
        internal static BuffDef AddNewBuff(string buffName, Sprite buffIcon, Color buffColor, bool canStack, bool isDebuff) {
            BuffDef buffDef = ScriptableObject.CreateInstance<BuffDef>();
            buffDef.name = buffName;
            buffDef.buffColor = buffColor;
            buffDef.canStack = canStack;
            buffDef.isDebuff = isDebuff;
            buffDef.eliteDef = null;
            buffDef.iconSprite = buffIcon;

            Modules.Content.AddBuffDef(buffDef);

            return buffDef;
        }
    }
}
