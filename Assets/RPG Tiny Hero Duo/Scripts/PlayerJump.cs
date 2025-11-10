using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    // Variables para el salto
    public float jumpForce = 5f; // Fuerza de salto normal
    public float thrust = 10;

    // Variables para verificar si el personaje está en el suelo
    bool isGrounded;
    public Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isGrounded = true;
    }

    void Update()
    {
        // Detectar el salto
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true)
        {
            Jump();
        }
    }

    void Jump()
    {
        isGrounded = false;
        rb.AddForce(0, thrust, 0, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
