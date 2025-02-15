using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public NavMeshAgent AI;
    public float Velocidad;
    public Transform[] Objetivos;
    private Transform Objetivo;
    public float Distancia;

    void Start()
    {
        if (Objetivos.Length > 0)
        {
            Objetivo = Objetivos[Random.Range(0, Objetivos.Length)];

            
            transform.position = Objetivo.position;
        }
        else
        {
            Debug.LogError("No hay objetivos asignados.");
        }
    }

    void Update()
    {
        Distancia = Vector3.Distance(transform.position, Objetivo.position);

        if (Distancia < 2)
        {
            Objetivo = Objetivos[Random.Range(0, Objetivos.Length)];
        }

        AI.destination = Objetivo.position;
        AI.speed = Velocidad;
    }
}
