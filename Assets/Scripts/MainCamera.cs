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
    public CinemachineVirtualCamera fpsCam;

    [HideInInspector] public CamState state;

    public Transform lockTarget; // da aggiornare per prenderlo dinamicamente

    [SerializeField] private float locCamBack;
    [SerializeField] private float locCamUp;
    [SerializeField] private float fpsCamBack;
    [SerializeField] private float fpsCamUp;
    [SerializeField] private float fpsCamRight;
    [SerializeField] private float sensibilitaX = 180f;
    [SerializeField] private float sensibilitaY = 180f;
    [SerializeField] private float pitchMin = -85f;
    [SerializeField] private float pitchMax = 85f;

    private float yaw;
    private float pitch;


    void Awake()
    {
        instance = this;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        state = CamState.FREE_LOOK_CAM;
        freeLookCam.Priority = 10;
        lockOnCam.Priority = 0;
        fpsCam.Priority = 0;
        yaw = 0f;
        pitch = 0f;
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
                (lockTarget.position - 
                    new Vector3(
                        Player.instance.transform.position.x,
                        lockTarget.position.y,
                        Player.instance.transform.position.z
                    )
                ).normalized;

            // AGGIORNA POSIZIONE E ORIENTAMENTE DELLA CAMERA IN BASE AL TARGET E AL PLAYER
            Player.instance.toFollowVirtual.transform.forward =
                (lockTarget.position - Player.instance.transform.position).normalized;

            Player.instance.toFollowVirtual.transform.position =
                Player.instance.transform.position
                - locCamBack * Player.instance.toFollowVirtual.transform.forward
                + locCamUp * Player.instance.toFollowVirtual.transform.up;
        }

        if (state == CamState.FPSCAM)
        {
            
            yaw   += Input.GetAxis("Mouse X") * sensibilitaX * Time.deltaTime;
            pitch -= Input.GetAxis("Mouse Y") * sensibilitaY * Time.deltaTime;
            pitch  = Mathf.Clamp(pitch, pitchMin, pitchMax);

            // AGGIORNA L'ORIENTAMENTO DEL PLAYER IN BASE AL MOUSE
            Player.instance.playerOrientation.transform.rotation = Quaternion.Euler(0f, yaw, 0f);

            // AGGIORNA LA ROTAZIONE DEL MODELLO DEL PLAYER IN BASE AL MOUSE
            Player.instance.playerModel.transform.rotation =
                Quaternion.Slerp(
                    Player.instance.playerModel.transform.rotation,
                    Quaternion.Euler(0f, yaw, 0f),
                    1f - Mathf.Exp(-10 * Time.deltaTime)
                );

        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (state != CamState.LOCKONCAM)
            {
                state = CamState.LOCKONCAM;
                freeLookCam.Priority = 0;
                lockOnCam.Priority = 10;
                fpsCam.Priority = 0;

                lockOnCam.LookAt = lockTarget;
            }
            else
            {
                state = CamState.FREE_LOOK_CAM;
                freeLookCam.Priority = 10;
                lockOnCam.Priority = 0;
                fpsCam.Priority = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (state != CamState.FPSCAM)
            {
                state = CamState.FPSCAM;
                freeLookCam.Priority = 0;
                lockOnCam.Priority = 0;
                fpsCam.Priority = 10;
            }
            else
            {
                state = CamState.FREE_LOOK_CAM;
                freeLookCam.Priority = 10;
                lockOnCam.Priority = 0;
                fpsCam.Priority = 0;
            }
        }
    
    }

    void LateUpdate()
    {
        if (state == CamState.FPSCAM)
        {
            // AGGIORNA POSIZIONE E ROTAZIONE DELLA CAMERA IN BASE AL MOUSE E AL PLAYER
            fpsCam.transform.rotation = Quaternion.Euler(pitch, yaw, 0f);

            fpsCam.transform.position =
                Player.instance.transform.position
                - fpsCamBack * fpsCam.transform.forward
                + fpsCamUp * fpsCam.transform.up
                + fpsCamRight * fpsCam.transform.right;
        }
    }

}

