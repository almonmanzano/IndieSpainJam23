using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class HauntedRoom : MonoBehaviour
{
    [SerializeField] private int m_roomNumber;
    [SerializeField] private GameObject[] m_monsterOptions;

    [SerializeField] private Transform m_guestPosition;
    [SerializeField] private Image m_portraitImage;
    [SerializeField] private Slider m_fearSlider;

    private bool m_haunted = false;
    private Monster m_monster;
    private Guest m_guest;

    private void Update()
    {
        if (m_haunted) AudioRandomizer.Instance.PlayRandom(m_roomNumber);
    }

    public void SetGuest(Guest guest)
    {
        m_guest = guest;
        m_guest.SetRoom(m_roomNumber, this, m_portraitImage, m_fearSlider);
        m_guest.transform.position = m_guestPosition.position;
    }

    public void SpawnMonster()
    {
        if (m_haunted) return;

        m_haunted = true;
        int rand = Random.Range(0, m_monsterOptions.Length);
        GameObject monsterPrefab = m_monsterOptions[rand];
        GameObject monsterGameObject = Instantiate(monsterPrefab, transform.position, Quaternion.identity);
        m_monster = monsterGameObject.GetComponent<Monster>();
        m_monster.SetHauntedRoom(this);

        if (m_guest.gameObject.activeInHierarchy)
        {
            m_guest.BeScared(true);
        }

        // Random voices
        AudioRandomizer.Instance.PlayRandom(m_roomNumber);
    }

    public bool IsHaunted()
    {
        return m_haunted;
    }

    public void StopHaunted()
    {
        m_haunted = false;
        m_monster = null;

        if (m_guest.gameObject.activeInHierarchy)
        {
            m_guest.BeScared(false);
        }
    }

    public void Restart()
    {
        m_haunted = false;
        if (m_monster)
        {
            Destroy(m_monster.gameObject);
        }
        if (m_guest)
        {
            m_guest.gameObject.SetActive(true);
            m_guest.Restart();
        }
        else
        {
            print("what");
        }
    }

    public Guest GetGuest()
    {
        return m_guest;
    }
}
