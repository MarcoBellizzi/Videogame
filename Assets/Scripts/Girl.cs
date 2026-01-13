using System;
using System.ComponentModel;
using TMPro;
using UnityEngine;

public enum GirlState
{
    WAIT_FOR_RUN,
    WAIT_FOR_JUMP,
    WAIT_FOR_ITEMS,
    WAIT_FOR_ATTACK,
    WAIT_FOR_TRAIN
}

public class Girl : MonoBehaviour
{
    public static Girl instance;
    [SerializeField] public Transform sword;
    [SerializeField] public Transform swordPoint;
    [SerializeField] public Transform rightHandPoint;
    private GirlState state;
    private Animator animator;
    private Canvas canvas;
    private TextMeshProUGUI content;
    private Vector3 canvasOffset;
    private SphereCollider sphereCollider;
    private bool isClicking;

    void Awake()
    {
        instance = this;
        // state = GirlState.WAIT_FOR_RUN;
        state = GirlState.WAIT_FOR_TRAIN;
        animator = GetComponentInChildren<Animator>();
        canvas = GameObject.Find("CanvasOnTop").GetComponent<Canvas>();
        content = GameObject.Find("TextOnTop").GetComponent<TextMeshProUGUI>();
        canvasOffset = new Vector3(0, 2.1f, 0);
        sphereCollider = GetComponent<SphereCollider>();
        isClicking = false;
    }

    void Start()
    {
        canvas.gameObject.SetActive(false);
    }

    void Update()
    {
        // non posso controllare il click del personaggio nel OnTriggerStay perchè viene chiamato meno dell update
        if (Input.GetKeyDown(KeyCode.Mouse0) && !PanelChoice.instance.gameObject.activeSelf)
        {
            isClicking = true; 
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            isClicking = false;
        }
    }

    void MoveToJump()
    {
        state = GirlState.WAIT_FOR_JUMP;
        transform.position = GameObject.Find("GirlPositionJump").transform.position;
        Wave();
    }

    void MoveToItems()
    {
        state = GirlState.WAIT_FOR_ITEMS;
        transform.position = GameObject.Find("GirlPositionItems").transform.position;
        Wave();
    }

    void MoveToAttack()
    {
        state = GirlState.WAIT_FOR_ATTACK;
        transform.position = GameObject.Find("GirlPositionAttack").transform.position;
        Wave();
    }

    void MoveToTrain()
    {
        state = GirlState.WAIT_FOR_TRAIN;
        transform.position = GameObject.Find("GirlPositionTrain").transform.position;
        Idle();
    }

    // void OnTriggerEnter(Collider other)
    // {
    //     if (other.gameObject.name == "Player")
    //     {
    //         canvas.gameObject.SetActive(true);
    //         content.text = "Clicca per parlare";
    //         Player.instance.Stop();
    //         Player.instance.canMove = true;
    //     }
    // }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            // NON IL MASSIMO QUI
            canvas.gameObject.SetActive(true);
            content.text = "Clicca per parlare";
            Player.instance.Stop();
            Player.instance.canMove = true;

            canvas.transform.position = transform.position + canvasOffset;
            canvas.transform.rotation = Quaternion.LookRotation(canvas.transform.position - MainCamera.instance.transform.position);

            if (isClicking)
            {
                if (state == GirlState.WAIT_FOR_RUN)
                {
                    Talk();
                    PanelDialogues.instance.Show("girl_run", this.MoveToJump);
                }

                if (state == GirlState.WAIT_FOR_JUMP)
                {
                    Talk();
                    PanelDialogues.instance.Show("girl_jump", this.MoveToItems);
                }

                if (state == GirlState.WAIT_FOR_ITEMS)
                {
                    Talk();
                    PanelDialogues.instance.Show("girl_items", this.MoveToAttack);
                }

                if (state == GirlState.WAIT_FOR_ATTACK)
                {
                    Talk();
                    PanelDialogues.instance.Show("girl_attack", this.MoveToTrain);
                }

                if (state == GirlState.WAIT_FOR_TRAIN)
                {
                    sphereCollider.enabled = false;
                    canvas.gameObject.SetActive(false);
                    Player.instance.canMove = false;
                    PanelChoice.instance.Show("train");
                }
            }
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            canvas.gameObject.SetActive(false);
            Player.instance.Resume();
        }
    }

    void Talk()
    {
        sphereCollider.enabled = false;
        canvas.gameObject.SetActive(false);
        Player.instance.canMove = false;
        animator.SetTrigger("talk");
    }

    public void Wave()
    {
        sphereCollider.enabled = true;
        canvas.gameObject.SetActive(false);
        Player.instance.Resume();
        animator.SetTrigger("wave");
    }

    public void Idle()
    {
        sphereCollider.enabled = true;
        canvas.gameObject.SetActive(false);
        Player.instance.Resume();
        animator.SetTrigger("idle");
    }

    public void Unsheathe()
    {
        sphereCollider.enabled = false;
        canvas.gameObject.SetActive(false);
        Player.instance.Resume();
        animator.SetTrigger("unsheathe");
    }

}
