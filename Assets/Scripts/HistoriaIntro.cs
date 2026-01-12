using System.Collections;
using UnityEngine;
using TMPro; // Necesario para TextMeshPro
using UnityEngine.SceneManagement;

public class HistoriaIntro : MonoBehaviour
{
    [Header("Configuración UI")]
    public TextMeshProUGUI textoHistoria; // Arrastra tu objeto de texto aquí
    public GameObject botonSaltar; // Opcional: Para mostrar "Presiona click para continuar"

    [Header("Ajustes Historia")]
    public float velocidadEscritura = 0.05f; // Cuanto más bajo, más rápido

    [TextArea(3, 10)] // Esto hace que en el inspector tengas mucho espacio para escribir
    public string[] parrafos; // Escribe aquí tu historia, párrafo a párrafo

    private int index = 0; // En qué párrafo estamos

    void Start()
    {
        textoHistoria.text = string.Empty;
        StartCoroutine(EscribirLinea());
    }

    void Update()
    {
        // Al hacer click o pulsar espacio
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            // Si el texto ya se escribió completo, pasamos al siguiente
            if (textoHistoria.text == parrafos[index])
            {
                SiguienteParrafo();
            }
            else
            {
                // Si el jugador es impaciente y pulsa click MIENTRAS se escribe:
                // Detenemos la escritura y mostramos todo el párrafo de golpe
                StopAllCoroutines();
                textoHistoria.text = parrafos[index];
            }
        }
    }

    IEnumerator EscribirLinea()
    {
        // Escribe letra a letra
        foreach (char letra in parrafos[index].ToCharArray())
        {
            textoHistoria.text += letra;
            // Aquí podrías poner un sonido de teclado: audioSource.PlayOneShot(sonidoTecla);
            yield return new WaitForSeconds(velocidadEscritura);
        }
    }

    void SiguienteParrafo()
    {
        if (index < parrafos.Length - 1)
        {
            index++;
            textoHistoria.text = string.Empty;
            StartCoroutine(EscribirLinea());
        }
        else
        {
            // Se acabó la historia, cargar el juego
            EmpezarJuego();
        }
    }

    void EmpezarJuego()
    {
        SceneManager.LoadScene("MainScene"); // Asegúrate de que este es el nombre de tu mapa
    }
}