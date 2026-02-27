using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubwayEC_LineRenderController : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    
    public Transform[] points;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        if (_lineRenderer != null)
        {
            // 라인의 시작 부분과 끝 부분의 굵기를 설정합니다.
            _lineRenderer.startWidth = 0.01f; // 시작 굵기
            _lineRenderer.endWidth = 0.01f;   // 끝 굵기
        }
        else
        {
            Debug.LogError("LineRenderer 컴포넌트를 찾을 수 없습니다!");
        }
        StartCoroutine(LineIt());
    }

    //라인 잇기
    IEnumerator LineIt()
    {
        //라인렌더러 어레이에 변수 양 입력
        _lineRenderer.positionCount = points.Length;
        while (true)
        {
            //Transform 어레이화
            _lineRenderer.SetPositions(Array.ConvertAll(points, point => point.position));
            yield return null;
        }
    }
}
