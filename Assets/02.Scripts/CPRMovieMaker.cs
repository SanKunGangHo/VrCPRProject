using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CPRMovieMaker : MonoBehaviour
{
    public Image cut;
    public Sprite[] cutSprites;
    
    IEnumerator Start()
    {
        cut = GetComponent<Image>();
        int index = 0;
        while (SoundManager.Instance.naSource.isPlaying)
        {
            cut.sprite = cutSprites[index];
            index++;
            yield return new WaitForSeconds(5f);
        }
    }
}
