using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartphoneSpawner : NetworkBehaviour
{
    public GameObject smartphone;
    
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void Rpc_SpawnSmartphone()
    {
        smartphone.SetActive(true);
    }
}
