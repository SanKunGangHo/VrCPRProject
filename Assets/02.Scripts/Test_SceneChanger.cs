using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class Test_SceneChanger : MonoBehaviour
{
    private int count = 0;
    private int materialCount = 0;
    private float changeDelay = 30f; // 씬 전환 딜레이 (초 단위)

    public Button btnBefore;
    public Button btnAfter;
    
    public TMP_Text title, content;

    public GameObject[] VisableUIs;
    
    private bool isHide = false;
    
    [Header("Skybox Textures")]
    public Texture platformTexture;
    public Texture stationTexture;
    public Texture turnsTileTexture;

    public GameObject sphereMaterial;
    public Material skyboxMaterial;

    public enum SceneNames
    {
        Platform = 0,
        Station = 1,
        TurnsTile = 2
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject); // 이 오브젝트를 씬 전환 시에도 파괴되지 않도록 설정
        btnBefore.onClick.AddListener(ChangeBeforeScene);
        btnAfter.onClick.AddListener(ChangeAfterScene);
        
        title.text = "Skybox";
        content.text = "Skybox로 구현한 맵";
        
        // 초기 스카이박스 텍스처 설정
        SetSkyboxTexture();
    }

    public void ChangeBeforeScene()
    {
        StartCoroutine(ChangeScene(-1));
    }

    public void ChangeAfterScene()
    {
        StartCoroutine(ChangeScene(1));
    }

    public void BTNHideUI()
    {
        isHide = !isHide;
        foreach (var uis in VisableUIs)
        {
            uis.SetActive(isHide);
        }

    }

    IEnumerator ChangeScene(int changeCount)
    {
        if (count == 0 && changeCount == -1)
        {
            count = 2;
        }
        
        count = (count + changeCount) % 3;

        Debug.Log($"Current Count: {count}"); // count가 제대로 증가되고 있는지 로그로 확인

        // SceneNames enum의 값을 문자열로 변환해서 LoadSceneAsync 사용
        string sceneName = ((SceneNames)count).ToString();
        Debug.Log($"Loading Scene: {sceneName}"); // 로드할 씬 이름 디버그 로그
            
        try
        {
            SceneManager.LoadScene(sceneName);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to load scene {sceneName}: {ex.Message}");
        }
        
        Debug.Log($"Loading Scene: {sceneName}");

        switch (sceneName)
        {
            case "Platform":
                title.text = "Skybox";
                content.text = "Skybox로 구현한 맵";
                break;
            case "Station":
                title.text = "Sphere";
                content.text = "Sphere로 구현한 맵";
                break;
            case "TurnsTile":
                title.text = "with Plane";
                content.text = "바닥이 있는 맵";
                break;
            default:
                Debug.Log("아직");
                break;
        }
        yield return null;
    }

    public void BTNSetSkyboxTexture()
    {
        materialCount++;
        SetSkyboxTexture();
    }
    
    private void SetSkyboxTexture()
    {
        materialCount %= 3;
        
        Texture newTexture = null;
        switch (materialCount)
        {
            case 0:
                newTexture = platformTexture;
                break;
            case 1:
                newTexture = stationTexture;
                break;
            case 2:
                newTexture = turnsTileTexture;
                break;
        }

        if (skyboxMaterial != null && newTexture != null)
        {
            // 스카이박스 마테리얼의 _MainTex 프로퍼티에 텍스처 설정
            RenderSettings.skybox.SetTexture("_MainTex", newTexture);
        }

        MeshRenderer meshRenderer = null; 
        materialFinder finder = FindObjectOfType<materialFinder>();
        if (finder != null)
        {
            if (finder.TryGetComponent<MeshRenderer>(out meshRenderer))
            {
                meshRenderer.material.SetTexture("_BaseMap", newTexture);
            }
            else
            {
                Debug.Log("MeshRenderer or Material not found!");
            }
        }
        //TODO: sphere 500의 마테리얼 텍스쳐 변경
        
        // if (sphereObject != null && sphereMaterial != null)
        // {
        //     MeshRenderer renderer = sphereObject.GetComponent<MeshRenderer>();
        //     if (renderer != null)
        //     {
        //         renderer.material = sphereMaterial;
        //     }
        // } 이런 느낌으로

    }
}