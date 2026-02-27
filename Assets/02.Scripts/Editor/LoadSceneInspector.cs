using UnityEditor;

[CustomEditor(typeof(LoadScene))]
public class LoadSceneInspector : Editor
{
    private LoadScene _loadScene;

    private void OnEnable()
    {
        _loadScene = (LoadScene)target;
    }

    public override void OnInspectorGUI()
    {
        if (_loadScene == null)
        {
            return;
        }

        if (_loadScene.UseSceneName)
        {
            base.OnInspectorGUI();
        }
        else
        {
            _loadScene.UseSceneName = EditorGUILayout.Toggle("UseSceneName", _loadScene.UseSceneName);
        }
    }
}
