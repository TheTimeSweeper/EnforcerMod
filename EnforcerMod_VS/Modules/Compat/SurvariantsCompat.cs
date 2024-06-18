using Modules;
using RoR2;
using Survariants;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace Enforcer.Modules.Compat
{
    public class SurvariantsCompat
    {
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void SetHeavyVariant()
        {
            if (!Config.survariantsCompat.Value) return;

            SurvivorDef enforcer = SurvivorCatalog.GetSurvivorDef(SurvivorCatalog.GetSurvivorIndexFromBodyIndex(BodyCatalog.FindBodyIndex("EnforcerBody")));
            SurvivorDef heavyTF2 = SurvivorCatalog.GetSurvivorDef(SurvivorCatalog.GetSurvivorIndexFromBodyIndex(BodyCatalog.FindBodyIndex("NemesisEnforcerBody")));

            if (enforcer && heavyTF2)
            {
                SurvivorVariantDef variant = ScriptableObject.CreateInstance<SurvivorVariantDef>();
                (variant as ScriptableObject).name = heavyTF2.cachedName;
                variant.DisplayName = heavyTF2.displayNameToken;
                variant.VariantSurvivor = heavyTF2;
                variant.TargetSurvivor = enforcer;
                variant.RequiredUnlock = heavyTF2.unlockableDef;
                variant.Description = "NEMFORCER_SUBTITLE";

                heavyTF2.hidden = true;
                SurvivorVariantCatalog.AddSurvivorVariant(variant);
            }
        }
    }
}
