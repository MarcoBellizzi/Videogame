using UnityEngine;

public class Woman : MonoBehaviour
{

    private int stato;
    private Animator animator;

    void Start()
    { 
        stato = 0;     
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        
    }

    void Sposta_0()
    {
        stato += 1;
        transform.position = new Vector3(0, 0, 45);
        animator.CrossFade("Waving", 0.2f);
    }

    void Sposta_1()
    {
        stato += 1;
        transform.position = new Vector3(0, 0, 55);
        animator.CrossFade("Waving", 0.2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (stato == 0)
        {
            animator.CrossFade("Talking", 0.2f);
            Panel.Instance.Mostra("ragazza_0", this.Sposta_0);
        }

        if (stato == 1)
        {
            animator.CrossFade("Talking", 0.2f);
            Panel.Instance.Mostra("ragazza_1", this.Sposta_1);
        }
    }

}
