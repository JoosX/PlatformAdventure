using UnityEngine;
using UnityEngine.InputSystem;

public class MenuPausa : MonoBehaviour
{
    [Header("UI de Pausa")]
    public GameObject panelPausa;

    [Header("Cursor de Luz")]
    public GameObject cursorLuz; // Objeto del cursor

    private bool juegoPausado = false;

    void Start()
    {
        
        if (panelPausa != null) panelPausa.SetActive(false);
        if (cursorLuz != null) cursorLuz.SetActive(false);

        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (juegoPausado) Reanudar();
            else Pausar();
        }
    }

    public void Pausar()
    {
        if (panelPausa != null) panelPausa.SetActive(true);
        if (cursorLuz != null) cursorLuz.SetActive(true); // Se activa el cursor al pausar

        Time.timeScale = 0f;
        juegoPausado = true;
    }

    public void Reanudar()
    {
        if (panelPausa != null) panelPausa.SetActive(false);
        if (cursorLuz != null) cursorLuz.SetActive(false); // Se apaga al volver a jugar

        Time.timeScale = 1f;
        juegoPausado = false;
    }

    public void SalirDelJuego()
    {
        Debug.Log("Cerrando el juego...");
        Application.Quit();
    }
}