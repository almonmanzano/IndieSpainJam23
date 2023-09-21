using System.Drawing;
using UnityEngine;

public class HauntedRoom : MonoBehaviour
{
    [SerializeField] private GameObject[] m_monsterOptions;
    [SerializeField] private Guest m_guest;

    private bool m_haunted = false;
    private Monster m_monster;

    public void SpawnMonster()
    {
        if (m_haunted) return;

        m_haunted = true;
        int rand = Random.Range(0, m_monsterOptions.Length);
        GameObject monsterPrefab = m_monsterOptions[rand];
        GameObject monsterGameObject = Instantiate(monsterPrefab, transform.position, Quaternion.identity);
        m_monster = monsterGameObject.GetComponent<Monster>();
        m_monster.SetHauntedRoom(this);

        if (m_guest)
        {
            m_guest.BeScared(true);
        }
    }

    public bool IsHaunted()
    {
        return m_haunted;
    }

    public void StopHaunted()
    {
        m_haunted = false;
        m_monster = null;

        if (m_guest)
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
            m_guest.Restart();
        }
    }
}
