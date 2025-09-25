using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    // Metodo chiamato al termine del login
    public void OnLoginSuccess()
    {
        // Carica la scena del menu
        SceneManager.LoadScene("MenuScene"); // Assicurati che "MenuScene" sia il nome corretto della scena
    }
}