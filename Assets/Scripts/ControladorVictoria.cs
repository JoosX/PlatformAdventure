using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorVictoria : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject panelVictoria;

    private bool victoriaActivada = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (victoriaActivada) return;

        // Comprueba si quien tocó la gema es el jugador
        if (collision.CompareTag("Player") || collision.transform.root.CompareTag("Player"))
        {
            victoriaActivada = true;
            MostrarVictoria();
        }
    }

    private void MostrarVictoria()
    {
        if (panelVictoria != null)
        {
            panelVictoria.SetActive(true);
        }

        // Congela el tiempo para detener jugador y enemigos
        Time.timeScale = 0f;
    }

    // Este método lo llamará el botón NEXT
    public void CargarSiguienteNivel()
    {
        // Reanudar el tiempo antes del cambio de escena
        Time.timeScale = 1f;

        int escenaActual = SceneManager.GetActiveScene().buildIndex;
        int siguienteEscena = escenaActual + 1;

        if (siguienteEscena < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(siguienteEscena);
        }
        else
        {
            SceneManager.LoadScene(0); // Vuelve al inicio si ya es el último nivel
        }
    }
}