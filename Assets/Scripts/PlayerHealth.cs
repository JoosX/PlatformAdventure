using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int vidasMaximas = 3;
    [SerializeField] private float duracionFlash = 0.15f;

    private int vidasActuales;
    private SpriteRenderer spriteRenderer;
    private Color colorOriginal;
    private bool esInvulnerable = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            colorOriginal = spriteRenderer.color;
        }
    }

    void Start()
    {
        vidasActuales = vidasMaximas;
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ActualizarCorazones(vidasActuales);
        }
    }

    public void RecibirDanio(int cantidad = 1)
    {
        if (esInvulnerable) return;

        vidasActuales = Mathf.Max(0, vidasActuales - cantidad);
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ActualizarCorazones(vidasActuales);
        }

        if (spriteRenderer != null)
        {
            StartCoroutine(FlashDanio());
        }

        if (vidasActuales <= 0)
        {
            Morir();
        }
    }

    private IEnumerator FlashDanio()
    {
        esInvulnerable = true;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(duracionFlash);
        spriteRenderer.color = colorOriginal;
        esInvulnerable = false;
    }

    void Morir()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}