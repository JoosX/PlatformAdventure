using UnityEngine;

public class LevelMusicTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip musicaNivelClip;

    private void Start()
    {
        if (AudioManager.Instance != null && musicaNivelClip != null)
        {
            // Cambia automáticamente la pista del menú por la pista del nivel
            AudioManager.Instance.PlayMusic(musicaNivelClip);
        }
    }
}