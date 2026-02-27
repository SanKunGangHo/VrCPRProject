using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TeleportActivate : MonoBehaviour
{
    [SerializeField]
    public List<Object> TpObjs = new List<Object>();
    private int ObjCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateOn()
    {
        if (ObjCount >= 3) 
            return;

        TpObjs[ObjCount].GetComponent<GameObject>().SetActive(false);
        ObjCount++;
        TpObjs[ObjCount].GetComponent<GameObject>().SetActive(true);
  
    }

}
