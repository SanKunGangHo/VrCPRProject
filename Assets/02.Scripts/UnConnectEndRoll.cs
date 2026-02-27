using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnConnectEndRoll : MonoBehaviour
{
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "StartScene")
        {
            Destroy(gameObject);
        }
    }
}
