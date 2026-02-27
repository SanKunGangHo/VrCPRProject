using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataReader : MonoBehaviour
{
    public PlayerData nowPlayerData;

    private void OnEnable()
    {
        PlayerData_Init();
    }

    public void PlayerData_Init()
    {
        nowPlayerData = NetworkManager1.RunnerInstance.GetPlayerObject(NetworkManager1.RunnerInstance.LocalPlayer).GetComponent<PlayerData>();
        Debug.Log(nowPlayerData.gameObject.name);
        PlayerData_Update();
    }

    private void PlayerData_Update()
    {
        if (nowPlayerData.isCPR)
        {
            gameObject.SetActive(false);
        }
    }
}
