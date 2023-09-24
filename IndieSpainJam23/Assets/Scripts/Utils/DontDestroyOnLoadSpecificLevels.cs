using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyOnLoadSpecificLevels : MonoBehaviour
{
    [SerializeField] private string[] m_scenes;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        // Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDisable()
    {
        // Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled.
        // Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        bool toBeDestroyed = true;

        foreach (string s in m_scenes)
        {
            if (s == scene.name)
            {
                toBeDestroyed = false;
                break;
            }
        }

        if (toBeDestroyed)
        {
            Destroy(gameObject);
        }
    }
}
