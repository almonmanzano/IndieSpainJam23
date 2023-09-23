using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;

public class FadeInAtStart : MonoBehaviour
{
    [SerializeField] private Animator m_anim;
    [SerializeField] private float m_transitionTime = 1f;

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        m_anim.SetTrigger("In");
        Cursor.lockState = CursorLockMode.Locked;

        yield return new WaitForSeconds(m_transitionTime);

        Cursor.lockState = CursorLockMode.Confined;
    }
}
