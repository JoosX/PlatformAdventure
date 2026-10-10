using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButtonAudio : MonoBehaviour
{
    [Header("Efecto de Clic")]
    public AudioClip sfxClick;

    private Button boton;

    private void Awake()
    {
        boton = GetComponent<Button>();
    }

    private void OnEnable()
    {
        if (boton != null)
        {
            boton.onClick.AddListener(ReproducirClick);
        }
    }

    private void OnDisable()
    {
        if (boton != null)
        {
            boton.onClick.RemoveListener(ReproducirClick);
        }
    }

    private void ReproducirClick()
    {
        if (sfxClick != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(sfxClick);
        }
    }
}