using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkPlayerWaitManager : MonoBehaviour
{
    [SerializeField] Image cut;
    [SerializeField] Image bar;
    [SerializeField] TMP_Text announce;
    [SerializeField] float waitTime = 180f;
    
    public Sprite[] cutSprites; 
    
    private NetworkRunner runner;
    
    void Start()
    {
        runner = NetworkManager1.RunnerInstance;
        StartCoroutine(ChangeCut());
        StartCoroutine(Wait());
    }

    IEnumerator ChangeCut()
    {
        int index = 0;
        while (runner.ActivePlayers.Count() < 2)
        {
            // Sprite를 현재 순서의 이미지로 변경
            cut.sprite = cutSprites[index];

            // 다음 Sprite로 이동하기 위해 인덱스 증가
            index = (index + 1) % cutSprites.Length;
            
            // 이미지 변경을 기다리는 시간
            yield return new WaitForSeconds(5f);
        }
    }

    IEnumerator Wait()
    {
        float waitedTime = 0f;
        while (waitedTime < waitTime)
        {
            waitedTime += Time.deltaTime;
            bar.fillAmount = waitedTime / waitTime;
            if (PlayerCheck()) break;
            if (waitedTime >= 120)
            {
                announce.text = (waitTime - waitedTime).ToString("00") + "초 후에 처음 화면으로 돌아갑니다.";
            }
            yield return null;
        }
        announce.text = "";
        StopCoroutine(ChangeCut());
        if (PlayerCheck())
        {
            gameObject.SetActive(false);
            yield break;
        }
        runner.Shutdown();//체크 필요
        SceneManager.LoadScene("StartScene");
    }

    bool PlayerCheck()
    {
        if (runner.ActivePlayers.Count() < 2)
        {
            return false;
        }
        else
        {
            announce.text = "다른 사람이 들어올 때까지 기다립니다. (2/2)";
            return true;
        }
    }

    
}
