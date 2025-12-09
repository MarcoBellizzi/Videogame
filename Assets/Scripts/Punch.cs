using UnityEngine;

public class Punch : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        string objectName = other.gameObject.name;
        // string layerName = LayerMask.LayerToName(other.gameObject.layer);

        if (objectName == "CylinderSpecial")
        {
            Destroy(other.gameObject);
            GameObject.Find("PunchBox").GetComponent<CapsuleCollider>().enabled = false;
        }

    }
}
