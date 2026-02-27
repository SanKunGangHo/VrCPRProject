
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    private Image PlayerLoadingImage;
    private Image playerEndingPanel;

    public Sprite[] sprites;
    public Sprite[] AEDsprites;
    public Sprite[] CPRsprites;
    public Sprite[] endingsprites;

    private int currentIdx = 0;
    public GameObject StationObj;
    public GameObject WallObj;
    public bool isMulti;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void RandomSprites()
    {
        bool randomBool = Random.value > 0.5f;
        if (randomBool)
        {
            sprites = AEDsprites;
        }
        else
        {
            sprites = CPRsprites;
        }
    }

    private void OnEnable()
    {
        RandomSprites();
        
        SetCullingMastEnable(5);
        
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            PlayerLoadingImage = UIManager.Instance.PlayerLodingUICanvas.transform.GetChild(1).gameObject
                .GetComponent<Image>();
            playerEndingPanel = UIManager.Instance.PlayerLodingUICanvas.transform.GetChild(0).gameObject
                .GetComponent<Image>();
            PlayerLoadingImage.sprite = sprites[0];
            //StationObj.SetActive(false);
        }
        else
        {
            PlayerLoadingImage = transform.GetChild(1).gameObject
                .GetComponent<Image>();
            playerEndingPanel = transform.GetChild(0).gameObject
                .GetComponent<Image>();
            PlayerLoadingImage.sprite = sprites[0];
        }

        currentIdx = 0;
        StartCoroutine(ChangeSprite());
    }

    private void OnDisable()
    {
        SetCullingMaskDisable();
    }


    // Update is called once per frame
    void Update()
    {
        
    }




    IEnumerator ChangeSprite()
    {
        while (currentIdx < sprites.Length)  // 4번까지만 반복
        {
            // 스프라이트 교체
            PlayerLoadingImage.sprite = sprites[currentIdx];

            // 다음 스프라이트로 인덱스 변경
            currentIdx++;

            // 지정한 시간 대기 (2초마다 변경)
            yield return new WaitForSeconds(2.5f);
        }

     
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            if (isMulti)
            {
                WallObj.SetActive(true);
                FindObjectOfType<NetworkGameManagerTest>().Situation7();
            }
            else
            {
                // 4번째 스프라이트가 끝난 후 오브젝트 비활성화
                GameManager.Instance.NextSituation();
            }
        }
        gameObject.SetActive(false);
    }

    

    //_num : 남길 Layer 번호
    void SetCullingMastEnable(int _num)
    {
        int layerMask = 1 << _num;
        transform.parent.GetComponent<Camera>().cullingMask = layerMask; 
    }

    void SetCullingMaskDisable()
    {
        // 모든 레이어를 켬
        transform.parent.GetComponent<Camera>().cullingMask = ~0; 
    }
}
