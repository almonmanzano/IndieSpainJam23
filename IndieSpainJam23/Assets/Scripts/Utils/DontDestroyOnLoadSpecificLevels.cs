using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyOnLoadSpecificLevels : MonoBehaviour
{
    [SerializeField] private string[] m_scenes;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnLevelWasLoaded(int level)
    {
        foreach (string scene in m_scenes)
        {
            if (SceneManager.GetSceneByName(scene) != SceneManager.GetActiveScene())
            {
                Destroy(gameObject);
            }
        }
    }
}
