using Fusion;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Gage_BreathCheck : NetworkBehaviour
{
    private float gazeTime = 10f;
    private Image img;
    
    public GameObject playerHeadObj;
    
    float elapsed = 0f;
    
    public GameObject NPC;
    
    [SerializeField] private bool isMulti = false;
    
    public NetworkGameManagerTest ngManager;
    public NetworkGameManagerTest_Station ngManager_Station;
    
    private void Start()
    {
        img = GetComponent<Image>();
        
        if (isMulti)
        {
            NetworkRunner runner = NetworkManager1.RunnerInstance;
            // if (runner.GetPlayerObject(runner.LocalPlayer).GetComponent<PlayerData>().isCPR)
            // {
            //     playerHeadObj = runner.GetPlayerObject(runner.LocalPlayer).transform.Find("Player2_Head").gameObject;
            // }
        }
    }
    
    public void RPC_BeginBreathCheck()
    {
        StartCoroutine(MultiBreathCheckCoroutine());
    }

    IEnumerator MultiBreathCheckCoroutine()
    {
        while (elapsed < gazeTime)
        {
            //ngManager_Station.Rpc_Log(1, elapsed.ToString());
            elapsed ++;
            if (img != null)
            {
                img.fillAmount += elapsed / gazeTime;
            }
            yield return new WaitForSeconds(1f);
        }

        if (ngManager_Station != null)
        {
            ngManager_Station.CP_PatientBreathCheck = true;
            transform.parent.parent.gameObject.SetActive(false);
        }

        if (ngManager != null)
        {
            ngManager.CP_NPC2BreathCheck = true;
        }
    }

    private void Update()
    {
        if (isMulti) return;
        NPCBreathCheckInSight();
    }

    void NPCBreathCheckInSight()
    {
        
        Ray headRay = new Ray(playerHeadObj.transform.position, playerHeadObj.transform.forward);
        Debug.DrawRay(headRay.origin, headRay.direction * 500, Color.blue);
        
        if (Physics.Raycast(headRay, out RaycastHit headHit))
        {
            Debug.Log("Check1");
            if(headHit.collider != null)
            {
                Debug.Log("Check2");
                StartCoroutine(SingleBreathCheckCoroutine());
            }
            else
            {
                //StopCoroutine(SingleBreathCheckCoroutine());
            }
        }
    }

    IEnumerator SingleBreathCheckCoroutine()
    {
        if (img.fillAmount >= 1)
        {
            if (!UIManager.Instance.isBreathing)
            {
                UIManager.Instance.isBreathing = true;
            }
            img.fillAmount = 0;
            transform.parent.parent.gameObject.SetActive(false);
            yield break;
        }

        elapsed += Time.deltaTime;
        if (img != null)
        {
            img.fillAmount += Time.deltaTime / gazeTime;
        }
        yield return null;
        //GameManager.Instance.NextSituation();
    }
}
