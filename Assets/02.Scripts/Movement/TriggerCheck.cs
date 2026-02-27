using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCheck : MonoBehaviour
{
    private bool isPlayer;

    public bool isHeadCheck = false;

    public bool isbtn = false;
    public bool isAed = false;

    

    // Start is called before the first frame update
    public GameObject step01;

    void Start()
    {
        
    }

    // Update is called once per frame
 


    private void OnTriggerEnter(Collider other)
    {
        
    
        if (isHeadCheck)
        {
            //손으로 일단 예시
            if (other.gameObject.name == "HandIndexFingertip")
            {
                UIManager.Instance.InActivePatientUI(1);
                GameManager.Instance.NextSituation();
            }

        }
        else if (isbtn)
        {
            //체크리스트 4번 키기
            if (other.gameObject.name == "HandIndexFingertip")
            {
                SoundManager.Instance.PlayNa(7);
                SoundManager.Instance.PlaySFX(3);
                step01.SetActive(true);

               
            }

        }
        else if(isAed)
        {
            if (other.gameObject.name == "HandIndexFingertip")
            {
                GameManager.Instance.NextSituation();

                this.gameObject.SetActive(false);
            }
        }
        else if (other.gameObject.layer == 6)
        {
            //isPlayer = true;
        }
       else
        {
            return;
        }
    }

    public void TutorialEnd()
    {
        Debug.Log("Finish");
    }


    public void BtnOn()
    {
        isbtn = true;
    }
}
