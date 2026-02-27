using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AEDCheckList : MonoBehaviour
{
    public GameObject CheckListUI;
    public List<GameObject> CheckList;
    public Action OnAllCheckListActive;

    // Start is called before the first frame update
    void Start()
    {
        CheckListUI.SetActive(true);
        OnAllCheckListActive += CheckActiveList;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ActiveCheckList(int _num)
    {
        CheckList[_num].SetActive(true);
    }

    public void CheckActiveList()
    {

    }
    
}
