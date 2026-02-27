using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CallUIManager : MonoBehaviour
{
    public Image callImage;
    public List<Sprite> callingUI;

    public void CallUI(int i)
    {
        callImage.sprite = callingUI[i];
    }
}
