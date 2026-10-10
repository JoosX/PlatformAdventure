using UnityEngine;

public class MenuMusicTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip musicaMenuClip;

    private void Start()
    {
        if (AudioManager.Instance != null && musicaMenuClip != null)
        {
            // Reproduce la pista en bucle a través del Mixer
            AudioManager.Instance.PlayMusic(musicaMenuClip);
        }
    }
}