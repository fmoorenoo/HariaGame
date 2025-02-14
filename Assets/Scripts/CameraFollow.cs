using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; 
    public Vector3 offset = new Vector3(0, 3, -5); 
    public float smoothSpeed = 5f; 
    public float lookAtHeightOffset = 1.5f; 
    public LayerMask obstacleMask; 
    public float minDistance = 1.0f; 

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + target.TransformDirection(offset);
        Vector3 direction = desiredPosition - target.position;
        float distance = direction.magnitude;
        direction.Normalize();

        RaycastHit hit;
        if (Physics.Raycast(target.position, direction, out hit, distance, obstacleMask))
        {
            desiredPosition = hit.point - direction * 0.2f; 
        }

        if (Vector3.Distance(desiredPosition, target.position) < minDistance)
        {
            desiredPosition = target.position + direction * minDistance;
        }

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        float extraLookUp = (target.position.y < -10) ? 1.0f : 0f; 

        Vector3 lookAtTarget = target.position + Vector3.up * (lookAtHeightOffset + extraLookUp);
        transform.LookAt(lookAtTarget);
    }
}
