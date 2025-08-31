using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;
    [Header("Theme Music")]
    [SerializeField] private AudioClip _themeMusic;
    [Header("References")]
    [SerializeField] private AudioSource soundFXObject;
    [SerializeField] private AudioSource _themeAudioSource;
    [SerializeField] private List<AudioClip> _currentSFX = new List<AudioClip>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        PlayThemeMusic(_themeMusic, 1);
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume, bool loop = false, bool regulated = true)
    {
        if (_currentSFX.Contains(audioClip)) return;
        _currentSFX.Add(audioClip);
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.Play();
        float clipLength = audioSource.clip.length;
        if (!loop)
        {
            StartCoroutine(RemoveAudioClip(audioClip, clipLength));
            Destroy(audioSource.gameObject, clipLength);
        }
    }

    public void PlayRandomSoundFXClip(AudioClip[] audioClip, Transform spawnTransform, float volume, bool loop = false)
    {
        int rand = Random.Range(0, audioClip.Length);
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
        audioSource.clip = audioClip[rand];
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.Play();
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }

    private void PlayThemeMusic(AudioClip music, float volume)
    {
        _themeAudioSource.clip = music;
        _themeAudioSource.volume = volume;
        _themeAudioSource.loop = true;
        _themeAudioSource.Play();
    }

    private IEnumerator RemoveAudioClip(AudioClip clip, float delay)
    {
        yield return new WaitForSeconds(delay);
        _currentSFX.Remove(clip);
    }
}
