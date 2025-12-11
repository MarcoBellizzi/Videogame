using UnityEngine;

public class Punch : MonoBehaviour
{
    SphereCollider sphereCollider;

    void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }
    void OnTriggerEnter(Collider other)
    {
        // string layerName = LayerMask.LayerToName(other.gameObject.layer);

        if (other.gameObject.name == "CylinderSpecial")
        {
            Destroy(other.gameObject);
            sphereCollider.enabled = false;
            MainCamera.instance.MoveToFreeLookCamera();
        }

    }
}
