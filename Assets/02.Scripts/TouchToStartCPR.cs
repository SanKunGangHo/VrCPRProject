using Fusion;
using System;
using UnityEngine;

public class TouchToStartCPR : NetworkBehaviour
{
    public CPRScoreUI cprScoreUI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hands"))
        {
            SoundManager.Instance.PlaySFX(0);

            if(!cprScoreUI.isMulti)
            {
                cprScoreUI.gameObject.SetActive(true);
                cprScoreUI.StartCPR();
                gameObject.SetActive(false);
            }
            else
            {
                RpcShowCPRUI(true);
            }
           
           
            AnimationManager.Instance.AnimationPlay(2, "Ani_1_1_npc2_01(CPR)");

        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RpcShowCPRUI(bool isActive)
    {
        // 모든 클라이언트에서 CPR UI의 활성화 상태를 동기화
        cprScoreUI.gameObject.SetActive(isActive);
        gameObject.SetActive(false);
    }
    //손이 닿으면, CPR 스타트.
}
