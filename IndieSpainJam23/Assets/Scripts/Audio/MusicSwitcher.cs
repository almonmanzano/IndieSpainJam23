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
        PlayMusic(m_musicNight);
    }

    public void PlayMusicDay()
    {
        PlayMusic(m_musicDay);
    }

    private void PlayMusic(AudioClip clip)
    {
        if (m_source.clip == clip)
            return;

        m_source.clip = clip;
        m_source.Play();
    }
}
