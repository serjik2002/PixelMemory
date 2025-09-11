#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public static class EditorSceneSwitcher
{
    private const string previousSceneKey = "EditorSceneSwitcher_PreviousScenePath";

    static EditorSceneSwitcher()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            string currentScenePath = EditorSceneManager.GetActiveScene().path;
            EditorPrefs.SetString(previousSceneKey, currentScenePath);

            string zeroScenePath = GetScenePathFromBuildSettings(0);
            if (string.IsNullOrEmpty(zeroScenePath))
            {
                return;
            }

            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(zeroScenePath, OpenSceneMode.Single);
            }
        }

        if (state == PlayModeStateChange.EnteredEditMode)
        {
            string previousScenePath = EditorPrefs.GetString(previousSceneKey);
            EditorPrefs.DeleteKey(previousSceneKey);

            string zeroScenePath = GetScenePathFromBuildSettings(0);

            if (!string.IsNullOrEmpty(previousScenePath) && previousScenePath != zeroScenePath)
            {
                EditorSceneManager.OpenScene(previousScenePath, OpenSceneMode.Single);
            }
        }
    }

    private static string GetScenePathFromBuildSettings(int index)
    {
        if (EditorBuildSettings.scenes.Length > index)
        {
            return EditorBuildSettings.scenes[index].path;
        }
        return null;
    }
}
#endif