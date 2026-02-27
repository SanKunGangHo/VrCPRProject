using _02.Scripts;
using _02.Scripts.Manager;
using EnumTypes;
using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.XR.Interaction;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class NetworkManager1 : MonoBehaviour, INetworkRunnerCallbacks
{
    public static NetworkRunner RunnerInstance;
    public static bool IsConnected = false;

    [Header("Option")]
    public int MaxRoomCount;
    public GameObject PlayerPrefab;
    
    [Header("StartScene")]
    public SoundManager_ForStart forStart;
    public GameObject _allInOnePanel;
    public GameObject totta;
    public Transform tottaTransform;
    public CircleTimer countPanel;
    public GameObject loadingPanel;
    
    [Header("Session")]
    public Transform SessionListContentParentSubway;
    public Transform SessionListContentParentStation; 
    public GameObject SessionListItemPrefab;
    public Dictionary<string, GameObject> SessionListDictionary = new Dictionary<string, GameObject>();

    [Header("Scene")]
    public EnumTypes.SceneName LobbyScene;

    private string roomName_private;
    private EnumTypes.SceneName _gamePlayScene;
    private int _sessionCount = 0;
    private int nextSceneCount = 0;
    
    private Color cyanColor = new Color(0.2f, 1f, 1f, 1f);
    private Color magentaColor = new Color(1f, 0.2941177f, 0.7921569f, 1f);
    
    public string gender;

    [Header("또타 애니메이터")]
    public Animator tottaAnim;
    
    [Header("Character")] 
    public GameObject genderUI;
    public GameObject manChar;
    public GameObject manCharAlter;
    public GameObject womanChar;
    public GameObject womanCharAlter;

    public bool isAlter = false;
    public  bool isHost = false;
    
    [Header("NetworkGameManager")]
    public NetworkGameManagerTest ngManager;
    public NetworkGameManagerTest_Station ngManager_Station;

    [Header("FloatManu")] 
    public GameObject floatingMenu;
    public bool isSubway = false;

    public GameObject tottaSkip;
    public bool isTottaSkip;

    public void TottaSkip()
    {
        isTottaSkip = true;
    }

    private void Awake()
    {
        RunnerInstance = gameObject.GetComponent<NetworkRunner>();

        if (RunnerInstance == null)
        {
            RunnerInstance = gameObject.AddComponent<NetworkRunner>();
        }

        JoinLobby();
        FadeManager.Instance.StartFadeOut();
        
        
    }

    public static void JoinLobby()
    {
        if (RunnerInstance.State != NetworkRunner.States.Running)
        {
            RunnerInstance.JoinSessionLobby(SessionLobby.Shared, "Default"); // fix me: Lobby Name
        }
    }

    public static void ReturnToLobby()
    {
        RunnerInstance.Despawn(RunnerInstance.GetPlayerObject(RunnerInstance.LocalPlayer));
        RunnerInstance.Shutdown(true, ShutdownReason.Ok);
    }

    //TODO: 수정
    public void CreateRoom()
    {
        isHost = true;
        if (_sessionCount <= MaxRoomCount)
        {
            string roomSessionName = " ";

            if (SelectManager.SelectPlace == EnumTypes.SelectPlace.Station)
            {
                roomSessionName = DateTime.Now.ToString("yyyy-MM-dd") + (_sessionCount+1);
                _gamePlayScene = EnumTypes.SceneName.StationMultiScene;
            }
            else
            {
                roomSessionName = DateTime.Now.ToString("yyyy-MM-dd") + (_sessionCount+1);
                _gamePlayScene = EnumTypes.SceneName.SubwayMultiScene;
            }

            RunnerInstance.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.Shared,
                Scene = SceneRef.FromIndex(GetSceneIndex(_gamePlayScene.ToString())),
                SessionName = roomSessionName
            });
        }
        else
        {
            Debug.Log("방이 초과됐습니다."); // delete
            // fix me: Pop up 띄우기 
        }
    }

    public void JoinRoom(string RoomName_True)
    {
        isHost = false;
        isAlter = true;
        SelectManager.SelectPeople = SelectPeople.Multi;
        //bc.OnButtonClick(true);
        
        _allInOnePanel.SetActive(false);
        genderUI.SetActive(true);
        
        genderUI.transform.GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
        genderUI.transform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
        
        genderUI.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(
            () => StartCoroutine(SelectGender(RoomName_True, true))
        );
        genderUI.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(
            () => StartCoroutine(SelectGender(RoomName_True, false))
        );
    }

    //dh
    public IEnumerator SelectGender(string RoomName_True, bool isMan)
    {
        
        yield return StartCoroutine(TottaAppear());
        
        if (isMan)
        {
            gender = "Male";
        }
        else
        {
            gender = "Female";
        }
        
        RunnerInstance.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = RoomName_True
        });
    }

    public void CreateRoom(string roomName)
    {
        forStart.PlaySFX(0);
        //소리
        //TODO: TottaAppear
        roomName_private = roomName;
        StartCoroutine(genderUICoroutine());
        //StartCoroutine(TottaAppear(roomName));
    }

    IEnumerator genderUICoroutine()
    {
        _allInOnePanel.SetActive(false);
        yield return new WaitForSeconds(1f);
        genderUI.SetActive(true);
    }

    public void GenderAppear()
    {
        forStart.PlaySFX(0);
        genderUI.gameObject.SetActive(false);
        
        StartCoroutine(TottaAppear(roomName_private));
        
        // switch(roomName_private)
        // {
        //     case "station":
        //         CreateRoom_Scenes(EnumTypes.SelectPlace.Station);
        //         break;
        //     case "subway":
        //         CreateRoom_Scenes(EnumTypes.SelectPlace.Subway);
        //         break;
        // }
    }

    public IEnumerator TottaAppear(string roomName = "")
    {
        _allInOnePanel.SetActive(false);
        tottaSkip.SetActive(true);
        Animator tottaAnim = Instantiate(totta, tottaTransform).GetComponent<Animator>();
        tottaAnim.SetBool("Ani_0_0_TT_01(Hello)", true);
        forStart.PlayNa(0);

        while (forStart.naSource.isPlaying)
        {
            if (isTottaSkip) break;
            yield return null;
        }

        tottaSkip.SetActive(false);
        forStart.naSource.Stop();
        tottaAnim.SetBool("Ani_0_0_TT_01(Hello)", false);
        tottaAnim.gameObject.SetActive(false);
        
        countPanel.gameObject.SetActive(true);
        while (countPanel._fillTime < countPanel.limitTime)
        {
            yield return null;
        }
        FadeManager.Instance.StartNetworkFadeOut();

        loadingPanel.SetActive(true);

        if (string.IsNullOrEmpty(roomName)) yield break;

        switch (roomName)
        {
            case "station":
                CreateRoom_Scenes(EnumTypes.SelectPlace.Station);
                break;
            case "subway":
                CreateRoom_Scenes(EnumTypes.SelectPlace.Subway);
                break;
        }
    }

    public void CreateRoom_Scenes(EnumTypes.SelectPlace scene)
    {
        isHost = true;
        if (_sessionCount <= MaxRoomCount)
        {
            string roomSessionName = " ";

            if (scene == EnumTypes.SelectPlace.Station)
            {
                roomSessionName = "Station_" + DateTime.Now.ToString("yyyy-MM-dd") + "_" + (_sessionCount+1);
                _gamePlayScene = EnumTypes.SceneName.StationMultiScene;
            }
            else
            {
                roomSessionName = "Subway_" + DateTime.Now.ToString("yyyy-MM-dd") + "_" + (_sessionCount+1);
                _gamePlayScene = EnumTypes.SceneName.SubwayMultiScene;
            }

            RunnerInstance.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.Shared,
                Scene = SceneRef.FromIndex(GetSceneIndex(_gamePlayScene.ToString())),
                SessionName = roomSessionName
            });
            
            //FadeManager.Instance.StartNetworkFadeIn();
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
            itemScript.PlayerCount.text = "입장하기";
            itemScript.PlayerCount.color = cyanColor;
            itemScript.JoinButton.onClick.RemoveAllListeners();
            itemScript.JoinButton.onClick.AddListener(()=> JoinRoom(itemScript.RoomName_True));
            itemScript.JoinButton.onClick.AddListener(
                () => forStart.PlaySFX(0));
        }
        else
        {
            itemScript.PlayerCount.text = "입장불가";
            itemScript.PlayerCount.color = magentaColor;
            itemScript.JoinButton.onClick.RemoveAllListeners();
            itemScript.JoinButton.onClick.AddListener(() => forStart.PlaySFX(1));
        }

        newItem.SetActive(session.IsVisible);
    }

    private void CreateItemUI(SessionInfo session)
    {
        GameObject newItem;
        
        if (session.Name.Contains("Station"))
        {
            newItem = Instantiate(SessionListItemPrefab, SessionListContentParentStation);
            newItem.GetComponent<SessionListItem>().RoomName_True = session.Name;
            newItem.GetComponent<SessionListItem>().RoomName.text = session.Name/*.Replace("Station", "")*/;
        }
        else
        {
            newItem = Instantiate(SessionListItemPrefab, SessionListContentParentSubway);
            newItem.GetComponent<SessionListItem>().RoomName_True = session.Name;
            newItem.GetComponent<SessionListItem>().RoomName.text = session.Name/*.Replace("Subway", "")*/;
        }
        
        SessionListItem itemScript = newItem.GetComponent<SessionListItem>();
        SessionListDictionary.Add(session.Name, newItem);

        
        if (session.PlayerCount <= 1)
        {
            itemScript.PlayerCount.text = "입장하기";
            itemScript.PlayerCount.color = cyanColor;
            itemScript.JoinButton.onClick.RemoveAllListeners();
            itemScript.JoinButton.onClick.AddListener(()=> JoinRoom(itemScript.RoomName_True));
            itemScript.JoinButton.onClick.AddListener(
                () => forStart.PlaySFX(0));
        }
        else
        {
            itemScript.PlayerCount.text = "입장불가";
            itemScript.PlayerCount.color = magentaColor;
            itemScript.JoinButton.onClick.RemoveAllListeners();
            itemScript.JoinButton.onClick.AddListener(
                () => forStart.PlaySFX(1));
        }
        
        //itemScript.JoinButton.interactable = session.IsOpen;

        newItem.SetActive(session.IsVisible);
    }
 
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        //if(!floatingMenu.activeSelf) return;
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
     
      
        PlayerPrefab = CheckGender();
        
        if (player == RunnerInstance.LocalPlayer)
        {
            Debug.Log("player == RunnerInstance.LocalPlayer");
            Debug.Log("runner.ActivePlayers.Count()" + runner.ActivePlayers.Count());

            NetworkObject playerNetworkObject = runner.Spawn(PlayerPrefab);

            // 플레이어 네트워크 오브젝트 생성 및 이름 설정
            //playerNetworkObject.transform.position = spawnPosition;
            playerNetworkObject.AssignInputAuthority(player);
            int playerIndex = runner.ActivePlayers.Count();  // 1 또는 2
            //playerNetworkObject.name = "Player" + playerIndex;
            playerNetworkObject.name = runner.LocalPlayer.ToString();

            playerNetworkObject.GetComponent<PlayerData>().isCPR = !isHost;

            if (playerNetworkObject.TryGetComponent<ManPartsManager>(out ManPartsManager manPartsManager))
            {
                Debug.Log("TryGetComponent<ManPartsManager>(out ManPartsManager manPartsManager)");
                manPartsManager.PartsDisable();
            }
            
            // 네트워크에 플레이어 오브젝트 설정
            runner.SetPlayerObject(player, playerNetworkObject);

            if (playerNetworkObject.HasInputAuthority)
            {
                Debug.Log("inputauthority");
            }

            if(ngManager == null)
            {
                ngManager = FindObjectOfType<NetworkGameManagerTest>();
            }
            //Debug.Log("ngManager : " + ngManager.name);

            if (ngManager_Station == null)
            {
                ngManager_Station = FindObjectOfType<NetworkGameManagerTest_Station>();
            }
            //Debug.Log("ngManager_Station : " + ngManager_Station.name);

            //NetworkGameManager gameManager = FindObjectOfType<NetworkGameManager>();
           // Debug.Log("NetworkGameManager : " + NetworkGameManager.Instance.name);
          // ngManager.myId = runner.UserId;
          // ngManager.PlayerObjList.Add(ngManager.myId, playerNetworkObject);
           Debug.Log("myId: " + runner.UserId);

            if(playerIndex == 2)
            {
                if (ngManager_Station != null)
                {
                    Debug.Log("station" + ngManager_Station.name);
                    ngManager_Station.RPC_OnPlayerJoined2();
                    return;
                }
                Debug.Log("ngManager_Station is null");
                ngManager.RPC_OnPlayerJoined2();
            }
         

        }
    }

    private GameObject CheckGender()
    {
        switch (gender)
        {
            case "Male":
                if (isAlter)
                {
                    return manCharAlter;
                }
                else
                {
                    return manChar;
                }
            case "Female":
                if (isAlter)
                {
                    return womanCharAlter;
                }
                else
                {
                    return womanChar;
                }
            default:
                return womanChar;
        }
    }

    public void GenderSetter(string genString)
    {
        gender = genString;
        
        if(SelectManager.SelectPeople == SelectPeople.Single) return;
        GenderAppear();
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        Debug.Log("ShutdownReason:" + shutdownReason);
        if (SceneManager.GetActiveScene().name.Contains("Multi"))
        {
            SceneManager.LoadScene(LobbyScene.ToString());
        }
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
