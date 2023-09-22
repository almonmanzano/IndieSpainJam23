using UnityEngine;

public class MusicSwitcher : MonoBehaviour
{
    [SerializeField] private AudioClip m_musicNight;
    [SerializeField] private AudioClip m_musicDay;

    private AudioSource m_source;

    private void Start()
    {
        m_source = GetComponent<AudioSource>();
    }

    public void PlayMusicNight()
    {
        if (m_source.clip == m_musicNight) return;

        m_source.clip = m_musicNight;
        m_source.Play();
    }

    public void PlayMusicDay()
    {
        if (m_source.clip == m_musicDay) return;

        m_source.clip = m_musicDay;
        m_source.Play();
    }
}
