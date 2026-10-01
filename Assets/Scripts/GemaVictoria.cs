using UnityEngine;
using UnityEngine.SceneManagement;

public class GemaVictoria : MonoBehaviour
{
    [Header("Efecto Flotante")]
    [SerializeField] private float velocidadFlotacion = 3f;
    [SerializeField] private float alturaFlotacion = 0.2f;
    private Vector3 posicionInicial;

    [Header("Finalización")]
    [SerializeField] private string siguienteEscena = ""; 
    [SerializeField] private GameObject panelVictoria;     
    [SerializeField] private AudioClip sonidoVictoria;

    private bool recolectada = false;

    void Start()
    {
        posicionInicial = transform.position;
    }

    void Update()
    {
        if (recolectada) return;

        // Movimiento oscilante suave arriba y abajo
        float nuevaY = posicionInicial.y + Mathf.Sin(Time.time * velocidadFlotacion) * alturaFlotacion;
        transform.position = new Vector3(posicionInicial.x, nuevaY, posicionInicial.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !recolectada)
        {
            recolectada = true;
            GanarNivel(collision.gameObject);
        }
    }

    private void GanarNivel(GameObject player)
    {
        // 1. Detener por completo la velocidad y clavar al personaje en su posición
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // linearVelocity para Unity 6 (si usas una versión anterior de Unity, cambia a rb.velocity)
            rb.linearVelocity = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        // 2. Desactivar el control del personaje para que no responda al teclado
        // (Usa PlayerController, que es el nombre estándar que tienes en tu carpeta de scripts)
        MonoBehaviour control = player.GetComponent("PlayerController") as MonoBehaviour;
        if (control != null)
        {
            control.enabled = false;
        }

        // 3. Reproducir audio si fue asignado
        if (sonidoVictoria != null)
        {
            AudioSource.PlayClipAtPoint(sonidoVictoria, transform.position);
        }

        // 4. Ocultar la gema
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.enabled = false;
        }

        // 5. Mensaje en consola
        Debug.Log("¡NIVEL COMPLETADO! ¡HAS CONSEGUIDO LA GEMA FINAL!");

        // 6. Mostrar panel o cargar la siguiente escena si está configurada
        if (panelVictoria != null)
        {
            panelVictoria.SetActive(true);
        }
        else if (!string.IsNullOrEmpty(siguienteEscena))
        {
            Invoke("CargarSiguienteEscena", 1.5f);
        }
    }

    private void CargarSiguienteEscena()
    {
        SceneManager.LoadScene(siguienteEscena);
    }
}