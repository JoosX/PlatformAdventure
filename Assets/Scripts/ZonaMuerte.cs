using UnityEngine;

public class ZonaMuerte : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Detecta si lo que cayó en la zona es el jugador
        if (collision.CompareTag("Player"))
        {
            PlayerHealth salud = collision.GetComponent<PlayerHealth>();
            if (salud != null)
            {
                // Le mandamos 999 de daño para que active tu método Morir() instantáneamente
                salud.RecibirDanio(999); 
            }
        }
    }
}