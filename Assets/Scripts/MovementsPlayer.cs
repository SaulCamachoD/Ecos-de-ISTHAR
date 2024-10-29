using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class MovementsPlayer : MonoBehaviour
{
    private InputSystem_Actions controls;

    public Vector2 directionPlayer;
    public bool isRunning;

    public Rigidbody rb;

    public PlayerSettings variables;
    private void Awake()
    {
        controls = new();

    }

    private void OnEnable()
    {
        controls.Enable();
        controls.InputsPlayer.Jump.started += Jump;
        controls.InputsPlayer.Sprint.started += StartRunning;
        controls.InputsPlayer.Sprint.canceled += StopRunning;
    }
    
    private void OnDisable()

    {
        controls.Disable();
        controls.InputsPlayer.Jump.started -= Jump;
        controls.InputsPlayer.Jump.performed -= Jump;
        controls.InputsPlayer.Jump.canceled -= Jump;
    }
    // Update is called once per frame
    private void Update()
    {
        directionPlayer = controls.InputsPlayer.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(directionPlayer.x, 0, directionPlayer.y);

        if (!isRunning)
        {
            Walk(movement, variables.speed);
        }
        else 
        {
            Walk(movement, variables.speedSprint);
        }

        

        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * variables.rotationSpeed);
        }
    }

    private void Jump(InputAction.CallbackContext context)
    {
        rb.AddForce(new Vector3(0, variables.jumpingForce));
    }

    private void Walk(Vector3 movementFinal, float speed)
    {
        rb.MovePosition(rb.position + Time.deltaTime * speed * movementFinal);
    }

    private void StartRunning(InputAction.CallbackContext context)
    {
        isRunning = true;
    }

    private void StopRunning(InputAction.CallbackContext context)
    {
        isRunning = false;
    }

}
