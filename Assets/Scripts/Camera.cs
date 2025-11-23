using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Transform playerOrientation;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        playerOrientation = GameObject.Find("PlayerOrientation").transform;
    }

    void Update()
    {
        // un vettore che parte dalla telecamera e finisce al player (ignorando l'asse verticale)
        playerOrientation.forward = (playerOrientation.position - new Vector3(transform.position.x, playerOrientation.position.y, transform.position.z)).normalized;
    }

}
