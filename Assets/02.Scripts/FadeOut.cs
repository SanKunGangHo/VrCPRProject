using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    private bool isFade;
    public Canvas canvas;
    private Color initialColor;
    private Color fadeoutColor;
    private Image fadeImage;  // 캐싱된 Image 컴포넌트
    public float fadeTime = 4f;
    public float waitTime = 2f;
    private float waitfadeTime = 0f;
    private float elapsedTime = 0.0f;

    void Start()
    {
        // Image 컴포넌트 캐싱
        fadeImage = canvas.transform.GetChild(0).GetComponent<Image>();
        initialColor = fadeImage.color;
        fadeoutColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0);
    }

    void Update()
    {
        if (isFade)
        {
            waitfadeTime += Time.deltaTime;

            if (waitfadeTime >= waitTime)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / fadeTime);

                // 페이드 아웃 적용
                fadeImage.color = new Color(initialColor.r, initialColor.g, initialColor.b, Mathf.Lerp(initialColor.a, fadeoutColor.a, t));

                if (t >= 1f)
                {
                    isFade = false;  // 페이드 아웃 종료
                    enabled = false;  // Update 비활성화하여 성능 최적화
                }
            }
        }
    }

    // 페이드 아웃을 시작하는 함수
    public void FadeOn()
    {
        isFade = true;
        enabled = true;  // Update 재활성화
    }
}