using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed = 10f;
    public Transform cameraTransform;
    public float cameraDistance = 5f;
    public float cameraHeight = 2f;
    public float mouseSensitivity = 200f;
    public float jumpForce = 5f;
    public float movementSmoothness = 0.1f; // Value between 0 and 1 for smoothness

    private Rigidbody rb;
    private float rotationY = 0f;
    private float rotationX = 0f;
    private bool isGrounded = true;

    public LayerMask groundLayer;
    public float groundCheckDistance = 1.1f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Ensure the cameraTransform is set in the Inspector
        if (cameraTransform == null)
        {
            Debug.LogError("Camera Transform is not assigned. Please assign it in the Inspector.");
        }

        // Lock the cursor for FPS-like control
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Handle jump input
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; // Temporarily set false to avoid multiple jumps
        }

        // Handle mouse input for camera rotation
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Update the rotation values
        rotationY += mouseX;
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -45f, 45f); // Clamp vertical rotation for a comfortable view
    }

    void FixedUpdate()
    {
        // Ground Check using Raycast
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);

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
        Vector3 movement = (forward * moveVertical + right * moveHorizontal).normalized * speed;

        // Smooth the transition of velocity
        Vector3 targetVelocity = new Vector3(movement.x, rb.velocity.y, movement.z);
        rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, movementSmoothness);

        // Update camera position to follow the ball directly, without smoothing
        UpdateCamera();
    }

    void UpdateCamera()
    {
        if (cameraTransform != null)
        {
            // Calculate desired position based on the rotation and set the camera position
            Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);
            Vector3 desiredPosition = transform.position - (rotation * Vector3.forward * cameraDistance) + Vector3.up * cameraHeight;

            // Directly set the camera position for instant response
            cameraTransform.position = desiredPosition;

            // Make the camera look at the ball
            cameraTransform.LookAt(transform.position + Vector3.up * 0.5f); // Look at a slightly higher point for better view
        }
    }

    // Check if the ball is grounded using collision detection
    private void OnCollisionEnter(Collision collision)
    {
        // Check if we have collided with anything on the ground layer
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = true;
        }
    }
}
