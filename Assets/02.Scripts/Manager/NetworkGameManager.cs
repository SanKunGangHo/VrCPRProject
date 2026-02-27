using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class NetworkGameManager : NetworkBehaviour, INetworkRunnerCallbacks
{
  

    private Queue<System.Action> eventQueue = new Queue<System.Action>();

    public NetworkRunner runner;

    public GameObject TestTT;

    public string myId;

    [Header("Player Settings")]
    public SI si;

    public GameObject PlayerTT;
    public List<GameObject> PlayeObjs;
    public GameObject PlayerGageObj;
    public List<GameObject> SpawnPoints;


    private bool player1On = false;
    private bool player2On = false;

    //private NetworkDictionary<string, NetworkObject> _playerObjList;

    //[Networked]
    //public NetworkDictionary<string, NetworkObject> PlayerObjList
    //{
    //    get => _playerObjList;
    //    //private set => _playerObjList = value;
    //}


    public void SetupEventQueue()
    {
        Debug.Log("SetupEventQueue");
        if(si == SI.SI1_2)
        {
            eventQueue.Enqueue(StartSituation);
            eventQueue.Enqueue(Situation21_1_1);
            eventQueue.Enqueue(Situation12_1_2);
            //eventQueue.Enqueue(Situation12_1_3_1);
            //eventQueue.Enqueue(Situation12_1_3_2);
            //eventQueue.Enqueue(Situation12_1_4);
            //eventQueue.Enqueue(Situation12_1_5);
            //eventQueue.Enqueue(Situation12_1_6_1);
            //eventQueue.Enqueue(Situation12_1_6_2);
            //eventQueue.Enqueue(Situation12_1_7);
            //eventQueue.Enqueue(Situation12_1_8);
            //eventQueue.Enqueue(Situation12_1_9);
            //eventQueue.Enqueue(Situation12_1_9_1);
            //eventQueue.Enqueue(Situation12_1_9_2);
            //eventQueue.Enqueue(Situation12_1_9_3);
            //eventQueue.Enqueue(Situation12_1_10);
            //eventQueue.Enqueue(ReturnToLobby);
        }
        else if (si == SI.SI2_2)
        {
            eventQueue.Enqueue(StartSituation);
            eventQueue.Enqueue(Situation21_1_1);
            eventQueue.Enqueue(Situation12_1_2);
            eventQueue.Enqueue(Situation12_1_3_1);
            eventQueue.Enqueue(Situation12_1_3_2);
            eventQueue.Enqueue(Situation12_1_4);
            eventQueue.Enqueue(Situation12_1_5);
            eventQueue.Enqueue(Situation12_1_6_1);
            eventQueue.Enqueue(Situation12_1_6_2);
            eventQueue.Enqueue(Situation12_1_7);
            eventQueue.Enqueue(Situation12_1_8);
            eventQueue.Enqueue(Situation12_1_9);
            eventQueue.Enqueue(Situation12_1_9_1);
            eventQueue.Enqueue(Situation12_1_9_2);
            eventQueue.Enqueue(Situation12_1_9_3);
            eventQueue.Enqueue(Situation12_1_10);
            eventQueue.Enqueue(ReturnToLobby);
        }
        else
        {
            Debug.Log("si empty");
        }

    }
    //protected override void Awake()
    //{
    //    base.Awake();
    //}
    
    private void Start()
    {
       // _playerObjList = new NetworkDictionary<string, NetworkObject>();
       // NetworkManager1.RunnerInstance.GetComponent<NetworkManager1>().ngManager = this;
        //SetupEventQueue();
       // InitializeNetwork();
        
    }


    public void InitializeNetwork()
    {
       
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


    private void StartSituation()
    {
        SoundManager.Instance.PlayAudioCoroutine(1, 0, () =>
        {
            SoundManager.Instance.PlayAudioCoroutine(1, 0, () =>
            {
                RpcNextSituation();
            });
        });
    }


    private void Situation21_1_1()
    {

        Debug.Log("Situation21_1_1");

        SoundManager.Instance.PlayBGM(0);
        //SoundManager.Instance.PlaySFX(6);
        // 1-1_title01 (응급환자 구조)

        UIManager.Instance.HideUIAfterTimeCoroutine(0, 0, 3, () =>
        {
            //애니메이션 2회 반복 후 또타 등장  또타가 말 다하면 다음상황으로 진행
            // Na_2_1_TT_01.mp3

            //AnimationManager.Instance.totta.AnimationPlay
            SoundManager.Instance.PlayAudioCoroutine(1, 1, () =>
            {
                //1초미루기 
                SoundManager.Instance.PlayAudioCoroutine(1, 0, () =>
                {
                    SoundManager.Instance.PlayAudioCoroutine(1, 2, () =>
                    {
                        RpcNextSituation();
                    });
                });

            });

        });
    }

    private void Situation12_1_2()
    { 
        //Debug.Log(_playerObjList.First().Key, _playerObjList.First().Value);
        //Debug.Log(_playerObjList.Last().Key, _playerObjList.Last().Value);

        //환자 의식 확인하기
        Debug.Log("Situation12_1_2");
        UIManager.Instance.HideUIAfterTimeCoroutine(0, 2, 3, () =>
        {

            int playerIndex = runner.ActivePlayers.Count();
            Debug.Log("PlayerIndex : " + playerIndex);
           
           
            foreach(var activePlayer in runner.ActivePlayers)
            {
                int i = 0;
                Debug.Log("Count : " + i);
                i++;

                if (activePlayer.ToString() == "Player:1")
                {
                    Debug.Log("activePlayer.ToString()" + runner.GetPlayerObject(activePlayer).name);
                    // 현재 로컬 플레이어라면 위치 설정
                    runner.GetPlayerObject(activePlayer).transform.position = new Vector3(-0.41f, 0f, 0.94f);
                  
                }
                else
                {
                    //to do : Player2 이동하게 만들기

                    // 다른 플레이어의 경우
                    Debug.Log("activePlayer.ToString()" + runner.GetPlayerObject(activePlayer).name);
                    runner.GetPlayerObject(activePlayer).transform.position = new Vector3(0.41f, 0f, 0.94f);
                   
                }

            }

            //나중에 동기화
            UIManager.Instance.ActivePatientUI(0);
        });

    }



    private void Situation12_1_3_1()
    {

        Debug.Log("Situation12_1_3_1");
        //1-1_title03 (구조요청하기)
        UIManager.Instance.HideUIAfterTimeCoroutine(0, 5, 3, () =>
        {


            //환자가 의식이 없어요 어떡하죠?
            SoundManager.Instance.PlayAudioCoroutine(1, 4, () =>
            {
                if (CheckPlayer() == 2)
                {
                    //Player1이 Player2에게 Gage 충전
                    PlayerGageObj.SetActive(true);

                    UIManager.Instance.HideUIAfterTimeCoroutine(0, 6, 10);
                }

            });

        });
    }

    private void Situation12_1_3_2()
    {

        Debug.Log("Situation12_1_3_2");
        //열차 내 비상전화기로 이동

        //비상 전화기로 이동 버튼 생성
        //Player1//
        UIManager.Instance.Player2MoveUI.SetActive(true);

        //Player2//
        //CPR 시작
        UIManager.Instance.HideUIAfterTimeCoroutine(0, 7, 3, () =>
        {
            //또타등장
            TestTT.SetActive(true);
        });
    }


    private void Situation12_1_4()
    {
        Debug.Log("Situation12_1_4");
        //로딩 창
        UIManager.Instance.PlayerLodingUICanvas.SetActive(true);
    }

    private void Situation12_1_5()
    {

        Debug.Log("Situation12_1_5");
        //AED찾아오기
        UIManager.Instance.HideUIAfterTimeCoroutine(0, 8, 3, () =>
        {
            //guidePopup -> 심폐소생술 진행
            UIManager.Instance.Player2_guidePopup_02UI.SetActive(true);

            //aed 가져오기
            UIManager.Instance.Player1_MoveUI.SetActive(true);
        });
    }

    private void Situation12_1_6_1()
    {
        //순간이동
        if (runner.gameObject.name == "Player2")
        {
            UIManager.Instance.PlayerLodingUICanvas.SetActive(true);

        }
    }

    private void Situation12_1_6_2()
    {
        Debug.Log("Situation12_1_6_2");
        //순간이동
        if (runner.gameObject.name == "Player2")
        {
            runner.gameObject.transform.position = new Vector3(-0.41f, 0f, 0.94f);
        }

        //AED 사용하기
        UIManager.Instance.HideUIAfterTimeCoroutine(0, 9, 3, () =>
        {
            //Player1는 AED붙이기 Player2은 흉부압박술 
            UIManager.Instance.HideUIAfterTimeCoroutine(0, 10, 3, () =>
            {
                SoundManager.Instance.PlayNa(5);

                UIManager.Instance.ActivePatientUI(3);
            });
        });

    }

    private void Situation12_1_7()
    {

        Debug.Log("Situation12_1_7");
        //Player2에게 나타남
        //흉부 압박을 멈추고 물러나세요
        UIManager.Instance.HideUIAfterTimeCoroutine(0, 11, 3, () =>
        {
            RpcNextSituation();
        });
    }

    private void Situation12_1_8()
    {

        Debug.Log("Situation12_1_8");
        //심전도 분석
        //Player2가 물릴 수 있도록 안내 팝업
        UIManager.Instance.HideUIAfterTimeCoroutine(0, 12, 5, () =>
        {

            SoundManager.Instance.PlayAudioCoroutine(2, 1, () =>
            {
                SoundManager.Instance.PlayAudioCoroutine(2, 1, () =>
                {
                    //전기충격이 필요합니다.
                });
            });

            //심장 충격 시작
            //Player1이 물릴 수 있도록 안내 팝업 생성
            UIManager.Instance.HideUIAfterTimeCoroutine(0, 13, 5, () =>
            {
                //제새동 슈팅 소리
            });
        });
    }

    private void Situation12_1_9()
    {
        Debug.Log("Situation12_1_9");
        //환자 상태 확인하기
        UIManager.Instance.HideUIAfterTimeCoroutine(0, 14, 3, () =>
        {
            UIManager.Instance.HideUIAfterTimeCoroutine(0, 15, 3, () =>
            {
                //Player1 흉부압박 실행 
                //Plyer2 aed다시 설치
            });
        });
    }

    private void Situation12_1_9_1()
    {
        Debug.Log("Situation12_1_9_1");
        //Player1이 심폐소생술하고 Player2가 AED 재설치

        //Player2만 나오는 초기화UI 보인 후 AED사용나옴
        UIManager.Instance.HideUIAfterTimeCoroutine(0, 16, 3, () =>
        {
            UIManager.Instance.ActivePatientUI(4);

        });
    }

    private void Situation12_1_9_2()
    {
        Debug.Log("Situation12_1_9_2");
        //Player1에게만 보이는 흉부압박 멈추고 자리에서 물러나세요

    }

    private void Situation12_1_9_3()
    {
        Debug.Log("Situation12_1_9_3");
        UIManager.Instance.HideUIAfterTimeCoroutine(0, 12, 5, () =>
        {
            //Player1이 물릴 수 있도록 안내 팝업 생성
            SoundManager.Instance.PlayAudioCoroutine(2, 1, () =>
            {
                SoundManager.Instance.PlayAudioCoroutine(2, 1, () =>
                {
                    //전기충격이 필요합니다.
                });
            });

            //심장 충격 시작
            //Player1이 물릴 수 있도록 안내 팝업 생성
            UIManager.Instance.HideUIAfterTimeCoroutine(0, 13, 5, () =>
            {
                //제새동 슈팅 소리
            });
        });
    }


    private void Situation12_1_10()
    {
        Debug.Log("Situation12_1_10");
        //상황 종료
        UIManager.Instance.HideUIAfterTimeCoroutine(0, 16, 3, () =>
        {
            SoundManager.Instance.PlaySFX(2);

            UIManager.Instance.HideUIAfterTimeCoroutine(0, 14, 4, () =>
            {
                SoundManager.Instance.StopSfx();
                //또타 등장
                //또타 대사 끝나면 배경음악 볼륨 커지고 종료 카운트 다운 3-2-1
                SoundManager.Instance.PlayNa(12);
                SoundManager.Instance.PlayAudioCoroutine(0, 1, () =>
                {
                    RpcNextSituation();
                });
               
            });
        });
    }
    #endregion

 
   [Rpc(RpcSources.All, RpcTargets.All)]
    public void RpcNextSituation()
    {
        Debug.Log("RpcNextSituation");
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
    
    private int CheckPlayer()
    {
        int playerIndex = runner.ActivePlayers.Count();
        Debug.Log("playerIndex : " + playerIndex);

        if (playerIndex == 1)
            return 1;
        else if (playerIndex == 2)
            return 2;
        else
            return 0;

    }

    private void ReturnToLobby()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("StartScene");
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("OnPlayerJoined");
        InitializeNetwork();
        SetupEventQueue();

        int playerIndex = runner.ActivePlayers.Count();  // 1 또는 2

       //Join한 Player가 2명일 경우 시작
        if (playerIndex == 2)
        {
            RpcNextSituation();
        }
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

    private IEnumerator DelayedRpcCall(NetworkGameManager gameManager)
    {
        yield return new WaitForSeconds(1f); // 적절한 지연 시간 설정

        Debug.Log("gameManager.Object : " + gameManager.Object);
        Debug.Log("gameManager.Object.IsValid: " + gameManager.Object.IsValid);
        if (gameManager.Object != null && gameManager.Object.IsValid)
        {
            Debug.LogWarning("NetworkObject is valid");
            gameManager.RpcNextSituation();
        }
        else
        {
            Debug.LogWarning("NetworkObject is still not valid after delay.");
        }
    }

    public static implicit operator NetworkGameManager(NetworkGameManagerTest v)
    {
        throw new NotImplementedException();
    }
}
