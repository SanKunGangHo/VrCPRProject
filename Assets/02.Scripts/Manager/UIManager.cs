using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    /// <summary>
    /// /User Interface UI////
    /// </summary>
    [Header("Player Settings")]
    public GameObject PlayerUICanvas;
    public GameObject PlayerLodingUICanvas;
    public GameObject EndingCanvas;
    public GameObject MoveUI;
    
    [Header("Player Multi Settings")]
    public GameObject Player2MoveUI;
    public bool isMulti = false;
    public GameObject Player2_guidePopup_02UI;
    public GameObject Player1_MoveUI;

    public List<Sprite> titleSprites;      

    public SpriteRenderer titleSR;

    public List<GameObject> patientUI;
    public List<GameObject> NPCUI;
    public GameObject TottaCanvas;

    public bool isBreathing = false;

    private bool isCompleteSituation = false;

    public int handTouchCount;
    public int HandTouchCount
    {
        get { return handTouchCount; }
        set
        {
             handTouchCount = value;
            // HandTouchCount가 2가 되면 UI 활성화
            if (handTouchCount == 2)
            {
                if(isBreathing) return;
                //멀티일때
                if (isMulti)
                {
                    if (SceneManager.GetActiveScene().name.Contains("Subway"))
                    {
                        FindObjectOfType<NetworkGameManagerTest>().RPC_TouchPaitent();
                    }

                    if (SceneManager.GetActiveScene().name.Contains("Station"))
                    {
                        NetworkGameManagerTest_Station ngManager_st = FindObjectOfType<NetworkGameManagerTest_Station>();
                        ngManager_st.Rpc_patientTouch();
                    }
                }
                //싱글일때
                else
                {
                    //양손 터치 UI 끄기
                    InActivePatientUI(0);
                    //머리, 가슴 UI 생성

                    Instance.HideUIAfterTimeCoroutine(0, 2, 8, () =>
                    {
                        AnimationManager.Instance.totta.SetActive(true);

                        ActivePatientUI(1);
                        //SoundManager.Instance.PlayNa(3);

                        SoundManager.Instance.PlayAudioCoroutine(1, 3, () =>
                        {
                            AnimationManager.Instance.totta.SetActive(false);
                        });
                        
                        StartCoroutine(Gage_BreathChecking());

                        // //icon_touch02 I/F 등장 + textbox_1-1_01 팝업 -> 어깨 두들기기
                        // HideUIAfterTimeCoroutine(0, 3, 10, () =>
                        // {
                        //     InActivePatientUI(1);
                        //     GameManager.Instance.NextSituation();
                        // });
                    });
                }
            }
        }
    }

    IEnumerator Gage_BreathChecking()
    {
        ActiveUI(0, 3);
        yield return new WaitForSeconds(5f);
        PlayerUICanvas.SetActive(false);
        
        while (!isBreathing)
        {
            yield return null;
        }
        
        GameManager.Instance.NextSituation();
    }
    
    void Start()
    {
        HandTouchCount = 0;
    }

    public void HideUIAfterTimeCoroutine(int _Listidx, int _changeidx, float _delay)
    {
        StartCoroutine(HideUIAfterTime(_Listidx, _changeidx, _delay));
    }
    public void HideUIAfterTimeCoroutine(int _Listidx, int _changeidx, float _delay, Action onComplete = null)
    {
        StartCoroutine(HideUIAfterTime(_Listidx, _changeidx, _delay, onComplete));
    }
    
    IEnumerator HideUIAfterTime(int _Listidx, int _changeidx, float _delay, Action onComplete)
    {
        // 기존 로직 유지
        if (!PlayerUICanvas.activeSelf)
        {
            switch (_Listidx)
            {
                case 0:
                    PlayerUICanvas.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = titleSprites[_changeidx];
                    break;
                case 1:
                    PlayerUICanvas.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = titleSprites[_changeidx];
                    break;
            }
            PlayerUICanvas.SetActive(true);
        }

        yield return new WaitForSeconds(_delay);

        PlayerUICanvas.SetActive(false);

        // 코루틴 완료 후 콜백 호출
        onComplete?.Invoke();
    }

    IEnumerator HideUIAfterTime(int _Listidx, int _changeidx, float _delay)
    {
        // 기존 로직 유지
        if (!PlayerUICanvas.activeSelf)
        {
            switch (_Listidx)
            {
                case 0:
                    PlayerUICanvas.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = titleSprites[_changeidx];
                    break;
                case 1:
                   
                    PlayerUICanvas.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = titleSprites[_changeidx];
                    break;
            }

            PlayerUICanvas.SetActive(true);
        }

        yield return new WaitForSeconds(_delay);

        PlayerUICanvas.SetActive(false);
    }
    
    public void CompleteSituationQuest()
    {
        isCompleteSituation = true;
    }
    
    public void ActivePatientUI(int _idx)
    {
        patientUI[_idx].SetActive(true);
    }

    public void InActivePatientUI(int _idx)
    {
        patientUI[_idx].SetActive(false);
    }

    public void ActiveUI(int _Listidx, int _changeidx)
    {
       if (_Listidx == 0)
       {
           PlayerUICanvas.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = titleSprites[_changeidx];
       }
       else if (_Listidx == 1)
       {
            PlayerUICanvas.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = titleSprites[_changeidx];
       }
       PlayerUICanvas.SetActive(true);
    }

    public void AED1off()
    {
        UIManager.Instance.patientUI[3].GetComponent<NetworkObjectController>().RPC_InActive();
        UIManager.Instance.patientUI[13].GetComponent<NetworkObjectController>().RPC_InActive();
        UIManager.Instance.patientUI[14].GetComponent<NetworkObjectController>().RPC_InActive();
        UIManager.Instance.patientUI[15].GetComponent<NetworkObjectController>().RPC_InActive();
        UIManager.Instance.patientUI[16].GetComponent<NetworkObjectController>().RPC_InActive();
        UIManager.Instance.patientUI[17].GetComponent<NetworkObjectController>().RPC_InActive();
        UIManager.Instance.patientUI[18].GetComponent<NetworkObjectController>().RPC_InActive();

    }
    
    public void AED2On()
    {
        //aed깔아놓기                    
        UIManager.Instance.patientUI[10].GetComponent<NetworkObjectController>().RPC_Active();
        UIManager.Instance.patientUI[11].GetComponent<NetworkObjectController>().RPC_Active();
        UIManager.Instance.patientUI[12].GetComponent<NetworkObjectController>().RPC_Active();
        //CPR 생성
        
        UIManager.Instance.patientUI[19].GetComponent<NetworkObjectController>().RPC_Active();
        UIManager.Instance.patientUI[20].GetComponent<NetworkObjectController>().RPC_Active();

    }

}
