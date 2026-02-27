using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterlockedHand_L : MonoBehaviour
{
    public InterlockedFinger headScript;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Hands")) headScript.leftHand = true;

        // if (other.CompareTag("Patient") && headScript)
        // {
        //     
        // }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Hands")) headScript.leftHand = false;
    }
}
