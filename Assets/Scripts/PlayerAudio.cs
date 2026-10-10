using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [Header("Efectos de Sonido")]
    [SerializeField] private AudioClip sfxSalto;
    [SerializeField] private AudioClip sfxDanio;

    public void ReproducirSalto()
    {
        if (sfxSalto != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(sfxSalto);
        }
    }

    public void ReproducirDanio()
    {
        if (sfxDanio != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(sfxDanio);
        }
    }
}