using TMPro;
using UnityEngine;

public class NPC2 : MonoBehaviour
{
    public enum ThaeliaState
    {
        WAIT_FOR_TALK_1,
        WAIT_FOR_TALK_2
    }
    
    public static NPC2 instance;
    private bool isClicking;
    private Canvas canvas;
    private TextMeshProUGUI content;
    private Vector3 canvasOffset;
    private ThaeliaState state;
    private SphereCollider sphereCollider;
    private Animator animator;

    void Awake()
    {
        instance = this;
        isClicking = false;
        canvas = GameObject.Find("CanvasOnTop").GetComponent<Canvas>();
        content = GameObject.Find("TextOnTop").GetComponent<TextMeshProUGUI>();
        canvasOffset = new Vector3(0, 2.1f, 0);
        state = ThaeliaState.WAIT_FOR_TALK_1;
        sphereCollider = GetComponent<SphereCollider>();
        animator = GetComponentInChildren<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // non posso controllare il click del personaggio nel OnTriggerStay perchè viene chiamato meno dell update
        if (Input.GetKeyDown(KeyCode.Mouse0) && !PanelChoice.instance.gameObject.activeSelf && !PanelDialogues.instance.gameObject.activeSelf)
        {
            isClicking = true; 
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            isClicking = false;
        }
    }

    
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
                if (state == ThaeliaState.WAIT_FOR_TALK_1)
                {
                    Talk1();
                }
                
                if (state == ThaeliaState.WAIT_FOR_TALK_2)
                {
                    Talk2();
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

    private void Talk1()
    {
        sphereCollider.enabled = false;
        canvas.gameObject.SetActive(false);
        Player.instance.canMove = false;
        animator.SetTrigger("talk");
        PanelDialogues.instance.Show("npc21");
    }

    public void Talk2()
    {
        sphereCollider.enabled = false;
        canvas.gameObject.SetActive(false);
        Player.instance.canMove = false;
        animator.SetTrigger("talk");
        PanelDialogues.instance.Show("npc22");
    }

    public void Idle()
    {
        state = ThaeliaState.WAIT_FOR_TALK_2;
        sphereCollider.enabled = true;
        canvas.gameObject.SetActive(false);
        Player.instance.Resume();
        animator.SetTrigger("idle");
    }

    public void GiveMask()
    {
        Bag.instance.AddItem("Frammeto Maschera 2", "I sopravvissuti si sono forniti di una maschera composta da fibra di Cupro con Tecnezio e Ittrio, elementi che combinati insieme garantiscono la protezione dalla tossicità dell'aria.");
        PanelObjects.instance.Show("Frammeto Maschera 2", "I sopravvissuti si sono forniti di una maschera composta da fibra di Cupro con Tecnezio e Ittrio, elementi che combinati insieme garantiscono la protezione dalla tossicità dell'aria.");
        GameManager.Instance.addFrammenti();
        Idle();
    }
}
