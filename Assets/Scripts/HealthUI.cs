using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    private HeartsUI heartsUI;
    private Stats statsScript; // Renombrado para evitar confusión con la clase Stats

    [Header("Configuración Combate")]
    public int maxHealth = 100;
    public int currentHealth = 100;
    public Image healthBar;
    public float velocidadBarra = 5f; // Velocidad de la animación

    [Header("Configuración Maná")]
    public int maxMana = 100;
    public int currentMana;
    public Image manaBar; // Renombrado de 'bar' a 'manaBar' para claridad

    void Start()
    {
        heartsUI = FindObjectOfType<HeartsUI>();
        statsScript = FindObjectOfType<Stats>();

        if (heartsUI == null) Debug.LogError("Falta HeartsUI");
        if (statsScript == null) Debug.LogError("Falta Stats");

        StartCombat();
    }

    // Eliminamos el Update() para ahorrar recursos. Solo actualizamos cuando algo cambia.

    public void StartCombat()
    {
        currentHealth = maxHealth;
        // Obtenemos maná global
        if (statsScript != null)
        {
            currentMana = statsScript.GetBotellas();
            maxMana = currentMana; // O un valor fijo si prefieres
        }

        // Actualización inicial instantánea
        if (healthBar) healthBar.fillAmount = 1;
        if (manaBar) manaBar.fillAmount = 1;
    }

    public void TakeCombatDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Actualizamos la barra visualmente con animación
        if (healthBar != null) StartCoroutine(AnimarBarra(healthBar, (float)currentHealth / maxHealth));

        if (currentHealth <= 0)
        {
            if (heartsUI != null) heartsUI.LoseHeart();
            if (heartsUI != null && heartsUI.GetHearts() <= 0) Die();
        }
    }

    public bool ConsumeMana(int amount)
    {
        if (amount > currentMana) return false;

        currentMana -= amount;

        // Animamos la barra de maná
        if (manaBar != null) StartCoroutine(AnimarBarra(manaBar, (float)currentMana / maxMana));

        return true;
    }

    public void RegenerateMana(int amount)
    {
        currentMana += amount;
        if (currentMana > maxMana) currentMana = maxMana;

        if (manaBar != null) StartCoroutine(AnimarBarra(manaBar, (float)currentMana / maxMana));
    }

    // CORRUTINA MÁGICA: Hace que la barra se mueva suavemente hacia el objetivo
    IEnumerator AnimarBarra(Image barra, float objetivo)
    {
        while (Mathf.Abs(barra.fillAmount - objetivo) > 0.01f)
        {
            barra.fillAmount = Mathf.Lerp(barra.fillAmount, objetivo, Time.deltaTime * velocidadBarra);
            yield return null;
        }
        barra.fillAmount = objetivo;
    }

    void Die()
    {
        heartsUI.GameOver();
    }

    // Getters
    public bool IsAlive() => currentHealth > 0;
    public int GetCurrentMana() => currentMana;
    public bool HasHeartsLeft() => heartsUI.GetHearts() > 0;
}