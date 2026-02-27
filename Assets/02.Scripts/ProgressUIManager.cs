using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressUIManager : MonoBehaviour
{
    public Image progressImage; 
    public List<Sprite> progressSprites;
    
    public void ProgressUpdate(int i)
    {
        progressImage.sprite = progressSprites[i];
    }
}
