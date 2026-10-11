using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private int danio = 1;
    [SerializeField] private float fuerzaRetroceso = 5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth salud = collision.GetComponent<PlayerHealth>();
            if (salud != null)
            {
                salud.RecibirDanio(danio);

                // Knockback al jugador lejos de la calavera
                Rigidbody2D rbJugador = collision.GetComponent<Rigidbody2D>();
                if (rbJugador != null)
                {
                    float direccionX = (collision.transform.position.x > transform.position.x) ? 1f : -1f;
                    rbJugador.linearVelocity = Vector2.zero;
                    rbJugador.AddForce(new Vector2(direccionX * fuerzaRetroceso, fuerzaRetroceso * 0.7f), ForceMode2D.Impulse);
                }
            }
        }
    }
}