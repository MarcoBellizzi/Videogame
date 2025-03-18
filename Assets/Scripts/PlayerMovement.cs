using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // setted from unity
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Transform playerObject;

    // loaded once
    private CharacterController controller;
    private Animator animator;
    private Transform playerOrientation;

    // used in update
    private float horInput;
    private float verInput;
    private Vector3 moveDirection;

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

        // move the player
        moveDirection = playerOrientation.forward * verInput + playerOrientation.right * horInput;

        if (moveDirection != Vector3.zero)
        {
            animator.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
        }
        else
        {
            animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
        }

        controller.Move(moveDirection.normalized * movementSpeed * Time.deltaTime);

        // rotate the player object
        if (moveDirection != Vector3.zero)
        {
            playerObject.forward = Vector3.Slerp(playerObject.forward, moveDirection.normalized, Time.deltaTime * rotationSpeed);
        }

    }
}
