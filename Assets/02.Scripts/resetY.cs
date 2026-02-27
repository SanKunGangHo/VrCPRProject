using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetY : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);       
    }
}
