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
    public AnimationsPlayer animationsPlayer;
    public SounPlayerManager sounPlayerManager;
    public AudioSource audioSource;
    public AudioSource audioSource3;
    public CursorContrloller cursorContrloller;
    public LayerMask obstacleLayer;
    public LayerMask wallLayer;
    public float obstacleDetectionDistance = 1.5f;
    public float WallDetectionDistance = 0.5f;
    public float stepHeight = 0.5f;
    public float stepSmooth = 0.2f;
    public float stepDetectionDistance = 0.5f;
    public WeaponVFX weaponVFX;
    public Vector3 movementRelativeToCamera;
    public float currentSpeed;

    public bool isRunning;
    public bool isJumping = false;
    public bool isDashing = false;
    public bool isAttackinMode = false;
    public bool isWalkingOnWall = false;
    public bool isAxesInverted = false;
    public bool WallRight = false;
    public bool Wallleft = false;
    public bool IsDead = false;
    public bool CanWalk = false;
    [SerializeField] bool didHit;
    [SerializeField] bool isPlayingStepWallSound = false;
    [SerializeField] bool isPlayingGunSound = false;




    private RaycastHit lastHit;

    private void Awake()
    {
        controls = new();
        mainCamera = Camera.main;
        audioSource = GetComponent<AudioSource>();

    }

    private void OnEnable()
    {
        controls.Enable();
        controls.InputsPlayer.Jump.started += Jump;
        controls.InputsPlayer.Sprint.started += StartRunning;
        controls.InputsPlayer.Sprint.canceled += StopRunning;
        controls.InputsPlayer.Dash.started += StartDash;
        controls.InputsPlayer.AttackMode.started += OnMoveTargetCam;
        controls.InputsPlayer.Attack.performed += FireWeapon;
        controls.InputsPlayer.Attack.canceled += StopFireWeapon;
        controls.InputsPlayer.ChangeWeapon.started += ChangeWeapon;
        controls.InputsPlayer.Pause.started += ActivatePauseMenu;
    }

    private void OnDisable()

    {
        controls.Disable();
        controls.InputsPlayer.Jump.started -= Jump;
        controls.InputsPlayer.Sprint.started -= StartRunning;
        controls.InputsPlayer.Sprint.canceled -= StopRunning;
        controls.InputsPlayer.Dash.started -= StartDash;
        controls.InputsPlayer.AttackMode.started -= OnMoveTargetCam;
        controls.InputsPlayer.Attack.performed -= FireWeapon;
        controls.InputsPlayer.Attack.canceled -= FireWeapon;
        controls.InputsPlayer.Attack.canceled -= StopFireWeapon;
        controls.InputsPlayer.ChangeWeapon.started -= ChangeWeapon;
        controls.InputsPlayer.Pause.started += ActivatePauseMenu;
    }

    private void Update()
    {
        if (!IsDead)
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

            if (directionPlayer.magnitude > 0.1f && !isWalkingOnWall)
            {
                if (!audioSource.isPlaying)
                {
                    sounPlayerManager.PlaySound("Walk");
                }

            }
            else
            {
                if (audioSource.isPlaying)
                {
                    sounPlayerManager.StopSound();
                }
            }

            if (isWalkingOnWall)
            {
                if (!isPlayingStepWallSound) // Solo reproducir si no está sonando ya
                {
                    sounPlayerManager.PlaySound3("StepWall");
                    isPlayingStepWallSound = true;
                }
            }
            else
            {
                if (isPlayingStepWallSound) // Detener el sonido si ya no está caminando en la pared
                {
                    sounPlayerManager.StopSound3();
                    isPlayingStepWallSound = false;
                }
            }
        }

        else 
        {
            rb.linearVelocity = Vector3.zero;
            return;
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

            movementRelativeToCamera = (camForward * movement.z + camRight * movement.x).normalized;
            currentSpeed = isRunning ? variables.speedSprint : variables.speed;

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

            // Ejecutar el SweepTest
            bool didHit = rb.SweepTest(movementFinal, out hit, speed * Time.deltaTime);

            // Ignorar colisiones con el Layer "Floor"
            if (didHit && hit.collider.gameObject.layer == LayerMask.NameToLayer("Floor"))
            {
                didHit = false; // Ignorar esta colisión
            }

            CanWalk = !didHit;

            // Si no hubo colisión o se ignoró, mover el personaje
            if (!didHit)
            {
                rb.MovePosition(rb.position + Time.deltaTime * speed * movementFinal);
            }
        }
    }

    private void StartRunning(InputAction.CallbackContext context)
    {
        isRunning = true;
        sounPlayerManager.PlaySound("Run");
    }

    private void StopRunning(InputAction.CallbackContext context)
    {
        isRunning = false;
        sounPlayerManager.StopSound();
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
        sounPlayerManager.PlaySound2("Dash");
        if (!isDashing)
        {
            isDashing = true;
            dashTimeElapsed = 0f;
            dashStartPosition = rb.position;

            Vector3 dashDirection = transform.forward;
            float dashDistance = variables.dashDistance;

            RaycastHit hit;
            Vector3 raycastOrigin = rb.position + Vector3.up * 0.5f;
            if (Physics.Raycast(raycastOrigin, dashDirection, out hit, dashDistance))
            {
                dashTargetPosition = hit.point - dashDirection * 0.1f;
            }
            else
            {
                dashTargetPosition = rb.position + dashDirection * dashDistance;
            }
        }
    }

    private void MoveTargetCam(bool manualActivation = false)
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

        if (manualActivation)
        {
            isAttackinMode = false;
        }
    }

    public void OnMoveTargetCam(InputAction.CallbackContext context)
    {
          MoveTargetCam(); 
    }


    private void RunWall()
    {
        Wallleft = Physics.Raycast(transform.position, transform.right, out lastHit, WallDetectionDistance, wallLayer);
        WallRight = Physics.Raycast(transform.position, -transform.right, out lastHit, WallDetectionDistance, wallLayer);
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
        sounPlayerManager.StopSound3();

    }

    private void FireWeapon(InputAction.CallbackContext context)
    {
        if (isAttackinMode)
        {
            if (weaponController.GetCurrentWeaponIndex() != 1)
            {
                weaponController.Fire(true);
                animationsPlayer.ActivateGun(true);
                if (!isPlayingGunSound)
                {
                    sounPlayerManager.PlaySound3("Gun2");
                    isPlayingGunSound = true;
                }
            }
            else 
            { 
                weaponVFX.PlayMuzzleFlash();
                if (!isWalkingOnWall)
                {
                    if (!isPlayingGunSound) 
                    {
                        sounPlayerManager.PlaySound3("Gun1");
                        isPlayingGunSound = true;
                    }
                }
            }
        }
    }
    private void StopFireWeapon(InputAction.CallbackContext context)
    {
        int currentWeaponIndex = weaponController.GetCurrentWeaponIndex();

        if (currentWeaponIndex == 1)
        {
            weaponController.HeavyAttack(true);
            weaponController.Fire(false);
            animationsPlayer.Shot();
            weaponVFX.StopMuzzleFlash();
        }
        else
        {
            weaponController.HeavyAttack(false);
            animationsPlayer.ActivateGun(false);
            weaponController.Fire(false);
        }

        // Detener sonido del disparo
        if (isPlayingGunSound)
        {
            sounPlayerManager.StopSound3();
            isPlayingGunSound = false; // Reiniciar estado
        }
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

    public void RestartVariables()
    {
        MoveTargetCam(false);
        rb.linearVelocity = Vector3.zero;
        IsDeath(true);
        StopWallRun();
        isAttackinMode = false;
        attackModeCamera.RestartHudVisibility();
    }

    public void IsDeath(bool death)
    {
        IsDead = death;
    }

    public void ActivatePauseMenu(InputAction.CallbackContext context)
    {
        cursorContrloller.ActivatePauseMenu();
    }

    public void StopSound()
    {
        sounPlayerManager.StopSound();
        sounPlayerManager.StopSound2();
        sounPlayerManager.StopSound3();
    }
}