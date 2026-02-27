using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerUIOffset;

public class PlayerUIOffset : MonoBehaviour
{

    public List<GameObject> elements;

    // 위치와 회전을 정의하는 Enum
    public enum PositionIndex
    {
        element_0,
        element_1,
        element_2,
        element_3,
        element_4,
        element_5,
        element_6,
        element_7,
        element_8,
        element_9,
        element_10,
        element_11,
        element_12,
        element_13,
        element_14,
        element_15,
    }

    public int positionIndex = -1; // 위치와 회전을 결정하는 인덱스 (0 ~ 9 사이의 값)

    // 위치와 회전을 함께 저장하기 위한 구조체 정의
    private struct TransformData
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
        public TransformData(Vector3 pos, Quaternion rot, Vector3 sc)
        {
            position = pos;
            rotation = rot;
            scale = sc;
        }
    }

    // Enum 값과 TransformData를 매핑하는 Dictionary
    private Dictionary<PositionIndex, TransformData> transformDictionary;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
      
        transformDictionary = new Dictionary<PositionIndex, TransformData>();

        for (int i = 0; i < elements.Count; i++)
        {
            // Ensure the index stays within the bounds of PositionIndex
            if (i >= System.Enum.GetValues(typeof(PositionIndex)).Length)
            {
                Debug.LogWarning("Exceeded available enum values for PositionIndex.");
                break;
            }

            // Capture the current position and rotation of each element in the list
            Vector3 currentPosition = elements[i].transform.position;
            Quaternion currentRotation = elements[i].transform.rotation;
            Vector3 currentScale = elements[i].transform.localScale;
            // Store it in the dictionary with the corresponding PositionIndex
            PositionIndex indexEnum = (PositionIndex)i;
            transformDictionary[indexEnum] = new TransformData(currentPosition, currentRotation, currentScale);
        }
       
    }
    void OnEnable()
    {
        positionIndex++;
        // 인덱스를 Enum으로 변환하여 위치 및 회전 설정
        PositionIndex indexEnum = (PositionIndex)positionIndex;

        if (transformDictionary.TryGetValue(indexEnum, out TransformData targetData))
        {
            transform.position = targetData.position; // 오브젝트 위치 변경
            transform.rotation = targetData.rotation; // 오브젝트 회전 변경
            transform.localScale = targetData.scale;
        }
        else
        {
            Debug.LogError("유효하지 않은 위치 인덱스입니다.");
        }
    }

}
