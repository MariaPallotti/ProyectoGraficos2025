using UnityEngine;
using TMPro;
using System.Collections;

public class Stats : MonoBehaviour
{
    private static int botellas = 0;

    [Header("UI References")]
    public TextMeshProUGUI botellastxt;

    void Start()
    {
        // Actualizar al inicio para que no salga vacío
        ActualizarTextoUI();
    }

    // Eliminado Update(). Ahora es dirigido por eventos.

    public void UpdateBotellas(int numBotellas)
    {
        botellas += numBotellas;
        ActualizarTextoUI();

        // Efecto visual de "Pop"
        if (botellastxt != null) StartCoroutine(AnimarTexto());
    }

    void ActualizarTextoUI()
    {
        if (botellastxt != null)
        {
            botellastxt.text = botellas.ToString();
        }
    }

    IEnumerator AnimarTexto()
    {
        // Escala hacia arriba
        float timer = 0;
        Vector3 escalaOriginal = Vector3.one;
        Vector3 escalaMax = new Vector3(1.5f, 1.5f, 1.5f); // Crece un 50%

        while (timer < 0.1f)
        {
            botellastxt.transform.localScale = Vector3.Lerp(escalaOriginal, escalaMax, timer / 0.1f);
            timer += Time.deltaTime;
            yield return null;
        }

        // Vuelve a su tamaño
        timer = 0;
        while (timer < 0.1f)
        {
            botellastxt.transform.localScale = Vector3.Lerp(escalaMax, escalaOriginal, timer / 0.1f);
            timer += Time.deltaTime;
            yield return null;
        }
        botellastxt.transform.localScale = escalaOriginal;
    }

    // Getters y Setters estáticos
    public int GetBotellas() => botellas;
    public void SetBotellas(int nBotellas) => botellas = nBotellas;
    public static void Reset() => botellas = 0;
}