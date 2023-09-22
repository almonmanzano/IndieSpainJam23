using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    [SerializeField] private Slider m_musicVolumeSlider;
    [SerializeField] private Slider m_sfxVolumeSlider;
    [SerializeField] private AudioSource[] m_musicSources;
    [SerializeField] private AudioSource[] m_sfxSources;

    private static float m_musicVolumeValue = 0.5f;
    private static float m_sfxVolumeValue = 0.5f;

    private void Start()
    {
        UpdateSliders();
        SetMusicVolume();
        SetSFXVolume();
    }

    public void SetMusicVolume()
    {
        foreach (AudioSource source in m_musicSources)
        {
            source.volume = m_musicVolumeSlider.value;
        }
    }

    public void SetSFXVolume()
    {
        foreach (AudioSource source in m_sfxSources)
        {
            source.volume = m_sfxVolumeSlider.value;
        }
    }

    public void UpdateSliders()
    {
        if (m_musicVolumeSlider != null)
        {
            m_musicVolumeSlider.value = m_musicVolumeValue;
        }

        if (m_sfxVolumeSlider != null)
        {
            m_sfxVolumeSlider.value = m_sfxVolumeValue;
        }
    }

    public void PlayAudio(AudioSource source, AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }

    public void PlayAudioRandom(AudioSource source, AudioClip[] clips)
    {
        source.clip = clips[Random.Range(0, clips.Length)];
        source.Play();
    }
}
