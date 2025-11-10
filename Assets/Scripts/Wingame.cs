using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Wingame : MonoBehaviour
{
    public CanvasGroup exitBackgroundImageCanvasGroup;
    float m_Timer;
    public float fadeDuration = 1f;
    public float displayImageDuration = 5f;

    public IEnumerator FadeOutAndQuit()
    {
        m_Timer = 0f; // Reinicia el temporizador

        // Proceso de fade
        while (m_Timer < fadeDuration)
        {
            m_Timer += Time.deltaTime;
            float alphaValue = m_Timer / fadeDuration; // Calcula el alpha
            exitBackgroundImageCanvasGroup.alpha = alphaValue;

            yield return null; // Espera un frame
        }

        // Espera el tiempo de duración de la pantalla de Game Over
        yield return new WaitForSeconds(displayImageDuration);

        Debug.Log("Saliendo del juego...");
        SceneManager.LoadScene("MenuPrincipal"); // Cambia a la escena del menú principal
    }
}
