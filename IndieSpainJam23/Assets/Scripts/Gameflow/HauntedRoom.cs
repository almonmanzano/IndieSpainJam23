using System.Drawing;
using UnityEngine;

public class HauntedRoom : MonoBehaviour
{
    [SerializeField] private GameObject[] m_monsterOptions;
    [SerializeField] private Guest m_guest;

    private bool m_haunted = false;

    public void SpawnMonster()
    {
        if (m_haunted) return;

        m_haunted = true;
        int rand = Random.Range(0, m_monsterOptions.Length);
        GameObject monster = m_monsterOptions[rand];
        GameObject monsterGameObject = Instantiate(monster, transform.position, Quaternion.identity);
        monsterGameObject.GetComponent<Monster>().SetHauntedRoom(this);

        if (m_guest)
        {
            m_guest.BeScared();
        }
    }

    public bool IsHaunted()
    {
        return m_haunted;
    }

    public void StopHaunted()
    {
        m_haunted = false;
    }
}
