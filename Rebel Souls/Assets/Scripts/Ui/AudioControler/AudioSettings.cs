using UnityEngine;
using UnityEngine.Audio;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup _audioMixerGroup;
    public void ChangeMasterVolume(float volume)
    {
        _audioMixerGroup.audioMixer.SetFloat("Master", Mathf.Lerp(-80, 0, volume));
    }

    public void ChangeMusicVolume(float volume)
    {
        _audioMixerGroup.audioMixer.SetFloat("Music", Mathf.Lerp(-80, 0, volume));
    }

    public void ChangeEffectsVolume(float volume)
    {
        _audioMixerGroup.audioMixer.SetFloat("Effects", Mathf.Lerp(-80, 0, volume));
    }

    public void ChangeVoicesVolume(float volume)
    {
        _audioMixerGroup.audioMixer.SetFloat("Voices", Mathf.Lerp(-80, 0, volume));
    }
}

//нужно сохранить переменные 