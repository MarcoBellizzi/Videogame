using UnityEngine;

public class Sword : MonoBehaviour
{
    public bool player; // from inspector
    private BoxCollider triggerCollider;

    void Awake()
    {
        triggerCollider = GetComponentInChildren<BoxCollider>();
    }

    void Start()
    {
        triggerCollider.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (player)
        {        
            if (other.gameObject.name == "CylinderSpecial")
            {
                Destroy(other.gameObject);
                triggerCollider.enabled = false;
                MainCamera.instance.MoveToFreeLookCamera();
            }
            if (other.gameObject.name == "Girl")
            {
                if (Girl.instance.canGetHit)
                {
                    Player.instance.sword.gameObject.GetComponentInChildren<BoxCollider>().enabled = false;
                    Girl.instance.GetHit(20f);
                }
            }
            if (other.gameObject.name == "Prosos")
            {
                if (Prosos.instance.canGetHit)
                {
                    Player.instance.sword.gameObject.GetComponentInChildren<BoxCollider>().enabled = false;
                    // Prosos.instance.GetHit(20f);
                }
            }
        }
        else
        {   
            if (other.gameObject.name == "Player")
            {
                if (Player.instance.canGetHit)
                {
                    // Girl.instance.sword.gameObject.GetComponentInChildren<BoxCollider>().enabled = false;
                    Prosos.instance.sword.gameObject.GetComponentInChildren<BoxCollider>().enabled = false;
                    Player.instance.GetHit(20f);
                }
            }
        }



    }

}
