using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;


public class NetworkGameManagerTest_Station : NetworkBehaviour, INetworkRunnerCallbacks
{


    private Queue<System.Action> eventQueue = new Queue<System.Action>();
    public NetworkRunner runner;

    public GameObject totta;
    public GameObject TestMovie;

    public GameObject CircleTimer;
    public GameObject playerloadingui;
    public  List<GameObject> playerTPs;
    public GameObject gotoCall;

    public GameObject MovetoMap;

    public GameObject totta_T;

    public GameObject GuidePopup_01;
    public GameObject GuidePopup_02;

    public GameObject[] playerpos = new GameObject[1];
    public NetworkLinkedList<NetworkObject> test;
    
    public NetworkObject player1;
    public NetworkObject player2;
    
    public GameObject playerTT;
    public List<Camera> TTsCameras;
    
    public ExtraUIManager_Station extraUIManager;
    public ProgressUIManager progressUIManager;
    public CallUIManager callUIManager;
    
    public NetworkLinkedList<NetworkObject> Test { get; private set; }

    [Header("Player Settings")]
    public SI si;

    public bool isTest = false;
    
    [Networked]
    public bool CP_PatientTouchUI { get; set; }
    [Networked]
    public bool CP_PatientBreathCheck { get; set; }
    [Networked]
    public bool CP_NPC4Gage { get; set; }
    [Networked]
    public bool CP_NPC4Departure { get; set; }
    [Networked]
    public bool CP_CPRDone { get; set; }
    [Networked] 
    public bool CP_EmergencyCall { get; set; }

    [Header("SkipGesture")] public GameObject helpButton;
    public bool wantSkip = false;

    private bool st2Player1BackingField;
    [Networked]
    public bool st2Player1
    {
        get => st2Player1BackingField; 
        set => st2Player1BackingField = value; 
    }
    
    private bool st2Player2BackingField;
    [Networked]
    public bool st2Player2
    {
        get => st2Player2BackingField; 
        set => st2Player2BackingField = value; 
    }
    
    public NavManager_Station navManager_Station;

    public void SkipButton()
    {
        wantSkip = true;
    }

    IEnumerator ShowHelp()
    {
        helpButton.SetActive(false);
        wantSkip = false;
        yield return new WaitForSeconds(10f); //10초동안 아무것도 못했을 때
        helpButton.SetActive(true);
    }

    private void Awake()
    {
        StartCoroutine(FindRunner());
    }

    private IEnumerator FindRunner()
    {
        int count = 0;
        while (NetworkManager1.RunnerInstance == null)
        {
            //Debug.Log("Connecting count: " + count);
            count++;
            yield return null;
        }
        InitializeNetwork();
    }

    private NetworkObject GetPlayerByRole(bool isCPR)
    {
        Debug.Log("GetPlayerByRole");
        // isCPR 값에 따라 플레이어를 구분하여 반환
        foreach (var player in runner.ActivePlayers)
        {
            NetworkObject playerObject = runner.GetPlayerObject(player);

            if (playerObject != null && playerObject.GetComponent<PlayerData>().isCPR == isCPR)
            {
                return playerObject;
            }
        }
        return null;
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    private void Rpc_Movement(NetworkObject player, int i)
    {
        Debug.Log("Movement: " + player.name);
        if (player != runner.GetPlayerObject(runner.LocalPlayer))
        {
            Debug.Log("Movement1: " + player.name);
            player.transform.position = playerpos[i].transform.position;
            player.transform.rotation = playerpos[i].transform.rotation;
        }
        else
        {
            Debug.Log("Movement2: " + player.name);
            playerTT.transform.position = playerpos[i].transform.position;
            playerTT.transform.rotation = playerpos[i].transform.rotation;
        }
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


    #region Situation_subway 
    
    bool IsCoroutineRunning(string coroutineName)
    {
        foreach (var activeCoroutine in GetComponents<MonoBehaviour>())
        {
            if (activeCoroutine.GetType().GetMethod(coroutineName) != null)
            {
                return true;
            }
        }
        return false;
    }


    IEnumerator StartSituation()
    {
        helpButton.SetActive(false);
        //먼저 이동
        yield return StartCoroutine(Situation1_PlayerCheck());
        yield return StartCoroutine(Situation_PlayerCharacterMove());

        yield return new WaitForSeconds(10f); //20초대기 UI로  변경
        
        Situation_ST1();
    }
    
    private void Situation_ST1()
    {
        SoundManager.Instance.PlayBGM(0);
        //응급환자 구조
        UIManager.Instance.HideUIAfterTimeCoroutine(0, 0, 3, () =>
        {
            AnimationManager.Instance.TottaPlay("Ani_1_1_TT_01(Pointing)", 0);
            //애니메 시작
            SoundManager.Instance.PlayAudioCoroutine(1, 1, () =>
            {//TOtta 움직임
                AnimationManager.Instance.totta.SetActive(false);
                //PlayNa(1);
                SoundManager.Instance.PlayAudioCoroutine(1, 0, () =>
                {
                    StartCoroutine(PatientUICoroutine());
                });
            });
        });
    }

    public IEnumerator PatientUICoroutine()
    {
        Debug.Log("PatientUICoroutine");
        UIManager.Instance.HideUIAfterTimeCoroutine(0, 1, 3, () =>
        {
            UIManager.Instance.HideUIAfterTimeCoroutine(0, 25, 5, () =>
            {
                UIManager.Instance.ActivePatientUI(0);
            });
        });
        
        while (!CP_PatientTouchUI)
        {
            yield return null;
        }
        
        AnimationManager.Instance.totta.SetActive(false);
        Rpc_Situation_ST2();
        
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void Rpc_Situation_ST2()
    {
        Debug.LogWarning("Situation_ST2");

        //응급환자 구조
        UIManager.Instance.HideUIAfterTimeCoroutine(0, 1, 3, () =>
        {
            StartCoroutine(Situation_ST2_PatientCheck());
        });
    }

    IEnumerator Situation_ST2_PatientCheck()
    {
        //바라보기
        UIManager.Instance.ActiveUI(0,15);
        yield return new WaitForSeconds(1f);
        //바라보는 로직
        UIManager.Instance.ActivePatientUI(1);
        UIManager.Instance.patientUI[15].GetComponent<Gage_BreathCheck>().RPC_BeginBreathCheck();
        // //응급환자 구조
        // UIManager.Instance.HideUIAfterTimeCoroutine(0, 15, 10, () =>
        // {
        while (!CP_PatientBreathCheck && !wantSkip)
        {
            yield return null;    
        } 
        
        //구조 요청하기
        UIManager.Instance.HideUIAfterTimeCoroutine(0, 3, 3, () =>
        {
            StartCoroutine(Situation_ST2_UICoroutine());
        });
    }

    private IEnumerator Situation_ST2_UICoroutine()
    {
        extraUIManager.MultiUIs_2[0].gameObject.SetActive(true);
        
        yield return new WaitForSeconds(9); //이 시간 동안 지목하여 신고요청
        
        extraUIManager.MultiUIs_2[0].gameObject.SetActive(false);
        
        AnimationManager.Instance.totta.SetActive(false);
        
        if (!runner.GetPlayerObject(runner.LocalPlayer).GetComponent<PlayerData>().isCPR)
        {
            StartCoroutine(Situation_ST2_Player1Side()); //AED 요청과 CPR
        }
        
        if (runner.GetPlayerObject(runner.LocalPlayer).GetComponent<PlayerData>().isCPR)
        {
            StartCoroutine(Situation_ST2_Player2Side()); //신고 시작
        }
        
        float player1count = 0;
        float player2count = 0;
        while (!st2Player1 || !st2Player2)
        {
            if (player1count > 10 || player2count > 20) break;
            if (st2Player1)
            {
                Debug.Log("Player1 대기중");
                player1count += 0.1f;
            }

            if (st2Player2)
            {
                Debug.Log("Player2 대기중");
                player2count += 0.1f;
            }
            
            //Debug.Log("st2 메인 스크립트 대기중");
            yield return new WaitForSeconds(0.1f);
        }
        
        Debug.Log("end");
        player2.GetComponent<PlayerData>().smartphone.GetComponent<NetworkObjectController>().RPC_InActive();
    
        
        st2Player1 = false;
        st2Player2 = false;
        
        //다음 시츄에이션
        Rpc_Situation_ST3();
    }
    
    private IEnumerator Situation_ST2_Player1Side()
    {
        // extraUIManager.MultiUIs_2[0].gameObject.SetActive(true);
        // extraUIManager.MultiUIs_2[0].sprite = UIManager.Instance.titleSprites[24];
        UIManager.Instance.HideUIAfterTimeCoroutine(0, 24, 3);
        yield return new WaitForSeconds(3);
        //extraUIManager.MultiUIs_2[0].gameObject.SetActive(false);
        
        //지목 UI 켜주기
        UIManager.Instance.NPCUI[0].SetActive(true);
        //StartCoroutine(ShowHelp());
        while (!CP_NPC4Gage && !wantSkip) //지목 끝날 때까지 안풀어주기
        {
            yield return new WaitForSeconds(0.1f);
        }
        
        navManager_Station.RPC_NPC4Move();
        
        // //npc4 이동 "끝날 때까지 안풀어주기"
        // while (!CP_NPC4Departure)
        // {
        //     yield return null;
        // }
        
        //CPR
        UIManager.Instance.patientUI[2].GetComponent<NetworkObjectController>().RPC_Active(); //CPRSet
        //CPR Totta 소환
        UIManager.Instance.HideUIAfterTimeCoroutine(0, 5, 3, () =>
        {
            TestMovie.SetActive(true);
            SoundManager.Instance.PlayNa(3);
        });

        //StartCoroutine(ShowHelp());
        
        while (!CP_CPRDone && !wantSkip)
        {
            yield return null;
        }
        
        //StartCoroutine(ShowHelp());

        while (!CP_NPC4Departure && !wantSkip)
        {
            yield return null;
        }

        //StartCoroutine(ShowHelp());
        
        while (!st2Player1 && !wantSkip)
        {
            if (CP_NPC4Gage && CP_NPC4Departure && CP_CPRDone)
            {
                Rpc_Log(1, "player 1: All Checked");
                st2Player1 = true;
            }
            else
            {
                Rpc_Log(1, "player 1: Not All Checked");
                st2Player1 = false;
            }
        }

        if (wantSkip)
        {
            st2Player1 = true;
        }

        wantSkip = false;
        //player1의 심폐소생술 & 지목
    }

    private IEnumerator Situation_ST2_Player2Side()
    {
        PlayerData player2Data = player2.GetComponent<PlayerData>();
        
        //스마트폰 켜줌
        //스마트폰 픽업 사운드
        
        player2Data.smartphone.GetComponent<NetworkObjectController>().RPC_Active();
        SoundManager.Instance.PlaySFX(16);
        

        //StartCoroutine(ShowHelp());
        while (!CP_EmergencyCall && !wantSkip)
        {
            yield return null;
        }
        Rpc_Log(2, "player 2: emergency call end");
        
        player2Data.smartphone.SetActive(false);
        
        wantSkip = false;
        st2Player2 = true;
        //player2의 전화
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    private void Rpc_Situation_ST3()
    {
        // AED사용
        UIManager.Instance.NPCUI[0].SetActive(false);
        UIManager.Instance.patientUI[2].GetComponent<NetworkObjectController>().RPC_Active(); //CPRSet
        UIManager.Instance.HideUIAfterTimeCoroutine(0, 7, 3, ()=>
        {
            StartCoroutine(Situation_ST3_Coroutine());
        });
    }

    IEnumerator Situation_ST3_Coroutine()
    {
        // Animator patient = AnimationManager.Instance.npc_anim.npcs[2].gameObject.GetComponent<Animator>();
        // patient.transform.GetChild(1).gameObject.SetActive(false);
        // patient.transform.GetChild(0).gameObject.SetActive(true);
        
        AnimationManager.Instance.PatientCloseOff();
        
        //AED : 바닥에 놓여있는 자동심장충격기를 사용하여, 응급환자를 구조하세요
        UIManager.Instance.HideUIAfterTimeCoroutine(0, 23, 2, () => {
            UIManager.Instance.ActivePatientUI(3);
            UIManager.Instance.patientUI[10].GetComponent<NetworkObjectController>().RPC_Active();
            UIManager.Instance.patientUI[11].GetComponent<NetworkObjectController>().RPC_Active();
            
            //IManager.Instance.patientUI[9].GetComponent<NetworkObjectController>().RPC_Active();
        });
        SoundManager.Instance.PlayAudioCoroutine(1, 5, () =>
        {
            
        });
        //StartCoroutine(ShowHelp());
        while (!wantSkip)
        {
            yield return null;
        }

        if (wantSkip)
        {
            UIManager.Instance.InActivePatientUI(3);
            UIManager.Instance.patientUI[10].GetComponent<NetworkObjectController>().RPC_InActive();
            UIManager.Instance.patientUI[11].GetComponent<NetworkObjectController>().RPC_InActive();
            Situation_ST4();
        }
        yield break;
    }

    public void Situation_ST4()
    {
        PlayerData player2Data = player2.GetComponent<PlayerData>();
        player2Data.smartphone.transform.GetChild(0).GetComponent<SmartphoneTriggerEvent>().AllCoroutineStop();
        player2Data.smartphone.GetComponent<NetworkObjectController>().RPC_InActive();
        Destroy(extraUIManager.MultiUIs_1[0].gameObject);
        
        SoundManager.Instance.sfxSource.loop = false;
        AnimationManager.Instance.npc_anim.npcs[2].gameObject.GetComponent<Animator>().SetBool("is_walk", false);
        //흉부 압박을 멈추고 자리에서 물러나세요
        UIManager.Instance.HideUIAfterTimeCoroutine(0, 12, 2, () =>
        {
            //심전도 분석중입니다. 모두 뒤로 물러나세요.
            UIManager.Instance.HideUIAfterTimeCoroutine(0, 8, 5, () =>
            {
                //전기충격이 필요합니다. 충전중입니다.
                SoundManager.Instance.PlayAudioCoroutine(1, 18, () =>
                {
                    //충전소리
                    SoundManager.Instance.PlayAudioCoroutine(2, 1, () => 
                    { 
                        //심장 충격을 시작합니다. 모두 뒤로 물러나세요
                        UIManager.Instance.HideUIAfterTimeCoroutine(0, 9, 5, () =>
                        {
                            //4번 npc 네브메쉬 지우기
                            AnimationManager.Instance.npc_anim.npcs[4].GetComponent<NavMeshAgent>().enabled = false;
                            //물러나기
                            AnimationManager.Instance.AnimationAllStop();
                            AnimationManager.Instance.AnimationAllBackWalk();
                            
                            //shoot
                            SoundManager.Instance.PlayAudioCoroutine(2, 3, () =>
                            {
                                extraUIManager.AED_Delete();
                                RPC_Situation_ST5();
                            });
                        });
                    });
                });
            });
        });
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RPC_Situation_ST5()
    {
        // Rpc_Movement(player2, 0);
        // Rpc_Movement(player1, 1);
        
        //환자 상태 확인하기
        UIManager.Instance.HideUIAfterTimeCoroutine(0, 13, 5, () =>
        {
            //SoundManager.Instance.PlayNa(5);
            //aed2  깔아두기
            UIManager.Instance.HideUIAfterTimeCoroutine(0, 13, 5, () =>
            {
                //UIManager.Instance.patientUI[8].GetComponent<NetworkObjectController>().RPC_Active();
                UIManager.Instance.HideUIAfterTimeCoroutine(0, 14, 3, () =>
                {
                    UIManager.Instance.patientUI[12].GetComponent<NetworkObjectController>().RPC_Active();
                    UIManager.Instance.patientUI[13].GetComponent<NetworkObjectController>().RPC_Active();
                    UIManager.Instance.patientUI[14].GetComponent<NetworkObjectController>().RPC_Active();
                    
                    UIManager.Instance.patientUI[9].GetComponent<NetworkObjectController>().RPC_Active();
                    //심폐소생술 AED 끝날 때 끝나도록 변환
                    
                    SoundManager.Instance.PlayNa(5);
                    CP_CPRDone = false;
                    //StartCoroutine(ShowHelp());
                    StartCoroutine(Situation_ST5_SkipCoroutine());
                });
            });
        });
    }

    IEnumerator Situation_ST5_SkipCoroutine()
    {
        while (!wantSkip)
        {
            yield return null;
        }

        if (wantSkip)
        {
            UIManager.Instance.patientUI[12].GetComponent<NetworkObjectController>().RPC_InActive();
            UIManager.Instance.patientUI[13].GetComponent<NetworkObjectController>().RPC_InActive();
            UIManager.Instance.patientUI[14].GetComponent<NetworkObjectController>().RPC_InActive();
            Situation_ST6();
        }
    }

    //AED진행하면 나옴
    public void Situation_ST6()
    {
        StopCoroutine(Situation_ST5_SkipCoroutine());
        //StartCoroutine(Situation_ST6_Coroutine_CPR());
        StartCoroutine(Situation_ST6_Coroutine_AED());
        
    }

    IEnumerator Situation_ST6_Coroutine_AED()
    {
        UIManager.Instance.patientUI[9].GetComponent<NetworkObjectController>().RPC_InActive();
        Debug.Log("SecondAED");
        yield return new WaitForSeconds(1f);
        //흉부압박을 멈추세요
        UIManager.Instance.HideUIAfterTimeCoroutine(0, 12, 2, () =>
        {
            //심전도 분석중입니다. 모두 뒤로 물러나세요.
            UIManager.Instance.HideUIAfterTimeCoroutine(0, 8, 5, () =>
            {
                //전기충격이 필요합니다. 충전중입니다.
                SoundManager.Instance.PlayAudioCoroutine(1, 18, () =>
                {
                    //충전소리
                    SoundManager.Instance.PlayAudioCoroutine(2, 1, () =>
                    {
                        //심장 충격을 시작합니다. 모두 뒤로 물러나세요
                        UIManager.Instance.HideUIAfterTimeCoroutine(0, 9, 5, () =>
                        {
                            //물러나기
                            AnimationManager.Instance.AnimationAllStop();
                            AnimationManager.Instance.AnimationAllBackWalk();

                            //shoot
                            SoundManager.Instance.PlayAudioCoroutine(2, 3, () =>
                            {
                                Situation_ST7();
                            });
                        });
                    });
                });
            });
        });
        yield break;
    }

    public void Situation_ST7()
    {
        Debug.Log("Situation_ST7");
        
        SoundManager.Instance.PlayAudioCoroutine(2, 5, () =>
        {
            //상황종료
            UIManager.Instance.HideUIAfterTimeCoroutine(0, 10, 3, () =>
            {
                AnimationManager.Instance.TottaPlay("Ani_1_1_TT_04(Bye)", 4);
                SoundManager.Instance.PlayNa(19);
                StartCoroutine(Situation_ST8());
            });
        });
    }

    IEnumerator Situation_ST8()
    {
        //엔딩곡
        SoundManager.Instance.PlayBGM(1);
        while (SoundManager.Instance.bgmSource.isPlaying)
        {
            yield return null;
        }
        CircleTimer.SetActive(true);
    }
    
    IEnumerator Situation1_PlayerCheck()
    {
        Debug.Log("Situation1_PlayerCheck");
        while (player1 == null)
        {
            player1 = GetPlayerByRole(false);

            yield return null;
        }

        while (player2 == null)
        {
            player2 = GetPlayerByRole(true);
            yield return null;
        }
        
        
    }

    IEnumerator Situation_PlayerCharacterMove()
    {
        if (player1 == runner.GetPlayerObject(runner.LocalPlayer))
        {
            Rpc_Log(0, "player 1: move");
            player1.transform.position = playerpos[0].transform.position;
            player1.transform.rotation = playerpos[0].transform.rotation;
        }
        
        yield return null;
        
        if (player2 == runner.GetPlayerObject(runner.LocalPlayer))
        {
            Rpc_Log(0, "player 2: move");
            player2.transform.position = playerpos[1].transform.position;
            player2.transform.rotation = playerpos[1].transform.rotation;
        }
    }

    public void ReturnToLobby()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("StartScene");
    }
    
    #endregion


    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_OnPlayerJoined2()
    {
        Debug.Log("OnPlayerJoined");
        FadeManager.Instance.StartNetworkFadeIn();
        
        InitializeNetwork();
        // //SetupEventQueue();
        
        int playerIndex = runner.ActivePlayers.Count();  // 1 또는 2
        
        Debug.Log("Player Index: " + playerIndex);
        //StartCoroutine(DelayedRpcCall());
        //Join한 Player가 2명일 경우 시작

        // foreach (var mainCamera in TTsCameras)
        // {
        //     switch (runner.GetPlayerObject(runner.LocalPlayer).GetComponent<PlayerData>().isCPR)
        //     {
        //         case true:
        //             int player2LayerMask = LayerMask.NameToLayer("Player2");
        //             mainCamera.cullingMask &= ~(1 << player2LayerMask); // "Player2" 레이어 제외
        //             break;
        //         case false:
        //             int player1LayerMask = LayerMask.NameToLayer("Player1");
        //             mainCamera.cullingMask &= ~(1 << player1LayerMask); // "Player1" 레이어 제외
        //             break;
        //     }
        // }
        
        if (playerIndex == 2)
        {
            Debug.Log("DelayedRpcCall");
            StartCoroutine(StartSituation());
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_NPC3()
    {
        AnimationManager.Instance.NPCPosition(3, AnimationManager.Instance.npc3_pos);
        AnimationManager.Instance.AnimationStop(3, "Ani_1_1_npc3_03(CPROffender)");
        AnimationManager.Instance.AnimationStop(3, "Ani_1_1_npc3_01(sltIdle)");
    }
    
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void Rpc_Log(int importance, string log)
    {
        switch (importance)
        {
            case 0 :
                Debug.Log(log);
                break;
            case 1 :
                Debug.LogWarning(log);
                break;
            case 2 :
                Debug.LogError(log);
                break;
            default :
                Debug.Log(log);
                break;
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void Rpc_patientTouch()
    {
        UIManager.Instance.InActivePatientUI(0);

        //여보세요
        UIManager.Instance.HideUIAfterTimeCoroutine(0, 15, 8, () =>
        {
            UIManager.Instance.ActivePatientUI(1);
            
            AnimationManager.Instance.TottaPlay("Ani_1_1_TT_02(Worry)", 0);
            SoundManager.Instance.PlayAudioCoroutine(1, 2, () =>
            {
                totta.SetActive(false);
            });
        });

        CP_PatientTouchUI = true;
    }
    

    #region RPC


    public static implicit operator NetworkGameManagerTest_Station(NetworkGameManager v)
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
