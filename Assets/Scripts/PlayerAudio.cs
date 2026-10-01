using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [Header("Efectos de Sonido")]
    public AudioClip sfxSalto;
    

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        
        if (audioSource == null) 
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void ReproducirSalto()
    {
        if (sfxSalto != null && audioSource != null)
        {
            audioSource.PlayOneShot(sfxSalto);
        }
    }
}