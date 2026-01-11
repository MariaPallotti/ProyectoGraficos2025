using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para reiniciar o salir al menú

public class PauseMenu : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject panelPausa;

    // Variable para saber si estamos pausados
    public AudioSource musicaJuego; // La música de fondo del nivel
    public AudioSource musicaPausa; // La música de ascensor/menú

    public static bool JuegoPausado = false;

    void Start()
    {
        // Al empezar, nos aseguramos de que la música de pausa esté callada
        if (musicaPausa != null) musicaPausa.Stop();
    }

    void Update()
    {
        // Detectar si pulsamos la tecla Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (JuegoPausado)
            {
                Reanudar();
            }
            else
            {
                Pausar();
            }
        }
    }

    public void Reanudar()
    {
        panelPausa.SetActive(false); // Ocultar menú
        Time.timeScale = 1f;         // El tiempo corre normal
        JuegoPausado = false;

        if (musicaPausa != null) musicaPausa.Stop(); // Parar música pausa
        if (musicaJuego != null) musicaJuego.UnPause(); // Reanudar música juego donde se quedó
    }

    void Pausar()
    {
        panelPausa.SetActive(true);  // Mostrar menú
        Time.timeScale = 0f;         // Congelar el tiempo
        JuegoPausado = true;

        if (musicaJuego != null) musicaJuego.Pause(); // Congelar música juego
        if (musicaPausa != null) musicaPausa.Play();  // Empezar música pausa
    }

    public void SalirAlMenu()
    {
              Time.timeScale = 1f;

        Debug.Log("Cargando MenuPrincipal...");

        // Carga la escena por su nombre exacto
        SceneManager.LoadScene("MenuPrincipal");
    }
}
