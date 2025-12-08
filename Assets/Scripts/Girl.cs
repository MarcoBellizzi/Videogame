using UnityEngine;

public class Girl : MonoBehaviour
{
    private int state;
    private Animator animator;

    void Awake()
    {
        state = 0;     
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void Move_0()
    {
        state += 1;
        transform.position = new Vector3(0, 0, 45);
        animator.CrossFade("Waving", 0.2f);
    }

    void Move_1()
    {
        state += 1;
        transform.position = new Vector3(0, 0, 55);
        animator.CrossFade("Waving", 0.2f);
    }

    void Move_2()
    {
        state += 1;
        transform.position = new Vector3(0, 0, 100);
        animator.CrossFade("Waving", 0.2f);
    }

    void Move_3()
    {
        state += 1;
        transform.position = new Vector3(0, 0, 120);
        animator.CrossFade("Waving", 0.2f);
    }

    void Move_4()
    {
        state += 1;
        animator.CrossFade("Waving", 0.2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (state == 0)
        {
            animator.CrossFade("Talking", 0.2f);
            PanelDialogues.instance.Show("girl_0", this.Move_0);
        }

        if (state == 1)
        {
            animator.CrossFade("Talking", 0.2f);
            PanelDialogues.instance.Show("girl_1", this.Move_1);
        }

        if (state == 2)
        {
            animator.CrossFade("Talking", 0.2f);
            PanelDialogues.instance.Show("girl_2", this.Move_2);
        }

        if (state == 3)
        {
            animator.CrossFade("Talking", 0.2f);
            PanelDialogues.instance.Show("girl_3", this.Move_3);
        }

        if (state == 4)
        {
            animator.CrossFade("Talking", 0.2f);
            PanelDialogues.instance.Show("girl_4", this.Move_4);
        }
    }

}
