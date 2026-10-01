using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int vidasMaximas = 3;
    [SerializeField] private float tiempoInvulnerabilidad = 0.15f;

    private int vidasActuales;
    private bool esInvulnerable = false;
    private PlayerFlash efectoFlash;

    void Awake()
    {
        efectoFlash = GetComponent<PlayerFlash>();
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

        // Avisa al script visual para que cambie el color
        if (efectoFlash != null)
        {
            efectoFlash.EjecutarFlash(tiempoInvulnerabilidad);
        }

        if (vidasActuales <= 0)
        {
            Morir();
        }
        else
        {
            StartCoroutine(RutinaInvulnerabilidad());
        }
    }

    private IEnumerator RutinaInvulnerabilidad()
    {
        esInvulnerable = true;
        yield return new WaitForSeconds(tiempoInvulnerabilidad);
        esInvulnerable = false;
    }

    void Morir()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}