using UnityEngine;

public class Throwable : MonoBehaviour
{

    private float speed = 40f;
    private float maxDistance = 100f;

    void Update()
    {
        this.gameObject.transform.position += this.gameObject.transform.forward.normalized * speed * Time.deltaTime;

        if (Vector3.Distance(this.gameObject.transform.position, Player.instance.transform.position) > maxDistance)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "SphereOnAir")
        {
            Destroy(other.gameObject);
        }
    }
}
