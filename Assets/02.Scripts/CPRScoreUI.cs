using Fusion;
using System;
using System.Collections;
using System.Net;
using System.Runtime.ConstrainedExecution;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CPRScoreUI : NetworkBehaviour
{
    public TMP_Text scoreText, timeText;
    public GameObject BPMImage;
  
    public float sixtySeconds = 60f;
    public float bpm = 100f;
    public PlayerData playerData;
    private float _countTime, _blinkInterval;
    public InterlockedFinger interlockedFinger;
    public GameManager gameManager;
    public GameObject cprUI;
    public bool isMulti = false;
    private NetworkRunner runner;

    private int cprnum = 0;

    public bool is911;
    public bool isStation = false;
    public bool isFirst = false;

    private bool rate;

    [Header("횟수")]
    public float maxcount = 100; 

    [Networked] private TickTimer countdownTimer { get; set; }
    
    [Networked] public bool isAED { get; set; }


    public void StartCPR()
    {
        if(!isMulti)
        {
            StartCoroutine(UpdateScoreUI());
            StartCoroutine(BlinkImage());
        }

        if (isMulti && !isFirst)
        {
            
        }

        //이거 왜 안올라가
    }

    private void Start()
    {
        rate = false;
        if(isMulti)
        {
            if (SceneManager.GetActiveScene().name.Contains("Station"))
            {
                SoundManager.Instance.PlaySFX(19); 
                //SoundManager.Instance.sfxSource.loop = true;
            }
            else
            {
                SoundManager.Instance.PlaySFX(8);    
                //SoundManager.Instance.sfxSource.loop = true;
            }
            runner = FindObjectOfType<NetworkRunner>();
            countdownTimer = TickTimer.CreateFromSeconds(NetworkManager1.RunnerInstance, 60f);

            if (is911)
            {
                AnimationManager.Instance.AnimationPlay(1, "Ani_1_1_npc2_01(CPR)");
            }
            else
            {
                AnimationManager.Instance.AnimationPlay(2, "Ani_1_1_npc2_01(CPR)");
            }

        }

    }

    void Update()
    {
        if (rate) return;
        if(isMulti)
        {
            // 타이머 동작 중일 때만 남은 시간을 체크
            if (countdownTimer.IsRunning)
            {
                float remainingTime = countdownTimer.RemainingTime(NetworkManager1.RunnerInstance) ?? 0f;

                // 남은 시간을 UI에 표시하거나 디버그로 확인
                Debug.Log("타이머 남은 시간: " + remainingTime);

                // 남은 시간을 UI 업데이트 (예시)
                UpdateTimerUI(remainingTime);
                
                if (runner.GetPlayerObject(runner.LocalPlayer).GetComponent<PlayerData>().NetworkedScore > maxcount)
                {
                    runner.GetPlayerObject(runner.LocalPlayer).GetComponent<PlayerData>().ChangeScore(0);
                    FindObjectOfType<InterlockedFinger>().count = 0;
                    RPC_OnTimerExpired();
                }

                if (!isFirst && !isAED)
                {
                    return;
                }

                if (remainingTime <= 0f)
                {
                    rate = true;

                    if (!IsCoroutineRunning("RPC_OnTimerExpired_Coroutine"))
                    {
                        StartCoroutine(RPC_OnTimerExpired_Coroutine());
                    }
                }

            }
        }
    
    }
    
    bool IsCoroutineRunning(string coroutineName)
    {
        foreach (var activeCoroutine in GetComponents<MonoBehaviour>())
        {
            if (activeCoroutine.GetType().GetMethod(coroutineName) != null)
            {
                return true;
            }
        }
        return false;
    }

    IEnumerator RPC_OnTimerExpired_Coroutine()
    {
        yield return null;
        RPC_OnTimerExpired();
    }
    

    // 타이머 만료 시 호출되는 함수

    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RPC_OnTimerExpired()
    {
        StopCoroutine(RPC_OnTimerExpired_Coroutine());
        SoundManager.Instance.StopSfx();
        SoundManager.Instance.sfxSource.loop = false;

        SoundManager.Instance.PlaySFX(7);
        countdownTimer = TickTimer.CreateFromSeconds(NetworkManager1.RunnerInstance, 60f);
        
        if (!is911)
        {
            cprnum++;
            Debug.Log("타이머가 만료되었습니다!");
            if (cprnum == 1)
            {
                if (!isStation)
                {
                    FindObjectOfType<NetworkGameManagerTest>().Situation6();
                }
                else
                {
                    FindObjectOfType<NetworkGameManagerTest_Station>().CP_CPRDone = true;
                }
            }
        }
        else
        {
            AnimationManager.Instance.AnimationAllStop();
            if(!isFirst) return;
            FindObjectOfType<NetworkGameManagerTest>().Situation8();
        }



        //AnimationManager.Instance.AnimationAllStop();
        countdownTimer = TickTimer.CreateFromSeconds(NetworkManager1.RunnerInstance, 60f);
        transform.parent.gameObject.SetActive(false);
        rate = true;
        // 타이머가 끝났을 때 수행할 작업
    }

    // 남은 시간을 UI에 표시하는 함수 (예시)
    private void UpdateTimerUI(float remainingTime)
    {
        if (!isFirst && remainingTime <= 0f)
        {
            timeText.text = "∞";
            return;
        }
        int roundedTime = Mathf.RoundToInt(remainingTime);
        timeText.text = roundedTime.ToString();
    }

    IEnumerator UpdateScoreUI()
    {
        //테스트로 바꿈 30->2
        while (_countTime < sixtySeconds)
        {
            if (interlockedFinger.count > maxcount) break;
            _countTime += 1;
            if (playerData != null)
            {
                playerData = interlockedFinger.playerData;
                scoreText.text = playerData.NetworkedScore.ToString();
            }
            else
            {
                scoreText.text = interlockedFinger.count+"/30";
            }

            timeText.text = (sixtySeconds - _countTime).ToString();
            yield return new WaitForSeconds(1f);
        }

        //테스트로 바꿈 30->2
        if (interlockedFinger.count < 20) //타이머가 멈췄을 때도 60번만 하면 다음으로 넘어가는 걸로
        {//실패
            AnimationManager.Instance.AnimationStop(2, "Ani_1_1_npc2_01(CPR)");
            SoundManager.Instance.StopSfx();
            _countTime = 0f;
            cprUI.SetActive(true);
            string timetext = "60";
            scoreText.text = timetext;
            interlockedFinger.count = 0;
       
            if (!isMulti)
            {
                SoundManager.Instance.PlayAudioCoroutine(2, 1, () =>
                {
                    GameManager.Instance.NextSituation();
                });
            }
        }
        else
        {
            //성공
            AnimationManager.Instance.AnimationStop(2, "Ani_1_1_npc2_01(CPR)");
            SoundManager.Instance.StopSfx();
            cprUI.SetActive(true);
            string timetext = "60";
            scoreText.text = timetext;
            //CPR 초기화
            interlockedFinger.count = 0;
            _countTime = 0;

            //멀티 아닐때만 다음 상황으로 싱글은 대기
            if (!isMulti)
            {
                SoundManager.Instance.PlayAudioCoroutine(2, 1, () =>
                {
                    GameManager.Instance.NextSituation();
                });
            }
        }
        transform.parent.gameObject.SetActive(false);
    }

    IEnumerator BlinkImage()
    {
        _blinkInterval = (60f / bpm) / 2f;
        while (true)
        {
            BPMImage.SetActive(!BPMImage.activeSelf);
            yield return new WaitForSeconds(_blinkInterval);
        }
    }

    public void OnEnable()
    {
        if (isMulti && !isFirst)
        {
            isAED = false;
            rate = false;
        }
        timeText.text = sixtySeconds.ToString();
    }
}
