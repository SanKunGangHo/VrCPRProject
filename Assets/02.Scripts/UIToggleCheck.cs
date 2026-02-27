using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIToggleCheck : MonoBehaviour
{
    public Toggle tg;
  
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "HandIndexFingertip")
        {
            tg.isOn = !tg.isOn;

        }
    }
}
