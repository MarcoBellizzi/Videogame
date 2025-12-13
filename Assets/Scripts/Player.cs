using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    [HideInInspector] public Transform playerOrientation;
    [HideInInspector] public Transform toFollowVirtual;
    [HideInInspector] public bool canMove;
    [HideInInspector] public bool canPunch;
    [HideInInspector] public bool canPunchNext;
    [HideInInspector] public bool canThrow;
    [HideInInspector] public Animator animator;

    // setted from unity
    [SerializeField] public Transform playerModel;

    // loaded once
    private CharacterController controller;

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
        toFollowVirtual = GameObject.Find("ToFollowVirtual").transform;
        isGrounded = true;  // è a terra (y position nell ispector 9.980798e-05)
        canMove = false;   // per il primo menu
        canPunch = false;   // per il primo menu
        canPunchNext = false;  // prima inizializzazione
        canThrow = false;  // non è nello stato fps
    }

    void Start()
    {
        StartCoroutine(ShowWelcome()); // per attivare il pannello dopo che entra nello state blend nell animator
    }

    IEnumerator ShowWelcome()
    {
        yield return new WaitForSeconds(0.1f);
        
        PanelDialogues.instance.Show("welcome");
    }

    void Update()
    {
        // tira un pugno
        if (canPunch && Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.SetTrigger("punch");
        }

        // evita di tirare un pugno quando chiudo lo schermo di una conversazione o degli oggetti
        if (canPunchNext)
        {
            canPunch = true;
            canPunchNext = false;
        }

        // lancia qualcosa
        if (canThrow && Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.SetTrigger("throw");
        }

        // è attivo un pannello
        if (!canMove)
        {
            horInput = 0;
            verInput = 0;
            jump = false;
            run = false;
        }
        // non sono attivi pannelli
        else
        {
            horInput = Input.GetAxis("Horizontal");
            verInput = Input.GetAxis("Vertical");
            jump = Input.GetKey(KeyCode.Space);
            run = Input.GetKey(KeyCode.LeftShift);

            animator.SetFloat("oxInput", horInput, 0.1f, Time.deltaTime);
            animator.SetFloat("vxInput", verInput, 0.1f, Time.deltaTime);
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

                if (MainCamera.instance.state != CamState.FPSCAM)
                {
                    animator.SetTrigger("blend");
                }
                else
                {
                    animator.SetTrigger("blendFPS");
                }
            }

            // ha appena iniziato il salto
            if (jump)
            {
                
                if (MainCamera.instance.state != CamState.FPSCAM)
                {
                    if (groundDirection != Vector3.zero)
                    {
                        airDirection = playerModel.forward * currentSpeed;
                    }
                    else
                    {
                       airDirection = Vector3.zero;
                    }
                }
                else
                {
                    airDirection = groundDirection * currentSpeed;
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

                if (MainCamera.instance.state != CamState.FPSCAM)
                {
                    // rotate the player model
                    playerModel.forward = Vector3.Slerp(playerModel.forward, groundDirection.normalized, Time.deltaTime * rotationSpeed);
                }
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

            // ha iniziato a cadere o ha appena iniziato il salto
            if (wasGrounded)
            {   
                
                animator.SetTrigger("jump");
                
                if (MainCamera.instance.state != CamState.FPSCAM)
                {
                    if (groundDirection != Vector3.zero)
                    {
                        airDirection.x = playerModel.forward.x * currentSpeed;
                        airDirection.z = playerModel.forward.z * currentSpeed;
                    }

                }
                else
                {
                    airDirection.x = groundDirection.x * currentSpeed;
                    airDirection.z = groundDirection.z * currentSpeed;
                }
            }
        }

        controller.Move(airDirection * Time.deltaTime);

    }

    // for menus
    public void Stop()
    {
        canMove = false;
        canPunch = false;
        canThrow = false;
    }

    // for menus
    public void Resume()
    {
        canMove = true;
        
        if (MainCamera.instance.state != CamState.FPSCAM)
        {
            canPunchNext = true;
        }
        else
        {
            canThrow = true;
        }
    }

}
