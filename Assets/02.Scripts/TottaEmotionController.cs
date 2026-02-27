using System;
using UnityEngine;

namespace _02.Scripts
{
    [Serializable]
    public class EmotionSettings
    {
        public GameObject happy;
        public GameObject smile;
        public GameObject surprised;
        public GameObject worry;
    }
    public class TottaEmotionController : MonoBehaviour
    {
        
        
        [Header("Totta Emotion Settings")]
        [Header("아래의 설정은 시작할 때의 표정을 정하는 것")]
        public EmotionType selectedEmotion = EmotionType.Smile;
        
        [Header("표정 변경하려면 \nTottaEmotionController.SetEmotion(EmotionType.'표정 이름')으로\n 변경하면 됨")]
        [Space(20)]
        
        [Tooltip("표정 게임오브젝트 모아둠")]
        [SerializeField]
        private EmotionSettings emotionSettings;
        
        
        public enum EmotionType
        {
            Happy,
            Smile,
            Surprised,
            Worry
        }
        
        private void Start()
        {
            //GetComponent<Animation>().Play();
            SetEmotion(selectedEmotion);
        }

        public void SetEmotion(EmotionType emotion)
        {
            if(emotion != EmotionType.Happy) emotionSettings.happy?.SetActive(false);
            if(emotion != EmotionType.Smile) emotionSettings.smile?.SetActive(false);
            //if(emotion != EmotionType.Surprised) emotionSettings.surprised?.SetActive(false);
            if(emotion != EmotionType.Worry) emotionSettings.worry?.SetActive(false);

            // 선택된 감정에 해당하는 오브젝트 활성화
            switch (emotion)
            {
                case EmotionType.Happy:
                    emotionSettings.happy?.SetActive(true);
                    break;
                case EmotionType.Smile:
                    emotionSettings.smile?.SetActive(true);
                    break;
                // case EmotionType.Surprised:
                //     emotionSettings.surprised?.SetActive(true);
                //     break;
                case EmotionType.Worry:
                    emotionSettings.worry?.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
