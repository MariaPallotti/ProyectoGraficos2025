using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HeartsUI : MonoBehaviour
{
    [Header("Configuracion Corazones")]
    public int maxHearts = 3;
    public static int currentHearts = 3;

    public Sprite fullHeart;
    public Sprite emptyHeart;
    public Image[] hearts;

    [Header("Game Over")]
    public CanvasGroup exitBackgroundImageCanvasGroup;
    public float fadeDuration = 1f;
    public float displayImageDuration = 5f;

    public static void Reset()
    {
        currentHearts = 3;
    }

    void Start()
    {
        ActualizarVisuales();
    }

    // Eliminado Update(). Ahora llamamos a ActualizarVisuales solo cuando hace falta.

    void ActualizarVisuales()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            // Sprite Lleno o Vacio
            if (i < currentHearts) hearts[i].sprite = fullHeart;
            else hearts[i].sprite = emptyHeart;

            // Mostrar u ocultar según el máximo
            hearts[i].enabled = (i < maxHearts);
        }
    }

    public void LoseHeart()
    {
        if (currentHearts > 0)
        {
            currentHearts--;
            Debug.Log("Corazon perdido. Restantes: " + currentHearts);

            ActualizarVisuales();
            StartCoroutine(EfectoTemblor()); // Feedback visual
        }

        if (currentHearts <= 0)
        {
            GameOver();
        }
    }

    // Efecto simple de vibración (Shake)
    IEnumerator EfectoTemblor()
    {
        Vector3 posicionOriginal = transform.localPosition;
        float duracion = 0.3f;
        float magnitud = 10f; // Cuanto se mueve

        float tiempo = 0;
        while (tiempo < duracion)
        {
            float x = Random.Range(-1f, 1f) * magnitud;
            float y = Random.Range(-1f, 1f) * magnitud;

            transform.localPosition = posicionOriginal + new Vector3(x, y, 0);

            tiempo += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = posicionOriginal;
    }

    public int GetHearts() => currentHearts;

    public void GameOver()
    {
        Debug.Log("Game Over.");
        if (exitBackgroundImageCanvasGroup != null)
        {
            exitBackgroundImageCanvasGroup.gameObject.SetActive(true);
            exitBackgroundImageCanvasGroup.blocksRaycasts = true;
            StartCoroutine(FadeOutAndQuit());
        }
    }

    private IEnumerator FadeOutAndQuit()
    {
        float m_Timer = 0f;
        while (m_Timer < fadeDuration)
        {
            m_Timer += Time.deltaTime;
            exitBackgroundImageCanvasGroup.alpha = m_Timer / fadeDuration;
            yield return null;
        }
        yield return new WaitForSeconds(displayImageDuration);

        // Aseguramos que el tiempo este normal antes de cargar
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }
}