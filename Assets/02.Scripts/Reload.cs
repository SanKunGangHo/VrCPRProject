using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reload : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager gamemanager = GameObject.FindObjectOfType<GameManager>();
        if (gamemanager != null)
        {
            Destroy(gamemanager.gameObject); // 기존 Manager 파괴
        }
        
        UIManager uimanager = GameObject.FindObjectOfType<UIManager>();
        if (uimanager != null)
        {
            Destroy(uimanager.gameObject); // 기존 Manager 파괴
        }
        
        SoundManager soundmanager = GameObject.FindObjectOfType<SoundManager>();
        if (soundmanager != null)
        {
            Destroy(soundmanager.gameObject); // 기존 Manager 파괴
        }

        AnimationManager animationManager = GameObject.FindObjectOfType<AnimationManager>();
        if (animationManager != null)
        {
            Destroy(animationManager.gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
