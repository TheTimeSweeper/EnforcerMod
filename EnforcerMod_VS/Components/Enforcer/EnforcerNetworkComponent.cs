using RoR2;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Modules;
using System;

public class EnforcerNetworkComponent : NetworkBehaviour {

    [SyncVar]
    public int parries;
    private int skin;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Uhh(skin);
            skin++;
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            skin = 0;
        }
    }

    #region applyskin
    public void Uhh(int skin) {
        if (skin == -1)
            return;

        if (NetworkServer.active) {
            RpcApplySkin(skin);
        } else {
            CmdApplySkin(skin);
        }
    }

    [Command]
    public void CmdApplySkin(int skin) {
        RpcApplySkin(skin);
    }

    [ClientRpc]
    public void RpcApplySkin(int skin) {
        ApplySkin(skin);
    }

    private void ApplySkin(int skin) {

        //fuckin nasty i'm calling this from modelskincontroller > charmodel > getcomponent body > enforcernetworkcomponent > here < just to go modeltransform < getcomponent < back to modelskincontroller fuck man
        GetComponent<CharacterBody>().modelLocator.modelTransform.GetComponent<ModelSkinController>().ApplySkin(skin);

        StartCoroutine(fuckthis(skin));
    }

    //I refuse to let this be the solution
    //I'd rather have the horrible hook hack in the serverachievement
        //apparently I don't refuse cause I'm just letting this happen for several months
    public IEnumerator fuckthis(int skin)
    {
        yield return new WaitForSeconds(1);

        GetComponent<CharacterBody>().modelLocator.modelTransform.GetComponent<ModelSkinController>().ApplySkin(skin);
    }
    #endregion applyskin

    #region bungus
    public void UhhBungus(bool shouldApply) {

        if (NetworkServer.active) {
            RpcApplyBungus(shouldApply);
        } else {
            CmdApplyBungus(shouldApply);
        }
    }

    [Command]
    private void CmdApplyBungus(bool shouldApply) {
        RpcApplyBungus(shouldApply);
    }

    [ClientRpc]
    private void RpcApplyBungus(bool shouldApply) {
        ApplyBungus(shouldApply);
    }

    private void ApplyBungus(bool shouldApply) {
        SkinDef skindef = shouldApply ? Skins.engiBungusSkin : Skins.engiNormalSkin;
        skindef.Apply(GetComponent<CharacterBody>().modelLocator.modelTransform.gameObject);
    }
    #endregion bungus

    #region door
    public void UhhDoor() {

        if (NetworkServer.active) {
            RpcApplyDoor();
        } else {
            CmdApplyDoor();
        }
    }

    [Command]
    private void CmdApplyDoor() {
        RpcApplyDoor();
    }

    [ClientRpc]
    private void RpcApplyDoor() {
        ApplyDoor();
    }

    private void ApplyDoor() {
        Skins.podDoorSkin.Apply(GetComponent<CharacterBody>().modelLocator.modelTransform.gameObject);
    }
    #endregion door
}