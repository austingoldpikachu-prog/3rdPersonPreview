using UnityEngine;
using UnityEngine.InputSystem;


public class UniversalMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public Transform orientation;
    float horizontalInput;
    float verticalInput;
    private Vector3 inputDir;
    private Vector2 moveInput;
    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = true;
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    private bool grounded;
    Vector3 moveDirection;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    public float startYScale;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;

    [Header("Input Action Refrences")]
    public InputActionReference jumpAction;
    public InputActionReference moveAction;
    public InputActionReference sprintAction;
    public InputActionReference crouchAction;

    Rigidbody rb;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; 
        moveSpeed = walkSpeed;

        startYScale = transform.localScale.y;
    }

    private void Update()
    {
    grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
    rb.linearDamping = grounded ? groundDrag :0f;

    // SlopeControl();
    }

    private void OnEnable()
    {
        jumpAction.action.performed += JumpCheck;
        sprintAction.action.performed += SprintCheck;
        sprintAction.action.canceled += SprintCheck;
        crouchAction.action.performed += CrouchCheck;
        crouchAction.action.canceled += CrouchCheck;


    }

    private void OnDisable()
    {
        jumpAction.action.performed -= JumpCheck;
        sprintAction.action.performed -= SprintCheck;
        sprintAction.action.canceled -= SprintCheck;
        crouchAction.action.performed -= CrouchCheck;
        crouchAction.action.canceled -= CrouchCheck;

    }


    private void FixedUpdate()
    {
        moveInput = moveAction.action.ReadValue<Vector2>();

        HandleMovement();
    }

    private void HandleMovement()
    {
        inputDir = orientation.forward * moveInput.y + orientation.right * moveInput.x; 

        // if(onSlope())
        // {
        //     rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

        //     if(rb.linearVelocity.y > 0)
        //         rb.AddForce(Vector3.down *80f, ForceMode.Force);
        // }

        if(grounded)
            rb.AddForce(inputDir.normalized * moveSpeed * 10f, ForceMode.Force);
        else
           rb.AddForce(inputDir.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        //    rb.useGravity = !onSlope();
    }

    // private void SlopeControl()
    // {
    //     if(onSlope())
    //     {
    //         if(rb.linearVelocity.magnitude > moveSpeed)
    //             rb.linearVelocity = rb.linearVelocity.normalized*moveSpeed; 
    //     }

    // }

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

    private void SprintCheck(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(grounded)
                state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }
        else if (context.canceled)
        {
            state = MovementState.walking;
        moveSpeed = walkSpeed;           
        }

    }

    private void CrouchCheck(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3 .down * 5f, ForceMode.Impulse);
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }
        else if(context.canceled)
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }
    }

    // private bool onSlope()
    // {
    //     if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
    //     {
    //         float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
    //         return angle < maxSlopeAngle && angle != 0;
    //     }

    //     return false;
    // }

    // private Vector3 GetSlopeMoveDirection()
    // {
    //     return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    // }
}
