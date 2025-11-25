using UnityEngine;

public class MainCamera : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Player.instance.playerOrientation.transform.forward = 
            (Player.instance.playerOrientation.transform.position -
                new Vector3(transform.position.x, Player.instance.playerOrientation.transform.position.y, transform.position.z)).normalized;
    }

}
