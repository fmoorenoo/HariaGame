using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController characterController;
    Animator animator;
    AudioSource audioSource;

    public AudioClip walkSound;
    public AudioClip runSound;

    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float gravity = 9.81f;
    private Vector3 velocity;

    [Range(0f, 1f)] public float walkVolume = 0.7f;  // Volumen al caminar
    [Range(0f, 1f)] public float runVolume = 1f;    // Volumen al correr

    [Range(0.5f, 2f)] public float walkPitch = 1f;  // Velocidad del sonido al caminar
    [Range(0.5f, 2f)] public float runPitch = 1.2f; // Velocidad del sonido al correr

    private int isWalkingHash;
    private int isRunningHash;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");

        audioSource.loop = true; // Asegurar que el sonido de pasos sea en bucle
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetKey(KeyCode.W) ? 1f : 0f;

        bool isRunning = Input.GetKey(KeyCode.LeftShift) && vertical > 0;
        bool isWalking = vertical != 0 || horizontal != 0;

        animator.SetBool(isWalkingHash, isWalking);
        animator.SetBool(isRunningHash, isRunning);

        float speed = isRunning ? runSpeed : walkSpeed;

        Vector3 moveDirection = Camera.main.transform.forward * vertical + Camera.main.transform.right * horizontal;
        moveDirection.y = 0;

        if (moveDirection.magnitude > 1)
            moveDirection.Normalize();

        if (characterController.isGrounded)
        {
            velocity.y = 0f;
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime;
        }

        characterController.Move((moveDirection * speed + velocity) * Time.deltaTime);

        if (moveDirection != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, moveDirection, 10f * Time.deltaTime);
        }

        HandleFootstepSounds(isWalking, isRunning);
    }

    void HandleFootstepSounds(bool isWalking, bool isRunning)
    {
        if (isRunning)
        {
            if (audioSource.clip != runSound || !audioSource.isPlaying)
            {
                audioSource.clip = runSound;
                audioSource.volume = runVolume;
                audioSource.pitch = runPitch;
                audioSource.Play();
            }
        }
        else if (isWalking)
        {
            if (audioSource.clip != walkSound || !audioSource.isPlaying)
            {
                audioSource.clip = walkSound;
                audioSource.volume = walkVolume;
                audioSource.pitch = walkPitch;
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}
