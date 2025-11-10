using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class HeartsUI : MonoBehaviour
{
    // Corazones
    public int maxHearts = 3; // Total de corazones
    public static int currentHearts = 3;
    public Sprite fullHeart; // Sprite del corazón lleno
    public Sprite emptyHeart; // Sprite del corazón vacío
    public Image[] hearts;

    public CanvasGroup exitBackgroundImageCanvasGroup;
    float m_Timer;
    public float fadeDuration = 1f;
    public float displayImageDuration = 5f;

    public static void Reset()
    {
        currentHearts = 3;
    }


    void Update()
    {
        // Actualizar corazones
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHearts)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < maxHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
       
    }

    // Método para perder un corazón
    public void LoseHeart()
    {
        if (currentHearts > 0)
        {
            currentHearts--;
            Debug.Log("Corazón perdido. Corazones restantes: " + currentHearts);
        }

        if (currentHearts <= 0)
        {
            Debug.Log("No quedan corazones. Fin del juego.");
            GameOver();
        }
    }

    public int GetHearts()
    {
        return currentHearts;
    }

    // Método para manejar el fin del juego
    public void GameOver()
    {

        Debug.Log("Game Over.");
        exitBackgroundImageCanvasGroup.gameObject.SetActive(true);

        exitBackgroundImageCanvasGroup.blocksRaycasts = true;

        StartCoroutine(FadeOutAndQuit());



    }

    private IEnumerator FadeOutAndQuit()
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

