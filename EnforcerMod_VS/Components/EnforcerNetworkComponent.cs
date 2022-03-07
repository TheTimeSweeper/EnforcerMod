using RoR2;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class EnforcerNetworkComponent : NetworkBehaviour {

    [SyncVar]
    public int parries;

    [ClientRpc]
    public void RpcUhh(int skin) {
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
