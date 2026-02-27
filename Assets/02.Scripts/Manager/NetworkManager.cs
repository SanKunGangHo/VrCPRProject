using EnumTypes;
using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
{
    public static NetworkRunner RunnerInstance;
    public static bool IsConnected = false;

    [Header("Option")]
    public int MaxRoomCount;
    public GameObject PlayerPrefab;

    [Header("Session")]
    public Transform SessionListContentParent;
    public GameObject SessionListItemPrefab;
    public Dictionary<string, GameObject> SessionListDictionary = new Dictionary<string, GameObject>();

    [Header("Scene")]
    public EnumTypes.SceneName LobbyScene;

    private EnumTypes.SceneName _gamePlayScene;
    private int _sessionCount = 0;

    private void Awake()
    {
        RunnerInstance = gameObject.GetComponent<NetworkRunner>();

        if (RunnerInstance == null)
        {
            RunnerInstance = gameObject.AddComponent<NetworkRunner>();
        }
    }

    private void Start()
    {
       // RunnerInstance.JoinSessionLobby(SessionLobby.Shared,"default");
    }

    public static void JoinLobby()
    {
        if (RunnerInstance.State != NetworkRunner.States.Running)
        {
            Debug.Log("JoinLobby");
            RunnerInstance.JoinSessionLobby(SessionLobby.Shared, "StartScene_test"); // fix me: Lobby Name
        }
    }

    public static void ReturnToLobby()
    {
        RunnerInstance.Despawn(RunnerInstance.GetPlayerObject(RunnerInstance.LocalPlayer));
        RunnerInstance.Shutdown(true, ShutdownReason.Ok);
    }

    public void CreateRoom()
    {
        if (_sessionCount <= MaxRoomCount)
        {
            string roomSessionName = DateTime.Now.ToString("yyyy-MM-dd") + "_" + (_sessionCount + 1).ToString();

            if (SelectManager.SelectPlace == EnumTypes.SelectPlace.Station)
            {
                _gamePlayScene = EnumTypes.SceneName.StationMultiScene;
            }
            else
            {
                _gamePlayScene = EnumTypes.SceneName.SubwayMultiScene;
            }

            RunnerInstance.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.Shared,
                Scene = SceneRef.FromIndex(GetSceneIndex(_gamePlayScene.ToString())),
                SessionName = roomSessionName,
            });
        }
        else
        {
            Debug.Log("방이 초과됐습니다."); // delete
            // fix me: Pop up 띄우기 
        }
    }

    public int GetSceneIndex(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; ++i)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string name = System.IO.Path.GetFileNameWithoutExtension(scenePath);

            if (name == sceneName)
            {
                return i;
            }
        }

        return -1;
    }

    private void DeleteOldSessionsFromUI(List<SessionInfo> sessionList)
    {
        GameObject uiToDelete = null;

        var copySessionList = new Dictionary<string, GameObject>(SessionListDictionary);

        foreach (KeyValuePair<string, GameObject> kvp in copySessionList)
        {
            bool isContained = false;
            string sessionKey = kvp.Key;

            foreach (SessionInfo session in sessionList)
            {
                if (session.Name == sessionKey)
                {
                    isContained = true;
                    break;
                }
            }

            if (!isContained)
            {
                uiToDelete = kvp.Value;
                SessionListDictionary.Remove(sessionKey);
                Destroy(uiToDelete);
            }
        }
    }

    private void CompareLists(List<SessionInfo> sessionList)
    {
        foreach (SessionInfo session in sessionList)
        {
            if (SessionListDictionary.ContainsKey(session.Name))
            {
                UpdateItemUI(session);
            }
            else
            {
                CreateItemUI(session);
            }
        }
    }

    private void UpdateItemUI(SessionInfo session)
    {
        SessionListDictionary.TryGetValue(session.Name, out GameObject newItem);
        SessionListItem itemScript = newItem.GetComponent<SessionListItem>();

        itemScript.RoomName.text = session.Name;
        if (session.PlayerCount <= 1)
        {
            itemScript.PlayerCount.color = Color.green;
        }
        else
        {
            itemScript.PlayerCount.color = Color.red;
        }
        itemScript.PlayerCount.name = session.PlayerCount.ToString();
        itemScript.PlayerCount.text = "( " + session.PlayerCount.ToString() + " / 2 )";

        newItem.SetActive(session.IsVisible);
    }

    private void CreateItemUI(SessionInfo session)
    {
        GameObject newItem = GameObject.Instantiate(SessionListItemPrefab);
        newItem.transform.SetParent(SessionListContentParent, false);
        SessionListItem itemScript = newItem.GetComponent<SessionListItem>();
        SessionListDictionary.Add(session.Name, newItem);

        itemScript.RoomName.text = session.Name;
        if (session.PlayerCount <= 1)
        {
            itemScript.PlayerCount.color = Color.green;
        }
        else
        {
            itemScript.PlayerCount.color = Color.red;
        }
        itemScript.PlayerCount.name = session.PlayerCount.ToString();
        itemScript.PlayerCount.text = "( " + session.PlayerCount.ToString() + " / 2 )";
        //itemScript.JoinButton.interactable = session.IsOpen;

        newItem.SetActive(session.IsVisible);
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        Debug.Log("Seesion List Update");
        _sessionCount = sessionList.Count;
        Debug.Log("Session Count:" + _sessionCount);
        DeleteOldSessionsFromUI(sessionList);

        CompareLists(sessionList);

        IsConnected = true;
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.ActivePlayers.Count() > 2)
        {
            Debug.Log("꽉참");
            // TODO: Pop-up 띄우기
            if (runner.LocalPlayer == player)
            {
                runner.Shutdown();
            }
            return;
        }

        if (player == RunnerInstance.LocalPlayer)
        {
            List<GameObject> spawnPoints = GameManager.Instance.SpawnPoints;
            Vector3 spawnPosition = new Vector3(player.RawEncoded % runner.Config.Simulation.PlayerCount, 1, 0);

            // 플레이어 네트워크 오브젝트 생성 및 이름 설정
            NetworkObject playerNetworkObject = runner.Spawn(PlayerPrefab, spawnPosition);
            playerNetworkObject.AssignInputAuthority(player);
            int playerIndex = runner.ActivePlayers.Count();  // 1 또는 2
            playerNetworkObject.name = "Player" + playerIndex;
           
            // Camera의 설정을 가져옴
            Camera camera = GameManager.Instance.PlayerTT.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Camera>();

            // Player 위치 및 Camera CullingMask 설정
            if (camera != null)
            {
                if (playerIndex == 1)
                {
                    GameManager.Instance.PlayerTT.transform.position = spawnPoints[1].transform.position;
                    camera.cullingMask &= ~(1 << 13);  // Player1 카메라에서 Player2 레이어 비활성화
                }
                else if (playerIndex == 2)
                {
                    GameManager.Instance.PlayerTT.transform.position = spawnPoints[0].transform.position;
                    camera.cullingMask &= ~(1 << 12);  // Player2 카메라에서 Player1 레이어 비활성화
                }
            }

            // 네트워크에 플레이어 오브젝트 설정
            runner.SetPlayerObject(player, playerNetworkObject);

            if(playerNetworkObject.HasInputAuthority)
            {
                Debug.Log("inputauthority");
            }
        }
    }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        SceneManager.LoadScene(LobbyScene.ToString());
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("Test");
    }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
}
