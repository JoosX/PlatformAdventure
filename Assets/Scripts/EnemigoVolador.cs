using UnityEngine;

public class EnemigoVolador : MonoBehaviour
{
    [Header("Detección y Movimiento")]
    [SerializeField] private float velocidad = 3f;
    [SerializeField] private float velocidadRetorno = 1.5f;
    [SerializeField] private float radioDeteccion = 5f;

    [Header("Combate")]
    [SerializeField] private int dano = 1;
    [SerializeField] private float fuerzaRetroceso = 8f;

    [Header("Visuales")]
    [SerializeField] private bool voltearSprite = true;

    private Vector3 posicionInicial;
    private Transform transformPlayer;
    private SpriteRenderer sr;

    void Start()
    {
        posicionInicial = transform.position;
        sr = GetComponent<SpriteRenderer>();

        // Busca automáticamente al jugador por su Tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            transformPlayer = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("EnemigoVolador: No se encontró ningún GameObject con el Tag 'Player'.");
        }
    }

    void Update()
    {
        if (transformPlayer == null) return;

        float distancia = Vector2.Distance(transform.position, transformPlayer.position);

        if (distancia <= radioDeteccion)
        {
            // Perseguir al jugador
            Vector3 direccion = (transformPlayer.position - transform.position).normalized;
            transform.position += direccion * velocidad * Time.deltaTime;

            if (voltearSprite && sr != null)
            {
                sr.flipX = direccion.x > 0;
            }
        }
        else
        {
            // Retornar al punto inicial si el jugador sale del radio
            if (Vector2.Distance(transform.position, posicionInicial) > 0.05f)
            {
                Vector3 direccion = (posicionInicial - transform.position).normalized;
                transform.position += direccion * velocidadRetorno * Time.deltaTime;

                if (voltearSprite && sr != null)
                {
                    sr.flipX = direccion.x > 0;
                }
            }
            else
            {
                transform.position = posicionInicial;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 1. Aplicar daño mediante PlayerHealth
            PlayerHealth health = collision.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.RecibirDanio(dano);
            }

            // 2. Aplicar retroceso físico (knockback)
            Rigidbody2D rbPlayer = collision.GetComponent<Rigidbody2D>();
            if (rbPlayer != null)
            {
                Vector2 direccionEmpuje = (collision.transform.position - transform.position).normalized;
                if (direccionEmpuje.y < 0.2f) direccionEmpuje.y = 0.5f;

                rbPlayer.linearVelocity = Vector2.zero;
                rbPlayer.AddForce(direccionEmpuje * fuerzaRetroceso, ForceMode2D.Impulse);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Rango de visión en rojo dentro de la Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioDeteccion);

        // Posición base inicial en cian
        if (!Application.isPlaying)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, 0.2f);
        }
    }
}