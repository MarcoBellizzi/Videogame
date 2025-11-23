using UnityEngine;

public class Woman : MonoBehaviour
{

    private int stato;

    void Start()
    { 
        stato = 0;     
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Sposta()
    {
        stato += 1;
        transform.position = new Vector3(0, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (stato == 0)
        {
            Panel.Instance.Mostra("ragazza_1", this.Sposta);
        }
    }

}
