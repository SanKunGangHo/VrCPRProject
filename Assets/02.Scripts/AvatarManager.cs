using Oculus.Movement.AnimationRigging;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.SceneManagement;

public class AvatarManager : Singleton<AvatarManager>
{
    public bool isMan;
    
    public GameObject avatarM;
    public GameObject avatarF;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name.Contains("Multi"))
        {
            Destroy(gameObject);
            return;
        }
        SceneManager.sceneLoaded += SummonPlayer;
    }

    private void SummonPlayer(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("SummonPlayer");
        if (SceneManager.GetActiveScene().name.Contains("Single"))
        {
            Transform chara = Instantiate(ChooseAvatar()).transform;
            chara.GetChild(0).GetComponent<RetargetingLayer>().enabled = true;
            chara.GetChild(0).GetComponent<RigBuilder>().enabled = true;
            chara.SetParent(GameManager.Instance.PlayeObjs[1].transform);
            chara.transform.position = GameManager.Instance.PlayeObjs[1].transform.position;
        }
    }

    private GameObject ChooseAvatar()
    {
        if (isMan)
        {
            return avatarM;
        }
        else
        {
            return avatarF;
        }
    }
    
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SummonPlayer;
    }
}
