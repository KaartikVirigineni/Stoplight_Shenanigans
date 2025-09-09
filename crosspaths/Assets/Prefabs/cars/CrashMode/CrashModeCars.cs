using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashModeCars : MonoBehaviour
{
    public string targetTag = "YourTargetTag"; // Specify the target tag here
    public Vector3 raycastDirection = Vector3.forward; // Direction of the raycast
    public Vector3 moveDirection = Vector3.forward; // Direction of movement
    public float speed = 5f;
    public LayerMask objectDetectionLayer; // Layer to detect objects in front of the car
    public float raycastDistance = 2f; // Distance for the raycast
    public float redLightStopDuration = 5f; // Duration to wait at red light before moving forward

    private bool isInTrigger = false; // Flag to check if the car is in the trigger
    private bool isObjectInFront = false; // Flag to check if an object is in front of the car
    private float redLightTimer = 0f; // Timer to count how long the car has been at the red light

    private void Update()
    {
        // Check if there is an object in front of the car
        isObjectInFront = IsObjectInFront();

        // Check the traffic light
        RaycastHit hit;
        int layerMask = 1 << LayerMask.NameToLayer("interactable"); // Use the "Interactable" layer

        if (Physics.Raycast(transform.position, raycastDirection, out hit, Mathf.Infinity, layerMask))
        {
            // Check if the hit object has the specified tag
            if (hit.collider.CompareTag(targetTag))
            {
                // Check the color of the hit object's renderer material
                Renderer rend = hit.collider.GetComponent<Renderer>();
                if (rend != null)
                {
                    if (rend.material.color == Color.green)
                    {
                        // Object is green, always move forward if in trigger or not
                        MoveForward();
                        redLightTimer = 0f; // Reset timer when moving forward
                    }
                    else if (rend.material.color == Color.red)
                    {
                        if (isInTrigger)
                        {
                            // Object is red and in the trigger
                            StopMovement();
                            // Increment the red light timer
                            redLightTimer += Time.deltaTime;

                            if (redLightTimer >= redLightStopDuration)
                            {
                                // Timer exceeded the duration, ignore the trigger and move forward
                                MoveForward();
                                redLightTimer = 0f; // Reset timer
                                isInTrigger = false; // Ignore spawn trigger
                            }
                        }
                    }
                }
            }
        }
        
        // Stop movement if an object is detected in front
        if (isObjectInFront)
        {
            StopMovement();
        }
        else if (!isInTrigger && !isObjectInFront)
        {
            // If not in trigger and no object in front, move forward
            MoveForward();
        }
    }

    private bool IsObjectInFront()
    {
        // Shoot a raycast in the specified direction for object detection
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f; // Adjust the ray origin if needed

        // Visualize the object detection raycast
        Debug.DrawRay(rayOrigin, raycastDirection * raycastDistance, Color.red);

        return Physics.Raycast(rayOrigin, raycastDirection, out hit, raycastDistance, objectDetectionLayer);
    }

    private void MoveForward()
    {
        // Move in the specified direction
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = moveDirection.normalized * speed;
    }

    private void StopMovement()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        // Set the velocity to zero to stop the object's movement
        rb.velocity = Vector3.zero;

        // Optionally, you can also set the angular velocity to zero
        rb.angularVelocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the trigger collider is the spawner's trigger
        if (other.CompareTag("SpawnerTrigger"))
        {
            isInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the trigger collider is the spawner's trigger
        if (other.CompareTag("SpawnerTrigger"))
        {
            isInTrigger = false;
        }
}
}
