using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningPopup : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(PopUpDisappear());
    }

    private IEnumerator PopUpDisappear()
    {
        yield return new WaitForSeconds(3f);
        this.gameObject.SetActive(false);
    }
}
