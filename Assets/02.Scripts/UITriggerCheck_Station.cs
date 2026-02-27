using Fusion;
using UnityEngine;

public class UITriggerCheck_Station : NetworkBehaviour
{
    public NetworkGameManagerTest_Station networkGameManagerT;
    public UITriggerCheck_Station otherChecker;
    
    //어느쪽이 먼저 눌리든 옆에 애도 같이 지워지도록
    public void OnTriggerEnter(Collider other)
    {
        RPC_TriggerEnter();
    }
    

    [Rpc( RpcSources.All, RpcTargets.All)]
    public void RPC_TriggerEnter()
    {
        networkGameManagerT.CP_PatientTouchUI = true;
        transform.parent.gameObject.SetActive(false);
    }
}
