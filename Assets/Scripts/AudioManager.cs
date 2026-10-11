using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Música de Fondo")]
    public AudioSource bgmSource;
    [Range(0f, 1f)] public float volumenMaximo = 0.8f; // Volumen objetivo normal

    [Header("Efectos de Sonido")]
    public AudioSource sfxSource;

    private Coroutine corrutinaFade;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (bgmSource == null)
            {
                bgmSource = GetComponent<AudioSource>();
            }

            // Aseguramos que arranque con volumen y sonando
            if (bgmSource != null)
            {
                bgmSource.volume = volumenMaximo;
                if (!bgmSource.isPlaying && bgmSource.clip != null)
                {
                    bgmSource.Play();
                }
            }
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += AlCargarEscena;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= AlCargarEscena;
    }

    private void AlCargarEscena(Scene scene, LoadSceneMode mode)
    {
        // Al entrar a una nueva escena, sube la música progresivamente si estaba atenuada
        HacerFadeIn(1.2f);
    }

    // --- REPRODUCCIÓN DE MÚSICA ---
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        if (bgmSource == null) bgmSource = GetComponent<AudioSource>();

        // Si ya está sonando la misma canción, solo asegúrate de que el volumen esté bien
        if (bgmSource.clip == clip && bgmSource.isPlaying)
        {
            return;
        }

        bgmSource.clip = clip;
        bgmSource.volume = volumenMaximo;
        bgmSource.Play();
    }

    // --- EFECTOS DE SONIDO ---
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        if (sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
        else if (bgmSource != null)
        {
            bgmSource.PlayOneShot(clip);
        }
    }

    // --- FADES DE VOLUMEN ---
    public void HacerFadeOut(float duracion)
    {
        if (corrutinaFade != null) StopCoroutine(corrutinaFade);
        corrutinaFade = StartCoroutine(FadeOutCoroutine(duracion));
    }

    public void HacerFadeIn(float duracion)
    {
        if (corrutinaFade != null) StopCoroutine(corrutinaFade);
        corrutinaFade = StartCoroutine(FadeInCoroutine(duracion));
    }

    private IEnumerator FadeOutCoroutine(float duracion)
    {
        if (bgmSource == null) yield break;
        float tiempo = 0f;
        float volInicial = bgmSource.volume;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(volInicial, 0f, tiempo / duracion);
            yield return null;
        }

        bgmSource.volume = 0f;
    }

    private IEnumerator FadeInCoroutine(float duracion)
    {
        if (bgmSource == null) yield break;

        // Si por alguna razón la música se detuvo, vuelve a darle Play
        if (!bgmSource.isPlaying && bgmSource.clip != null)
        {
            bgmSource.Play();
        }

        float tiempo = 0f;
        float volInicial = bgmSource.volume;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(volInicial, volumenMaximo, tiempo / duracion);
            yield return null;
        }

        bgmSource.volume = volumenMaximo;
    }
}