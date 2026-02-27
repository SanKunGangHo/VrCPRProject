using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadUIManager : MonoBehaviour
{
    public GameObject loadingScreen;
    
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void loadScreen(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        loadingScreen.SetActive(true);
        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while(!asyncLoad.isDone)
        {
            yield return null;
        }

        Destroy(loadingScreen);
    }
}
