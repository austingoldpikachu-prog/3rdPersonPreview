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

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = true;
    public InputActionReference jumpAction;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    private bool grounded;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; 
    }

    private void Update()
    {
    grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
    rb.linearDamping = grounded ? groundDrag :0f;
    }

    private void OnEnable()
    {
        jumpAction.action.performed += JumpCheck;
    }

    private void OnDisable()
    {
        jumpAction.action.performed -= JumpCheck;
    }


    private void FixedUpdate()
    {
        moveInput = moveAction.action.ReadValue<Vector2>();

        HandleMovement();
    }

    private void HandleMovement()
    {
        inputDir = orientation.forward * moveInput.y + orientation.right * moveInput.x; 

        if(grounded)
            rb.AddForce(inputDir.normalized * moveSpeed * 10f, ForceMode.Force);
        else
           rb.AddForce(inputDir.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector3 (rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }    

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void JumpCheck(InputAction.CallbackContext context)
    {
        
    if(grounded)
        rb.linearDamping = groundDrag;
    else
        rb.linearDamping = 0;
    
        if (readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }
}
