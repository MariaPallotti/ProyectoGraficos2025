using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // <--- NECESARIO PARA MANEJAR IMÁGENES

public class HistoriaIntro : MonoBehaviour
{
    [Header("Configuración UI")]
    public TextMeshProUGUI textoHistoria;
    public GameObject botonSaltar;

    [Header("Visuales")]
    public Image displayImagen; // <--- Arrastra aquí el objeto Image del Canvas
    public Sprite[] listaImagenes; // <--- Aquí pondremos tus ilustraciones

    [Header("Ajustes Historia")]
    public float velocidadEscritura = 0.05f;

    [TextArea(3, 10)]
    public string[] parrafos;

    private int index = 0;

    void Start()
    {
        textoHistoria.text = string.Empty;

        // Cargar la primera imagen al empezar
        ActualizarImagen();

        StartCoroutine(EscribirLinea());
    }

    void Update()
    {
        // Al hacer click o pulsar espacio
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (textoHistoria.text == parrafos[index])
            {
                SiguienteParrafo();
            }
            else
            {
                StopAllCoroutines();
                textoHistoria.text = parrafos[index];
            }
        }
    }

    IEnumerator EscribirLinea()
    {
        foreach (char letra in parrafos[index].ToCharArray())
        {
            textoHistoria.text += letra;
            yield return new WaitForSeconds(velocidadEscritura);
        }
    }

    void SiguienteParrafo()
    {
        if (index < parrafos.Length - 1)
        {
            index++;
            textoHistoria.text = string.Empty;

            // CAMBIAR IMAGEN AL AVANZAR PÁRRAFO
            ActualizarImagen();

            StartCoroutine(EscribirLinea());
        }
        else
        {
            EmpezarJuego();
        }
    }

    void ActualizarImagen()
    {
        // Verificamos que hay imágenes asignadas y que no nos salimos del índice
        if (displayImagen != null && index < listaImagenes.Length)
        {
            // Cambiamos el "Source Image" del componente Image
            displayImagen.sprite = listaImagenes[index];
        }
    }

    void EmpezarJuego()
    {
        SceneManager.LoadScene("MainScene");
    }
}