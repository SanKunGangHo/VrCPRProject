using Fusion;
using UnityEngine;

public class PlayerData : NetworkBehaviour
{

    public bool isCPR;
    
    public GameObject smartphone;

    [Networked]
    public int NetworkedScore { get; set; }


    //[Networked]
    //public int NetworkedScore2 { get; set; }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_UpdateScore(int newScore)
    {
        NetworkedScore = newScore;
        newScore = NetworkedScore;
        Debug.Log($"Updated score to {NetworkedScore} across network");
    }
    
    // 점수 변경 호출
    public void ChangeScore(int newScore)
    {
        if (HasInputAuthority)
        {
            RPC_UpdateScore(newScore);
        }
        else
        {
            Debug.LogWarning("Local simulation is not allowed to send this RPC");
        }
    }
}
