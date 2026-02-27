using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManPartsManager : MonoBehaviour
{
    public GameObject[] parts;

    public void PartsDisable()
    {
        Debug.Log("PartsDisable");
        DisableParts();    
    }

    private void DisableParts()
    {
        Debug.Log("DisableParts");
        foreach (var part in parts)
        {
            part.SetActive(false);
        }
        Debug.Log("DisableParts_end");
    }
}
