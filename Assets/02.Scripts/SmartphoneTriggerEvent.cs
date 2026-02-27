using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SmartphoneTriggerEvent : MonoBehaviour
{
    private bool isNineOneOne;
    private bool isStationCall;
    private NetworkGameManagerTest_Station _station;
    private Image _callUI;

    private void OnEnable()
    {
        if (SceneManager.GetActiveScene().name.Contains("Station"))
        {
            _station = FindObjectOfType<NetworkGameManagerTest_Station>();
            _callUI = _station.extraUIManager.MultiUIs_1[0];
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isNineOneOne) return;
        if (other.gameObject.name == "HandIndexFingertip")
        {
            //스마트폰 누르는 소리 톡...
            if (!isNineOneOne)
            {
                StartCoroutine(EmergencyCalls_119());
                isNineOneOne = true;
            }
        }
    }

    IEnumerator EmergencyCalls_119()
    {
        //통화 연결음
        SoundManager.Instance.PlaySFX(8);
        while (SoundManager.Instance.sfxSource.isPlaying)
        {
            yield return null;
        }
        
        //알림UI 팝업 (이미 팝업되어있을 것임)
        
        //1:네 119 상황실입니다.
        SoundManager.Instance.PlayNa(9);
        //T답변 UI

        while (SoundManager.Instance.naSource.isPlaying)
        {
            yield return null;
        }
        
        _callUI.gameObject.SetActive(true);
        _callUI.sprite = UIManager.Instance.titleSprites[16];
        
        //답변 딜레이
        yield return new WaitForSeconds(7f);
        
        _callUI.gameObject.SetActive(false);
        
        //2:구조자분 응급환자가 발생하셨다는 거죠. 지금 계시는 위치가 어디세요?
        SoundManager.Instance.PlayNa(10);

        while (SoundManager.Instance.naSource.isPlaying)
        {
            yield return null;
        }
        _callUI.gameObject.SetActive(true);
        _callUI.sprite = UIManager.Instance.titleSprites[17];
        
        //답변 딜레이
        yield return new WaitForSeconds(7f);
        
        //답변창 비활성화
        _callUI.gameObject.SetActive(false);
        
        //3:지금 위치 확인되었습니다. 계시는 곳으로 저희 대원들이 출동하였습니다.....
        SoundManager.Instance.PlayNa(11);

        while (SoundManager.Instance.naSource.isPlaying)
        {
            yield return null;
        }
        
        //전화 종료음
        SoundManager.Instance.PlaySFX(18);

        //TODO: 스마트폰 화면 변경

        //119 전화 완료

        StartCoroutine(EmergencyCalls_Station());
    }

    IEnumerator EmergencyCalls_Station()
    {
        //답변창 다음 전화로 변경
        _callUI.gameObject.SetActive(true);
        _callUI.sprite = UIManager.Instance.titleSprites[20];
     
        //전화 거는 소리
        SoundManager.Instance.PlaySFX(15);
        //전화 거는 소리 기다림
        while (SoundManager.Instance.sfxSource.isPlaying)
        {
            yield return null;
        }
        //네 체험관 역입니다.
        
        SoundManager.Instance.PlayNa(12);
        //말 끝나기까지 대기
        while (SoundManager.Instance.naSource.isPlaying)
        {
            yield return null;
        }
        //여기 성인 남성 한 분이 쓰러졌습니다.
        _callUI.sprite = UIManager.Instance.titleSprites[18];
        
        //답변시간
        yield return new WaitForSeconds(7f);
        _callUI.gameObject.SetActive(false);
        
        //구조자분 응급환자가 발생하셨다는 거죠. 지금 계시는 위치가 어디세요?
        SoundManager.Instance.PlayNa(13);
        //말 끝나기까지 대기
        while (SoundManager.Instance.naSource.isPlaying)
        {
            yield return null;
        }
        _callUI.gameObject.SetActive(true);
        _callUI.sprite = UIManager.Instance.titleSprites[19];
        
        yield return new WaitForSeconds(7f);
        _callUI.gameObject.SetActive(false);
        
        //지금 위치 확인되었습니다.. 계시는 곳으로 저희 직원들이 갔습니다. 침착하게 대기해주세요.
        SoundManager.Instance.PlayNa(14);
        while (SoundManager.Instance.naSource.isPlaying)
        {
            yield return null;
        }
        
        _station.CP_EmergencyCall = true;
        //신고완료_UI 필요하지 않을까?
        //_station.st2Player2 = true;
    }

    public void AllCoroutineStop()
    {
        StopCoroutine(EmergencyCalls_119());
        StopCoroutine(EmergencyCalls_Station());
    }
}
