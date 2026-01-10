using UnityEngine;

public class Sword : MonoBehaviour
{
    BoxCollider triggerCollider;

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
        if (other.gameObject.name == "CylinderSpecial")
        {
            Destroy(other.gameObject);
            triggerCollider.enabled = false;
            MainCamera.instance.MoveToFreeLookCamera();
        }
    }

}
