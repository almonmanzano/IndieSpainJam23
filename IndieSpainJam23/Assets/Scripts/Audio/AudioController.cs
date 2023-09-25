using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSettings m_audioSettings;
    [SerializeField] private Slider m_musicVolumeSlider;
    [SerializeField] private Slider m_sfxVolumeSlider;
    [SerializeField] private AudioSource[] m_musicSources;
    [SerializeField] private AudioSource[] m_sfxSources;

    private void Start()
    {
        UpdateSliders();
        SetMusicVolume();
        SetSFXVolume();
    }

    public void SetMusicVolume()
    {
        if (m_musicVolumeSlider != null) m_audioSettings.MusicVolume = m_musicVolumeSlider.value;
        foreach (AudioSource source in m_musicSources)
        {
            source.volume = m_audioSettings.MusicVolume;
        }
    }

    public void SetSFXVolume()
    {
        if (m_sfxVolumeSlider != null) m_audioSettings.SFXVolume = m_sfxVolumeSlider.value;
        foreach (AudioSource source in m_sfxSources)
        {
            source.volume = m_audioSettings.SFXVolume;
        }
    }

    public void UpdateSliders()
    {
        if (m_musicVolumeSlider != null) m_musicVolumeSlider.value = m_audioSettings.MusicVolume;
        if (m_sfxVolumeSlider != null) m_sfxVolumeSlider.value = m_audioSettings.SFXVolume;
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

    public void StopAllSFX()
    {
        foreach (AudioSource source in m_sfxSources)
        {
            source.Stop();
        }
    }
}
