using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MultiCheck : NetworkBehaviour
{
    //[Networked] public bool IsToggled { get; set; }
    // Start is called before the first frame update
    public Toggle tgl_1;
    public Toggle tgl_2;

    public Toggle tgl_a1;
    public Toggle tgl_a2;  




    public override void Spawned()
    {
        Debug.Log("Spawned호출");
        tgl_1.onValueChanged.AddListener(OnPlayer1ToggleChanged);
        tgl_2.onValueChanged.AddListener(OnPlayer2ToggleChanged);

        //if (Object.HasStateAuthority)
        //{
         
        //    tgl_1.onValueChanged.AddListener(OnPlayer1ToggleChanged);
        //    tgl_2.onValueChanged.AddListener(OnPlayer2ToggleChanged);
        //}
        //else
        //{
        //    Debug.Log("Spawned함수 Object Out");
        //}
    }

    void OnPlayer1ToggleChanged(bool isOn)
    {
        //if (Object.HasStateAuthority)
        //{
            Debug.Log("OnPlayer1ToggleChanged");
            RPC_UpdateToggleState(isOn);
        //}
        //else
        //{
        //    Debug.Log("OnPlayer1ToggleChanged n");
        //}
    }
    void OnPlayer2ToggleChanged(bool isOn)
    {
        //if (Object.HasStateAuthority)
        //{
            Debug.Log("OnPlayer2ToggleChanged");
            RPC_UpdateToggleState2(isOn);
        //}
        //else
        //{
        //    Debug.Log("OnPlayer2ToggleChanged n");
        //}
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    void RPC_UpdateToggleState(bool isOn)
    {
        tgl_a1.isOn = isOn;
       
    }
    [Rpc(RpcSources.All, RpcTargets.All)]
    void RPC_UpdateToggleState2(bool isOn)
    {
        tgl_a2.isOn = isOn;
        Debug.Log("RPC_UpdateToggleState2");
    }


 
    public override void FixedUpdateNetwork()
    {
        //if (tgl_a1.isOn != IsToggled)
        //{
        //    tgl_a1.isOn = IsToggled;

        //}
        Debug.Log("FixedUpdateNetwork");

        if (tgl_1.isOn)
        {
            Debug.Log("tgl1 is true");
            tgl_a1.isOn = true;
        }

        if (tgl_2.isOn)
        {
            Debug.Log("tgl2 is true");
            tgl_a2.isOn = true;
        }
    }

}
