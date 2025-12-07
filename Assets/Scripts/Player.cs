using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    [HideInInspector] public Transform playerOrientation;
    [HideInInspector] public bool canMove;

    // setted from unity
    [SerializeField] private Transform playerModel;

    // loaded once
    private CharacterController controller;
    private Animator animator;

    // used in update
    private float horInput;
    private float verInput;
    private bool jump;
    private bool run;
    private float currentSpeed;
    private bool isGrounded;
    private bool wasGrounded;
    private Vector3 groundDirection;
    private Vector3 airDirection;

    // fixed
    private float movementSpeed = 7f;
    private float rotationSpeed = 7f;
    private float gravityValue = -9.81f;
    private float jumpHeight = 2.0f;

    void Awake()
    {
        instance = this;
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        playerOrientation = GameObject.Find("PlayerOrientation").transform;
        canMove = true;
    }

    void Start()
    {
        PanelDialogues.instance.Show("welcome");
    }

    void Update()
    {
        // è attivo il pannello di una conversazione
        if (!canMove)
        {
            horInput = 0;
            verInput = 0;
            jump = false;
            run = false;
        }
        else
        {
            horInput = Input.GetAxis("Horizontal");
            verInput = Input.GetAxis("Vertical");
            jump = Input.GetKey(KeyCode.Space);
            run = Input.GetKey(KeyCode.LeftShift);
        }
        
        groundDirection = playerOrientation.forward * verInput + playerOrientation.right * horInput;

        wasGrounded = isGrounded;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.01f);


        // è a terra
        if (isGrounded)
        {
            // è appena atterrato
            if(!wasGrounded)
            {
                airDirection = Vector3.zero;
                animator.CrossFade("Empty", 0.2f); // landing
            }

            // ha appena iniziato il salto
            if (jump)
            {
                // da fermo
                if (groundDirection == Vector3.zero)
                {
                    airDirection = Vector3.zero;
                }
                // in movimento
                else
                {
                    airDirection = playerModel.forward * currentSpeed;
                }

                airDirection.y += Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);

            }

            // si sposta a terra
            if (groundDirection != Vector3.zero)
            {
                if (!run)
                {
                    animator.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
                    currentSpeed = movementSpeed;
                }
                else
                {
                    animator.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);
                    currentSpeed = movementSpeed * 2;
                }

                controller.Move(groundDirection * currentSpeed * Time.deltaTime);

                // rotate the player object
                playerModel.forward = Vector3.Slerp(playerModel.forward, groundDirection.normalized, Time.deltaTime * rotationSpeed);

            }
            // è fermo a terra
            else
            {
                animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
            }

        }
        // è in volo
        else
        {
            airDirection.y += gravityValue * Time.deltaTime;

            // ha appena iniziato il salto o ha iniziato a cadere
            if (wasGrounded)
            {
                animator.CrossFade("Falling", 0.2f);

                if (groundDirection != Vector3.zero)
                {
                    airDirection.x = playerModel.forward.x * currentSpeed; // improve with more phisics
                    airDirection.z = playerModel.forward.z * currentSpeed; // improve with more phisics
                }
            }
        }

        controller.Move(airDirection * Time.deltaTime);

    }
}
