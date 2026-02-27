using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionFollower : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float distance = 3.0f;
    [SerializeField] private bool isUI;
    
    //[SerializeField] private float  = 1.0f;

    [SerializeField] private bool _isCentered = false;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        // UI의 경우, 카메라로 부터 보이는지 확인
        if (isUI)
        {
            if (!IsVisibleInViewport())
            {
                _isCentered = false; // 화면 밖으로 나가면 다시 중심 이동
            }
            Movement();
            transform.LookAt(cameraTransform); // UI 오브젝트는 카메라를 항상 바라보게 처리
        }
    }

    private void Movement()
    {
        if (!_isCentered)
        {
            Vector3 targetPosition = FindTargetPosition();

            MoveTowards(targetPosition);
        }
    }

    private Vector3 FindTargetPosition()
    {
        return cameraTransform.position + cameraTransform.forward * distance;
    }

    private void MoveTowards(Vector3 targetPosition)
    {
        // Lerp를 사용해 현재 위치와 목표 위치를 천천히 가까워지게 함
        transform.position = Vector3.Lerp(transform.position, targetPosition, 0.05f);
    }

    private bool ReachedPosition(Vector3 targetPosition)
    {
        return Vector3.Distance(targetPosition, transform.position) < 0.01f;
    }
    
    private bool IsVisibleInViewport()
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);

        // Viewport 좌표에서 (0,0) ~ (1,1) 사이면 카메라에 보이는 상태
        return viewportPos.x >= 0.3 && viewportPos.x <= 0.7 &&
               viewportPos.y >= 0.3 && viewportPos.y <= 0.7 &&
               viewportPos.z > 0; // 카메라 뒤(음수 z 축)에 있으면 false
    }
    
}
