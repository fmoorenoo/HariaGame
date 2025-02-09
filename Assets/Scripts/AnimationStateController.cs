using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    Animator animator;
    CharacterController characterController;

    int isWalkingHash;
    int isRunningHash;

    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float gravity = 9.81f;  // Para simular caída
    private Vector3 velocity;  // Almacena la velocidad vertical

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
    }

    void Update()
    {
        bool isRunning = animator.GetBool(isRunningHash);
        bool isWalking = animator.GetBool(isWalkingHash);
        bool forwardPressed = Input.GetKey("w");
        bool runPressed = Input.GetKey("left shift");

        // Cambiar animaciones
        if (!isWalking && forwardPressed)
        {
            animator.SetBool(isWalkingHash, true);
        }
        if (isWalking && !forwardPressed)
        {
            animator.SetBool(isWalkingHash, false);
        }
        if (!isRunning && (forwardPressed && runPressed))
        {
            animator.SetBool(isRunningHash, true);
        }
        if (isRunning && (!forwardPressed || !runPressed))
        {
            animator.SetBool(isRunningHash, false);
        }

        // Aplicar movimiento
        float currentSpeed = 0f;
        if (isRunning) currentSpeed = runSpeed;
        else if (isWalking) currentSpeed = walkSpeed;

        Vector3 moveDirection = transform.forward * currentSpeed;

        // Aplicar gravedad manualmente
        if (characterController.isGrounded)
        {
            velocity.y = 0f;  // Reiniciar velocidad en el suelo
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime; // Simular caída
        }

        // Mover al personaje
        characterController.Move((moveDirection + velocity) * Time.deltaTime);
    }
}
