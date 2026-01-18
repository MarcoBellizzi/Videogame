using System.Collections;
using TMPro;
using UnityEngine;

public enum GirlState
{
    WAIT_FOR_RUN,
    WAIT_FOR_JUMP,
    WAIT_FOR_ITEMS,
    WAIT_FOR_ATTACK,
    WAIT_FOR_TRAIN,
    COMBAT_IDLE,
    COMBAT_RUN,
    COMBAT_RUN_TARGET,
    COMBAT_ATTACK,
    DEATH,
    RUN_BACK
}

public class Girl : MonoBehaviour
{
    public static Girl instance;
    [SerializeField] public Transform sword;
    [SerializeField] public Transform swordPoint;
    [SerializeField] public Transform rightHandPoint;
    public float healthPoints;
    [HideInInspector] public bool canGetHit;
    private GirlState state;
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
        state = GirlState.WAIT_FOR_RUN;
        // state = GirlState.WAIT_FOR_TRAIN;
        animator = GetComponentInChildren<Animator>();
        canvas = GameObject.Find("CanvasOnTop").GetComponent<Canvas>();
        content = GameObject.Find("TextOnTop").GetComponent<TextMeshProUGUI>();
        canvasOffset = new Vector3(0, 2.1f, 0);
        sphereCollider = GetComponent<SphereCollider>();
        isClicking = false;
        healthPoints = 100f;
        canGetHit = false;
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

        if (state == GirlState.COMBAT_RUN)
        {
            this.transform.forward = Player.instance.transform.position - 
                new Vector3(
                    this.transform.position.x, 
                    Player.instance.transform.position.y, 
                    this.transform.position.z);

            if (Vector3.Distance(this.transform.position, Player.instance.transform.position) < distanceToTarget)
            {
                target = new Vector3(
                    Player.instance.transform.position.x, 
                    Player.instance.transform.position.y,  // evitare il salto
                    Player.instance.transform.position.z);

                state = GirlState.COMBAT_RUN_TARGET;
            }
            else
            {
                this.transform.position += this.transform.forward * 5f * Time.deltaTime;
            }
        }

        if (state == GirlState.COMBAT_RUN_TARGET)
        {
            if (Vector3.Distance(this.transform.position, target) < distanceToAttack)
            {
                state = GirlState.COMBAT_ATTACK;
                animator.SetTrigger("attack");
            }
            else
            {
                this.transform.position += this.transform.forward * 5f * Time.deltaTime;
            }
        }

        if (state == GirlState.RUN_BACK)
        {
            
            this.transform.position -= this.transform.forward * 8f * Time.deltaTime;
        }

    }

    public void MoveToJump()
    {
        state = GirlState.WAIT_FOR_JUMP;
        transform.position = GameObject.Find("GirlPositionJump").transform.position;
        Wave();
    }

    public void MoveToItems()
    {
        state = GirlState.WAIT_FOR_ITEMS;
        transform.position = GameObject.Find("GirlPositionItems").transform.position;
        Wave();
    }

    public void MoveToAttack()
    {
        state = GirlState.WAIT_FOR_ATTACK;
        transform.position = GameObject.Find("GirlPositionAttack").transform.position;
        Wave();
    }

    public void MoveToTrain()
    {
        state = GirlState.WAIT_FOR_TRAIN;
        transform.position = GameObject.Find("GirlPositionTrain").transform.position;
        Idle();
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
                if (state == GirlState.WAIT_FOR_RUN)
                {
                    Talk();
                    PanelDialogues.instance.Show("girl_run");
                }

                if (state == GirlState.WAIT_FOR_JUMP)
                {
                    Talk();
                    PanelDialogues.instance.Show("girl_jump");
                }

                if (state == GirlState.WAIT_FOR_ITEMS)
                {
                    Talk();
                    PanelDialogues.instance.Show("girl_items");
                }

                if (state == GirlState.WAIT_FOR_ATTACK)
                {
                    Talk();
                    PanelDialogues.instance.Show("girl_attack");
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

    public void Talk()
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

        if (Vector3.Distance(this.transform.position, Player.instance.transform.position) > distanceToTarget)
        {
            state = GirlState.COMBAT_RUN;
            animator.SetTrigger("run");
        }
        else
        {
            if (Vector3.Distance(this.transform.position, Player.instance.transform.position) > distanceToAttack)
            {
                target = new Vector3(
                    Player.instance.transform.position.x, 
                    Player.instance.transform.position.y,  // evitare il salto
                    Player.instance.transform.position.z);
    
                state = GirlState.COMBAT_RUN_TARGET;            
                animator.SetTrigger("run");
            }
            else
            {
                state = GirlState.COMBAT_ATTACK;
                animator.SetTrigger("attack");
            }
        }
    }

    public void CombatIdle()
    {
        state = GirlState.COMBAT_IDLE;
        StopAllCoroutines();
        StartCoroutine(Wait(1.5f));
    }

    public void GetHit(float demage)
    {
        healthPoints -= demage;
        PanelHeathBars.instance.enemyHealtPoints = healthPoints;

        StopAllCoroutines();

        if (healthPoints <= 0)
        {
            state = GirlState.DEATH;
            animator.SetTrigger("death");
        }
        else
        { 
            animator.SetTrigger("getHit");
        }
    }

    IEnumerator WaitRebird()
    {
        yield return new WaitForSeconds(6f);
        
        healthPoints = 100f;
        PanelHeathBars.instance.sliderEnemy.maxValue = healthPoints;
        PanelHeathBars.instance.enemyHealtPoints = healthPoints;
        PanelHeathBars.instance.sliderEnemy.gameObject.SetActive(false);
        animator.SetTrigger("rebird");
    }

    public void Rebird()
    {
        StopAllCoroutines();
        StartCoroutine(WaitRebird());
    }

    public void ReadyForTrain()
    {
        sphereCollider.enabled = true;
        state = GirlState.WAIT_FOR_TRAIN;
    }

    public void RunBack()
    {
        animator.ResetTrigger("attack");

        this.transform.forward = Player.instance.transform.position - 
            new Vector3(
                this.transform.position.x, 
                Player.instance.transform.position.y, 
                this.transform.position.z);

        state = GirlState.RUN_BACK;

        StopAllCoroutines();
        StartCoroutine(Wait(1.5f));
    }

}
