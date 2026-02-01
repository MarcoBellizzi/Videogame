using TMPro;
using UnityEngine;

public class NPC1 : MonoBehaviour
{
    public enum ThaeliaState
    {
        WAIT_FOR_TALK_1,
        WAIT_FOR_TALK_2
    }
    
    public static NPC1 instance;
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
        PanelDialogues.instance.Show("npc11");
    }

    public void Talk2()
    {
        sphereCollider.enabled = false;
        canvas.gameObject.SetActive(false);
        Player.instance.canMove = false;
        animator.SetTrigger("talk");
        PanelDialogues.instance.Show("npc12");
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
        Bag.instance.AddItem("Frammeto Maschera 1", "Nel 2020 un'alluvione tossica ha ridotto la popolazione del 70% e solo pochi fortunati riuscirono a sopravvivere grazie ad una maschera speciale che li ha protetti.");
        PanelObjects.instance.Show("Frammeto Maschera 1", "Nel 2020 un'alluvione tossica ha ridotto la popolazione del 70% e solo pochi fortunati riuscirono a sopravvivere grazie ad una maschera speciale che li ha protetti.");
        GameManager.Instance.addFrammenti();
        Idle();
    }

}
