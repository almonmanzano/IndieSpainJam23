using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameManagement : MonoBehaviour
{
    public static GameManagement Instance { get; private set; }

    [SerializeField] private int m_initialStars = 5;
    [SerializeField] private Volume m_globalVolume;
    [SerializeField] private GameObject[] m_toDisableAtPause;
    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private AudioClip m_audioGameOver;
    [SerializeField] private AudioController m_audioController;

    private int m_stars;

    private bool m_playable = false;
    private bool m_gameOver = false;
    private bool m_paused = false;

    private DepthOfField m_depthOfField;

    private PlayerAction m_playerAction;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        m_stars = m_initialStars;
        m_globalVolume.profile.TryGet(out m_depthOfField);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && m_playable && !m_gameOver)
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
        if (m_depthOfField) m_depthOfField.active = true;  // Blur

        foreach (GameObject obj in m_toDisableAtPause)
        {
            obj.SetActive(false);
        }

        m_paused = true;
        Time.timeScale = 0f;
        if (showUI)
        {
            UI_InGame.Instance.PauseGame(true);
        }
        else
        {
            m_audioController.StopAllSFX();
            m_playerAction.StopVacuum();
            m_playerAction.gameObject.GetComponent<PlayerMovement>().StopRunningAnimation();
        }
    }

    public void UnpauseGame()
    {
        if (m_depthOfField) m_depthOfField.active = false;  // Blur

        foreach (GameObject obj in m_toDisableAtPause)
        {
            obj.SetActive(true);
        }

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

    public void LoseStar(Sprite portrait)
    {
        if (m_gameOver) return;

        m_stars--;

        UI_InGame.Instance.UpdateStars();

        ReviewsControl.Instance.SendReview(portrait);

        if (m_stars == 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        if (m_depthOfField) m_depthOfField.active = true;  // Blur

        foreach (GameObject obj in m_toDisableAtPause)
        {
            obj.SetActive(false);
        }

        m_audioSource.PlayOneShot(m_audioGameOver);
        m_gameOver = true;
        Gameflow.Instance.StopAllCoroutines();
        UI_InGame.Instance.GameOver();
    }

    public void SetPlayer(PlayerAction player)
    {
        m_playerAction = player;
    }
}
