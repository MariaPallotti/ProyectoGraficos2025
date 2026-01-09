using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public HealthUI playerStats;
    public Stats manaSetter;
    public CharacterStats enemyStats;
    public Animator animatorPlayer;
    public Animator animatorEnemy;

    public TextMeshProUGUI combatLog;

    private bool isPlayerTurn = true;
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

        // Asegurarse de que la imagen de Game Over esté oculta al inicio
        if (exitBackgroundImageCanvasGroup != null)
        {
            exitBackgroundImageCanvasGroup.alpha = 0f;
            exitBackgroundImageCanvasGroup.gameObject.SetActive(false);
        }

        if (playerStats.GetCurrentMana() < 20)
        {
            combatLog.text += "\n¡No tienes maná! El turno pasa al enemigo.";
            noMana = true;
            isPlayerTurn = false;
            Invoke("EnemyAttack", 1.5f);
        }
    }

    public void PlayerAttack(int cardValue)
    {
        if (!isPlayerTurn) return;

        if (!playerStats.IsAlive())
        {
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
                StartCoroutine(SecuenciaVictoria());
                return;
            }

            isPlayerTurn = false;
            Invoke("EnemyAttack", 1.5f);
        }
        else
        {
            combatLog.text = "No tienes suficiente maná para usar esta carta.";
        }
    }

    IEnumerator AnimarMuerteEnemigo()
    {
        Transform enemyTransform = animatorEnemy.transform;
        Vector3 escalaOriginal = enemyTransform.localScale;
        float duracion = 0.5f;
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float porcentaje = tiempo / duracion;
            enemyTransform.localScale = Vector3.Lerp(escalaOriginal, Vector3.zero, porcentaje);
            enemyTransform.Rotate(0, 0, 360 * Time.deltaTime);
            yield return null;
        }

        enemyTransform.localScale = Vector3.zero;
    }

    IEnumerator SecuenciaVictoria()
    {
        Time.timeScale = 0.1f;
        yield return new WaitForSecondsRealtime(0.3f);
        Time.timeScale = 1f;

        PlaySound(sonidoVictoria);
        StartCoroutine(AnimarMuerteEnemigo());
        combatLog.text = "¡ENEMIGO ELIMINADO! Has ganado la batalla.";
        yield return new WaitForSeconds(1.5f);
        EndBattle();
    }

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

        combatLog.text = $"El enemigo atacó con {damage} de daño. Tu vida: {playerStats.currentHealth}";

        if (!playerStats.IsAlive())
        {
            StartCoroutine(ManejarDerrota());
            return;
        }

        animatorPlayer.SetTrigger("backToIdle");
        animatorEnemy.SetTrigger("backToIdle");

        if (playerStats.GetCurrentMana() < 20)
        {
            if (!noMana)
            {
                combatLog.text += "\n¡Te has quedado sin maná y no puedes atacar! El turno pasa al enemigo.";
                noMana = true;
            }
            else
            {
                combatLog.text += "\nNo tienes maná, turno del enemigo.";
            }

            isPlayerTurn = false;
            Invoke("EnemyAttack", 1.5f);
            return;
        }

        isPlayerTurn = true;
    }

    IEnumerator ManejarDerrota()
    {
        yield return new WaitForSeconds(1.0f);

        animatorPlayer.SetTrigger("backToIdle");
        animatorEnemy.SetTrigger("backToIdle");

        PlaySound(sonidoDerrota);
        combatLog.color = Color.red;
        combatLog.text = "Has sido derrotado...";

        yield return new WaitForSeconds(2f);

        EndBattle();
    }

    void EndBattle()
    {
        Time.timeScale = 1f;
        manaSetter.SetBotellas(playerStats.GetCurrentMana());

        if (!enemyStats.IsAlive())
        {
            Debug.Log("Ganaste la batalla. Cambiando de escena en 2 segundos...");
            EnemyManager.AddTestigo();

            if (exitBackgroundImageCanvasGroup == null)
            {
                StartCoroutine(DelayedSceneChange());
            }
            else
            {
                StartCoroutine(FadeOutAndQuit());
            }
        }
        else if (!playerStats.IsAlive())
        {
            if (!playerStats.HasHeartsLeft())
            {
                if (exitBackgroundImageCanvasGroup != null)
                {
                    StartCoroutine(MostrarGameOver());
                }
                else
                {
                    Debug.LogError("¡exitBackgroundImageCanvasGroup no está asignado!");
                    StartCoroutine(DelayedSceneChange());
                }
                return;
            }
            Debug.Log("Perdiste la batalla. Cambiando de escena en 2 segundos...");
            StartCoroutine(DelayedSceneChange());
        }
    }

    private IEnumerator DelayedSceneChange()
    {
        yield return new WaitForSeconds(2f);
        ReturnToMainScene();
    }

    void ReturnToMainScene()
    {
        Debug.Log("Cambiando a la escena MainScene...");
        SceneManager.LoadScene("MainScene");
    }

    private IEnumerator MostrarGameOver()
    {
        Debug.Log("Mostrando pantalla de Game Over...");

        exitBackgroundImageCanvasGroup.gameObject.SetActive(true);
        exitBackgroundImageCanvasGroup.alpha = 0f;
        m_Timer = 0f;

        while (m_Timer < fadeDuration)
        {
            m_Timer += Time.deltaTime;
            float alphaValue = m_Timer / fadeDuration;
            exitBackgroundImageCanvasGroup.alpha = alphaValue;
            yield return null;
        }

        exitBackgroundImageCanvasGroup.alpha = 1f;
        yield return new WaitForSeconds(displayImageDuration);

        Debug.Log("Saliendo del juego...");
        SceneManager.LoadScene("MenuPrincipal");
    }

    private IEnumerator FadeOutAndQuit()
    {
        exitBackgroundImageCanvasGroup.gameObject.SetActive(true);
        exitBackgroundImageCanvasGroup.alpha = 0f;
        m_Timer = 0f;

        while (m_Timer < fadeDuration)
        {
            m_Timer += Time.deltaTime;
            float alphaValue = m_Timer / fadeDuration;
            exitBackgroundImageCanvasGroup.alpha = alphaValue;
            yield return null;
        }

        exitBackgroundImageCanvasGroup.alpha = 1f;
        yield return new WaitForSeconds(displayImageDuration);

        Debug.Log("Saliendo del juego...");
        SceneManager.LoadScene("MenuPrincipal");
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}