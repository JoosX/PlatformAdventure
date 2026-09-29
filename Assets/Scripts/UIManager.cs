using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Necesario para TextMeshPro

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Puntos")]
    public TextMeshProUGUI textoPuntosTMP; // Arrastra TextoPuntos si usa TextMeshPro
    public Text textoPuntosLegacy;          // O arrástralo aquí si usa UI Text tradicional

    [Header("Contenedores de Corazones")]
    public GameObject[] corazones; // Arrastra Corazon1, Corazon2, Corazon3

    [Header("Efecto Último Corazón")]
    public float shakeIntensity = 3f;  // Amplitud del temblor en píxeles
    public float shakeSpeed = 25f;      // Velocidad del temblor

    private Coroutine shakeRoutine;
    private Vector3 originalPos;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Este método resuelve el error CS1061 con GameManager
    public void ActualizarPuntos(int puntos)
    {
        string textoFormateado = puntos.ToString("D2"); // Formato "00", "01", etc.

        if (textoPuntosTMP != null)
        {
            textoPuntosTMP.text = textoFormateado;
        }
        else if (textoPuntosLegacy != null)
        {
            textoPuntosLegacy.text = textoFormateado;
        }
    }

    public void ActualizarCorazones(int vidaActual)
    {
        // 1. Mostrar u ocultar corazones según la vida restante
        for (int i = 0; i < corazones.Length; i++)
        {
            if (corazones[i] != null)
            {
                corazones[i].SetActive(i < vidaActual);
            }
        }

        // 2. Si solo queda 1 corazón, activar temblor
        if (vidaActual == 1 && corazones.Length > 0 && corazones[0] != null)
        {
            if (shakeRoutine == null)
            {
                originalPos = corazones[0].transform.localPosition;
                shakeRoutine = StartCoroutine(ShakeHeart(corazones[0].transform));
            }
        }
        else
        {
            DetenerTemblor();
        }
    }

    private IEnumerator ShakeHeart(Transform heartTransform)
    {
        while (true)
        {
            float offsetX = Mathf.Sin(Time.time * shakeSpeed) * shakeIntensity;
            float offsetY = Mathf.Cos(Time.time * shakeSpeed * 1.3f) * (shakeIntensity * 0.5f);

            heartTransform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0f);
            yield return null;
        }
    }

    private void DetenerTemblor()
    {
        if (shakeRoutine != null)
        {
            StopCoroutine(shakeRoutine);
            shakeRoutine = null;

            if (corazones.Length > 0 && corazones[0] != null)
            {
                corazones[0].transform.localPosition = originalPos;
            }
        }
    }

    void OnDisable()
    {
        DetenerTemblor();
    }
}