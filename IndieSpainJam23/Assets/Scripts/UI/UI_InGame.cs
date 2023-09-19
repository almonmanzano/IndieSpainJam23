using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    public static UI_InGame Instance;

    [SerializeField] private Image[] m_starImages;
    [SerializeField] private GameObject m_gameOver;

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
            Color color = m_starImages[i].color;
            m_starImages[i].color = new Color(color.r, color.g, color.b, i < stars ? 1f : 0.5f);
        }
    }

    public void GameOver()
    {
        m_gameOver.SetActive(true);
    }
}
