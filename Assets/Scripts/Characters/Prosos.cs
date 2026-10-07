using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PrososState
{
    WAIT_FOR_TALK,
    COMBAT_IDLE,
    COMBAT_RUN,
    COMBAT_RUN_TARGET,
    COMBAT_ATTACK,
    DEATH,
    RUN_BACK
}

public class Prosos : MonoBehaviour
{
    public static Prosos instance;
    [SerializeField] public Transform sword;
    [SerializeField] public Transform swordPoint;
    [SerializeField] public Transform rightHandPoint;
    
    [SerializeField] public Transform playerModel;
    public float healthPoints;
    [HideInInspector] public bool canGetHit;
    private PrososState state;
    private Animator animator;
    private Canvas canvas;
    private TextMeshProUGUI content;
    private Vector3 canvasOffset;
    private SphereCollider sphereCollider;
    private bool isClicking;
    private Vector3 target;
    private float distanceToTarget = 3f;
    private float distanceToAttack = 1.2f;

    void Awake()
    {
        instance = this;
        state = PrososState.WAIT_FOR_TALK;
        animator = GetComponentInChildren<Animator>();
        canvas = GameObject.Find("CanvasOnTop").GetComponent<Canvas>();
        content = GameObject.Find("TextOnTop").GetComponent<TextMeshProUGUI>();
        canvasOffset = new Vector3(0, 2.1f, 0);
        sphereCollider = GetComponent<SphereCollider>();
        isClicking = false;
        healthPoints = 387f;
        canGetHit = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // canvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
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

        if (state == PrososState.COMBAT_RUN)
        {
            
            StopAllCoroutines();

            Debug.Log("stato run");
            this.transform.forward = Player.instance.transform.position - 
                new Vector3(
                    this.transform.position.x, 
                    Player.instance.transform.position.y, 
                    this.transform.position.z);

            this.playerModel.forward = Player.instance.transform.position - 
                new Vector3(
                    this.transform.position.x, 
                    Player.instance.transform.position.y, 
                    this.transform.position.z);

            if (Vector3.Distance(this.transform.position, Player.instance.transform.position) <= distanceToAttack)
            {
                
                Debug.Log("stato Attack");
                state = PrososState.COMBAT_ATTACK;
                animator.SetTrigger("attack");
            }
            // if (Vector3.Distance(this.transform.position, Player.instance.transform.position) <= distanceToTarget)
            // {
            //     Debug.Log("meno distance to target");

            //     target = new Vector3(
            //         Player.instance.transform.position.x, 
            //         Player.instance.transform.position.y,  // evitare il salto
            //         Player.instance.transform.position.z);

            //     state = PrososState.COMBAT_RUN_TARGET;
            // }
            else
            {
                Debug.Log("avanza");
                this.transform.position += this.transform.forward * 5f * Time.deltaTime;
            }
        }

        // if (state == PrososState.COMBAT_RUN_TARGET)
        // {
        //     Debug.Log("run>target");

            
        //     StopAllCoroutines();

        //     this.transform.forward = Player.instance.transform.position - 
        //         new Vector3(
        //             this.transform.position.x, 
        //             Player.instance.transform.position.y, 
        //             this.transform.position.z);

        //     if (Vector3.Distance(this.transform.position, target) <= distanceToAttack)
        //     {
        //         Debug.Log("stato Attack");
        //         state = PrososState.COMBAT_ATTACK;
        //         animator.SetTrigger("attack");
        //     }
        //     else
        //     {
        //         this.transform.position += this.transform.forward * 5f * Time.deltaTime;
        //     }
        // }

        if (state == PrososState.RUN_BACK)
        {
            Debug.Log("run back");
            this.transform.forward = Player.instance.transform.position - 
                new Vector3(
                    this.transform.position.x, 
                    Player.instance.transform.position.y, 
                    this.transform.position.z);

            this.playerModel.forward = Player.instance.transform.position - 
                new Vector3(
                    this.transform.position.x, 
                    Player.instance.transform.position.y, 
                    this.transform.position.z);

            this.transform.position -= this.transform.forward * 8f * Time.deltaTime;
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
                if (state == PrososState.WAIT_FOR_TALK)
                {
                    sphereCollider.enabled = false;
                    canvas.gameObject.SetActive(false);
                    Player.instance.canMove = false;
                    animator.SetTrigger("talk");
                    PanelDialogues.instance.Show("prosos");
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

    public void StartFight()
    {
        MainCamera.instance.MoveToLockOnCamera(this.transform);
        sphereCollider.enabled = false;
        canvas.gameObject.SetActive(false);
        Player.instance.Resume();
        animator.SetTrigger("unsheathe");
        PanelHeathBars.instance.sliderEnemy.maxValue = healthPoints;
        PanelHeathBars.instance.enemyHealtPoints = healthPoints;
        PanelHeathBars.instance.sliderEnemy.gameObject.SetActive(true);
    }

    IEnumerator Wait(float duration)
    {
        yield return new WaitForSeconds(duration);
        
        this.transform.forward = Player.instance.transform.position - 
            new Vector3(
                this.transform.position.x, 
                Player.instance.transform.position.y, 
                this.transform.position.z);

        if (Vector3.Distance(this.transform.position, Player.instance.transform.position) > distanceToAttack)
        {
            Debug.Log("insegui 194");
            state = PrososState.COMBAT_RUN;
            animator.SetTrigger("run");
        }
        else
        {
            // if (Vector3.Distance(this.transform.position, Player.instance.transform.position) > distanceToAttack)
            // {
            //     Debug.Log("torunTurget");
            //     target = new Vector3(
            //         Player.instance.transform.position.x, 
            //         Player.instance.transform.position.y,  // evitare il salto
            //         Player.instance.transform.position.z);
    
            //     state = PrososState.COMBAT_RUN_TARGET;            
            //     animator.SetTrigger("run");
            // }
            // else
            // {
            //     state = PrososState.COMBAT_ATTACK;
            //     animator.SetTrigger("attack");
            // }

            
            state = PrososState.COMBAT_ATTACK;
            animator.SetTrigger("attack");
        }
    }

    public void CombatIdle()
    {
        state = PrososState.COMBAT_IDLE;
        // animator.ResetTrigger("run");
        // animator.ResetTrigger("attack");
        StopAllCoroutines();
        StartCoroutine(Wait(1.5f));
    }

    IEnumerator WaitRebird()
    {
        yield return new WaitForSeconds(6f);
        
        // healthPoints = 100f;
        // PanelHeathBars.instance.sliderEnemy.maxValue = healthPoints;
        // PanelHeathBars.instance.enemyHealtPoints = healthPoints;
        // PanelHeathBars.instance.sliderEnemy.gameObject.SetActive(false);
        // animator.SetTrigger("rebird");
        
        SceneManager.LoadScene("Menu");
    }

    public void Rebird()
    {
        StopAllCoroutines();
        StartCoroutine(WaitRebird());
    }

    public void RunBack()
    {
        // animator.ResetTrigger("attack");

        this.transform.forward = Player.instance.transform.position - 
            new Vector3(
                this.transform.position.x, 
                Player.instance.transform.position.y, 
                this.transform.position.z);

        state = PrososState.RUN_BACK;

        StopAllCoroutines();
        StartCoroutine(Wait(0.2f));
    }

    public void GetHit(float demage)
    {
        healthPoints -= demage;
        PanelHeathBars.instance.enemyHealtPoints = healthPoints;

        StopAllCoroutines();

        if (healthPoints <= 0)
        {
            state = PrososState.DEATH;
            animator.SetTrigger("death");
        }
        else
        { 
            animator.SetTrigger("getHit");
        }
    }
}
