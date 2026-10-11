using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GemaVictoria : MonoBehaviour
{
    [Header("Efecto Flotante")]
    [SerializeField] private float velocidadFlotacion = 3f;
    [SerializeField] private float alturaFlotacion = 0.2f;
    private Vector3 posicionInicial;

    [Header("Finalización")]
    [SerializeField] private string siguienteEscena = "Level_02"; 
    [SerializeField] private GameObject panelVictoria;     
    [SerializeField] private AudioClip sonidoVictoria;

    [Header("Transición")]
    [SerializeField] private float duracionFade = 1.0f;
    private float alphaFade = 0f;
    private Texture2D texturaNegra;
    private bool haciendoFade = false;

    private bool recolectada = false;

    void Awake()
    {
        // Generar un pixel negro puro en memoria para el fade
        texturaNegra = new Texture2D(1, 1);
        texturaNegra.SetPixel(0, 0, Color.black);
        texturaNegra.Apply();
    }

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
        // 1. Detener velocidad y congelar personaje
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        // 2. Desactivar controles del jugador
        MonoBehaviour control = player.GetComponent("PlayerController") as MonoBehaviour;
        if (control != null)
        {
            control.enabled = false;
        }

        // 3. Audio de victoria
        if (sonidoVictoria != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(sonidoVictoria);
        }

        // 4. Ocultar la gema
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.enabled = false;
        }

        // 5. Mensaje de consola
        Debug.Log("¡NIVEL COMPLETADO! ¡HAS CONSEGUIDO LA GEMA FINAL!");

        // 6. Panel opcional
        if (panelVictoria != null)
        {
            panelVictoria.SetActive(true);
        }

        // 7. Iniciar la transición visual y cambio de escena
        StartCoroutine(TransicionCambioNivel());
    }

    private IEnumerator TransicionCambioNivel()
    {
        yield return new WaitForSeconds(0.3f); // Breve pausa tras tocar la gema

        haciendoFade = true;
        float tiempo = 0f;

        // Bajar la música progresivamente al mismo tiempo que el fade visual
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.HacerFadeOut(duracionFade);
        }

        // Animación progresiva oscureciendo la pantalla a negro
        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;
            alphaFade = Mathf.Clamp01(tiempo / duracionFade);
            yield return null;
        }

        alphaFade = 1f;

        // Carga por nombre si está asignado, de lo contrario por índice
        if (!string.IsNullOrEmpty(siguienteEscena))
        {
            SceneManager.LoadScene(siguienteEscena);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    private void OnGUI()
    {
        // Dibuja el fundido negro directamente sobre toda la pantalla
        if (haciendoFade && alphaFade > 0f)
        {
            GUI.color = new Color(0f, 0f, 0f, alphaFade);
            GUI.depth = -1000; // Garantiza que quede encima de todo
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texturaNegra);
        }
    }

    private void OnDestroy()
    {
        if (texturaNegra != null)
        {
            Destroy(texturaNegra);
        }
    }
}