using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubwaySingleManager : MonoBehaviour
{
    public List<Sprite> titleSprites;

    public Image titleImage;

    public void ChangeTitle(int chap)
    {
        Debug.Log(titleSprites[chap].name);
        titleImage.sprite = titleSprites[chap];
    }
}
