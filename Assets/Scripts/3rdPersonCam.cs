using UnityEngine;
using UnityEngine.InputSystem;


public class NewMonoBehaviourScript : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;
    //use Unity to recieve information on certain objects

    public InputActionReference moveAction;
    //Unity new input system action for moving
   

    public float rotationSpeed;

    // public Transform combatLookAt;

    // public CameraStyle currentStyle;

    // public enum CameraStyle
    // {
    //     Basic,
    //     Combat
    // }

    public UnityEngine.Vector3 oldPos = default(UnityEngine.Vector3);

    private void Update()
    {

        //rotate orientation witch is an empty object parented to the player object
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        // if(currentStyle == CameraStyle.Basic)
        {
            Vector2 moveInput = moveAction.action.ReadValue<Vector2>();
            Vector3 inputDir = orientation.forward *moveInput.y + orientation.right * moveInput.x;

            if (inputDir != Vector3.zero)
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        // }
        // else if(currentStyle == CameraStyle.Combat)
        // {
        //     Vector3 dirToCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
        // orientation.forward = dirToCombatLookAt.normalized;

        //  playerObj.forward = dirToCombatLookAt.normalized;
        }
        
    }
}
