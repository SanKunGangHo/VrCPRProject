using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeManager : Singleton<FadeManager>
{
    [SerializeField]
    private Image myImage;
    [SerializeField]
    private float _fadeTime;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        StartCoroutine(SceneChecker());
        myImage.gameObject.SetActive(true);
        //myImage.gameObject.SetActive(false);
        

    }

    IEnumerator SceneChecker()
    {
        while (true)
        {
            if (SceneManager.GetActiveScene().name.Contains("Multi"))
            {
                myImage.gameObject.SetActive(false);
                Destroy(this.gameObject);
            }
            else if (SceneManager.GetActiveScene().name.Contains("Single"))
            {
                myImage.gameObject.SetActive(false);
                yield break; 
            }

            yield return null;
        }
    }
    
    public void StartFadeIn()
    {
        StartCoroutine(FadeIn(_fadeTime));
    }

    public void StartFadeOut()
    {
        StartCoroutine(FadeOut(_fadeTime));
    }

    public void StartNetworkFadeIn()
    {
        StartCoroutine(NetworkFadeIn(_fadeTime));
    }

    public void StartNetworkFadeOut()
    {
        StartCoroutine(NetworkFadeOut(_fadeTime));
    }

    private IEnumerator FadeIn(float fadeOutTime, System.Action nextEvent = null)
    {
        myImage.gameObject.SetActive(true);

        Color tempColor = myImage.color;
        tempColor.a = 0f;

        while (tempColor.a < 1f)
        {
            tempColor.a += Time.deltaTime / fadeOutTime;
            myImage.color = tempColor;

            if (tempColor.a >= 1f)
            {
                tempColor.a = 1f;
                myImage.gameObject.SetActive(false);
            }

            yield return null;
        }

        myImage.color = tempColor;

        if (nextEvent != null)
        {
            nextEvent();
        }
    }

    private IEnumerator NetworkFadeIn(float fadeOutTime, System.Action nextEvent = null)
    {
        myImage.gameObject.SetActive(true);

        Color tempColor = myImage.color;
        tempColor.a = 0f;

        while (tempColor.a < 1f)
        {
            tempColor.a += Time.deltaTime / fadeOutTime;
            myImage.color = tempColor;

            if (tempColor.a >= 1f && NetworkManager.IsConnected)
            {
                tempColor.a = 1f;
                myImage.gameObject.SetActive(false);
            }

            yield return null;
        }

        myImage.color = tempColor;

        if (nextEvent != null)
        {
            nextEvent();
        }
    }

    private IEnumerator FadeOut(float fadeOutTime, System.Action nextEvent = null)
    {
        myImage.gameObject.SetActive(true);
        Color tempColor = myImage.color;
        tempColor.a = 1f;

        while (tempColor.a > 0f)
        {
            tempColor.a -= Time.deltaTime / fadeOutTime;
            myImage.color = tempColor;

            if (tempColor.a <= 0f)
            {
                tempColor.a = 0f;
                myImage.gameObject.SetActive(false);
            }

            yield return null;
        }

        myImage.color = tempColor;

        if (nextEvent != null)
        {
            nextEvent();
        }
    }

    private IEnumerator NetworkFadeOut(float fadeOutTime, System.Action nextEvent = null)
    {
        myImage.gameObject.SetActive(true);
        Color tempColor = myImage.color;
        tempColor.a = 1f;

        while (tempColor.a > 0f)
        {
            if (NetworkManager.IsConnected)
            {
                tempColor.a -= Time.deltaTime / fadeOutTime;
            }

            myImage.color = tempColor;

            if (tempColor.a <= 0f)
            {
                tempColor.a = 0f;
                myImage.gameObject.SetActive(false);
            }

            yield return null;
        }

        myImage.color = tempColor;

        if (nextEvent != null)
        {
            nextEvent();
        }
    }
}
