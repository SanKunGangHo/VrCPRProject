using System.Collections;
using Fusion;
using Oculus.Movement.AnimationRigging;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class NetworkController_ : NetworkBehaviour
{
    public RigBuilder rigBuilder;
    public RetargetingLayer retargetingLayer;
    public OVRManager player;

    public override void Spawned()
    {
        SetIOBT(Object);
        //FindObjectOfType<CPRScoreUI>().playerData = GetComponent<PlayerData>();
    }

    private void SetIOBT(NetworkObject networkObject)
    {
        if (networkObject.HasStateAuthority)
        {
            rigBuilder.enabled = true;
            player = FindObjectOfType<OVRManager>();
            StartCoroutine(SetPosition());
        }
    }

    IEnumerator SetPosition()
    {
        retargetingLayer.enabled = true;
        
        while (true)
        {
            
                player.transform.parent.position = this.transform.position;
                this.transform.rotation = player.transform.parent.rotation;

                //ResetRigging();

            yield return null;
        }
    }
}
