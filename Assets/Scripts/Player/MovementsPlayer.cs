using UnityEngine;
using UnityEngine.InputSystem;

public class MovementsPlayer : MonoBehaviour
{
    private InputSystem_Actions controls;
    private Vector3 jumpTargetPosition;
    private Vector3 wallNormal;
    private Camera mainCamera;
    private float dashTimeElapsed;
    private float dashEndTime;
    private Vector3 dashStartPosition;
    private Vector3 dashTargetPosition;
    public Vector2 directionPlayer;
    public Rigidbody rb;
    public PlayerSettings variables;
    public AttackModeCamera attackModeCamera;
    public WeaponController weaponController;
    public LayerMask obstacleLayer;
    public LayerMask wallLayer;
    public float obstacleDetectionDistance = 1.5f;
    public float WallDetectionDistance = 1.5f;
    public float stepHeight = 0.5f;
    public float stepSmooth = 0.2f;
    public float stepDetectionDistance = 0.5f;
    public WeaponVFX weaponVFX;

    public bool isRunning;
    public bool isJumping = false;
    public bool isDashing = false;
    public bool isAttackinMode = false;
    public bool isWalkingOnWall = false;
    public bool isAxesInverted = false;


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
        controls.InputsPlayer.Attack.performed += FireWeapon;
        controls.InputsPlayer.Attack.canceled += StopFireWeapon;
        controls.InputsPlayer.ChangeWeapon.started += ChangeWeapon;
    }

    private void OnDisable()

    {
        controls.Disable();
        controls.InputsPlayer.Jump.started -= Jump;
        controls.InputsPlayer.Sprint.started -= StartRunning;
        controls.InputsPlayer.Sprint.canceled -= StopRunning;
        controls.InputsPlayer.Dash.started -= StartDash;
        controls.InputsPlayer.AttackMode.started -= MoveTargetCam;
        controls.InputsPlayer.Attack.performed -= FireWeapon;
        controls.InputsPlayer.Attack.canceled -= FireWeapon;
        controls.InputsPlayer.Attack.canceled -= StopFireWeapon;
        controls.InputsPlayer.ChangeWeapon.started -= ChangeWeapon;
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
            Vector3 movement;

            if (isAxesInverted)
            {
                movement = new Vector3(0, 0, directionPlayer.x);
            }
            else
            {
                movement = new Vector3(directionPlayer.x, 0, directionPlayer.y);
            }

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
        if (!isJumping && movementFinal != Vector3.zero)
        {
            RaycastHit hit;

            // Detección de colisiones con un SweepTest
            if (!rb.SweepTest(movementFinal, out hit, speed * Time.deltaTime) || hit.collider.isTrigger)
            {
                // Detectar y manejar escalones antes de moverse
                DetectAndHandleSteps(movementFinal, stepHeight, stepSmooth);

                // Mover el personaje
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

            Vector3 dashDirection = transform.forward;
            float dashDistance = variables.dashDistance;

            RaycastHit hit;
            if (Physics.Raycast(rb.position, dashDirection, out hit, dashDistance))
            {
                dashTargetPosition = hit.point - dashDirection * 0.1f;
            }
            else
            {

                dashTargetPosition = rb.position + dashDirection * dashDistance;
            }
        }
    }

    private void MoveTargetCam(InputAction.CallbackContext context)
    {
        attackModeCamera.MoveTarget();

        if (!isAttackinMode && !isAxesInverted)
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

    private void FireWeapon(InputAction.CallbackContext context)
    {
        if (isAttackinMode)
        {
            if (weaponController.GetCurrentWeaponIndex() != 1)
            {
                weaponController.Fire(true);
            }
            else 
            { 
                weaponVFX.PlayMuzzleFlash();
            }
        }
    }
    private void StopFireWeapon(InputAction.CallbackContext context)
    {
        if (weaponController.GetCurrentWeaponIndex() == 1)
        {
            weaponController.HeavyAttack(true);
            weaponController.Fire(true);
            weaponVFX.StopMuzzleFlash();
        }
        else
        {
            weaponController.HeavyAttack(false);
        }

        weaponController.Fire(false);
    }

    private void ChangeWeapon(InputAction.CallbackContext context)
    {
        if (isAttackinMode)
        {
            weaponController.HandleWeaponSwitch();
        }
    }

    public void ToggleAxesInversion(bool inverted)
    {
        isAxesInverted = inverted;
        Alinear();
    }

    public void Alinear()
    {
        transform.rotation = Quaternion.Euler(0, 90, 0);
    }

    private void DetectAndHandleSteps(Vector3 direction, float stepHeight, float stepSmooth)
    {
        RaycastHit lowerHit;
        RaycastHit upperHit;

       
        Vector3 lowerRayOrigin = rb.position + Vector3.up * 0.1f; 
        Vector3 upperRayOrigin = rb.position + Vector3.up * stepHeight;

        if (Physics.Raycast(lowerRayOrigin, direction, out lowerHit, stepDetectionDistance))
        {
            if (!Physics.Raycast(upperRayOrigin, direction, out upperHit, stepDetectionDistance))
            {
                rb.position += Vector3.up * stepSmooth;
            }
        }
    }

}