using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed = 10f;
    public Transform cameraTransform;
    public float cameraDistance = 10f;
    public float cameraHeight = 5f;
    public float cameraSmoothness = 0.1f;
    public float mouseSensitivity = 100f;

    private Rigidbody rb;
    private float rotationY = 0f;
    private float rotationX = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Ensure the cameraTransform is set in the Inspector
        if (cameraTransform == null)
        {
            Debug.LogError("Camera Transform is not assigned. Please assign it in the Inspector.");
        }
    }

    void FixedUpdate()
    {
        // Get input for both horizontal and vertical (supports WASD and Arrow keys)
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Calculate movement relative to camera
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Remove the y-component to keep movement on the XZ plane
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        // Compute movement vector
        Vector3 movement = (forward * moveVertical + right * moveHorizontal) * speed * Time.fixedDeltaTime;

        // Apply movement to Rigidbody
        rb.MovePosition(rb.position + movement);
    }

    void LateUpdate()
    {
        if (cameraTransform != null)
        {
            // Handle mouse input for camera rotation
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            // Update the rotation values
            rotationY += mouseX;
            rotationX -= mouseY;
            rotationX = Mathf.Clamp(rotationX, -30f, 60f); // Clamp vertical rotation for a comfortable view

            // Calculate camera rotation
            Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);

            // Calculate desired position based on the rotation and set the camera position and rotation
            Vector3 desiredPosition = transform.position - (rotation * Vector3.forward * cameraDistance) + Vector3.up * cameraHeight;
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, desiredPosition, cameraSmoothness);

            // Make the camera look at the ball
            cameraTransform.LookAt(transform.position);
        }
    }
}
