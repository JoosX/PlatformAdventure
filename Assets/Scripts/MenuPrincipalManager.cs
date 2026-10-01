using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipalManager : MonoBehaviour
{
    public void EmpezarJuego()
    {
        // Cambia "SampleScene" si la escena de tu nivel se llama diferente
        SceneManager.LoadScene("SampleScene"); 
    }

    public void SalirDelJuego()
    {
        Debug.Log("Cerrando el juego...");
        Application.Quit();
    }
}