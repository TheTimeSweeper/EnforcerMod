using RoR2;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Modules;

public class EnforcerNetworkComponent : NetworkBehaviour {

    [SyncVar]
    public int parries;

    public void Uhh(int skin) {

        EnforcerPlugin.EnforcerModPlugin.StaticLogger.LogWarning("RpcUhh");

        //ApplySkin(skin);

        if (NetworkServer.active) {
            RpcApplySkin(skin);
        } else {
            CmdApplySkin(skin);
        }
    }

    [Command]
    public void CmdApplySkin(int skin) {
        EnforcerPlugin.EnforcerModPlugin.StaticLogger.LogWarning("CmdApplySkin");
        RpcApplySkin(skin);
    }

    [ClientRpc]
    public void RpcApplySkin(int skin) {
        EnforcerPlugin.EnforcerModPlugin.StaticLogger.LogWarning("RpcApplySkin");
        ApplySkin(skin);
    }

    private void ApplySkin(int skin) {

        EnforcerPlugin.EnforcerModPlugin.StaticLogger.LogWarning("ApplySkin");

        //fuckin nasty i'm calling this from modelskincontroller > charmodel > getcomponent body > enforcernetworkcomponent > here < just to go modeltransform < getcomponent < back to modelskincontroller fuck man
        GetComponent<CharacterBody>().modelLocator.modelTransform.GetComponent<ModelSkinController>().ApplySkin(skin);

        StartCoroutine(fuckthis(skin));
    }

    //I refuse to let this be the solution
    //I'd rather have the horrible hook hack in the serverachievement
    public IEnumerator fuckthis(int skin)
    {

        yield return new WaitForSeconds(1);

        GetComponent<CharacterBody>().modelLocator.modelTransform.GetComponent<ModelSkinController>().ApplySkin(skin);
    }
}