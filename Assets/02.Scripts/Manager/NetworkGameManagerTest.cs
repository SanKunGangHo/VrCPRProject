using Fusion;
using Fusion.Sockets;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;


public class NetworkGameManagerTest : NetworkBehaviour, INetworkRunnerCallbacks
{
    [Header("Network")]
    public NetworkRunner runner;
    [Header("Totta, NPC Settings")]
   

    public GameObject totta;
    public GameObject TestMovie;

    public GameObject CircleTimer;
    public GameObject playerloadingui;
    public  List<GameObject> playerTPs;
    public GameObject gotoCall;

    public GameObject MovetoMap;

    public GameObject GuidePopup_01;
    public GameObject GuidePopup_02;

    [Header("Position Setting")]
    public GameObject[] playerpos = new GameObject[1];
    public GameObject[] playerpos_aed = new GameObject[1];
    
    public GameObject TottaEndingPos;

    public GameObject NPCS;
    public GameObject NPCS_Pos;

    public  List<GameObject> dummynpc;
    public  List<GameObject> npcsMovePos;

    private Queue<System.Action> eventQueue = new Queue<System.Action>();

    public GameObject _testPlayer;

    public NetworkObject player1; //오른쪽
    public NetworkObject player2; //왼쪽
    
    [Networked]
    public bool CP_NPC2BreathCheck{ get; set; }
    
    [Networked]
    public bool CP_CprCheck { get; set; }
    

    [Header("Player Settings")]
    public SI si;

    public void SetupEventQueue()
    {
        Debug.Log("SetupEventQueue");
    }

    private void Start()
    {
        NetworkManager1.RunnerInstance.GetComponent<NetworkManager1>().ngManager = this;
    }

    public void InitializeNetwork()
    {
        runner = NetworkManager1.RunnerInstance;

        if (runner != null)
        {
            runner.AddCallbacks(this);
            Debug.Log("Callbacks added to runner");
        }
        else
        {
            Debug.LogError("Runner is null in InitializeNetwork");
        }

    }


  

    //Player 배치
    private void Situation1()
    {

        Debug.LogWarning("Situation1 위치 선정");

        SoundManager.Instance.PlayBGM(0);
        SoundManager.Instance.PlayBGM2(0);

     
        if (runner.GetPlayerObject(runner.LocalPlayer).GetComponent<PlayerData>().isCPR)
        {
            player1 = runner.GetPlayerObject(runner.LocalPlayer);
            runner.GetPlayerObject(runner.LocalPlayer).transform.position = playerpos[0].transform.position;
            runner.GetPlayerObject(runner.LocalPlayer).transform.rotation = playerpos[0].transform.rotation;
        }
        
        if (!runner.GetPlayerObject(runner.LocalPlayer).GetComponent<PlayerData>().isCPR)
        {
            player2 = runner.GetPlayerObject(runner.LocalPlayer);
            runner.GetPlayerObject(runner.LocalPlayer).transform.position = playerpos[1].transform.position;
            runner.GetPlayerObject(runner.LocalPlayer).transform.rotation = playerpos[1].transform.rotation;
        }

        //Situation5();
        Invoke(nameof(Situation2), 10f);
        //10초 뒤에 시작 그동안 마네킹 세팅하라고 지시
    }


    private void Situation2()
    {
        Debug.LogWarning("Situation2");
      
        //응급환자 구조
        UIManager.Instance.HideUIAfterTimeCoroutine(0, 0, 3, () =>
        {
            totta.SetActive(true);

            SoundManager.Instance.PlayAudioCoroutine(1, 1, () =>
            {
                totta.SetActive(false);
                Situation3();
            });
        });
    }

    private void Situation3()
    {
       
        Debug.LogWarning("Situation3");

        //환자 의식 확인하기
        UIManager.Instance.HideUIAfterTimeCoroutine(0, 1, 3, () =>
        {
            //터치 ui 생성
            UIManager.Instance.patientUI[0].SetActive(true);
        });
    }

    private void Situation4()
    {
        Debug.LogWarning("Situation4");

        //구조 요청하기
        UIManager.Instance.HideUIAfterTimeCoroutine(0, 3, 3, () =>
        {
            totta.SetActive(true);
            SoundManager.Instance.PlayAudioCoroutine(1, 2, () =>
            {
                totta.SetActive(false);
                
                UIManager.Instance.HideUIAfterTimeCoroutine(0, 4, 10, () =>
                {
                    //남캐에 Gage 생성
                    Situation5();
                });
            });
        });
    }


    private void Situation5()
    {
        Debug.LogWarning("Situation5");

        //cpr실시하기
        UIManager.Instance.HideUIAfterTimeCoroutine(0, 5, 3, () =>
        {
            SoundManager.Instance.PlayNa(3);
            TestMovie.SetActive(true);

            if (runner.GetPlayerObject(runner.LocalPlayer).gameObject == player2.gameObject)
            {
                
                ////또타 애니메이션 생성 + 영상종료누르면 cpr 생성
                gotoCall.SetActive(true);
                //gotoCall.GetComponent<NetworkObjectController>().RPC_Active();
            }
        });
    }


    public void Situation6()
    {
        Debug.LogWarning("Situation6");

        //또타가 도착을 알림
        //맵 지우고 로딩창
        totta.SetActive(true);
        gotoCall.GetComponent<NetworkObjectController>().RPC_InActive();
        //SoundManager.Instance.PlayNa(4);

        SoundManager.Instance.PlayAudioCoroutine(1, 4, () =>
        {
            totta.SetActive(false);
            playerloadingui.SetActive(true);
        });
    }

 


    public void Situation7()
    {
        Debug.LogWarning("Situation7");

        SoundManager.Instance.StopBGM2();
        //NPC이동
        RPC_NPCOut();

        RPC_PlayersMove();

        //AED찾아오기
        //Player2가 심폐소생술 실시
        UIManager.Instance.HideUIAfterTimeCoroutine(0, 6, 3, () =>
        {
            Debug.LogWarning("찾아오기");
            //뒤로 물러나기 -> 실시하기 버튼 누를떄
            
            //심폐소생술 ui 생성
            if (player2.gameObject == runner.GetPlayerObject(runner.LocalPlayer).gameObject)
            {
                GuidePopup_02.SetActive(true);
            }
           
            //GuidePopup_02.GetComponent<NetworkObjectController>().RPC_Active();
        });
    }

    public void Situation7_1()
    {

        Debug.LogWarning("Situation7_1");
        AnimationManager.Instance.AnimationPlay(3, "Ani_1_1_npc3_01(sltIdle)");
        AnimationManager.Instance.AnimationPlay(3, "Ani_1_1_npc3_02(sitPointing)");
        //SoundManager.Instance.PlayNa(13);

        SoundManager.Instance.PlayAudioCoroutine(1, 13, () =>
        {
            if (player1.gameObject == runner.GetPlayerObject(runner.LocalPlayer).gameObject)
            {
                GuidePopup_01.SetActive(true);
            }
            AnimationManager.Instance.AnimationAllStop();
        });

    }
  
    public void Situation8()
    {
        RPC_PlayersMove();

        //UI지우기
        GuidePopup_01.GetComponent<NetworkObjectController>().RPC_InActive();

        Debug.LogWarning("Situation8");

        //NPC3 전화중
        //이젠 스마트폰이 나타납니다.
        AnimationManager.Instance.npc_anim.npcs[3].GetComponent<SmartphoneSpawner>().Rpc_SpawnSmartphone();
        AnimationManager.Instance.AnimationStop(3, "Ani_1_1_npc3_01(sltIdle)");
        AnimationManager.Instance.AnimationPlay(3, "Ani_1_2_npc3_02(PhoneCall)");
        
        UIManager.Instance.patientUI[22].GetComponent<NetworkObjectController>().RPC_Active();
  
        SoundManager.Instance.PlayAudioCoroutine(1, 0, () =>
        {
            //4초딜레이 후 시작
            SoundManager.Instance.PlayAudioCoroutine(1, 0, () =>
            {
                SoundManager.Instance.PlayAudioCoroutine(1, 0, () =>
                {

                    SoundManager.Instance.PlayAudioCoroutine(1, 0, () =>
                    {
                        SoundManager.Instance.PlayAudioCoroutine(1, 15, () =>
                        {

                            AnimationManager.Instance.AnimationAllStop();
                            //aed사용하기
                            UIManager.Instance.HideUIAfterTimeCoroutine(0, 7, 3, () =>
                            {
                                //바닥에 놓여있는
                                UIManager.Instance.HideUIAfterTimeCoroutine(0, 8, 3, () =>
                                {
                                    RPC_PlayersMove();
                                    SoundManager.Instance.PlayNa(5);
                                    Debug.Log("na5");
                                    //aed깔아놓기                    
                                    UIManager.Instance.patientUI[3].GetComponent<NetworkObjectController>().RPC_Active();
                                    UIManager.Instance.patientUI[4].GetComponent<NetworkObjectController>().RPC_Active();
                                    UIManager.Instance.patientUI[5].GetComponent<NetworkObjectController>().RPC_Active();
                                });
                            });
                        });
                    });
                });
            });
        });
    }

    public void Situation9()
    {

        Debug.LogWarning("Situation9");

        //1초기다림
        SoundManager.Instance.PlayAudioCoroutine(1, 0, () =>
        {   
            //심전도 분석, 물러나세요
            UIManager.Instance.HideUIAfterTimeCoroutine(0, 9, 5, () =>
            {
                //충전중입니다.
                SoundManager.Instance.PlayAudioCoroutine(1, 9, () =>
                {
                    //충전 소리
                    SoundManager.Instance.PlayAudioCoroutine(2, 1, () =>
                    {
                        UIManager.Instance.HideUIAfterTimeCoroutine(0, 10, 5, () =>
                        {
                            //애니메이션 뒤로 물리기
                            AnimationManager.Instance.StopAllCoroutines();
                            AnimationManager.Instance.AnimationAllBackWalk();

                            //심장충격을 시작합니다.
                            //슈팅소리
                            SoundManager.Instance.PlayAudioCoroutine(2, 3, () =>
                            {
                                RPC_Situation10();
                            });
                        });
                    });
                });
            });
        });
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_Situation10()
    {
        RPC_PlayersMove();
        Debug.LogWarning("Situation10");
        
        //aed1번 끄기
        UIManager.Instance.AED1off();

        //환자 상태 확인하기 
        UIManager.Instance.HideUIAfterTimeCoroutine(0, 11, 5, () =>
        {
            //자동심장충격기 사용학습을 위해
            UIManager.Instance.HideUIAfterTimeCoroutine(0, 16, 5, () =>
            {
                SoundManager.Instance.PlayNa(5);
                UIManager.Instance.patientUI[22].GetComponent<NetworkObjectController>().RPC_Active(); 
                UIManager.Instance.AED2On();
            });
        });
    }

    public void Situation11()
    {
        SoundManager.Instance.StopSfx();
        UIManager.Instance.patientUI[22].GetComponent<NetworkObjectController>().RPC_InActive(); 
        
        Debug.LogWarning("Situation11");
        //1초기다림
        SoundManager.Instance.PlayAudioCoroutine(1, 0, () =>
        {
                //심전도 분석, 물러나세요
                UIManager.Instance.HideUIAfterTimeCoroutine(0, 9, 5, () =>
                {
                    //충전중입ㄴ디ㅏ
                    SoundManager.Instance.PlayAudioCoroutine(1, 9, () =>
                    {
                        //충전 소리
                        SoundManager.Instance.PlayAudioCoroutine(2, 1, () =>
                        {

                            UIManager.Instance.HideUIAfterTimeCoroutine(0, 10, 5, () =>
                            {

                                //애니메이션 뒤로 물리기
                                AnimationManager.Instance.StopAllCoroutines();
                                AnimationManager.Instance.AnimationAllBackWalk();

                               //슈팅소리
                               SoundManager.Instance.PlayAudioCoroutine(2, 3, () =>
                               {
                                   Situation12();
                               });
                            });
                        });
                    });
                });
        });
    }



    public void Situation12()
    {
        Debug.LogWarning("Situation11");

        //사이렌소리
        SoundManager.Instance.PlaySFX(5);

        //상황종료
        UIManager.Instance.HideUIAfterTimeCoroutine(0, 13, 5, () =>
        {
            SoundManager.Instance.StopSfx();
            //또타등장
            totta.SetActive(true);

            SoundManager.Instance.PlayAudioCoroutine(1, 11,()=>
            {
                totta.SetActive(false);
            });

            SoundManager.Instance.PlayAudioCoroutine(0, 1, () =>
            {
                Situation13();
            });


        });
    }
    public void Situation13()
    {
        Debug.LogWarning("Situation12");
       CircleTimer.SetActive(true);
        //엔딩
    }
    
 


    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RpcNextSituation()
    {
        Debug.LogWarning("RpcNextSituation");
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


    public void ReturnToLobby()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("StartScene");
    }


    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_OnPlayerJoined2()
    {
        Debug.Log("OnPlayerJoined");
        FadeManager.Instance.StartNetworkFadeIn();
        InitializeNetwork();
        SetupEventQueue();

       
        int playerIndex = runner.ActivePlayers.Count();  // 1 또는 2

        //Join한 Player가 2명일 경우 시작
        if (playerIndex == 2)
        {

            Debug.Log("DelayedRpcCall");
            StartCoroutine(DelayedRpcCall());
          
        }

    }



    // runner가 Player를 저장하기 위한 시간 딜레이
    private IEnumerator DelayedRpcCall()
    {
        Debug.Log("딜레이 후 상황 시작");
        yield return new WaitForSeconds(2f);

        Situation1();

    }


    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_Player1Move(int i)
    {
        StartCoroutine(RPC_Player1Move_Coroutine(i));
    }

    IEnumerator RPC_Player1Move_Coroutine(int i)
    {
        yield return StartCoroutine(RPC_Player1Move_Coroutine2(i));
        if (i == 2)
        {
            yield return StartCoroutine(RPC_Player1Move_Coroutine2(i));
            Debug.LogWarning("RPC_Player1Move_222222");
            GuidePopup_01.GetComponent<NetworkObjectController>().RPC_InActive();
        }
    }

    IEnumerator RPC_Player1Move_Coroutine2(int i)
    {
        player1.transform.position = playerTPs[i].transform.position;
        player1.transform.rotation = playerTPs[i].transform.rotation;
        yield break;
    }


    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_Player2Move(int i)
    {
        //왼손으로 눌러야 가짐 
        Debug.LogWarning("RPC_Player2Move");
        Debug.Log(player2.gameObject);
        player2.transform.position = playerTPs[i].transform.position;
        player2.transform.rotation = playerTPs[i].transform.rotation;
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_NPC3()
    {
        AnimationManager.Instance.AnimationStop(3, "Ani_1_1_npc3_03(CPROffender)");
        AnimationManager.Instance.AnimationStop(3, "Ani_1_1_npc3_01(sltIdle)");
        AnimationManager.Instance.AnimateSingleNPCBackWalk(3);

        SoundManager.Instance.PlayAudioCoroutine(1, 0, () =>
        {
            SoundManager.Instance.PlayAudioCoroutine(1, 0, () =>
            {
                Situation7_1();
            });
        });
       
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_NPCOut()
    {
        foreach(GameObject dummy in dummynpc)
        {
            dummy.SetActive(false);
        }

        AnimationManager.Instance.NPCPosition(1, npcsMovePos[1]);
        AnimationManager.Instance.NPCPosition(5, npcsMovePos[5]);
        AnimationManager.Instance.NPCPosition(6, npcsMovePos[6]);

    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_Player1Rrturn()
    {
        player1.transform.position = playerpos_aed[0].transform.position;
        //player1.transform.rotation = playerpos_aed[0].transform.rotation;
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_TouchPaitent()
    {
        if (FindObjectOfType<NetworkGameManagerTest>().si == SI.SI1_2)
        {

            UIManager.Instance.InActivePatientUI(0);

            //여보세요
            UIManager.Instance.HideUIAfterTimeCoroutine(0, 14, 8, () =>
            {
                UIManager.Instance.ActivePatientUI(1);
               // SoundManager.Instance.PlayNa(12);

                totta.SetActive(true);
                SoundManager.Instance.PlayAudioCoroutine(1, 12, () =>
                {
                    totta.SetActive(false);
                });

                StartCoroutine(RPC_TouchPaitent_Coroutine());
            });
        }
    }

    IEnumerator RPC_TouchPaitent_Coroutine()
    {
        //icon_touch02 I/F 등장 + textbox_1-1_01 팝업 -> 어깨 두들기기
        //환자의식확인위해
        UIManager.Instance.HideUIAfterTimeCoroutine(0, 2, 10, () =>
        {
            UIManager.Instance.ActiveUI(0,2);
        });
        
        UIManager.Instance.patientUI[21].GetComponent<Gage_BreathCheck>().RPC_BeginBreathCheck();
        
        while (!CP_NPC2BreathCheck)
        {
            yield return null;
        }
            
        UIManager.Instance.InActivePatientUI(1);
        Situation4();
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_PlayersMove()
    {
        //이동
        if (runner.GetPlayerObject(runner.LocalPlayer).GetComponent<PlayerData>().isCPR)
        {
            player1.transform.position = playerpos_aed[0].transform.position;
            _testPlayer.transform.rotation = playerpos_aed[0].transform.rotation;
            //player1.transform.rotation = playerpos_aed[0].transform.rotation;
        }
        else
        {
            player2.transform.position = playerpos_aed[1].transform.position;
            _testPlayer.transform.rotation = playerpos_aed[1].transform.rotation;
            //player2.transform.rotation = playerpos_aed[1].transform.rotation;
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_AfterMovie()
    {
        if (player2 == null)
        {
            Debug.Log("player2 null");
        }

        if (runner.GetPlayerObject(runner.LocalPlayer).gameObject.TryGetComponent<PlayerData>(out PlayerData playerData))
        {
            Debug.Log("runner.GetPlayerObject(runner.LocalPlayer).gameObject.TryGetComponent<PlayerData>(out PlayerData playerData)");
        }
        
        if (!playerData.isCPR)
        {
            Debug.Log("player2 cpr false");
            gotoCall.GetComponent<NetworkObjectController>().RPC_Active();
        }
        else
        {
            Debug.Log("player1");
        }
    }
    
    #region RPC


    public static implicit operator NetworkGameManagerTest(NetworkGameManager v)
    {
        throw new NotImplementedException();
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }


    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {

    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }
    #endregion
    
}
