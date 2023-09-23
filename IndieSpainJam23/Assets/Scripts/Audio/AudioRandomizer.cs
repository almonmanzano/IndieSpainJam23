using UnityEngine;

public class AudioRandomizer : MonoBehaviour
{
    public static AudioRandomizer Instance { get; private set; }

    [SerializeField] private float m_cd = 10f;
    [SerializeField] private AudioClip[] m_voicesRoom1;
    [SerializeField] private AudioClip[] m_voicesRoom2;
    [SerializeField] private AudioClip[] m_voicesRoom3;
    [SerializeField] private AudioClip[] m_voicesRoom4;
    [SerializeField] private AudioClip[] m_voicesRoom5;

    private AudioSource m_source;
    private float m_timeWithoutVoice;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        m_source = GetComponent<AudioSource>();
        m_timeWithoutVoice = m_cd;
    }

    private void Update()
    {
        m_timeWithoutVoice += Time.deltaTime;
    }

    public void PlayRandom(int roomID)
    {
        if (m_timeWithoutVoice < m_cd) return;

        AudioClip[] randomVoices;
        if (roomID == 0) randomVoices = m_voicesRoom1;
        else if (roomID == 1) randomVoices = m_voicesRoom2;
        else if (roomID == 2) randomVoices = m_voicesRoom3;
        else if (roomID == 3) randomVoices = m_voicesRoom4;
        else randomVoices = m_voicesRoom5;

        if (randomVoices.Length == 0) return;

        int rand = Random.Range(0, randomVoices.Length);
        m_source.clip = randomVoices[rand];
        m_source.Play();
        m_timeWithoutVoice = 0f;
    }
}
