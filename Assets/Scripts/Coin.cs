using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int valor = 100;
    [SerializeField] private AudioClip sfxMoneda; // Casilla para tu pickupcoin.wav

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Reproduce el sonido en la posición antes de destruir el objeto
            if (sfxMoneda != null)
            {
                AudioSource.PlayClipAtPoint(sfxMoneda, transform.position);
            }

            GameManager.Instance.SumarPuntos(valor);
            Destroy(gameObject);
        }
    }
}