using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookAt : MonoBehaviour
{
    private Transform player;
    void Start()
    {
         player = FindObjectOfType<OVRCameraRig>().transform;
    }
    
    void Update()
    {
        gameObject.transform.LookAt(player);
    }
}
