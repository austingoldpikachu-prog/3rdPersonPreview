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
    private bool grounded;
    // private float _verticalVelocity;
    // [SerializeField] private float initialFallVelocity;
    // private CharacterController _characterController;
    // public float gravity = -9.81f;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; 
        //I'll Have to fix this one later

        // _characterController = GetComponent<CharacterController>();
    }


    // private void Update()
    // {

        // HandleGravity();

        // _isGrounded = _characterController.isGrounded;
    // }

    private void FixedUpdate()
    {
        moveInput = moveAction.action.ReadValue<Vector2>();

        Vector3 flatForward = orientation.forward;
        flatForward.y = 0;
        flatForward.Normalize();

        Vector3 flatRight = orientation.right;
        flatRight.y =0;
        flatRight.Normalize();
        inputDir = flatForward * moveInput.y + flatRight * moveInput.x; 

        HandleMovement();

        SpeedControl();
                grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        if(grounded)
            rb.linearDamping = groundDrag;
        else
        rb.linearDamping = 0;



    }

    private void HandleMovement()
    {
        rb.AddForce(inputDir.normalized * moveSpeed * 10f, ForceMode.Force);

        // Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        // float currentSpeed = moveSpeed;
        // Vector3 Move = move * currentSpeed;
        // Move.y = _verticalVelocity;

        // _characterController.Move(move);

        // CollisionFlags collisions = _characterController.Move(move);
        // if ((collisions & CollisionFlags.Above) !=0)
        // {
        //     _verticalVelocity = initialFallVelocity;
        // }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

//     private void HandleGravity()
//     {
//         if (_isGrounded && _verticalVelocity < 0)
//         {
//             _verticalVelocity = initialFallVelocity;
//         }
//         else
//         {
//             _verticalVelocity += gravity * Time.deltaTime;
//         }
//     }
}
