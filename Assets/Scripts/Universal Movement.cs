using UnityEngine;
using UnityEngine.InputSystem;


public class UniversalMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    public InputActionReference moveAction;

    private Vector3 inputDir;
    private Vector2 moveInput;

    public float groundDrag;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; 
        //I'll Have to fix this one later
    }


    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        if(grounded)
            rb.linearDamping = groundDrag;
        else
        rb.linearDamping = 0;
    }

    private void FixedUpdate()
    {
        moveInput = moveAction.action.ReadValue<Vector2>();
        inputDir = orientation.forward * moveInput.y + orientation.right * moveInput.x; 

        HandleMovement();


    }

    private void HandleMovement()
    {
        rb.AddForce(inputDir.normalized * moveSpeed * 10f, ForceMode.Force);
    }
}
