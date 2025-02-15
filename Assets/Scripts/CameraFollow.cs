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
    public float immobilizedLookAtOffset = 0.5f; 
    public float immobilizedDistanceOffset = 2f; 
    private PlayerController playerController;

    void Start()
    {
        if (target != null)
        {
            playerController = target.GetComponent<PlayerController>();
        }
    }

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

        if (DoorInteraction.isTeleported)
        {
            transform.position = desiredPosition;
            DoorInteraction.isTeleported = false;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        }

        if (playerController != null && playerController.isImmobilized)
        {
            Vector3 immobilizedPosition = target.position - target.forward * immobilizedDistanceOffset + Vector3.down * immobilizedLookAtOffset;
            transform.position = Vector3.Lerp(transform.position, immobilizedPosition, smoothSpeed * Time.deltaTime);
            transform.LookAt(target.position + Vector3.down * immobilizedLookAtOffset);
        }
        else
        {
            float extraLookUp = (target.position.y < -10) ? 1.0f : 0f;
            Vector3 lookAtTarget = target.position + Vector3.up * (lookAtHeightOffset + extraLookUp);
            transform.LookAt(lookAtTarget);
        }
    }
}