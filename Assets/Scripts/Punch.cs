using UnityEngine;

public class Punch : MonoBehaviour
{
    SphereCollider sphereCollider;

    void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    void Start()
    {
        sphereCollider.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "CylinderSpecial")
        {
            Destroy(other.gameObject);
            sphereCollider.enabled = false;
            MainCamera.instance.MoveToFreeLookCamera();
        }
    }
}
