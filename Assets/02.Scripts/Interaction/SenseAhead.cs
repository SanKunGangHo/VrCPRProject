using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SenseAhead : MonoBehaviour
{
    public float InteractionSpeed;
    [HideInInspector]
    public bool IsInteraction;

    public GameObject HitObject;
    public InteractionInfo HitObjectInfo;

    [SerializeField]
    private float _raycastDistance;
    private bool _hasInteraction;

    private RaycastHit _hit;
    private Ray _ray;

    private void Update()
    {
        IsInteraction = Input.GetKey(KeyCode.E);
        Show();
        Debug.DrawLine(_ray.origin, _ray.origin + _ray.direction * _raycastDistance, Color.red);

        //_ray = new Ray(transform.position, transform.forward);

        //if (Physics.Raycast(_ray, out _hit, _raycastDistance))
        //{
        //    HitObject = _hit.collider.gameObject;
        //    _hasInteraction = HitObject.GetComponent<InteractionInfo>() != null;

        //    UIManager.Instance.InteractionShowText.text = "\0";
        //}
        //else
        //{
        //    HitObject = null;
        //    _hasInteraction = false;

        //    UIManager.Instance.InteractionShowText.text = "\0";
        //    UIManager.Instance.InteractionImage.fillAmount = 0f;
        //}

        //if (_hasInteraction)
        //{
        //    HitObjectInfo = HitObject.GetComponent<InteractionInfo>();

        //    if (!HitObjectInfo.ActionComplete)
        //    {

        //    }
        //}
    }

    private void Show()
    {
        //UIManager.Instance.InteractionShowText.text = HitObject.transform.GetComponent<Item>.itme

        //if (IsInteraction)
        //{
        //    if (!UIManager.Instance.InteractionImage.gameObject.activeSelf)
        //    {
        //        UIManager.Instance.InteractionImage.gameObject.SetActive(true);
        //    }

        //    UIManager.Instance.InteractionImage.fillAmount += (Time.deltaTime * InteractionSpeed * 0.1f);

        //    if (UIManager.Instance.InteractionImage.fillAmount >= 1f)
        //    {
        //        Debug.Log("Test");
        //        UIManager.Instance.InteractionImage.gameObject.SetActive(false);
        //    }
        //}
        //else
        //{
        //    UIManager.Instance.InteractionImage.fillAmount -= (Time.deltaTime * InteractionSpeed * 0.1f);

        //    if (UIManager.Instance.InteractionImage.gameObject.activeSelf && UIManager.Instance.InteractionImage.fillAmount == 0f)
        //    {
        //        UIManager.Instance.InteractionImage.gameObject.SetActive(false);
        //    }
        //}
    }
}
