using TMPro;
using UnityEngine;

public class Girl : MonoBehaviour
{
    private int state;
    private Animator animator;
    private Canvas canvas;
    private TextMeshProUGUI content;
    private Vector3 canvasOffset;
    private SphereCollider sphereCollider;
    private bool isClicking;

    void Awake()
    {
        state = 0;     
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
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            isClicking = true; 
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            isClicking = false;
        }
    }

    void Move_0()
    {
        state += 1;
        transform.position = GameObject.Find("GirlPosition1").transform.position;
        animator.CrossFade("Waving", 0.2f);
        canvas.gameObject.SetActive(false);
        sphereCollider.enabled = true;
        Player.instance.Resume();
    }

    void Move_1()
    {
        state += 1;
        transform.position = GameObject.Find("GirlPosition2").transform.position;
        animator.CrossFade("Waving", 0.2f);
        canvas.gameObject.SetActive(false);
        sphereCollider.enabled = true;
        Player.instance.Resume();
    }

    void Move_2()
    {
        state += 1;
        transform.position = GameObject.Find("GirlPosition3").transform.position;
        animator.CrossFade("Waving", 0.2f);
        canvas.gameObject.SetActive(false);
        sphereCollider.enabled = true;
        Player.instance.Resume();
    }

    void Move_3()
    {
        state += 1;
        transform.position = GameObject.Find("GirlPosition4").transform.position;
        animator.CrossFade("Waving", 0.2f);
        canvas.gameObject.SetActive(false);
        sphereCollider.enabled = true;
        Player.instance.Resume();
    }

    void Move_4()
    {
        state += 1;
        transform.position = GameObject.Find("GirlPosition5").transform.position;
        animator.CrossFade("Waving", 0.2f);
        canvas.gameObject.SetActive(false);
        sphereCollider.enabled = true;
        Player.instance.Resume();
    }

    void Move_5()
    {
        state += 1;
        // transform.position = GameObject.Find("GirlPosition6").transform.position;
        animator.CrossFade("Waving", 0.2f);
        canvas.gameObject.SetActive(false);
        sphereCollider.enabled = true;
        Player.instance.Resume();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            canvas.gameObject.SetActive(true);
            content.text = "Clicca per parlare";
            Player.instance.Stop();
            Player.instance.canMove = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            canvas.transform.position = transform.position + canvasOffset;
            canvas.transform.rotation = 
                Quaternion.LookRotation(canvas.transform.position - MainCamera.instance.transform.position);

            if (isClicking)
            {
                if (state == 0)
                {
                    sphereCollider.enabled = false;
                    canvas.gameObject.SetActive(false);
                    Player.instance.canMove = false;
                    animator.CrossFade("Talking", 0.2f);
                    PanelDialogues.instance.Show("girl_0", this.Move_0);
                }

                if (state == 1)
                {
                    sphereCollider.enabled = false;
                    canvas.gameObject.SetActive(false);
                    Player.instance.canMove = false;
                    animator.CrossFade("Talking", 0.2f);
                    PanelDialogues.instance.Show("girl_1", this.Move_1);
                }

                if (state == 2)
                {
                    sphereCollider.enabled = false;
                    canvas.gameObject.SetActive(false);
                    Player.instance.canMove = false;
                    animator.CrossFade("Talking", 0.2f);
                    PanelDialogues.instance.Show("girl_2", this.Move_2);
                }

                if (state == 3)
                {
                    sphereCollider.enabled = false;
                    canvas.gameObject.SetActive(false);
                    Player.instance.canMove = false;
                    animator.CrossFade("Talking", 0.2f);
                    PanelDialogues.instance.Show("girl_3", this.Move_3);
                }

                if (state == 4)
                {
                    sphereCollider.enabled = false;
                    canvas.gameObject.SetActive(false);
                    Player.instance.canMove = false;
                    animator.CrossFade("Talking", 0.2f);
                    PanelDialogues.instance.Show("girl_4", this.Move_4);
                }

                if (state == 5)
                {
                    sphereCollider.enabled = false;
                    canvas.gameObject.SetActive(false);
                    Player.instance.canMove = false;
                    animator.CrossFade("Talking", 0.2f);
                    PanelDialogues.instance.Show("girl_5", this.Move_5);
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

}
