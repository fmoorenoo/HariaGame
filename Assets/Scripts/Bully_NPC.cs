using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bully_NPC : MonoBehaviour
{
    public NavMeshAgent AI;
    public float Velocidad;
    public float VelocidadPersecucion;
    public Transform[] Objetivos;
    private Transform Objetivo;
    public float Distancia;
    public Transform Jugador;
    public float RangoVision = 10f;
    public float AnguloVision = 60f;
    private bool persiguiendo = false;
    private bool golpeando = false;
    private bool golpeIniciado = false;

    public Animator animator;
    private Animator jugadorAnimator;

    void Start()
    {
        if (Objetivos.Length > 0)
        {
            Objetivo = Objetivos[Random.Range(0, Objetivos.Length)];
        }
        else
        {
            Debug.LogError("No hay objetivos asignados.");
        }

        if (animator != null)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            Debug.LogError("Animator no asignado.");
        }

        if (Jugador != null)
        {
            jugadorAnimator = Jugador.GetComponent<Animator>();
            if (jugadorAnimator == null)
            {
                Debug.LogError("El jugador no tiene un Animator.");
            }
        }
        else
        {
            Debug.LogError("Jugador no asignado.");
        }
    }

    void Update()
    {
        if (golpeando) return;  // No hacer nada si está golpeando

        if (PuedeVerJugador())
        {
            AI.speed = VelocidadPersecucion;
            AI.destination = Jugador.position;
            persiguiendo = true;

            if (animator != null)
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", true);
            }
        }
        else
        {
            if (persiguiendo)
            {
                persiguiendo = false;
                if (Objetivos.Length > 0)
                {
                    Objetivo = Objetivos[Random.Range(0, Objetivos.Length)];
                }
                else
                {
                    Debug.LogError("No hay objetivos asignados.");
                }
            }

            AI.speed = Velocidad;
            Distancia = Vector3.Distance(transform.position, Objetivo.position);

            if (Distancia < 2)
            {
                if (Objetivos.Length > 0)
                {
                    Objetivo = Objetivos[Random.Range(0, Objetivos.Length)];
                }
                else
                {
                    Debug.LogError("No hay objetivos asignados.");
                }
            }

            AI.destination = Objetivo.position;

            if (animator != null)
            {
                animator.SetBool("isWalking", true);
                animator.SetBool("isRunning", false);
            }
        }

        if (Vector3.Distance(transform.position, Jugador.position) < 3f && !golpeando && !golpeIniciado)
        {
            golpeIniciado = true;
            StartCoroutine(GolpearJugador());
        }
    }

    bool PuedeVerJugador()
    {
        Vector3 direccionJugador = (Jugador.position - transform.position).normalized;
        float distanciaJugador = Vector3.Distance(transform.position, Jugador.position);

        if (distanciaJugador < RangoVision)
        {
            float angulo = Vector3.Angle(transform.forward, direccionJugador);

            if (angulo < AnguloVision / 2)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position + Vector3.up * 1.5f, direccionJugador, out hit, RangoVision))
                {
                    if (hit.transform == Jugador)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    IEnumerator GolpearJugador()
    {
        golpeando = true;
        AI.isStopped = true; // Detiene al NPC
        animator.SetBool("isPunching", true);

        // 🔹 Activar animación de caída en el jugador
        if (jugadorAnimator != null)
        {
            jugadorAnimator.SetBool("isFalling", true);
        }

        // 🔹 Esperar a que la animación de golpe realmente inicie
        yield return new WaitForSeconds(0.1f);

        // 🔹 Asegurar que la animación cambió antes de obtener su duración
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Golpear"))
        {
            yield return null; // Esperar un frame
        }

        // 🔹 Obtener duración correcta de la animación de golpe
        float duracionGolpe = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(duracionGolpe);

        // 🔹 Desactivar animación de golpear en NPC
        animator.SetBool("isPunching", false);

        // 🔹 Esperar duración de animación de caída del jugador antes de permitir que se levante
        if (jugadorAnimator != null)
        {
            yield return new WaitForSeconds(jugadorAnimator.GetCurrentAnimatorStateInfo(0).length);
            jugadorAnimator.SetBool("isFalling", false);
        }

        // 🔹 Reactivar movimiento del NPC
        AI.isStopped = false;
        golpeando = false;
        golpeIniciado = false;
    }
}
