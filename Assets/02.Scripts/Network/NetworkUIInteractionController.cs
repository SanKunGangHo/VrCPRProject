using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkUIInteractionController : NetworkBehaviour
{
    public Toggle toggle;
    public Button button;
    public bool testbool;

    private void Start()
    {
        TryGetComponent(out toggle);
        TryGetComponent(out button);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "HandIndexFingertip")
        {
            if (toggle != null)
            {
                toggle.isOn = !toggle.isOn;
            }

            if (button != null)
            {
                button.onClick.Invoke();
            }


        }
    }


}
