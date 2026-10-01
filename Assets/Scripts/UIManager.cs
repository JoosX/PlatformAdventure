using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Puntos")]
    public TextMeshProUGUI textoPuntosTMP;
    public Text textoPuntosLegacy;

    [Header("Contenedores de Corazones")]
    public GameObject[] corazones;

    [Header("Efecto Último Corazón")]
    public float shakeIntensity = 3f;
    public float shakeSpeed = 25f;

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

    // Patrón Observer: Suscripción a eventos
    void OnEnable()
    {
        GameManager.OnPuntosCambiados += ActualizarPuntos;
        PlayerHealth.OnVidasCambiadas += ActualizarCorazones;
    }

    void OnDisable()
    {
        // Cancelación de suscripción para evitar fugas de memoria
        GameManager.OnPuntosCambiados -= ActualizarPuntos;
        PlayerHealth.OnVidasCambiadas -= ActualizarCorazones;
        DetenerTemblor();
    }

    public void ActualizarPuntos(int puntos)
    {
        string textoFormateado = puntos.ToString("D2");

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
        for (int i = 0; i < corazones.Length; i++)
        {
            if (corazones[i] != null)
            {
                corazones[i].SetActive(i < vidaActual);
            }
        }

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
}