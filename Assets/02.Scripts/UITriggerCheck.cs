using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Theme.Primitives;

public class UITriggerCheck : MonoBehaviour
{
    
    public Button btn;
    public GameObject leftTouthObj;
    public GameObject rightTouthObj;
    public GameObject AEDTouchObj;
    public GameObject SOSObj;

    public GameObject Aed;

    public void OnTriggerEnter(Collider other)
    {
        if(btn != null)
        {
            if (other.gameObject.name == "HandIndexFingertip")
            {
                Debug.Log(other.transform.parent.gameObject.name);
                if (btn != null) btn.onClick.Invoke();
            }
        }
        
        if(leftTouthObj != null)
        {
            if(other.gameObject.name == "HandIndexFingertip")
            {
                UIManager.Instance.HandTouchCount++;
               // Debug.Log(UIManager.Instance.HandTouchCount);
            }
        }
        
        if(rightTouthObj != null)
        {
            if (other.gameObject.name == "HandIndexFingertip")
            {
                
                UIManager.Instance.HandTouchCount++;
               // Debug.Log(UIManager.Instance.HandTouchCount);
            }
        }

        if (AEDTouchObj != null)
        {
            if (other.gameObject.name == "PinchArea")
            {
                GetComponent<BoxCollider>().enabled = false;
                Aed.GetComponent<BoxCollider>().enabled = true;
                SoundManager.Instance.PlaySFX(9);
                StartCoroutine(RotateObj());
               
              
               //AEDTouchObj.SetActive(false);
            }
        }

        if (SOSObj != null)
        {
            if (other.gameObject.name == "PinchArea")
            {
                GetComponent<BoxCollider>().enabled = false;
                StartCoroutine(RotateObj());

            }
        }
    }

    public void OnTriggerExit(Collider other)
    {

        if (leftTouthObj != null)
        {
            if (other.gameObject.name == "HandIndexFingertip")
            {
                UIManager.Instance.HandTouchCount--;
            }
        }

        if (rightTouthObj != null)
        {
            if (other.gameObject.name == "HandIndexFingertip")
            {
                UIManager.Instance.HandTouchCount--;
            }
        }
    }

    private IEnumerator RotateObj()
    {

        float elapsedTime = 0f;

        if (AEDTouchObj != null)
        {
            Debug.LogWarning("RotateObj");
            
            Quaternion initRot = AEDTouchObj.gameObject.transform.rotation;
            Quaternion targetRot = Quaternion.Euler(0, -90f, 0) * initRot;

            while (elapsedTime < 1)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / 1;

                AEDTouchObj.gameObject.transform.rotation = Quaternion.Lerp(initRot, targetRot, t);

                yield return null;
            }

            AEDTouchObj.gameObject.transform.rotation = targetRot;
            AEDTouchObj.gameObject.transform.GetChild(0).gameObject.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("RotateObj");

            Quaternion initRot = SOSObj.gameObject.transform.rotation;
            Quaternion targetRot = Quaternion.Euler(0, -90f, 0) * initRot;

            while (elapsedTime < 1)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / 1;

                SOSObj.gameObject.transform.rotation = Quaternion.Lerp(initRot, targetRot, t);

                yield return null;
            }

            SOSObj.gameObject.transform.rotation = targetRot;
            SOSObj.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            gameObject.SetActive(false);
            SOSObj.GetComponent<NetworkObjectController>().RPC_InActive();
        }

   
    }
}
