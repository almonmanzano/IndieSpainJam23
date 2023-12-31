using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private Guest[] m_guests;

    [SerializeField] private float m_nightDuration = 10f;
    [SerializeField] private GameObject m_dayMenu;
    [SerializeField] private GameObject m_nightBegins;
    [SerializeField] private TMP_Text m_nightNumberText;
    [SerializeField] private float m_nightBeginsDuration = 2f;
    [SerializeField] private float m_spawnTimeReduceRate = 0.2f;

    [SerializeField] private Animator m_anim;
    [SerializeField] private float m_transitionTime = 1f;

    [SerializeField] private MusicSwitcher m_musicSwitcher;

    [SerializeField] private DailySummary m_dailySummary;

    [SerializeField] private GameObject m_tutorial;

    [SerializeField] private AudioSource m_stepsAudioSource;
    [SerializeField] private AudioSource m_vacuumAudioSource;
    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private AudioClip m_audioNightBegins;
    [SerializeField] private AudioClip m_audioNightEnds;
    [SerializeField] private AudioClip m_audioWinGame;

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
        m_player.SetStepsAudioSource(m_stepsAudioSource);
        m_player.GetComponent<PlayerAction>().SetVacuumSFX(m_vacuumAudioSource);

        FindObjectOfType<CinemachineVirtualCamera>().Follow = player.transform;

        RandomizeRoomsAndGuests();

        StartCoroutine(StartGame());
    }

    public void RandomizeRoomsAndGuests()
    {
        List<Guest> guests = new List<Guest>();
        guests.AddRange(m_guests);

        foreach (HauntedRoom room in m_hauntedRooms)
        {
            int rand = Random.Range(0, guests.Count);
            room.SetGuest(guests[rand]);
            guests.RemoveAt(rand);
        }
    }

    private IEnumerator StartGame()
    {
        m_anim.SetTrigger("In");
        yield return new WaitForSeconds(m_transitionTime);

        m_tutorial.SetActive(true);
    }

    public void StartFirstNight()
    {
        StartCoroutine(StartFirstNightCoroutine());
    }

    private IEnumerator StartFirstNightCoroutine()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(StartNightCoroutine());
    }

    private IEnumerator StartNightCoroutine()
    {
        GameManagement.Instance.SetPlayable(true);
        HotelManager.Instance.ResetSimpas();
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
            m_audioSource.PlayOneShot(m_audioWinGame);
        }
        else
        {
            m_audioSource.PlayOneShot(m_audioNightEnds);
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
        RandomizeRoomsAndGuests();
        UI_Clock.Instance.Restart();
        m_player.Restart();
        foreach (HauntedRoom room in m_hauntedRooms)
        {
            room.Restart();
        }

        GameManagement.Instance.UnpauseGame();

        m_dayMenu.SetActive(false);

        m_nightBegins.SetActive(true);
        m_nightNumberText.text = "- Noche " + (m_nights + 1) + " -";
        m_nightBegins.GetComponent<Animator>().SetTrigger("NightBegins");
        m_audioSource.PlayOneShot(m_audioNightBegins);

        m_musicSwitcher.PlayMusicNight();

        yield return new WaitForSeconds(m_nightBeginsDuration);

        m_nightBegins.SetActive(false);

        StartCoroutine(StartNightCoroutine());
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
