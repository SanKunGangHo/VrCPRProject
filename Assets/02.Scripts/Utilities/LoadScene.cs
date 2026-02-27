using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public bool UseSceneName;

    [SerializeField]
    private EnumTypes.SceneName _sceneName;

    public void ReturnToTheLobby()
    {
        NetworkManager.ReturnToLobby();
    }

    public void LoadTheScene()
    {
        SceneManager.LoadScene(_sceneName.ToString());
    }
}