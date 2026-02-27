using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class TeleportPointInfo
{
    public Transform guideMapPoint;
    public Transform patientPoint;
    public Transform aedPoint;
    public Transform player2CallPoint;

}

public class TouchToTeleportManager : MonoBehaviour
{ 
    public enum TeleportPoint
    {
        GuideMapPoint,
        PatientPoint,
        AEDPoint,
        Player2CallPoint,
        //이동 포인트가 더 필요하면 여기에 추가
    }

    public float waitTime = 3f;
    
    public TeleportPoint teleportPoint;
    
    public TeleportPointInfo teleportPointInfo;
    //이동 포인트 Transform을 추가

    private GameObject _playerGameObject;
    private GameObject _playerDummy;
    public bool isSingle;

    private void Start()
    {
        _playerGameObject = GameObject.FindGameObjectWithTag("Player");
        _playerDummy = GameObject.Find("ArmatureSkinningUpdateRetargetSkeletonProcessor");
    }

    public void TeleportTo_()
    {
        Transform tmp = teleportPointInfo.guideMapPoint;
        switch (teleportPoint)
        {
            case TeleportPoint.GuideMapPoint:
                tmp = teleportPointInfo.guideMapPoint;
                break;
            case TeleportPoint.PatientPoint:
                tmp = teleportPointInfo.patientPoint;
                break;
            case TeleportPoint.AEDPoint:
                tmp = teleportPointInfo.aedPoint;
                break;
                case TeleportPoint.Player2CallPoint:
                tmp = teleportPointInfo.player2CallPoint;
                break;
            default:
                tmp = teleportPointInfo.patientPoint;
                break;
        }

        StartCoroutine(TeleportTimerCoroutine(tmp));
    }

    IEnumerator TeleportTimerCoroutine(Transform target)
    {
        //Rotation도 추가함 (2024-11-07 송승윤)
        //TODo: 이미지 팝업 띄우기
        yield return new WaitForSeconds(waitTime);
        _playerGameObject.transform.position = target.position;
        _playerGameObject.transform.rotation = target.rotation;
        if(isSingle)
        {
            _playerDummy.transform.position = target.position;
            _playerDummy.transform.rotation = target.rotation;
        }

    }
}
