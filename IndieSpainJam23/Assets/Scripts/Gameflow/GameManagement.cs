using Cinemachine;
using UnityEngine;

public class GameManagement : MonoBehaviour
{
    public static GameManagement Instance { get; private set; }

    [SerializeField] private int m_stars = 1;

    private bool m_playable = false;
    private bool m_gameOver = false;
    private bool m_paused = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !m_gameOver)
        {
            if (!m_paused)
            {
                PauseGame();
            }
            else
            {
                UnpauseGame();
            }
        }
    }

    public void SetPlayable(bool playable)
    {
        m_playable = playable;
    }

    public bool IsPlayable()
    {
        return m_playable && !m_gameOver && !m_paused;
    }

    public void PauseGame(bool showUI=true)
    {
        m_paused = true;
        Time.timeScale = 0f;
        if (showUI)
        {
            UI_InGame.Instance.PauseGame(true);
        }
    }

    public void UnpauseGame()
    {
        m_paused = false;
        Time.timeScale = 1f;
        UI_InGame.Instance.PauseGame(false);
    }

    public int GetStars()
    {
        return m_stars;
    }

    public bool GameIsOver()
    {
        return m_gameOver;
    }

    public bool GamePaused()
    {
        return m_paused;
    }

    public void LoseStar()
    {
        if (m_gameOver) return;

        m_stars--;

        UI_InGame.Instance.UpdateStars();

        ReviewsControl.Instance.SendReview();

        if (m_stars == 0)
        {
            m_gameOver = true;
            Gameflow.Instance.StopAllCoroutines();
            UI_InGame.Instance.GameOver();
        }
    }
}
