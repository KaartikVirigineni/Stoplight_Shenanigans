using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public string targetTag = "Target";
    public Vector3 raycastDirection = Vector3.forward;
    public Vector3 moveDirection = Vector3.forward;
    public float speed = 5f;
    public LayerMask objectDetectionLayer;
    public float raycastDistance = 2f;
    public float redLightStopDuration = 5f;
    private bool isInTrigger = false;
    private bool isObjectInFront = false;
    private float redLightTimer = 0f;

    private void Update()
    {
        isObjectInFront = IsObjectInFront();
        RaycastHit hit;
        int layerMask = 1 << LayerMask.NameToLayer("interactable");

        if (Physics.Raycast(transform.position, raycastDirection, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.collider.CompareTag(targetTag))
            {
                Renderer rend = hit.collider.GetComponent<Renderer>();
                if (rend != null)
                {
                    if (rend.material.color == Color.green)
                    {
                        MoveForward();
                        redLightTimer = 0f;
                    }
                    else if (rend.material.color == Color.red)
                    {
                        if (isInTrigger)
                        {
                            StopMovement();
                            redLightTimer += Time.deltaTime;
                            if (redLightTimer >= redLightStopDuration)
                            {
                                MoveForward();
                                redLightTimer = 0f;
                                isInTrigger = false;
                            }
                        }
                    }
                }
            }
        }
        
        if (isObjectInFront)
        {
            StopMovement();
        }
        else if (!isInTrigger && !isObjectInFront)
        {
            MoveForward();
        }
    }

    private bool IsObjectInFront()
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f; // Adjust the ray origin if needed
        Debug.DrawRay(rayOrigin, raycastDirection * raycastDistance, Color.red);
        return Physics.Raycast(rayOrigin, raycastDirection, out hit, raycastDistance, objectDetectionLayer);
    }

    private void MoveForward()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = moveDirection.normalized * speed;
    }

    private void StopMovement()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpawnerTrigger"))
        {
            isInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SpawnerTrigger"))
        {
            isInTrigger = false;
        }
    }
}
