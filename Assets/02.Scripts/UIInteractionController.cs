using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIInteractionController : MonoBehaviour
{
        public Toggle toggle;
        public Button button;
    public bool testbool;

        private void Start()
        {
            TryGetComponent(out toggle);
            TryGetComponent(out button);
            StartCoroutine(ButtonActivate(button));
        }

        private void OnEnable()
        {
            Debug.Log("button interactable false");
            if (button != null && SceneManager.GetActiveScene().name == "StartScene" && button.interactable == false)
            {
                Debug.Log("button interactable false");
                StartCoroutine(ButtonActivate(button));
            }
        }

        IEnumerator ButtonActivate(Button button)
        {
            yield return new WaitForSeconds(2);
            button.interactable = true;
        }

        private void OnDisable()
        {
            if(SceneManager.GetActiveScene().name != "StartScene") return;
            button.interactable = false;
        }

        private void OnTriggerEnter(Collider other)
        {

            if (other.gameObject.name == "HandIndexFingertip")
            {
                    if (toggle != null)
                    {
                        toggle.isOn = !toggle.isOn;
                    }

                    if (button != null && button.interactable == true)
                    {
                        button.onClick.Invoke();
                    }
            }
        }
}
