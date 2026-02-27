using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerUI : NetworkBehaviour
{
    private NetworkRunner runner;

    //private GameObject 
    // Start is called before the first frame update
    void Start()
    {
         runner = NetworkManager1.RunnerInstance;
    }

    // Update is called once per frame
    void Update()
    {
        //FollowPlayer();
    }

    private void FollowPlayer()
    {
        

       
        if (!runner.GetPlayerObject(runner.LocalPlayer).GetComponent<PlayerData>().isCPR)
        {

            Debug.LogWarning(runner.GetPlayerObject(runner.LocalPlayer).transform.GetChild(1).GetChild(0));
            Debug.LogWarning(runner.GetPlayerObject(runner.LocalPlayer).transform.GetChild(1).GetChild(0).transform.position);
            gameObject.transform.position = runner.GetPlayerObject(runner.LocalPlayer).transform.GetChild(1).GetChild(0).transform.position;
        }
    

    }
}
