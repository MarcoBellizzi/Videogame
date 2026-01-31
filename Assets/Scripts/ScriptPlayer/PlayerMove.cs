using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerMove : MonoBehaviour
{
    private Player player;

    // Parametri Movimento
    private float movementSpeed = 7f;
    private float rotationSpeed = 7f;
    private float gravityValue = -9.81f;
    private float jumpHeight = 2.0f;

    // Stato Interno
    private float horInput;
    private float verInput;
    private bool run;
    private float currentSpeed;
    private Vector3 groundDirection;
    private Vector3 airDirection;

    // Gestione Ground
    private bool isGrounded;
    private bool wasGrounded = false;

    void Start()
    {
        player = GetComponent<Player>();
        isGrounded = true;
    }

    public void Update()
    {
        if (!player.canMove) return;

        HandleInput();
        HandleGroundCheck();
        CalculateMovement();
        ApplyMovement();
        UpdateAnimator();
    }

    void HandleInput()
    {
        horInput = Input.GetAxis("Horizontal");
        verInput = Input.GetAxis("Vertical");
        run = Input.GetKey(KeyCode.LeftShift);
    }

    void HandleGroundCheck()
    {
        // Qui ho mantenuto la logica originale (semplificata). 
        // Idealmente dovresti usare Physics.CheckSphere o player.controller.isGrounded
        wasGrounded = true;
        isGrounded = true;
    }

    void CalculateMovement()
    {
        if (isGrounded)
        {
            groundDirection = player.playerOrientation.forward * verInput + player.playerOrientation.right * horInput;

            // Logica atterraggio / cambio camera mode
            if (!wasGrounded)
            {
                airDirection = Vector3.zero;
                UpdateCameraBasedAnimations();
            }

            // Calcolo velocità
            if (groundDirection != Vector3.zero)
            {
                currentSpeed = run ? movementSpeed * 2 : movementSpeed;

                // Rotazione del modello (solo in Free Look)
                if (MainCamera.instance.state == CamState.FREE_LOOK_CAM)
                {
                    player.playerModel.forward = Vector3.Slerp(player.playerModel.forward, groundDirection.normalized, Time.deltaTime * rotationSpeed);
                }
            }
            else
            {
                currentSpeed = 0;
            }
        }
        else
        {
            // Logica Gravità
            airDirection.y += gravityValue * Time.deltaTime;
            groundDirection = Vector3.zero;
        }
    }

    void ApplyMovement()
    {
        player.controller.Move((airDirection + groundDirection * currentSpeed) * Time.deltaTime);
    }

    void UpdateAnimator()
    {
        player.animator.SetFloat("horInput", horInput, 0.1f, Time.deltaTime);
        player.animator.SetFloat("verInput", verInput, 0.1f, Time.deltaTime);

        if (groundDirection != Vector3.zero)
        {
            float targetAnimSpeed = run ? 1f : 0.5f;
            player.animator.SetFloat("speed", targetAnimSpeed, 0.1f, Time.deltaTime);
        }
        else
        {
            player.animator.SetFloat("speed", 0, 0.1f, Time.deltaTime);
        }
    }

    void UpdateCameraBasedAnimations()
    {
        // Trigger animazioni basate sullo stato della camera
        string triggerPrefix = (MainCamera.instance.state == CamState.FREE_LOOK_CAM) ? "freeMode" : "lockMode";
        string triggerName = player.isHolding ? triggerPrefix + "Holding" : triggerPrefix;
        player.animator.SetTrigger(triggerName);
    }
}