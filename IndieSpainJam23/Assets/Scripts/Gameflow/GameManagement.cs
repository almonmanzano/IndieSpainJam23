using Cinemachine;
using UnityEngine;

public class GameManagement : MonoBehaviour
{
    public static GameManagement Instance { get; private set; }

    [SerializeField] private int m_stars = 1;

    private bool m_gameOver = false;

    private void Awake()
    {
        Instance = this;
    }

    public int GetStars()
    {
        return m_stars;
    }

    public bool GameIsOver()
    {
        return m_gameOver;
    }

    public void LoseStar()
    {
        if (m_gameOver) return;

        m_stars--;

        UI_InGame.Instance.UpdateStars();

        if (m_stars == 0)
        {
            print("GAME OVER");
            m_gameOver = true;
            UI_InGame.Instance.GameOver();
        }
    }
}
