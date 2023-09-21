using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameflow : MonoBehaviour
{
    [SerializeField] private float m_startTime = 1f;
    [SerializeField] private float m_minTime = 5f;
    [SerializeField] private float m_maxTime = 10f;
    [SerializeField] private HauntedRoom[] m_hauntedRooms;
    [SerializeField] private float m_nightDuration = 10f;
    [SerializeField] private GameObject m_dayMenu;
    [SerializeField] private GameObject m_nightBegins;

    private void Start()
    {
        StartCoroutine(StartNight());
    }

    private IEnumerator StartNight()
    {
        GameManagement.Instance.SetPlayable(true);
        yield return new WaitForSeconds(m_startTime);
        StartCoroutine(HauntRoom());
        yield return new WaitForSeconds(m_nightDuration - m_startTime);
        EndNight();
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

    private void EndNight()
    {
        StopAllCoroutines();
        GameManagement.Instance.PauseGame(false);
        GameManagement.Instance.SetPlayable(false);
        m_dayMenu.SetActive(true);
    }

    private IEnumerator RestartNightCoroutine()
    {
        foreach (HauntedRoom room in m_hauntedRooms)
        {
            room.Restart();
        }

        GameManagement.Instance.UnpauseGame();

        m_dayMenu.SetActive(false);

        m_nightBegins.SetActive(true);
        m_nightBegins.GetComponent<Animator>().SetTrigger("NightBegins");

        yield return new WaitForSeconds(1f);

        m_nightBegins.SetActive(false);

        StartCoroutine(StartNight());
    }

    public void RestartNight()
    {
        StartCoroutine(RestartNightCoroutine());
    }
}
