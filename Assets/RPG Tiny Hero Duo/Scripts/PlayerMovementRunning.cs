using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementRunning : MonoBehaviour
{
    public float turnSpeed = 20f;
    public float walkSpeed = 5f;  // Velocidad de caminar
    public float runSpeed = 10f;   // Velocidad de correr

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Verificar si la tecla Shift está presionada
        bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;

        // Actualizar las animaciones
        m_Animator.SetBool("IsWalking", isWalking);
        m_Animator.SetBool("IsRunning", isRunning);

        // Determinar la velocidad basada en el estado
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        // Mover el personaje
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);

        // Actualizar la posición del Rigidbody
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * currentSpeed * Time.deltaTime);
        m_Rigidbody.MoveRotation(m_Rotation);
    }

    void OnAnimatorMove()
    {
        // Esta función puede no ser necesaria para el movimiento si ya estás usando MovePosition
    }
}
