using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnconnectController : MonoBehaviour
{
    public OVRManager player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<OVRManager>();
        StartCoroutine(SetPosition());
    }
    
    
    IEnumerator SetPosition()
    {
        while (true)
        {
            this.transform.position = player.transform.parent.position ;
            this.transform.rotation = player.transform.parent.rotation ;
            yield return null;
        }
    }
}
