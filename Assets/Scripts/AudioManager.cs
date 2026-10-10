using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Consola de Mezcla")]
    [SerializeField] private AudioMixer mainMixer;

    [Header("Emisores de Audio")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    private Coroutine fadeCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySFX(AudioClip clip, float vol = 1f)
    {
        if (clip == null) return;
        sfxSource.pitch = Random.Range(0.9f, 1.1f);
        sfxSource.PlayOneShot(clip, vol);
    }

    public void PlayMusic(AudioClip clip, bool loop = true, float targetVolume = 0.35f, float fadeDuration = 2.5f)
    {
        if (clip == null) return;
        if (musicSource.clip == clip && musicSource.isPlaying) return;

        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeMusicRoutine(clip, loop, targetVolume, fadeDuration));
    }

    private IEnumerator FadeMusicRoutine(AudioClip clip, bool loop, float targetVol, float duration)
    {
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.volume = 0f;
        musicSource.Play();

        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0f, targetVol, timer / duration);
            yield return null;
        }
        musicSource.volume = targetVol;
    }

    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }

    /// <summary>
    /// Ajusta el volumen del canal Música recibiendo un valor de slider entre 0.0001f y 1f.
    /// </summary>
    public void SetMusicVolume(float sliderValue)
    {
        sliderValue = Mathf.Clamp(sliderValue, 0.0001f, 1f);
        float decibels = Mathf.Log10(sliderValue) * 20f;
        mainMixer.SetFloat("MusicaVol", decibels);
    }

    /// <summary>
    /// Ajusta el volumen del canal SFX recibiendo un valor de slider entre 0.0001f y 1f.
    /// </summary>
    public void SetSFXVolume(float sliderValue)
    {
        sliderValue = Mathf.Clamp(sliderValue, 0.0001f, 1f);
        float decibels = Mathf.Log10(sliderValue) * 20f;
        mainMixer.SetFloat("SFXVol", decibels);
    }
}