using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int valor = 100;
    [SerializeField] private AudioClip sfxMoneda; // Casilla para tu pickupcoin.wav

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Reproduce el sonido a través del AudioManager (enrutado a SFX)
            if (sfxMoneda != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(sfxMoneda);
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.SumarPuntos(valor);
            }

            Destroy(gameObject);
        }
    }
}