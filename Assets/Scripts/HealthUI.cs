using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{

    private HeartsUI heartsUI; 
    private Stats Stats;

    // Vida en combate
    public int maxHealth = 100; // Salud máxima durante el combate
    public int currentHealth = 100; // Salud actual durante el combate
    public Image healthBar; // Barra de vida (Image con Fill)

    // Maná
    public int maxMana = 100;
    public int currentMana;
    public Image bar; // Barra de maná (Image con Fill)



    // Start is called before the first frame update
    void Start()
    {
        heartsUI = FindObjectOfType<HeartsUI>(); // Buscar automáticamente el script HeartsUI
        if (heartsUI == null)
        {
            Debug.LogError("No se encontró HeartsUI en la escena.");
        }
        Stats = FindObjectOfType<Stats>();
        if (Stats == null)
        {
            Debug.LogError("No se encontró Stats en la escena.");
        }
        currentHealth = maxHealth;
        UpdateHealthBar();

        currentMana = Stats.GetBotellas();
        maxMana = currentMana;
        UpdateManaBar();
    }

    void Update()
    {
        // Actualizar las barras de vida y mana
        UpdateHealthBar();
        UpdateManaBar();
    }

    // Método para actualizar visualmente la barra de vida
    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = (float)currentHealth / maxHealth;
        }
    }

    // Método para inicializar los stats al comienzo de un combate
    public void StartCombat()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
        currentMana = Stats.GetBotellas();
        maxMana = currentMana;
        UpdateManaBar();
    }


    // Método para recibir daño durante el combate
    public void TakeCombatDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);  // Asegúrate de que la salud no baje de 0

        if (currentHealth <= 0)
        {
            Debug.Log("El jugador ha perdido el combate.");
            if (heartsUI != null)
            {
                heartsUI.LoseHeart(); // Llamar a la función LoseHeart()
            }

            // Comprobar si el jugador aún tiene corazones restantes
            if (heartsUI != null && heartsUI.GetHearts() <= 0)
            {
                Die(); // Si no hay corazones, el jugador muere
            }
        }
        UpdateHealthBar();
    }

    void Die()
    {
        heartsUI.GameOver();
    }

    // Verificar si el personaje sigue vivo
    public bool IsAlive()
    {
        return currentHealth > 0;
    }

    // Método para actualizar visualmente la barra de maná
    private void UpdateManaBar()
    {
        if (bar != null)
        {
            bar.fillAmount = (float)currentMana / maxMana;
        }
    }

    // Método para consumir maná
    public bool ConsumeMana(int amount)
    {
        if (amount > currentMana)
        {
            return false;
        }

        currentMana -= amount;
        UpdateManaBar();
        return true;
    }

    // Método para regenerar maná
    public void RegenerateMana(int amount)
    {
        currentMana += amount;

        if (currentMana > maxMana)
        {
            currentMana = maxMana;
        }

        UpdateManaBar();
    }

    public int GetCurrentMana()
    {
        return currentMana;
    }

    public bool HasHeartsLeft()
    {
        return heartsUI.GetHearts() != 0;
    }
}
