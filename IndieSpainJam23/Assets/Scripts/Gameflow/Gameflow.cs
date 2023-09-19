using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameflow : MonoBehaviour
{
    [SerializeField] private float m_startTime = 1f;
    [SerializeField] private float m_minTime = 5f;
    [SerializeField] private float m_maxTime = 10f;
    [SerializeField] private HauntedRoom[] m_hauntedRooms;

    private void Start()
    {
        StartCoroutine(StartGameFlow());
    }

    private IEnumerator StartGameFlow()
    {
        yield return new WaitForSeconds(m_startTime);
        StartCoroutine(HauntRoom());
    }

    private IEnumerator HauntRoom()
    {
        List<HauntedRoom> rooms = new List<HauntedRoom>();
        foreach (HauntedRoom room in m_hauntedRooms)
        {
            if (!room.IsHaunted())
            {
                rooms.Add(room);
            }
        }

        if (rooms.Count > 0)
        {
            int rand = Random.Range(0, rooms.Count);
            rooms[rand].SpawnMonster();
        }

        float t = Random.Range(m_minTime, m_maxTime);
        yield return new WaitForSeconds(t);

        StartCoroutine(HauntRoom());
    }
}
