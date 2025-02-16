using System.Collections;
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
    private bool cayendo = false;
    private PlayerController playerController;

    public Animator animator;
    private Animator jugadorAnimator;

    public AudioSource golpeAudio;
    public AudioSource caidaAudio; // AudioSource para el sonido de caída
    public float tiempoEsperaGolpe = 0.5f;
    public float velocidadRotacion = 8f;

    private Collider npcCollider;

    void Start()
    {
        npcCollider = GetComponent<Collider>();
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
            playerController = Jugador.GetComponent<PlayerController>();
            if (jugadorAnimator == null)
            {
                Debug.LogError("El jugador no tiene un Animator.");
            }
        }
        else
        {
            Debug.LogError("Jugador no asignado.");
        }

        if (golpeAudio == null)
        {
            golpeAudio = GetComponent<AudioSource>();
        }

        if (caidaAudio == null)
        {
            caidaAudio = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (golpeando || cayendo) return;

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

        if (Vector3.Distance(transform.position, Jugador.position) < 2f && !golpeando && !golpeIniciado)
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
        AI.isStopped = true;

        yield return StartCoroutine(MirarHaciaJugador());

        animator.SetBool("isPunching", true);

        yield return new WaitForSeconds(tiempoEsperaGolpe);

        if (golpeAudio != null)
        {
            golpeAudio.Play();
        }

        if (jugadorAnimator != null)
        {
            jugadorAnimator.SetBool("isFalling", true);
        }

        if (playerController != null)
        {
            playerController.isImmobilized = true;
        }

        yield return new WaitForSeconds(0.1f);

        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Golpear"))
        {
            yield return null;
        }

        float duracionGolpe = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(duracionGolpe);

        animator.SetBool("isPunching", false);

        if (jugadorAnimator != null)
        {
            yield return new WaitForSeconds(jugadorAnimator.GetCurrentAnimatorStateInfo(0).length);
            jugadorAnimator.SetBool("isFalling", false);
        }

        AI.isStopped = false;
        golpeando = false;
        golpeIniciado = false;
    }

    IEnumerator MirarHaciaJugador()
    {
        Vector3 direccion = (Jugador.position - transform.position).normalized;
        direccion.y = 0;
        Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);

        while (Quaternion.Angle(transform.rotation, rotacionObjetivo) > 5f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, velocidadRotacion * Time.deltaTime);
            yield return null;
        }

        transform.rotation = rotacionObjetivo;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water") && !cayendo)
        {
            cayendo = true;
            AI.isStopped = true;
            animator.SetBool("isFalling", true);

            if (caidaAudio != null)
            {
                caidaAudio.Play();
            }

            StartCoroutine(ActivarTriggerTrasCaer());
        }
    }

    IEnumerator ActivarTriggerTrasCaer()
    {
        yield return new WaitForSeconds(1f);

        if (npcCollider != null)
        {
            npcCollider.isTrigger = true;
        }
    }
}
