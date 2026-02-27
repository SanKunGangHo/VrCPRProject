using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DontDestroyObejct : MonoBehaviour
{
    private static DontDestroyObejct _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;

            if (transform.root != null && transform.parent != null)
            {
                DontDestroyOnLoad(this.transform.root.gameObject);
            }
            else
            {
                DontDestroyOnLoad(this.gameObject);
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
