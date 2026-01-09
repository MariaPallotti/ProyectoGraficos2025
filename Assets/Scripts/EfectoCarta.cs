using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EfectoCarta : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [Header("Animación")]
    public float escalaMouseEncima = 1.2f;
    public float velocidadAnimacion = 10f;
    private Vector3 escalaOriginal;
    private Vector3 escalaObjetivo;

    [Header("Sonido")]
    public AudioClip sonidoSeleccion;
    private AudioSource audioSource;
    private Button boton;

    [Header("Efectos Visuales")]
    public ParticleSystem particulasHover; // <--- NUEVO: Arrastra aquí el sistema de partículas

    void Start()
    {
        escalaOriginal = transform.localScale;
        escalaObjetivo = escalaOriginal;

        // Buscamos el AudioSource de la escena
        BattleManager manager = FindObjectOfType<BattleManager>();
        if (manager != null) audioSource = manager.GetComponent<AudioSource>();

        boton = GetComponent<Button>();
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, escalaObjetivo, Time.deltaTime * velocidadAnimacion);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (boton != null && !boton.interactable) return;

        escalaObjetivo = escalaOriginal * escalaMouseEncima;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        escalaObjetivo = escalaOriginal;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (boton != null && !boton.interactable) return;

        if (audioSource != null && sonidoSeleccion != null)
        {
            audioSource.PlayOneShot(sonidoSeleccion);
        }

        transform.localScale = escalaOriginal * 0.9f;

    }
}