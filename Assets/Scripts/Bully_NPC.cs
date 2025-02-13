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

    public Animator animator; 

    void Start()
    {
        Objetivo = Objetivos[Random.Range(0, Objetivos.Length)];
        animator.SetBool("isWalking", true); 
    }

    void Update()
    {
        if (PuedeVerJugador())
        {
            Debug.Log("Jugador detectado, persiguiendo...");
            AI.speed = VelocidadPersecucion;
            AI.destination = Jugador.position;
            persiguiendo = true;

            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", true);
        }
        else
        {
            if (persiguiendo)
            {
                persiguiendo = false;
                Objetivo = Objetivos[Random.Range(0, Objetivos.Length)];
            }

            AI.speed = Velocidad;
            Distancia = Vector3.Distance(transform.position, Objetivo.position);

            if (Distancia < 2)
            {
                Objetivo = Objetivos[Random.Range(0, Objetivos.Length)];
            }

            AI.destination = Objetivo.position;

            animator.SetBool("isWalking", true);
            animator.SetBool("isRunning", false);
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
}
