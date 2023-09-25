using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Guest : MonoBehaviour
{
    [SerializeField] private Sprite m_portrait;
    [SerializeField] private float m_maxTranquility = 100f;
    [SerializeField] private float m_fearAmount = 5f;
    [SerializeField] private float m_restoreAmount = 3f;
    [SerializeField] private float m_wakeUpTime = 1f;
    [SerializeField] private GameObject m_vfx;
    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private AudioClip m_audioWakeUp;

    private bool m_beingScared = false;
    private float m_tranquility;
    private bool m_awake = false;
    private HauntedRoom m_room;
    private int m_roomID;
    private Image m_portraitImage;
    private Slider m_fearSlider;

    private void Start()
    {
        Restart();
    }

    private void Update()
    {
        if (m_awake || !GameManagement.Instance.IsPlayable() || m_fearSlider == null) return;

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
        GameManagement.Instance.LoseStar(m_portrait);
        HotelManager.Instance.AddSimpa(m_roomID);
        GetComponent<Animator>().SetTrigger("WakeUp");
        Instantiate(m_vfx, transform.position, Quaternion.identity);
        m_audioSource.PlayOneShot(m_audioWakeUp);

        yield return new WaitForSeconds(m_wakeUpTime);

        m_room.StopHaunted();
        gameObject.SetActive(false);
    }

    public void SetRoom(int roomID, HauntedRoom room, Image portraitImage, Slider fearSlider)
    {
        m_roomID = roomID;
        m_room = room;
        m_portraitImage = portraitImage;
        m_portraitImage.sprite = m_portrait;
        m_fearSlider = fearSlider;
        m_fearSlider.value = 1f;
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
