using UnityEngine;
using UnityEngine.SceneManagement;

public class CicloDiaNoche : MonoBehaviour
{
    public static CicloDiaNoche instancia;

    [Header("Tiempo")]
    public float duracionDiaMinutos = 5f;
    [Range(0, 1)]
    public float horaActual = 0.3f; // Empezamos de mañana

    [Header("Luces")]
    public Light sol;
    public Light luna;

    [Header("Configuración Colores")]
    public Gradient colorSol;
    public Gradient colorLuna; // El color de la luz de la luna (Azules)
    public Gradient colorCielo; // Fondo de la cámara
    public Gradient colorAmbiente; // IMPORTANTE: El brillo mínimo del juego

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable() { SceneManager.sceneLoaded += OnSceneLoaded; }
    void OnDisable() { SceneManager.sceneLoaded -= OnSceneLoaded; }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MenuPrincipal" || scene.name == "IntroScene")
        {
            Destroy(gameObject);
            return;
        }

        BuscarLuces();

        // Forzamos que la luz se ponga bien ANTES de que el jugador vea nada
        ActualizarIntensidad();
    }

    void BuscarLuces()
    {
        // 1. Buscamos el Sol
        GameObject objSol = GameObject.Find("Sol");
        if (objSol != null)
        {
            sol = objSol.GetComponent<Light>();
            sol.transform.position = new Vector3(0f, 50f, 0f); // Tu fix de posición

            // --- LA LÍNEA MÁGICA ---
            // Le decimos al Cielo (Skybox) que esta es la luz que debe seguir
            RenderSettings.sun = sol;
        }

        // 2. Buscamos la Luna
        GameObject objLuna = GameObject.Find("Luna");
        if (objLuna != null)
        {
            luna = objLuna.GetComponent<Light>();
            luna.transform.position = new Vector3(0f, 50f, 0f); // Tu fix de posición
        }
    }

    void Update()
    {
        // 1. Avanzar Tiempo
        horaActual += (Time.deltaTime / (duracionDiaMinutos * 60f));
        if (horaActual >= 1) horaActual = 0;

        // 2. Rotación (El Sol y la Luna giran opuestos)
        float angulo = (horaActual * 360f) - 90f;

        if (sol != null) sol.transform.rotation = Quaternion.Euler(angulo, 170f, 0);
        if (luna != null) luna.transform.rotation = Quaternion.Euler(angulo + 180f, 170f, 0); // +180 para estar al otro lado

        // 3. Intensidad y Colores
        ActualizarIntensidad();

        // 4. Ambiente Global (Para que nunca sea negro total)
        RenderSettings.ambientLight = colorAmbiente.Evaluate(horaActual);

        // 5. Color de fondo de cámara (Skybox o Solid Color)
        if (Camera.main != null) Camera.main.backgroundColor = colorCielo.Evaluate(horaActual);
    }

    void ActualizarIntensidad()
    {
        // DÍA (0.25 a 0.75 aprox)
        if (horaActual >= 0.25f && horaActual < 0.75f)
        {
            if (sol != null)
            {
                sol.intensity = 1f; // Sol a tope
                sol.color = colorSol.Evaluate(horaActual);
            }
            if (luna != null) luna.intensity = 0f; // Luna apagada
        }
        // NOCHE
        else
        {
            if (sol != null) sol.intensity = 0f; // Sol apagado
            if (luna != null)
            {
                luna.intensity = 0.5f; // Luna encendida suave
                luna.color = colorLuna.Evaluate(horaActual);
            }
        }
    }
}
