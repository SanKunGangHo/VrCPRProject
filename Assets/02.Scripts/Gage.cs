using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Gage : MonoBehaviour
{
    private float gazeTime = 10f;
    private Image img;

    public bool isMulti = false;

    public GameObject playerHeadObj;
    public GameObject playerLeftHandObj;
    public GameObject playerRightHandObj;

    public GameObject EyeContactObj;

    public GameObject NPC;
    
    private bool isrightCheck = false;
    private bool isLeftCheck = false;
    public bool isHeadCheck = false;
    
    public NetworkGameManagerTest ngManager;
    public NetworkGameManagerTest_Station ngManager_Station;

    public float multipleTime = 1.5f;

    public bool iWantMoreHard;

    // Start is called before the first frame update
    void Start()
    {
        img = gameObject.GetComponent<Image>();
     
        MutiInit();
    }

    private void MutiInit()
    {
        NetworkRunner runner = NetworkManager1.RunnerInstance;

        if (runner != null)
        {
        
            if (SceneManager.GetActiveScene().name.Contains("StationMultiScene"))
            {
                // if (!runner.GetPlayerObject(runner.LocalPlayer).GetComponent<PlayerData>().isCPR)
                // {
                //     playerHeadObj = runner.GetPlayerObject(runner.LocalPlayer).transform.Find("Playr1_Head")
                //         .gameObject;
                //     playerLeftHandObj = runner.GetPlayerObject(runner.LocalPlayer).transform.Find("Playr1_Left")
                //         .gameObject;
                //     playerRightHandObj = runner.GetPlayerObject(runner.LocalPlayer).transform.Find("Playr1_RIght")
                //         .gameObject;
                // }

                //NPC = ngManager_Station.player1.gameObject;
        
                return;
            }
        }

        if(runner != null)
        {
            if (runner.GetPlayerObject(runner.LocalPlayer).GetComponent<PlayerData>().isCPR)
            {
                playerHeadObj = runner.GetPlayerObject(runner.LocalPlayer).transform.Find("Player2_Head").gameObject;
                playerLeftHandObj = runner.GetPlayerObject(runner.LocalPlayer).transform.Find("Player2_Left").gameObject;
                playerRightHandObj = runner.GetPlayerObject(runner.LocalPlayer).transform.Find("Player2_Right").gameObject;
            }
            NPC = GameObject.Find("Player_Man_Connected_Network(Clone)");
            Debug.LogWarning("npc : " + NPC);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        NPCCheckInSight();
    }

    private void OnTriggerStay(Collider other)
    {
        
    }

    void NPCCheckInSight()
    {
       Debug.Log("Check");
        //Head 방향에서 나오는 Ray Check 
        RaycastHit headhit;
        //HeadRay
        Ray ray = new Ray(playerHeadObj.transform.transform.position, playerHeadObj.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 500, Color.blue);

        if (Physics.Raycast(ray, out headhit))
        {
            //Player2인경우 인덱스로 찾아야함 13번
            if (headhit.collider != null && (headhit.collider.gameObject == NPC || headhit.collider.gameObject.layer == 13))
                isHeadCheck = true;
            else
                isHeadCheck = false;
        }

        // LeftObj에서 Check중
        if (playerLeftHandObj != null)
        {
            Vector3 leftHandforward = playerLeftHandObj.transform.forward;

            Vector3 npcforward = NPC.transform.forward;

            float dotProduct = Vector3.Dot(leftHandforward, npcforward);

            if (dotProduct >= -1f && dotProduct <= -0.9f)
            {
                
                isLeftCheck = true;
            }
            else
            {
                isLeftCheck = false;
            }
        }

        //Right Obj에서 Check중
        if (playerRightHandObj != null)
        {
            Vector3 rightHandforward = playerRightHandObj.transform.forward;

            Vector3 npc2forward = NPC.transform.forward;

            float dotProduct2 = Vector3.Dot(rightHandforward, npc2forward);

            if (dotProduct2 <= 1f && dotProduct2 >= 0.9f)
            {
                isrightCheck = true;
            }
            else
            {
                isrightCheck = false;
            }
        }
        
        if (isHeadCheck && (isLeftCheck || isrightCheck))
        {
            // if (iWantMoreHard)
            // {
                if (img.fillAmount >= 1f)
                {

                    this.transform.parent.gameObject.SetActive(false);
                    CheckMulti();
                    if (SceneManager.GetActiveScene().name.Contains("Multi")) return;
                    UIManager.Instance.PlayerUICanvas.SetActive(false);
                    GameManager.Instance.NextSituation();
                }
                else
                {
                    img.fillAmount += (Time.deltaTime * multipleTime) / gazeTime;
                }
            //}
            // else
            // {
            //     StartCoroutine(GazeCoroutine());
            // }
        }
    }

    IEnumerator GazeCoroutine()
    {
        while (img.fillAmount < 1)
        {
            img.fillAmount += (Time.deltaTime * multipleTime) / gazeTime;
            yield return null;
        }
        
        this.transform.parent.gameObject.SetActive(false);
        CheckMulti();
        if (SceneManager.GetActiveScene().name.Contains("Multi")) yield break;
        UIManager.Instance.PlayerUICanvas.SetActive(false);
        GameManager.Instance.NextSituation();
    }

    void CheckMulti()
    {
        if (isMulti)
        {
            if (ngManager != null)
            {
                //ngManager situation
            }

            if (ngManager_Station != null)
            {
                ngManager_Station.CP_NPC4Gage = true;
            }
        }
    }

}
