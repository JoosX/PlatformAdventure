using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    // Patrón Observer: Evento de vida para desacoplar la UI
    public static event Action<int> OnVidasCambiadas;

    [SerializeField] private int vidasMaximas = 3;
    [SerializeField] private float tiempoInvulnerabilidad = 0.15f;

    private int vidasActuales;
    private bool esInvulnerable = false;
    private PlayerFlash efectoFlash;
    private PlayerAudio playerAudio; // Referencia al componente de audio

    void Awake()
    {
        efectoFlash = GetComponent<PlayerFlash>();
        playerAudio = GetComponent<PlayerAudio>();
    }

    void Start()
    {
        vidasActuales = vidasMaximas;
        // Notifica la vida inicial al HUD
        OnVidasCambiadas?.Invoke(vidasActuales);
    }

    public void RecibirDanio(int cantidad = 1)
    {
        if (esInvulnerable) return;

        vidasActuales = Mathf.Max(0, vidasActuales - cantidad);
        
        // Emite el evento con la nueva vida
        OnVidasCambiadas?.Invoke(vidasActuales);

        // Dispara el sonido de daño a través de PlayerAudio / AudioManager
        if (playerAudio != null)
        {
            playerAudio.ReproducirDanio();
        }

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