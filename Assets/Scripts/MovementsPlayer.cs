using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class MovementsPlayer : MonoBehaviour
{
    private InputSystem_Actions controls;

    public Vector2 directionPlayer;

    public Rigidbody rb;

    public float speed;
    public float jumpingForce;
    private void Awake()
    {
        controls = new();
    }

    private void OnEnable()
    {
        controls.Enable();
        controls.InputsPlayer.Jump.started += Jump;
        controls.InputsPlayer.Jump.performed += Jump;
        controls.InputsPlayer.Jump.canceled += Jump;
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
        rb.MovePosition(rb.position + Time.deltaTime * speed * new Vector3(directionPlayer.x , 0 , directionPlayer.y));
    }

    private void Jump(InputAction.CallbackContext context)
    {
        rb.AddForce(new Vector3(0, jumpingForce));
    }


}
