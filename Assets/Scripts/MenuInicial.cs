using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour
{
    [Header("Configuración")]
    public float espera = 0.6f; // Tiempo para ver el efecto antes de cambiar
    public AudioClip sonidoJugar;

    private AudioSource audioSource;

    void Start()
    {
        // Buscamos o añadimos un AudioSource automáticamente
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Esta función es la que llamas desde el Botón
    public void Jugar()
    {
        StartCoroutine(SecuenciaJugar());
    }

    IEnumerator SecuenciaJugar()
    {
        // 1. Reproducir Sonido
        if (sonidoJugar != null)
        {
            audioSource.PlayOneShot(sonidoJugar);
        }

        // 2. Esperar (Aquí es donde se verá la animación del botón)
        yield return new WaitForSeconds(espera);

        // 3. Resetear todo (Tu lógica original)
        BattlePositionManager.DeleteSavedPosition();
        Stats.Reset();
        HeartsUI.Reset();
        EnemyManager.Reset();

        // 4. Cambiar Escena
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
