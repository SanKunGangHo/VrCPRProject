using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace _02.Scripts
{
    [Serializable]
    public class CountSprite
    {
        public Sprite ZeroOne;
        public Sprite ZeroTwo;
        public Sprite ZeroThree;
    }

    public class CircleTimer : MonoBehaviour
    {
        [SerializeField]
        public int limitTime = 2;

        public float _fillTime = 0;
        
        public Image countImage;
        public Image fillImage;
        public bool isMulti;

        public CountSprite CountSprite;
        
        //??: 흰색 배경 필요한가?
        
        private void OnEnable()
        {
            StartCoroutine(countStart());
        }

        private void FixedUpdate()
        {
            if(_fillTime >= limitTime) return;
            _fillTime += Time.deltaTime;
            fillImage.fillAmount = _fillTime/limitTime;
        }

        IEnumerator countStart()
        {
            int count = 0;
            while (count <= limitTime)
            {
                countImage.sprite = SwitchSprite(count);
                yield return new WaitForSeconds(1f);
                count++;
            }

            //멀티랑 솔로 나눠야함
            if(isMulti)
            {
                GameManager.Instance.ReturnToLobby();
            }
            else
            {
                GameManager.Instance.NextSituation();
            }
           

        }

        private Sprite SwitchSprite(int count)
        {
            switch (count)
            {
                case 0:
                    return CountSprite.ZeroOne;
                case 1:
                    return CountSprite.ZeroTwo;
                case 2:
                    return CountSprite.ZeroThree;
                default:
                    return CountSprite.ZeroOne;
            }
        }
    }
}