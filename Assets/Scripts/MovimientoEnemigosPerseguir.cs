using UnityEngine;
using UnityEngine.AI;

public class MovimientoEnemigosPerseguir : MonoBehaviour
{
    public Transform player; // Referencia al jugador
    public float detectionRange = 10f; // Rango para detectar al jugador

    private NavMeshAgent navMeshAgent; // Referencia al componente NavMeshAgent
    private Animator animator; // Referencia al componente Animator
    private bool isChasing = false;

    void Start()
    {
        // Obtener referencias a los componentes necesarios
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Calcula la distancia entre el enemigo y el jugador
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            isChasing = true;
        }
        else
        {
            isChasing = false;
        }

        if (isChasing)
        {
            // Activar animación de correr y perseguir al jugador
            animator.SetBool("isRunning", true);
            navMeshAgent.SetDestination(player.position);
        }
        else
        {
            // Desactivar animación de correr y detener movimiento
            animator.SetBool("isRunning", false);
            navMeshAgent.ResetPath(); // Detiene al agente
        }
    }

    // Visualiza el rango de detección en la escena
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
