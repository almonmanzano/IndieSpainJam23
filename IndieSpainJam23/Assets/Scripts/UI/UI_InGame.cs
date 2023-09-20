using UnityEngine;

public class UI_InGame : MonoBehaviour
{
    public static UI_InGame Instance;

    [SerializeField] private GameObject[] m_starImages;
    [SerializeField] private GameObject m_gameOver;
    [SerializeField] private GameObject m_pausePanel;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateStars();
    }

    public void UpdateStars()
    {
        int stars = GameManagement.Instance.GetStars();
        for (int i = 0; i < m_starImages.Length; i++)
        {
            m_starImages[i].SetActive(i < stars ? true : false);
        }
    }

    public void GameOver()
    {
        m_gameOver.SetActive(true);
    }

    public void PauseGame(bool pause)
    {
        m_pausePanel.SetActive(pause);
    }
}
