using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class MovementsPlayer : MonoBehaviour
{
    private InputSystem_Actions controls;
    private Vector3 jumpTargetPosition;
    private Camera mainCamera;
    private float dashEndTime;
    public Vector2 directionPlayer;
    public Rigidbody rb;
    public PlayerSettings variables;
    public LayerMask obstacleLayer;
    public float obstacleDetectionDistance = 1.5f;

    public bool isRunning;
    public bool isJumping = false;
    public bool isDashing = false;
    private float dashTimeElapsed; // Nueva variable para el tiempo de dash
    private Vector3 dashStartPosition; // Posici�n inicial del dash
    private Vector3 dashTargetPosition; // Posici�n objetivo del dash

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
        controls.InputsPlayer.Dash.started += StartDash;
    }

    private void OnDisable()

    {
        controls.Disable();
        controls.InputsPlayer.Jump.started -= Jump;
        controls.InputsPlayer.Sprint.started -= StartRunning;
        controls.InputsPlayer.Sprint.canceled -= StopRunning;
        controls.InputsPlayer.Dash.started -= StartDash;
    }

    private void Update()
    {
        directionPlayer = controls.InputsPlayer.Move.ReadValue<Vector2>();

        if (isDashing)
        {
            dashTimeElapsed += Time.deltaTime;
            float dashProgress = dashTimeElapsed / variables.dashDuration;

            // Interpolaci�n de posici�n
            rb.position = Vector3.Lerp(dashStartPosition, dashTargetPosition, dashProgress);

            // Finalizaci�n del dash
            if (dashTimeElapsed >= variables.dashDuration)
            {
                isDashing = false;
            }
        }
    }

    private void FixedUpdate()
    {

        if (!isDashing) // Solo permitir movimiento normal si no se est� haciendo dash
        {
            Vector3 movement = new Vector3(directionPlayer.x, 0, directionPlayer.y);

            Vector3 camForward = mainCamera.transform.forward;
            Vector3 camRight = mainCamera.transform.right;

            camForward.y = 0;
            camRight.y = 0;

            camForward.Normalize();
            camRight.Normalize();

            Vector3 movementRelativeToCamera = (camForward * movement.z + camRight * movement.x).normalized;

            float currentSpeed = isRunning ? variables.speedSprint : variables.speed;
            Walk(movementRelativeToCamera, currentSpeed);

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
    }


    private void Walk(Vector3 movementFinal, float speed)
    {
        if(!isJumping && movementFinal != Vector3.zero)
    {
            RaycastHit hit;
            if (!rb.SweepTest(movementFinal, out hit, speed * Time.deltaTime))
            {
                rb.MovePosition(rb.position + Time.deltaTime * speed * movementFinal);
            }
            else
            {
                isRunning = false;
            }
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

    private void StartDash(InputAction.CallbackContext context)
    {
        if (!isDashing)
        {
            isDashing = true;
            dashTimeElapsed = 0f; 
            dashStartPosition = rb.position;
            dashTargetPosition = rb.position + transform.forward * variables.dashDistance; 
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position - transform.forward * obstacleDetectionDistance);
    }


}