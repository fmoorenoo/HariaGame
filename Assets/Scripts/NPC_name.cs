using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_name : MonoBehaviour
{
    public Transform playerCamera;
    
    void LateUpdate()
    {
        transform.LookAt(transform.position + playerCamera.forward);
    }
}
