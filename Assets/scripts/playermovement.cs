using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollABallMovement : MonoBehaviour
{
    public float speed = 10f;
    public float rotationSpeed = 100f;
    public Camera playerCamera;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (playerCamera == null)
        {
            Debug.LogError("Player Camera is not assigned.");
        }
    }

    void Update()
    {
        RotatePlayer();
        MoveForward();
        UpdateCameraPosition();
    }

    void RotatePlayer()
    {
        float mouseX = Input.GetAxis("Mouse X");
        Vector3 rotation = Vector3.up * mouseX * rotationSpeed * Time.deltaTime;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
    }

    void MoveForward()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 movement = transform.forward * speed * Time.deltaTime;
            rb.MovePosition(rb.position + movement);
        }
    }

    void UpdateCameraPosition()
    {
        if (playerCamera != null)
        {
            playerCamera.transform.position = transform.position - transform.forward * 5f + Vector3.up * 3f;
            playerCamera.transform.LookAt(transform.position);
        }
    }
}
