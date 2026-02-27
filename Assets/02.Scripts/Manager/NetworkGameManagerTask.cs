// using Fusion;
// using Fusion.Sockets;
// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using Unity.VisualScripting;
// using UnityEngine;
// using UnityEngine.SceneManagement;
//
//
// public class NetworkGameManagerTask : NetworkBehaviour, INetworkRunnerCallbacks
// {
//
//
//     private Queue<System.Action> eventQueue = new Queue<System.Action>();
//     public NetworkRunner runner;
//
//     public GameObject totta;
//     
//     public GameObject TestMovie;
//
//
//     public NetworkLinkedList<NetworkObject> test;
//
//
//     public NetworkLinkedList<NetworkObject> Test { get; private set; }
//
//     [Header("Player Settings")]
//     public SI si;
//
//     public void SetupEventQueue()
//     {
//         Debug.Log("SetupEventQueue");
//       
//         eventQueue.Enqueue(Situation1);
//         eventQueue.Enqueue(Situation2);
//         eventQueue.Enqueue(Situation3);
//         eventQueue.Enqueue(Situation4);
//         eventQueue.Enqueue(Situation5);
//
//     }
//
//     private void Start()
//     {
//        NetworkManager1.RunnerInstance.GetComponent<NetworkManager1>().ngTask = this;
//     }
//
//
//     public void InitializeNetwork()
//     {
//         runner = NetworkManager1.RunnerInstance;
//         if (runner != null)
//         {
//             runner.AddCallbacks(this);
//             Debug.Log("Callbacks added to runner");
//         }
//         else
//         {
//             Debug.LogError("Runner is null in InitializeNetwork");
//         }
//
//     }
//
//
//     #region Situation_subway 
//
//
//     private void StartSituation()
//     {
//      
//         RpcNextSituation();
//        
//     }
//
//
//
//     private void Situation1()
//     {
//         Debug.LogWarning("Situation1");
//         //Player 이동 
//         if (runner.GetPlayerObject(runner.LocalPlayer).GetComponent<PlayerData>().isCPR)
//         {
//             runner.GetPlayerObject(runner.LocalPlayer).transform.position = new Vector3(-0.513f, 0f, -0.2f);
//         }          
//         else
//         {
//             runner.GetPlayerObject(runner.LocalPlayer).transform.position = new Vector3(0.513f, 0f, -0.2f);
//         }
//
//
//         Situation2();
//
//     }
//
//
//     private void Situation2()
//     {
//         Debug.LogWarning("Situation2");
//
//         totta.SetActive(true);
//
//         
//         SoundManager.Instance.PlayAudioCoroutine(1, 1, () =>
//         {
//             totta.SetActive(false);
//             Situation3();
//         });
//     }
//
//     private void Situation3()
//     {
//         Debug.LogWarning("Situation3");
//
//         //환자의식 확인을 위해 10초간 바라본다
//         UIManager.Instance.HideUIAfterTimeCoroutine(0, 0, 10, () =>
//         {
//             Situation4();
//         });
//
//     }
//
//     private void Situation4()
//     {
//         Debug.LogWarning("Situation4");
//
//         //구조 요청하기
//         UIManager.Instance.HideUIAfterTimeCoroutine(0, 1, 3, () =>
//         {
//             //거기 안경쓰고 흰티 입으신 여자분
//             UIManager.Instance.HideUIAfterTimeCoroutine(0, 2, 10, () =>
//             {
//                 //여캐에 Gage 생성
//                 if (runner.GetPlayerObject(runner.LocalPlayer).GetComponent<PlayerData>().isCPR)
//                 {
//                     runner.GetPlayerObject(runner.LocalPlayer).transform.GetChild(0).gameObject.SetActive(true);
//                 }
//             });
//         });
//     }
//
//
//     private void Situation5()
//     {
//         Debug.LogWarning("Situation5");
//
//         //이동 버튼 활성화 + 가슴 압박 소생술 실시 
//
//
//         UIManager.Instance.HideUIAfterTimeCoroutine(0, 3, 3, () =>
//         {
//             //또타 애니메이션 생성 + 영상종료누르면 cpr 생성
//             UIManager.Instance.ActivePatientUI(2);
//
//
//         });
//     }
//
//
//     #endregion
//
//     private NetworkObject Player1()
//     {
//         if (runner.GetPlayerObject(runner.LocalPlayer).GetComponent<PlayerData>().isCPR)
//             return runner.GetPlayerObject(runner.LocalPlayer);
//         else
//             return null;
//     }
//
//     private NetworkObject Player2()
//     {
//         if (!runner.GetPlayerObject(runner.LocalPlayer).GetComponent<PlayerData>().isCPR)
//             return runner.GetPlayerObject(runner.LocalPlayer);
//         else
//             return null;
//     }
//
//
//     [Rpc(RpcSources.All, RpcTargets.All)]
//     public void RpcNextSituation()
//     {
//         Debug.LogWarning("RpcNextSituation");
//         if (eventQueue.Count > 0)
//         {
//             System.Action currentEvent = eventQueue.Dequeue();
//             currentEvent.Invoke();  // 현재 상황 실행
//         }
//         else
//         {
//             Debug.Log("모든 상황 완료!");
//         }
//     }
//
//
//     private void ReturnToLobby()
//     {
//         UnityEngine.SceneManagement.SceneManager.LoadScene("StartScene");
//     }
//
//
//     public void OnPlayerJoined2(NetworkRunner runner, PlayerRef player)
//     {
//         Debug.Log("OnPlayerJoined");
//         InitializeNetwork();
//         SetupEventQueue();
//
//
//         int playerIndex = runner.ActivePlayers.Count();  // 1 또는 2
//
//         //Join한 Player가 2명일 경우 시작
//         if (playerIndex == 2)
//         {
//             Debug.Log("DelayedRpcCall");
//             StartCoroutine(DelayedRpcCall());
//         }
//     }
//
//
//
//     // runner가 Player를 저장하기 위한 시간 딜레이
//     private IEnumerator DelayedRpcCall()
//     {
//         yield return new WaitForSeconds(2f); 
//         RpcNextSituation();
//     }
//
//
//     #region RPC
//
//
//     public static implicit operator NetworkGameManagerTask(NetworkGameManager v)
//     {
//         throw new NotImplementedException();
//     }
//
//     public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
//     {
//     }
//
//     public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
//     {
//     }
//
//
//     public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
//     {
//
//     }
//     public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
//     {
//     }
//
//     public void OnInput(NetworkRunner runner, NetworkInput input)
//     {
//     }
//
//     public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
//     {
//     }
//
//     public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
//     {
//     }
//
//     public void OnConnectedToServer(NetworkRunner runner)
//     {
//     }
//
//     public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
//     {
//     }
//
//     public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
//     {
//     }
//
//     public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
//     {
//     }
//
//     public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
//     {
//     }
//
//     public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
//     {
//     }
//
//     public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
//     {
//     }
//
//     public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
//     {
//     }
//
//     public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
//     {
//     }
//
//     public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
//     {
//     }
//
//     public void OnSceneLoadDone(NetworkRunner runner)
//     {
//     }
//
//     public void OnSceneLoadStart(NetworkRunner runner)
//     {
//     }
//     #endregion
//
//
// }
