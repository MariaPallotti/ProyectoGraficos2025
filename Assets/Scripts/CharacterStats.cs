using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CharacterStats : MonoBehaviour
{
    public int health;  // Vida del enemigo

    // Referencia al Slider (barra de vida)
    public Slider healthBar;

    void Start()
    {
        healthBar.maxValue = health;
    }

    // Método para reducir vida
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health < 0) health = 0; // Asegurarse que la vida no sea negativa

        // Actualizar la barra de vida
        UpdateHealthBar();
    }

    // Método para actualizar la barra de vida
    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            // Establecer el valor de la barra de vida en función de la vida actual
            healthBar.value = health;
        }
    }

    // Verificar si el personaje sigue vivo
    public bool IsAlive()
    {
        return health > 0;
    }

}
