using UnityEngine;
using UnityEngine.SceneManagement;

public class CicloDiaNoche : MonoBehaviour
{
    public static CicloDiaNoche instancia;

    [Header("Configuración Tiempo")]
    public float duracionDiaMinutos = 2f; // Cuánto tarda un día real (minutos)
    [Range(0, 1)]
    public float horaActual = 0.25f; // 0.25 = Amanecer, 0.5 = Mediodía, 0.75 = Atardecer

    [Header("Configuración Sol")]
    public Light sol; // Referencia a la luz (se buscará sola si está vacía)
    public Gradient colorSol;
    public Gradient colorAmbiente;

    private float velocidadGiro;

    void Awake()
    {
        // SISTEMA SINGLETON CON AUTODESTRUCCIÓN EN MENÚ
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject); // Sobrevive al cambio de escenas
        }
        else
        {
            Destroy(gameObject); // Si ya hay uno, borramos el nuevo
            return;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Se llama cada vez que cargamos una escena nueva
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 1. Si volvemos al menú, nos autodestruimos para reiniciar el reloj en la próxima partida
        if (scene.name == "MenuPrincipal" || scene.name == "IntroScene")
        {
            Destroy(gameObject);
            return;
        }

        // 2. Si entramos a Mapa o Batalla, buscamos el "Sol" de esa escena
        BuscarSol();
    }

    void BuscarSol()
    {
        // Buscamos la primera Luz Direccional que encontremos en la escena nueva
        Light[] luces = FindObjectsOfType<Light>();
        foreach (Light l in luces)
        {
            if (l.type == LightType.Directional)
            {
                sol = l;
                return;
            }
        }
    }

    void Update()
    {
        if (sol == null) return; // Si no hay luz en esta escena, no hacemos nada

        // 1. Avanzar Tiempo
        horaActual += (Time.deltaTime / (duracionDiaMinutos * 60f));
        if (horaActual >= 1) horaActual = 0;

        // 2. Rotar el Sol (Efecto 3D de sombras moviéndose)
        // Rotamos 360 grados en el eje X. (horaActual * 360) - 90 para que 0.25 sea amanecer
        float anguloSol = (horaActual * 360f) - 90f;
        sol.transform.rotation = Quaternion.Euler(anguloSol, 170f, 0);

        // 3. Cambiar Colores (Intensidad y Ambiente)
        sol.color = colorSol.Evaluate(horaActual);
        RenderSettings.ambientLight = colorAmbiente.Evaluate(horaActual);

        // 4. Apagar el sol si es de noche (para que no ilumine desde abajo del suelo)
        if (horaActual > 0.8f || horaActual < 0.2f)
        {
            sol.intensity = Mathf.Lerp(sol.intensity, 0, Time.deltaTime * 2);
        }
        else
        {
            sol.intensity = Mathf.Lerp(sol.intensity, 1, Time.deltaTime * 2);
        }
    }
}
