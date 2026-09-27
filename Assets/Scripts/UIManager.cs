using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Puntuación")]
    [SerializeField] private TextMeshProUGUI textoPuntos;

    [Header("Corazones")]
    [SerializeField] private Image[] corazones;
    [SerializeField] private Sprite corazonLleno;
    [SerializeField] private Sprite corazonVacio;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ActualizarPuntos(int puntos)
    {
        if (textoPuntos != null)
        {
            textoPuntos.text = puntos.ToString("D4");
        }
    }

    public void ActualizarCorazones(int vidasActuales)
    {
        for (int i = 0; i < corazones.Length; i++)
        {
            if (i < vidasActuales)
            {
                if (corazonLleno != null) corazones[i].sprite = corazonLleno;
                corazones[i].enabled = true;
            }
            else
            {
                if (corazonVacio != null)
                {
                    corazones[i].sprite = corazonVacio;
                }
                else
                {
                    corazones[i].enabled = false;
                }
            }
        }
    }
}