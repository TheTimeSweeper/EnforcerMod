using BepInEx;
using R2API.Utils;

namespace TKNemforcer
{

    [R2APISubmoduleDependency(new string[] 
    {
        "LanguageAPI",
    })]

    [BepInDependency("com.TeamMoonstorm.TKStarstorm2", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency(EnforcerPlugin.EnforcerModPlugin.MODUID, BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin(MODUID, "Add TK Nemesis", "6.9.0")]
    public class EpicIntegrationPlugin : BaseUnityPlugin
    {
        public const string MODUID = "com.EnforcerGang.AddTKNemforcer";

        void Awake()
        {
            //shartstorm 3 xDDDD
            //soft dependancy cause the user doesn't need to be warned that ss2 isn't installed. this can be ignored.
            if (!BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.TeamMoonstorm.TKStarstorm2"))
                return;

            Logger.LogWarning("doin it doin it doin it now");

            NemforcerStarstorm.Init();
        }
    }
}


