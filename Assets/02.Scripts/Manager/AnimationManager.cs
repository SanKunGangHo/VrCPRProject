using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class NPC_Anim
{
    public List<Animator> npcs = new List<Animator>();

}

public class AnimationManager : Singleton<AnimationManager>
{
    public NPC_Anim npc_anim;
    public GameObject cprobj;
    public GameObject totta;
    public GameObject npc3_pos;


    public List<GameObject> tottaPositions;


   //애니메이션 끝나고 이벤트 실행 시키는 함수
    public void WaitForAnimationCoroutine(int _npcNum, string _name, Action onComplete = null)
    {
        Animator _npc = npc_anim.npcs[_npcNum];
        Debug.Log(_npc.name);   
        
        StartCoroutine(WaitForAnimation(_npc, _name, onComplete));

    }

    public void AnimationPlay(int _npcNum, string _name)
    {
        Animator _npc = npc_anim.npcs[_npcNum];
        _npc.SetBool(_name, true);
    }

    [Rpc(RpcSources.All,RpcTargets.All)]
    public void RPC_AnimationPlay(int _npcNum, string _name)
    {
        Animator _npc = npc_anim.npcs[_npcNum];
        _npc.SetBool(_name, true);
    }

    public void AnimationStop(int _npcNum, string _name)
    {
        Animator _npc = npc_anim.npcs[_npcNum];
        _npc.SetBool(_name, false);
    }

    //모든 애니메이션 idle상태로 돌림
    public void AnimationAllStop()
    {
        // npc_anim과 npc_anim.npcs가 null인지 한 번만 확인
        if (npc_anim == null || npc_anim.npcs == null)
        {
            Debug.LogWarning("npc_anim 또는 npc_anim.npcs가 초기화되지 않았습니다.");
            return;
        }

        foreach (Animator ani in npc_anim.npcs)
        {
            if (ani == null)
                continue;

            foreach (AnimatorControllerParameter parameter in ani.parameters)
            {
                // 파라미터가 bool 타입인지 확인하고 false로 설정
                if (parameter.type == AnimatorControllerParameterType.Bool)
                {
                    ani.SetBool(parameter.name, false);
                }
            }
        }
    }

    public void AnimateSingleNPCBackWalk(int num, float backDistance = 1f)
    {
     
        // Animator에 "BackWalk" 파라미터가 있는지 확인
        bool hasBackWalkParameter = false;

        foreach (AnimatorControllerParameter parameter in npc_anim.npcs[num].parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool && parameter.name == "BackWalk")
            {
                hasBackWalkParameter = true;
                break;
            }
        }

        // "BackWalk" 파라미터가 있을 경우에만 true로 설정하고 이동
        if (hasBackWalkParameter)
        {
            npc_anim.npcs[num].SetBool("BackWalk", true);

            // NPC를 로컬 좌표계에서 뒤로 이동
            StartCoroutine(MoveNPCBackOverTime(npc_anim.npcs[num].transform, backDistance, 1));
        }
        else
        {
            Debug.LogWarning("Animator에 'BackWalk' 파라미터가 없습니다.");
        }
    }

    public void AnimationAllBackWalk(float backDistance = 1f)
    {
        // npc_anim 또는 npc_anim.npcs가 null인지 먼저 확인
        if (npc_anim == null || npc_anim.npcs == null)
        {
            Debug.LogWarning("npc_anim 또는 npc_anim.npcs가 초기화되지 않았습니다.");
            return;
        }

        foreach (Animator ani in npc_anim.npcs)
        {
            if (ani == null)
                continue;

            // Animator에 "BackWalk" 파라미터가 있는지 확인
            bool hasBackWalkParameter = false;

            foreach (AnimatorControllerParameter parameter in ani.parameters)
            {
                if (parameter.type == AnimatorControllerParameterType.Bool && parameter.name == "BackWalk")
                {
                    hasBackWalkParameter = true;
                    break;
                }
            }

            // "BackWalk" 파라미터가 있을 경우에만 true로 설정하고 이동
            if (hasBackWalkParameter)
            {
                ani.SetBool("BackWalk", true);

                // NPC를 로컬 좌표계에서 뒤로 이동
                //ani.transform.Translate(Vector3.back * backDistance, Space.Self);
                StartCoroutine(MoveNPCBackOverTime(ani.transform, backDistance, 1));
            }
        }
    }


    //위치값 이동하는 애니메이션 
    public void AnimationPlay(int _npcNum, string _name, string _str)
    {
        Animator _npc = npc_anim.npcs[_npcNum];
   
        _npc.SetBool(_name, true);
        Debug.Log("AnimationPlay");
        if (_str == "CPR")
        {
            Debug.Log(npc_anim.npcs[_npcNum]);

            npc_anim.npcs[_npcNum].transform.position = cprobj.transform.position;
            npc_anim.npcs[_npcNum].transform.rotation = cprobj.transform.rotation;
        }
    }


    //정해진 _delay 시간 만큼 실행하고 이벤트 실행 시키는 함수
    public void DelayWaitForAnimationCoroutine(int _npcnum, string _name, float _delay, Action onComplete = null)
    {
        
        Animator _npc = npc_anim.npcs[_npcnum];
       

        StartCoroutine(DelayWaitForAnimation(_npc, _name, _delay, onComplete));
    }



    private IEnumerator WaitForAnimation(Animator _npc, string _name, Action onComplete = null)
    {
        _npc.SetBool(_name, true);
        AnimatorStateInfo stateInfo = _npc.GetCurrentAnimatorStateInfo(0);

 
        // 애니메이션이 끝날 때까지 대기
        while (stateInfo.normalizedTime < 1.0f)
        {
            yield return null;
            stateInfo = _npc.GetCurrentAnimatorStateInfo(0); // 상태 정보 업데이트
        }

        // 지정한 지연 시간만큼 대기
        //yield return new WaitForSeconds(_delay);

       // _npc.SetBool(_name, false);
        // 코루틴 완료 후 콜백 호출
        onComplete?.Invoke();
    }



    private IEnumerator MoveNPCBackOverTime(Transform _npcTransform, float _distance, float _duration)
    {
        Vector3 startPosition = _npcTransform.localPosition;
        Vector3 targetPosition = startPosition + _npcTransform.TransformDirection(Vector3.back) * _distance;

        float elapsedTime = 0f;
        while (elapsedTime < _duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / _duration;

            // 서서히 이동하도록 Lerp 사용
            _npcTransform.localPosition = Vector3.Lerp(startPosition, targetPosition, progress);

            yield return null;
        }

        // 최종 위치 설정
        _npcTransform.localPosition = targetPosition;
    
         AnimationManager.Instance.AnimationAllStop();
      

    }

    private IEnumerator DelayWaitForAnimation(Animator _npc, string _name, float _delay, Action onComplete = null)
    {
        _npc.SetBool(_name, true);

        //지정한 지연 시간만큼 대기
        yield return new WaitForSeconds(_delay);
        _npc.SetBool(_name, false);
        // 코루틴 완료 후 콜백 호출
        onComplete?.Invoke();
        
        //yield return new WaitForSeconds(_delay/2);
        
    }

    private IEnumerator TottaPlay(string _name, int _positionnum, float _delay, Action onComplete = null)
    {
        totta.GetComponent<Animator>().SetBool(_name, true);

        // totta.transform.position = tottaPositions[_positionnum].transform.position;
        // totta.transform.rotation = tottaPositions[_positionnum].transform.rotation;

        yield return new WaitForSeconds(_delay);


        onComplete?.Invoke();
    }

    public void TottaPlayCoroutine(string _name, int _positionnum, float _delay, Action onComplete = null)
    {
        StartCoroutine(TottaPlay(_name, _positionnum, _delay, onComplete));
    }

    public void TottaPlay(string _name, int _positionnum)
    {
        totta.SetActive(true);
        totta.GetComponent<Animator>().SetBool(_name, true);

        // totta.transform.position = tottaPositions[_positionnum].transform.position;
        // totta.transform.rotation = tottaPositions[_positionnum].transform.rotation;
    }
    
    public void NPCPosition(int _positionnum, GameObject _posobj)
    {
        Debug.Log("NPCPosition");
         npc_anim.npcs[_positionnum].transform.position = _posobj.transform.position;
         npc_anim.npcs[_positionnum].transform.rotation = _posobj.transform.rotation;
    }

 
    public void PatientCloseOff()
    {
        if(SceneManager.GetActiveScene().name.Contains("Station") && npc_anim.npcs[2].name.Contains("Without")) return;
        npc_anim.npcs[2].transform.GetChild(0).gameObject.SetActive(true);
        npc_anim.npcs[2].transform.GetChild(1).gameObject.SetActive(false);

        //npc_anim.npcs[2].GetComponent<Animator>().avatar = npc_anim.npcs[2].transform.GetChild(0).GetComponent<Animator>().avatar;
        npc_anim.npcs[2].transform.GetChild(1).GetComponent<Animator>().enabled = false;
        npc_anim.npcs[2] = npc_anim.npcs[2].transform.GetChild(0).GetComponent<Animator>();
    }
}
   


