using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Transform playerOrientation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerOrientation = GameObject.Find("PlayerOrientation").transform;
    }

    // Update is called once per frame
    void Update()
    {
        // rotate orientation of the player
        playerOrientation.forward = (playerOrientation.position - new Vector3(transform.position.x, playerOrientation.position.y, transform.position.z)).normalized;
    }

}
