using UnityEngine;
#pragma warning disable
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 6.0f;
    [SerializeField] private float jumpSpeed = 8.0f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float mouseSens = 1f;

    [SerializeField] private float groundDistance = .4f;

    [SerializeField] private Transform playerCamera;
    [SerializeField] private CharacterController controller;

    private Vector3 velocity;
    private bool isGrounded;

    private void Start() 
    {
        Cursor.visible = false;
    }

    void Update()
    {
        transform.Rotate(0, Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime, 0);
        playerCamera.Rotate(-Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime, 0, 0);

        isGrounded = controller.isGrounded;

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;

        controller.Move(moveDirection * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        
        controller.Move(velocity * Time.deltaTime);
    }
}