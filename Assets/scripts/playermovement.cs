using UnityEngine;

public class RollABallController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float jumpForce = 5f;
    public Transform cameraTransform;
    public float cameraDistance = 10f;
    public float cameraHeight = 5f;
    public float cameraSmoothness = 0.1f;
    public LayerMask wallLayer;
    public float mouseSensitivity = 2f;

    private Rigidbody rb;
    private bool isGrounded;
    private Vector3 cameraVelocity = Vector3.zero;
    private float cameraRotationX = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // WASD movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * moveSpeed);

        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // Camera Mouse Control
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        cameraRotationX -= mouseY;
        cameraRotationX = Mathf.Clamp(cameraRotationX, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(cameraRotationX, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // Camera Follow
        Vector3 targetPosition = transform.position - cameraTransform.forward * cameraDistance + Vector3.up * cameraHeight;
        cameraTransform.position = Vector3.SmoothDamp(cameraTransform.position, targetPosition, ref cameraVelocity, cameraSmoothness);

        // Handle camera wall collision
        RaycastHit hit;
        if (Physics.Linecast(transform.position, cameraTransform.position, out hit, wallLayer))
        {
            cameraTransform.position = hit.point;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
