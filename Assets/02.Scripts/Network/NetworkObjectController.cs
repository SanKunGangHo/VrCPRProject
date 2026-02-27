using Fusion;
using POpusCodec.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkObjectController : NetworkBehaviour
{
    public bool isnotice;


    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_InActive()
    {
        gameObject.SetActive(false);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_Active()
    {
        if(isnotice)
        {
            gameObject.SetActive(true);
            //RPC_ReturnPlayer();

        }
        else
        gameObject.SetActive(true);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_ReturnPlayer()
    {
        // 코루틴을 시작하도록 수정
        Debug.LogWarning("RPC_ReturnPlayer");
        StartCoroutine(ReturnPlayerCoroutine());
    }

    private IEnumerator ReturnPlayerCoroutine()
    {
        yield return new WaitForSeconds(3);

        Debug.LogWarning("RPC_Player2Move(5)");
        FindAnyObjectByType<NetworkGameManagerTest>().RPC_Player2Move(4);
      
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_StopNa()
    {
        SoundManager.Instance.StopNa();
    }
}
