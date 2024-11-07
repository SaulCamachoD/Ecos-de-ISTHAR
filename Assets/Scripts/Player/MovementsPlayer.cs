using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class MovementsPlayer : MonoBehaviour
{
    private InputSystem_Actions controls;
    private Vector3 jumpTargetPosition;
    private Vector3 wallNormal;
    private Camera mainCamera;
    private float dashEndTime;
    public Vector2 directionPlayer;
    public Rigidbody rb;
    public PlayerSettings variables;
    public AttackModeCamera attackModeCamera;
    public LayerMask obstacleLayer;
    public LayerMask wallLayer;
    public float obstacleDetectionDistance = 1.5f;
    public float WallDetectionDistance = 1.5f;

    public bool isRunning;
    public bool isJumping = false;
    public bool isDashing = false;
    public bool isAttackinMode = false;
    private bool isWalkingOnWall = false;
    private float dashTimeElapsed; 
    private Vector3 dashStartPosition; 
    private Vector3 dashTargetPosition;


    private RaycastHit lastHit;

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
        controls.InputsPlayer.AttackMode.started += MoveTargetCam;
    }

    private void OnDisable()

    {
        controls.Disable();
        controls.InputsPlayer.Jump.started -= Jump;
        controls.InputsPlayer.Sprint.started -= StartRunning;
        controls.InputsPlayer.Sprint.canceled -= StopRunning;
        controls.InputsPlayer.Dash.started -= StartDash;
        controls.InputsPlayer.AttackMode.started -= MoveTargetCam;
    }

    private void Update()
    {
        directionPlayer = controls.InputsPlayer.Move.ReadValue<Vector2>();

        if (isDashing)
        {
            dashTimeElapsed += Time.deltaTime;
            float dashProgress = dashTimeElapsed / variables.dashDuration;

            rb.position = Vector3.Lerp(dashStartPosition, dashTargetPosition, dashProgress);

            if (dashTimeElapsed >= variables.dashDuration)
            {
                isDashing = false;
            }
        }
    }

    private void FixedUpdate()
    {

        if (!isDashing)
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

            if (!isWalkingOnWall)
            {
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
            RunWall();
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

    private void MoveTargetCam(InputAction.CallbackContext context)
    {
        attackModeCamera.MoveTarget();

        if (!isAttackinMode)
        {
            isAttackinMode = true;
        }

        else
        {
            isAttackinMode = false;
        }
    }

    private void RunWall()
    {
        if (Physics.Raycast(transform.position, transform.right, out lastHit, WallDetectionDistance, wallLayer) ||
            Physics.Raycast(transform.position, -transform.right, out lastHit, WallDetectionDistance, wallLayer))
        {
            wallNormal = lastHit.normal;
            StartWallRun();
        }
        else if (isWalkingOnWall)
        {
            StopWallRun();
        }
    }

    private void StartWallRun()
    {
        isWalkingOnWall = true;
        rb.useGravity = false;

        Vector3 wallForward = Vector3.Cross(wallNormal, Vector3.up);
        if (Vector3.Dot(wallForward, transform.forward) < 0)
        {
            wallForward = -wallForward;
        }


        Quaternion wallRotation = Quaternion.LookRotation(wallForward, Vector3.up);
        rb.rotation = Quaternion.Slerp(rb.rotation, wallRotation, Time.deltaTime * variables.rotationSpeed);

        WalkOnWall(wallForward);
    }

    private void WalkOnWall(Vector3 wallDirection)
    {
        // Movimiento en la dirección de la pared
        float currentSpeed = variables.wallRunSpeed;
        Vector3 movement = wallDirection * currentSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);

        rb.AddForce(-wallNormal * variables.wallGravity, ForceMode.Acceleration);
    }

    private void StopWallRun()
    {
        isWalkingOnWall = false;
        rb.useGravity = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 startPosition = transform.position;
        Vector3 direction = transform.right;
        Gizmos.DrawLine(startPosition, startPosition + direction * WallDetectionDistance);
        Gizmos.DrawSphere(startPosition + direction * WallDetectionDistance, 0.1f);

        if (lastHit.collider != null)
        {
            Gizmos.color = Color.green; 
            Gizmos.DrawLine(lastHit.point, lastHit.point + lastHit.normal);
            Gizmos.DrawSphere(lastHit.point, 0.1f);
            print("Detectado");
        }

    }

}