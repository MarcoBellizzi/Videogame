using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // public, used in animator
    [HideInInspector] public bool canMove;
    [HideInInspector] public bool canAttack;
    [HideInInspector] public bool canAttackNext;
    [HideInInspector] public bool isHolding;
    [HideInInspector] public bool canGetHit;
    
    // public
    [HideInInspector] public float healthPoints;

    // setted from unity
    [SerializeField] public Transform playerModel;
    [SerializeField] public Transform sword;
    [SerializeField] public Transform swordPoint;
    [SerializeField] public Transform rightHandPoint;

    // loaded once
    public static Player instance;
    [HideInInspector] public Animator animator;
    [HideInInspector] public  CharacterController controller;
    [HideInInspector] public Transform playerOrientation;
    [HideInInspector] public Transform toFollowVirtual;

    // privates, used in update
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
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        playerOrientation = GameObject.Find("Orientation").transform;
        toFollowVirtual = GameObject.Find("ToFollow").transform;
        isGrounded = true;  // è a terra (y position nell ispector 9.980798e-05)
        canMove = false;   // per il primo menu
        // canMove = true;   // per il primo menu
        canAttack = false;   // per il primo menu
        canAttackNext = false;  // prima inizializzazione
        isHolding = false;
        healthPoints = 100f;
        canGetHit = true;
    }

    void Start()
    {
        StartCoroutine(ShowWelcome()); // per attivare il pannello dopo che entra nello state blend nell animator
    }

    IEnumerator ShowWelcome()
    {
        yield return new WaitForSeconds(0.1f); // perchè??

        if (PanelDialogues.instance != null)
        {
            PanelDialogues.instance.Show("welcome");
        }
    }

    void Update()
    {

        // external input
        if (Input.GetKeyDown(KeyCode.Z))
        {
            healthPoints -= 10f;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            // Girl.instance.healthPoints -= 10f;
            PanelHeathBars.instance.enemyHealtPoints -= 10f;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!isHolding)
            {
                animator.SetTrigger("unsheathe");
            }
            else
            {
                animator.SetTrigger("sheathe");
            }
        }

        if (isHolding && canAttack && Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.SetTrigger("attack");
        }

        // evita di tirare un pugno quando chiudo lo schermo di una conversazione o degli oggetti
        if (canAttackNext)
        {
            canAttack = true;
            canAttackNext = false;
        }

        // è attivo un pannello
        if (!canMove)
        {
            horInput = 0;
            verInput = 0;
            jump = false;
            run = false;

            animator.SetFloat("horInput", 0, 0.1f, Time.deltaTime);
            animator.SetFloat("verInput", 0, 0.1f, Time.deltaTime);
        }
        // non sono attivi pannelli
        else
        {
            horInput = Input.GetAxis("Horizontal");
            verInput = Input.GetAxis("Vertical");
            jump = Input.GetKeyDown(KeyCode.Space);
            run = Input.GetKey(KeyCode.LeftShift);

            animator.SetFloat("horInput", horInput, 0.1f, Time.deltaTime);
            animator.SetFloat("verInput", verInput, 0.1f, Time.deltaTime);
        }

        wasGrounded = isGrounded;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.01f); // da fixare meglio


        // è a terra
        if (isGrounded)
        {
            groundDirection = playerOrientation.forward * verInput + playerOrientation.right * horInput;

            // è appena atterrato
            if(!wasGrounded)
            {
                airDirection = Vector3.zero; // valutare se negativo

                if (MainCamera.instance.state == CamState.FREE_LOOK_CAM)
                {
                    if (!isHolding)
                    {
                        animator.SetTrigger("freeMode");
                    }
                    else
                    {
                        animator.SetTrigger("freeModeHolding");
                    }
                }
                if (MainCamera.instance.state == CamState.LOCKONCAM)
                {
                    if (!isHolding)
                    {
                        animator.SetTrigger("lockMode");
                    }
                    else
                    {
                        animator.SetTrigger("lockModeHolding");
                    }
                }
            }

            // ha appena iniziato il salto
            if (jump)
            {
                if (MainCamera.instance.state == CamState.FREE_LOOK_CAM)
                {
                    if (groundDirection != Vector3.zero)
                    {
                        airDirection = playerModel.forward * currentSpeed; // airDirection.y=0
                    }
                    else
                    {
                       airDirection = Vector3.zero;
                    }
                }
                if (MainCamera.instance.state == CamState.LOCKONCAM)
                {
                    airDirection = groundDirection * currentSpeed; // airDirection.y=0
                }

                airDirection.y += Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);

            }

            // si sposta a terra
            if (groundDirection != Vector3.zero)
            {
                if (!run)
                {
                    animator.SetFloat("speed", 0.5f, 0.1f, Time.deltaTime);
                    currentSpeed = movementSpeed;
                }
                else
                {
                    animator.SetFloat("speed", 1f, 0.1f, Time.deltaTime);
                    currentSpeed = movementSpeed * 2;
                }

                if (MainCamera.instance.state == CamState.FREE_LOOK_CAM)
                {
                    // rotate the player model
                    playerModel.forward = Vector3.Slerp(playerModel.forward, groundDirection.normalized, Time.deltaTime * rotationSpeed);
                }
            }
            // è fermo a terra
            else
            {
                animator.SetFloat("speed", 0, 0.1f, Time.deltaTime);
                currentSpeed = 0;
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
                
                //if (MainCamera.instance.state == CamState.FREE_LOOK_CAM)
                {
                    if (groundDirection != Vector3.zero)
                    {
                        airDirection.x = playerModel.forward.x * currentSpeed;
                        airDirection.z = playerModel.forward.z * currentSpeed;
                    }
                }
                if (MainCamera.instance.state == CamState.LOCKONCAM)
                {
                    airDirection.x = groundDirection.x * currentSpeed;
                    airDirection.z = groundDirection.z * currentSpeed;
                }
            }

            groundDirection = Vector3.zero;
        }

        controller.Move((airDirection + groundDirection * currentSpeed) * Time.deltaTime);
        
    }
    

    // for menus
    public void Stop()
    {
        canMove = false;
        canAttack = false;
    }

    // for menus
    public void Resume()
    {
        canMove = true;
        canAttackNext = true;
    }

    public void GetHit(float demage)
    {
        healthPoints -= demage;

        if (healthPoints <= 0)
        {
            animator.SetTrigger("death");
        }

        animator.SetTrigger("getHit");
    }

    IEnumerator WaitForEnd()
    {
        yield return new WaitForSeconds(6f);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("Menu");
    }

    public void Death()
    {
        StartCoroutine(WaitForEnd());
    }

}
