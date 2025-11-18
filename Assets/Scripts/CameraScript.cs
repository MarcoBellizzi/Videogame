using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Transform playerOrientation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerOrientation = GameObject.Find("PlayerOrientation").transform;
    }

    void Update()
    {
        // rotate orientation of the player
        playerOrientation.forward = (playerOrientation.position - new Vector3(transform.position.x, playerOrientation.position.y, transform.position.z)).normalized;
    }

}
