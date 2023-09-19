using UnityEngine;

public class Guest : MonoBehaviour
{
    [SerializeField] private float m_initialTranquility = 100f;
    [SerializeField] private float m_fearAmount = 5f;

    private bool m_beingScared = false;
    private float m_tranquility;
    private bool m_awake = false;

    private void Start()
    {
        m_tranquility = m_initialTranquility;
    }

    private void Update()
    {
        if (m_awake) return;

        if (m_beingScared)
        {
            m_tranquility -= Time.deltaTime * m_fearAmount;
            if (m_tranquility <= 0)
            {
                m_awake = true;
                WakeUp();
            }
        }
        print(m_tranquility);
    }

    private void WakeUp()
    {
        GameManagement.Instance.LoseStar();
        Destroy(gameObject);
    }

    public void BeScared(bool scared)
    {
        m_beingScared = scared;
    }
}
