using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;



public class BattleManager : MonoBehaviour
{
    public HealthUI playerStats; // Referencia al script del jugador.
    public Stats manaSetter;
    public CharacterStats enemyStats;  // Referencia al script del enemigo.
    public Animator animatorPlayer;
    public Animator animatorEnemy;

    public TextMeshProUGUI combatLog; // Texto para mostrar mensajes de combate.

    private bool isPlayerTurn = true; // ¿Es el turno del jugador?
    private bool noMana = false;

    public CanvasGroup exitBackgroundImageCanvasGroup;
    float m_Timer;
    public float fadeDuration = 1f;
    public float displayImageDuration = 5f;

    public AudioClip sonidoAtaqueJugador;
    public AudioClip sonidoAtaqueEnemigo;
    public AudioClip sonidoVictoria;
    public AudioClip sonidoDerrota;

    private AudioSource audioSource;

    void Start()
    {
        combatLog.text = "Comienza la batalla. ¡Es tu turno!";
        playerStats.StartCombat();
        audioSource = GetComponent<AudioSource>();

        if (playerStats.GetCurrentMana() < 20)
        {
            combatLog.text += "\n¡No tienes maná! El turno pasa al enemigo.";
            noMana = true;
            isPlayerTurn = false;
            Invoke("EnemyAttack", 1.5f);
        }
    }

    // Método llamado cuando el jugador usa una carta.
    public void PlayerAttack(int cardValue)
    {
        if (!isPlayerTurn) return; // Solo puede atacar en su turno.

        if (!playerStats.IsAlive()) {
            combatLog.text = "Estás fuera de combate y no puedes atacar.";
            EndBattle();
            return;
        }

        if (playerStats.ConsumeMana(cardValue))
        {
            PlaySound(sonidoAtaqueJugador);
            animatorPlayer.SetTrigger("attack");
            animatorEnemy.SetTrigger("getHit");
            enemyStats.TakeDamage(cardValue);
            animatorEnemy.SetTrigger("backToIdle");
            combatLog.text = $"Atacaste con {cardValue} de daño. Vida del enemigo: {enemyStats.health}";
            animatorPlayer.SetTrigger("backToIdle");

            if (!enemyStats.IsAlive())
            {
                combatLog.text += "\n¡Has ganado la batalla!";
                PlaySound(sonidoVictoria);
                EndBattle();
                return;
            }


            // Cambiar turno al enemigo.
            isPlayerTurn =  false;
            Invoke("EnemyAttack", 1.5f); // El enemigo ataca después de un pequeño retraso.
        }
        else
        {
            combatLog.text = "No tienes suficiente maná para usar esta carta.";
        }
    }

    // Método para el ataque del enemigo.
    void EnemyAttack()
    {
        if (!enemyStats.IsAlive() || !playerStats.IsAlive()) 
        {
            EndBattle();
            return;
        }

        int[] attackOptions = { 10, 20, 30, 40 };
        int damage = attackOptions[Random.Range(0, attackOptions.Length)];
        PlaySound(sonidoAtaqueEnemigo);

        animatorEnemy.SetTrigger("attack");
        animatorPlayer.SetTrigger("getHit");
        playerStats.TakeCombatDamage(damage);
        animatorPlayer.SetTrigger("backToIdle");
        animatorEnemy.SetTrigger("backToIdle");

        combatLog.text = $"El enemigo atacó con {damage} de daño. Tu vida: {playerStats.currentHealth}";

        if (!playerStats.IsAlive())
        {
            combatLog.text += "\nHas perdido la batalla.";
            PlaySound(sonidoDerrota);
            EndBattle();
            return;
        }

        if (playerStats.GetCurrentMana() < 20)
        {
            if (!noMana)
            {
                combatLog.text += "\n¡Te has quedado sin maná y no puedes atacar! El turno pasa al enemigo.";
                noMana = true;
            } else
            {
                combatLog.text += "\nNo tienes maná, turno del enemigo.";
            }
            
            isPlayerTurn = false;
            Invoke("EnemyAttack", 1.5f);
        }

        // Cambiar turno al jugador.
        isPlayerTurn =  true;
    }

    void EndBattle()
    {
        manaSetter.SetBotellas(playerStats.GetCurrentMana());
        if (!enemyStats.IsAlive())
        {
            Debug.Log("Ganaste la batalla. Cambiando de escena en 2 segundos...");
            EnemyManager.AddTestigo();
            if (exitBackgroundImageCanvasGroup == null)
            {
                StartCoroutine(DelayedSceneChange());
            } else
            {
                StartCoroutine(FadeOutAndQuit());
            }
        }
        else if (!playerStats.IsAlive())
        {
            if(!playerStats.HasHeartsLeft())
            {
                return;
            }
            Debug.Log("Perdiste la batalla. Cambiando de escena en 2 segundos...");
            StartCoroutine(DelayedSceneChange());
        }
    }

    private IEnumerator DelayedSceneChange()
    {
        yield return new WaitForSeconds(2f); // Espera 2 segundos
        ReturnToMainScene();
    }

    private IEnumerator waitForSceneChange()
    {
        yield return new WaitForSeconds(10f);
    }


    void ReturnToMainScene()
    {
        Debug.Log("Cambiando a la escena MainScene...");
        SceneManager.LoadScene("MainScene");
    }

    private IEnumerator FadeOutAndQuit()
    {
        m_Timer = 0f; // Reinicia el temporizador

        // Proceso de fade
        while (m_Timer < fadeDuration)
        {
            m_Timer += Time.deltaTime;
            float alphaValue = m_Timer / fadeDuration; // Calcula el alpha
            exitBackgroundImageCanvasGroup.alpha = alphaValue;

            yield return null; // Espera un frame
        }

        // Espera el tiempo de duración de la pantalla de Game Over
        yield return new WaitForSeconds(displayImageDuration);

        Debug.Log("Saliendo del juego...");
        SceneManager.LoadScene("MenuPrincipal"); // Cambia a la escena del menú principal
    }
     
    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

}
