using UnityEngine;

public class EnemigoPatrulla : MonoBehaviour
{
    [Header("Ruta de Patrulla")]
    [SerializeField] private Transform puntoA;
    [SerializeField] private Transform puntoB;
    [SerializeField] private float velocidad = 2f;

    private Transform objetivoActual;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        objetivoActual = puntoB;
    }

    void Update()
    {
        if (puntoA == null || puntoB == null) return;

        // Si ya está muy cerca del punto objetivo horizontalmente, cambia de meta
        if (Mathf.Abs(transform.position.x - objetivoActual.position.x) < 0.5f)
        {
            if (objetivoActual == puntoB)
            {
                objetivoActual = puntoA;
            }
            else
            {
                objetivoActual = puntoB;
            }

            Voltear();
        }
    }

    void FixedUpdate()
    {
        if (puntoA == null || puntoB == null || rb == null) return;

        // Calcula si debe caminar a la derecha (1) o a la izquierda (-1)
        float direccion = (objetivoActual.position.x > transform.position.x) ? 1f : -1f;

        // Mover conservando la gravedad
        rb.linearVelocity = new Vector2(direccion * velocidad, rb.linearVelocity.y);
    }

    private void Voltear()
    {
        Vector3 escala = transform.localScale;
        escala.x *= -1f;
        transform.localScale = escala;
    }

    private void OnDrawGizmos()
    {
        if (puntoA != null && puntoB != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(puntoA.position, 0.25f);
            Gizmos.DrawWireSphere(puntoB.position, 0.25f);
            Gizmos.DrawLine(puntoA.position, puntoB.position);
        }
    }
}