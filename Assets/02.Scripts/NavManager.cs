using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavManager : MonoBehaviour
{
    Animator nowPlayer;
   
    public GameObject npc1Obj;


    public GameObject npc4Obj;
    public GameObject prenpc1Obj;
    // Start is called before the first frame update
    void Start()
    {
    
    }
    
    public void NPC1Move()
    {
        nowPlayer = AnimationManager.Instance.npc_anim.npcs[1];
        nowPlayer.GetComponent<NavMeshAgent>().SetDestination(npc1Obj.transform.position);
    }

    public void NPC1Return()
    {
        nowPlayer = AnimationManager.Instance.npc_anim.npcs[1];
        nowPlayer.GetComponent<NavMeshAgent>().SetDestination(prenpc1Obj.transform.position);
    }

    public void NPC4Move()
    {
        nowPlayer = AnimationManager.Instance.npc_anim.npcs[4];
        nowPlayer.GetComponent<NavMeshAgent>().SetDestination(npc4Obj.transform.position);
    }

    public void NPC4Return()
    {
        nowPlayer = AnimationManager.Instance.npc_anim.npcs[4];
        nowPlayer.GetComponent<NavMeshAgent>().SetDestination(prenpc1Obj.transform.position);
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
