using System.Collections;
using UnityEngine;

public class StartSceneStarter : MonoBehaviour
{
    public GameObject startScene;
    IEnumerator Start()
    {
        yield return new WaitForSeconds(3);
        startScene.SetActive(true);
    }
}
