using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public enum SI
{
    SI1_1,
    SI2_1,
    SI1_2,
    SI2_2,
}

public class GameManager : Singleton<GameManager>
{
    //int 정리 상황//
    //상황 1-1 -> 0
    //상황 1-2 -> 1
    //상황 2-1 -> 2
    //상황 2-2 -> 3

    private Queue<System.Action> eventQueue = new Queue<System.Action>();
    //private Queue<System.Action> eventQueue_1_1 = new Queue<System.Action>();
    
    private bool isComplete2_1 = false;
   
    public NetworkRunner runner;
  
    public SI si;
    [Header("Totta Settings")]
    public GameObject TestTT;
    public GameObject totta;
    public GameObject tottaOffset;

    [Header("Player Settings")]

    public GameObject PlayerTT;
    public List<GameObject> PlayeObjs;
    public GameObject PlayerGageObj;
    public List<GameObject> SpawnPoints;
    public GameObject playerPrePos;

    private bool player1On = false;
    private bool player2On = false;

    public NavManager navManager;

    public GameObject banpo;
    public GameObject subway;

    public GameObject ArrowUI;

    public GameObject Gallery;
    public GameObject NPC4Pos;
    public GameObject NPC1Pos;

    public GameObject Information;
    
    public GameObject skipButton;

   
    /// <summary>

    /// </summary>

    //Scene 전환하기 위한 Button Check 변수
    //[SerializeField]
    // [Networked] private int nextSceneCount { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Scene " + SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene().name.Contains("Start"))
        {
            Destroy(gameObject);
            return;
        }
        InitializeNetwork();
        SetupEventQueue();
        NextSituation();
    }

    private void SetupEventQueue()
    {

        if (si == SI.SI1_1)
        {
            //1-1 상황 저장
            eventQueue.Enqueue(StartSituation);
            eventQueue.Enqueue(Situation21_1_1);
            eventQueue.Enqueue(Situation21_1_2);
            eventQueue.Enqueue(Situation21_1_3);
            eventQueue.Enqueue(Situation11_1_4);
            eventQueue.Enqueue(Situation11_1_4_1);
            eventQueue.Enqueue(Situation11_1_4_2);
            eventQueue.Enqueue(Situation11_1_5);
            eventQueue.Enqueue(Situation11_1_6);
            eventQueue.Enqueue(Situation11_1_6_01);
            eventQueue.Enqueue(Situation11_1_6_1);
            eventQueue.Enqueue(Situation11_1_7);
            eventQueue.Enqueue(Situation11_1_8);
            eventQueue.Enqueue(Situation11_1_8_1);
            eventQueue.Enqueue(Situation11_1_9);
            eventQueue.Enqueue(Situation11_1_10);
            eventQueue.Enqueue(ReturnToLobby);

        }
        else if (si == SI.SI2_1)
        {
            ////2-1 상황 저장
            eventQueue.Enqueue(StartSituation);
            eventQueue.Enqueue(Situation21_1_1);
            eventQueue.Enqueue(Situation21_1_2);
            eventQueue.Enqueue(Situation21_1_3);
            eventQueue.Enqueue(Situation21_1_4);
            eventQueue.Enqueue(Situation21_1_4_1);
            eventQueue.Enqueue(Situation21_1_5);
            eventQueue.Enqueue(Situation21_1_5_1);
            eventQueue.Enqueue(Situation21_1_5_2);
            eventQueue.Enqueue(Situation21_1_6);
            eventQueue.Enqueue(Situation21_1_6_1);
            eventQueue.Enqueue(Situation21_1_7);
            eventQueue.Enqueue(Situation21_1_7_1);
            eventQueue.Enqueue(Situation21_1_8);
            eventQueue.Enqueue(Situation21_1_9);
            eventQueue.Enqueue(ReturnToLobby);
        }
        else if(si == SI.SI2_2)
        {

        }
        else
        {
            Debug.Log("si empty");
        }

    }

    public void Btn_Skip()
    {
        NextSituation();
    }

    private void InitializeNetwork()
    {
        Debug.Log("InitializeNetwork");
        runner = NetworkManager1.RunnerInstance;
        if (runner != null)
        {
            foreach (PlayerRef playerRef in runner.ActivePlayers)
            {
                NetworkObject playerobj = runner.GetPlayerObject(playerRef);
                //Debug.Log("Player name is: " + playerobj.name);
            }
        }
    }

    public void StartSituation()
    {
        SoundManager.Instance.PlayAudioCoroutine(1, 0, () =>
        {
            SoundManager.Instance.PlayAudioCoroutine(1, 0, () =>
            {
                NextSituation();
            });
        });
    }

    public void NextSituation()
    {
        if (eventQueue.Count > 0)
        {
            System.Action currentEvent = eventQueue.Dequeue();
            currentEvent.Invoke();  // 현재 상황 실행
        }
        else
        {
            Debug.Log("모든 상황 완료!");
        }
    }
    

    //또타 애니메이션 끝날때
    IEnumerator AnimCheck(string _boolname, string _animname, System.Action onAnimEnd)
    {
        Animator tottaAnim = totta.GetComponent<Animator>();
        tottaAnim.SetBool(_boolname, true);

        while (tottaAnim.GetBool(_boolname))
        {

            AnimatorStateInfo stateInfo = tottaAnim.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName(_animname) && stateInfo.normalizedTime >= 1.0f)
            {
                
                tottaAnim.SetBool(_boolname, false);
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }

        onAnimEnd.Invoke();
    }

    #region SI_1-1

    private void Situation11_1_4()
    {
        navManager.NPC1Move();
        //NPC1이 달려가서 전화받음
        AnimationManager.Instance.DelayWaitForAnimationCoroutine(1, "Ani_1_1_npc1_02(Run)", 1.5f ,() =>
        {
            //수화기소리
            SoundManager.Instance.PlayAudioCoroutine(2, 10, () =>
            {
                //신호음 소리
                SoundManager.Instance.PlayAudioCoroutine(2, 11, () =>
                {
                    SoundManager.Instance.PlayNa(15);
                });
            });
            //통화함
            AnimationManager.Instance.DelayWaitForAnimationCoroutine(1, "Ani_2_1_npc1_03(CallToPolice)", 20, () =>
            {
                navManager.NPC1Return();
                //돌아옴
                AnimationManager.Instance.DelayWaitForAnimationCoroutine(1, "Ani_1_1_npc1_04(Walk)", 1f ,() =>
                {
                    navManager.NavOff();
                    
                    //흉부 압박술CPR 실시
                    UIManager.Instance.HideUIAfterTimeCoroutine(0, 6, 3, () =>
                    {
                        TestTT.SetActive(true);
                        SoundManager.Instance.PlayNa(5);
                    });
                });
            });
        });
    }
    
    private void Situation11_1_4_1()
    {

        //흉부 압박술CPR
        UIManager.Instance.ActivePatientUI(5);
        //TODO: 스킵버튼
        //UIManager.Instance.ActivePatientUI(6);
    }

    private void Situation11_1_4_2()
    {
        SoundManager.Instance.PlayAudioCoroutine(1, 6, () =>
        {
            NextSituation();
        });
      //또타가 역 도착 알림
    }

    //로딩씬
    private void Situation11_1_5()
    {
        //또타가 다음 역 도착을 알리고 로딩 창 
        // Na_1_1_TT_04.mp3 

         UIManager.Instance.PlayerLodingUICanvas.SetActive(true);
         subway.SetActive(false);
         if (Information != null)
         {
             Information.SetActive(true);
         }
         banpo.SetActive(true);
        //AnimationManager.Instance.npc_anim.npcs[1].gameObject.SetActive(false);
        //AnimationManager.Instance.npc_anim.npcs[7].gameObject.SetActive(true);

        //NPC1이 심폐소생술 중
        //Nav Agent꺼야함
        AnimationManager.Instance.AnimationPlay(1, "Ani_1_1_npc1_06(CPR)", "CPR");
        AnimationManager.Instance.npc_anim.npcs[1].GetComponent<NavMeshAgent>().enabled = false;
        AnimationManager.Instance.NPCPosition(3, AnimationManager.Instance.npc3_pos);

        //환자 움직임
        AnimationManager.Instance.AnimationPlay(2, "Ani_1_1_npc2_01(CPR)");

        //지하철 오브젝트 InActive  ->Loading.cs에서 해결

        //NPC1이 환자에게 CPR 실시 중
    }

    private void Situation11_1_6()
    {
        AnimationManager.Instance.PatientCloseOff();
        AnimationManager.Instance.AnimationPlay(2, "Ani_1_1_npc2_01(CPR)");
       //자동 심장 충격기(AED) 찾아오기 
       UIManager.Instance.HideUIAfterTimeCoroutine(0, 7, 3, () =>
        {
            SoundManager.Instance.PlayNa(13);

            AnimationManager.Instance.AnimationPlay(3, "Ani_1_1_npc3_01(sltIdle)");
            AnimationManager.Instance.DelayWaitForAnimationCoroutine(3, "Ani_1_1_npc3_02(sitPointing)", 3, () =>
            {
                UIManager.Instance.MoveUI.SetActive(true);
            });
        });
    }

    private void Situation11_1_6_01()
    {
        // popUp_noti01 실행  ( 자동심장충격기를 찾았습니다. )
        UIManager.Instance.HideUIAfterTimeCoroutine(0, 8, 3, () =>
        {

            AnimationManager.Instance.AnimationStop(1, "Ani_1_1_npc1_06(CPR)");

            AnimationManager.Instance.npc_anim.npcs[1].transform.position = navManager.prenpc1Obj.transform.position;
            AnimationManager.Instance.npc_anim.npcs[1].transform.rotation = navManager.prenpc1Obj.transform.rotation;
            
            UIManager.Instance.PlayerLodingUICanvas.SetActive(true);
        });
        
    }
       
    private void Situation11_1_6_1()
    {
        //심폐소생술 중
        AnimationManager.Instance.npc_anim.npcs[7].transform.position = NPC1Pos.transform.position;
        AnimationManager.Instance.npc_anim.npcs[7].transform.rotation = NPC1Pos.transform.rotation;
        AnimationManager.Instance.AnimationStop(1, "Ani_1_1_npc1_06(CPR)");

        AnimationManager.Instance.AnimationPlay(3, "Ani_1_1_npc3_01(sltIdle)");
        AnimationManager.Instance.AnimationPlay(3, "Ani_1_1_npc3_03(CPROffender)", "CPR");

        UIManager.Instance.MoveUI.SetActive(false);

        PlayeObjs[0].transform.position = playerPrePos.transform.position;
        PlayeObjs[1].transform.position = playerPrePos.transform.position;
        PlayeObjs[0].transform.rotation = playerPrePos.transform.rotation;
        PlayeObjs[1].transform.rotation = playerPrePos.transform.rotation;

        //자동심장충격기 사용하기
        UIManager.Instance.HideUIAfterTimeCoroutine(0, 9, 5, () =>
        {
            SoundManager.Instance.PlayNa(8);

            UIManager.Instance.ActivePatientUI(2);
            UIManager.Instance.ActivePatientUI(3);
            UIManager.Instance.ActivePatientUI(4);

        });
    }

    private void Situation11_1_7()
    {

        SoundManager.Instance.PlaySFX(4);
        //심전도 분석

        SoundManager.Instance.PlayAudioCoroutine(1, 16, () =>
        {
            SoundManager.Instance.PlayAudioCoroutine(1, 17, () =>
            {
                AnimationManager.Instance.AnimationStop(3, "Ani_1_1_npc3_03(CPROffender)");
                AnimationManager.Instance.AnimationStop(3, "Ani_1_1_npc3_01(sltIdle)");
                AnimationManager.Instance.AnimationStop(2, "Ani_1_1_npc2_01(CPR)");

                UIManager.Instance.HideUIAfterTimeCoroutine(0, 10, 7, () =>
                {
                    
                    AnimationManager.Instance.AnimationAllStop();
                    AnimationManager.Instance.AnimationAllBackWalk(1f);

                    //2초 지연
                    SoundManager.Instance.PlayAudioCoroutine(1, 0, () =>
                    {
                        AnimationManager.Instance.AnimationAllStop();
                        SoundManager.Instance.PlayAudioCoroutine(1, 0, () =>
                        {
                            UIManager.Instance.HideUIAfterTimeCoroutine(0, 11, 3, () =>
                            {
                                //슈팅소리
                                SoundManager.Instance.PlayAudioCoroutine(2, 5, () =>
                                {
                                    NextSituation();
                                });
                            });
                        });
                    });
                });
            });
        });
    }

    private void Situation11_1_8()
    {
       
        UIManager.Instance.HideUIAfterTimeCoroutine(0, 12, 5, () =>
        {
            SoundManager.Instance.PlaySFX(4);
            UIManager.Instance.HideUIAfterTimeCoroutine(0, 13, 5, () =>
            {
                //심패소생술 active
                NextSituation();
            });
        });
    }

    private void Situation11_1_8_1()
    {
        //1-1_title07 (환자상태 확인하기)
        UIManager.Instance.ActivePatientUI(5);
        //UIManager.Instance.ActivePatientUI(6);
    }

    private void Situation11_1_9()
    {
        SoundManager.Instance.bgmSource.loop = false;

        Debug.Log("Situation11_1_9");
        SoundManager.Instance.PlayAudioCoroutine(2, 2, () =>
        {
            SoundManager.Instance.PlayAudioCoroutine(0, 1, ()=>
            {
                NextSituation();
            });

            //1-1_title07 (상황 종료)
            UIManager.Instance.HideUIAfterTimeCoroutine(0, 14, 3, () =>
            {
                AnimationManager.Instance.TottaPlay("Ani_1_1_TT_01(Pointing)", 3);

                SoundManager.Instance.PlayAudioCoroutine(1, 14, () =>
                {
                    AnimationManager.Instance.totta.SetActive(false);
                });
            });
        });
    }

    private void Situation11_1_10()
    {
        Debug.Log("Situation11_1_10");
        UIManager.Instance.EndingCanvas.SetActive(true);
     
    }
    #endregion

    #region SI_1-2

    #endregion

    #region SI_2-1

    private void Situation21_1_1()
    {

        Debug.Log("Situation21_1_1");

       
        SoundManager.Instance.PlayBGM(0);
        SoundManager.Instance.PlayBGM2(0);
       
        // 1-1_title01 (응급환자 구조)
       UIManager.Instance.HideUIAfterTimeCoroutine(0, 0, 3, () =>
       {
           //애니메이션 시작
           SoundManager.Instance.PlayNa(1);
           AnimationManager.Instance.DelayWaitForAnimationCoroutine(1, "Ani_1_1_npc1_01(LookAround)", 4, () =>
           {
            
               //애니메이션 2회 반복 후 또타 등장  또타가 말 다하면 다음상황으로 진행
               // Na_2_1_TT_01.mp3
               SoundManager.Instance.PlayAudioCoroutine(1, 0, () =>
               {
                   AnimationManager.Instance.TottaPlay("Ani_1_1_TT_02(Worry)", 0);

                   SoundManager.Instance.PlayAudioCoroutine(1, 2, () =>
                   {
                       AnimationManager.Instance.totta.SetActive(false);

                        NextSituation();
                   });
               });
       
           });
       
       });
    }

    private void Situation21_1_2()
    {
        Debug.Log("Situation21_1_2");
        //1-1_title02 (환자의식 확인하기)
        Debug.Log("============환자의식 확인하기==============");

        UIManager.Instance.HideUIAfterTimeCoroutine(0, 1, 3, () =>
        {
            // if (si == SI.SI1_1)
            // {
            //     PlayeObjs[0].transform.position = new Vector3(-0.41f, 0f, 0.94f);
            //     PlayeObjs[1].transform.position = new Vector3(-0.41f, 0f, 0.94f);
            // }
            // else if (si == SI.SI2_1)
            // {
            //     PlayeObjs[0].transform.position = new Vector3(0.41f, 0f, 0.94f);
            //     PlayeObjs[1].transform.position = new Vector3(0.41f, 0f, 0.94f);
            // }
            // else
            // {
            //     Debug.Log("SI is Empty");
            // }

            UIManager.Instance.ActivePatientUI(0);
        });
    }

    private void Situation21_1_3()
    {
        Debug.Log("Situation21_1_3");
        //1-1_title03 (구조요청하기)
        //
        UIManager.Instance.HideUIAfterTimeCoroutine(0, 4, 3, () =>
        {
            AnimationManager.Instance.TottaPlay("Ani_1_1_TT_02(Worry)", 1);

            SoundManager.Instance.PlayAudioCoroutine(1, 4, () =>
            {

                AnimationManager.Instance.TottaPlay("Ani_1_1_TT_02(Worry)", 2);

                //gage 생성 NPC1 가리키고 풀되면 다음 -> Gage함수에서 처리
                UIManager.Instance.NPCUI[0].SetActive(true);

                //StartCoroutine(tottaFirst());
                
                UIManager.Instance.HideUIAfterTimeCoroutine(0, 5, 10,()=>
                {
                    AnimationManager.Instance.totta.SetActive(false);
                });
            });
        });
    }

    IEnumerator tottaFirst()
    {
        UIManager.Instance.ActiveUI(0, 5);
        yield return new WaitForSeconds(10f);
        AnimationManager.Instance.totta.SetActive(false);
    }

    private void Situation21_1_3_1()
    {

        SoundManager.Instance.PlayAudioCoroutine(1, 0, () =>
        {
            //수화기소리
            SoundManager.Instance.PlayAudioCoroutine(2, 8, () =>
            {
          
              SoundManager.Instance.PlayNa(13);
                
            });
        });
        
       //통화함
       AnimationManager.Instance.DelayWaitForAnimationCoroutine(1, "Ani_1_1_npc1_03(CallWithSmartphone)", 20, () =>
       {
          // NextSituation();
       });
    }

    private void Situation21_1_4()
    {
        Debug.Log("Situation21_1_4");

       
        SoundManager.Instance.PlayAudioCoroutine(1, 0, () =>
        {
            AnimationManager.Instance.DelayWaitForAnimationCoroutine(1, "Ani_2_1_npc1_03(CallToPolice)", 20, () =>
            {
                // NextSituation();
            });

            SoundManager.Instance.PlayAudioCoroutine(2, 8,() =>
            {
                //사운드 조절 필요함
                SoundManager.Instance.PlayNa(13);
           
            });
           
          
        });

        //통화함
        ArrowUI.SetActive(true);

        //또타 등장
        AnimationManager.Instance.TottaPlay("Ani_1_1_TT_02(Worry)", 3);
        //NPC4에게 ui 생성
        UIManager.Instance.NPCUI[1].SetActive(true);
        //textbox_2_02 팝업
        UIManager.Instance.HideUIAfterTimeCoroutine(0, 6, 10,()=>
        {
            AnimationManager.Instance.totta.SetActive(false);
        });
    }

    private void Situation21_1_4_1()
    {
        //NPC4 애니메이션
        //통화함
        navManager.NPC4Move();
        AnimationManager.Instance.totta.SetActive(false);

        AnimationManager.Instance.DelayWaitForAnimationCoroutine(4 , "Ani_1_1_npc4_03(Running)", 10f, () =>
        {
             NextSituation();
             ArrowUI.SetActive(false);
        });
    }

    private void Situation21_1_5()
    {
        Debug.Log("Situation21_1_5");
        //1-1_title04 (흉부압박술(cpr) 실시하기)
        UIManager.Instance.HideUIAfterTimeCoroutine(0, 7, 3, () =>
        {
            TestTT.SetActive(true);
            SoundManager.Instance.PlayNa(5);
        });
    }

    private void Situation21_1_5_1()
    {
        Debug.Log("Situation21_1_5_1");
        //심폐소생술 버튼 생성
        UIManager.Instance.ActivePatientUI(5);
        UIManager.Instance.ActivePatientUI(6);
    }

    private void Situation21_1_5_2()
    {
        Debug.Log("Situation21_1_5_2");
        //구조자 도착
        UIManager.Instance.HideUIAfterTimeCoroutine(0, 8, 3, () =>
        {
            UIManager.Instance.PlayerLodingUICanvas.SetActive(true);
            AnimationManager.Instance.npc_anim.npcs[4].transform.position = NPC4Pos.transform.position;
            AnimationManager.Instance.npc_anim.npcs[4].transform.rotation = NPC4Pos.transform.rotation;
        });
    }

    private void Situation21_1_6()
    {
        Debug.Log("Situation21_1_6");

        Gallery.SetActive(true);
        AnimationManager.Instance.PatientCloseOff();
        //NPC3 심폐소생술 실시 
        AnimationManager.Instance.AnimationPlay(3, "Ani_1_1_npc3_01(sltIdle)");
        AnimationManager.Instance.npc_anim.npcs[3].gameObject.SetActive(true);
        AnimationManager.Instance.AnimationPlay(3, "Ani_1_1_npc3_03(CPROffender)", "CPR");
        AnimationManager.Instance.AnimationPlay(2, "Ani_1_1_npc2_01(CPR)");
       //1-1_title05 (자동심장충격기(AED) 사용하기)
       UIManager.Instance.HideUIAfterTimeCoroutine(0, 9, 3, () =>
        {
            UIManager.Instance.ActivePatientUI(4);
            //NPC3 흉부압박 멈춤
            AnimationManager.Instance.AnimationAllStop();
            // checkList_aedboard 팝업 생성 Player가 실행하면 step1 부터 체크
            //UIManager.Instance.ActivePatientUI(5);
            SoundManager.Instance.PlayNa(6);
        });
    }

    private void Situation21_1_6_1()
    {
        Debug.Log("Situation21_1_6_1");

        //NPC3 흉부압박 멈춤
        //AnimationManager.Instance.AnimationAllStop();
    

        SoundManager.Instance.PlaySFX(5);
        //심전도 분석
        UIManager.Instance.HideUIAfterTimeCoroutine(0, 10, 5, () =>
        {
            //모든 플레이어 뒤로 감
            AnimationManager.Instance.AnimationAllBackWalk(1);
            
            SoundManager.Instance.PlayAudioCoroutine(1, 10, () =>
            {
                UIManager.Instance.HideUIAfterTimeCoroutine(0, 11, 5, () =>
                {
                    //슈팅소리
                    SoundManager.Instance.PlayAudioCoroutine(2, 6, () =>
                    {
                        NextSituation();
                    });
                });
            });
        });
    }

    private void Situation21_1_7()
    {
        Debug.Log("Situation21_1_7");
        //1-1_title07 (환자상태 확인하기)

        UIManager.Instance.HideUIAfterTimeCoroutine(0, 12, 3.5f, () =>
        {

            SoundManager.Instance.PlayAudioCoroutine(2, 5, () =>
            {
                UIManager.Instance.HideUIAfterTimeCoroutine(0, 13, 3.5f, () =>
                {
                    // CPR Set 한번 더 나옴

                    NextSituation();
                });
            });
            //충전 완료될떄까지 심폐소생술을 멈추지 마세여
         
        });

     
        //심폐소생술 -> 성공하면 사이렌 소리

    }

    private void Situation21_1_7_1()
    {
        Debug.Log("Situation21_1_7_1");
        //CPR 반복
        UIManager.Instance.ActivePatientUI(5);
        UIManager.Instance.ActivePatientUI(6);
    }

    private void Situation21_1_8()
    {
        Debug.Log("Situation21_1_8");
        SoundManager.Instance.PlaySFX(2);
       
       UIManager.Instance.HideUIAfterTimeCoroutine(0, 14, 4, () =>
       {
           AnimationManager.Instance.TottaPlay("Ani_1_1_TT_01(Pointing)", 4);

           SoundManager.Instance.StopSfx();
           //또타 등장
           //또타 대사 끝나면 배경음악 볼륨 커지고 종료 카운트 다운 3-2-1
           SoundManager.Instance.PlayAudioCoroutine(1,12,()=>
           {
               AnimationManager.Instance.totta.SetActive(false);

           });

           SoundManager.Instance.PlayAudioCoroutine(0, 1, () =>
           {
               NextSituation();
           });
           //SoundManager.Instance.PlayNa()
       });
      
    }

    private void Situation21_1_9()
    {

        Debug.Log("Situation21_1_9");
     
            SoundManager.Instance.PlayAudioCoroutine(1, 0, () =>
            {
                UIManager.Instance.EndingCanvas.SetActive(true);
            });
      
    }
    #endregion

    public void ReturnToLobby()
    {
        SceneManager.LoadScene("StartScene");
    }

}
