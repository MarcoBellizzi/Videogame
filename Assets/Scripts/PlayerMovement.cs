using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // setted from unity
    [SerializeField] private Transform playerObject;

    // loaded once
    private CharacterController controller;
    private Animator animator;
    private Transform playerOrientation;

    // used in update
    private float horInput;
    private float verInput;
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

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        playerOrientation = GameObject.Find("PlayerOrientation").transform;
    }

    void Update()
    {
        horInput = Input.GetAxis("Horizontal");
        verInput = Input.GetAxis("Vertical");
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
            if (Input.GetKey(KeyCode.Space))
            {
                // da fermo
                if (groundDirection == Vector3.zero)
                {
                    airDirection = Vector3.zero;
                }
                // in movimento
                else
                {
                    airDirection = playerObject.forward * currentSpeed;
                }

                airDirection.y += Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);

            }


            // si sposta a terra
            if (groundDirection != Vector3.zero)
            {
                if (!Input.GetKey(KeyCode.LeftShift))
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
                playerObject.forward = Vector3.Slerp(playerObject.forward, groundDirection.normalized, Time.deltaTime * rotationSpeed);

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

            // ha appena iniziato il salto o ha iniato a cadere
            if (wasGrounded)
            {
                animator.CrossFade("Falling", 0.2f);

                if (groundDirection != Vector3.zero)
                {
                    airDirection.x = playerObject.forward.x * currentSpeed; // improve with more phisics
                    airDirection.z = playerObject.forward.z * currentSpeed; // improve with more phisics
                }
            }
        }

        controller.Move(airDirection * Time.deltaTime);

    }
}
