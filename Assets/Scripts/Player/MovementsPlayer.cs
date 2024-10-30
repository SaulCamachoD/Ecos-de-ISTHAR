using Unity.VisualScripting;
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
    public LayerMask obstacleLayer;
    public float obstacleDetectionDistance = 1.5f;
    private Camera mainCamera;

    public bool isJumping = false;
    private Vector3 jumpTargetPosition;

    private void Awake()
    {
        controls = new();
        mainCamera = Camera.main;

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
        controls.InputsPlayer.Sprint.started -= StartRunning;
        controls.InputsPlayer.Sprint.canceled -= StopRunning;
    }

    private void Update()
    {
        directionPlayer = controls.InputsPlayer.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {

        Vector3 movement = new Vector3(directionPlayer.x, 0, directionPlayer.y);

        Vector3 camForward = mainCamera.transform.forward;
        Vector3 camRight = mainCamera.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 movementRelativeToCamera = (camForward * movement.z + camRight * movement.x).normalized;

        Walk(movementRelativeToCamera, isRunning ? variables.speedSprint : variables.speed);

        if (movementRelativeToCamera != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementRelativeToCamera);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * variables.rotationSpeed);
        }

        if (isJumping)
        {
            MoveTowardsJumpTarget();
        }
    }


    private void Walk(Vector3 movementFinal, float speed)
    {
        if (!isJumping)
        {
            rb.MovePosition(rb.position + Time.deltaTime * speed * movementFinal);
        }
    }

    private void StartRunning(InputAction.CallbackContext context)
    {
        isRunning = true;
    }

    private void StopRunning(InputAction.CallbackContext context)
    {
        isRunning = false;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (isJumping) return;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, obstacleDetectionDistance, obstacleLayer))
        {
            Obstacle obstacle = hit.collider.GetComponent<Obstacle>();
            if (obstacle != null)
            {
                obstacle.SetTrigger(true);

                isJumping = true;
                rb.AddForce(Vector3.up * variables.jumpForce, ForceMode.Impulse);
                jumpTargetPosition = hit.point + hit.normal * -1.5f;
            }
        }
    }

    private void MoveTowardsJumpTarget()
    {
        rb.position = Vector3.MoveTowards(rb.position, jumpTargetPosition, Time.deltaTime * variables.speed);
        if (Vector3.Distance(rb.position, jumpTargetPosition) < 0.1f)
        {
            isJumping = false;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.forward, out hit, obstacleDetectionDistance, obstacleLayer))
            {
                Obstacle obstacle = hit.collider.GetComponent<Obstacle>();
                if (obstacle != null)
                {
                    obstacle.ResetTrigger();
                }
            }
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position - transform.forward * obstacleDetectionDistance);
    }


}