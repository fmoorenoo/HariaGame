using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController characterController;
    Animator animator;

    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float gravity = 9.81f;
    private Vector3 velocity;

    private int isWalkingHash;
    private int isRunningHash;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");  // Movimiento lateral (A-D)
        float vertical = Input.GetAxis("Vertical");      // Movimiento hacia adelante/atrás (W-S)

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool isWalking = vertical != 0 || horizontal != 0;

        animator.SetBool(isWalkingHash, isWalking);
        animator.SetBool(isRunningHash, isRunning);

        float speed = isRunning ? runSpeed : walkSpeed;
        
        // Obtener dirección según la cámara
        Vector3 moveDirection = Camera.main.transform.forward * vertical + Camera.main.transform.right * horizontal;
        moveDirection.y = 0;  // No queremos que se mueva en el eje Y

        if (moveDirection.magnitude > 1)
            moveDirection.Normalize();

        // Aplicar gravedad manualmente
        if (characterController.isGrounded)
        {
            velocity.y = 0f;
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime;
        }

        // Mover al personaje
        characterController.Move((moveDirection * speed + velocity) * Time.deltaTime);

        // Hacer que el personaje mire en la dirección del movimiento
        if (moveDirection != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, moveDirection, 10f * Time.deltaTime);
        }
    }
}
