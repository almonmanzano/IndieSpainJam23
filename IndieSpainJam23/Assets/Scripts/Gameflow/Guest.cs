using UnityEngine;
using UnityEngine.UI;

public class Guest : MonoBehaviour
{
    [SerializeField] private float m_maxTranquility = 100f;
    [SerializeField] private float m_fearAmount = 5f;
    [SerializeField] private float m_restoreAmount = 3f;
    [SerializeField] private Slider m_fearSlider;

    private bool m_beingScared = false;
    private float m_tranquility;
    private bool m_awake = false;
    private int m_roomID;

    private void Start()
    {
        Restart();
    }

    private void Update()
    {
        if (m_awake || !GameManagement.Instance.IsPlayable()) return;

        if (m_beingScared)
        {
            m_tranquility = Mathf.Max(0f, m_tranquility - Time.deltaTime * m_fearAmount);
            m_fearSlider.value = m_tranquility / m_maxTranquility;
            if (m_tranquility <= 0)
            {
                m_awake = true;
                WakeUp();
            }
        }
        else
        {
            m_tranquility = Mathf.Min(m_maxTranquility, m_tranquility + Time.deltaTime * m_restoreAmount);
            m_fearSlider.value = m_tranquility / m_maxTranquility;
        }
    }

    private void WakeUp()
    {
        GameManagement.Instance.LoseStar();
        HotelManager.Instance.AddSimpa(m_roomID);
        gameObject.SetActive(false);
    }

    public void SetRoom(int room)
    {
        m_roomID = room;
    }

    public void BeScared(bool scared)
    {
        m_beingScared = scared;
    }

    public void Restart()
    {
        m_awake = false;
        m_beingScared = false;
        m_tranquility = m_maxTranquility;
        m_fearSlider.value = m_tranquility / m_maxTranquility;
    }

    public void RelaxFear()
    {
        m_fearAmount *= 0.75f;
    }

    public void IncreaseRestore()
    {
        m_restoreAmount *= 1.5f;
    }
}
