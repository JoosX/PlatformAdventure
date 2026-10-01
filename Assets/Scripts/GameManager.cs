using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Patrón Singleton
    public static GameManager Instance { get; private set; }

    // Patrón Observer: Evento público al que se suscribe la UI
    public static event Action<int> OnPuntosCambiados;

    private int puntuacion = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        // Notifica el puntaje inicial al arrancar
        OnPuntosCambiados?.Invoke(puntuacion);
    }

    public void SumarPuntos(int puntos)
    {
        puntuacion += puntos;

        // Dispara el evento sin llamar directamente al UIManager
        OnPuntosCambiados?.Invoke(puntuacion);
    }
}