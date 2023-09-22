using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    [SerializeField] private Animator m_anim;
    [SerializeField] private float m_transitionTime = 1f;

    public void LoadScene(string scene)
    {
        StartCoroutine(PlayScene(scene));
    }

    private IEnumerator PlayScene(string scene)
    {
        m_anim.SetTrigger("Out");
        Cursor.lockState = CursorLockMode.Locked;

        yield return new WaitForSeconds(m_transitionTime);
        SceneManager.LoadScene(scene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
