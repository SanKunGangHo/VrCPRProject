using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 2024-10-17
// Object 까지 수정가능하게 스크립트 변경

public class UIBlink : MonoBehaviour
{
    private Image image;
    private Material objectMaterial; 
    private Color imgcolor;
    private float blinkTime = 1;
    private float minAlpha = 0.3f;
    private float maxAlpha = 1f;
    private bool isUI = true;
    // Start is called before the first frame update
    void Start()
    {
        // UI인 경우 (image 존재하는 경우)
        if (gameObject.GetComponent<Image>() != null)
        {
            image = gameObject.GetComponent<Image>();
            imgcolor = image.color;
            isUI = true;
        }
        // Object인 경우 (MeshRenderer가 존재하는 경우)
        else if (gameObject.GetComponent<MeshRenderer>() != null)
        {
            objectMaterial = gameObject.GetComponent<MeshRenderer>().material;
            imgcolor = objectMaterial.color;
            isUI = false;
        }

        // Blink Coroutine 시작
        StartCoroutine(Blink());
    }

    // Update is called once per frame
    IEnumerator Blink()
    {
        while (true)
        {
            // 최소 알파에서 최대 알파로 페이드
            yield return StartCoroutine(Fade(minAlpha, maxAlpha, blinkTime));

            // 최대 알파에서 최소 알파로 페이드
            yield return StartCoroutine(Fade(maxAlpha, minAlpha, blinkTime));
        }
    }

    IEnumerator Fade(float _startAlpha, float _endAlpha, float _blinkTime)
    {
        float elapsedTime = 0f;
        Color color = imgcolor;

        while (elapsedTime < _blinkTime)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(_startAlpha, _endAlpha, elapsedTime / _blinkTime);

            if (isUI)
            {
                // SpriteRenderer가 있을 경우
                image.color = color;
            }
            else
            {
                // MeshRenderer가 있을 경우
                objectMaterial.color = color;
            }

            yield return null;
        }

        color.a = _endAlpha;
        imgcolor = color;

        if (isUI)
        {
            image.color = imgcolor;
        }
        else
        {
            objectMaterial.color = imgcolor;
        }
    }

    public void MtrlInActive()
    {
        if (gameObject.GetComponent<MeshRenderer>().materials[0] != null)
        {
            gameObject.GetComponent<MeshRenderer>().materials[0].color = new Color(0, 0, 0, 0);
        }
    }
}
