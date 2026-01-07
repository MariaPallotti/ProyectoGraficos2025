using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para reiniciar o salir al menú

public class PauseMenu : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject panelPausa; // Arrastra aquí tu objeto "PanelPausa"

    // Variable para saber si estamos pausados
    public static bool JuegoPausado = false;

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
    }

    void Pausar()
    {
        panelPausa.SetActive(true);  // Mostrar menú
        Time.timeScale = 0f;         // Congelar el tiempo
        JuegoPausado = true;
    }

    public void SalirAlMenu()
    {
        // IMPORTANTE: Antes de cambiar de escena, devolvemos el tiempo a la normalidad.
        // Si no lo hacemos, ¡el menú principal o la siguiente partida empezarán congelados!
        Time.timeScale = 1f;

        Debug.Log("Cargando MenuPrincipal...");

        // Carga la escena por su nombre exacto
        SceneManager.LoadScene("MenuPrincipal");
    }
}
