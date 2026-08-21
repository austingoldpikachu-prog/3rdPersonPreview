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
        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();
        Vector3 inputDir = orientation.forward * moveInput.y + orientation.right * moveInput.x; 

    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        rb.AddForce(inputDir.normalized * moveSpeed * 10f, ForceMode.Force);
    }
}
