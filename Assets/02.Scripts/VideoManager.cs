using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    private VideoPlayer _videoPlayer;
    public VideoClip playClip;
    public TMP_Text countText;
    public RawImage videoImage;
    private WaitForSeconds _wait = new WaitForSeconds(1f);
    
    // Start is called before the first frame update
    void Start()
    {
        _videoPlayer = gameObject.GetComponent<VideoPlayer>();
        _videoPlayer.clip = playClip;
        _videoPlayer.loopPointReached += OnVideoEnd;
        countText.gameObject.SetActive(false);
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        videoImage.color = new Color(0, 0, 0, 1);
        StartCoroutine(EndVideo());
    }

    IEnumerator EndVideo()
    {
        countText.gameObject.SetActive(true);
        int count = 3;
        while (true)
        {
            countText.text = count.ToString();
            if (count <= 0)
            {
                break;
            }
            count--;
            yield return _wait;
        }
        NextMove();
        yield return null;
    }

    void NextMove()
    {
        Debug.Log("NextMove");
        //TODO: Do Something
    }
}
