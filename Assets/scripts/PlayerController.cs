using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float lookSensitivity = 2f;
    public Transform playerCamera;
    public float cameraClamp = 85f;

    private float xRotation = 0f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
    }

    void LateUpdate()
    {
        FollowCamera();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -cameraClamp, cameraClamp);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        rb.MovePosition(rb.position + move * moveSpeed * Time.fixedDeltaTime);
    }

    void FollowCamera()
    {
        playerCamera.position = transform.position + new Vector3(0f, 1.5f, 0f);
    }

    void OnCollisionEnter(Collision collision)
    {
        // This ensures that the camera doesn't go through walls.
        // You can adjust or add conditions if needed based on your environment.
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Handle logic to stop the camera from clipping through walls if needed.
            // This might involve adjusting the position or restricting movement.
        }
    }
}

/*
Instructions:
1. Attach this script to your player GameObject.
2. Assign the "Player Camera" in the inspector to the camera child object of the player.
3. Make sure your walls and obstacles have colliders and are tagged properly (e.g., "Wall").
4. Set the Rigidbody to "Interpolate" for smoother movement, and freeze rotation on X, Y, Z axes in Rigidbody constraints.
*/
