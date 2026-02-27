using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavManager_Station : NetworkBehaviour
{
    public NetworkGameManagerTest_Station stationManager;
    
    Animator nowPlayer;
    
    public Vector3 npc4Position;

    public List<Transform> npc4Transforms;
    
    // public void NPC1Move()
    // {
    //     nowPlayer = AnimationManager.Instance.npc_anim.npcs[1];
    //     nowPlayer.GetComponent<NavMeshAgent>().SetDestination(npc1Obj.transform.position);
    // }
    //
    // public void NPC1Return()
    // {
    //     nowPlayer = AnimationManager.Instance.npc_anim.npcs[1];
    //     nowPlayer.GetComponent<NavMeshAgent>().SetDestination(prenpc1Obj.transform.position);
    // }

    private void Start()
    {
        //NPC4Move();
    }
    
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_NPC4Move()
    {
        Debug.Log("RPC_NPC4Move");
        // nowPlayer = AnimationManager.Instance.npc_anim.npcs[4];
        // nowPlayer.GetComponent<NavMeshAgent>().SetDestination(npc4Obj.transform.position);
        StartCoroutine(NPC4MoveCoroutine());
    }

    IEnumerator NPC4MoveCoroutine()
    {
        Debug.Log("RPC_NPC4Move");
        nowPlayer = AnimationManager.Instance.npc_anim.npcs[4];
        
        npc4Position = nowPlayer.transform.position;
        
        nowPlayer.SetBool("Ani_1_1_npc4_03(Running)", true);
        Debug.Log("Ani_1_1_npc4_03(Running)");
        
        nowPlayer.GetComponent<NavMeshAgent>().SetDestination(npc4Transforms[0].position);

        while (Vector3.Distance(nowPlayer.transform.position, npc4Transforms[0].position) > 0.2f)
        {
            yield return null;
        }
        
        nowPlayer.GetComponent<NavMeshAgent>().SetDestination(npc4Transforms[1].position);

        while (Vector3.Distance(nowPlayer.transform.position, npc4Transforms[1].position) > 0.2f)
        {
            yield return null;
        }
        
        nowPlayer.SetBool("Ani_1_1_npc4_03(Running)", false);
        
        yield return new WaitForSeconds(3);
        
        NPC4Return();
    }
    
    //TODO:  RPC 필요?
    public void NPC4Return()
    {
        StartCoroutine(NPC4ReturnCoroutine());
    }

    IEnumerator NPC4ReturnCoroutine()
    {
        nowPlayer = AnimationManager.Instance.npc_anim.npcs[4];
        nowPlayer.SetBool("Ani_1_1_npc4_03(Running)", true);
        
        nowPlayer.GetComponent<NavMeshAgent>().SetDestination(npc4Transforms[0].position);

        while (Vector3.Distance(nowPlayer.transform.position, npc4Transforms[0].position) > 0.2f)
        {
            yield return null;
        }
        
        nowPlayer.GetComponent<NavMeshAgent>().SetDestination(npc4Position);

        while (Vector3.Distance(nowPlayer.transform.position, npc4Position) > 0.2f)
        {
            yield return null;
        }
        
        nowPlayer.SetBool("Ani_1_1_npc4_03(Running)", false);
        
        yield return new WaitForSeconds(3);

        stationManager.CP_NPC4Departure = true;
    }


    public void NavOff()
    {
        nowPlayer.GetComponent<NavMeshAgent>().isStopped = true;    
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
