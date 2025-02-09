using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; 
    public Vector3 offset = new Vector3(0, 3, -5); 
    public float smoothSpeed = 5f; 
    public float lookAtHeightOffset = 1.5f; 

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + target.TransformDirection(offset);

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        Vector3 lookAtTarget = target.position + Vector3.up * lookAtHeightOffset;

        transform.LookAt(lookAtTarget);
    }
}
