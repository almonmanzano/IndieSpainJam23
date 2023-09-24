using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameflow : MonoBehaviour
{
    public static Gameflow Instance { get; private set; }

    [SerializeField] private int m_totalNights = 3;
    [SerializeField] private GameObject m_winScreen;

    [SerializeField] private CharacterSelectedData m_characterData;
    [SerializeField] private Transform m_playerInitialPos;

    [SerializeField] private float m_startTime = 1f;
    [SerializeField] private float m_minTime = 5f;
    [SerializeField] private float m_maxTime = 10f;
    [SerializeField] private HauntedRoom[] m_hauntedRooms;
    [SerializeField] private float m_nightDuration = 10f;
    [SerializeField] private GameObject m_dayMenu;
    [SerializeField] private GameObject m_nightBegins;
    [SerializeField] private float m_nightBeginsDuration = 2f;
    [SerializeField] private float m_spawnTimeReduceRate = 0.2f;

    [SerializeField] private Animator m_anim;
    [SerializeField] private float m_transitionTime = 1f;

    [SerializeField] private MusicSwitcher m_musicSwitcher;

    [SerializeField] private DailySummary m_dailySummary;

    private PlayerMovement m_player;

    private int m_nights = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameObject player = Instantiate(m_characterData.Prefab, m_playerInitialPos.position, Quaternion.identity);
        m_player = player.GetComponent<PlayerMovement>();

        FindObjectOfType<CinemachineVirtualCamera>().Follow = player.transform;

        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        m_anim.SetTrigger("In");
        yield return new WaitForSeconds(m_transitionTime);
        StartCoroutine(StartNight());
    }

    private IEnumerator StartNight()
    {
        HotelManager.Instance.ResetSimpas();
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
            if (!room.IsHaunted() && room.GetGuest().gameObject.activeInHierarchy)
            {
                rooms.Add(room);
            }
        }

        if (rooms.Count > 0)
        {
            int rand = Random.Range(0, rooms.Count);
            rooms[rand].SpawnMonster();
        }

        float t = Random.Range(m_minTime-(m_nights*m_spawnTimeReduceRate), m_maxTime-(m_nights*m_spawnTimeReduceRate));
        yield return new WaitForSeconds(t);

        StartCoroutine(HauntRoom());
    }

    private void EndNight()
    {
        m_musicSwitcher.PlayMusicDay();

        // Destroy objects that were meant to be destroyed but were not yet
        foreach (DestroyAfterSeconds go in FindObjectsOfType(typeof(DestroyAfterSeconds)))
        {
            Destroy(go.gameObject);
        }

        StopAllCoroutines();
        GameManagement.Instance.PauseGame(false);
        GameManagement.Instance.SetPlayable(false);

        m_nights++;

        if (m_nights == m_totalNights)
        {
            m_winScreen.SetActive(true);
        }
        else
        {
            StartDay();
        }
    }

    private void StartDay()
    {
        m_dailySummary.FillTexts();
        m_dayMenu.SetActive(true);
    }

    private IEnumerator RestartNightCoroutine()
    {
        UI_Clock.Instance.Restart();
        m_player.Restart();
        foreach (HauntedRoom room in m_hauntedRooms)
        {
            room.Restart();
        }

        GameManagement.Instance.UnpauseGame();

        m_dayMenu.SetActive(false);

        m_nightBegins.SetActive(true);
        m_nightBegins.GetComponent<Animator>().SetTrigger("NightBegins");

        m_musicSwitcher.PlayMusicNight();

        yield return new WaitForSeconds(m_nightBeginsDuration);

        m_nightBegins.SetActive(false);

        StartCoroutine(StartNight());
    }

    public void RestartNight()
    {
        StartCoroutine(RestartNightCoroutine());
    }

    public float GetNightDuration()
    {
        return m_nightDuration;
    }

    public HauntedRoom GetHauntedRoom(int roomID)
    {
        return m_hauntedRooms[roomID];
    }
}
