using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Guest : MonoBehaviour
{
    [SerializeField] private Sprite m_portrait;
    [SerializeField] private Image m_portraitImage;
    [SerializeField] private float m_maxTranquility = 100f;
    [SerializeField] private float m_fearAmount = 5f;
    [SerializeField] private float m_restoreAmount = 3f;
    [SerializeField] private Slider m_fearSlider;
    [SerializeField] private float m_wakeUpTime = 1f;
    [SerializeField] private GameObject m_vfx;

    private bool m_beingScared = false;
    private float m_tranquility;
    private bool m_awake = false;
    private HauntedRoom m_room;
    private int m_roomID;

    private void Start()
    {
        m_portraitImage.sprite = m_portrait;
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
        StartCoroutine(WakeUpCoroutine());
    }

    private IEnumerator WakeUpCoroutine()
    {
        if (m_portrait == null) print("what");
        m_room.StopHaunted();
        GameManagement.Instance.LoseStar(m_portrait);
        HotelManager.Instance.AddSimpa(m_roomID);
        GetComponent<Animator>().SetTrigger("WakeUp");
        Instantiate(m_vfx, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(m_wakeUpTime);

        gameObject.SetActive(false);
    }

    public void SetRoom(int roomID, HauntedRoom room)
    {
        m_roomID = roomID;
        m_room = room;
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
