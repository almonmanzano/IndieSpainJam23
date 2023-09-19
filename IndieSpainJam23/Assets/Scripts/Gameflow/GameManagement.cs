using Cinemachine;
using UnityEngine;

public class GameManagement : MonoBehaviour
{
    public static GameManagement Instance { get; private set; }

    private int m_stars = 1;

    private void Awake()
    {
        Instance = this;
    }

    public int GetStars()
    {
        return m_stars;
    }

    public void LoseStar()
    {
        m_stars--;

        UI_InGame.Instance.UpdateStars();

        if (m_stars == 0)
        {
            print("GAME OVER");
        }
    }
}
