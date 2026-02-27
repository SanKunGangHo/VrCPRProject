using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExtraUIManager_Station : MonoBehaviour
{
    public List<Image> MultiUIs_1;
    public List<Image> MultiUIs_2;
    
    public List<GameObject> AED1;

    public void AED_Delete()
    {
        foreach (var parts in AED1)
        {
            if(parts.TryGetComponent(out NetworkObjectController part))
            {
                part.RPC_InActive();
            }
            else
            {
                parts.SetActive(false);
            }
            //parts.SetActive(false)
        }
    }
}
