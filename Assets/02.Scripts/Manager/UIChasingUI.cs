using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIChasingUI : MonoBehaviour
{
    public Transform player;
    public GameObject UIChaser;

    private void Start()
    {
        player = FindObjectOfType<OVRCameraRig>().transform;
    }

    public void ChaseUI(Transform target1, Transform target2)
    {
        StartCoroutine(ChaseUI_Coroutine(target1, target2));
    }

    IEnumerator ChaseUI_Coroutine(Transform target1, Transform target2)
    {
        Vector3 middlePoint = (target1.position + target2.position) / 2;
        GameObject chaser = Instantiate(UIChaser, middlePoint, GetTargetRotation());
        
        //TODO: 대충 chaser가 깜빡이는 함수
        
        yield return new WaitForSeconds(3f);
        UIChaser.SetActive(false);
    }
    
    public Quaternion GetTargetRotation()
    {
        return Quaternion.LookRotation(player.position - transform.position);
    }
    
    public void StopChaseUI()
    {
        UIChaser.SetActive(false);
    }
}
