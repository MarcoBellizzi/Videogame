using System;
using Cinemachine;
using UnityEngine;

public enum CamState
{
    FREE_LOOK_CAM,
    LOCKONCAM,
    FPSCAM
}

public class MainCamera : MonoBehaviour
{
    public static MainCamera instance;

    public CinemachineFreeLook freeLookCam;
    public CinemachineVirtualCamera lockOnCam;
    // public CinemachineVirtualCamera fpsCam;

    [HideInInspector] public CamState state;

    // private GameObject viewFinder;

    // private float yaw;
    // private float pitch;

    private float locCamBack = 0f;
    private float locCamUp = 1f;
    // private float fpsCamBack = 3f;
    // private float fpsCamUp = 1.5f;
    // private float fpsCamRight = 0.6f;
    // private float sensibilitaX = 180f;
    // private float sensibilitaY = 180f;
    // private float pitchMin = -85f;
    // private float pitchMax = 85f;
    private float maxLockDistance = 40f;


    void Awake()
    {
        instance = this;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        state = CamState.FREE_LOOK_CAM;
        freeLookCam.Priority = 10;
        lockOnCam.Priority = 0;
        // fpsCam.Priority = 0;
        // yaw = 0f;
        // pitch = 0f;
        // viewFinder = GameObject.Find("ViewFinder").gameObject;
    }

    void Start()
    {
        // viewFinder.SetActive(false);
    }

    void Update()
    {

        if (state == CamState.FREE_LOOK_CAM)
        {
            // AGGIORNA L'ORIENTAMENTO DEL PLAYER IN BASE ALLA CAMERA
            Player.instance.playerOrientation.transform.forward = 
                (Player.instance.playerOrientation.transform.position -
                    new Vector3(
                        transform.position.x, 
                        Player.instance.playerOrientation.transform.position.y,
                        transform.position.z
                    )
                ).normalized;
        }
        
        if (state == CamState.LOCKONCAM)
        {
            // AGGIORNA L'ORIENTAMENTO DEL PLAYER IN BASE AL TARGET
            Player.instance.playerOrientation.transform.forward = 
                (lockOnCam.LookAt.position - 
                    new Vector3(
                        Player.instance.transform.position.x,
                        lockOnCam.LookAt.position.y,
                        Player.instance.transform.position.z
                    )
                ).normalized;

            // AGGIORNA L'ORIENTAMENTO DEL MODELLO IN BASE AL TARGET
            Player.instance.playerModel.transform.forward = 
                (lockOnCam.LookAt.position - 
                    new Vector3(
                        Player.instance.transform.position.x,
                        lockOnCam.LookAt.position.y,
                        Player.instance.transform.position.z
                    )
                ).normalized;

            // AGGIORNA POSIZIONE E ORIENTAMENTO DELLA CAMERA IN BASE AL TARGET E AL PLAYER
            Player.instance.toFollowVirtual.transform.forward =
                (lockOnCam.LookAt.position - Player.instance.transform.position).normalized;

            Player.instance.toFollowVirtual.transform.position =
                Player.instance.transform.position
                - locCamBack * Player.instance.toFollowVirtual.transform.forward
                + locCamUp * Player.instance.toFollowVirtual.transform.up;

        }

        // if (state == CamState.FPSCAM)
        // {
        //     float mouseInputX = Input.GetAxis("Mouse X");
        //     float mouseInputY = Input.GetAxis("Mouse Y");

        //     if (Player.instance.isThrowing)
        //     {
        //         mouseInputX = 0;
        //         mouseInputY = 0;
        //     }

        //     Player.instance.animator.SetFloat("oxMouseInput", Mathf.Clamp(mouseInputX, -1, 1), 0.1f, Time.deltaTime);

        //     yaw   += mouseInputX * sensibilitaX * Time.deltaTime;
        //     pitch -= mouseInputY * sensibilitaY * Time.deltaTime;
        //     pitch  = Mathf.Clamp(pitch, pitchMin, pitchMax);

        //     // AGGIORNA L'ORIENTAMENTO DEL PLAYER IN BASE AL MOUSE
        //     Player.instance.playerOrientation.transform.rotation =
        //         Quaternion.Slerp(
        //             Player.instance.playerOrientation.transform.rotation,
        //             Quaternion.Euler(0f, yaw, 0f),
        //             1f - Mathf.Exp(-10 * Time.deltaTime)
        //         );

        //     // AGGIORNA LA ROTAZIONE DEL MODELLO DEL PLAYER IN BASE AL MOUSE
        //     Player.instance.playerModel.transform.rotation =
        //         Quaternion.Slerp(
        //             Player.instance.playerModel.transform.rotation,
        //             Quaternion.Euler(0f, yaw, 0f),
        //             1f - Mathf.Exp(-10 * Time.deltaTime)
        //         );

        // }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            switch (state)
            {
                case CamState.FREE_LOOK_CAM:
                    Transform best = GetBest();
                    if (best != null)
                    {   
                        MoveToLockOnCamera(best);
                    }
                    break;

                case CamState.LOCKONCAM:
                    MoveToFreeLookCamera();
                    break;
            }

        }

        // if (Input.GetKeyDown(KeyCode.E))
        // {
        //     if (state != CamState.FPSCAM)
        //     {
        //         MoveToFpsCamera();
        //     }
        //     else
        //     {
        //         MoveToFreeLookCamera();
        //     }
        // }
    
    }

    // void LateUpdate()
    // {
    //     if (state == CamState.FPSCAM)
    //     {
    //         // AGGIORNA POSIZIONE E ROTAZIONE DELLA CAMERA IN BASE AL MOUSE E AL PLAYER
    //         fpsCam.transform.rotation = Quaternion.Euler(pitch, yaw, 0f);

    //         fpsCam.transform.position =
    //             Player.instance.transform.position
    //             - fpsCamBack * fpsCam.transform.forward
    //             + fpsCamUp * fpsCam.transform.up
    //             + fpsCamRight * fpsCam.transform.right;
    //     }
    // }

    public Transform GetBest()
    {
        Transform best = null;
        float bestDistSq = float.PositiveInfinity;

        foreach (Collider collider in Physics.OverlapSphere(
                Player.instance.transform.position, 
                maxLockDistance,
                1 << LayerMask.NameToLayer("CanLock")))
        {
            float d2 = (collider.transform.position - Player.instance.transform.position).sqrMagnitude;
            if (d2 < bestDistSq)
            {
                bestDistSq = d2;
                best = collider.gameObject.transform;
            }
        }

        return best;
    }

    public void MoveToFreeLookCamera()
    {
        if (state == CamState.LOCKONCAM)
        {
            state = CamState.FREE_LOOK_CAM;
            freeLookCam.Priority = 10;
            lockOnCam.Priority = 0;
            // fpsCam.Priority = 0;
            // viewFinder.SetActive(false);

            if (!Player.instance.isHolding)
            {
                Player.instance.animator.SetTrigger("freeMode");
            }
            else
            {
                Player.instance.animator.SetTrigger("freeModeHolding");
            }
        }
        // if (state == CamState.FPSCAM)
        // {
        //     state = CamState.FREE_LOOK_CAM;
        //     freeLookCam.Priority = 10;
        //     lockOnCam.Priority = 0;
        //     fpsCam.Priority = 0;
        //     Player.instance.animator.SetTrigger("blend");
        //     viewFinder.SetActive(false);
        // }
    }

    public void MoveToLockOnCamera(Transform target)
    {
        if (state == CamState.FREE_LOOK_CAM)
        {
            state = CamState.LOCKONCAM;
            freeLookCam.Priority = 0;
            lockOnCam.Priority = 10;
            // fpsCam.Priority = 0;
            lockOnCam.LookAt = target;
            // viewFinder.SetActive(false);

            if (!Player.instance.isHolding)
            {
                Player.instance.animator.SetTrigger("lockMode");
            }
            else
            {
                Player.instance.animator.SetTrigger("lockModeHolding");
            }
        }
        // if (state == CamState.FPSCAM)
        // {
        //     state = CamState.LOCKONCAM;
        //     freeLookCam.Priority = 10;
        //     lockOnCam.Priority = 0;
        //     fpsCam.Priority = 0;
        //     lockOnCam.LookAt = target;
        //     Player.instance.animator.SetTrigger("blend");
        //     viewFinder.SetActive(false);
        // }
    }

    // public void MoveToFpsCamera()
    // {
    //     if (state != CamState.FPSCAM)
    //     {
    //         state = CamState.FPSCAM;
    //         freeLookCam.Priority = 0;
    //         lockOnCam.Priority = 0;
    //         fpsCam.Priority = 10;
    //         Player.instance.animator.SetTrigger("blendFPS");
    //         viewFinder.SetActive(true);
    //     }
    // }

}

