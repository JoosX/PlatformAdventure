using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FadeAudio : MonoBehaviour
{
    [Header("Configuración del Fade")]
    [SerializeField] private float duracionFade = 2.5f; // Segundos que tarda en subir
    [SerializeField] private float volumenObjetivo = 0.35f; // Volumen final deseado

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0f; // Empieza en silencio absoluto
    }

    void Start()
    {
        StartCoroutine(IniciarFadeIn());
    }

    private IEnumerator IniciarFadeIn()
    {
        float tiempo = 0f;

        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, volumenObjetivo, tiempo / duracionFade);
            yield return null;
        }

        audioSource.volume = volumenObjetivo;
    }
} 