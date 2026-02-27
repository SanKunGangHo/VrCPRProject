using EnumTypes;
using Fusion;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SessionListItem : MonoBehaviour
{
    public TMP_Text RoomName, PlayerCount;
    public Button JoinButton;
    public string RoomName_True;
    private ButtonClick bc;
    private NetworkManager1 netManager;

    private void Start()
    {
        netManager = NetworkManager1.RunnerInstance.GetComponent<NetworkManager1>();
    }

    public void JoinRoom()
    {
        netManager.JoinRoom(RoomName_True);
    }

    
}
